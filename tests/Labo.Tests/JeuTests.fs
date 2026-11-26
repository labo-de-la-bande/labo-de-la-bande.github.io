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
          test "une partie commence par les constats, puis toutes les décisions de la tranche" {
              let p = ok (nouvelle "vivre")
              let ids = ouvertes p |> List.map _.id

              Expect.equal
                  ids
                  [ "vivre.constat-1"; "vivre.constat-2"; "vivre.monnaie"; "vivre.ouverture"; "vivre.energie"; "vivre.engrais"
                    "vivre.coord"; "vivre.relais"; "vivre.dette-etat"; "vivre.dette-menages"; "vivre.paris" ]
                  "ouvertes"
          }
          test "on ne peut pas décider ce qui n'est pas proposé" {
              let p = ok (nouvelle "vivre")
              let p = ok (nouvelleAvec 2026L true [ "vivre"; "foyers" ])
              Expect.equal (decider p "foyers.logement" [ { levier = "logement"; option = (Tables.optionsDe["logement"] |> List.head).id } ]) (Error(SituationFermee "foyers.logement")) "la tranche suivante attend"
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
              Expect.equal
                  apres.reputation["agriculteur"]
                  (Labo.Data.Coefficients.jeuReputationPour + Labo.Data.Coefficients.jeuReputationRessenti)
                  "Bruno voulait des échanges ciblés, et ses engrais en profitent"

              for (h, d) in a.reputation do
                  Expect.floatClose Accuracy.high (apres.reputation.TryFind h |> Option.defaultValue 0.0) d $"l'aperçu annonce la confiance de {h}"
          }
          test "une tranche se joue en une révolution, jusqu'au bilan" {
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
              Expect.equal tours 1 "les 9 décisions (8 dilemmes et un pari) se prennent avant que l'année passe ; le bilan la clôt"
              let bilans = p.journal |> List.choose (function Revolu(r, _, _, _, _) -> Some r | _ -> None)
              Expect.equal bilans [ 0 ] "un bilan, celui de la révolution 0"
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

/// L'issue de la partie : le principe est souverain (décision de l'auteur, 26 novembre 2026).
let testsIssue =
    let police = Map [ "violence", "gardes"; "bande", "gardes"; "surveillance", "generalisee"; "invasion", "armee" ]

    let candidats (s: Labo.Data.Situation) =
        match s.genre with
        | Labo.Data.Paris -> [ { levier = "paris"; option = "aucun" } ] :: (s.paris |> List.map (fun b -> [ { levier = "paris"; option = b } ]))
        | Labo.Data.Prevention ->
            s.leviers |> List.fold (fun acc l -> acc |> List.collect (fun c -> [ c @ [ { levier = l; option = "oui" } ]; c @ [ { levier = l; option = "non" } ] ])) [ [] ]
        | _ -> s.leviers |> List.collect (fun l -> Tables.optionsDe[l] |> List.map (fun o -> [ { levier = l; option = o.id } ]))

    /// une partie entière : `choisir` pour chaque décision, `forcer` impose la police
    let jouer (choisir: Partie -> Labo.Data.Situation -> Choix list) forcer =
        let rec boucle p =
            match ouvertes p |> List.tryFind estDecision with
            | Some s ->
                let c =
                    match s.leviers with
                    | [ l ] when forcer && police.ContainsKey l -> [ { levier = l; option = police[l] } ]
                    | _ -> choisir p s

                boucle (ok (decider p s.id c))
            | None when terminee p -> p
            | None -> boucle (ok (finirRevolution p))

        boucle (ok (Journal.commencer { region = "france"; graine = 1L; hasard = false; parcours = parcoursComplet }))

    let premier _ s = List.head (candidats s)

    let plusFidele p (s: Labo.Data.Situation) =
        candidats s
        |> List.mapi (fun i c -> i, c, (match apercu p s.id c with Ok a -> a.aTerme.fidelite | Error _ -> -infinity))
        |> List.sortBy (fun (i, _, v) -> -v, i)
        |> List.head
        |> fun (_, c, _) -> c

    testList
        "issue de la partie"
        [ test "la première option partout : perdue, la société ne tient pas" {
              match issue (jouer premier false) with
              | Perdue raisons -> Expect.exists raisons (function PasViable _ -> true | _ -> false) "pas viable"
              | Gagnee -> failtest "devait être perdue"
          }
          test "le chemin le plus fidèle au but, sans police : gagnée" {
              Expect.equal (issue (jouer plusFidele false)) Gagnee "gagnée"
          }
          test "le même chemin avec une police partout : perdue par les principes seuls, même mieux que la France" {
              match issue (jouer plusFidele true) with
              | Perdue [ PrincipesAbandonnes v ] -> Expect.isLessThan v 0.75 "principes sous le seuil"
              | autre -> failtestf "attendu : perdue par les principes seuls, obtenu %A" autre
          } ]
