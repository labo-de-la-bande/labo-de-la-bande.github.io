/// Le moteur : un état donne toutes les jauges, les liens actifs, le score et les trois réponses
/// (viable ? fidèle au but ? mieux que la France ?). Né du portage de l'ancien moteur JavaScript
/// (le premier jeu, abandonné le 27 novembre 2026). Pur, sans E/S : compilé en .NET et par Fable.
///
/// Aucun nombre du modèle n'est écrit ici : les coefficients viennent de data/coefficients.csv
/// (`C.`), les faits de la France de data/pays/fr/ (`F.`), avec leur statut et leur lentille.
/// Restent dans le code les constantes de forme (un pourcentage sur 100, le milieu d'une
/// fourchette) et la structure des formules.
module Labo.Engine.Moteur

open Labo.Data
open Labo.Engine.Tables

module C = Labo.Data.Coefficients
module F = Labo.Data.Pays.Fr

// ---------- Lectures ----------

/// Un curseur (ancien `V(id)`).
let curseur id (e: Etat) =
    match Map.tryFind id e.s with
    | Some v -> v
    | None -> curseursParDefaut |> Map.tryFind id |> Option.defaultValue nan

let horizon e = int (Etat.nombreOu "hz" 0.0 e)
let ouverture e = int (Etat.nombreOu "ouv" 0.0 e)
let monnaie e = Etat.texte "monnaie" e
let monnaieEst m e = monnaie e = Some m
let avecMonnaieFondanteOuPlafonnee e = monnaieEst "fondante" e || monnaieEst "plafond" e

let population0 (e: Etat) = Tables.population0 e.p
let population e = population0 e * populationHorizon[horizon e] / F.populationAujourdhui

// ---------- Options : l'état désigne-t-il telle option de tel levier ? ----------

/// La valeur que l'état donne au champ d'un levier (`c.x` ou `s.x`).
let valeurChamp (champ: string) (e: Etat) =
    match champ.Split '.' with
    | [| "c"; k |] -> Etat.choix k e
    | [| "s"; k |] -> Map.tryFind k e.s |> Option.map Num
    | _ -> None

/// L'option est-elle celle de l'état ?
let optionChoisie (levier: Levier) (o: OptionLevier) (e: Etat) =
    if levier.champ = "" then
        // préréglage pur : reconnu à ce qu'il a écrit
        match o.ecritAussi with
        | [ Nomme "paliers" ] -> Js.vrai (Etat.choix "proto" e)
        | [ Nomme "sans-paliers" ] -> Js.vrai (Etat.choix "palNo" e)
        | _ -> false
    else
        match valeurChamp levier.champ e with
        | None
        | Some Nul -> levier.defaut = Some o.id
        | Some v ->
            match levier.genre, v with
            | OuiNon, Bool b -> (if b then "oui" else "non") = o.valeur
            | _, Txt t -> t = o.valeur
            | _, Num n ->
                // `float` lit le point décimal quelle que soit la langue du système, en .NET comme avec Fable
                System.Text.RegularExpressions.Regex.IsMatch(o.valeur, "^-?[0-9]+(\\.[0-9]+)?$") && float o.valeur = n
            | _ -> false

let atomeVrai (a: Atome) (e: Etat) =
    let levier = levierDe[a.levier]

    let un =
        match a.options with
        | [] ->
            if levier.champ = "" then
                optionsDe[a.levier] |> List.exists (fun o -> optionChoisie levier o e)
            else
                match valeurChamp levier.champ e with
                | None
                | Some Nul -> false
                | Some _ -> true
        | ids -> optionsDe[a.levier] |> List.exists (fun o -> List.contains o.id ids && optionChoisie levier o e)

    if a.egal then un else not un

let conditionVraie atomes e = atomes |> List.forall (fun a -> atomeVrai a e)

/// `est "invasion" "armee" e` : l'option est-elle celle de l'état ? Le code désigne les options par
/// leur identifiant (data/options.csv), jamais par leur rang.
let est levier option e =
    if not (optionsDe[levier] |> List.exists (fun o -> o.id = option)) then
        failwith $"option inconnue {levier}={option}"

    atomeVrai { levier = levier; egal = true; options = [ option ] } e

