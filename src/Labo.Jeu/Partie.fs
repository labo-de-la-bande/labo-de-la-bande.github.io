/// Une partie : des révolutions solaires, quelques situations à chacune, des décisions qui
/// s'installent dans le temps, des événements, des paris qui mûrissent, le conseil des lentilles
/// et la réputation auprès des habitants. Pur : chaque fonction rend une nouvelle partie, rien
/// n'est modifié en place. Le hasard vient d'une graine : une partie se rejoue à l'identique.
/// Voir docs/bible.md.
module Labo.Jeu.Partie

open Labo.Data
open Labo.Engine

module C = Labo.Data.Coefficients
module F = Labo.Data.Pays.Fr

/// Un choix : une option d'un levier (pour un pari : le levier `paris` et l'identifiant du pari,
/// ou `aucun`).
type Choix = { levier: string; option: string }

/// Ce que fait le joueur. Une partie est sa configuration et la liste de ses actions : l'état se
/// recalcule en les rejouant (Journal.fs), ce qui permet de sauvegarder, reprendre et partager.
type Action =
    | Decider of situation: string * choix: Choix list
    | Repondre of situation: string * replique: string
    | ProposerIdee of situation: string * texte: string
    | RevenirSur of situation: string
    | FinirRevolution

/// Une ligne du journal de la partie.
type Entree =
    | Lu of revolution: int * situation: string
    | Decide of revolution: int * situation: string * choix: Choix list
    | Survenu of revolution: int * evenement: string
    /// un pari choisi arrive à maturité
    | Muri of revolution: int * pari: string
    /// une idée du joueur : une proposition, qui tient en théorie et attend sa preuve
    | Idee of revolution: int * situation: string * texte: string
    /// une réplique choisie dans une scène
    | Repondu of revolution: int * situation: string * replique: string
    /// le joueur revient sur une décision de la révolution en cours
    | Revenu of revolution: int * situation: string * choix: Choix list
    /// le bilan d'une révolution qui s'achève
    | Revolu of revolution: int * score: float * scoreFrance: float * viable: bool * fidelite: float

/// Un effet qui dure : des points sur une jauge, une valeur par révolution à venir (la première
/// est celle de la révolution en cours).
type EffetEnCours =
    { source: string
      jauge: string
      valeurs: float list }

type Partie =
    { /// révolutions solaires écoulées depuis le début (0 = l'année en cours)
      revolution: int
      /// les tranches à jouer, dans l'ordre
      parcours: string list
      /// l'état du modèle
      etat: Etat
      /// confiance de chaque habitant
      reputation: Map<string, float>
      /// situations réglées
      reglees: Set<string>
      /// décisions prises pendant la révolution en cours
      decisionsCetteRevolution: int
      effets: EffetEnCours list
      evenementsVus: Set<string>
      /// la graine choisie au début de la partie (elle définit la partie avec les actions)
      graineDepart: int64
      /// état du générateur de hasard
      graine: int64
      /// sans hasard, seuls surviennent les événements certains (probabilité 1)
      hasard: bool
      /// la dernière décision de la révolution, et ce qu'il faut pour revenir dessus : la situation,
      /// les choix, l'état du modèle et les effets d'avant
      annulable: (string * Choix list * Etat * EffetEnCours list) option
      /// des décisions anciennes que le joueur reconsidère : elles se présentent à nouveau
      rouvertes: string list
      /// le prénom de chaque habitant pour cette partie (tiré dans la banque du pays)
      prenoms: Map<string, string>
      /// pour chaque décision : ce qu'elle a fait aux jauges (à terme), d'où vient le ressenti
      /// des habitants
      ressentis: Map<string, (string * float) list>
      /// pour chaque décision : le ressenti de chaque habitant, tel qu'il l'a vécu à ce moment-là
      vecus: Map<string, Map<string, float>>
      /// la région où se trouve la cellule de départ (`france` : sans région)
      region: string
      /// les actions du joueur depuis le début, de la plus ancienne à la plus récente
      actions: Action list
      /// du plus récent au plus ancien
      journal: Entree list }

type ErreurJeu =
    | TrancheInconnue of tranche: string
    | RegionInconnue of region: string
    | SituationInconnue of situation: string
    /// la situation n'est pas proposée en ce moment (déjà réglée, pas encore venue, ou condition fausse)
    | SituationFermee of situation: string
    | PasUneDecision of situation: string
    /// les choix ne correspondent pas à la situation (un par levier, ou un pari proposé)
    | ChoixInvalides of situation: string
    | OptionRefusee of Options.ErreurOption
    | RevolutionInachevee

