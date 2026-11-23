// Fabrique data/pays/fr/prenoms.csv à partir du « fichier des prénoms » de l'Insee, édition 2025
// (naissances par prénom, sexe et année depuis 1900, pour la France et par région ; licence
// ouverte). Source : sources.csv, id `insee-prenoms`. Usage, depuis la racine du dépôt :
//   dotnet fsi data/pays/fr/scripts/prenoms.fsx <prenoms-2025-nat.csv> <prenoms-2025-reg.csv>
// (fichiers prenoms-2025-nat_csv.zip et prenoms-2025-reg_csv.zip de la page de la source)
// - région `france` : par décennie et par sexe, de 1930 à aujourd'hui, les prénoms qui couvrent
//   95 % des naissances (400 au plus) ;
// - chaque région de data/pays/fr/regions.csv (colonne code_insee) : les décennies 1940 à 2010 (les
//   âges des personnages), les prénoms qui couvrent 85 % des naissances (60 au plus).
// Les coupes gardent la banque légère : le tirage est pondéré par les naissances, et un prénom
// au-delà de la coupe n'aurait presque aucune chance d'être tiré (mesuré le 23 novembre 2026,
// CHANTIERS « Les prénoms »). L'Insee ne publie pas les prénoms très rares : aucun prénom ne peut
// désigner une personne réelle.

open System
open System.Globalization
open System.IO
open System.Text

let args = fsi.CommandLineArgs
let fichierNational, fichierRegions = args[1], args[2]

/// « MARIE-CLAIRE » → « Marie-Claire », « INÈS » → « Inès »
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
    |> Seq.map (fun l ->
        let c = l.Split ','
        c[c.Length - 1], c[0])
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

let utile (prenom: string) (annee: string) = not (prenom.StartsWith "_") && annee <> "XXXX"
let sexeDe s = if s = "1" then "m" else "f"

// sexe;prenom;periode;valeur;rang
lire fichierNational
|> Seq.filter (fun c -> c.Length = 5 && utile c[1] c[2])
|> Seq.map (fun c -> sexeDe c[0], int c[2] / 10 * 10, casse c[1], int c[3])
|> Seq.filter (fun (_, d, _, _) -> d >= 1930)
|> ecrire "france" 0.95 400

// sexe;prenom;periode;reg;valeur;rang
let regions =
    lire fichierRegions
    |> Seq.filter (fun c -> c.Length = 6 && utile c[1] c[2] && regionDe.ContainsKey c[3])
    |> Seq.map (fun c -> regionDe[c[3]], (sexeDe c[0], int c[2] / 10 * 10, casse c[1], int c[4]))
    |> Seq.filter (fun (_, (_, d, _, _)) -> d >= 1940 && d <= 2010)
    |> Seq.toList

for region, lignes in regions |> List.groupBy fst |> List.sortBy fst do
    ecrire region 0.85 60 (lignes |> Seq.map snd)

File.WriteAllText("data/pays/fr/prenoms.csv", sortie.ToString())
printfn "data/pays/fr/prenoms.csv : %d lignes" (sortie.ToString().Split('\n').Length - 2)
