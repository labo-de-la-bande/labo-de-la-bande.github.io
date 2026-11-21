/// Le moteur : un état donne toutes les jauges, les liens actifs, le score et les trois réponses
/// (viable ? fidèle au but ? mieux que la France ?). Portage fidèle de l'ancien moteur
/// JavaScript, vérifié contre l'oracle (tests/Labo.Tests/OracleTests.fs). Pur, sans E/S : compilé
/// en .NET et par Fable.
module Labo.Engine.Moteur

open Labo.Data
open Labo.Engine.Tables

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
let population e = population0 e * populationHorizon[horizon e] / 69.1

// ---------- Gang : probabilité réduite par la prévention ----------

let risqueGang e =
    prevention |> List.fold (fun p (k, f) -> p * (if Js.vrai (Etat.choix k e) then f else 1.0)) 1.0

// ---------- Paris ----------

let pariActif e (b: Pari) = Etat.coche "bets" b.id e && b.pret <= anneesHorizon[horizon e]

let sommeParis (champ: Pari -> float) e =
    paris
    |> List.fold
        (fun s b ->
            let v = champ b

            if not (pariActif e b) || v = 0.0 then s
            elif b.monnaie && not (avecMonnaieFondanteOuPlafonnee e) then s
            else s + v)
        0.0

let couvertureParPari ressource e =
    paris
    |> List.fold
        (fun meilleur b ->
            match pariActif e b, List.tryFind (fun (r, _) -> r = ressource) b.couvre with
            | true, Some(_, v) ->
                match meilleur with
                | Some m when rangCouverture v <= rangCouverture m -> meilleur
                | _ -> Some v
            | _ -> meilleur)
        None

let risqueParis e =
    paris
    |> List.fold (fun s b -> s + (if pariActif e b then float b.maturite else 0.0)) 0.0
    |> fun s -> Js.clamp100 (s * 7.0)

// ---------- Nourriture et énergie ----------

let rendementPerdu e =
    let y = curseur "yield" e
    let y = if Etat.estNum "res_engrais" 1.0 e && ouverture e < 2 then max y 25.0 else y
    max 0.0 (y - sommeParis _.rendement e)

let besoinParPersonne e = max 0.15 (curseur "need" e - sommeParis _.besoin e)

let energieDisponible e =
    curseur "supply" e + sommeParis _.energie e
    - (if Etat.estNum "res_engrais" 2.0 e then nh3Twh else 0.0)
    - (if Etat.estNum "coord" 3.0 e then coordIaTwh else 0.0)

/// Personnes que les terres peuvent nourrir.
let nourris e =
    (27.9 - curseur "loss" e * 44.0 / 1000.0) / besoinParPersonne e * (1.0 - rendementPerdu e / 100.0) * 1e6

// ---------- Échanges ----------

let exportationActive id e =
    match Etat.choix "exp" e with
    | Some(Obj m) -> Js.vrai (Map.tryFind id m)
    | v when Js.vrai v -> false
    | _ -> exportationsParDefaut.Contains id

let valeurExportation id e =
    match id with
    | "elec" -> 5.1 + 0.057 * max 0.0 (sommeParis _.energie e)
    | "food" -> max 0.0 ((nourris e - population e) / 1e6 * 0.2)
    | "vin" -> 15.6
    | "savoir" -> 5.0
    | "accueil" -> 10.0
    | "bois" -> 1.0
    | "industrie" -> 30.0
    | autre -> failwith $"exportation inconnue {autre}"

/// Importations, exportations (Md€).
let echanges e =
    let l = ouverture e

    if l = 3 then
        importationsAujourdhui, exportationsAujourdhui
    else
        let imp =
            [ 1..l ] |> List.collect (fun i -> importations |> Map.tryFind i |> Option.defaultValue []) |> List.fold (+) 0.0

        let exp =
            exportations
            |> List.fold (fun s id -> s + (if exportationActive id e then valeurExportation id e else 0.0)) 0.0

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
    let manque = if nourriture then 0.12 else 0.154
    let g = Js.clamp01 ((e.mp.ecart - 1.0) / (ecartD9D1 - 1.0))

    if enNature e then s
    elif monnaieEst "fondante" e then 1.0 - manque * g
    elif nourriture then (if e.mp.socle >= 2.0 then s else 1.0 - manque * g)
    else max 0.0 (s - (1.0 - e.mp.socle / 5.0) * manque * g)

// ---------- Patriarcat restant (1 = la France d'aujourd'hui) et violences dans le couple ----------

