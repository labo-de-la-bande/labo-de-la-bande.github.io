/// Le journal d'actions : rejouer redonne la même partie, le code de partie aussi.
module Labo.Tests.JournalTests

open Expecto
open Labo.Jeu
open Labo.Jeu.Partie
open Labo.Jeu.Journal

let ok r =
    match r with
    | Ok v -> v
    | Error e -> failtest $"%A{e}"

let config = { region = "france"; graine = 77L; hasard = true; parcours = parcoursComplet }

let actions =
    [ Repondre("vivre.monnaie", "j1")
      Decider("vivre.monnaie", [ { levier = "monnaie"; option = "sans" } ])
      Repondre("vivre.monnaie", "s3")
      ProposerIdee("vivre.ouverture", "Une coopérative d’import par région | avec des : séparateurs ~")
      Decider("vivre.ouverture", [ { levier = "ouverture"; option = "cible" } ])
      RevenirSur "vivre.ouverture"
      Decider("vivre.ouverture", [ { levier = "ouverture"; option = "limite" } ])
      FinirRevolution
      Decider("vivre.energie", [ { levier = "energie"; option = "renouvelables" } ])
      Decider("vivre.engrais", [ { levier = "engrais"; option = "organique" } ])
      FinirRevolution ]

/// La partie jouée action par action, comme le ferait l'interface.
let jouee () =
    actions |> List.fold (fun p a -> ok (jouer a p)) (ok (commencer config))

let tests =
    testList
        "journal"
        [ test "rejouer les actions redonne exactement la même partie" {
              let p = jouee ()
              let r = ok (rejouer config actions)
              Expect.equal r p "même partie"
              Expect.equal r.actions actions "les actions sont au journal"
          }
          test "le code de partie redonne la même partie, idées comprises" {
              let p = jouee ()
              let code = encoder p
              let r = ok (decoder code)
              Expect.equal r p "même partie"
              Expect.equal (idees r) (idees p) "même idée, séparateurs compris"
          }
          test "un code illisible est refusé avec une raison" {
              Expect.isError (decoder "bonjour") "pas un code"
              Expect.isError (decoder "labo1|france|abc|1|vivre") "graine illisible"
          }
          test "une action impossible est refusée et nommée" {
              match rejouer config [ Decider("vivre.coord", [ { levier = "coord"; option = "main" } ]) ] with
              | Error(ActionRefusee(0, _, _)) -> ()
              | autre -> failtest $"%A{autre}"
          } ]

let testsRegistre =
    testList
        "registre des manques"
        [ test "l'option du ticket n° 1 se choisit et change le modèle" {
              let avant =
                  [ Decider("vivre.monnaie", [ { levier = "monnaie"; option = "sans" } ])
                    Decider("vivre.ouverture", [ { levier = "ouverture"; option = "limite" } ])
                    FinirRevolution
                    Decider("vivre.energie", [ { levier = "energie"; option = "renouvelables" } ])
                    Decider("vivre.engrais", [ { levier = "engrais"; option = "organique" } ])
                    FinirRevolution ]

              let p = ok (rejouer config avant)
              Expect.contains (ouvertes p |> List.map _.id) "vivre.coord" "la question est ouverte"
              let p2 = ok (jouer (Decider("vivre.coord", [ { levier = "coord"; option = "registre" } ])) p)
              Expect.equal (Labo.Engine.Moteur.est "coord" "registre" p2.etat) true "le registre est choisi"
          } ]
