/// `labo publier` : met le jeu assemblé en ligne. Le site entier est écrit dans la branche `site`,
/// que GitHub Pages sert et que personne ne modifie à la main : une racine qui renvoie vers jeu/,
/// et les pages assemblées par `labo assembler`. Un seul commit, remplacé à chaque publication :
/// le dépôt ne garde pas une copie du jeu assemblé par mise en ligne (le code est dans main).
/// Remplace l'action JavaScript de publication : le projet n'utilise ni Node ni Python, même dans
/// sa mise en ligne.
///
/// `labo publier --essai` fait tout sauf pousser, et dit ce qui aurait changé.
module Labo.Cli.Publier

open System
open System.IO
open CliWrap
open CliWrap.Buffered

let branche = "site"

/// Les pages publiées : la page assemblée par `labo assembler` → son nom en ligne, sous jeu/.
let pages =
    [ "autonome.html", "index.html"
      "proposer-autonome.html", "proposer.html"
      "prenom-autonome.html", "prenom.html"
      "atelier-autonome.html", "atelier.html" ]

/// La racine du site : le jeu est sous jeu/ (les liens déjà donnés y mènent).
let racine =
    """<!doctype html>
<html lang="fr">
<head>
<meta charset="utf-8">
<meta name="viewport" content="width=device-width, initial-scale=1">
<meta http-equiv="refresh" content="0; url=./jeu/">
<link rel="canonical" href="./jeu/">
<title>Le Labo de la Bande</title>
</head>
<body>
<p><a href="./jeu/">Le Labo de la Bande</a></p>
</body>
</html>
"""

let message sha = $"Le jeu en ligne, depuis main {sha}"

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

/// Le même appel, sans échouer : le code de sortie et la sortie.
let gitPeutEchouer (dossier: string) (args: string list) =
    let r =
        Cli.Wrap("git").WithArguments(args).WithWorkingDirectory(dossier).WithValidation(CommandResultValidation.None).ExecuteBufferedAsync().Task.Result

    r.ExitCode, r.StandardOutput.Trim()

let executer (args: string list) =
    let essai = List.contains "--essai" args
    let depot = Repo.path []

    match pages |> List.filter (fun (src, _) -> not (File.Exists(Repo.path [ "jeu"; src ]))) with
    | (src, _) :: _ ->
        eprintfn "jeu/%s manque : lancer d'abord `labo assembler`." src
        1
    | [] ->
        // le site, construit à part dans un dépôt neuf : le dépôt courant n'est pas touché
        let arbre = Path.Combine(Path.GetTempPath(), $"labo-publier-{Guid.NewGuid():N}")

        try
            try
                let distant = (git depot [ "remote"; "get-url"; "origin" ]).Trim()
                let sha = (git depot [ "rev-parse"; "HEAD" ]).Trim()

                Directory.CreateDirectory(Path.Combine(arbre, "jeu")) |> ignore
                File.WriteAllText(Path.Combine(arbre, "index.html"), racine)
                File.WriteAllText(Path.Combine(arbre, ".nojekyll"), "")

                for (src, nom) in pages do
                    File.Copy(Repo.path [ "jeu"; src ], Path.Combine(arbre, "jeu", nom), true)

                git arbre [ "init"; "-q"; "-b"; branche ] |> ignore
                git arbre [ "add"; "-A" ] |> ignore
                let nouveau = (git arbre [ "write-tree" ]).Trim()

                // ce qui est en ligne : la branche site, si elle existe déjà
                let ancien =
                    match gitPeutEchouer arbre [ "fetch"; "-q"; "--depth"; "1"; distant; branche ] with
                    | 0, _ -> Some((git arbre [ "rev-parse"; "FETCH_HEAD^{tree}" ]).Trim())
                    | _ -> None

                if ancien = Some nouveau then
                    printfn "Rien à publier : la branche %s est déjà à jour." branche
                    0
                else
                    if ancien.IsSome then
                        printfn "%s" ((git arbre [ "diff"; "--stat"; "FETCH_HEAD"; "--cached" ]).Trim())
                    else
                        printfn "Première publication dans la branche %s." branche

                    git arbre [ "-c"; $"user.name={auteur}"; "-c"; $"user.email={courriel}"; "commit"; "-q"; "-m"; message sha ]
                    |> ignore

                    if essai then
                        printfn "Essai : rien n'est poussé."
                    else
                        git arbre [ "push"; "-q"; "--force"; distant; $"HEAD:{branche}" ] |> ignore
                        printfn "Publié dans la branche %s." branche

                    0
            with e ->
                eprintfn "%s" e.Message
                1
        finally
            if Directory.Exists arbre then
                try Directory.Delete(arbre, true) with _ -> ()