let decrire =
    function
    | TrancheInconnue t -> $"tranche inconnue : {t}"
    | RegionInconnue r -> $"région inconnue : {r}"
    | SituationInconnue s -> $"situation inconnue : {s}"
    | SituationFermee s -> $"la situation {s} n'est pas proposée en ce moment"
    | PasUneDecision s -> $"la situation {s} ne demande pas de décision"
    | ChoixInvalides s -> $"ces choix ne correspondent pas à la situation {s}"
    | OptionRefusee(Options.LevierInconnu l) -> $"levier inconnu : {l}"
    | OptionRefusee(Options.OptionInconnue(l, o)) -> $"option inconnue : {l}={o}"
    | RevolutionInachevee -> "des décisions attendent encore cette révolution"

let situationDe = Jeu.situations |> List.map (fun s -> s.id, s) |> Map.ofList
let pariDe = Modele.paris |> List.map (fun b -> b.id, b) |> Map.ofList

// ---------- Hasard rejouable ----------

/// Le générateur de l'oracle (congruence linéaire modulo 2³¹) : un nombre entre 0 et 1, et la
/// graine suivante.
let tirer (graine: int64) =
    let suivante = (graine * 1103515245L + 12345L) % 2147483648L
    float suivante / 2147483648.0, suivante

// ---------- Effets dans l'état ----------

/// Écrit dans l'état ce que le moteur du modèle doit savoir de la partie : les effets en cours
/// (`c.effets`) et les années écoulées (`c.annees`, `c.hz`).
let synchroniser p =
    let sommes =
        p.effets
        |> List.choose (fun f -> f.valeurs |> List.tryHead |> Option.map (fun v -> f.jauge, v))
        |> List.groupBy fst
        |> List.map (fun (j, vs) -> j, Num(vs |> List.sumBy snd))

    let horizon =
        if float p.revolution >= C.horizon30Ans then 2
        elif float p.revolution >= C.horizon10Ans then 1
        else 0

    let c = p.etat.c |> Map.add "annees" (Num(float p.revolution))
    let c = if sommes.IsEmpty then c.Remove "effets" else c.Add("effets", Obj(Map sommes))
    let c = if horizon > 0 then c.Add("hz", Num(float horizon)) else c.Remove "hz"
    { p with etat = { p.etat with c = c } }

/// Les prénoms de la partie : pour chaque habitant, un prénom donné l'année de sa naissance (selon
/// son âge à la révolution 0), tiré au prorata des naissances réelles, tous différents.
let tirerPrenoms (region: string) (graine: int64) =
    Jeu.habitants
    |> List.fold
        (fun (pris: Map<string, string>, g) h ->
            let decennie = (int F.anneeDepart - h.age) / 10 * 10

            let dans r =
                F.prenoms
                |> List.filter (fun p -> p.region = r && p.decennie = decennie && (h.sexe = "x" || p.sexe = h.sexe))
                |> List.filter (fun p -> not (pris |> Map.exists (fun _ v -> v = p.prenom)))

            // la banque de la région, sinon la banque nationale (Mayotte n'a que des naissances récentes)
            let lot =
                match dans region with
                | [] -> dans "france"
                | l -> l

            match lot with
            | [] -> pris.Add(h.id, h.nom), g
            | _ ->
                let total = lot |> List.sumBy _.naissances |> float
                let x, g2 = tirer g

                let rec choisir cumul =
                    function
                    | [ p: Prenom ] -> p
                    | p :: reste -> if cumul + float p.naissances >= x * total then p else choisir (cumul + float p.naissances) reste
                    | [] -> lot.Head

                pris.Add(h.id, (choisir 0.0 lot).prenom), g2)
        (Map.empty, abs graine % 2147483647L + 1L)
    |> fst

let regionConnue region =
    region = "france" || F.regions |> List.exists (fun r -> r.id = region)

let nouvelleDans region graine hasard (parcours: string list) =
    match parcours |> List.tryFind (fun t -> not (Jeu.tranches |> List.exists (fun x -> x.id = t))) with
    | Some t -> Error(TrancheInconnue t)
    | None when parcours.IsEmpty -> Error(TrancheInconnue "")
    | None when not (regionConnue region) -> Error(RegionInconnue region)
    | None ->
        { revolution = 0
          parcours = parcours
          etat = Tables.etatDeDepart
          reputation = Jeu.habitants |> List.map (fun h -> h.id, 0.0) |> Map.ofList
          reglees = Set.empty
          decisionsCetteRevolution = 0
          effets = []
          evenementsVus = Set.empty
          graineDepart = graine
          graine = graine
          hasard = hasard
          rouvertes = []
          prenoms = tirerPrenoms region graine
          region = region
          actions = []
          ressentis = Map.empty
          vecus = Map.empty
          annulable = None
          journal = [] }
        |> synchroniser
        |> Ok

