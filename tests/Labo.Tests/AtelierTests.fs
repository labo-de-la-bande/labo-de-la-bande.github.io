/// L'atelier des données : les garde-fous d'une modification, et la fusion à trois.
module Labo.Tests.AtelierTests

open Expecto
open Labo.Relais.Atelier
open Labo.Cli.AtelierCli

let types = tables |> List.find (fun t -> t.chemin = "data/jeu/types-partie.csv")

let texte =
    "id,titre_fr,description_fr,interdits,provenance\n"
    + "sans-prison,\"Sans prison, ni police\",Le départ.,police|surveillance,auteur\n"
    + "autre,Autre,Une autre.,police,auteur\n"

let ok r =
    match r with
    | Ok v -> v
    | Error e -> failtestf "attendu Ok, obtenu : %s" e

let erreur r =
    match r with
    | Ok _ -> failtest "attendu une erreur"
    | Error (e: string) -> e

let tests =
    testList
        "atelier"
        [ test "lire puis écrire rend le même texte" {
              Expect.equal (ecrire (ok (lire texte))) texte "aller-retour"
          }
          test "modifier un champ : la ligne change, les autres gardent leur texte, la provenance devient l'invité" {
              let nouveau, resume = ok (appliquer types texte "camille" (Modifier("autre", [ "interdits", "police|force-armee" ], [ "interdits", "police" ])))
              Expect.stringContains nouveau "autre,Autre,Une autre.,police|force-armee,camille" "ligne modifiée"
              Expect.stringContains nouveau "sans-prison,\"Sans prison, ni police\",Le départ.,police|surveillance,auteur" "ligne intacte"
              Expect.equal resume "autre : interdits" "résumé"
          }
          test "refusé : une colonne fermée, la provenance, l'identifiant" {
              Expect.stringContains (erreur (appliquer types texte "camille" (Modifier("autre", [ "id", "x" ], [])))) "ne se modifie pas" "id"
              Expect.stringContains (erreur (appliquer types texte "camille" (Modifier("autre", [ "provenance", "auteur" ], [])))) "ne se modifie pas" "provenance"
          }
          test "refusé : un retour à la ligne, un texte trop long, un invité mal nommé" {
              Expect.stringContains (erreur (appliquer types texte "camille" (Modifier("autre", [ "titre_fr", "a\nb" ], [])))) "une seule ligne" "retour à la ligne"
              Expect.stringContains (erreur (appliquer types texte "camille" (Modifier("autre", [ "titre_fr", String.replicate 1001 "a" ], [])))) "trop long" "longueur"
              Expect.isError (appliquer types texte "Camille" (Modifier("autre", [ "titre_fr", "b" ], []))) "invité"
          }
          test "refusé : la ligne a changé depuis que l'invité l'a lue" {
              Expect.stringContains (erreur (appliquer types texte "camille" (Modifier("autre", [ "interdits", "x" ], [ "interdits", "autre-chose" ])))) "entre-temps" "avant"
          }
          test "ajouter une ligne : identifiant neuf et bien formé" {
              let nouveau, _ = ok (appliquer types texte "camille" (Ajouter [ "id", "sans-armee"; "titre_fr", "Sans armée"; "interdits", "force-armee" ]))
              Expect.stringEnds nouveau "sans-armee,Sans armée,,force-armee,camille\n" "ajoutée à la fin"
              Expect.stringContains (erreur (appliquer types texte "camille" (Ajouter [ "id", "autre" ]))) "existe déjà" "doublon"
              Expect.isError (appliquer types texte "camille" (Ajouter [ "id", "Pas Bon" ])) "forme"
          }
          test "un guillemet dans une valeur est échappé" {
              let nouveau, _ = ok (appliquer types texte "camille" (Modifier("autre", [ "titre_fr", "Le \"vrai\", enfin" ], [])))
              Expect.stringContains nouveau "autre,\"Le \"\"vrai\"\", enfin\",Une autre.,police,camille" "RFC 4180"
          }
          test "la fusion verse les changements de l'atelier et garde ceux d'ici" {
              let atelier, _ = ok (appliquer types texte "camille" (Modifier("autre", [ "interdits", "police|force-armee" ], [])))
              let local = texte.Replace("Le départ.", "Le point de départ.")
              let f = ok (fusionner3 types texte local atelier)
              Expect.stringContains f.texte "Le point de départ." "changement d'ici"
              Expect.stringContains f.texte "police|force-armee,camille" "changement de l'atelier"
              Expect.isEmpty f.conflits "aucun conflit"
              Expect.equal f.changements [ "autre : modifiée par camille" ] "changements"
          }
          test "la fusion : un même champ changé des deux côtés reste local, et c'est dit" {
              let atelier, _ = ok (appliquer types texte "camille" (Modifier("autre", [ "interdits", "force-armee" ], [])))
              let local = texte.Replace(",police,auteur", ",prison,auteur")
              let f = ok (fusionner3 types texte local atelier)
              Expect.stringContains f.texte "autre,Autre,Une autre.,prison,auteur" "la version d'ici"
              Expect.hasLength f.conflits 1 "un conflit"
          }
          test "la fusion : une ligne ajoutée dans l'atelier arrive à la fin" {
              let atelier, _ = ok (appliquer types texte "camille" (Ajouter [ "id", "sans-armee"; "interdits", "force-armee" ]))
              let f = ok (fusionner3 types texte texte atelier)
              Expect.stringEnds f.texte "sans-armee,,,force-armee,camille\n" "ajoutée"
          }
          test "les tables de l'atelier existent dans data/ avec leur en-tête" {
              for t in tables do
                  let lignes = ok (lire (System.IO.File.ReadAllText(Labo.Cli.Repo.path (t.chemin.Split '/' |> List.ofArray))))
                  Expect.equal (List.head lignes).champs t.entete t.chemin
          }
          test "dans data/, chaque ligne d'une table de l'atelier a une clé unique, et ses textes se modifient sans être refusés" {
              for t in tables do
                  let lignes = ok (lire (System.IO.File.ReadAllText(Labo.Cli.Repo.path (t.chemin.Split '/' |> List.ofArray)))) |> List.tail
                  let cles = lignes |> List.map (fun l -> cleDe t l.champs)
                  Expect.equal (List.distinct cles).Length cles.Length $"{t.chemin} : clés uniques"

                  for l in lignes do
                      for col in t.modifiables do
                          let v = l.champs[t.entete |> List.findIndex ((=) col)]
                          Expect.isOk (valeurPropre v) $"{t.chemin} {cleDe t l.champs} {col}"
          }
          test "une table de textes : on modifie par la clé composée, on n'ajoute pas" {
              let repliques = tables |> List.find (fun t -> t.chemin = "data/jeu/repliques.csv")
              let entete = String.concat "," repliques.entete

              let texte =
                  entete + "\nvivre.monnaie,m1,0,avant,,ainee,dit,\"Au village, on portait du bois.\",,,m2,,,,assistant\n"
                  + "foyers.logement,m1,0,avant,,ado,dit,Autre.,,,,,,,auteur\n"

              let nouveau, resume = ok (appliquer repliques texte "camille" (Modifier("vivre.monnaie/m1", [ "texte_fr", "Au village, on partageait." ], [ "texte_fr", "Au village, on portait du bois." ])))
              Expect.stringContains nouveau "vivre.monnaie,m1,0,avant,,ainee,dit,\"Au village, on partageait.\",,,m2,,,,camille" "la bonne ligne"
              Expect.stringContains nouveau "foyers.logement,m1,0,avant,,ado,dit,Autre.,,,,,,,auteur" "l'autre m1 intacte"
              Expect.equal resume "vivre.monnaie/m1 : texte_fr" "résumé"
              Expect.stringContains (erreur (appliquer repliques texte "camille" (Modifier("vivre.monnaie/m1", [ "locuteur", "ado" ], [])))) "ne se modifie pas" "colonne de structure"
              Expect.stringContains (erreur (appliquer repliques texte "camille" (Ajouter [ "situation", "x"; "texte_fr", "y" ]))) "pas de ligne nouvelle" "pas d'ajout"
          }
          test "un code d'invitation : préfixe et 256 bits" {
              Expect.isTrue (formeCode ("atl_" + String.replicate 43 "a")) "bon"
              Expect.isFalse (formeCode ("atl_" + String.replicate 42 "a")) "court"
              Expect.isFalse (formeCode ("atl_" + String.replicate 42 "a" + "/")) "caractère"
          } ]
