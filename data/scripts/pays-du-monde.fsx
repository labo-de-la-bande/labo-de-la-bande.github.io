// Fabrique data/pays-du-monde.csv : les codes de pays ISO 3166-1 alpha-2 que connaît la
// bibliothèque ICU (Unicode CLDR) embarquée par .NET, avec leur nom anglais comme repère. Sert à la
// liste des pays de naissance (page « Mon prénom ») et aux banques de prénoms par pays ; la page
// affiche les noms dans la langue du joueur avec Intl.DisplayNames (CLDR, dans le navigateur).
// Usage, depuis la racine du dépôt : dotnet fsi data/scripts/pays-du-monde.fsx
open System
open System.Globalization
open System.IO

// codes réservés ou « utilisateur » que CLDR accepte sans désigner un pays
let exclus = set [ "AA"; "QM"; "QN"; "QO"; "QP"; "QQ"; "QR"; "QS"; "QT"; "QU"; "QV"; "QW"; "QX"; "QY"; "QZ"; "XA"; "XB"; "ZZ"; "EU"; "EZ"; "UN"; "QO" ]

let pays =
    [ for a in 'A' .. 'Z' do
          for b in 'A' .. 'Z' do
              let code = string a + string b

              if not (exclus.Contains code) && (not (code.StartsWith "X") || code = "XK") then
                  match (try Some(RegionInfo code) with _ -> None) with
                  | Some r when r.TwoLetterISORegionName = code && r.EnglishName <> code -> yield code.ToLowerInvariant(), r.EnglishName
                  | _ -> () ]
    |> List.distinctBy fst
    |> List.sortBy fst

let echapper (s: string) = if s.Contains "," || s.Contains "\"" then "\"" + s.Replace("\"", "\"\"") + "\"" else s
File.WriteAllLines("data/pays-du-monde.csv", "code,nom_en" :: (pays |> List.map (fun (c, n) -> c + "," + echapper n)))
printfn "data/pays-du-monde.csv : %d pays" pays.Length