let patriarcat e =
    let parts =
        [ -0.10
          -(max 0.0 ((curseur "prSocle" e - 90.0) / 10.0)) * 0.20
          (match Etat.texte "heritage" e with
           | Some "lfi" -> -0.05
           | Some "egaux" -> -0.10
           | Some "communs" -> -0.15
           | Some "choisie" -> -0.03
           | Some "libre" -> 0.10
           | _ -> 0.0)
          (if Etat.estNum "logt" 2.0 e then -0.05 else 0.0)
          (if Etat.estNum "soin" 1.0 e then -0.03
           elif Etat.estNum "soin" 2.0 e then -0.10
           else 0.0)
          -(curseur "pTirage" e) / 100.0 * 0.10
          (if Etat.estNum "vc" 2.0 e then -0.15 else 0.0)
          (if Etat.estNum "laic" 2.0 e then 0.05 else 0.0) ]

    parts |> List.fold (fun r d -> if abs d >= 0.005 then r + d else r) 1.0 |> Js.clamp01

let baisseViolencesCouple e =
    max 0.0 ((1.0 - patriarcat e) / 0.70) * 0.5 * [| 0.3; 0.6; 1.0 |][horizon e]

// ---------- Criminalité : fourchette d'homicides (100 = aujourd'hui) ----------

let fourchetteCrimes e =
    let mutable hi = 100.0 * 1.16 * 1.05
    let mutable lo = 100.0 * 0.85 * 0.90 * 0.95
    let vb = baisseViolencesCouple e
    hi <- hi * (1.0 - 0.14 * vb)
    lo <- lo * (1.0 - 0.14 * vb)
    let p = risqueGang e

    if Etat.estNum "def0" 0.0 e then hi <- hi * (1.0 - 0.05 * p)

    if Etat.estNum "def0" 1.0 e then
        hi <- hi * (1.0 - 0.02 * p)
        lo <- lo * (1.0 - 0.02 * p)

    if Etat.estNum "def0" 2.0 e then hi <- hi * (1.0 - 0.01 * p)

    if Etat.estNum "def2" 0.0 e then
        hi <- hi * 1.10
        lo <- lo * 1.05

    if Etat.estNum "dl0" 2.0 e then hi <- hi * 1.02
    if enNature e || e.mp.ecart <= 3.0 then lo <- lo * 0.95
    lo, hi

let jaugeCrimes c = Js.clamp01 (1.0 - c / 200.0)

// ---------- Pouvoirs : risque de prise du pouvoir ----------

/// Nombre de niveaux de la pyramide des cellules, et nombre d'unités au sommet.
let structure (e: Etat) =
    let c, d, p = curseur "cell" e, curseur "del" e, population e

    let tailles =
        List.unfold (fun s -> if s <= p then Some(s, s * d) else None) c |> Array.ofList

    let mutable sommet = tailles.Length - 1

    if e.r then
        while sommet > 0 && p / tailles[sommet] < 10.0 do
            sommet <- sommet - 1

    sommet, ceil (p / tailles[sommet] - 1e-9)

let paradeActive e (p: Parade) =
    if p.adoptee then
        true
    else
        match p.auto with
        | Some MonnaiePlafonnee -> monnaieEst "plafond" e
        | Some MonnaieAvecEcart -> avecMonnaieFondanteOuPlafonnee e && e.mp.ecart >= 2.0
        | Some ProtocoleAdopte -> Js.vrai (Etat.choix "proto" e)
        | None -> Etat.coche "par" p.id e

let captationParades e =
    faiblesses |> List.concat |> List.fold (fun c p -> if p.captation <> 0.0 && paradeActive e p then c + p.captation else c) 0.0

let captationMonnaie e =
    match monnaie e with
    | Some("fondante" | "plafond" as m) ->
        (e.mp.ecart - 1.0) * 2.0 + (if m = "plafond" then max 0.0 ((e.mp.plafond - 10.0) * 0.4) else 0.0)
    | _ -> 0.0

/// Risque de prise du pouvoir, sans bornes (ancien `captureCalc(true)`).
let risquePouvoirBrut e =
    let niveaux, unites = structure e

    30.0
    + (curseur "pMandat" e - 12.0) * 1.5
    + (100.0 - curseur "pTirage" e) * 0.25
    + (curseur "pRevoc" e - 20.0) * 0.6
    + (curseur "pDom" e - 3.0) * 5.0
    + float (niveaux - 3) * 6.0
    + (if unites < 10.0 then 15.0 else 0.0)
    + (if Etat.estNum "def2" 2.0 e then 15.0 else 0.0)
    + (if Etat.estNum "def0" 0.0 e then 5.0 * risqueGang e else 0.0)
    + captationMonnaie e
    + sommeParis _.captation e
    + captationParades e

