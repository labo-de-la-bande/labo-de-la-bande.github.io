/// Le moteur de jeu : dérouler une partie.
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

let tests =
    testList
        "jeu"
        [ test "une partie commence par les constats, puis toutes les décisions de la tranche" {
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
          test "le même chemin avec une police partout : perdue par ce qu'elle garde, même mieux que la France" {
              match issue (jouer plusFidele true) with
              | Perdue [ InterditsGardes l ] ->
                  Expect.containsAll (l |> List.map _.id) [ "gardes-gang"; "gardes-violence"; "surveillance-generalisee" ] "la police et la surveillance"
                  Expect.isFalse (l |> List.exists (fun a -> a.id = "armee")) "l'armée n'est pas interdite dans ce type de partie"
              | autre -> failtestf "attendu : perdue par ce qu'elle garde seulement, obtenu %A" autre
          }
          test "chaque abolition se reconnaît à sa condition" {
              let p = jouer plusFidele true
              let ids = garde p.etat |> List.map _.id
              Expect.containsAll ids [ "gardes-gang"; "gardes-violence"; "surveillance-generalisee"; "armee" ] "les options imposées"
              Expect.isEmpty (garde (jouer plusFidele false).etat) "le chemin le plus fidèle ne garde rien"
          }
          test "ce qu'il faut pour gagner vient du type de partie : viable, mieux que la France, des seuils" {
              let t = Labo.Data.Jeu.typesPartie |> List.find (fun t -> t.id = "sans-prison")
              let premiere = jouer premier false
              let fidele = jouer plusFidele false

              // sans l'exigence de tenir debout, la première option partout ne perd plus par là
              match issueSelon { t with viable = false } premiere with
              | Perdue raisons -> Expect.isFalse (raisons |> List.exists (function PasViable _ -> true | _ -> false)) "pas de raison « viable »"
              | Gagnee -> ()

              // un seuil hors d'atteinte fait perdre le chemin le plus fidèle, et c'est dit
              match issueSelon { t with seuils = [ "principes", Labo.Data.Superieur, 0.999 ] } fidele with
              | Perdue [ SeuilManque("principes", Labo.Data.Superieur, 0.999, v) ] -> Expect.isLessThan v 0.999 "la valeur est dite"
              | autre -> failtestf "attendu : perdue par le seuil seulement, obtenu %A" autre

              // un seuil atteint ne change rien
              Expect.equal (issueSelon { t with seuils = [ "principes", Labo.Data.Superieur, 0.5 ] } fidele) Gagnee "seuil atteint"
              Expect.equal (conditionsVictoire t) "tenir debout, ne rien garder de ce qu'elle voulait abolir et faire mieux que la France" "en mots"
          } ]

/// Labo reconnaît sa part quand ses conseils suivis ont coûté les principes (choix de conception).
let testsConseilsSuivis =
    let jouerAvec (choisir: Partie -> Labo.Data.Situation -> Choix list option) =
        let rec boucle (p: Partie) =
            match ouvertes p |> List.tryFind estDecision with
            | Some s ->
                let c =
                    match choisir p s with
                    | Some c -> c
                    | None ->
                        match s.genre with
                        | Labo.Data.Paris -> [ { levier = "paris"; option = "aucun" } ]
                        | Labo.Data.Prevention -> s.leviers |> List.map (fun l -> { levier = l; option = "non" })
                        | _ -> s.leviers |> List.map (fun l -> { levier = l; option = (List.head Tables.optionsDe[l]).id })

                boucle (ok (Journal.jouer (Action.Decider(s.id, c)) p))
            | None when terminee p -> p
            | None -> boucle (ok (Journal.jouer Action.FinirRevolution p))

        boucle (ok (Journal.commencer { region = "france"; graine = 1L; hasard = false; parcours = parcoursComplet }))

    let conseil p (s: Labo.Data.Situation) =
        conseilApprentissage p s.id |> Option.filter (fun k -> k.gain > gainNegligeable) |> Option.map _.choix

    testList
        "conseils de Labo suivis"
        [ test "suivre Labo partout : des conseils suivis ont fait garder ce que la partie interdit" {
              let p = jouerAvec conseil
              let fautifs = Journal.conseilsSuivis p |> List.filter (fun c -> c.suivi && not c.interditsAjoutes.IsEmpty)
              Expect.isNonEmpty fautifs "au moins un conseil suivi a ajouté un interdit"
          }
          test "ne jamais suivre Labo : aucun conseil suivi" {
              let p = jouerAvec (fun _ _ -> None)
              let suivisCoutant = Journal.conseilsSuivis p |> List.filter (fun c -> c.suivi && not c.interditsAjoutes.IsEmpty)
              Expect.isEmpty suivisCoutant "rien à reprocher à Labo"
          } ]
