/// `labo explorer` : jouer tout le parcours selon une façon de choisir, puis laisser passer les
/// années jusqu'à l'horizon de 30 ans, pour voir où le modèle pousse. Une mesure du modèle, pas un
/// conseil : les liens sont pour la plupart des jugements et des hypothèses.
module Labo.Jouer.Explorer

open System
open Labo.Data
open Labo.Engine
open Labo.Jeu
open Labo.Jeu.Partie

module C = Labo.Data.Coefficients

type Strategie =
    { id: string
      description: string
      /// la mesure à maximiser, à terme ; aucune = la première option partout
      mesure: (Moteur.Resultat -> float) option
      /// le conseil du mode apprentissage, quand il en donne un
      apprentissage: bool
      /// garder le moins possible sur la liste, puis le meilleur score à terme
      parListe: bool }

let strategies =
    [ { id = "premier"; description = "la première option partout"; mesure = None; apprentissage = false; parListe = false }
      { id = "score"; description = "l'option au meilleur score à terme"; mesure = Some _.score; apprentissage = false; parListe = false }
      { id = "fidelite"; description = "l'option la plus fidèle au but, à terme"; mesure = Some _.fidelite; apprentissage = false; parListe = false }
      { id = "apprentissage"
        description = "le conseil du mode apprentissage (le maillon faible, puis le score), sinon la première option"
        mesure = None
        apprentissage = true
        parListe = false }
      { id = "liste"
        description = "garder le moins possible sur la liste, puis le meilleur score à terme"
        mesure = Some _.score
        apprentissage = false
        parListe = true } ]

// ---------- Ce que la société garde (data/jeu/abolitions.csv) ----------

/// La France d'aujourd'hui, d'après le constat de l'auteur du 26 novembre 2026 (CHANTIERS.md) :
/// pas un état du moteur, seulement pour comparer. La prison n'est l'option d'aucun dilemme.
let listeFrance =
    [ "une monnaie sans plafond"
      "une police contre la délinquance"
      "une police contre les violences dans le couple"
      "une vidéosurveillance algorithmique (loi de 2023)"
      "des prisons"
      "une armée"
      "un État centralisé" ]

let libelleListe (l: string list) = if l.IsEmpty then "rien" else String.concat ", " l

let fr = Globalization.CultureInfo "fr-FR"
let nombre (x: float) = x.ToString("F1", fr)
let signe (x: float) = if x >= 0.0 then "+" + nombre x else "−" + nombre (abs x)

/// Toutes les réponses possibles à une situation : chaque option d'un dilemme, chaque
/// combinaison de moyens de prévention, aucun pari ou un seul.
let candidats (s: Situation) =
    match s.genre with
    | Paris -> [ { levier = "paris"; option = "aucun" } ] :: (s.paris |> List.map (fun b -> [ { levier = "paris"; option = b } ]))
    | Prevention ->
        s.leviers
        |> List.fold
            (fun acc l -> acc |> List.collect (fun c -> [ c @ [ { levier = l; option = "oui" } ]; c @ [ { levier = l; option = "non" } ] ]))
            [ [] ]
    | _ -> s.leviers |> List.collect (fun l -> Options.optionsDe[l] |> List.map (fun o -> [ { levier = l; option = o.id } ]))

let choisir (st: Strategie) p (s: Situation) =
    let cs = candidats s
    let premier = List.head cs

    if st.apprentissage then
        match conseilApprentissage p s.id with
        | Some c when c.gain > 0.0005 -> c.choix
        | _ -> premier
    else
        match st.mesure with
        | None -> premier
        | Some f ->
            // à égalité, la première option : le résultat ne dépend pas de l'ordre de calcul
            // par la liste : chaque élément gardé compte plus que tout écart de score (au plus 100)
            let gardes c =
                if st.parListe then
                    match decider p s.id c with
                    | Ok p2 -> float (garde p2.etat).Length * 1000.0
                    | Error _ -> infinity
                else 0.0

            cs
            |> List.mapi (fun i c -> i, c, (match apercu p s.id c with Ok a -> f a.aTerme - gardes c | Error _ -> -infinity))
            |> List.sortBy (fun (i, _, v) -> -v, i)
            |> List.head
            |> fun (_, c, _) -> c

let rec jouerParcours st p choix =
    match ouvertes p |> List.tryFind estDecision with
    | Some s ->
        let c = choisir st p s

        match decider p s.id c with
        | Ok p2 -> jouerParcours st p2 ((s, c) :: choix)
        | Error e -> Error(decrire e)
    | None when terminee p -> Ok(p, List.rev choix)
    | None ->
        match finirRevolution p with
        | Ok p2 -> jouerParcours st p2 choix
        | Error e -> Error(decrire e)

let libelleJauge id = (Jauges.tous |> List.find (fun j -> j.id = id)).libelle

/// L'issue de la partie, en une ligne : gagné, ou perdu et pourquoi.
let libelleIssue p =
    match issue p with
    | Gagnee -> "GAGNÉ : la société tient debout, garde ses principes et fait mieux que la France."
    | Perdue raisons ->
        raisons
        |> List.map (function
            | PasViable(j, _) -> $"{libelleJauge j} sous son seuil vital"
            | InterditsGardes l -> "la société garde ce que la partie interdit : " + (l |> List.map _.libelle |> String.concat ", ")
            | PasMieuxQueLaFrance(s, f) -> $"pas mieux que la France ({nombre s} contre {nombre f})")
        |> String.concat " ; "
        |> sprintf "PERDU : %s."