/// Le rang d'une option, pour les leviers ordonnés (l'ouverture du pays).
let rangDe levier option =
    (optionsDe[levier] |> List.find (fun o -> o.id = option)).rang

// ---------- Gang : probabilité réduite par la prévention ----------

let risqueGang e =
    prevention |> List.fold (fun p (k, f) -> p * (if Js.vrai (Etat.choix k e) then f else 1.0)) 1.0

// ---------- Paris ----------

/// Années jouées depuis le début de la partie (`c.annees`, écrit par le moteur de jeu) ; sans
/// partie, les années de l'horizon choisi.
let annees e =
    match Etat.choix "annees" e with
    | Some(Num n) -> n
    | _ -> anneesHorizon[horizon e]

/// Un pari est actif quand il est choisi et prêt. Choisi dans l'ancien jeu (`true`), il compte depuis
/// le départ ; choisi dans une partie (l'année du choix), il compte depuis cette année-là.
let pariActif e (b: Pari) =
    match Etat.choix "bets" e with
    | Some(Obj m) ->
        match Map.tryFind b.id m with
        | Some(Num debut) -> annees e - debut >= float b.pretDans
        | v when Js.vrai v -> float b.pretDans <= annees e
        | _ -> false
    | _ -> false

let sommeParis (champ: Pari -> float) e =
    Modele.paris
    |> List.fold
        (fun s b ->
            let v = champ b

            if not (pariActif e b) || v = 0.0 then s
            elif b.avecMonnaie && not (avecMonnaieFondanteOuPlafonnee e) then s
            else s + v)
        0.0

let couvertureParPari ressource e =
    Modele.paris
    |> List.fold
        (fun meilleur b ->
            match pariActif e b, b.couvre with
            | true, Some(r, v) when r = ressource ->
                match meilleur with
                | Some m when rangCouverture v <= rangCouverture m -> meilleur
                | _ -> Some v
            | _ -> meilleur)
        None

let risqueParis e =
    Modele.paris
    |> List.fold (fun s b -> s + (if pariActif e b then float rangMaturite[b.maturite] else 0.0)) 0.0
    |> fun s -> Js.clamp100 (s * C.parisRisqueParMaturite)

// ---------- Nourriture et énergie ----------

let rendementPerdu e =
    let y = curseur "yield" e

    let y =
        if est "engrais" "moitie" e && ouverture e < rangDe "ouverture" "cible" then
            max y C.engraisSansGazPerteRendement
        else
            y

    max 0.0 (y - sommeParis _.rendement e)

let besoinParPersonne e = max C.besoinMinimum (curseur "need" e - sommeParis _.besoin e)

let energieDisponible e =
    curseur "supply" e + sommeParis _.energie e
    - (if est "engrais" "electrique" e then C.nh3Electricite else 0.0)
    - (if est "coord" "ia" e then C.coordIaElectricite else 0.0)

/// Personnes que les terres peuvent nourrir.
let nourris e =
    (F.surfaceAgricole - curseur "loss" e * F.perteTerresFacteur / 1000.0) / besoinParPersonne e
    * (1.0 - rendementPerdu e / 100.0)
    * 1e6

// ---------- Échanges ----------

let exportationActive (x: Exportation) e =
    match Etat.choix "exp" e with
    | Some(Obj m) -> Js.vrai (Map.tryFind x.id m)
    | v when Js.vrai v -> false
    | _ -> x.parDefaut

let valeurExportation (x: Exportation) e =
    match x.valeur with
    | Fixe v -> v
    | Calcul "export-electricite" -> F.exportElectricite2024 + C.exportElectriciteParTwh * max 0.0 (sommeParis _.energie e)
    | Calcul "export-nourriture" -> max 0.0 ((nourris e - population e) / 1e6 * C.exportNourritureParMillion)
    | Calcul autre -> failwith $"calcul d'exportation inconnu {autre}"

/// Importations, exportations (Md€).
let echanges e =
    let l = ouverture e

    if l = rangDe "ouverture" "ouvert" then
        F.importationsAujourdhui, F.exportationsAujourdhui
    else
        let imp =
            F.importations |> List.filter (fun i -> i.ouverture <= l) |> List.fold (fun s i -> s + i.valeur) 0.0

        let exp =
            F.exportations
            |> List.fold (fun s x -> s + (if exportationActive x e then valeurExportation x e else 0.0)) 0.0

        imp, exp

