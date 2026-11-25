/// Les textes du jeu à l'écran : les marqueurs `[réf:id]` deviennent des appels de note qui
/// nomment la source, et les nombres s'écrivent à la française.
module Labo.Interface.Textes

open System.Text.RegularExpressions
open Labo.Data
open Labo.Interface.Html

let sourceDe = Sources.tous |> List.map (fun s -> s.id, s) |> Map.ofList

let titreSource id =
    match sourceDe.TryFind id with
    | Some s when s.titre <> "" -> s.titre
    | Some s -> s.id
    | None -> "source à trouver"

/// Les prénoms de la partie en cours (tirés au début de chaque partie).
let mutable prenoms: Map<string, string> = Map.empty

let nomHabitant id =
    match prenoms.TryFind id with
    | Some n -> n
    | None -> (Jeu.habitants |> List.find (fun h -> h.id = id)).nom

/// Remplace les marqueurs `{rôle}` par les prénoms de la partie.
let nommer (t: string) =
    Regex.Replace(t, "\\{([a-z-]+)\\}", fun m -> nomHabitant m.Groups[1].Value)

/// Un texte avec ses appels de note.
let riche (t: string) =
    let morceaux = Regex.Split(nommer t, "(\\s*\\[réf:[^\\]]*\\])")

    morceaux
    |> Array.toList
    |> List.map (fun m ->
        let r = Regex.Match(m, "\\[réf:([^\\]]*)\\]")

        if r.Success then
            let id = r.Groups[1].Value

            if id = "?" then
                el "sup" [ "class", "ref manque"; "title", "Une source reste à trouver" ] [ texte "?" ]
            else
                el "sup" [ "class", "ref"; "title", titreSource id ] [ texte "•" ]
        else
            texte m)

let libelleStatut =
    function
    | Sourcé -> "sourcé"
    | Hypothèse -> "hypothèse"
    | Jugement -> "jugement"
    | Témoignage -> "témoignage"
    | DeMémoire -> "de mémoire"
    | ADocumenter -> "à documenter"

/// 63,7 ; −2,0
let nombre (x: float) =
    let s = (sprintf "%.1f" (abs x)).Replace(".", ",")
    if x < 0.0 && s <> "0,0" then "−" + s else s

let signe (x: float) =
    if x >= 0.0 then "+" + nombre x else nombre x

let libelleJauge id =
    (Jauges.tous |> List.find (fun j -> j.id = id)).libelle

let initiales (nom: string) =
    nom.Split([| ' '; '-'; '’'; '\'' |], System.StringSplitOptions.RemoveEmptyEntries)
    |> Array.filter (fun w -> w.Length > 2)
    |> Array.truncate 2
    |> Array.map (fun w -> string w[0])
    |> String.concat ""
    |> fun i -> if i = "" && nom <> "" then string nom[0] else i

/// Une teinte stable pour un identifiant (les avatars en formes simples).
let teinte (id: string) =
    let h = id |> Seq.fold (fun acc c -> (acc * 31 + int c) % 360) 7
    $"hsl({h} 55%% 48%%)"
