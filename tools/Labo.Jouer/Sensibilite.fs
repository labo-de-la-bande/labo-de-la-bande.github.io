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
    | ADocumenter -> "à documenter"

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

// ---------- Les réglages du modèle de base, à ±20 % ----------

module F = Labo.Data.Pays.Fr

/// Une valeur du modèle de base qu'on fait varier : de quoi la lire et l'écrire.
type Reglage =
    { nom: string
      genre: string
      statut: string
      lire: unit -> float
      ecrire: float -> unit }

/// Les coefficients, les constantes de la France, les seuils vitaux, les valeurs de la France.
let reglages () =
    let statutDe ids id = ids |> List.tryFind (fst >> (=) id) |> Option.map snd |> Option.defaultValue "?"
    let statutsC = C.tous |> List.map (fun c -> c.id, statut c.statut)
    let statutsF = F.constantes |> List.map (fun c -> c.id, statut c.statut)
    let unites = F.constantes |> List.map (fun c -> c.id, c.unite) |> Map.ofList

    [ for (id, lire, ecrire) in C.reglages do
          yield { nom = id; genre = "coefficient"; statut = statutDe statutsC id; lire = lire; ecrire = ecrire }
      // une année de départ ne varie pas de 20 %
      for (id, lire, ecrire) in F.reglages do
          if unites.TryFind id <> Some "année" then
              yield { nom = id; genre = "constante de la France"; statut = statutDe statutsF id; lire = lire; ecrire = ecrire }
      for j in Jauges.tous do
          match j.seuilVital with
          | Some _ ->
              let id = j.id
              yield
                  { nom = $"seuil vital de {libelleJauge id}"
                    genre = "seuil vital"
                    statut = "jugement"
                    lire = (fun () -> (Jauges.tous |> List.find (fun x -> x.id = id)).seuilVital.Value)
                    ecrire = (fun v -> Jauges.tous <- Jauges.tous |> List.map (fun x -> if x.id = id then { x with seuilVital = Some v } else x)) }
          | None -> ()
      for v in F.jauges do
          match v.valeur with
          | Some _ ->
              let id = v.jauge
              yield
                  { nom = $"valeur de la France pour {libelleJauge id}"
                    genre = "valeur de la France"
                    statut = "d'après data/pays/fr/jauges.csv"
                    lire = (fun () -> (F.jauges |> List.find (fun x -> x.jauge = id)).valeur.Value)
                    ecrire = (fun x -> F.jauges <- F.jauges |> List.map (fun y -> if y.jauge = id then { y with valeur = Some(min 1.0 x) } else y)) }
          | None -> () ]

/// Pour chaque réglage, à −20 % puis à +20 %, les bilans qui basculent ; la valeur est remise après.
let varier (etats: (string * Etat) list) =
    let avant = etats |> List.map (fun (q, e) -> q, bilan (Moteur.evaluer e))

    [ for r in reglages () do
          for f in [ 0.8; 1.2 ] do
              let v0 = r.lire ()

              let changes =
                  try
                      r.ecrire (v0 * f)

                      [ for ((q, e), (_, b)) in List.zip etats avant do
                            match bascules b (bilan (Moteur.evaluer e)) with
                            | [] -> ()
                            | bs ->
                                let texte = String.concat ", " bs
                                yield $"{q} : {texte}" ]
                  finally
                      r.ecrire v0

              if not changes.IsEmpty then yield r, f, v0, changes ]

/// Les verdicts du stress test sont des jugements, pas des nombres : chaque attaque, d'un cran plus
/// solide puis d'un cran plus fragile (casse ou ouvert < fissure < tient), quand les choix ne la
/// fixent pas. Le verdict est remis après.
let varierVerdicts (etats: (string * Etat) list) =
    let avant = etats |> List.map (fun (q, e) -> q, bilan (Moteur.evaluer e))
    let plus = function "tient" -> None | "fissure" -> Some "tient" | _ -> Some "fissure"
    let moins = function "tient" -> Some "fissure" | "fissure" -> Some "casse" | _ -> None

    [ for a in Modele.attaques do
          for (sens, f) in [ "plus solide", plus; "plus fragile", moins ] do
              match f a.verdict with
              | None -> ()
              | Some v ->
                  let tous = Modele.attaques

                  let changes =
                      try
                          Modele.attaques <- tous |> List.map (fun x -> if x.id = a.id then { x with verdict = v } else x)

                          [ for ((q, e), (_, b)) in List.zip etats avant do
                                match bascules b (bilan (Moteur.evaluer e)) with
                                | [] -> ()
                                | bs ->
                                    let texte = String.concat ", " bs
                                    yield $"{q} : {texte}" ]
                      finally
                          Modele.attaques <- tous

                  yield a, sens, a.verdict, v, changes ]

