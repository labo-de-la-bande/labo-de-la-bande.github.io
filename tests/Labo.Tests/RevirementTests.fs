/// Revenir sur une décision : la dernière de la révolution se reprend, une plus ancienne se
/// reconsidère ; dans les deux cas, la confiance garde la trace du revirement.
module Labo.Tests.RevirementTests

open Expecto
open Labo.Engine
open Labo.Jeu
open Labo.Jeu.Partie

let ok r =
    match r with
    | Ok v -> v
    | Error e -> failtest $"%A{e}"

let depart () = ok (nouvelleAvec 1L false [ "vivre" ])
let monnaie o = [ { levier = "monnaie"; option = o } ]

let tests =
    testList
        "revirement"
        [ test "reprendre la dernière décision rend le modèle d'avant et rouvre la situation" {
              let p = depart ()
              let p1 = ok (decider p "vivre.ouverture" [ { levier = "ouverture"; option = "cible" } ])
              let p2 = ok (revenirSur p1 "vivre.ouverture")
              Expect.equal (Moteur.evaluer p2.etat).score (Moteur.evaluer p.etat).score "score d'avant"
              Expect.isTrue (estOuverte p2 "vivre.ouverture") "la situation se rouvre"
              Expect.equal (revirements p2) 1 "au journal"
          }
          test "ceux qui approuvaient perdent plus qu'ils n'avaient gagné" {
              let p = depart ()
              let p1 = ok (decider p "vivre.ouverture" [ { levier = "ouverture"; option = "cible" } ])
              let p2 = ok (revenirSur p1 "vivre.ouverture")
              Expect.isTrue (p2.reputation["agriculteur"] < p.reputation["agriculteur"]) "Bruno, qui voulait les échanges ciblés, finit plus bas qu'au départ"
          }
          test "ceux qui redoutaient la décision regagnent moins qu'ils n'avaient perdu" {
              let p = depart ()
              let p1 = ok (decider p "vivre.ouverture" [ { levier = "ouverture"; option = "ferme" } ])
              let p2 = ok (revenirSur p1 "vivre.ouverture")
              Expect.isTrue (p2.reputation["ainee"] < p.reputation["ainee"] && p2.reputation["ainee"] > p1.reputation["ainee"]) "Jeanne, qui redoutait la fermeture"
          }
          test "une décision ancienne se reconsidère : elle revient et prend une place de la révolution" {
              let p = depart ()
              let p = ok (decider p "vivre.monnaie" (monnaie "sans"))
              let p = ok (decider p "vivre.ouverture" [ { levier = "ouverture"; option = "cible" } ])
              let p = ok (finirRevolution p)
              let p = ok (revenirSur p "vivre.monnaie")
              let decisions = ouvertes p |> List.filter estDecision |> List.map _.id
              Expect.equal decisions [ "vivre.monnaie"; "vivre.energie" ] "la monnaie d'abord, une seule nouvelle"
              let p = ok (decider p "vivre.monnaie" (monnaie "plafond"))
              Expect.equal (Etat.texte "monnaie" p.etat) (Some "plafond") "le nouveau choix s'applique à l'état présent"
              Expect.equal (dernierChoix p "vivre.monnaie") (Some(monnaie "plafond")) "le journal le retient"
          }
          test "on ne revient pas sur ce qui n'a pas été décidé" {
              Expect.isError (revenirSur (depart ()) "vivre.coord") ""
          } ]