/// Toutes les tranches, dans l'ordre du jeu.
let parcoursComplet = Jeu.tranches |> List.sortBy _.rang |> List.map _.id

let nouvelleAvec graine hasard parcours = nouvelleDans "france" graine hasard parcours

let nouvelle tranche = nouvelleAvec 2026L true [ tranche ]

let estDecision (s: Situation) =
    s.genre = Dilemme || s.genre = Prevention || s.genre = Paris

// ---------- Les situations de la révolution ----------

let resteDans p tranche =
    Jeu.situations
    |> List.filter (fun s -> s.tranche = tranche && not (p.reglees.Contains s.id) && Moteur.conditionVraie s.quand p.etat)
    |> List.sortBy _.rang

/// La tranche en cours : la première du parcours qui n'est pas finie.
let trancheCourante p =
    p.parcours |> List.tryFind (fun t -> not (resteDans p t).IsEmpty)

/// Les situations qui restent à régler dans la tranche en cours, dans l'ordre du récit.
let enAttente p =
    // les décisions reconsidérées d'abord, puis la tranche en cours
    let rouvertes = p.rouvertes |> List.choose situationDe.TryFind

    let tranche =
        match trancheCourante p with
        | Some t -> resteDans p t |> List.filter (fun s -> not (List.contains s.id p.rouvertes))
        | None -> []

    rouvertes @ tranche

/// Ce qui se présente pendant cette révolution : les constats qui précèdent, et au plus
/// `jeu-situations-par-revolution` décisions. Le bilan vient seul, quand tout le reste est réglé.
let ouvertes p =
    let restantes = int C.jeuSituationsParRevolution - p.decisionsCetteRevolution

    let rec prendre n acc =
        function
        | [] -> List.rev acc
        | (s: Situation) :: reste when s.genre = Bilan -> if acc.IsEmpty && reste.IsEmpty then [ s ] else List.rev acc
        | s :: reste when estDecision s -> if n > 0 then prendre (n - 1) (s :: acc) reste else List.rev acc
        | s :: reste -> prendre n (s :: acc) reste

    prendre restantes [] (enAttente p)

let estOuverte p id = ouvertes p |> List.exists (fun s -> s.id = id)

/// La partie est finie quand tout le parcours est réglé.
let terminee p = enAttente p |> List.isEmpty

/// Applique les choix d'une situation à l'état (un par levier, ou un pari proposé).
let appliquerChoix (s: Situation) (choix: Choix list) p =
    match s.genre, choix with
    | Paris, [ { levier = "paris"; option = "aucun" } ] -> Ok p.etat
    | Paris, [ { levier = "paris"; option = b } ] when List.contains b s.paris ->
        let bets =
            match Etat.choix "bets" p.etat with
            | Some(Obj m) -> m
            | _ -> Map.empty

        Ok { p.etat with c = p.etat.c.Add("bets", Obj(bets.Add(b, Num(float p.revolution)))) }
    | Paris, _ -> Error(ChoixInvalides s.id)
    | _ ->
        if List.sort (choix |> List.map _.levier) <> List.sort s.leviers then
            Error(ChoixInvalides s.id)
        else
            choix
            |> List.fold
                (fun acc c -> acc |> Result.bind (fun e -> Options.appliquer c.levier c.option e |> Result.mapError OptionRefusee))
                (Ok p.etat)

/// Ce que la décision change pour la confiance de chaque habitant qui s'est exprimé.
let effetReputation (s: Situation) (choix: Choix list) =
    let choisies = choix |> List.map _.option |> Set.ofList

    Jeu.voix
    |> List.filter (fun v -> v.situation = s.id)
    |> List.map (fun v ->
        let d =
            match choisies.Contains v.option, v.position with
            | true, Pour -> C.jeuReputationPour
            | true, Contre -> -C.jeuReputationContre
            | false, Pour -> -C.jeuReputationEspoirDecu
            | false, Contre -> C.jeuReputationCrainteEvitee
            | _, Reserve -> 0.0

        v.habitant, d)
    |> List.groupBy fst
    |> List.map (fun (h, ds) -> h, ds |> List.sumBy snd)

/// Le ressenti d'un habitant : la variation des jauges qui comptent pour lui, chacune multipliée
/// par son poids (data/jeu/attachements.csv).
let ressenti (deltas: (string * float) list) habitant =
    Jeu.attachements
    |> List.filter (fun a -> a.habitant = habitant)
    |> List.sumBy (fun a -> a.poids * (deltas |> List.tryFind (fun (j, _) -> j = a.jauge) |> Option.map snd |> Option.defaultValue 0.0))

