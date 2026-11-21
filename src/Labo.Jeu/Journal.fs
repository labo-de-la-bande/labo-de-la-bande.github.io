/// Le journal d'actions : une partie est sa configuration (région, graine, hasard, parcours) et
/// la liste des actions du joueur. L'état se recalcule en les rejouant : on sauvegarde, on
/// reprend et on partage une partie avec un code court, et la même liste rejoue la même partie.
module Labo.Jeu.Journal

open System
open Labo.Jeu.Partie

type Configuration =
    { region: string
      graine: int64
      hasard: bool
      parcours: string list }

type ErreurJournal =
    /// une action ne s'applique pas à l'état où elle arrive
    | ActionRefusee of rang: int * action: Action * raison: string
    | CodeIllisible of raison: string

let decrireErreur =
    function
    | ActionRefusee(i, a, r) -> $"action {i + 1} ({a}) refusée : {r}"
    | CodeIllisible r -> $"code de partie illisible : {r}"

let configuration (p: Partie) =
    { region = p.region
      graine = p.graineDepart
      hasard = p.hasard
      parcours = p.parcours }

let commencer (c: Configuration) =
    nouvelleDans c.region c.graine c.hasard c.parcours |> Result.mapError decrire

/// Joue une action et l'inscrit au journal.
let jouer (a: Action) (p: Partie) : Result<Partie, string> =
    let resultat =
        match a with
        | Decider(s, c) -> decider p s c |> Result.mapError decrire
        | Repondre(s, id) ->
            match Scene.repliqueDe.TryFind id with
            | Some r when r.situation = s -> Ok(repondre p s r)
            | _ -> Error $"réplique inconnue : {id}"
        | ProposerIdee(s, t) -> Ok(proposerIdee p s t)
        | RevenirSur s -> revenirSur p s |> Result.mapError (fun _ -> $"rien à reprendre dans {s}")
        | FinirRevolution -> finirRevolution p |> Result.mapError decrire

    resultat |> Result.map (fun p2 -> { p2 with actions = p.actions @ [ a ] })

/// Rejoue une partie depuis sa configuration et ses actions.
let rejouer (c: Configuration) (actions: Action list) =
    match commencer c with
    | Error e -> Error(CodeIllisible e)
    | Ok depart ->
        actions
        |> List.indexed
        |> List.fold
            (fun acc (i, a) -> acc |> Result.bind (fun p -> jouer a p |> Result.mapError (fun r -> ActionRefusee(i, a, r))))
            (Ok depart)

// ---------- Le code de partie ----------
// labo1|région|graine|hasard|tranche,tranche|action|action…
// D:situation:levier=option,levier=option · R:situation:réplique · I:situation:texte · B:situation · F

let encoderAction =
    function
    | Decider(s, c) -> "D:" + s + ":" + String.Join(",", c |> List.map (fun x -> x.levier + "=" + x.option))
    | Repondre(s, r) -> "R:" + s + ":" + r
    | ProposerIdee(s, t) -> "I:" + s + ":" + Uri.EscapeDataString t
    | RevenirSur s -> "B:" + s
    | FinirRevolution -> "F"

let decoderAction (texte: string) =
    match texte.Split ':' with
    | [| "F" |] -> Ok FinirRevolution
    | [| "B"; s |] -> Ok(RevenirSur s)
    | [| "R"; s; r |] -> Ok(Repondre(s, r))
    | [| "I"; s; t |] -> Ok(ProposerIdee(s, Uri.UnescapeDataString t))
    | [| "D"; s; c |] ->
        let choix =
            c.Split ','
            |> Array.map (fun x ->
                match x.Split '=' with
                | [| l; o |] -> Some { levier = l; option = o }
                | _ -> None)

        if choix |> Array.forall Option.isSome then
            Ok(Decider(s, choix |> Array.choose id |> List.ofArray))
        else
            Error(CodeIllisible $"décision mal écrite : {texte}")
    | _ -> Error(CodeIllisible $"action inconnue : {texte}")

let encoder (p: Partie) =
    let c = configuration p

    String.Join(
        "|",
        [ "labo1"; c.region; string c.graine; (if c.hasard then "1" else "0"); String.Join(",", c.parcours) ]
        @ (p.actions |> List.map encoderAction)
    )

let decoder (code: string) =
    match code.Trim().Split '|' |> List.ofArray with
    | "labo1" :: region :: graine :: hasard :: parcours :: actions ->
        match Int64.TryParse graine with
        | true, g ->
            let decodees = actions |> List.filter ((<>) "") |> List.map decoderAction

            match decodees |> List.tryPick (function Error e -> Some e | Ok _ -> None) with
            | Some e -> Error e
            | None ->
                let c =
                    { region = region
                      graine = g
                      hasard = (hasard = "1")
                      parcours = parcours.Split(',', StringSplitOptions.RemoveEmptyEntries) |> List.ofArray }

                rejouer c (decodees |> List.choose (function Ok a -> Some a | Error _ -> None))
        | _ -> Error(CodeIllisible "graine illisible")
    | _ -> Error(CodeIllisible "ce n'est pas un code de partie du Labo")