let risquePouvoir e = Js.clamp100 (risquePouvoirBrut e)

// ---------- Passage à l'échelle ----------

let faiblessesCouvertes e =
    faiblesses |> List.filter (fun f -> (f |> List.filter (paradeActive e)).Length >= 2) |> List.length

let jaugeEchelle e =
    if faiblessesCouvertes e < 5 then 0.33
    elif not (Js.vrai (Etat.choix "proto" e)) then 0.66
    else 1.0

// ---------- Stress test ----------

/// Verdict de chaque attaque, selon les choix (ancien `stressV`, sans les textes).
let verdicts e =
    let cap = Js.round (risquePouvoir e)
    let inv0, inv2 = Etat.estNum "def2" 0.0 e, Etat.estNum "def2" 2.0 e

    stress
    |> List.map (fun (k, v) ->
        match k with
        | Some "deleg" when cap > 60.0 -> "fissure"
        | Some "arme" when inv0 || inv2 -> "fissure"
        | Some "impulsif" when inv0 -> "fissure"
        | Some "dissu" when Etat.estNum "def0" 0.0 e -> "tient"
        | Some "foule" when Etat.estNum "def0" 2.0 e -> "fissure"
        | Some "cache" when baisseViolencesCouple e >= 0.3 -> "tient"
        | _ -> v)

// ---------- Test de fermeture ----------

let couvertureRessource (nom, metropole, franceEntiere) (e: Etat) =
    let l = ouverture e

    let initiale =
        if [ 1..l ] |> List.exists (fun i -> couvertParOuverture |> Map.tryFind i |> Option.exists (List.contains nom)) then
            "imp"
        else
            match ressourceReglee.TryFind nom with
            | Some table when Etat.defini ("res_" + table) e ->
                match Etat.choix ("res_" + table) e with
                | Some(Num i) -> couvertureOption[table][int i]
                | _ -> failwith $"res_{table} n'est pas un rang"
            | _ -> if e.p = "om" then franceEntiere else metropole

    match couvertureParPari nom e with
    | Some b when rangCouverture b > rangCouverture initiale -> b
    | _ -> initiale

// ---------- Les liens du réseau ----------

let levierDe = Leviers.tous |> List.map (fun l -> l.id, l) |> Map.ofList

let optionsDe =
    Leviers.options |> List.groupBy _.levier |> List.map (fun (k, os) -> k, os) |> Map.ofList

/// La valeur que l'état donne au champ d'un levier (`c.x` ou `s.x`).
let valeurChamp (champ: string) (e: Etat) =
    match champ.Split '.' with
    | [| "c"; k |] -> Etat.choix k e
    | [| "s"; k |] -> Map.tryFind k e.s |> Option.map Num
    | _ -> None

/// L'option `id` du levier est-elle celle de l'état ?
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

let facteurPoids poids e =
    match poids with
    | Plein -> 1.0
    | Si(c, f) -> if conditionVraie c e then f else 1.0
    | Facteur "risque-gang" -> risqueGang e
    | Facteur autre -> failwith $"facteur inconnu {autre}"

let champDuLevier id =
    match levierDe[id].champ.Split '.' with
    | [| _; k |] -> k
    | _ -> failwith $"le levier {id} n'a pas de champ"

