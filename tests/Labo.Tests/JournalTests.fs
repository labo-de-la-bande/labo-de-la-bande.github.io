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

let testsHistoires =
    testList
        "histoires des habitants"
        [ test "chaque décision où un habitant avait un avis entre dans son histoire, avec son humeur" {
              let p = jouee ()
              let h = histoires p

              for v in Labo.Data.Jeu.voix |> List.filter (fun v -> v.situation = "vivre.monnaie") do
                  let moments = h.TryFind v.habitant |> Option.defaultValue []
                  let m = moments |> List.tryFind (fun m -> m.situation = "vivre.monnaie" && m.humeur.IsSome)
                  Expect.isSome m $"{v.habitant} : la décision sur la monnaie est dans son histoire"
                  Expect.equal m.Value.humeur (Some(humeurDe situationDe["vivre.monnaie"] [ { levier = "monnaie"; option = "sans" } ] v.habitant)) v.habitant
          }
          test "la dernière confiance de l'histoire est la confiance actuelle" {
              let p = jouee ()

              for KeyValue(h, moments) in histoires p do
                  Expect.floatClose Accuracy.high (List.last moments).confiance (p.reputation.TryFind h |> Option.defaultValue 0.0) h
          } ]

/// Toutes les décisions de la première tranche, depuis le début, avec le ressenti de chaque habitant.
let casDeRessenti () =
    let p0 = ok (commencer { config with parcours = [ "vivre" ] })

    [ for s in Labo.Data.Jeu.situations |> List.filter (fun s -> s.tranche = "vivre" && estDecision s && estOuverte p0 s.id) do
          for l in s.leviers do
              for o in Labo.Engine.Tables.optionsDe[l] do
                  let c = [ { levier = l; option = o.id } ]

                  match decider p0 s.id c with
                  | Ok p -> yield s, c, p
                  | Error _ -> () ]

let testsRessenti =
    testList
        "ressenti des habitants"
        [ test "chaque habitant a au moins un attachement, sur une jauge qui existe" {
              for h in Labo.Data.Jeu.habitants do
                  Expect.isNonEmpty (Labo.Data.Jeu.attachements |> List.filter (fun a -> a.habitant = h.id)) h.id
          }
          test "un habitant sans avis mais nettement touché réagit, et sa confiance bouge" {
              let touches =
                  [ for (s, c, p) in casDeRessenti () do
                        for h in Labo.Data.Jeu.habitants do
                            let r = p.vecus[s.id][h.id]

                            if (avisDe s.id h.id).IsNone && abs r >= Labo.Data.Coefficients.jeuRessentiSeuil then
                                yield s, c, p, h.id, r ]

              Expect.isNonEmpty touches "au moins un cas dans la première tranche"

              for (s, c, p, h, r) in touches do
                  let attendu = if r > 0.0 then Labo.Data.Approuve else Labo.Data.Desapprouve
                  Expect.equal (humeurDans p s c h) attendu $"{s.id} {h}"
                  let d = effetDecision p s c |> List.tryFind (fun (x, _) -> x = h) |> Option.map snd
                  Expect.equal d (Some((if r > 0.0 then 1.0 else -1.0) * Labo.Data.Coefficients.jeuReputationRessenti)) $"{s.id} {h} confiance"
          }
          test "revenir sur la dernière décision efface son ressenti" {
              for (s, c, p) in casDeRessenti () |> List.truncate 3 do
                  let p2 = match revenirSur p s.id with Ok x -> x | Error e -> failtest $"%A{e}"
                  Expect.isFalse (p2.ressentis.ContainsKey s.id) s.id
          } ]

let testsSensibilite =
    testList
        "ce qu'on vit pèse plus"
        [ test "une perte sur ce qui compte pour un habitant augmente le poids de cet attachement" {
              let cas =
                  [ for (s, _, p) in casDeRessenti () do
                        for a in Labo.Data.Jeu.attachements do
                            match p.ressentis[s.id] |> List.tryFind (fun (j, _) -> j = a.jauge) with
                            | Some(_, d) when d < -1.0 -> yield p, a, d
                            | _ -> () ]

              Expect.isNonEmpty cas "au moins une perte dans la première tranche"

              for (p, a, _) in cas do
                  Expect.isTrue (sensibilite p a.habitant a.jauge > 0.0) $"{a.habitant} / {a.jauge}"
                  Expect.isTrue (poidsEffectif p a > a.poids) $"{a.habitant} / {a.jauge} : poids"
          }
          test "la sensibilité retombe en partie à chaque révolution" {
              for (s, _, p) in casDeRessenti () do
                  for a in Labo.Data.Jeu.attachements do
                      let maintenant = sensibilite p a.habitant a.jauge

                      if maintenant > 0.0 && maintenant < Labo.Data.Coefficients.jeuSensibilitePlafond then
                          let plusTard = sensibilite { p with revolution = p.revolution + 1 } a.habitant a.jauge
                          Expect.floatClose Accuracy.high plusTard (maintenant * (1.0 - Labo.Data.Coefficients.jeuApaisementRevolution)) $"{s.id} {a.habitant}"
          }
          test "la sensibilité ne dépasse jamais le plafond" {
              for (_, _, p) in casDeRessenti () do
                  for a in Labo.Data.Jeu.attachements do
                      Expect.isTrue (sensibilite p a.habitant a.jauge <= Labo.Data.Coefficients.jeuSensibilitePlafond) a.jauge
          } ]

