/// data/ est valide et le F# généré est à jour : ce que `labo doctor` vérifie, en test.
module Labo.Tests.DataTests

open Expecto
open Labo.Cli

let tests =
    testList
        "data"
        [ test "data/ se valide sans erreur" {
              match Validate.charger (Repo.path [ "data" ]) with
              | Ok _ -> ()
              | Error e -> failtest (System.String.Join("\n", e |> List.map Schema.decrire))
          }
          test "le F# généré est à jour" {
              match Validate.charger (Repo.path [ "data" ]) with
              | Error _ -> failtest "data/ invalide"
              | Ok(d, _) ->
                  for (chemin, contenu) in Generate.sorties d do
                      Expect.equal (Generate.etat chemin contenu) Generate.AJour $"{chemin} (lancer `labo generate`)"
          }
          test "une condition qui vise une option inexistante est refusée, avec sa ligne" {
              let tmp = System.IO.Directory.CreateTempSubdirectory "labo-data-"

              try
                  let src = Repo.path [ "data" ]

                  for f in System.IO.Directory.GetFiles(src, "*.csv", System.IO.SearchOption.AllDirectories) do
                      let cible = System.IO.Path.Combine(tmp.FullName, System.IO.Path.GetRelativePath(src, f))
                      System.IO.Directory.CreateDirectory(System.IO.Path.GetDirectoryName cible) |> ignore
                      System.IO.File.Copy(f, cible)

                  let liens = System.IO.Path.Combine(tmp.FullName, "liens.csv")
                  let texte = System.IO.File.ReadAllText liens
                  System.IO.File.WriteAllText(liens, texte.Replace(",coord=plateforme,", ",coord=blockchain,"))

                  match Validate.charger tmp.FullName with
                  | Ok _ -> failtest "aurait dû échouer"
                  | Error e ->
                      let attendu (x: Schema.Erreur) =
                          match x with
                          | Schema.Reference("liens.csv", ligne, "quand", "coord=blockchain", "options.csv") -> ligne > 1
                          | _ -> false

                      Expect.exists e attendu (System.String.Join("\n", e |> List.map Schema.decrire))
              finally
                  tmp.Delete true
          }
          test "le F# compilé contient ce que disent les CSV" {
              match Validate.charger (Repo.path [ "data" ]) with
              | Error _ -> failtest "data/ invalide"
              | Ok(d, _) ->
                  // deux types distincts pour .NET (Types.fs est lié dans la CLI) : on compare leur rendu
                  let rendu v = sprintf "%A" v
                  Expect.equal (rendu Labo.Data.Liens.tous) (rendu d.liens) "liens"
                  Expect.equal (rendu Labo.Data.Leviers.options) (rendu d.options) "options"
                  Expect.equal (rendu Labo.Data.Sources.tous) (rendu d.sources) "sources"
          } ]