/// Ce qu'on vit pèse plus : la sensibilité d'un habitant à une jauge, d'après les décisions
/// déjà prises (chaque point perdu sensibilise, chaque point gagné apaise deux fois moins), qui
/// retombe en partie à chaque révolution. Sources de la direction : kahneman1979, brown2024,
/// lucas2007 ; l'ampleur est un réglage de jeu (coefficients jeu-sensibilisation-*).
let sensibilite p habitant jauge =
    let vecues =
        p.journal
        |> List.choose (function Decide(r, s, _) -> Some(r, s) | _ -> None)
        |> List.distinctBy snd

    vecues
    |> List.sumBy (fun (r, s) ->
        match p.ressentis.TryFind s with
        | None -> 0.0
        | Some deltas ->
            let d = deltas |> List.tryFind (fun (j, _) -> j = jauge) |> Option.map snd |> Option.defaultValue 0.0
            let apport = if d < 0.0 then -d * C.jeuSensibilisationPerte else -d * C.jeuApaisementGain
            apport * System.Math.Pow(1.0 - C.jeuApaisementRevolution, float (max 0 (p.revolution - r))))
    |> max 0.0
    |> min C.jeuSensibilitePlafond

/// Le poids qu'a aujourd'hui un attachement pour un habitant.
let poidsEffectif p (a: Attachement) = a.poids * (1.0 + sensibilite p a.habitant a.jauge)

/// Le ressenti d'un habitant, avec les poids que sa vie dans la partie a donnés à ce qui compte.
let ressentiDans p (deltas: (string * float) list) habitant =
    Jeu.attachements
    |> List.filter (fun a -> a.habitant = habitant)
    |> List.sumBy (fun a -> poidsEffectif p a * (deltas |> List.tryFind (fun (j, _) -> j = a.jauge) |> Option.map snd |> Option.defaultValue 0.0))

/// Ce que le ressenti fait à la confiance : un peu, et seulement quand il est net.
let effetDeRessentis (vecus: Map<string, float>) =
    vecus
    |> Map.toList
    |> List.choose (fun (h, r) ->
        if abs r >= C.jeuRessentiSeuil then Some(h, (if r > 0.0 then C.jeuReputationRessenti else -C.jeuReputationRessenti)) else None)

/// Additionne les effets par habitant.
let cumuler (effets: (string * float) list) =
    effets |> List.groupBy fst |> List.map (fun (h, l) -> h, l |> List.sumBy snd) |> List.filter (fun (_, d) -> d <> 0.0)

/// Ce que les jauges ont bougé entre deux évaluations, en points sur 100 (l'unité affichée au
/// joueur ; le moteur, lui, compte de 0 à 1).
let variations (avant: Moteur.Resultat) (apres: Moteur.Resultat) =
    List.zip avant.jauges apres.jauges |> List.map (fun (x, y) -> x.id, 100.0 * (y.valeur.Milieu - x.valeur.Milieu))

let ajouterReputation (deltas: (string * float) list) (m: Map<string, float>) =
    deltas |> List.fold (fun (m: Map<string, float>) (h, d) -> m.Add(h, (m.TryFind h |> Option.defaultValue 0.0) + d)) m

// ---------- Une décision s'installe : l'inertie ----------

/// Le retard d'une décision : ce qui manque encore de son effet, révolution après révolution.
/// Avec `jeu-inertie-revolutions` = N, une part 1/N agit tout de suite, puis 2/N, etc.
let inertie source (avant: Moteur.Resultat) (apres: Moteur.Resultat) =
    let n = max 1 (int C.jeuInertieRevolutions)

    List.zip avant.jauges apres.jauges
    |> List.choose (fun (a, b) ->
        let d = (b.valeur.Milieu - a.valeur.Milieu) * 100.0

        if abs d < 1e-9 || n = 1 then
            None
        else
            Some
                { source = source
                  jauge = a.id
                  valeurs = [ for k in 1 .. n - 1 -> -d * float (n - k) / float n ] })

// ---------- L'aperçu : ce qu'une décision changerait, avant de la prendre ----------

/// Un lien dont l'effet change avec la décision, et la lentille qui le porte.
type Argument =
    { lien: Lien
      avant: float
      apres: float }

    member a.Ecart = a.apres - a.avant

/// Ce qu'une lentille dit d'une décision : les liens qu'elle porte et qui bougent.
type Avis =
    { lentille: Lentille
      arguments: Argument list }