/// Tout le stress test d'un cran à la fois : plus pessimiste, puis plus optimiste. Rend, pour chaque
/// sens, les bilans qui basculent et l'écart avec la France le plus bas.
let stressEntier (etats: (string * Etat) list) =
    let avant = etats |> List.map (fun (q, e) -> q, bilan (Moteur.evaluer e))
    let plus = function "tient" -> "tient" | "fissure" -> "tient" | _ -> "fissure"
    let moins = function "tient" -> "fissure" | "fissure" -> "casse" | v -> v

    [ for (sens, f) in [ "toutes plus fragiles", moins; "toutes plus solides", plus ] do
          let tous = Modele.attaques

          let resultat =
              try
                  // la France passe le même test : ses verdicts bougent du même cran
                  Modele.attaques <- tous |> List.map (fun x -> { x with verdict = f x.verdict; verdictFrance = f x.verdictFrance })

                  [ for ((q, e), (_, b)) in List.zip etats avant do
                        let r = Moteur.evaluer e
                        yield q, bascules b (bilan r), r.score - r.scoreFrance ]
              finally
                  Modele.attaques <- tous

          yield sens, resultat ]

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

    // les seuils vitaux : la valeur la plus basse atteinte, face au seuil (une marge étroite rend
    // le bilan fragile, quel que soit le réglage qui la ferme)
    printfn "\nLes seuils vitaux : la valeur la plus basse atteinte dans ces parties, face au seuil (un jugement)."

    for j in Jauges.tous do
        match j.seuilVital with
        | Some seuil ->
            let valeurs =
                [ for (q, e) in etats do
                      let r = Moteur.evaluer e
                      yield q, (r.jauges |> List.find (fun x -> x.id = j.id)).valeur.Milieu ]

            let q, v = valeurs |> List.minBy snd
            let enTenant = valeurs |> List.filter (fun (q, _) -> not (q.StartsWith "premier")) |> List.map snd
            let minTenant = if enTenant.IsEmpty then v else List.min enTenant
            printfn "  %-14s seuil %s %%, au plus bas %s %% (%s) ; hors « premier » : %s %%" j.libelle (nombre (seuil * 100.0)) (nombre (v * 100.0)) q (nombre (minTenant * 100.0))
        | None -> ()

    // le stress test : chaque verdict d'un cran
    printfn "\nLe stress test, chaque verdict d'un cran (la Sécurité vaut la moyenne : tient 1, fissure %s, casse ou ouvert 0) :" (nombre C.securiteFissure)

    for (a, sens, v0, v, changes) in varierVerdicts (List.ofSeq etats) do
        printfn "  %-45s %s → %s : %s" a.attaque v0 v (if changes.IsEmpty then "aucun bilan ne bascule" else $"{changes.Length} bilan(s)")

        for c in changes do
            printfn "      %s" c

    for (sens, resultat) in stressEntier (List.ofSeq etats) do
        let basculent = resultat |> List.filter (fun (_, bs, _) -> not bs.IsEmpty)
        let _, _, pire = resultat |> List.minBy (fun (_, _, d) -> d)
        printfn "  %-45s %d bilan(s) basculent ; écart avec la France au plus bas %s" ("Toutes les attaques, " + sens) basculent.Length (signe pire)

        for (q, bs, _) in basculent do
            printfn "      %s : %s" q (String.concat ", " bs)

    // le modèle de base : chaque réglage à ±20 %
    let variations = varier (List.ofSeq etats)
    let n = (reglages ()).Length
    printfn "\nLe modèle de base, chaque valeur à −20 %% puis à +20 %% (%d valeurs) : %d variation(s) font basculer au moins un bilan." n variations.Length

    for (r, f, v0, changes) in variations do
        // un seuil vital au-dessus de 1 ne peut être atteint par aucune jauge : un artefact du ±20 %
        let horsDAtteinte = if r.genre = "seuil vital" && v0 * f >= 1.0 then " ; seuil hors d'atteinte (au-dessus de 100 %), un artefact du ±20 %" else ""
        printfn "  %s %s (%s, %s) : %d bilan(s)%s" r.nom (if f < 1.0 then "−20 %" else "+20 %") r.genre r.statut changes.Length horsDAtteinte

        for c in changes do
            printfn "      %s" c

    if not variations.IsEmpty then
        let parGenre = variations |> List.countBy (fun (r, _, _, _) -> r.genre) |> List.map (fun (g, k) -> $"{k} {g}") |> String.concat ", "
        printfn "Par genre : %s." parGenre

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
