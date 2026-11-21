/// Les scènes : une conversation avant une décision. Les habitants parlent, les lentilles
/// paraphrasent, Labo rappelle le modèle, et le joueur choisit ses répliques ou écrit sa propre
/// idée. Une scène se déroule réplique après réplique jusqu'à un choix du joueur ou jusqu'à la fin.
module Labo.Jeu.Scene

open Labo.Data

let repliqueDe = Jeu.repliques |> List.map (fun r -> r.id, r) |> Map.ofList

/// La première réplique de la scène d'une situation, s'il y en a une.
let entree situation =
    Jeu.repliques |> List.filter (fun r -> r.situation = situation) |> List.sortBy _.rang |> List.tryHead

/// Les répliques à montrer à partir de `depuis`, et les choix du joueur qui suivent (vide si la
/// scène est finie).
let derouler (depuis: Replique) =
    let rec aller (r: Replique) acc =
        let acc = r :: acc
        let suivantes = r.suite |> List.choose repliqueDe.TryFind

        match suivantes with
        | [] -> List.rev acc, []
        | s when s |> List.forall (fun x -> x.nature = Joueur) -> List.rev acc, s
        | [ x ] -> aller x acc
        | _ -> List.rev acc, []

    aller depuis []

/// Rejoue une scène : l'entrée, puis chaque choix du joueur dans l'ordre. Rend les répliques à
/// montrer et les choix encore ouverts.
let rejouer situation (choix: string list) =
    match entree situation with
    | None -> [], []
    | Some e ->
        choix
        |> List.fold
            (fun (lignes, ouverts: Replique list) id ->
                match ouverts |> List.tryFind (fun c -> c.id = id) with
                | Some c ->
                    let l2, o2 = derouler c
                    lignes @ l2, o2
                | None -> lignes, ouverts)
            (derouler e)
