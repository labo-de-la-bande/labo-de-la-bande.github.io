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
          test "la part des personnes nées à l'étranger suit l'âge (Insee)" {
              Expect.isTrue (partNeesAilleurs 16 < partNeesAilleurs 42) "plus rare chez les mineurs"
              for age in [ 5; 16; 27; 42; 56; 74; 90 ] do
                  let p = partNeesAilleurs age
                  Expect.isTrue (p > 0.0 && p < 0.3) $"{age} ans : {p}"
          }
          test "un habitant que le récit fait naître ici y est toujours né" {
              for g in 1L .. 50L do
                  let p = partie g

                  for h in Labo.Data.Jeu.habitants |> List.filter (fun h -> h.naissance = "ici") do
                      Expect.equal p.origines[h.id] "ici" $"{h.id}, graine {g}"
          }
          test "sur beaucoup de parties, la part des habitants nés à l'étranger suit l'Insee" {
              let h = Labo.Data.Jeu.habitants |> List.find (fun h -> h.naissance = "tirée" && h.age >= 18)
              let n = 400
              let ailleurs = [ 1L .. int64 n ] |> List.filter (fun g -> (partie (g * 104729L)).origines[h.id] <> "ici") |> List.length
              let attendu = partNeesAilleurs h.age * float n
              Expect.isTrue (abs (float ailleurs - attendu) < 0.5 * attendu) $"{h.id} : {ailleurs} sur {n}, attendu vers {attendu}"
          }
          test "né·e dans un groupe de pays qui a ses banques, le prénom vient de ces banques" {
              let mutable vus = 0

              for g in 1L .. 400L do
                  let p = partie (g * 104729L)

                  for h in Labo.Data.Jeu.habitants do
                      match Labo.Data.Pays.Fr.origines |> List.tryFind (fun o -> o.id = p.origines[h.id]) with
                      | Some o when o.pays |> List.exists (fun c -> Labo.Data.Prenoms.prenoms |> List.exists (fun x -> x.pays = c)) ->
                          vus <- vus + 1
                          let dans = Labo.Data.Prenoms.prenoms |> List.exists (fun x -> List.contains x.pays o.pays && x.prenom = p.prenoms[h.id])
                          Expect.isTrue dans $"{h.id} né·e ({o.id}) : {p.prenoms[h.id]}"
                      | _ -> ()

              Expect.isTrue (vus > 0) "au moins un cas"
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

let testsUsage =
    testList
        "usage des prénoms déclarés"
        [ test "un prénom donné aux deux convient à tous, un prénom « je ne sais pas » seulement à un personnage sans sexe fixé" {
              Expect.isTrue (usageConvient "f" "fm") "aux deux"
              Expect.isTrue (usageConvient "m" "m") "aux garçons"
              Expect.isFalse (usageConvient "f" "m") "pas l'inverse"
              Expect.isFalse (usageConvient "m" "x") "je ne sais pas"
              Expect.isTrue (usageConvient "x" "x") "personnage sans sexe fixé"
              Expect.isTrue (usageConvient "x" "f") "personnage sans sexe fixé, tout convient"
          } ]
