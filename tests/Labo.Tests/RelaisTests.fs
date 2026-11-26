/// Le relais des idées sans compte : ce qu'il accepte, ce qu'il publie, ce qu'il écrit au joueur.
module Labo.Tests.RelaisTests

open Expecto
open Labo.Relais.Proposition

let bonne =
    { situation = "Faut-il une monnaie ?"
      idee = "Un registre des manques par cellule, sans centre."
      appuis = ""
      provenance = "moi"
      citation = ""
      mail = None }

let refus p =
    match valider p with
    | Error r -> r
    | Ok _ -> []

let tests =
    testList
        "relais"
        [ test "une proposition complète passe, nettoyée" {
              match valider { bonne with idee = "  " + bonne.idee + "\n"; mail = Some "  " } with
              | Ok p ->
                  Expect.equal p.idee bonne.idee "espaces retirés"
                  Expect.isNone p.mail "une adresse vide n'est pas une adresse"
              | Error r -> failtest $"%A{r}"
          }
          test "tous les refus d'un coup" {
              let r = refus { bonne with idee = "court"; provenance = ""; mail = Some "pas-une-adresse"; citation = String.replicate 61 "x" }
              Expect.containsAll r [ IdeeTropCourte; ProvenanceInconnue; MailInvalide; TropLong("citation", 60) ] "quatre refus"
          }
          test "un texte de joueur ne mentionne personne sur GitHub" {
              let p = { bonne with idee = "Demandez à @quelquun ce qu'il en pense"; citation = "@moi" }
              Expect.isFalse ((corpsPublic p false).Contains "@quelquun") "mention cassée dans le corps"
              Expect.isFalse ((titre p).Contains "@quelquun") "et dans le titre"
              Expect.isFalse ((corpsPublic p false).Contains "@moi") "et dans la signature"
          }
          test "le ticket public ne contient ni le code de suivi ni l'adresse" {
              let p = { bonne with mail = Some "joueur@exemple.fr" }
              let c = corpsPublic p false
              Expect.isFalse (c.Contains "joueur@") "pas d'adresse"
              Expect.isFalse ((corpsRelecture p "code-secret").Contains "joueur@") "pas d'adresse non plus dans la relecture"
              Expect.isTrue ((corpsRelecture p "code-secret").Contains "code-secret") "le code est dans la relecture (privée)"
          }
          test "sans pseudonyme, l'idée est signée « un joueur »" {
              Expect.stringContains (corpsPublic bonne false) "un joueur" ""
          }
          test "le titre reste court" {
              let t = titre { bonne with idee = String.replicate 200 "a" }
              Expect.isTrue (t.Length <= 70) t
          }
          test "après une décision finale, le message dit que l'adresse est effacée" {
              let _, texte = mailStatut Integree (Some "https://exemple/1") None "https://page#suivi=x"
              Expect.stringContains texte "effacée" ""
              let _, texte = mailStatut Publiee (Some "https://exemple/1") None "https://page#suivi=x"
              Expect.stringContains texte "https://page#suivi=x" "sinon, le lien pour suivre et se désinscrire"
          }
          test "les statuts se relisent" {
              for s in [ ARelire; Publiee; ALEtude; Integree; Ecartee ] do
                  Expect.equal (statutDe (codeStatut s)) (Some s) (codeStatut s)
          } ]

let testsDeclarations =
    testList
        "prénoms déclarés"
        [ test "une déclaration complète passe, avec la casse corrigée" {
              match validerDeclaration { prenom = "  MARIE-CLAIRE "; decennie = 1980; pays = "DZ"; usage = "f" } with
              | Ok d ->
                  Expect.equal d.prenom "Marie-Claire" "casse"
                  Expect.equal d.pays "dz" "pays en minuscules"
              | Error e -> failtest $"%A{e}"
          }
          test "une casse mêlée est gardée, les lettres de toutes les écritures passent" {
              for p in [ "DeShawn"; "Nguyễn Thị"; "Ilyès"; "N'Golo"; "Мария"; "محمد" ] do
                  Expect.isOk (validerDeclaration { prenom = p; decennie = 1990; pays = "fr"; usage = "fm" }) p
          }
          test "pas de chiffres, de liens ni de champ manquant" {
              for p in [ "a"; "x1"; "http://spam"; "<b>"; String.replicate 41 "a" ] do
                  Expect.isError (validerDeclaration { prenom = p; decennie = 1990; pays = "fr"; usage = "f" }) p

              Expect.isError (validerDeclaration { prenom = "Léa"; decennie = 1985; pays = "fr"; usage = "f" }) "décennie"
              Expect.isError (validerDeclaration { prenom = "Léa"; decennie = 1990; pays = "france"; usage = "f" }) "pays"
              Expect.isError (validerDeclaration { prenom = "Léa"; decennie = 1990; pays = "fr"; usage = "autre" }) "usage"
          }
          test "la clé d'un compte se relit" {
              let d = { prenom = "Aïcha"; decennie = 1990; pays = "ma"; usage = "f" }
              Expect.equal (declarationDeCle (cleDeclaration d)) (Some d) ""
          } ]

