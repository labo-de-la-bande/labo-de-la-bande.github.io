/// `labo sensibilite` : ce qu'une conclusion du modèle doit à chaque lien. Pour chaque façon de
/// jouer de `labo explorer`, à la fin du parcours et à 30 ans, on retire chaque lien actif, un par
/// un, et on regarde si le bilan bascule : la société tient-elle debout, fait-elle mieux que la
/// France ? Un bilan qui bascule quand on retire un seul jugement ne vaut pas plus que ce jugement.
///
/// Limite : l'état est celui de la partie jouée avec tous les liens ; sans un lien, une façon de
/// jouer qui vise le score aurait pu choisir autrement. On mesure la conclusion, pas le chemin.
module Labo.Jouer.Sensibilite

open System
open Labo.Data
open Labo.Engine
open Labo.Jeu
open Labo.Jeu.Partie
open Labo.Jouer.Explorer

module C = Labo.Data.Coefficients

/// Le bilan du modèle, sans verdict de jeu : tient-elle debout (sinon, quelle jauge lâche), et
/// fait-elle mieux que la France ?
type Bilan =
    { viable: bool
      maillon: string option
      mieux: bool }

let bilan (r: Moteur.Resultat) =
    { viable = r.viable.ok
      maillon = (if r.viable.ok then None else Some(fst r.viable.maillon))
      mieux = r.score > r.scoreFrance }

let decrireBilan (b: Bilan) (r: Moteur.Resultat) =
    let tient =
        match b.maillon with
        | None -> "tient debout"
        | Some j -> $"ne tient pas ({libelleJauge j})"

    let france = if b.mieux then "devant la France" else "derrière la France"
    $"{tient}, {france} ({signe (r.score - r.scoreFrance)})"

/// Ce qui change dans le bilan quand on retire un lien.
let bascules (avant: Bilan) (apres: Bilan) =
    [ match avant.maillon, apres.maillon with
      | None, Some j -> yield $"ne tient plus ({libelleJauge j})"
      | Some _, None -> yield "tient debout"
      | Some a, Some b when a <> b -> yield $"lâche ailleurs ({libelleJauge b} au lieu de {libelleJauge a})"
      | _ -> ()
      if avant.mieux && not apres.mieux then yield "passe derrière la France"
      if not avant.mieux && apres.mieux then yield "passe devant la France" ]

let statut =
    function
    | Sourcé -> "sourcé"
    | Hypothèse -> "hypothèse"
    | Jugement -> "jugement"
    | Témoignage -> "témoignage"
    | DeMémoire -> "de mémoire"

/// Les liens dont le retrait, seul, fait basculer le bilan d'un état.
let fragiles (e: Etat) =
    let r = Moteur.evaluer e
    let b = bilan r

    let liste =
        r.liens
        |> List.choose (fun a ->
            let r2 = Moteur.evaluerSans (Set.singleton a.rang) e

            match bascules b (bilan r2) with
            | [] -> None
            | bs -> Some(a, bs, r2.score - r.score))

    r, b, liste

/// Des groupes de liens à retirer d'un coup : un biais vient plutôt d'un ensemble de liens qui
/// penchent du même côté que d'un lien seul. Par statut, par lentille, et ceux que l'assistant a
/// proposés.
let groupes =
    let rangs f = Liens.tous |> List.indexed |> List.filter (snd >> f) |> List.map fst |> Set.ofList
    let nomLentille id = Modele.lentilles |> List.tryFind (fun l -> l.id = id) |> Option.map _.nom |> Option.defaultValue id

    [ for st in [ Jugement; Hypothèse; DeMémoire ] do
          yield $"tous les liens « {statut st} »", rangs (fun l -> l.statut = st)
      yield "tous les liens proposés par l'assistant", rangs (fun l -> l.lentillePar = "assistant")
      for id in Liens.tous |> List.map _.lentille |> List.distinct |> List.sort do
          yield $"la lentille « {nomLentille id} »", rangs (fun l -> l.lentille = id) ]
    |> List.filter (fun (_, r) -> not r.IsEmpty)