let ratioEchanges e =
    let imp, exp = echanges e
    if imp > 0.0 then min 1.0 (exp / imp) else 1.0

// ---------- Accès : la part des personnes que les besoins de base atteignent ----------

let enNature e =
    match monnaie e with
    | None
    | Some "sans"
    | Some "frontiere"
    | Some "bons" -> true
    | _ -> false

let acces nourriture (e: Etat) =
    let s = curseur "prSocle" e / 100.0
    let manque = if nourriture then F.faim else F.pauvrete
    let g = Js.clamp01 ((e.mp.ecart - 1.0) / (F.ecartD9D1 - 1.0))

    if enNature e then s
    elif monnaieEst "fondante" e then 1.0 - manque * g
    elif nourriture then (if e.mp.socle >= C.accesSocleNourriture then s else 1.0 - manque * g)
    else max 0.0 (s - (1.0 - e.mp.socle / C.accesBesoins) * manque * g)

// ---------- Patriarcat restant (1 = la France d'aujourd'hui) et violences dans le couple ----------

let patriarcat e =
    let parts =
        [ C.patriarcatVieSeule
          -(max 0.0 ((curseur "prSocle" e - C.patriarcatSoclePlancher) / C.patriarcatSoclePlage)) * C.patriarcatSocle
          (match Etat.texte "heritage" e with
           | Some "lfi" -> C.patriarcatHeritageLfi
           | Some "egaux" -> C.patriarcatHeritageEgaux
           | Some "communs" -> C.patriarcatHeritageCommuns
           | Some "choisie" -> C.patriarcatHeritageChoisie
           | Some "libre" -> C.patriarcatHeritageLibre
           | _ -> 0.0)
          (if est "logement" "modulable" e then C.patriarcatFoyersChoisis else 0.0)
          (if est "soin" "services" e then C.patriarcatSoinServices
           elif est "soin" "partage" e then C.patriarcatSoinPartage
           else 0.0)
          -(curseur "pTirage" e) / 100.0 * C.patriarcatTirage
          (if est "violence" "cellule" e then C.patriarcatCellulesMobilisees else 0.0)
          (if est "laicite" "concordat" e then C.patriarcatCultesReconnus else 0.0) ]

    parts |> List.fold (fun r d -> if abs d >= C.patriarcatSeuil then r + d else r) 1.0 |> Js.clamp01

let baisseViolencesCouple e =
    let partHorizon =
        [| C.violencesPartAujourdhui; C.violencesPart10Ans; C.violencesPart30Ans |][horizon e]

    max 0.0 ((1.0 - patriarcat e) / C.violencesReculPlein) * C.violencesBaisseMax * partHorizon

// ---------- Criminalité : fourchette d'homicides (100 = aujourd'hui) ----------

let fourchetteCrimes e =
    let mutable hi = 100.0 * C.crimePessimistePolice * C.crimePessimistePrison
    let mutable lo = 100.0 * C.crimeOptimistePrevention * C.crimeOptimisteSocle * C.crimeOptimisteReparations
    let vb = baisseViolencesCouple e
    hi <- hi * (1.0 - C.crimePartCouple * vb)
    lo <- lo * (1.0 - C.crimePartCouple * vb)
    let p = risqueGang e

    if est "bande" "gardes" e then hi <- hi * (1.0 - C.crimeGardes * p)

    if est "bande" "mediation" e then
        hi <- hi * (1.0 - C.crimeMediation * p)
        lo <- lo * (1.0 - C.crimeMediation * p)

    if est "bande" "voisins" e then hi <- hi * (1.0 - C.crimeVoisins * p)

    if est "invasion" "milice" e then
        hi <- hi * C.crimeMiliceHaut
        lo <- lo * C.crimeMiliceBas

    if est "froid" "risque" e then hi <- hi * C.crimeCalculFroid
    if enNature e || e.mp.ecart <= C.crimeEcartMax then lo <- lo * C.crimePeuDEcart
    lo, hi

let jaugeCrimes c = Js.clamp01 (1.0 - c / C.crimeJaugePlage)

// ---------- Pouvoirs : risque de prise du pouvoir ----------