type Apercu =
    { avant: Moteur.Resultat
      /// la révolution même où la décision est prise (l'effet ne s'est pas encore installé)
      aussitot: Moteur.Resultat
      /// une fois l'effet installé
      aTerme: Moteur.Resultat
      /// l'écart de chaque jauge à terme (milieu de la fourchette pour la Criminalité)
      jauges: (string * float) list
      /// le conseil : un avis par lentille concernée, du plus au moins remué
      conseil: Avis list
      reputation: (string * float) list
      voix: Voix list
      /// pour un pari : le pari, et dans combien de révolutions il agira
      pari: (Pari * int) option }

let lentilleDe = Modele.lentilles |> List.map (fun l -> l.id, l) |> Map.ofList

let conseil (avant: Moteur.Resultat) (apres: Moteur.Resultat) =
    let carte (r: Moteur.Resultat) = r.liens |> List.map (fun l -> l.rang, l.effet) |> Map.ofList
    let a, b = carte avant, carte apres
    let rangs = Set.union (a |> Map.keys |> Set.ofSeq) (b |> Map.keys |> Set.ofSeq)

    rangs
    |> Seq.map (fun i ->
        { lien = Liens.tous[i]
          avant = a.TryFind i |> Option.defaultValue 0.0
          apres = b.TryFind i |> Option.defaultValue 0.0 })
    |> Seq.filter (fun x -> abs x.Ecart > 1e-9)
    |> Seq.groupBy _.lien.lentille
    |> Seq.map (fun (l, args) ->
        { lentille = lentilleDe[l]
          arguments = args |> Seq.sortByDescending (fun x -> abs x.Ecart) |> List.ofSeq })
    |> Seq.sortByDescending (fun av -> av.arguments |> List.sumBy (fun x -> abs x.Ecart))
    |> List.ofSeq

/// Ce que décider ferait : la partie après la décision, sans la journaliser.
let jouerDecision p (s: Situation) choix =
    appliquerChoix s choix p
    |> Result.map (fun e2 ->
        let avant = Moteur.evaluer p.etat
        let aTerme = Moteur.evaluer e2
        let deltas = variations avant aTerme
        // chacun vit la décision avec la sensibilité que la partie lui a donnée jusqu'ici
        let vecus = Jeu.habitants |> List.map (fun h -> h.id, ressentiDans p deltas h.id) |> Map.ofList

        let p2 =
            { p with
                etat = e2
                effets = p.effets @ inertie ("decision:" + s.id) avant aTerme
                ressentis = p.ressentis.Add(s.id, deltas)
                vecus = p.vecus.Add(s.id, vecus)
                reputation = p.reputation |> ajouterReputation (effetReputation s choix) |> ajouterReputation (effetDeRessentis vecus) }
            |> synchroniser

        p2, avant, aTerme)

let apercu p situation (choix: Choix list) =
    match situationDe.TryFind situation with
    | None -> Error(SituationInconnue situation)
    | Some s when not (estDecision s) -> Error(PasUneDecision situation)
    | Some s ->
        jouerDecision p s choix
        |> Result.map (fun (p2, avant, aTerme) ->
            let choisies = choix |> List.map _.option |> Set.ofList

            { avant = avant
              aussitot = Moteur.evaluer p2.etat
              aTerme = aTerme
              jauges = List.zip avant.jauges aTerme.jauges |> List.map (fun (x, y) -> x.id, y.valeur.Milieu - x.valeur.Milieu)
              conseil = conseil avant aTerme
              reputation = cumuler (effetReputation s choix @ effetDeRessentis p2.vecus[s.id])
              voix = Jeu.voix |> List.filter (fun v -> v.situation = s.id && choisies.Contains v.option)
              pari =
                match s.genre, choix with
                | Paris, [ { option = b } ] when pariDe.ContainsKey b -> Some(pariDe[b], pariDe[b].pretDans)
                | _ -> None })

// ---------- Jouer ----------

/// Lire un constat ou le bilan : il est réglé.
let lire p situation =
    match situationDe.TryFind situation with
    | None -> Error(SituationInconnue situation)
    | Some s when estDecision s -> Error(PasUneDecision situation)
    | Some _ when not (estOuverte p situation) -> Error(SituationFermee situation)
    | Some _ ->
        Ok
            { p with
                reglees = p.reglees.Add situation
                journal = Lu(p.revolution, situation) :: p.journal }