/// La configuration du relais, lue par `labo relais deployer` (sans wrangler) : ce qu'il enverrait.
let testsDeploiement =
    testList
        "déploiement du relais"
        [ test "wrangler.toml se lit : nom, point d'entrée, variables, stockage" {
              let c = Labo.Cli.RelaisDeployer.lireConfig (System.IO.File.ReadAllText(Labo.Cli.Repo.path [ "src"; "Labo.Relais"; "wrangler.toml" ]))
              Expect.equal c.nom "labo-relais" "le nom du Worker"
              Expect.equal c.principal "js/Worker.js" "le point d'entrée"
              Expect.containsAll (c.vars |> List.map fst) [ "DEPOT_PUBLIC"; "ORIGINES"; "HOTES"; "PAGE_SUIVI"; "LIMITE_PAR_HEURE" ] "les variables"
              Expect.equal (c.kv |> List.map fst) [ "IDEES" ] "le stockage des idées"
          }
          test "les secrets déjà posés sont gardés à chaque déploiement" {
              let c = Labo.Cli.RelaisDeployer.lireConfig "name = \"x\"\nmain = \"js/W.js\"\ncompatibility_date = \"2026-11-01\"\n"
              let m = Labo.Cli.RelaisDeployer.metadonnees c
              Expect.equal (m["keep_bindings"].ToJsonString()) "[\"secret_text\"]" "keep_bindings"
              Expect.equal (m["main_module"].GetValue<string>()) "W.js" "le module principal, sans son dossier"
          }
          test "une section inconnue est refusée plutôt qu'ignorée" {
              Expect.throws (fun () -> Labo.Cli.RelaisDeployer.lireConfig "name = \"x\"\n[routes]\npattern = \"a\"\n" |> ignore) "section inconnue"
          } ]

/// Les rumeurs déjà vérifiées : lecture du fichier de Data Commons (format DataFeed de ClaimReview).
let testsRumeurs =
    let exemple =
        """{"@type":"DataFeed","dataFeedElement":[{"item":[{"@type":"ClaimReview","author":{"name":"Les Surligneurs"},"claimReviewed":"Une loi va interdire l'argent liquide pour les paiements","datePublished":"2025-11-13","itemReviewed":{"author":{"name":"Vidéos TikTok"},"datePublished":"2025-11-01"},"reviewRating":{"alternateName":"Faux","ratingExplanation":"Aucun texte ne le prévoit."},"url":"https://exemple.fr/verif"}]},{"item":null},{"item":[{"claimReviewed":"The police joined the protest and the crowd cheered","author":{"name":"PolitiFact"},"reviewRating":{"alternateName":"False"}}]}]}"""

    let lues = Labo.Cli.Rumeurs.lireFlux (System.Text.Json.JsonDocument.Parse(exemple).RootElement)

    testList
        "rumeurs déjà vérifiées"
        [ test "le fichier se lit : affirmation, qui, quand, verdict, vérificateur, lien ; un élément vide est ignoré" {
              Expect.equal lues.Length 2 "deux vérifications"
              let t = List.head lues
              Expect.equal t.qui "Vidéos TikTok" "qui"
              Expect.equal t.quand "2025-11-01" "quand"
              Expect.equal t.verdict "Faux" "verdict"
              Expect.equal t.verificateur "Les Surligneurs" "vérificateur"
              Expect.equal t.url "https://exemple.fr/verif" "lien"
          }
          test "la langue se reconnaît à ses petits mots" {
              Expect.equal (lues |> List.map Labo.Cli.Rumeurs.langueDe) [ "fr"; "en" ] "français puis anglais"
          }
          test "le sujet se cherche sans accents ni majuscules" {
              Expect.isTrue (Labo.Cli.Rumeurs.correspond "ARGENT" (List.head lues)) "argent"
              Expect.isTrue (Labo.Cli.Rumeurs.correspond "loi" (List.head lues)) "loi"
              Expect.isFalse (Labo.Cli.Rumeurs.correspond "prison" (List.head lues)) "pas prison"
          } ]

/// Les flux d'actualité des vérificateurs : titre, résumé sans balises, date, lien.
let testsFlux =
    testList
        "flux des vérificateurs"
        [ test "un flux RSS se lit" {
              let xml =
                  """<?xml version="1.0"?><rss><channel><title>X</title><item><title>Non, cette vidéo ne montre pas la police suisse</title><description><![CDATA[<p>Hors contexte. La séquence date de 2019.</p>]]></description><pubDate>Wed, 25 Nov 2026 10:00:00 +0100</pubDate><link>https://exemple.eu/non</link></item></channel></rss>"""

              match Labo.Cli.Rumeurs.lireRss "Les Surligneurs" xml with
              | [ a ] ->
                  Expect.equal a.titre "Non, cette vidéo ne montre pas la police suisse" "titre"
                  Expect.equal a.resume "Hors contexte. La séquence date de 2019." "résumé sans balises"
                  Expect.equal a.date "2026-11-25" "date"
                  Expect.equal a.lien "https://exemple.eu/non" "lien"
              | autre -> failtestf "un article attendu, obtenu %A" autre
          } ]