/// Nombre de niveaux de la pyramide des cellules, et nombre d'unités au sommet.
let structure (e: Etat) =
    let c, d, p = curseur "cell" e, curseur "del" e, population e

    let tailles =
        List.unfold (fun s -> if s <= p then Some(s, s * d) else None) c |> Array.ofList

    let mutable sommet = tailles.Length - 1

    if e.r then
        while sommet > 0 && p / tailles[sommet] < C.pyramideSommetMinimum do
            sommet <- sommet - 1

    sommet, ceil (p / tailles[sommet] - 1e-9)

let paradeActive e (p: Parade) =
    if p.adoptee then
        true
    else
        match p.automatique with
        | Some MonnaiePlafonnee -> monnaieEst "plafond" e
        | Some MonnaieAvecEcart -> avecMonnaieFondanteOuPlafonnee e && e.mp.ecart >= C.paradeEcartMinimum
        | Some ProtocoleAdopte -> Js.vrai (Etat.choix "proto" e)
        | None -> Etat.coche "par" p.id e

let captationParades e =
    faiblesses
    |> List.concat
    |> List.fold (fun c p -> if p.captation <> 0.0 && paradeActive e p then c + p.captation else c) 0.0

let captationMonnaie e =
    match monnaie e with
    | Some("fondante" | "plafond" as m) ->
        (e.mp.ecart - 1.0) * C.pouvoirMonnaieEcart
        + (if m = "plafond" then
               max 0.0 ((e.mp.plafond - C.pouvoirMonnaiePlafondReference) * C.pouvoirMonnaiePlafond)
           else
               0.0)
    | _ -> 0.0

/// Risque de prise du pouvoir, sans bornes (ancien `captureCalc(true)`).
let risquePouvoirBrut e =
    let niveaux, unites = structure e

    C.pouvoirBase
    + (curseur "pMandat" e - C.pouvoirMandatReference) * C.pouvoirMandat
    + (100.0 - curseur "pTirage" e) * C.pouvoirTirage
    + (curseur "pRevoc" e - C.pouvoirRevocationReference) * C.pouvoirRevocation
    + (curseur "pDom" e - C.pouvoirDomainesReference) * C.pouvoirDomaines
    + (float niveaux - C.pouvoirNiveauxReference) * C.pouvoirNiveau
    + (if unites < C.pyramideSommetMinimum then C.pouvoirSommetPetit else 0.0)
    + (if est "invasion" "armee" e then C.pouvoirArmee else 0.0)
    + (if est "bande" "gardes" e then C.pouvoirGardes * risqueGang e else 0.0)
    + captationMonnaie e
    + sommeParis _.captation e
    + captationParades e

let risquePouvoir e = Js.clamp100 (risquePouvoirBrut e)

// ---------- Passage à l'échelle ----------

let faiblessesCouvertes e =
    faiblesses |> List.filter (fun f -> float (f |> List.filter (paradeActive e)).Length >= C.paradesSuffisantes) |> List.length

let jaugeEchelle e =
    if faiblessesCouvertes e < faiblesses.Length then C.echelleReduit
    elif not (Js.vrai (Etat.choix "proto" e)) then C.echelleSurLePapier
    else 1.0

// ---------- Stress test ----------

/// Verdict de chaque attaque, selon les choix (ancien `stressV`, sans les textes).
let verdicts e =
    let cap = Js.round (risquePouvoir e)
    let inv0, inv2 = est "invasion" "milice" e, est "invasion" "armee" e

    Modele.attaques
    |> List.map (fun a ->
        match a.id with
        | "deleg" when cap > C.stressSeuilPouvoir -> "fissure"
        | "arme" when inv0 || inv2 -> "fissure"
        | "impulsif" when inv0 -> "fissure"
        | "dissu" when est "bande" "gardes" e -> "tient"
        | "foule" when est "bande" "voisins" e -> "fissure"
        | "cache" when baisseViolencesCouple e >= C.stressSeuilViolences -> "tient"
        // toute monnaie a de la valeur, donc crée des cibles (objections de l'auteur, 26 novembre 2026) :
        // à la frontière, un trésor et une frontière à garder, que des gardes contre un gang ne gardent pas ;
        // bons, monnaie fondante ou plafonnée : sans gardes, personne pour protéger les cibles
        | "vol" when est "monnaie" "frontiere" e -> "fissure"
        | "vol" when (est "monnaie" "bons" e || est "monnaie" "fondante" e || est "monnaie" "plafond" e) && not (est "bande" "gardes" e) -> "fissure"
        | _ -> a.verdict)