/// Prendre une décision : l'état change (l'effet s'installe sur quelques révolutions), la
/// confiance des habitants aussi.
let decider p situation (choix: Choix list) =
    match situationDe.TryFind situation with
    | None -> Error(SituationInconnue situation)
    | Some s when not (estDecision s) -> Error(PasUneDecision situation)
    | Some _ when not (estOuverte p situation) -> Error(SituationFermee situation)
    | Some s ->
        jouerDecision p s choix
        |> Result.map (fun (p2, _, _) ->
            { p2 with
                reglees = p2.reglees.Add situation
                annulable = Some(situation, choix, p.etat, p.effets)
                rouvertes = p2.rouvertes |> List.filter ((<>) situation)
                decisionsCetteRevolution = p2.decisionsCetteRevolution + 1
                journal = Decide(p.revolution, situation, choix) :: p2.journal })

/// L'horizon du modèle (aujourd'hui, 10 ans, 30 ans) qui correspond à une révolution.
let horizonDe revolution =
    if float revolution >= C.horizon30Ans then 2
    elif float revolution >= C.horizon10Ans then 1
    else 0

/// Les conditions d'un événement sont-elles réunies ?
let conditionsReunies p (ev: Evenement) =
    (ev.tranche |> Option.forall (fun t -> trancheCourante p = Some t))
    && not (ev.uneFois && p.evenementsVus.Contains ev.id)
    && Moteur.conditionVraie ev.quand p.etat
    && (ev.siJauge.IsEmpty
        || (let r = Moteur.evaluer p.etat

            ev.siJauge
            |> List.forall (fun (j, c, seuil) ->
                let v = (r.jauges |> List.find (fun x -> x.id = j)).valeur.Milieu

                match c with
                | Inferieur -> v < seuil
                | Superieur -> v > seuil)))

/// Tire les événements de la révolution : chacun dont les conditions sont réunies survient selon
/// sa probabilité, au plus `jeu-evenements-par-revolution`.
let tirerEvenements p =
    let maximum = int C.jeuEvenementsParRevolution

    Jeu.evenements
    |> List.fold
        (fun (p, n) ev ->
            if n >= maximum || not (conditionsReunies p ev) then
                p, n
            else
                let x, graine = tirer p.graine
                let p = { p with graine = graine }
                let survient = if p.hasard then x < ev.probabilite else ev.probabilite >= 1.0

                if not survient then
                    p, n
                else
                    let effets =
                        ev.effets
                        |> List.map (fun f ->
                            { source = "evenement:" + ev.id
                              jauge = f.jauge
                              valeurs = List.replicate f.duree f.points })

                    { p with
                        effets = p.effets @ effets
                        reputation = ajouterReputation ev.reputation p.reputation
                        evenementsVus = p.evenementsVus.Add ev.id
                        journal = Survenu(p.revolution, ev.id) :: p.journal }
                    |> synchroniser,
                    n + 1)
        (p, 0)
    |> fst

/// Finir la révolution : son bilan entre au journal, une année passe, les effets avancent, les
/// paris mûrissent, des événements surviennent. Les constats ouverts sont considérés comme lus ;
/// une décision ouverte doit d'abord être prise.
let finirRevolution p =
    let ouv = ouvertes p

    if ouv |> List.exists estDecision then
        Error RevolutionInachevee
    else
        let r = Moteur.evaluer p.etat
        let actifsAvant = Modele.paris |> List.filter (Moteur.pariActif p.etat) |> List.map _.id |> Set.ofList

        let p =
            { p with
                revolution = p.revolution + 1
                reglees = ouv |> List.fold (fun (acc: Set<string>) s -> acc.Add s.id) p.reglees
                decisionsCetteRevolution = 0
                annulable = None
                effets =
                    p.effets
                    |> List.map (fun f -> { f with valeurs = List.tail f.valeurs })
                    |> List.filter (fun f -> not f.valeurs.IsEmpty)
                journal =
                    Revolu(p.revolution, r.score, r.scoreFrance, r.viable.ok, r.fidelite)
                    :: (ouv |> List.map (fun s -> Lu(p.revolution, s.id)) |> List.rev)
                    @ p.journal }
            |> synchroniser

        let muris =
            Modele.paris
            |> List.filter (fun b -> Moteur.pariActif p.etat b && not (actifsAvant.Contains b.id))
            |> List.map (fun b -> Muri(p.revolution, b.id))

        Ok(tirerEvenements { p with journal = muris @ p.journal })

/// Ce qui est arrivé au début d'une révolution : événements et paris mûris.
let nouvellesDe p revolution =
    p.journal
    |> List.filter (function
        | Survenu(r, _)
        | Muri(r, _) -> r = revolution
        | _ -> false)
    |> List.rev

