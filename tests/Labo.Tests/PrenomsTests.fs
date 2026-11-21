/// Les prénoms : tirés à chaque partie dans la banque du pays, selon l'âge et le sexe.
module Labo.Tests.PrenomsTests

open Expecto
open Labo.Jeu.Partie

let partie g =
    match nouvelleAvec g true [ "vivre" ] with
    | Ok p -> p
    | Error e -> failtest $"%A{e}"

let tests =
    testList
        "prénoms"
        [ test "chaque habitant reçoit un prénom de sa décennie de naissance et de son sexe" {
              let p = partie 11L

              for h in Labo.Data.Jeu.habitants do
                  let decennie = (int Labo.Data.Pays.Fr.anneeDepart - h.age) / 10 * 10
                  let n = p.prenoms[h.id]

                  let existe =
                      Labo.Data.Pays.Fr.prenoms |> List.exists (fun x -> x.prenom = n && x.decennie = decennie && (h.sexe = "x" || x.sexe = h.sexe))

                  Expect.isTrue existe $"{h.id} : {n}, années {decennie}"
          }
          test "les prénoms d'une partie sont tous différents" {
              for g in 1L .. 30L do
                  let noms = (partie g).prenoms |> Map.toList |> List.map snd
                  Expect.equal (List.distinct noms).Length noms.Length $"graine {g}"
          }
          test "la même graine donne les mêmes prénoms, d'autres graines en donnent d'autres" {
              Expect.equal (partie 5L).prenoms (partie 5L).prenoms "rejouable"
              let differents = [ 1L .. 20L ] |> List.map (fun g -> (partie g).prenoms["ainee"]) |> List.distinct
              Expect.isTrue (differents.Length > 5) $"variété : {differents}"
          }
          test "les marqueurs des textes prennent les prénoms de la partie" {
              let p = partie 3L
              let t = nommer p "{agriculteur} donne une tape sur l’épaule de {boulanger}."
              let a, b = p.prenoms["agriculteur"], p.prenoms["boulanger"]
              Expect.equal t (a + " donne une tape sur l’épaule de " + b + ".") ""
          } ]

let dans region g =
    match nouvelleDans region g true [ "vivre" ] with
    | Ok p -> p
    | Error e -> failtest $"%A{e}"

let testsRegions =
    testList
        "régions"
        [ test "en Bretagne, les prénoms viennent de la banque bretonne" {
              let p = dans "bretagne" 4L

              for h in Labo.Data.Jeu.habitants do
                  let decennie = (int Labo.Data.Pays.Fr.anneeDepart - h.age) / 10 * 10

                  let existe =
                      Labo.Data.Pays.Fr.prenoms
                      |> List.exists (fun x -> x.region = "bretagne" && x.prenom = p.prenoms[h.id] && x.decennie = decennie)

                  Expect.isTrue existe $"{h.id} : {p.prenoms[h.id]}"
          }
          test "à Mayotte, sans naissances anciennes dans la banque, le tirage reprend la banque nationale" {
              let p = dans "mayotte" 4L
              Expect.equal p.prenoms.Count Labo.Data.Jeu.habitants.Length "un prénom pour chacun"
          }
          test "une région inconnue est refusée" {
              Expect.isError (nouvelleDans "atlantide" 1L true [ "vivre" ]) ""
          }
          test "le code de partie garde la région" {
              let p = dans "corse" 9L
              match Labo.Jeu.Journal.decoder (Labo.Jeu.Journal.encoder p) with
              | Ok r ->
                  Expect.equal r.region "corse" "région"
                  Expect.equal r.prenoms p.prenoms "mêmes prénoms"
              | Error e -> failtest $"%A{e}"
          } ]
