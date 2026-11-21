/// Le mouvement dans le temps : effets qui durent, décisions qui s'installent, paris qui
/// mûrissent, événements rejouables.
module Labo.Tests.TempsTests

open Expecto
open Labo.Engine
open Labo.Jeu
open Labo.Jeu.Partie

let ok r =
    match r with
    | Ok v -> v
    | Error e -> failtest $"%A{e}"

let sansHasard () = ok (nouvelleAvec 1L false [ "vivre" ])

/// Règle les décisions ouvertes avec les choix donnés (situation → choix), puis finit la révolution.
let rec jouerJusqua (choix: Map<string, Choix list>) n p =
    if n = 0 then
        p
    else
        let p =
            ouvertes p
            |> List.filter estDecision
            |> List.fold (fun p s -> ok (decider p s.id (choix.TryFind s.id |> Option.defaultValue [ { levier = "paris"; option = "aucun" } ]))) p

        jouerJusqua choix (n - 1) (ok (finirRevolution p))

let defaut =
    Map
        [ "vivre.monnaie", [ { levier = "monnaie"; option = "sans" } ]
          "vivre.ouverture", [ { levier = "ouverture"; option = "cible" } ]
          "vivre.energie", [ { levier = "energie"; option = "nucleaire-stocks" } ]
          "vivre.engrais", [ { levier = "engrais"; option = "organique" } ]
          "vivre.coord", [ { levier = "coord"; option = "main" } ]
          "vivre.relais", [ { levier = "relais"; option = "relais" } ] ]

let tests =
    testList
        "temps"
        [ test "un effet du temps s'ajoute à la jauge comme un lien" {
              let e = Tables.etatDeDepart
              let e2 = { e with c = e.c.Add("effets", Obj(Map [ "egalite", Num 5.0 ])) }
              let v (e: Etat) = ((Moteur.evaluer e).jauges |> List.find (fun j -> j.id = "egalite")).valeur.Milieu
              Expect.floatClose Accuracy.high (v e2 - v e) 0.05 "+5 points"
          }
          test "une décision s'installe : la moitié aussitôt, tout à la révolution suivante" {
              let p = sansHasard ()
              let choix = [ { levier = "ouverture"; option = "cible" } ]
              let a = ok (apercu p "vivre.ouverture" choix)
              Expect.isTrue (a.aussitot.score > a.avant.score && a.aussitot.score < a.aTerme.score) "aussitôt entre avant et à terme"
              let p = ok (decider p "vivre.ouverture" choix)
              let p = ok (decider p "vivre.monnaie" [ { levier = "monnaie"; option = "sans" } ])
              let p = ok (finirRevolution p)
              Expect.isEmpty p.effets "plus d'effet en cours"
              Expect.floatClose Accuracy.medium (Moteur.evaluer p.etat).score a.aTerme.score "à terme"
          }
          test "un pari mûrit le nombre d'années qu'il demande après avoir été choisi" {
              let p = jouerJusqua defaut 3 (sansHasard ())
              Expect.isTrue (estOuverte p "vivre.paris") "le pari se présente"
              let p = ok (decider p "vivre.paris" [ { levier = "paris"; option = "iso" } ])
              let choisiA = p.revolution
              let p = jouerJusqua defaut 10 p
              let muri = p.journal |> List.tryPick (function Muri(r, "iso") -> Some r | _ -> None)
              Expect.equal muri (Some(choisiA + 10)) "l'isolation demande 10 ans"
          }
          test "un pari choisi n'agit pas avant d'être prêt" {
              let p = jouerJusqua defaut 3 (sansHasard ())
              let p = ok (decider p "vivre.paris" [ { levier = "paris"; option = "iso" } ])
              let iso = Labo.Data.Modele.paris |> List.find (fun b -> b.id = "iso")
              Expect.isFalse (Moteur.pariActif p.etat iso) "pas encore"
          }
          test "un événement certain survient quand ses conditions sont réunies, une seule fois" {
              let choix =
                  defaut
                  |> Map.add "vivre.ouverture" [ { levier = "ouverture"; option = "ferme" } ]
                  |> Map.add "vivre.engrais" [ { levier = "engrais"; option = "moitie" } ]

              let p = jouerJusqua choix 6 (sansHasard ())
              let fois = p.journal |> List.filter (function Survenu(_, "engrais-manquent") -> true | _ -> false) |> List.length
              Expect.equal fois 1 "les usines d'engrais s'arrêtent une fois"
          }
          test "sans hasard, rien d'incertain ne survient" {
              let p = jouerJusqua defaut 8 (sansHasard ())

              let incertains =
                  p.journal
                  |> List.choose (function Survenu(_, id) -> Some id | _ -> None)
                  |> List.filter (fun id -> (Labo.Data.Jeu.evenements |> List.find (fun ev -> ev.id = id)).probabilite < 1.0)

              Expect.isEmpty incertains "aucun événement probable"
          }
          test "la même graine rejoue la même partie" {
              let jouer () = jouerJusqua defaut 12 (ok (nouvelleAvec 42L true [ "vivre" ]))
              Expect.equal (jouer ()).journal (jouer ()).journal "même journal"
          }
          test "avec du hasard, des événements finissent par survenir" {
              let p = jouerJusqua defaut 20 (ok (nouvelleAvec 7L true [ "vivre" ]))
              Expect.isTrue (p.journal |> List.exists (function Survenu _ -> true | _ -> false)) "au moins un événement en 20 révolutions"
          } ]
