module Labo.Cli.Program

open Labo.Cli.Schema

let usage =
    """labo : la CLI du Labo de la Bande

  labo self install                 installe ou met à jour la commande labo (outil .NET global)
  labo --version                    le commit d'où vient la commande installée
  labo doctor                       vérifie data/ et que le F# généré est à jour (ne modifie rien)
  labo generate                     valide data/ et écrit src/Labo.Data/Generated/
  labo servir [dossier] [port]       sert le jeu en local (par défaut jeu/ sur le port 8080)
  labo assembler                    réunit le jeu compilé en une page autonome (jeu/autonome.html)
  labo idees [publier|ecarter|suivre]  relire les idées envoyées sans compte (collaborateurs du dépôt, avec gh) : docs/relais.md
  labo prenoms importer [export]    verse les prénoms déclarés par les joueurs dans data/prenoms/joueurs.csv, à valider (docs/relais.md)
  labo sources verifier             va sur internet : liens morts, rétractations et corrections (Crossref) ; rapport dans .tmp/, ne modifie rien
  labo explorer [stratégie] [--choix] [--hasard N]  joue tout le parcours selon une façon de choisir (premier, score, fidelite, apprentissage), puis jusqu'à 30 ans : score, fidélité et viabilité face à la France
  labo sensibilite [--hasard N]    ce que les conclusions du modèle doivent aux liens : chaque lien retiré seul, puis par statut et par lentille
  labo publier [--essai]           met le site (le jeu assemblé) dans la branche site, servie par GitHub Pages ; --essai : tout sauf pousser
  labo relais deployer [--essai]    met le relais des idées sur Cloudflare par son API, sans wrangler (CLOUDFLARE_API_TOKEN)
  labo relais secret <NOM>          pose un secret du relais (valeur lue sur l'entrée standard)
  labo atelier inviter <id> <nom> | invitations | retirer <id> | cle <pem> | fusionner | envoyer  l'atelier des données, sans compte GitHub (docs/atelier.md)
  labo rumeurs telecharger | chercher <sujet> | flux [sujet] | archives [sujet]  des fausses nouvelles déjà vérifiées (Data Commons ; flux des vérificateurs français), dans une pile à trier (.tmp/)
"""

let afficherErreurs (erreurs: Erreur list) =
    for e in erreurs do
        eprintfn "✗ %s" (decrire e)

    eprintfn "%d erreur(s)" erreurs.Length

let afficherAvertissements (avs: Validate.Avertissement list) =
    for a in avs do
        printfn "· %s" (Validate.decrireAvertissement a)

let resume (d: Validate.Donnees) =
    printfn
        "data/ : %d sources, %d jauges, %d leviers, %d options, %d liens"
        d.sources.Length
        d.jauges.Length
        d.leviers.Length
        d.options.Length
        d.liens.Length

let doctor () =
    match Validate.charger (Repo.path [ "data" ]) with
    | Error e ->
        afficherErreurs e
        1
    | Ok(d, avs) ->
        afficherAvertissements avs
        resume d

        let erreurs =
            Generate.sorties d
            |> List.choose (fun (chemin, contenu) ->
                match Generate.etat chemin contenu with
                | Generate.AJour -> None
                | Generate.Obsolete -> Some(GenereObsolete chemin)
                | Generate.Absent -> Some(GenereAbsent chemin))

        if erreurs.IsEmpty then
            printfn "✓ F# généré à jour"
            0
        else
            afficherErreurs erreurs
            1

let generate () =
    match Validate.charger (Repo.path [ "data" ]) with
    | Error e ->
        afficherErreurs e
        1
    | Ok(d, avs) ->
        afficherAvertissements avs
        resume d

        for (chemin, contenu) in Generate.sorties d do
            match Generate.etat chemin contenu with
            | Generate.AJour -> ()
            | _ ->
                Repo.writeText (Repo.path [ chemin ]) contenu
                printfn "écrit %s" chemin

        0

let commande argv =
    match List.ofArray argv with
    | [ "--version" ] -> Self.version ()
    | [ "self"; "install" ]
    | [ "self"; "update" ] -> Self.installer ()
    | [ "doctor" ] -> doctor ()
    | [ "generate" ] -> generate ()
    | [ "assembler" ] -> Assembler.executer ()
    | "idees" :: reste -> Idees.executer reste
    | [ "prenoms"; "importer" ] -> Prenoms.executer None
    | [ "prenoms"; "importer"; fichier ] -> Prenoms.executer (Some fichier)
    | [ "sources"; "verifier" ] -> SourcesVerifier.executer ()
    | "explorer" :: reste -> Explorer.executer reste
    | "sensibilite" :: reste -> Explorer.lancer "sensibilite" reste
    | "publier" :: reste -> Publier.executer reste
    | "relais" :: reste -> RelaisDeployer.executer reste
    | "atelier" :: reste -> AtelierCli.executer generate reste
    | "rumeurs" :: reste -> Rumeurs.executer reste
    | [ "servir" ] -> Servir.servir (Repo.path [ "jeu" ]) 8080; 0
    | [ "servir"; dossier ] -> Servir.servir dossier 8080; 0
    | [ "servir"; dossier; port ] -> Servir.servir dossier (int port); 0
    | _ ->
        eprintf "%s" usage
        2

[<EntryPoint>]
let main argv =
    // hors du dépôt : un message clair plutôt qu'une trace d'erreur
    if argv.Length > 0 && (try Repo.root.Force() |> ignore; false with _ -> true) then
        eprintfn "labo : à lancer depuis le dépôt labo-de-la-bande (ou l'un de ses dossiers)."
        1
    else
        if argv.Length > 0 && argv[0] <> "self" then Self.verifierAJour ()
        commande argv