// ---------- Test de fermeture ----------

let couvertureRessource (r: RessourcePays) (e: Etat) =
    let initiale =
        match r.importeeDes with
        | Some n when n <= ouverture e -> "imp"
        | _ ->
            match r.regleePar with
            | Some lev when Etat.defini (champDuLevier lev) e ->
                match Etat.choix (champDuLevier lev) e with
                | Some(Num i) -> couvertureOption lev (int i)
                | _ -> failwith $"{champDuLevier lev} n'est pas un rang"
            | _ -> if e.p = "om" then r.franceEntiere else r.metropole

    match couvertureParPari r.id e with
    | Some b when rangCouverture b > rangCouverture initiale -> b
    | _ -> initiale

// ---------- Les liens du réseau ----------

let facteurPoids poids e =
    match poids with
    | Plein -> 1.0
    | Si(c, f) -> if conditionVraie c e then f else 1.0
    | Facteur "risque-gang" -> risqueGang e
    | Facteur autre -> failwith $"facteur inconnu {autre}"

/// Les courbes : les liens qui dépendent d'un curseur ou d'une valeur calculée.
let courbe (c: Courbe) e =
    let n i = float c.args[i]

    match c.nom with
    | "risque-evite" ->
        let k = champDuLevier c.args[0]
        let p = risqueGang e
        let f = prevention |> List.find (fun (x, _) -> x = k) |> snd
        if Js.vrai (Etat.choix k e) && p < 1.0 then (1.0 - p) * log f / log p else 0.0
    | "baisse-violences-couple" -> baisseViolencesCouple e * C.courbeViolencesFacteur
    | "duree-vie-commune" ->
        let d =
            match Map.tryFind "accD" e.s with
            | Some v -> v
            | None -> etatDeDepart.s["accD"]

        C.courbeAccueilMax
        - C.courbeAccueilBaisse * (min C.courbeAccueilDureeMax (max 1.0 d) - 1.0) / (C.courbeAccueilDureeMax - 1.0)
    | "rendement-eleve" -> if rendementPerdu e >= C.courbeRendementSeuil then 1.0 else 0.0
    | "tirage-sous-moitie" -> min 0.0 ((curseur "pTirage" e - C.courbeTiragePivot) / C.courbeTiragePivot * n 0)
    | "tirage-au-dessus-moitie" -> max 0.0 ((curseur "pTirage" e - C.courbeTiragePivot) / C.courbeTiragePivot * n 0)
    | "revocation-facile" ->
        max 0.0 (min C.courbeRevocationFacileMax ((C.courbeRevocationFacileSeuil - curseur "pRevoc" e) / C.courbeRevocationFacilePas))
    | "mandat-court" -> max 0.0 (min 1.0 ((C.courbeMandatCourtSeuil - curseur "pMandat" e) / C.courbeMandatCourtPas))
    | "socle" ->
        let v = curseur "prSocle" e

        if v < C.courbeSoclePivot then
            -(C.courbeSoclePivot - v) / C.courbeSoclePas * n 0
        else
            (v - C.courbeSoclePivot) * n 1
    | "revocation" ->
        let v = curseur "pRevoc" e

        if v <= C.courbeRevocationPivot then
            (C.courbeRevocationPivot - v) / C.courbeRevocationPasBas * C.courbeRevocationEffetBas
        else
            -(v - C.courbeRevocationPivot) / C.courbeRevocationPasHaut * C.courbeRevocationEffetHaut
    | "mandat-inexperience" ->
        let v = curseur "pMandat" e

        if v < C.courbeMandatPivot then
            -(C.courbeMandatPivot - v) / C.courbeMandatPas * C.courbeMandatEffet
        else
            0.0
    | autre -> failwith $"courbe inconnue {autre}"