/// Le joueur propose sa propre idée pendant une scène : elle entre au journal comme une
/// proposition. Elle ne change pas le modèle : une idée n'y pèse qu'après relecture humaine.
let proposerIdee p situation (texte: string) =
    let t = texte.Trim()

    if t = "" then p
    else { p with journal = Idee(p.revolution, situation, t) :: p.journal }

/// Les idées proposées pendant la partie, de la plus ancienne à la plus récente.
let idees p =
    p.journal |> List.choose (function Idee(r, s, t) -> Some(r, s, t) | _ -> None) |> List.rev

/// Le joueur choisit une réplique dans une scène : la confiance des habitants qu'elle touche
/// change, et elle entre au journal.
let repondre p situation (r: Replique) =
    { p with
        reputation = ajouterReputation r.confiance p.reputation
        journal = Repondu(p.revolution, situation, r.id) :: p.journal }

/// Le contexte d'une scène pour cette partie.
let contexteScene p : Scene.Contexte = { graine = p.graine; confiance = p.reputation }

/// Le conseil du mode apprentissage : l'option qui remonte le plus le maillon faible (la jauge
/// vitale la plus loin de son seuil), ou le score quand la société est viable. Un calcul, pas un
/// jugement : les principes, l'égalité et la confiance restent au joueur.
type ConseilLabo =
    { choix: Choix list
      /// la jauge visée ; aucune = le score
      jauge: string option
      gain: float }

let conseilApprentissage p situation =
    match situationDe.TryFind situation with
    | None -> None
    | Some s ->
        let candidats =
            match s.genre with
            | Dilemme ->
                let l = List.exactlyOne s.leviers
                Tables.optionsDe[l] |> List.map (fun o -> [ { levier = l; option = o.id } ])
            | Paris -> s.paris |> List.map (fun b -> [ { levier = "paris"; option = b } ])
            | _ -> []

        let avant = Moteur.evaluer p.etat
        let maillon = if avant.viable.ok then None else Some(fst avant.viable.maillon)

        let mesure (r: Moteur.Resultat) =
            match maillon with
            | Some j -> (r.jauges |> List.find (fun x -> x.id = j)).valeur.Milieu * 100.0
            | None -> r.score

        candidats
        |> List.choose (fun c ->
            match appliquerChoix s c p with
            | Ok e -> Some(c, mesure (Moteur.evaluer e) - mesure avant)
            | Error _ -> None)
        |> List.sortByDescending snd
        |> List.tryHead
        |> Option.map (fun (c, g) -> { choix = c; jauge = maillon; gain = g })

/// L'humeur d'un habitant après une décision : elle découle de ce qu'il avait dit de l'option
/// choisie ou de celle qu'il soutenait. Sans réplique dans la situation, il est neutre.
let humeurDe (s: Situation) (choix: Choix list) habitant =
    let choisies = choix |> List.map _.option |> Set.ofList

    match Jeu.voix |> List.tryFind (fun v -> v.situation = s.id && v.habitant = habitant) with
    | None -> Neutre
    | Some v ->
        match choisies.Contains v.option, v.position with
        | true, Pour -> Approuve
        | true, Contre -> Desapprouve
        | false, Pour -> Decu
        | false, Contre -> Soulage
        | true, Position.Reserve -> Dubitatif
        | false, Position.Reserve -> Neutre

/// Ce que la décision prise dans cette situation fait à la confiance : l'avis de chacun, et son
/// ressenti.
let effetDecision p (s: Situation) (choix: Choix list) =
    cumuler (effetReputation s choix @ effetDeRessentis (p.vecus.TryFind s.id |> Option.defaultValue Map.empty))

/// L'humeur d'un habitant après une décision : son avis s'il en avait un ; sinon, ce que la
/// décision fait à ce qui compte pour lui.
let humeurDans p (s: Situation) (choix: Choix list) habitant =
    match humeurDe s choix habitant, Jeu.voix |> List.exists (fun v -> v.situation = s.id && v.habitant = habitant) with
    | Neutre, false ->
        let r = p.vecus.TryFind s.id |> Option.bind (fun m -> m.TryFind habitant) |> Option.defaultValue 0.0
        if r >= C.jeuRessentiSeuil then Approuve
        elif r <= -C.jeuRessentiSeuil then Desapprouve
        else Neutre
    | h, _ -> h

