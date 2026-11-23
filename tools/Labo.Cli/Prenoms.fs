/// `labo prenoms importer [export.json]` : verse dans data/prenoms/joueurs.csv les prénoms que les
/// joueurs ont proposés au relais (des comptes). Sans fichier, les lit au relais en tant que
/// collaborateur du dépôt (avec le jeton de gh, voir Moderation.fs).
/// Seuls les comptes au-dessus du seuil (coefficient prenoms-joueurs-seuil) entrent ; un prénom
/// nouveau arrive sans validation (colonne valide_par vide) : le tirage ne s'en sert qu'une fois
/// validé par l'auteur. Les comptes des lignes déjà présentes sont mis à jour, leur validation gardée.
module Labo.Cli.Prenoms

open System
open System.IO
open System.Net.Http
open System.Text.Json

type Compte =
    { pays: string
      decennie: int
      usage: string
      prenom: string
      declarations: int }

let lireExport (json: string) =
    use doc = JsonDocument.Parse json

    [ for e in doc.RootElement.EnumerateArray() ->
          { pays = e.GetProperty("pays").GetString()
            decennie = e.GetProperty("decennie").GetInt32()
            usage = e.GetProperty("usage").GetString()
            prenom = e.GetProperty("prenom").GetString()
            declarations = e.GetProperty("declarations").GetInt32() } ]

/// Les comptes du relais, lus en tant que collaborateur du dépôt (le jeton de gh).
let telecharger () =
    Moderation.appeler System.Net.Http.HttpMethod.Get "/moderation/prenoms" None

let champ (s: string) =
    if s.Contains "," || s.Contains "\"" then "\"" + s.Replace("\"", "\"\"") + "\"" else s

let seuil () =
    File.ReadLines(Repo.path [ "data"; "coefficients.csv" ])
    |> Seq.tryPick (fun l ->
        match l.Split(',') with
        | c when c.Length > 1 && c[0] = "prenoms-joueurs-seuil" -> Some(int c[1])
        | _ -> None)
    |> Option.defaultValue 3

let importer (comptes: Compte list) =
    let fichier = Repo.path [ "data"; "prenoms"; "joueurs.csv" ]
    let paysConnus = File.ReadLines(Repo.path [ "data"; "pays-du-monde.csv" ]) |> Seq.skip 1 |> Seq.map (fun l -> l.Split(',')[0]) |> Set.ofSeq
    let s = seuil ()

    let existants =
        match Csv.analyser (File.ReadAllText fichier) with
        | Ok(_ :: lignes) ->
            lignes
            |> List.map (fun l ->
                match l.champs with
                | [ p; d; u; n; k; v ] -> (p, int d, u, n), (int k, v)
                | _ -> failwith $"joueurs.csv, ligne {l.numero} illisible")
        | Ok [] -> []
        | Error e -> failwith $"joueurs.csv illisible : %A{e}"
        |> Map.ofList

    let retenus = comptes |> List.filter (fun c -> c.declarations >= s && paysConnus.Contains c.pays)
    let nouveaux = retenus |> List.filter (fun c -> not (existants.ContainsKey(c.pays, c.decennie, c.usage, c.prenom)))

    let fusion =
        retenus
        |> List.fold (fun (m: Map<_, _>) c ->
            let k = (c.pays, c.decennie, c.usage, c.prenom)
            let valide = m.TryFind k |> Option.map snd |> Option.defaultValue ""
            m.Add(k, (max c.declarations (m.TryFind k |> Option.map fst |> Option.defaultValue 0), valide))) existants

    let lignes =
        fusion
        |> Map.toList
        |> List.sortBy (fun ((p, d, u, n), _) -> p, d, u, n)
        |> List.map (fun ((p, d, u, n), (k, v)) -> String.Join(",", [ p; string d; u; champ n; string k; v ]))

    File.WriteAllLines(fichier, "pays,decennie,usage,prenom,declarations,valide_par" :: lignes)
    printfn "%d compte(s) dans l'export, %d au-dessus du seuil (%d déclarations)." comptes.Length retenus.Length s
    printfn "%d prénom(s) nouveau(x), à valider (colonne valide_par = auteur) :" nouveaux.Length

    for c in nouveaux do
        printfn "  %s · %s · années %d · %s · %d déclarations" c.prenom c.pays c.decennie c.usage c.declarations

    printfn "Puis : labo generate, et labo doctor."
    0

let executer (fichier: string option) =
    let json =
        match fichier with
        | Some f -> Ok(File.ReadAllText f)
        | None -> telecharger ()

    match json with
    | Error e ->
        eprintfn "%s" e
        1
    | Ok j -> importer (lireExport j)
