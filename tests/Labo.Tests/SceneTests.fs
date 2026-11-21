/// Les scènes : se déroulent jusqu'à un choix du joueur, chaque choix mène à sa suite, l'idée du
/// joueur entre au journal sans toucher au modèle.
module Labo.Tests.SceneTests

open Expecto
open Labo.Engine
open Labo.Jeu
open Labo.Jeu.Partie


let ctx: Scene.Contexte = { graine = 1L; confiance = Map.empty }

let tests =
    testList
        "scène"
        [ test "la scène sur la monnaie se déroule jusqu'aux trois répliques du joueur" {
              let lignes, choix = Scene.rejouer ctx "vivre.monnaie" Labo.Data.Avant None []
              Expect.equal (lignes |> List.map _.id) [ "m1"; "m2"; "m3"; "m4"; "m5" ] "répliques"
              Expect.equal (choix |> List.map _.id) [ "j1"; "j2"; "j3" ] "choix"
          }
          test "chaque choix mène à sa réponse, puis à la fin de la scène" {
              for (choix, reponse) in [ "j1", "m6"; "j2", "m7"; "j3", "m8" ] do
                  let lignes, ouverts = Scene.rejouer ctx "vivre.monnaie" Labo.Data.Avant None [ choix ]
                  Expect.equal (lignes |> List.map _.id |> List.skip 5) [ choix; reponse; "m9" ] choix
                  Expect.isEmpty ouverts "scène finie"
          }
          test "un choix qui n'est pas proposé est ignoré" {
              let lignes, choix = Scene.rejouer ctx "vivre.monnaie" Labo.Data.Avant None [ "m9" ]
              Expect.equal (lignes |> List.map _.id).Length 5 "rien de plus"
              Expect.equal (choix |> List.map _.id) [ "j1"; "j2"; "j3" ] "les mêmes choix"
          }
          test "une situation sans scène n'a pas de répliques" {
              Expect.equal (Scene.rejouer ctx "foyers.logement" Labo.Data.Avant None []) ([], []) ""
          }
          test "l'idée du joueur entre au journal et ne change pas le modèle" {
              let p = match nouvelle "vivre" with Ok p -> p | Error e -> failtest $"%A{e}"
              let p2 = proposerIdee p "vivre.monnaie" "  Une monnaie locale par région, fondante.  "
              Expect.equal (idees p2) [ 0, "vivre.monnaie", "Une monnaie locale par région, fondante." ] "idée notée"
              Expect.equal (Moteur.evaluer p2.etat).score (Moteur.evaluer p.etat).score "modèle inchangé"
              Expect.equal (proposerIdee p "vivre.monnaie" "   ") p "une idée vide ne compte pas"
          } ]

let partie graine =
    match nouvelleAvec graine true [ "vivre" ] with
    | Ok p -> p
    | Error e -> failtest $"%A{e}"

let reaction (p: Partie) option =
    Scene.rejouer (contexteScene p) "vivre.monnaie" Labo.Data.Apres (Some option) [] |> fst |> List.map _.id

let testsSuite =
    testList
        "scène, suite"
        [ test "chaque option de la monnaie a sa réaction" {
              for o in [ "sans"; "frontiere"; "bons"; "fondante"; "plafond" ] do
                  Expect.isTrue (Scene.aUneReaction "vivre.monnaie" o) o
          }
          test "le tirage d'une réplique est rejouable avec la même graine" {
              for g in [ 1L; 2L; 3L; 42L ] do
                  Expect.equal (reaction (partie g) "bons") (reaction (partie g) "bons") $"graine {g}"
          }
          test "selon la graine, Sam ne dit pas toujours la même chose" {
              let premieres = [ 1L .. 20L ] |> List.map (fun g -> reaction (partie g) "bons" |> List.head) |> List.distinct
              Expect.equal (List.sort premieres) [ "b1"; "b1b" ] "les deux variantes sortent"
          }
          test "Jeanne se souvient : avec sa confiance, elle remercie le joueur" {
              let p = partie 5L
              let sansConfiance = reaction p "sans" |> List.head
              let p2 = { p with reputation = p.reputation.Add("jeanne", 1.0) }
              Expect.equal (reaction p2 "sans" |> List.head) "s1" "la réplique conditionnée l'emporte"
              Expect.equal sansConfiance "s1b" "sans confiance, l'autre réplique"
          }
          test "une réplique du joueur change la confiance et entre au journal" {
              let p = partie 1L
              let p2 = repondre p "vivre.monnaie" Scene.repliqueDe["s3"]
              Expect.equal p2.reputation["malik"] (p.reputation["malik"] + 0.5) "Malik +0,5"
              Expect.isTrue (p2.journal |> List.exists (function Repondu(_, _, "s3") -> true | _ -> false)) "au journal"
          }
          test "le mode apprentissage indique l'option qui remonte le plus le maillon faible" {
              let p = partie 1L
              let p = match decider p "vivre.monnaie" [ { levier = "monnaie"; option = "sans" } ] with Ok p -> p | Error e -> failtest $"%A{e}"
              match conseilApprentissage p "vivre.ouverture" with
              | Some c ->
                  Expect.equal c.jauge (Some "ressources") "le maillon faible au départ"
                  Expect.equal c.choix [ { levier = "ouverture"; option = "ouvert" } ] "ouvrir couvre le plus de ressources"
                  Expect.isTrue (c.gain > 0.0) "un gain"
              | None -> failtest "un conseil attendu"
          } ]