/// Les courbes : les liens qui dépendent d'un curseur ou d'une valeur calculée.
let courbe (c: Courbe) e =
    let n i = float c.args[i]

    match c.nom with
    | "risque-evite" ->
        let k = champDuLevier c.args[0]
        let p = risqueGang e
        let f = prevention |> List.find (fun (x, _) -> x = k) |> snd
        if Js.vrai (Etat.choix k e) && p < 1.0 then (1.0 - p) * log f / log p else 0.0
    | "baisse-violences-couple" -> baisseViolencesCouple e * 10.0
    | "duree-vie-commune" ->
        let d =
            match Map.tryFind "accD" e.s with
            | Some v -> v
            | None -> 12.0

        4.0 - 3.0 * (min 36.0 (max 1.0 d) - 1.0) / 35.0
    | "rendement-eleve" -> if rendementPerdu e >= 20.0 then 1.0 else 0.0
    | "tirage-sous-moitie" -> min 0.0 ((curseur "pTirage" e - 50.0) / 50.0 * n 0)
    | "tirage-au-dessus-moitie" -> max 0.0 ((curseur "pTirage" e - 50.0) / 50.0 * n 0)
    | "revocation-facile" -> max 0.0 (min 2.0 ((15.0 - curseur "pRevoc" e) / 5.0))
    | "mandat-court" -> max 0.0 (min 1.0 ((6.0 - curseur "pMandat" e) / 3.0))
    | "socle" ->
        let v = curseur "prSocle" e
        if v < 99.0 then -(99.0 - v) / 9.0 * n 0 else (v - 99.0) * n 1
    | "revocation" ->
        let v = curseur "pRevoc" e
        if v <= 20.0 then (20.0 - v) / 10.0 * 3.0 else -(v - 20.0) / 20.0 * 2.0
    | "mandat-inexperience" ->
        let v = curseur "pMandat" e
        if v < 12.0 then -(12.0 - v) / 9.0 * 2.0 else 0.0
    | autre -> failwith $"courbe inconnue {autre}"

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
            if conditionVraie l.quand e then
                facteurPoids l.poids e * (match l.courbe with Some c -> courbe c e | None -> 1.0)
            else
                0.0

        let x = f * l.effet
        if abs x >= 0.05 then Some { rang = i; lien = l; effet = x } else None)

let sommeLiens (actifs: LienActif list) jauge =
    (actifs |> List.filter (fun a -> a.lien.jauge = jauge) |> List.fold (fun s a -> s + a.effet) 0.0) / 100.0

/// Bornes souples : près de 0 ou de 1, une jauge ralentit mais bouge encore.
let jaugesQuiMontent =
    Liens.tous |> List.filter (fun l -> l.effet > 0.0) |> List.map _.jauge |> Set.ofList |> Set.add "pouvoirs" |> Set.add "securite"

let jaugesQuiBaissent =
    Liens.tous |> List.filter (fun l -> l.effet < 0.0) |> List.map _.jauge |> Set.ofList |> Set.add "pouvoirs" |> Set.add "securite"

let borneSouple jauge x =
    let k = 0.03

    if x < k && jaugesQuiBaissent.Contains jauge then k * exp ((x - k) / k)
    elif x > 1.0 - k && jaugesQuiMontent.Contains jauge then 1.0 - k * exp (-(x - 1.0 + k) / k)
    else Js.clamp01 x

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

/// Seuils vitaux (jugements) : une famine ne se compense pas par un excédent ailleurs.
let seuilsVitaux =
    [ "nourriture", 0.85; "niveau-de-vie", 0.40; "securite", 0.50; "ressources", 0.50; "pouvoirs", 0.40 ]

let viabilite (valeur: string -> float) =
    let it = seuilsVitaux |> List.map (fun (j, s) -> j, valeur j / s)
    let pire = it |> List.reduce (fun a b -> if snd b < snd a then b else a)
    { ok = snd pire >= 1.0; maillon = pire }

/// La valeur France d'une jauge, aujourd'hui (data/pays/fr/jauges.csv) ou selon la tendance à
/// l'horizon choisi.
let valeurFrance jauge (e: Etat) =
    let hz = horizon e
    let ans = float anneesHorizon[hz]
    let pf = populationHorizon[hz]
    let aujourdhui () = (Pays.Fr.jauges |> List.find (fun v -> v.jauge = jauge)).valeur |> Option.defaultValue nan

    match jauge with
    | "niveau-de-vie" when hz > 0 -> 0.993 ** (ans + 3.0) * 69.1 / pf * (1.0 - 0.154)
    | "nourriture" when hz > 0 ->
        min 1.0 ((27.9 - 0.06 * ans) / 0.31 / pf) * (1.0 - min 1.0 (0.12 * 1.0385 ** ans * 69.1 / pf))
    | _ -> aujourdhui ()

