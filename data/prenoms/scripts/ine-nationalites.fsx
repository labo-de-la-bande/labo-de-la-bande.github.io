// Fabrique data/prenoms/pays.csv : les dix prénoms les plus portés, par sexe, par les résidents
// d'Espagne de chaque grande nationalité (INE, Estadística del Padrón Continuo au 1er janvier
// 2022 ; source `ine-nombres-nacionalidad`). Une banque par pays, qui sert quel que soit le pays
// joué : pour les personnages nés dans ce pays.
// Usage, depuis la racine du dépôt :
//   dotnet fsi data/prenoms/scripts/ine-nationalites.fsx <nombres_por_nacionalidades.xlsx>
// L'INE publie un .xls (ancien format) : le convertir d'abord en .xlsx, par exemple avec
// LibreOffice (soffice --headless --convert-to xlsx nombres_por_nacionalidades.xls).
// Limites : dix prénoms par sexe, tous âges confondus (pas de décennie de naissance), des
// résidents en Espagne et non des immigrés en France ; l'INE retire accents et traits d'union.

open System
open System.Globalization
open System.IO
open System.IO.Compression
open System.Text
open System.Xml.Linq

/// Le nom espagnol de la nationalité dans le fichier de l'INE → code ISO du pays, nom en français.
let pays =
    Map
        [ "ESPAÑA", ("es", "Espagne"); "ALEMANIA", ("de", "Allemagne"); "BÉLGICA", ("be", "Belgique")
          "BULGARIA", ("bg", "Bulgarie"); "FRANCIA", ("fr", "France"); "IRLANDA", ("ie", "Irlande")
          "ITALIA", ("it", "Italie"); "LITUANIA", ("lt", "Lituanie"); "MOLDAVIA", ("md", "Moldavie")
          "NORUEGA", ("no", "Norvège"); "PAÍSES BAJOS", ("nl", "Pays-Bas"); "POLONIA", ("pl", "Pologne")
          "PORTUGAL", ("pt", "Portugal"); "REINO UNIDO", ("gb", "Royaume-Uni"); "RUMANÍA", ("ro", "Roumanie")
          "RUSIA", ("ru", "Russie"); "SUECIA", ("se", "Suède"); "SUIZA", ("ch", "Suisse"); "UCRANIA", ("ua", "Ukraine")
          "ARGELIA", ("dz", "Algérie"); "GAMBIA", ("gm", "Gambie"); "GUINEA ECUATORIAL", ("gq", "Guinée équatoriale")
          "MALI", ("ml", "Mali"); "MARRUECOS", ("ma", "Maroc"); "NIGERIA", ("ng", "Nigeria"); "SENEGAL", ("sn", "Sénégal")
          "ARGENTINA", ("ar", "Argentine"); "BOLIVIA", ("bo", "Bolivie"); "BRASIL", ("br", "Brésil"); "CHILE", ("cl", "Chili")
          "COLOMBIA", ("co", "Colombie"); "CUBA", ("cu", "Cuba"); "ECUADOR", ("ec", "Équateur")
          "ESTADOS UNIDOS", ("us", "États-Unis"); "HONDURAS", ("hn", "Honduras"); "MÉXICO", ("mx", "Mexique")
          "PARAGUAY", ("py", "Paraguay"); "PERÚ", ("pe", "Pérou"); "REP. DOMINICANA", ("do", "République dominicaine")
          "URUGUAY", ("uy", "Uruguay"); "VENEZUELA", ("ve", "Venezuela"); "CHINA", ("cn", "Chine")
          "FILIPINAS", ("ph", "Philippines"); "INDIA", ("in", "Inde"); "PAKISTÁN", ("pk", "Pakistan") ]

let ns = XNamespace.Get "http://schemas.openxmlformats.org/spreadsheetml/2006/main"
let z = ZipFile.OpenRead fsi.CommandLineArgs[1]

let partages =
    XDocument.Load(z.GetEntry("xl/sharedStrings.xml").Open()).Descendants(ns + "si")
    |> Seq.map (fun si -> si.Descendants(ns + "t") |> Seq.map _.Value |> String.concat "")
    |> Array.ofSeq

/// Les lignes d'une feuille, cellule par cellule à leur colonne (A = 0).
let lignes (feuille: string) =
    let colonne (r: string) =
        r |> Seq.takeWhile Char.IsLetter |> Seq.fold (fun acc c -> acc * 26 + int c - int 'A' + 1) 0 |> fun n -> n - 1

    XDocument.Load(z.GetEntry(feuille).Open()).Descendants(ns + "row")
    |> Seq.map (fun r ->
        r.Elements(ns + "c")
        |> Seq.choose (fun c ->
            match c.Element(ns + "v") with
            | null -> None
            | v ->
                let texte = if string (c.Attribute(XName.Get "t")) = "t=\"s\"" then partages[int v.Value] else v.Value
                Some(colonne (c.Attribute(XName.Get "r").Value), texte.Trim()))
        |> Map.ofSeq)
    |> List.ofSeq

/// « MARIA CARMEN » → « Maria Carmen »
let casse (p: string) =
    let bas = p.ToLower(CultureInfo "es-ES")
    let sb = StringBuilder()
    let mutable debut = true

    for c in bas do
        sb.Append(if debut then Char.ToUpper(c, CultureInfo "es-ES") else c) |> ignore
        debut <- c = '-' || c = ' ' || c = '\''

    sb.ToString()

let sortie = StringBuilder("pays,nom_fr,sexe,prenom,pour_mille\n")
let mutable n = 0

for f in z.Entries |> Seq.map _.FullName |> Seq.filter (fun f -> f.StartsWith "xl/worksheets/sheet") |> Seq.sort do
    let mutable colonnes: (int * (string * string)) list = []
    let mutable sexe = ""

    for l in lignes f do
        // une ligne d'en-tête : les nationalités, toutes les trois colonnes
        let entetes = l |> Map.toList |> List.choose (fun (c, v) -> pays.TryFind v |> Option.map (fun p -> c, p))

        if not entetes.IsEmpty then
            colonnes <- entetes
        elif l |> Map.exists (fun _ v -> v.StartsWith "NOMBRE (hombres)") then
            sexe <- "m"
        elif l |> Map.exists (fun _ v -> v.StartsWith "NOMBRE (mujeres)") then
            sexe <- "f"
        elif sexe <> "" && (l.TryFind 0 |> Option.exists (fun r -> fst (Int32.TryParse r))) then
            for (c, (code, nom)) in colonnes do
                match l.TryFind c, l.TryFind(c + 2) with
                | Some prenom, Some pm when prenom <> "" ->
                    let pm = Double.Parse(pm, CultureInfo.InvariantCulture)
                    sortie.Append($"{code},{nom},{sexe},{casse prenom},{pm.ToString(CultureInfo.InvariantCulture)}\n") |> ignore
                    n <- n + 1
                | _ -> ()

Directory.CreateDirectory "data/prenoms" |> ignore
File.WriteAllText("data/prenoms/pays.csv", sortie.ToString())
printfn "data/prenoms/pays.csv : %d prénoms" n
