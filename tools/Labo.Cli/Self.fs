/// `labo self install` : installe la CLI comme outil .NET global (`labo` partout, depuis le dépôt ou
/// l'un de ses dossiers), à la manière de `franz self install`. La première fois, depuis le dépôt :
///   dotnet run --project tools/Labo.Cli -- self install
/// ensuite : `labo self install` (ou `labo self update`). L'outil retient le commit d'où il a été
/// construit (`labo --version`) et prévient quand son code a changé depuis.
module Labo.Cli.Self

open System
open System.IO
open System.Reflection
open CliWrap
open CliWrap.Buffered

/// Ce que la CLI lit pour se construire : si l'un de ces chemins change, elle est à réinstaller.
let sourcesCli = [ "tools/Labo.Cli"; "src/Labo.Types"; "src/Labo.Relais/Proposition.fs"; "paket.lock" ]

let lancer (exe: string) (args: string list) =
    let r =
        Cli.Wrap(exe).WithArguments(args).WithWorkingDirectory(Repo.root.Value).WithValidation(CommandResultValidation.None).ExecuteBufferedAsync().Task.Result

    r.ExitCode, r.StandardOutput.Trim(), r.StandardError.Trim()

/// Le commit d'où vient l'outil installé (vide pour une construction de développement).
let commitConstruit () =
    match Assembly.GetEntryAssembly().GetCustomAttribute<AssemblyInformationalVersionAttribute>() with
    | null -> ""
    | a ->
        let v = a.InformationalVersion.Split('+')[0]
        if v.Length = 40 && v |> Seq.forall Uri.IsHexDigit then v else ""

let version () =
    match commitConstruit () with
    | "" -> printfn "labo (construction de développement)"
    | c -> printfn "labo, construit depuis le commit %s" (c.Substring(0, 8))

    0

let installer () =
    match lancer "git" [ "rev-parse"; "HEAD" ] with
    | 0, commit, _ ->
        let propre = (lancer "git" ([ "status"; "--porcelain"; "--" ] @ sourcesCli) |> fun (_, s, _) -> s) = ""
        // une version qui grandit à chaque installation : dotnet tool installe bien la nouvelle
        let n = int ((DateTime.UtcNow - DateTime(2026, 1, 1)).TotalMinutes)
        let version = $"1.0.{n}"
        let dossier = Repo.path [ ".tmp"; "outil" ]
        Directory.CreateDirectory dossier |> ignore
        printfn "Construction de labo (commit %s%s)…" (commit.Substring(0, 8)) (if propre then "" else ", avec des changements non commités")

        match
            lancer "dotnet" [ "pack"; "tools/Labo.Cli"; "-c"; "Release"; "-o"; dossier; $"-p:Version={version}"; $"-p:InformationalVersion={commit}"; "--nologo"; "-v"; "q" ]
        with
        | 0, _, _ ->
            lancer "dotnet" [ "tool"; "uninstall"; "--global"; "labo-cli" ] |> ignore

            match lancer "dotnet" [ "tool"; "install"; "--global"; "labo-cli"; "--add-source"; dossier; "--version"; version ] with
            | 0, _, _ ->
                let outils = Path.Combine(Environment.GetFolderPath Environment.SpecialFolder.UserProfile, ".dotnet", "tools")
                printfn "labo est installé dans %s : tape `labo` depuis le dépôt ou l'un de ses dossiers." outils
                printfn "Pour le mettre à jour : labo self install."

                // le dossier des outils .NET doit être dans le PATH, écrit en entier (un `~` n'y est
                // pas remplacé par le dossier personnel quand le terminal cherche une commande)
                let chemins = (Environment.GetEnvironmentVariable "PATH").Split(Path.PathSeparator)

                let dansLePath (d: string) = chemins |> Array.exists (fun c -> c.TrimEnd('/') = d)

                if not (dansLePath outils) then
                    // l'installateur de .NET écrit souvent `~/.dotnet/tools` tel quel dans le PATH, et le
                    // terminal ne le comprend pas : un lien dans un dossier personnel du PATH, comme
                    // ~/.local/bin, fait le même effet sans toucher à la configuration du terminal
                    let maison = Environment.GetFolderPath Environment.SpecialFolder.UserProfile

                    match [ ".local/bin"; "bin" ] |> List.map (fun d -> Path.Combine(maison, d)) |> List.tryFind (fun d -> dansLePath d && Directory.Exists d) with
                    | Some d ->
                        let lien = Path.Combine(d, "labo")
                        if File.Exists lien || (FileInfo lien).LinkTarget <> null then File.Delete lien
                        File.CreateSymbolicLink(lien, Path.Combine(outils, "labo")) |> ignore
                        printfn "Lien posé dans %s (le PATH ne contient pas %s en entier)." d outils
                    | None ->
                        printfn ""
                        printfn "Ton terminal ne cherche pas encore les commandes dans %s." outils
                        printfn "Une fois pour toutes, lance :"
                        printfn "  echo 'export PATH=\"$HOME/.dotnet/tools:$PATH\"' >> ~/.zprofile"
                        printfn "puis ouvre un nouvel onglet du terminal."

                0
            | _, sortie, erreur ->
                eprintfn "dotnet tool install a échoué :\n%s\n%s" sortie erreur
                1
        | _, sortie, erreur ->
            eprintfn "dotnet pack a échoué :\n%s\n%s" sortie erreur
            1
    | _, _, erreur ->
        eprintfn "git rev-parse a échoué : %s" erreur
        1

/// Prévient, sans bloquer, quand le code de la CLI a changé depuis l'installation de l'outil.
let verifierAJour () =
    match commitConstruit () with
    | "" -> ()
    | c ->
        match lancer "git" ([ "diff"; "--quiet"; c; "--" ] @ sourcesCli) with
        | 1, _, _ -> eprintfn "· le code de labo a changé depuis son installation : labo self install pour le mettre à jour"
        | _ -> ()
