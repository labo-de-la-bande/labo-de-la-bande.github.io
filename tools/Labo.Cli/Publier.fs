/// `labo publier` : met le jeu assemblé en ligne, dans le dossier jeu/ de la branche main (servie
/// par GitHub Pages), sans toucher au reste de main. Remplace l'action JavaScript de publication :
/// le projet n'utilise ni Node ni Python, même dans sa mise en ligne.
///
/// `labo publier --essai` fait tout sauf pousser, et dit ce qui aurait changé.
module Labo.Cli.Publier

open System
open System.IO
open CliWrap
open CliWrap.Buffered

/// Les pages publiées : la page assemblée par `labo assembler` → son nom en ligne, sous jeu/.
let pages =
    [ "autonome.html", "index.html"
      "proposer-autonome.html", "proposer.html"
      "prenom-autonome.html", "prenom.html" ]

let message sha = $"Nouveau jeu en ligne (jeu/), depuis refonte {sha}"

/// le robot de GitHub, pas le compte qui a poussé
let auteur = "github-actions[bot]"
let courriel = "41898282+github-actions[bot]@users.noreply.github.com"

let git (dossier: string) (args: string list) =
    let r =
        Cli
            .Wrap("git")
            .WithArguments(args)
            .WithWorkingDirectory(dossier)
            .WithValidation(CommandResultValidation.None)
            .ExecuteBufferedAsync()
            .Task.Result

    if r.ExitCode <> 0 then
        failwith $"git {String.Join(' ', args)} : {r.StandardError.Trim()}"

    r.StandardOutput

let executer (args: string list) =
    let essai = List.contains "--essai" args
    let depot = Repo.path []

    match pages |> List.filter (fun (src, _) -> not (File.Exists(Repo.path [ "jeu"; src ]))) with
    | (src, _) :: _ ->
        eprintfn "jeu/%s manque : lancer d'abord `labo assembler`." src
        1
    | [] ->
        let arbre = Path.Combine(Path.GetTempPath(), $"labo-publier-{Guid.NewGuid():N}")

        try
            try
                // main, à part : un arbre de travail détaché, le dépôt courant n'est pas touché
                git depot [ "fetch"; "--depth"; "1"; "origin"; "main" ] |> ignore
                git depot [ "worktree"; "add"; "--detach"; arbre; "FETCH_HEAD" ] |> ignore

                Directory.CreateDirectory(Path.Combine(arbre, "jeu")) |> ignore

                for (src, nom) in pages do
                    File.Copy(Repo.path [ "jeu"; src ], Path.Combine(arbre, "jeu", nom), true)

                git arbre [ "add"; "--"; "jeu" ] |> ignore
                let changes = (git arbre [ "diff"; "--cached"; "--stat" ]).Trim()

                if changes = "" then
                    printfn "Rien à publier : jeu/ est déjà à jour sur main."
                    0
                else
                    printfn "%s" changes

                    git arbre [ "-c"; $"user.name={auteur}"; "-c"; $"user.email={courriel}"; "commit"; "-q"; "-m"; message ((git depot [ "rev-parse"; "HEAD" ]).Trim()) ]
                    |> ignore

                    if essai then
                        printfn "Essai : rien n'est poussé."
                    else
                        git arbre [ "push"; "origin"; "HEAD:main" ] |> ignore
                        printfn "Publié dans jeu/ de main."

                    0
            with e ->
                eprintfn "%s" e.Message
                1
        finally
            if Directory.Exists arbre then
                try git depot [ "worktree"; "remove"; "--force"; arbre ] |> ignore with _ -> ()