let evaluer (e: Etat) =
    let v = verdicts e
    let tient = v |> List.filter ((=) "tient") |> List.length |> float
    let fissure = v |> List.filter ((=) "fissure") |> List.length |> float

    let couvertures = ressources |> List.map (fun r -> couvertureRessource r e)
    let couvertes = couvertures |> List.filter (fun c -> c = "ok" || c = "imp") |> List.length |> float
    let partielles = couvertures |> List.filter ((=) "part") |> List.length |> float

    let p = population e
    let energieParPersonne = energieDisponible e * 1e6 / p
    let reference = energieAujourdhui * 1e6 / population0 e
    let service = energieParPersonne * (1.0 + sommeParis _.confort e)
    let gang = risqueGang e

    // Principes : la monnaie, l'armée, les gardes, la centralisation, l'héritage libre, la grande industrie
    let partMonnaie =
        match monnaie e with
        | Some "frontiere"
        | Some "bons" -> 0.25
        | Some "fondante" -> 0.6
        | Some "plafond" -> 1.0
        | _ -> 0.0

    let mutable ecart = partMonnaie * 0.35
    if Etat.estNum "def2" 2.0 e then ecart <- ecart + 0.15
    if Etat.estNum "def0" 0.0 e then ecart <- ecart + 0.1 * gang
    let pd = curseur "pDom" e
    if pd > 3.0 then ecart <- ecart + min 0.15 ((pd - 3.0) * 0.03)
    if Etat.estTxt "heritage" "libre" e then ecart <- ecart + 0.1
    if ouverture e >= 1 && ouverture e < 3 && exportationActive "industrie" e then ecart <- ecart + 0.1

    let lo, hi = fourchetteCrimes e
    let moyens = 1.0 - (0.4 + (if Etat.estNum "def0" 0.0 e then 0.1 * gang else 0.0))
    let imp, _ = echanges e

    // Liberté : chaque contrainte retire des points
    let mutable lib = 1.0
    if Etat.estNum "def0" 0.0 e then lib <- lib - 0.15 * gang
    if Etat.estNum "def2" 0.0 e then lib <- lib - 0.05
    if Etat.estNum "dl0" 0.0 e then lib <- lib - 0.05
    elif Etat.estNum "dl0" 1.0 e then lib <- lib - 0.05
    if Etat.estNum "dl1" 1.0 e then lib <- lib - 0.1
    let lib = 0.9 - (1.0 - lib)

    let ouvert = ouverture e = 3
    let actifs = liensActifs e
    let somme = sommeLiens actifs
    let fid = 1.0 - ecart + somme "principes"
    let risque = risqueParis e

    // id, valeur avant le réseau, hors score, solide, solide pour la France
    let brutes =
        [ "securite", Valeur((tient + fissure * 0.5) / float v.Length + somme "securite"), false, 0.3, 0.3
          "criminalite", Fourchette(jaugeCrimes hi, jaugeCrimes lo), false, 0.5, 1.0
          "liberte", Valeur lib, true, nan, nan
          "moyens-liberes", Valeur moyens, true, nan, nan
          "niveau-de-vie", Valeur(min 1.0 (service / reference) * acces false e), false, (if ouvert then 0.75 else 0.5), 1.0
          "nourriture", Valeur(min 1.0 (nourris e / p) * acces true e), false, (if ouvert then 0.75 else 0.5), 1.0
          "ressources", Valeur((couvertes + partielles * 0.5) / 10.0), false, (if ouvert then 1.0 else 0.5), 1.0
          "echanges", Valeur(ratioEchanges e), false, (if ouvert || imp = 0.0 then 1.0 else 0.5), 1.0
          "pouvoirs", Valeur(1.0 - risquePouvoirBrut e / 100.0), false, 0.3, 0.5
          "echelle", Valeur(jaugeEchelle e), false, 0.3, 1.0
          "paris", Valeur(1.0 - risque / 100.0), false, (if risque > 0.0 then 0.5 else 1.0), 1.0
          "egalite", Valeur 0.45, true, 0.3, 0.5
          "principes", Valeur fid, true, nan, nan ]

    // le réseau : chaque jauge reçoit les effets de ses liens (Sécurité et Principes les ont déjà)
    let jauges =
        brutes
        |> List.map (fun (id, valeur, hors, sol, fsol) ->
            let d = if id = "securite" || id = "principes" then 0.0 else somme id

            let valeur =
                match valeur with
                | Valeur x -> Valeur(borneSouple id (x + d))
                | Fourchette(a, b) -> Fourchette(borneSouple id (a + d), borneSouple id (b + d))

            { id = id
              valeur = valeur
              horsScore = hors
              solide = sol
              solideFrance = fsol })

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
      fideliteFrance = r2 ((0.15 + 0.0 + valeurFrance "egalite" e) / 3.0 * 100.0)
      viableFrance = viabilite (fun id -> Js.clamp01 (valeurFrance id e)) }