let testsAttitudes =
    testList
        "attitudes"
        [ test "l'humeur découle de ce que l'habitant avait dit" {
              let s = situationDe["vivre.monnaie"]
              let choix = [ { levier = "monnaie"; option = "sans" } ]
              Expect.equal (humeurDe s choix "jeanne") Labo.Data.Approuve "Jeanne soutenait « sans monnaie »"
              Expect.equal (humeurDe s choix "malik") Labo.Data.Decu "Malik soutenait la monnaie à la frontière"
              Expect.equal (humeurDe s choix "sam") Labo.Data.Neutre "Sam était réservé sur les bons, écartés"
              Expect.equal (humeurDe s choix "aicha") Labo.Data.Neutre "Aïcha n'avait rien dit"
              Expect.equal (humeurDe s [ { levier = "monnaie"; option = "bons" } ] "sam") Labo.Data.Dubitatif "réservé sur l'option choisie"
          }
          test "ceux qui parlent dans la scène n'ont pas de geste en plus, les autres si" {
              let p = partie 3L
              let s = situationDe["vivre.monnaie"]
              let lot = attitudes p s [ { levier = "monnaie"; option = "sans" } ] (set [ "jeanne"; "malik" ])
              let qui = lot |> List.map (fun (h, _, _) -> h.id) |> Set.ofList
              Expect.equal qui (set [ "sam"; "ines"; "bruno"; "aicha" ]) "les quatre autres"
          }
          test "le geste est rejouable avec la même graine" {
              let s = situationDe["vivre.monnaie"]
              let geste g = attitudes (partie g) s [ { levier = "monnaie"; option = "sans" } ] Set.empty |> List.map (fun (_, _, a) -> a.texte)
              Expect.equal (geste 9L) (geste 9L) "même graine, mêmes gestes"
          } ]

/// Tous les chemins d'une scène, en choisissant chaque réplique du joueur tour à tour.
let rec chemins ctx situation moment option (choisis: string list) =
    let lignes, ouverts = Scene.rejouer ctx situation moment option choisis

    match ouverts with
    | [] -> [ lignes ]
    | _ -> ouverts |> List.collect (fun o -> chemins ctx situation moment option (choisis @ [ o.id ]))

let testsCompletude =
    testList
        "scènes complètes"
        [ test "chaque chemin d'une scène avant la décision mène à la décision" {
              let ctx: Scene.Contexte = { graine = 1L; confiance = Map.empty }

              for s in Labo.Data.Jeu.situations do
                  match Scene.entree ctx s.id Labo.Data.Avant None with
                  | None -> ()
                  | Some _ ->
                      for lignes in chemins ctx s.id Labo.Data.Avant None [] do
                          let fin = List.last lignes
                          Expect.equal fin.action (Some Labo.Data.Decider) $"{s.id} : le chemin finit par {fin.id}"
          }
          test "une situation qui a une scène a une réaction pour chacune de ses options" {
              let ctx: Scene.Contexte = { graine = 1L; confiance = Map.empty }

              for s in Labo.Data.Jeu.situations do
                  if (Scene.entree ctx s.id Labo.Data.Avant None).IsSome then
                      for l in s.leviers do
                          for o in Labo.Engine.Tables.optionsDe[l] do
                              Expect.isTrue (Scene.aUneReaction s.id o.id) $"{s.id} : pas de réaction à {o.id}"
          } ]
