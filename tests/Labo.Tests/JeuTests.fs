/// Le moteur de jeu : appliquer une option comme l'ancien jeu, et dérouler une partie.
module Labo.Tests.JeuTests

open System.IO
open System.Text.Json
open Expecto
open Labo.Engine
open Labo.Jeu
open Labo.Jeu.Partie
open Labo.Cli

let ok r =
    match r with
    | Ok v -> v
    | Error e -> failtest $"%A{e}"

/// Le rendu d'une valeur de l'état comme dans oracle/options.json.
let rec jsonDe (v: Val) =
    match v with
    | Num n -> if n = floor n then string (int64 n) else n.ToString(System.Globalization.CultureInfo.InvariantCulture)
    | Txt t -> JsonSerializer.Serialize t
    | Bool b -> if b then "true" else "false"
    | Obj m -> "{" + System.String.Join(",", m |> Map.toList |> List.map (fun (k, x) -> JsonSerializer.Serialize k + ":" + jsonDe x)) + "}"
    | Nul -> "null"

/// Ce qui a changé entre deux états, champ par champ, au format de l'oracle.
let difference (a: Etat) (b: Etat) =
    let cles (m: Map<string, _>) = m |> Map.keys |> Set.ofSeq

    [ for k in Set.union (cles a.c) (cles b.c) do
          match a.c.TryFind k, b.c.TryFind k with
          | x, Some y when x <> Some y -> yield "c." + k, jsonDe y
          | _ -> ()
      for k in Set.union (cles a.s) (cles b.s) do
          match a.s.TryFind k, b.s.TryFind k with
          | x, Some y when x <> Some y -> yield "s." + k, jsonDe (Num y)
          | _ -> () ]
    |> Map.ofList

let rec normaliser (j: JsonElement) =
    match j.ValueKind with
    | JsonValueKind.Object ->
        "{" + System.String.Join(",", j.EnumerateObject() |> Seq.sortBy _.Name |> Seq.map (fun p -> JsonSerializer.Serialize p.Name + ":" + normaliser p.Value)) + "}"
    | JsonValueKind.Number -> let n = j.GetDouble() in if n = floor n then string (int64 n) else n.ToString(System.Globalization.CultureInfo.InvariantCulture)
    | _ -> j.GetRawText()

let tests =
    testList
        "jeu"
        [ test "chaque option écrit dans l'état ce qu'écrivait l'ancien jeu" {
              use doc = JsonDocument.Parse(File.ReadAllText(Repo.path [ "oracle"; "options.json" ]))

              let fautes =
                  [ for o in doc.RootElement.GetProperty("options").EnumerateArray() do
                        let levier, option = o.GetProperty("levier").GetString(), o.GetProperty("option").GetString()
                        let attendu = o.GetProperty("ecrit").EnumerateObject() |> Seq.map (fun p -> p.Name, normaliser p.Value) |> Map.ofSeq
                        let obtenu = difference Tables.etatDeDepart (ok (Options.appliquer levier option Tables.etatDeDepart))
                        if attendu <> obtenu then yield $"{levier}={option} : attendu %A{attendu}, obtenu %A{obtenu}" ]

              Expect.isEmpty fautes (System.String.Join("\n", fautes))
          }
          test "une partie commence par les constats, puis deux décisions au plus" {
              let p = ok (nouvelle "vivre")
              let ids = ouvertes p |> List.map _.id
              Expect.equal ids [ "vivre.constat-1"; "vivre.constat-2"; "vivre.monnaie"; "vivre.ouverture" ] "ouvertes"
          }
          test "on ne peut pas décider ce qui n'est pas proposé" {
              let p = ok (nouvelle "vivre")
              Expect.equal (decider p "vivre.coord" [ { levier = "coord"; option = "main" } ]) (Error(SituationFermee "vivre.coord")) ""
              Expect.equal (decider p "vivre.monnaie" [ { levier = "ouverture"; option = "ferme" } ]) (Error(ChoixInvalides "vivre.monnaie")) ""
              Expect.equal (decider p "vivre.monnaie" [ { levier = "monnaie"; option = "euro" } ]) (Error(OptionRefusee(Options.OptionInconnue("monnaie", "euro")))) ""
          }
          test "l'aperçu annonce exactement ce que la décision produit" {
              let p = ok (nouvelle "vivre")
              let choix = [ { levier = "ouverture"; option = "cible" } ]
              let a = ok (apercu p "vivre.ouverture" choix)
              let apres = ok (decider p "vivre.ouverture" choix)
              Expect.equal a.aussitot.score (Moteur.evaluer apres.etat).score "score aussitôt"
              Expect.isNonEmpty a.conseil "le conseil a quelque chose à dire"
              Expect.equal apres.reputation["bruno"] Labo.Data.Coefficients.jeuReputationPour "Bruno voulait des échanges ciblés"
          }
          test "une tranche se joue jusqu'au bilan, révolution après révolution" {
              let mutable p = ok (nouvelle "vivre")
              let mutable tours = 0

              while not (terminee p) && tours < 20 do
                  for s in ouvertes p do
                      if estDecision s then
                          let choix =
                              if s.genre.IsParis then [ { levier = "paris"; option = "aucun" } ]
                              else s.leviers |> List.map (fun l -> { levier = l; option = (Tables.optionsDe[l] |> List.head).id })
                          p <- ok (decider p s.id choix)

                  p <- ok (finirRevolution p)
                  tours <- tours + 1

              Expect.isTrue (terminee p) "tranche terminée"
              Expect.equal tours 4 "7 décisions (6 dilemmes et un pari) à 2 par révolution ; le bilan clôt la dernière"
              let bilans = p.journal |> List.choose (function Revolu(r, _, _, _, _) -> Some r | _ -> None)
              Expect.equal (List.sort bilans) [ 0; 1; 2; 3 ] "un bilan par révolution"
          }
          test "finir une révolution exige d'avoir pris ses décisions" {
              let p = ok (nouvelle "vivre")
              Expect.equal (finirRevolution p) (Error RevolutionInachevee) ""
          }
          test "le temps passe : à la 10e révolution, le modèle est à l'horizon de 10 ans" {
              Expect.equal (horizonDe 9) 0 ""
              Expect.equal (horizonDe 10) 1 ""
              Expect.equal (horizonDe 30) 2 ""
          } ]