let executer (args: string list) =
    let hasard =
        match args |> List.skipWhile ((<>) "--hasard") with
        | _ :: n :: _ -> Some(int64 n)
        | _ -> None

    printfn "Ce qu'une conclusion du modèle doit à chaque lien : on retire chaque lien actif, un par un."
    printfn "Un bilan qui bascule sans un seul jugement ne vaut pas plus que ce jugement."
    printfn "Limite : l'état reste celui de la partie jouée avec tous les liens (on mesure la conclusion, pas le chemin)."

    // tous les liens qui font basculer au moins un bilan, et combien de fois
    let compte = Collections.Generic.Dictionary<string, Lien * int>()
    let mutable bilans = 0
    let etats = ResizeArray<string * Etat>()
    let mutable stables = 0

    for st in strategies do
        let config: Journal.Configuration =
            { region = "france"; graine = defaultArg hasard 1L; hasard = hasard.IsSome; parcours = parcoursComplet }

        match Journal.commencer config |> Result.bind (fun p -> jouerParcours st p []) with
        | Error e -> eprintfn "%s : %s" st.id e
        | Ok(fin, _) ->
            printfn "\n%s : %s" st.id st.description

            let rec vivre (p: Partie) =
                if p.revolution >= int C.horizon30Ans then p
                else match finirRevolution p with Ok q -> vivre q | Error _ -> p

            for (titre, p) in [ $"fin du parcours (rév. {fin.revolution})", fin; $"30 ans (rév. {int C.horizon30Ans})", vivre fin ] do
                etats.Add(st.id + ", " + titre, p.etat)
                let r, b, liste = fragiles p.etat
                bilans <- bilans + 1
                printfn "  %s : %s ; %d liens actifs" titre (decrireBilan b r) r.liens.Length

                match liste with
                | [] ->
                    stables <- stables + 1
                    printfn "    aucun lien seul ne fait basculer ce bilan"
                | l ->
                    for (a, bs, d) in l do
                        let lien = a.lien
                        printfn "    sans %s (%s, %s, %s) : %s ; score %s" lien.id (statut lien.statut) lien.lentille (lien.source |> Option.defaultValue "sans source") (String.concat ", " bs) (signe d)

                        match compte.TryGetValue lien.id with
                        | true, (x, n) -> compte[lien.id] <- (x, n + 1)
                        | _ -> compte[lien.id] <- (lien, 1)

    printfn "\nEn tout : %d bilans sur %d ne tiennent à aucun lien seul." stables bilans

    // les groupes : chaque bilan, sans tout un groupe de liens
    printfn "\nSans tout un groupe de liens :"

    for (nom, rangs) in groupes do
        let changes =
            [ for (quoi, e) in etats do
                  let b = bilan (Moteur.evaluer e)
                  let r2 = Moteur.evaluerSans rangs e

                  match bascules b (bilan r2) with
                  | [] -> ()
                  | bs ->
                      let texte = String.concat ", " bs
                      yield $"{quoi} : {texte}" ]

        // l'ampleur : l'écart avec la France, avec et sans le groupe
        let ecarts =
            [ for (_, e) in etats do
                  let r, r2 = Moteur.evaluer e, Moteur.evaluerSans rangs e
                  yield (r2.score - r2.scoreFrance) - (r.score - r.scoreFrance) ]

        let ampleur = $"écart avec la France de {signe (List.min ecarts)} à {signe (List.max ecarts)} points"
        printfn "  %s (%d liens) : %s ; %s" nom rangs.Count (if changes.IsEmpty then "aucun bilan ne bascule" else $"{changes.Length} bilan(s) basculent") ampleur

        for c in changes do
            printfn "      %s" c

    if compte.Count > 0 then
        printfn "Les liens dont une conclusion dépend à elle seule (%d) :" compte.Count

        for KeyValue(id, (l, n)) in compte |> Seq.sortByDescending (fun (KeyValue(_, (_, n))) -> n) do
            printfn "  %-40s %d bilan(s) · %s · %s" id n (statut l.statut) (if l.lentillePar = "assistant" then "proposé par l'assistant" else "par " + l.lentillePar)

        let parStatut =
            compte.Values |> Seq.countBy (fun (l, _) -> statut l.statut) |> Seq.map (fun (s, n) -> $"{n} {s}") |> String.concat ", "

        printfn "Par statut : %s." parStatut

    0
