// Fabrique data/pays/fr/prenoms.csv à partir du « fichier des prénoms » de l'Insee
// (naissances par prénom, sexe et année depuis 1900 ; licence ouverte). Source : sources.csv,
// id `insee-prenoms`. Usage, depuis la racine du dépôt :
//   dotnet fsi data/pays/fr/scripts/prenoms.fsx <chemin vers nat2021.csv>
// Par décennie de naissance et par sexe, garde les prénoms les plus donnés jusqu'à couvrir 90 %
// des naissances (200 au plus). Les prénoms très rares, que l'Insee regroupe déjà sous
// `_PRENOMS_RARES`, ne sont pas repris : aucun prénom ne peut désigner une personne réelle.

open System
open System.Globalization
open System.IO
open System.Text

let source = fsi.CommandLineArgs |> Array.tryItem 1 |> Option.defaultWith (fun () -> failwith "chemin du fichier Insee attendu")
let couverture, plafond = 0.90, 200

/// « MARIE-CLAIRE » → « Marie-Claire »
let casse (p: string) =
    let bas = p.ToLower(CultureInfo "fr-FR")
    let sb = StringBuilder()
    let mutable debut = true

    for c in bas do
        sb.Append(if debut then Char.ToUpper(c, CultureInfo "fr-FR") else c) |> ignore
        debut <- c = '-' || c = ' ' || c = '\''

    sb.ToString()

let lignes =
    File.ReadLines(source, Encoding.UTF8)
    |> Seq.skip 1
    |> Seq.map (fun l -> l.Split ';')
    |> Seq.filter (fun c -> c.Length = 4 && c[1] <> "_PRENOMS_RARES" && c[2] <> "XXXX")
    |> Seq.map (fun c -> (if c[0] = "1" then "m" else "f"), int c[2] / 10 * 10, casse c[1], int c[3])
    |> Seq.toList

let sortie = StringBuilder("sexe,decennie,prenom,naissances\n")

for (sexe, decennie), groupe in lignes |> List.groupBy (fun (s, d, _, _) -> s, d) |> List.sortBy fst do
    let parPrenom =
        groupe
        |> List.groupBy (fun (_, _, p, _) -> p)
        |> List.map (fun (p, l) -> p, l |> List.sumBy (fun (_, _, _, n) -> n))
        |> List.sortByDescending snd

    let total = parPrenom |> List.sumBy snd |> float
    let mutable cumul = 0

    for (p, n) in parPrenom |> List.truncate plafond do
        if float cumul < couverture * total then
            sortie.Append($"{sexe},{decennie},{p},{n}\n") |> ignore
            cumul <- cumul + n

File.WriteAllText("data/pays/fr/prenoms.csv", sortie.ToString(), UTF8Encoding false)
printfn "data/pays/fr/prenoms.csv : %d lignes" (sortie.ToString().Split('\n').Length - 2)
