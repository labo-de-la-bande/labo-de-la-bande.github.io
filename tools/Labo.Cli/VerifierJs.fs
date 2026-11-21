/// `labo verifier-js` : le moteur compilé en JavaScript donne-t-il les nombres de l'oracle ? Sert
/// la racine du dépôt, ouvre jeu/verifier.html dans Chrome sans écran et lit le résultat. Il faut
/// d'abord compiler l'interface : dotnet fable src/Labo.Interface -o jeu/js
module Labo.Cli.VerifierJs

open System
open System.Text.RegularExpressions
open System.Threading
open CliWrap
open CliWrap.Buffered

let verifier () =
    task {
        match Oracle.chrome () with
        | None ->
            eprintfn "%s" (Oracle.decrire Oracle.ChromeIntrouvable)
            return 1
        | Some exe ->
            let port = 8791
            let serveur = Thread((fun () -> Servir.servir (Repo.path [ "." ]) port), IsBackground = true)
            serveur.Start()
            Thread.Sleep 500
            let profil = IO.Directory.CreateTempSubdirectory "labo-verifier-"

            let! r =
                Cli
                    .Wrap(exe)
                    .WithArguments(
                        [ "--headless"
                          "--disable-gpu"
                          "--user-data-dir=" + profil.FullName
                          "--virtual-time-budget=600000"
                          "--dump-dom"
                          $"http://localhost:{port}/jeu/verifier.html" ]
                    )
                    .WithValidation(CommandResultValidation.None)
                    .ExecuteBufferedAsync()

            try profil.Delete true with _ -> ()
            let m = Regex.Match(r.StandardOutput, "<pre id=\"resultat\" data-fini=\"oui\">(.*?)</pre>", RegexOptions.Singleline)

            if not m.Success then
                eprintfn "La vérification n'a pas abouti (interface compilée ? dotnet fable src/Labo.Interface -o jeu/js)."
                return 1
            else
                let texte = Net.WebUtility.HtmlDecode m.Groups[1].Value
                printfn "%s" texte
                return (if texte.Contains "ÉCART" || texte.Contains "ERREUR" then 1 else 0)
    }
