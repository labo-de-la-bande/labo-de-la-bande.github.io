/// Une partie : des révolutions solaires, quelques situations à chacune, des décisions, le
/// conseil des lentilles et la réputation auprès des habitants. Pur : chaque fonction rend une
/// nouvelle partie, rien n'est modifié en place. Voir docs/bible.md.
module Labo.Jeu.Partie

open Labo.Data
open Labo.Engine

module C = Labo.Data.Coefficients

/// Un choix : une option d'un levier.
type Choix = { levier: string; option: string }

type Evenement =
    /// un constat lu
    | Lu of revolution: int * situation: string
    | Decide of revolution: int * situation: string * choix: Choix list
    /// le bilan d'une révolution qui s'achève
    | Revolu of revolution: int * score: float * scoreFrance: float * viable: bool * fidelite: float

type Partie =
    { /// révolutions solaires écoulées depuis le début (0 = l'année en cours)
      revolution: int
      tranche: string
      /// l'état du modèle
      etat: Etat
      /// confiance de chaque habitant
      reputation: Map<string, float>
      /// situations réglées
      reglees: Set<string>
      /// décisions prises pendant la révolution en cours
      decisionsCetteRevolution: int
      /// du plus récent au plus ancien
      journal: Evenement list }

type ErreurJeu =
    | TrancheInconnue of tranche: string
    | SituationInconnue of situation: string
    /// la situation n'est pas proposée en ce moment (déjà réglée, pas encore venue, ou condition fausse)
    | SituationFermee of situation: string
    | PasUneDecision of situation: string
    /// les choix ne correspondent pas aux leviers de la situation (un par levier)
    | ChoixInvalides of situation: string
    | OptionRefusee of Options.ErreurOption
    | RevolutionInachevee

let decrire =
    function
    | TrancheInconnue t -> $"tranche inconnue : {t}"
    | SituationInconnue s -> $"situation inconnue : {s}"
    | SituationFermee s -> $"la situation {s} n'est pas proposée en ce moment"
    | PasUneDecision s -> $"la situation {s} ne demande pas de décision"
    | ChoixInvalides s -> $"il faut un choix par levier de la situation {s}"
    | OptionRefusee(Options.LevierInconnu l) -> $"levier inconnu : {l}"
    | OptionRefusee(Options.OptionInconnue(l, o)) -> $"option inconnue : {l}={o}"
    | RevolutionInachevee -> "des décisions attendent encore cette révolution"

let situationDe = Jeu.situations |> List.map (fun s -> s.id, s) |> Map.ofList

let nouvelle tranche =
    if Jeu.tranches |> List.exists (fun t -> t.id = tranche) then
        Ok
            { revolution = 0
              tranche = tranche
              etat = Tables.etatDeDepart
              reputation = Jeu.habitants |> List.map (fun h -> h.id, 0.0) |> Map.ofList
              reglees = Set.empty
              decisionsCetteRevolution = 0
              journal = [] }
    else
        Error(TrancheInconnue tranche)

let estDecision (s: Situation) = s.genre = Dilemme || s.genre = Prevention

/// Les situations qui restent à régler dans la tranche, dans l'ordre du récit.
let enAttente p =
    Jeu.situations
    |> List.filter (fun s -> s.tranche = p.tranche && not (p.reglees.Contains s.id) && Moteur.conditionVraie s.quand p.etat)
    |> List.sortBy _.rang

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

/// La partie est finie quand toute la tranche est réglée.
let terminee p = enAttente p |> List.isEmpty

/// Les choix sont valides : un par levier de la situation, chacun applicable.
let appliquerChoix (s: Situation) (choix: Choix list) (e: Etat) =
    let leviers = choix |> List.map _.levier

    if List.sort leviers <> List.sort s.leviers then
        Error(ChoixInvalides s.id)
    else
        choix
        |> List.fold
            (fun acc c -> acc |> Result.bind (fun e -> Options.appliquer c.levier c.option e |> Result.mapError OptionRefusee))
            (Ok e)

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
      apres: Moteur.Resultat
      /// l'écart de chaque jauge (milieu de la fourchette pour la Criminalité)
      jauges: (string * float) list
      /// le conseil : un avis par lentille concernée, du plus au moins remué
      conseil: Avis list
      reputation: (string * float) list
      voix: Voix list }

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

let apercu p situation (choix: Choix list) =
    match situationDe.TryFind situation with
    | None -> Error(SituationInconnue situation)
    | Some s when not (estDecision s) -> Error(PasUneDecision situation)
    | Some s ->
        appliquerChoix s choix p.etat
        |> Result.map (fun e2 ->
            let avant, apres = Moteur.evaluer p.etat, Moteur.evaluer e2

            let jauges =
                List.zip avant.jauges apres.jauges
                |> List.map (fun (x, y) -> x.id, y.valeur.Milieu - x.valeur.Milieu)

            let choisies = choix |> List.map _.option |> Set.ofList

            { avant = avant
              apres = apres
              jauges = jauges
              conseil = conseil avant apres
              reputation = effetReputation s choix
              voix = Jeu.voix |> List.filter (fun v -> v.situation = s.id && choisies.Contains v.option) })

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

/// Prendre une décision : l'état change, la confiance des habitants aussi.
let decider p situation (choix: Choix list) =
    match situationDe.TryFind situation with
    | None -> Error(SituationInconnue situation)
    | Some s when not (estDecision s) -> Error(PasUneDecision situation)
    | Some _ when not (estOuverte p situation) -> Error(SituationFermee situation)
    | Some s ->
        appliquerChoix s choix p.etat
        |> Result.map (fun e2 ->
            let reputation =
                effetReputation s choix
                |> List.fold (fun (m: Map<string, float>) (h, d) -> m.Add(h, (m.TryFind h |> Option.defaultValue 0.0) + d)) p.reputation

            { p with
                etat = e2
                reputation = reputation
                reglees = p.reglees.Add situation
                decisionsCetteRevolution = p.decisionsCetteRevolution + 1
                journal = Decide(p.revolution, situation, choix) :: p.journal })

/// L'horizon du modèle (aujourd'hui, 10 ans, 30 ans) qui correspond à une révolution.
let horizonDe revolution =
    if float revolution >= C.horizon30Ans then 2
    elif float revolution >= C.horizon10Ans then 1
    else 0

/// Finir la révolution : son bilan entre au journal, une année passe. Les constats ouverts sont
/// considérés comme lus ; une décision ouverte doit d'abord être prise.
let finirRevolution p =
    let ouv = ouvertes p

    if ouv |> List.exists estDecision then
        Error RevolutionInachevee
    else
        let r = Moteur.evaluer p.etat
        let suivante = p.revolution + 1
        let hz = horizonDe suivante
        let etat = if hz > 0 then { p.etat with c = p.etat.c.Add("hz", Num(float hz)) } else p.etat

        let lus = ouv

        Ok
            { p with
                revolution = suivante
                etat = etat
                reglees = lus |> List.fold (fun (acc: Set<string>) s -> acc.Add s.id) p.reglees
                decisionsCetteRevolution = 0
                journal =
                    Revolu(p.revolution, r.score, r.scoreFrance, r.viable.ok, r.fidelite)
                    :: (lus |> List.map (fun s -> Lu(p.revolution, s.id)) |> List.rev)
                    @ p.journal }
