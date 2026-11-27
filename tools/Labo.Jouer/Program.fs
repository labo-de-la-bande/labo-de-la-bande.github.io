/// Le jeu en mode texte : une tranche, révolution après révolution, au clavier.
/// Une maquette de la boucle de jeu (docs/bible.md), avant l'interface graphique.
module Labo.Jouer.Program

open System
open System.Text.RegularExpressions
open Labo.Data
open Labo.Engine
open Labo.Jeu
open Labo.Jeu.Partie

/// Les marqueurs de source `[réf:id]` ne s'affichent pas en mode texte.
let propre (t: string) = Regex.Replace(t, "\\s*\\[réf:[^\\]]*\\]", "")
let libelleJauge id = (Jauges.tous |> List.find (fun j -> j.id = id)).libelle
let mutable prenoms: Map<string, string> = Map.empty

let nomHabitant id =
    match prenoms.TryFind id with
    | Some n -> n
    | None -> (Jeu.habitants |> List.find (fun h -> h.id = id)).nom
let fr = Globalization.CultureInfo "fr-FR"
let nombre (x: float) = x.ToString("F1", fr)
let signe (x: float) = if x >= 0.0 then "+" + nombre x else "−" + nombre (abs x)

let position =
    function
    | Pour -> "pour"
    | Contre -> "contre"
    | Reserve -> "réservé"

let rec lire (invite: string) =
    Console.Write invite
    match Console.ReadLine() with
    | null -> None
    | l -> Some(l.Trim())

let afficherEtat (p: Partie) =
    let r = Moteur.evaluer p.etat
    let v = if r.viable.ok then "viable" else $"pas viable (maillon faible : {libelleJauge (fst r.viable.maillon)})"
    printfn ""
    let tranche =
        match trancheCourante p with
        | Some t -> let x = Jeu.tranches |> List.find (fun y -> y.id = t) in $" · {x.embleme} {x.titre}"
        | None -> ""

    printfn "══════ Révolution %d%s ══════" p.revolution tranche
    printfn "Score %s (France %s) · fidélité au but %s · %s" (nombre r.score) (nombre r.scoreFrance) (nombre r.fidelite) v

    let nouvelles = nouvellesDe p p.revolution

    if not nouvelles.IsEmpty then
        printfn ""
        printfn "Ce qui s'est passé :"

        for n in nouvelles do
            match n with
            | Survenu(_, id) ->
                let ev = Jeu.evenements |> List.find (fun x -> x.id = id)
                let effets = String.Join(", ", ev.effets |> List.map (fun f -> $"{libelleJauge f.jauge} {signe f.points}" + (if f.duree > 1 then $" pendant {f.duree} révolutions" else "")))
                printfn "  • %s. %s%s" ev.titre (propre ev.texte) (if effets = "" then "" else $" ({effets})")
            | Muri(_, b) ->
                let pari = Modele.paris |> List.find (fun x -> x.id = b)
                printfn "  • Ton pari arrive à maturité : %s. %s" pari.titre (propre pari.effet)
            | _ -> ()

let afficherApercu (a: Apercu) =
    let jauges = a.jauges |> List.filter (fun (_, d) -> abs d >= 0.0005)

    if jauges.IsEmpty then
        printfn "  Aucune jauge ne bouge."
    else
        printfn "  Jauges : %s" (String.Join(" · ", jauges |> List.map (fun (j, d) -> $"{libelleJauge j} {signe (d * 100.0)}")))

    printfn "  Score : %s → %s aussitôt, %s à terme" (nombre a.avant.score) (nombre a.aussitot.score) (nombre a.aTerme.score)

    match a.pari with
    | Some(b, 0) -> printfn "  Ce pari agit dès maintenant (%s)." (propre b.effet)
    | Some(b, n) -> printfn "  Ce pari demande %d révolutions avant d'agir (%s). Risque : %s" n (propre b.effet) (propre b.echec)
    | None -> ()

    for av in a.conseil do
        let par = if av.arguments |> List.exists (fun x -> x.lien.lentillePar = "assistant") then " (classement proposé par l'assistant)" else ""
        printfn "  ▸ %s%s" av.lentille.nom par

        for x in av.arguments |> List.truncate 3 do
            let etat =
                if x.avant = 0.0 then ""
                elif x.apres = 0.0 then " (ne joue plus)"
                else " (change)"

            printfn "      %s %s%s : %s (%A)" (signe x.Ecart) (libelleJauge x.lien.jauge) etat (propre x.lien.raison) x.lien.statut

    for v in a.voix do
        printfn "  %s : « %s » (%s)" (nomHabitant v.habitant) v.texte (position v.position)

