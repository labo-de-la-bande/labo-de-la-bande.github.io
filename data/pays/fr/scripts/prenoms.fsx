// Fabrique data/pays/fr/prenoms.csv à partir du « fichier des prénoms » de l'Insee
// (naissances par prénom, sexe et année depuis 1900, pour la France et par département ;
// licence ouverte). Source : sources.csv, id `insee-prenoms`. Usage, depuis la racine du dépôt :
//   dotnet fsi data/pays/fr/scripts/prenoms.fsx <nat2021.csv> <dpt2021.csv>
// - région `france` : par décennie et par sexe, les prénoms qui couvrent 90 % des naissances
//   (200 au plus), de 1900 à aujourd'hui ;
// - chaque région de data/pays/fr/regions.csv : les décennies 1940 à 2010 (les âges des
//   personnages), les prénoms qui couvrent 80 % des naissances (50 au plus).
// Les prénoms très rares, que l'Insee regroupe sous `_PRENOMS_RARES`, ne sont pas repris : aucun
// prénom ne peut désigner une personne réelle.

open System
open System.Globalization
open System.IO
open System.Text

let args = fsi.CommandLineArgs
let fichierNational, fichierDepartements = args[1], args[2]

/// « MARIE-CLAIRE » → « Marie-Claire »
let casse (p: string) =
    let bas = p.ToLower(CultureInfo "fr-FR")
    let sb = StringBuilder()
    let mutable debut = true

    for c in bas do
        sb.Append(if debut then Char.ToUpper(c, CultureInfo "fr-FR") else c) |> ignore
        debut <- c = '-' || c = ' ' || c = '\''

    sb.ToString()

let regionDe =
    File.ReadLines "data/pays/fr/regions.csv"
    |> Seq.skip 1
    |> Seq.collect (fun l ->
        let c = l.Split ','
        (c[c.Length - 1]).Split '|' |> Seq.map (fun d -> d, c[0]))
    |> Map.ofSeq

let sortie = StringBuilder("region,sexe,decennie,prenom,naissances\n")

/// Garde, par groupe, les prénoms les plus donnés jusqu'à la couverture voulue.
let ecrire region couverture plafond (lignes: (string * int * string * int) seq) =
    for (sexe, decennie), groupe in lignes |> Seq.groupBy (fun (s, d, _, _) -> s, d) |> Seq.sortBy fst do
        let parPrenom =
            groupe
            |> Seq.groupBy (fun (_, _, p, _) -> p)
            |> Seq.map (fun (p, l) -> p, l |> Seq.sumBy (fun (_, _, _, n) -> n))
            |> Seq.sortByDescending snd
            |> List.ofSeq

        let total = parPrenom |> List.sumBy snd |> float
        let mutable cumul = 0

        for (p, n) in parPrenom |> List.truncate plafond do
            if float cumul < couverture * total then
                sortie.Append($"{region},{sexe},{decennie},{p},{n}\n") |> ignore
                cumul <- cumul + n

let lire (chemin: string) =
    File.ReadLines(chemin, Encoding.UTF8) |> Seq.skip 1 |> Seq.map (fun l -> l.Split ';')

let utile (c: string array) = c[1] <> "_PRENOMS_RARES" && c[2] <> "XXXX"
let sexeDe s = if s = "1" then "m" else "f"

lire fichierNational
|> Seq.filter (fun c -> c.Length = 4 && utile c)
|> Seq.map (fun c -> sexeDe c[0], int c[2] / 10 * 10, casse c[1], int c[3])
|> ecrire "france" 0.90 200

let departements =
    lire fichierDepartements
    |> Seq.filter (fun c -> c.Length = 5 && utile c && regionDe.ContainsKey c[3])
    |> Seq.map (fun c -> regionDe[c[3]], (sexeDe c[0], int c[2] / 10 * 10, casse c[1], int c[4]))
    |> Seq.filter (fun (_, (_, d, _, _)) -> d >= 1940 && d <= 2010)
    |> Seq.toList

for region, lignes in departements |> List.groupBy fst |> List.sortBy fst do
    ecrire region 0.80 50 (lignes |> Seq.map snd)

File.WriteAllText("data/pays/fr/prenoms.csv", sortie.ToString(), UTF8Encoding false)
printfn "data/pays/fr/prenoms.csv : %d lignes" (sortie.ToString().Split('\n').Length - 2)
