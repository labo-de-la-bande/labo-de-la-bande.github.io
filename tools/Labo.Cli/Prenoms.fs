/// `labo prenoms importer [export.json]` : verse dans data/prenoms/joueurs.csv les prénoms que les
/// joueurs ont déclarés au relais (des comptes agrégés). Sans fichier, lit l'export du relais :
/// adresse dans LABO_RELAIS, jeton dans LABO_EXPORT_CLE (le secret EXPORT_CLE du relais).
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

let telecharger () =
    let adresse = Environment.GetEnvironmentVariable "LABO_RELAIS"
    let cle = Environment.GetEnvironmentVariable "LABO_EXPORT_CLE"

    if String.IsNullOrEmpty adresse || String.IsNullOrEmpty cle then
        Error "sans fichier, il faut LABO_RELAIS (l'adresse du relais) et LABO_EXPORT_CLE (son jeton d'export)"
    else
        use client = new HttpClient()
        use req = new HttpRequestMessage(HttpMethod.Get, adresse.TrimEnd('/') + "/prenoms/export")
        req.Headers.Add("Authorization", "Bearer " + cle)
        let rep = client.Send req
        let texte = rep.Content.ReadAsStringAsync().Result

        if rep.IsSuccessStatusCode then Ok texte else Error $"le relais répond {int rep.StatusCode} : {texte}"

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