let testsConfianceRegard =
    testList
        "la confiance colore le regard"
        [ test "une déception coûte moins à qui fait confiance, plus à qui se méfie ; un gain ne change pas" {
              let p0 = ok (commencer { config with parcours = [ "vivre" ] })
              let mutable vus = 0

              for s in Labo.Data.Jeu.situations |> List.filter (fun s -> s.tranche = "vivre" && estDecision s && estOuverte p0 s.id) do
                  for l in s.leviers do
                      for o in Labo.Engine.Tables.optionsDe[l] do
                          let c = [ { levier = l; option = o.id } ]

                          let effet (rep: float) =
                              let p = { p0 with reputation = Labo.Data.Jeu.habitants |> List.map (fun h -> h.id, rep) |> Map.ofList }
                              match decider p s.id c with
                              | Ok q -> effetDecision q s c |> Map.ofList
                              | Error e -> failtest $"%A{e}"

                          let neutre, confiant, mefiant = effet 0.0, effet 5.0, effet -5.0

                          for KeyValue(h, d) in neutre do
                              vus <- vus + 1
                              if d < 0.0 then
                                  Expect.floatClose Accuracy.high confiant[h] (d * 0.5) $"{s.id} {o.id} {h} confiant"
                                  Expect.floatClose Accuracy.high mefiant[h] (d * 1.5) $"{s.id} {o.id} {h} méfiant"
                              else
                                  Expect.floatClose Accuracy.high confiant[h] d $"{s.id} {o.id} {h} gain"

              Expect.isTrue (vus > 0) "des effets à comparer"
          }
          test "revenir sur une décision défait exactement ce qu'elle avait fait à la confiance, colorée comprise" {
              let p0 = { (ok (commencer { config with parcours = [ "vivre" ] })) with reputation = Map [ "boulanger", 4.0; "ainee", -4.0 ] }
              let c = [ { levier = "monnaie"; option = "sans" } ]
              let p1 = ok (jouer (Action.Decider("vivre.monnaie", c)) p0)
              let appliques = effetDecision p1 situationDe["vivre.monnaie"] c
              let attendu = effetRevirement p1 situationDe["vivre.monnaie"] c
              let p2 = ok (jouer (Action.RevenirSur "vivre.monnaie") p1)

              for (h, d) in attendu do
                  Expect.floatClose Accuracy.high (p2.reputation[h] - p1.reputation[h]) d h

              Expect.isNonEmpty appliques "la décision avait touché quelqu'un"
          } ]

let testsVieAvance =
    testList
        "la vie avance"
        [ test "chaque révolution vieillit les habitants d'un an" {
              let p = ok (commencer config)

              for h in Labo.Data.Jeu.habitants do
                  Expect.equal (ageDe p h.id) h.age h.id
                  Expect.equal (ageDe { p with revolution = 3 } h.id) (h.age + 3) h.id
          }
          test "jusqu'à l'âge impressionnable, ce qu'on vit marque une fois et demie plus ; ensuite, comme tout le monde" {
              let seuil = int Labo.Data.Coefficients.jeuAgeImpressionnable
              Expect.equal (malleabilite 16) Labo.Data.Coefficients.jeuMalleabiliteJeunes "16 ans"
              Expect.equal (malleabilite seuil) Labo.Data.Coefficients.jeuMalleabiliteJeunes "à la limite"
              Expect.equal (malleabilite (seuil + 1)) 1.0 "après"
              Expect.equal (malleabilite 74) 1.0 "rien au grand âge"
          }
          test "la sensibilité d'un jeune suit la règle : perte × 1,5" {
              let jeune = Labo.Data.Jeu.habitants |> List.minBy _.age

              for (s, _, p) in casDeRessenti () do
                  for a in Labo.Data.Jeu.attachements |> List.filter (fun a -> a.habitant = jeune.id) do
                      match p.ressentis[s.id] |> List.tryFind (fun (j, _) -> j = a.jauge) with
                      | Some(_, d) when d < 0.0 ->
                          let attendu = min Labo.Data.Coefficients.jeuSensibilitePlafond (-d * Labo.Data.Coefficients.jeuSensibilisationPerte * Labo.Data.Coefficients.jeuMalleabiliteJeunes)
                          Expect.floatClose Accuracy.high (sensibilite p jeune.id a.jauge) attendu $"{s.id} {a.jauge}"
                      | _ -> ()
          } ]
