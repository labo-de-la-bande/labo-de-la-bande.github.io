/// `labo explorer` : lance l'exploration du modèle de tools/Labo.Jouer, qui connaît le moteur de
/// jeu. La CLI ne dépend pas du F# généré (pour pouvoir le régénérer quand il est cassé) : elle
/// passe donc par `dotnet run`.
module Labo.Cli.Explorer

open System
open CliWrap

let executer (args: string list) =
    let sortie = Console.OpenStandardOutput()
    let erreurs = Console.OpenStandardError()

    let r =
        Cli
            .Wrap("dotnet")
            .WithArguments([ "run"; "--project"; Repo.path [ "tools"; "Labo.Jouer" ]; "--"; "explorer" ] @ args)
            .WithWorkingDirectory(Repo.path [])
            .WithStandardOutputPipe(PipeTarget.ToStream sortie)
            .WithStandardErrorPipe(PipeTarget.ToStream erreurs)
            .WithValidation(CommandResultValidation.None)
            .ExecuteAsync()
            .Task.Result

    r.ExitCode