/// La lentille d'un lien est-elle active ? Par défaut toutes le sont ; `c.lentilles` (un objet
/// lentille → vrai ou faux) permet d'en écarter.
let lentilleActive (l: Lien) e =
    match Etat.choix "lentilles" e with
    | Some(Obj m) ->
        match Map.tryFind l.lentille m with
        | Some v -> Js.vrai (Some v)
        | None -> true
    | _ -> true

type LienActif =
    { /// rang dans data/liens.csv (et dans l'ancien tableau LINKS)
      rang: int
      lien: Lien
      /// effet en points, après condition, poids et courbe
      effet: float }

/// Les liens actifs : effet d'au moins 0,05 point (ancien `linksOn`).
let liensActifs e =
    Liens.tous
    |> List.indexed
    |> List.choose (fun (i, l) ->
        let f =
            if lentilleActive l e && conditionVraie l.quand e then
                facteurPoids l.poids e
                * (match l.courbe with
                   | Some c -> courbe c e
                   | None -> 1.0)
            else
                0.0

        let x = f * l.effet
        if abs x >= C.lienSeuil then Some { rang = i; lien = l; effet = x } else None)

let sommeLiens (actifs: LienActif list) jauge =
    (actifs |> List.filter (fun a -> a.lien.jauge = jauge) |> List.fold (fun s a -> s + a.effet) 0.0) / 100.0

/// Les effets du temps (`c.effets` : jauge → points), écrits par le moteur de jeu : un événement,
/// une décision qui s'installe. Comptés comme des liens.
let effetsDuTemps (e: Etat) jauge =
    match Etat.choix "effets" e with
    | Some(Obj m) ->
        match Map.tryFind jauge m with
        | Some(Num points) -> points / 100.0
        | _ -> 0.0
    | _ -> 0.0

/// Bornes souples : près de 0 ou de 1, une jauge ralentit mais bouge encore. Pour toutes les
/// jauges (l'ancien moteur ne les appliquait qu'aux jauges qu'un lien pouvait faire monter, ou
/// baisser : ajouter un lien, même inactif, changeait tous les scores).
let borneSouple (x: float) =
    let k = C.borneSouple

    if x < k then k * exp ((x - k) / k)
    elif x > 1.0 - k then 1.0 - k * exp (-(x - 1.0 + k) / k)
    else x

// ---------- Les jauges ----------

type ValeurJauge =
    | Valeur of float
    | Fourchette of bas: float * haut: float

    member v.Milieu =
        match v with
        | Valeur x -> x
        | Fourchette(a, b) -> (a + b) / 2.0

type JaugeCalculee =
    { id: string
      valeur: ValeurJauge
      /// hors score : la jauge mesure le but
      horsScore: bool
      /// seuil « solide » (pondère la légitimité), pour le modèle et pour la France
      solide: float
      solideFrance: float }

type Viabilite =
    { ok: bool
      /// la jauge vitale la plus loin de son seuil, et son rapport valeur / seuil
      maillon: string * float }

type Resultat =
    { jauges: JaugeCalculee list
      liens: LienActif list
      /// score : moyenne des jauges de résultat, sur 100
      score: float
      legitimite: float
      fidelite: float
      viable: Viabilite
      scoreFrance: float
      legitimiteFrance: float
      fideliteFrance: float
      viableFrance: Viabilite }

let r2 x = Js.round (x * 100.0) / 100.0

let jaugeDe = Jauges.tous |> List.map (fun j -> j.id, j) |> Map.ofList

/// Seuils vitaux (data/jauges.csv) : une famine ne se compense pas par un excédent ailleurs.
let viabilite (valeur: string -> float) =
    let it =
        Jauges.tous |> List.choose (fun j -> j.seuilVital |> Option.map (fun s -> j.id, valeur j.id / s))

    let pire = it |> List.reduce (fun a b -> if snd b < snd a then b else a)
    { ok = snd pire >= 1.0; maillon = pire }