let rec choisirDilemme (p: Partie) (s: Situation) =
    let levier = List.exactlyOne s.leviers
    let options = Tables.optionsDe[levier]
    printfn ""
    printfn "%s %s" s.embleme s.titre
    printfn "%s" (propre s.texte)

    for o in options do
        printfn "  %d. %s" (o.rang + 1) (propre o.libelle)

    match lire "Ton choix (un numéro, ou ?numéro pour l'avis du conseil) : " with
    | None -> None
    | Some l when l.StartsWith "?" ->
        match Int32.TryParse(l.Substring 1) with
        | true, n when n >= 1 && n <= options.Length ->
            match apercu p s.id [ { levier = levier; option = options[n - 1].id } ] with
            | Ok a -> afficherApercu a
            | Error e -> printfn "%s" (decrire e)
        | _ -> printfn "Numéro inconnu."

        choisirDilemme p s
    | Some l ->
        match Int32.TryParse l with
        | true, n when n >= 1 && n <= options.Length -> Some [ { levier = levier; option = options[n - 1].id } ]
        | _ ->
            printfn "Numéro inconnu."
            choisirDilemme p s

let rec choisirPari (p: Partie) (s: Situation) =
    let paris = s.paris |> List.map (fun id -> Modele.paris |> List.find (fun b -> b.id = id))
    printfn ""
    printfn "%s %s" s.embleme s.titre
    printfn "%s" (propre s.texte)

    paris
    |> List.iteri (fun i b ->
        let quand = if b.pretDans = 0 then "agit tout de suite" else $"agit dans {b.pretDans} révolutions"
        printfn "  %d. %s (%s)" (i + 1) b.titre quand)

    printfn "  0. Aucun"

    match lire "Ton choix (un numéro, ou ?numéro pour l'avis du conseil) : " with
    | None -> None
    | Some l when l.StartsWith "?" ->
        match Int32.TryParse(l.Substring 1) with
        | true, n when n >= 1 && n <= paris.Length ->
            match apercu p s.id [ { levier = "paris"; option = paris[n - 1].id } ] with
            | Ok a -> afficherApercu a
            | Error e -> printfn "%s" (decrire e)
        | _ -> printfn "Numéro inconnu."

        choisirPari p s
    | Some "0" -> Some [ { levier = "paris"; option = "aucun" } ]
    | Some l ->
        match Int32.TryParse l with
        | true, n when n >= 1 && n <= paris.Length -> Some [ { levier = "paris"; option = paris[n - 1].id } ]
        | _ ->
            printfn "Numéro inconnu."
            choisirPari p s

let choisirPrevention (s: Situation) =
    printfn ""
    printfn "%s %s" s.embleme s.titre
    printfn "%s" (propre s.texte)

    let rec un levier =
        match lire $"  {Tables.levierDe[levier].titre} (o/n) : " with
        | None -> None
        | Some("o" | "oui") -> Some { levier = levier; option = "oui" }
        | Some("n" | "non") -> Some { levier = levier; option = "non" }
        | _ -> un levier

    let choix = s.leviers |> List.map un
    if choix |> List.forall Option.isSome then Some(choix |> List.choose id) else None

