/// Les scènes : se déroulent jusqu'à un choix du joueur, chaque choix mène à sa suite, l'idée du
/// joueur entre au journal sans toucher au modèle.
module Labo.Tests.SceneTests

open Expecto
open Labo.Engine
open Labo.Jeu
open Labo.Jeu.Partie


let tests =
    testList
        "scène"
        [ test "la scène sur la monnaie se déroule jusqu'aux trois répliques du joueur" {
              let lignes, choix = Scene.rejouer "vivre.monnaie" []
              Expect.equal (lignes |> List.map _.id) [ "m1"; "m2"; "m3"; "m4"; "m5" ] "répliques"
              Expect.equal (choix |> List.map _.id) [ "j1"; "j2"; "j3" ] "choix"
          }
          test "chaque choix mène à sa réponse, puis à la fin de la scène" {
              for (choix, reponse) in [ "j1", "m6"; "j2", "m7"; "j3", "m8" ] do
                  let lignes, ouverts = Scene.rejouer "vivre.monnaie" [ choix ]
                  Expect.equal (lignes |> List.map _.id |> List.skip 5) [ choix; reponse; "m9" ] choix
                  Expect.isEmpty ouverts "scène finie"
          }
          test "un choix qui n'est pas proposé est ignoré" {
              let lignes, choix = Scene.rejouer "vivre.monnaie" [ "m9" ]
              Expect.equal (lignes |> List.map _.id).Length 5 "rien de plus"
              Expect.equal (choix |> List.map _.id) [ "j1"; "j2"; "j3" ] "les mêmes choix"
          }
          test "une situation sans scène n'a pas de répliques" {
              Expect.equal (Scene.rejouer "vivre.ouverture" []) ([], []) ""
          }
          test "l'idée du joueur entre au journal et ne change pas le modèle" {
              let p = match nouvelle "vivre" with Ok p -> p | Error e -> failtest $"%A{e}"
              let p2 = proposerIdee p "vivre.monnaie" "  Une monnaie locale par région, fondante.  "
              Expect.equal (idees p2) [ 0, "vivre.monnaie", "Une monnaie locale par région, fondante." ] "idée notée"
              Expect.equal (Moteur.evaluer p2.etat).score (Moteur.evaluer p.etat).score "modèle inchangé"
              Expect.equal (proposerIdee p "vivre.monnaie" "   ") p "une idée vide ne compte pas"
          } ]