/// La valeur France d'une jauge, aujourd'hui (data/pays/fr/jauges.csv) ou selon la tendance à
/// l'horizon choisi, avant les bornes souples.
let valeurFranceBrute jauge (e: Etat) =
    let hz = horizon e
    let ans = anneesHorizon[hz]
    let pf = populationHorizon[hz]

    let aujourdhui () =
        (F.jauges |> List.find (fun v -> v.jauge = jauge)).valeur |> Option.defaultValue nan

    match jauge with
    | "niveau-de-vie" when hz > 0 ->
        F.tendanceEnergie ** (ans + F.tendanceEnergieDecalage) * F.populationAujourdhui / pf * (1.0 - F.pauvrete)
    | "nourriture" when hz > 0 ->
        min 1.0 ((F.surfaceAgricole - F.terresPerduesParAn * ans) / F.besoinParPersonne / pf)
        * (1.0 - min 1.0 (F.faim * F.croissanceAideAlimentaire ** ans * F.populationAujourdhui / pf))
    | _ -> aujourdhui ()

/// La valeur France d'une jauge, mesurée comme celles de la partie (mêmes bornes souples) : sans
/// cela, une jauge pleine en France (Paris, Échelle : 1) resterait hors d'atteinte.
let valeurFrance jauge (e: Etat) = borneSouple (valeurFranceBrute jauge e)

/// Plus de domaines nationaux que la référence : le pouvoir remonte (test `centralisation`).
let centralise e = curseur "pDom" e > C.pouvoirDomainesReference

/// La grande industrie exporte, dans un pays entrouvert (test `industrie-exportatrice`).
let industrieExportatrice e =
    let industrie = F.exportations |> List.tryFind (fun x -> x.id = "industrie")
    ouverture e >= rangDe "ouverture" "limite" && ouverture e < rangDe "ouverture" "ouvert" && industrie |> Option.exists (fun x -> exportationActive x e)

/// Un test nommé de data/jeu/abolitions.csv (colonne calcul).
let calculNomme nom e =
    match nom with
    | "centralisation" -> centralise e
    | "industrie-exportatrice" -> industrieExportatrice e
    | _ -> failwith $"test inconnu du moteur : {nom}"

