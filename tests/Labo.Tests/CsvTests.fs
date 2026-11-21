module Labo.Tests.CsvTests

open Expecto
open Labo.Cli

let champs texte =
    match Csv.analyser texte with
    | Ok lignes -> lignes |> List.map (fun l -> l.numero, l.champs)
    | Error e -> failtest $"%A{e}"

let tests =
    testList
        "csv"
        [ test "champs simples, lignes vides ignorées" {
              Expect.equal (champs "a,b\n\n1,2\n") [ 1, [ "a"; "b" ]; 3, [ "1"; "2" ] ] ""
          }
          test "guillemets : virgule, guillemet doublé, saut de ligne" {
              let l = champs "a,b\n\"x, y\",\"il dit \"\"oui\"\"\"\n\"deux\nlignes\",z\n"
              Expect.equal l [ 1, [ "a"; "b" ]; 2, [ "x, y"; "il dit \"oui\"" ]; 3, [ "deux\nlignes"; "z" ] ] ""
          }
          test "le numéro de ligne suit les sauts de ligne cités" {
              let l = champs "a\n\"1\n2\"\n3\n"
              Expect.equal (l |> List.map fst) [ 1; 2; 4 ] ""
          }
          test "champs vides et fin sans saut de ligne" {
              Expect.equal (champs "a,b,c\n,,x") [ 1, [ "a"; "b"; "c" ]; 2, [ ""; ""; "x" ] ] ""
          }
          test "fins de ligne CRLF et BOM" { Expect.equal (champs "﻿a,b\r\n1,2\r\n") [ 1, [ "a"; "b" ]; 2, [ "1"; "2" ] ] "" }
          test "guillemet non fermé : erreur avec la ligne de début" {
              Expect.equal (Csv.analyser "a\nb\n\"c\n") (Error(Csv.GuillemetNonFerme 3)) ""
          }
          test "guillemet au milieu d'un champ : erreur" { Expect.equal (Csv.analyser "a\nb\"c\n") (Error(Csv.GuillemetDansChamp 2)) "" }
          test "citer, puis relire, rend le texte d'origine" {
              for v in [ "simple"; "a,b"; "il dit \"oui\""; "deux\nlignes"; "" ] do
                  // deux colonnes : une ligne d'un seul champ vide ne se distingue pas d'une ligne vide
                  Expect.equal (champs ("x,y\n" + Csv.citer v + ",z\n") |> List.last |> snd) [ v; "z" ] v
          } ]
