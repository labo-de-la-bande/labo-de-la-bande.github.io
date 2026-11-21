/// Les scènes : une conversation avant une décision, et la réaction qui la suit. Les habitants
/// parlent, les lentilles paraphrasent, Labo rappelle le modèle, et le joueur choisit ses
/// répliques ou écrit sa propre idée. Une scène se déroule jusqu'à un choix du joueur ou jusqu'à
/// la fin. Quand plusieurs répliques sont possibles au même moment, le jeu en tire une avec la
/// graine de la partie (rejouable) ; une réplique conditionnée à la confiance l'emporte quand sa
/// condition est remplie : c'est ainsi qu'un personnage se souvient du joueur.
module Labo.Jeu.Scene

open Labo.Data

/// Ce dont dépend le déroulé : la graine de la partie et la confiance des habitants.
type Contexte =
    { graine: int64
      confiance: Map<string, float> }

let repliqueDe = Jeu.repliques |> List.map (fun r -> r.id, r) |> Map.ofList

let conditionRemplie ctx (r: Replique) =
    r.siConfiance
    |> List.forall (fun (h, c, seuil) ->
        let v = ctx.confiance.TryFind h |> Option.defaultValue 0.0

        match c with
        | Inferieur -> v < seuil
        | Superieur -> v > seuil)

/// Un tirage stable : la même graine et la même clé donnent toujours la même réplique.
let tirer ctx (cle: string) (candidats: Replique list) =
    match candidats |> List.filter (conditionRemplie ctx) with
    | [] -> None
    | [ x ] -> Some x
    | possibles ->
        let specifiques = possibles |> List.filter (fun r -> not r.siConfiance.IsEmpty)
        let lot = if specifiques.IsEmpty then possibles else specifiques
        let h = cle |> Seq.fold (fun (h: int64) c -> (h * 31L + int64 c) % 2147483647L) (abs ctx.graine % 2147483647L)
        Some lot[int (h % int64 lot.Length)]

/// La première réplique d'une scène : avant la décision, ou en réaction à l'option choisie.
let entree ctx situation moment (option: string option) =
    let lot =
        Jeu.repliques
        |> List.filter (fun r -> r.situation = situation && r.moment = moment && (moment = Avant || r.option = option))

    match lot with
    | [] -> None
    | _ ->
        let premier = lot |> List.map _.rang |> List.min
        tirer ctx $"{situation}/{moment}/{option}" (lot |> List.filter (fun r -> r.rang = premier))

/// Les répliques à montrer à partir de `depuis`, et les choix du joueur qui suivent (vide si la
/// scène est finie).
let derouler ctx (depuis: Replique) =
    let rec aller (r: Replique) acc =
        let acc = r :: acc
        let suivantes = r.suite |> List.choose repliqueDe.TryFind

        match suivantes with
        | [] -> List.rev acc, []
        | s when s |> List.forall (fun x -> x.nature = Joueur) -> List.rev acc, s |> List.filter (conditionRemplie ctx)
        | s ->
            match tirer ctx r.id s with
            | Some x -> aller x acc
            | None -> List.rev acc, []

    aller depuis []

/// Rejoue une scène : l'entrée, puis chaque choix du joueur dans l'ordre. Rend les répliques à
/// montrer et les choix encore ouverts.
let rejouer ctx situation moment option (choix: string list) =
    match entree ctx situation moment option with
    | None -> [], []
    | Some e ->
        choix
        |> List.fold
            (fun (lignes, ouverts: Replique list) id ->
                match ouverts |> List.tryFind (fun c -> c.id = id) with
                | Some c ->
                    let l2, o2 = derouler ctx c
                    lignes @ l2, o2
                | None -> lignes, ouverts)
            (derouler ctx e)

/// Y a-t-il une réaction à cette option ?
let aUneReaction situation option =
    Jeu.repliques |> List.exists (fun r -> r.situation = situation && r.moment = Apres && r.option = Some option)
