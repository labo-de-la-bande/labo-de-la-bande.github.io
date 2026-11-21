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
let nomHabitant id = (Jeu.habitants |> List.find (fun h -> h.id = id)).nom
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
    printfn "══════ Révolution %d ══════" p.revolution
    printfn "Score %s (France %s) · fidélité au but %s · %s" (nombre r.score) (nombre r.scoreFrance) (nombre r.fidelite) v

let afficherApercu (a: Apercu) =
    let jauges = a.jauges |> List.filter (fun (_, d) -> abs d >= 0.0005)

    if jauges.IsEmpty then
        printfn "  Aucune jauge ne bouge."
    else
        printfn "  Jauges : %s" (String.Join(" · ", jauges |> List.map (fun (j, d) -> $"{libelleJauge j} {signe (d * 100.0)}")))

    printfn "  Score : %s → %s" (nombre a.avant.score) (nombre a.apres.score)

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
            | Prevention ->
                let choix = if s.genre = Dilemme then choisirDilemme p s else choisirPrevention s

                match choix with
                | None -> None
                | Some c ->
                    match decider p s.id c with
                    | Ok p2 ->
                        for (h, d) in effetReputation s c |> List.filter (fun (_, d) -> d <> 0.0) do
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

[<EntryPoint>]
let main argv =
    let tranche = argv |> Array.tryHead |> Option.defaultValue "vivre"

    match nouvelle tranche with
    | Error e ->
        eprintfn "%s ; tranches : %s" (decrire e) (String.Join(", ", Jeu.tranches |> List.map _.id))
        2
    | Ok p ->
        let t = Jeu.tranches |> List.find (fun x -> x.id = tranche)
        printfn "%s %s : %s" t.embleme t.titre t.sousTitre
        jouer p
        0
