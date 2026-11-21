module Labo.Tests.DslTests

open Expecto
open Labo.Data
open Labo.Cli.Validate

let ok r =
    match r with
    | Ok v -> v
    | Error e -> failtest e

let tests =
    testList
        "mini-langage des liens"
        [ test "quand vide : toujours" { Expect.equal (ok (analyserQuand "")) [] "" }
          test "conjonction, alternatives, négation, n'importe quelle option" {
              Expect.equal
                  (ok (analyserQuand "coord=plateforme|ia & relais!=relais & accueil=*"))
                  [ { levier = "coord"; egal = true; options = [ "plateforme"; "ia" ] }
                    { levier = "relais"; egal = false; options = [ "relais" ] }
                    { levier = "accueil"; egal = true; options = [] } ]
                  ""
          }
          test "quand mal formé : refusé" {
              for t in [ "coord"; "coord=="; "Coord=ia"; "coord=ia &"; "coord=a||b" ] do
                  Expect.isError (analyserQuand t) t
          }
          test "poids" {
              Expect.equal (ok (analyserPoids "")) Plein ""
              Expect.equal (ok (analyserPoids "@risque-gang")) (Facteur "risque-gang") ""

              Expect.equal
                  (ok (analyserPoids "monnaie!=sans:0.5"))
                  (Si([ { levier = "monnaie"; egal = false; options = [ "sans" ] } ], 0.5))
                  ""

              Expect.isError (analyserPoids "@inconnu") ""
              Expect.isError (analyserPoids "monnaie=sans:demi") ""
          }
          test "courbes : nom connu du moteur et bon nombre d'arguments" {
              Expect.equal (ok (analyserCourbe "socle(8,5)")) (Some { nom = "socle"; args = [ "8"; "5" ] }) ""
              Expect.equal (ok (analyserCourbe "mandat-court")) (Some { nom = "mandat-court"; args = [] }) ""
              Expect.isError (analyserCourbe "socle(8)") ""
              Expect.isError (analyserCourbe "inventee") ""
          }
          test "écritures d'une option" {
              Expect.equal (ok (analyserEcritures "s.supply=829; s.mwh=11.9")) [ Champ("s.supply", "829"); Champ("s.mwh", "11.9") ] ""
              Expect.equal (ok (analyserEcritures "@paliers")) [ Nomme "paliers" ] ""
              Expect.isError (analyserEcritures "supply=829") ""
          } ]