let evaluer (e: Etat) =
    let v = verdicts e
    let tient = v |> List.filter ((=) "tient") |> List.length |> float
    let fissure = v |> List.filter ((=) "fissure") |> List.length |> float

    let couvertures = F.ressources |> List.map (fun r -> couvertureRessource r e)
    let couvertes = couvertures |> List.filter (fun c -> c = "ok" || c = "imp") |> List.length |> float
    let partielles = couvertures |> List.filter ((=) "part") |> List.length |> float

    let p = population e
    let energieParPersonne = energieDisponible e * 1e6 / p
    let reference = F.energieAujourdhui * 1e6 / population0 e
    let service = energieParPersonne * (1.0 + sommeParis _.confort e)
    let gang = risqueGang e

    // Principes : la monnaie, l'armée, les gardes, la centralisation, l'héritage libre, la grande industrie
    let partMonnaie =
        match monnaie e with
        | Some "frontiere" -> C.principesMonnaieFrontiere
        | Some "bons" -> C.principesMonnaieBons
        | Some "fondante" -> C.principesMonnaieFondante
        | Some "plafond" -> C.principesMonnaiePlafond
        | _ -> 0.0

    let mutable ecart = partMonnaie * C.principesMonnaiePoids
    if est "invasion" "armee" e then ecart <- ecart + C.principesArmee
    // des gardes sont une police, quelle que soit la menace (décision de l'auteur, 26 novembre 2026)
    if est "bande" "gardes" e then ecart <- ecart + C.principesGardes
    if est "violence" "gardes" e then ecart <- ecart + C.principesGardesViolence
    // exclusions, surveillance par les machines, milices armées (décision de l'auteur, 26 novembre 2026)
    if est "froid" "exil" e then ecart <- ecart + C.principesExil
    if est "froid" "eloignement" e then ecart <- ecart + C.principesEloignement
    if est "surveillance" "encadree" e then ecart <- ecart + C.principesSurveillanceEncadree
    if est "surveillance" "generalisee" e then ecart <- ecart + C.principesSurveillanceGeneralisee
    if est "invasion" "milice" e then ecart <- ecart + C.principesMilice
    if est "secession" "milice" e then ecart <- ecart + C.principesMilice
    let pd = curseur "pDom" e

    if pd > C.pouvoirDomainesReference then
        ecart <- ecart + min C.principesDomainesMax ((pd - C.pouvoirDomainesReference) * C.principesDomaines)

    if est "heritage" "libre" e then ecart <- ecart + C.principesHeritageLibre

    if industrieExportatrice e then
        ecart <- ecart + C.principesIndustrie

    let lo, hi = fourchetteCrimes e
    let moyens = 1.0 - (C.moyensCouts + (if est "bande" "gardes" e then C.moyensGardes * gang else 0.0))
    let imp, _ = echanges e

    // Liberté : chaque contrainte retire des points
    let mutable lib = 1.0
    if est "bande" "gardes" e then lib <- lib - C.liberteGardes * gang
    if est "invasion" "milice" e then lib <- lib - C.liberteMilice
    if est "froid" "exil" e then lib <- lib - C.liberteExil
    elif est "froid" "eloignement" e then lib <- lib - C.liberteEloignement
    if est "partir" "engagement" e then lib <- lib - C.liberteEngagement
    let lib = C.liberteBase - (1.0 - lib)

    let ouvert = ouverture e = rangDe "ouverture" "ouvert"
    let actifs = liensActifs e
    let somme j = sommeLiens actifs j + effetsDuTemps e j
    let fid = 1.0 - ecart + somme "principes"
    let risque = risqueParis e

    let solide id =
        let j = jaugeDe[id]
        let s = Option.defaultValue nan j.solide
        let so = Option.defaultValue nan j.solideOuvert

        match id with
        | "echanges" -> if ouvert || imp = 0.0 then so else s
        | "paris" -> if risque > 0.0 then s else 1.0
        | _ -> if ouvert then so else s

    let brutes =
        [ "securite", Valeur((tient + fissure * C.securiteFissure) / float v.Length + somme "securite")
          "criminalite", Fourchette(jaugeCrimes hi, jaugeCrimes lo)
          "liberte", Valeur lib
          "moyens-liberes", Valeur moyens
          "niveau-de-vie", Valeur(min 1.0 (service / reference) * acces false e)
          "nourriture", Valeur(min 1.0 (nourris e / p) * acces true e)
          "ressources", Valeur((couvertes + partielles * C.ressourcesPartielle) / float F.ressources.Length)
          "echanges", Valeur(ratioEchanges e)
          "pouvoirs", Valeur(1.0 - risquePouvoirBrut e / 100.0)
          "echelle", Valeur(jaugeEchelle e)
          "paris", Valeur(1.0 - risque / 100.0)
          "egalite", Valeur C.egaliteDepart
          "principes", Valeur fid ]

    // le réseau : chaque jauge reçoit les effets de ses liens (Sécurité et Principes les ont déjà)
    let jauges =
        brutes
        |> List.map (fun (id, valeur) ->
            let d = if id = "securite" || id = "principes" then 0.0 else somme id
            let j = jaugeDe[id]

            let valeur =
                match valeur with
                | Valeur x -> Valeur(borneSouple (x + d))
                | Fourchette(a, b) -> Fourchette(borneSouple (a + d), borneSouple (b + d))

            { id = id
              valeur = valeur
              horsScore = j.horsScore
              solide = if j.horsScore then nan else solide id
              solideFrance = Option.defaultValue nan j.solideFrance })

    let jauge id = jauges |> List.find (fun j -> j.id = id)
    let resultat = jauges |> List.filter (fun j -> not j.horsScore)
    let n = float resultat.Length
    let france j = Js.clamp01 (valeurFrance j.id e)

    let moyenne f = (resultat |> List.fold (fun a j -> a + f j) 0.0) / n * 100.0 |> r2
    let pat = patriarcat e

    { jauges = jauges
      liens = actifs
      score = moyenne _.valeur.Milieu
      legitimite = moyenne (fun j -> j.valeur.Milieu * j.solide)
      fidelite = r2 (((jauge "principes").valeur.Milieu + (1.0 - pat) + (jauge "egalite").valeur.Milieu) / 3.0 * 100.0)
      viable = viabilite (fun id -> (jauge id).valeur.Milieu)
      scoreFrance = moyenne france
      legitimiteFrance = moyenne (fun j -> france j * j.solideFrance)
      fideliteFrance = r2 ((valeurFrance "principes" e + 0.0 + valeurFrance "egalite" e) / 3.0 * 100.0)
      viableFrance = viabilite (fun id -> Js.clamp01 (valeurFrance id e)) }