/// Ce que font les habitants qui ne parlent pas après une décision : un geste selon leur humeur,
/// tiré avec la graine de la partie (rejouable). `exclus` : ceux qui parlent déjà dans la scène.
let attitudes p (s: Situation) (choix: Choix list) (exclus: Set<string>) =
    Jeu.habitants
    |> List.filter (fun h -> not (exclus.Contains h.id))
    |> List.choose (fun h ->
        let humeur = humeurDans p s choix h.id
        let lot = Jeu.attitudes |> List.filter (fun a -> a.habitant = h.id && a.humeur = humeur)

        match lot with
        | [] -> None
        | _ ->
            let cle = s.id + "/" + h.id
            let n = cle |> Seq.fold (fun (n: int64) c -> (n * 31L + int64 c) % 2147483647L) (abs p.graine % 2147483647L)
            Some(h, humeur, lot[int (n % int64 lot.Length)]))

type ErreurRevirement =
    /// seule la dernière décision de la révolution en cours peut être reprise
    | RienAReprendre of situation: string

/// Ce qu'un revirement fait à la confiance : ceux que la décision avait réjouis perdent plus
/// qu'ils n'avaient gagné, ceux qu'elle avait inquiétés regagnent moins qu'ils n'avaient perdu.
let effetRevirement p (s: Situation) (choix: Choix list) =
    effetDecision p s choix
    |> List.map (fun (h, d) ->
        h,
        if d > 0.0 then -d * C.jeuRevirementDeception
        elif d < 0.0 then -d * C.jeuRevirementSoulagement
        else 0.0)

/// Revenir sur la dernière décision de la révolution : le modèle revient à l'état d'avant, la
/// situation se rouvre, la confiance porte la trace du revirement, et le journal aussi.
let revenir p situation =
    match p.annulable with
    | Some(id, choix, etat, effets) when id = situation ->
        let s = situationDe[id]

        { p with
            etat = etat
            effets = effets
            reglees = p.reglees.Remove id
            decisionsCetteRevolution = max 0 (p.decisionsCetteRevolution - 1)
            annulable = None
            reputation = ajouterReputation (effetRevirement p s choix) p.reputation
            ressentis = p.ressentis.Remove id
            vecus = p.vecus.Remove id
            journal = Revenu(p.revolution, id, choix) :: p.journal }
        |> synchroniser
        |> Ok
    | _ -> Error(RienAReprendre situation)

/// Peut-on revenir sur la décision prise dans cette situation ?
let peutRevenir p situation =
    match p.annulable with
    | Some(id, _, _, _) -> id = situation
    | None -> false

/// Combien de fois le joueur est revenu sur ses décisions.
let revirements p =
    p.journal |> List.sumBy (function Revenu _ -> 1 | _ -> 0)

/// Le dernier choix fait dans une situation, s'il y en a un.
let dernierChoix p situation =
    p.journal |> List.tryPick (function Decide(_, s, c) when s = situation -> Some c | _ -> None)

/// Reconsidérer une décision plus ancienne : on ne remonte pas le temps, la situation se
/// présente à nouveau (elle prend la place d'une décision de la révolution), la confiance porte
/// tout de suite la trace du revirement, et le nouveau choix s'appliquera à l'état présent.
let reconsiderer p situation =
    match situationDe.TryFind situation, dernierChoix p situation with
    | Some s, Some choix when estDecision s && not (List.contains situation p.rouvertes) ->
        { p with
            rouvertes = p.rouvertes @ [ situation ]
            reputation = ajouterReputation (effetRevirement p s choix) p.reputation
            ressentis = p.ressentis.Remove situation
            vecus = p.vecus.Remove situation
            annulable = (match p.annulable with Some(id, _, _, _) when id = situation -> None | a -> a)
            journal = Revenu(p.revolution, situation, choix) :: p.journal }
        |> Ok
    | _ -> Error(RienAReprendre situation)

/// Revenir sur une décision, quelle qu'elle soit : la dernière de la révolution se reprend (le
/// modèle revient à l'état d'avant) ; une plus ancienne se reconsidère.
let revenirSur p situation =
    if peutRevenir p situation then revenir p situation else reconsiderer p situation

/// Les décisions prises, de la plus récente à la plus ancienne (une par situation).
let decisionsPrises p =
    p.journal
    |> List.choose (function Decide(r, s, c) -> Some(r, s, c) | _ -> None)
    |> List.distinctBy (fun (_, s, _) -> s)

/// Le prénom d'un habitant dans cette partie.
let prenomDe p habitant =
    match p.prenoms.TryFind habitant with
    | Some n -> n
    | None -> (Jeu.habitants |> List.tryFind (fun h -> h.id = habitant) |> Option.map _.nom |> Option.defaultValue habitant)

/// Remplace les marqueurs `{rôle}` d'un texte par les prénoms de la partie.
let nommer p (texte: string) =
    System.Text.RegularExpressions.Regex.Replace(texte, "\\{([a-z-]+)\\}", fun m -> prenomDe p m.Groups[1].Value)