let libelleChoix (c: Choix list) =
    c
    |> List.map (fun ch ->
        if ch.levier = "paris" then
            match Modele.paris |> List.tryFind (fun b -> b.id = ch.option) with
            | Some b -> b.titre
            | None -> "aucun pari"
        else
            let o = Options.optionsDe[ch.levier] |> List.find (fun o -> o.id = ch.option)
            if o.id = "oui" || o.id = "non" then $"{ch.levier} : {o.id}" else o.libelle)
    |> String.concat " ; "

/// Une ligne du tableau : où en est la cellule face à la France, à une révolution donnée.
let ligne titre (p: Partie) =
    let r = Moteur.evaluer p.etat

    let sous =
        r.jauges
        |> List.filter (fun j -> j.valeur.Milieu < Moteur.valeurFrance j.id p.etat)
        |> List.map (fun j -> libelleJauge j.id)

    let viable =
        if r.viable.ok then "viable" else $"pas viable ({libelleJauge (fst r.viable.maillon)} sous son seuil)"

    printfn "  %-26s score %s (France %s, %s) · fidélité %s (France %s) · %s" titre (nombre r.score) (nombre r.scoreFrance) (signe (r.score - r.scoreFrance)) (nombre r.fidelite) (nombre r.fideliteFrance) viable
    printfn "  %-26s sous la France : %s" "" (if sous.IsEmpty then "aucune jauge" else String.Join(", ", sous))

let explorer (st: Strategie) graine hasard afficherChoix =
    let config: Journal.Configuration = { region = "france"; graine = graine; hasard = hasard; parcours = parcoursComplet }

    match Journal.commencer config |> Result.bind (fun p -> jouerParcours st p []) with
    | Error e ->
        eprintfn "%s : %s" st.id e
        false
    | Ok(fin, choix) ->
        printfn "\n%s : %s" st.id st.description

        if afficherChoix then
            for (s, c) in choix do
                printfn "    %-40s %s" s.titre (libelleChoix c)

        // les années passent sans nouvelle décision, jusqu'à l'horizon de 30 ans
        let derniere = max (fin.revolution + 1) (int C.horizon30Ans)

        let rec vivre (p: Partie) acc =
            if p.revolution >= derniere then List.rev (p :: acc)
            else match finirRevolution p with Ok q -> vivre q (p :: acc) | Error _ -> List.rev (p :: acc)

        let annees = vivre fin []
        let a n = annees |> List.find (fun p -> p.revolution >= n)
        ligne $"fin du parcours (rév. {fin.revolution})" fin
        ligne $"10 ans (rév. {int C.horizon10Ans})" (a (int C.horizon10Ans))
        ligne $"30 ans (rév. {int C.horizon30Ans})" (a (int C.horizon30Ans))
        printfn "  Fin du parcours : %s" (libelleIssue fin)
        let l = garde fin.etat
        printfn "  Ce qu'elle garde (%d) : %s" l.Length (libelleListe (l |> List.map (fun a -> $"{a.libelle} ({a.famille}, {a.palier})")))

        let depasse = annees |> List.tryFind (fun p -> let r = Moteur.evaluer p.etat in r.score > r.scoreFrance)

        match depasse with
        | Some p when p.revolution = fin.revolution -> printfn "  Mieux que la France dès la fin du parcours."
        | Some p -> printfn "  Mieux que la France à partir de la révolution %d." p.revolution
        | None -> printfn "  Pas mieux que la France d'ici 30 ans."

        true

let usage =
    """labo explorer [stratégie] [--choix] [--hasard N]

  Joue tout le parcours selon une façon de choisir, puis laisse passer les années jusqu'à 30 ans :
  score, fidélité et viabilité face à la France, à la fin du parcours, à 10 ans et à 30 ans.
  Sans stratégie : toutes. --choix : les choix faits. --hasard N : avec les événements tirés au
  hasard (graine N) ; sans, seuls les événements certains arrivent, et le résultat est toujours le même.

  Stratégies :
"""

let executer (args: string list) =
    let hasard =
        match args |> List.skipWhile ((<>) "--hasard") with
        | _ :: n :: _ -> Some(int64 n)
        | _ -> None

    let noms = args |> List.filter (fun a -> not (a.StartsWith "--") && not (Seq.forall Char.IsDigit a))

    let choisies =
        match noms with
        | [] -> Ok strategies
        | ns ->
            ns
            |> List.map (fun n -> match strategies |> List.tryFind (fun s -> s.id = n) with Some s -> Ok s | None -> Error n)
            |> List.fold (fun acc r -> match acc, r with Ok l, Ok s -> Ok(l @ [ s ]) | Error e, _ | _, Error e -> Error e) (Ok [])

    match choisies with
    | Error inconnue ->
        eprintfn "stratégie inconnue : %s\n" inconnue
        eprintf "%s" usage
        for s in strategies do eprintfn "    %-14s %s" s.id s.description
        2
    | Ok sts ->
        printfn "Une mesure du modèle, pas un conseil : ses liens sont pour la plupart des jugements et des hypothèses."
        printfn "Une révolution compte pour un an ; le modèle ne bouge qu'aux horizons (aujourd'hui, 10 ans, 30 ans)."
        let afficherChoix = List.contains "--choix" args || sts.Length = 1
        printfn "\nLa France garde (%d) : %s" listeFrance.Length (libelleListe listeFrance)
        let ok = sts |> List.map (fun st -> explorer st (defaultArg hasard 1L) hasard.IsSome afficherChoix)
        if List.forall id ok then 0 else 1
