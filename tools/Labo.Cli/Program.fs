module Labo.Cli.Program

open Labo.Cli.Schema

let usage =
    """labo : la CLI du Labo de la Bande

  labo doctor                       vérifie data/ et que le F# généré est à jour (ne modifie rien)
  labo generate                     valide data/ et écrit src/Labo.Data/Generated/
  labo oracle capture [page.html]   fige les réponses de l'ancien moteur JavaScript dans oracle/
  labo servir [dossier] [port]       sert le jeu en local (par défaut jeu/ sur le port 8080)
  labo verifier-js                  le moteur compilé en JavaScript rejoue l'oracle dans Chrome
  labo assembler                    réunit le jeu compilé en une page autonome (jeu/autonome.html)
  labo sources verifier             va sur internet : liens morts, rétractations et corrections (Crossref) ; rapport dans .tmp/, ne modifie rien
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

[<EntryPoint>]
let main argv =
    match List.ofArray argv with
    | [ "doctor" ] -> doctor ()
    | [ "generate" ] -> generate ()
    | [ "oracle"; "capture" ] -> (Oracle.capturerTout (Repo.path [ "index.html" ])).Result
    | [ "oracle"; "capture"; page ] -> (Oracle.capturerTout (System.IO.Path.GetFullPath page)).Result
    | [ "assembler" ] -> Assembler.executer ()
    | [ "sources"; "verifier" ] -> SourcesVerifier.executer ()
    | [ "verifier-js" ] -> (VerifierJs.verifier "verifier.html").Result
    | [ "verifier-js"; "assemble" ] -> (VerifierJs.verifier "verifier-assemble.html").Result
    | [ "servir" ] -> Servir.servir (Repo.path [ "jeu" ]) 8080; 0
    | [ "servir"; dossier ] -> Servir.servir dossier 8080; 0
    | [ "servir"; dossier; port ] -> Servir.servir dossier (int port); 0
    | _ ->
        eprintf "%s" usage
        2