let afficherConstat (s: Situation) =
    printfn ""
    printfn "— %s —" s.titre

    for c in Jeu.chiffres |> List.filter (fun c -> c.situation = s.id) do
        printfn "  %s %s" c.valeur c.legende

    printfn "%s" (propre s.texte)

let afficherBilan (p: Partie) =
    let r = Moteur.evaluer p.etat
    printfn ""
    printfn "══════ Bilan après %d révolutions ══════" p.revolution
    printfn "Viable ? %s" (if r.viable.ok then "oui" else $"non : {libelleJauge (fst r.viable.maillon)} sous son seuil vital")
    printfn "Fidèle au but ? %s (France %s)" (nombre r.fidelite) (nombre r.fideliteFrance)
    printfn "Mieux que la France ? %s contre %s" (nombre r.score) (nombre r.scoreFrance)
    let l = garde p.etat
    printfn "Ce qu'elle garde de ce qu'une société sans prison ni police voudrait abolir : %s" (if l.IsEmpty then "rien" else l |> List.map _.libelle |> String.concat ", ")
    // les défis : des objectifs qu'on se donne, pas des conclusions du modèle
    for d in Jeu.defis do
        printfn "Défi « %s » : %s" d.titre (Explorer.libelleIssue d p)
    printfn "Confiance des habitants : %s" (String.Join(" · ", p.reputation |> Map.toList |> List.map (fun (h, v) -> $"{nomHabitant h} {signe v}")))

let rec jouer (p: Partie) =
    afficherEtat p

    let rec situations (p: Partie) =
        function
        | [] -> Some p
        | (s: Situation) :: reste ->
            match s.genre with
            | Constat ->
                afficherConstat s
                situations p reste
            | Bilan -> situations p reste
            | Dilemme
            | Prevention
            | Paris ->
                let choix =
                    match s.genre with
                    | Dilemme -> choisirDilemme p s
                    | Paris -> choisirPari p s
                    | _ -> choisirPrevention s

                match choix with
                | None -> None
                | Some c ->
                    match decider p s.id c with
                    | Ok p2 ->
                        if p2.effets |> List.exists (fun f -> f.source = "decision:" + s.id) then
                            printfn "  L'effet s'installe : une partie tout de suite, le reste à la prochaine révolution."

                        for (h, d) in effetDecision p2 s c |> List.filter (fun (_, d) -> d <> 0.0) do
                            printfn "  %s : confiance %s" (nomHabitant h) (signe d)

                        situations p2 reste
                    | Error e ->
                        printfn "%s" (decrire e)
                        situations p (s :: reste)

    match situations p (ouvertes p) with
    | None -> printfn "\nÀ bientôt."
    | Some p ->
        match finirRevolution p with
        | Error e -> printfn "%s" (decrire e)
        | Ok p when terminee p -> afficherBilan p
        | Ok p -> jouer p

let jouerEnTexte (argv: string array) =
    // labo-jouer [tranche] [--graine N] : sans tranche, tout le parcours
    let args = List.ofArray argv

    let graine =
        match args |> List.skipWhile ((<>) "--graine") with
        | _ :: n :: _ -> int64 n
        | _ -> int64 (DateTime.Now.Ticks % 2147483647L)

    let parcours =
        match args |> List.filter (fun a -> not (a.StartsWith "--") && not (Seq.forall Char.IsDigit a)) with
        | t :: _ -> [ t ]
        | [] -> parcoursComplet

    match nouvelleAvec graine true parcours with
    | Error e ->
        eprintfn "%s ; tranches : %s" (decrire e) (String.Join(", ", parcoursComplet))
        2
    | Ok p ->
        prenoms <- p.prenoms
        printfn "Le Labo de la Bande · graine %d (pour rejouer la même partie : --graine %d)" graine graine
        jouer p
        0

[<EntryPoint>]
let main argv =
    match List.ofArray argv with
    | "explorer" :: reste -> Explorer.executer reste
    | "sensibilite" :: reste -> Sensibilite.executer reste
    | _ -> jouerEnTexte argv
