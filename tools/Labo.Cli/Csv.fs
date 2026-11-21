/// Lecture des CSV de data/ : UTF-8, séparateur `,`, guillemets RFC 4180 (un champ qui contient
/// une virgule, un guillemet ou un saut de ligne est entre guillemets, un guillemet intérieur est
/// doublé). La première ligne est l'en-tête. Les lignes vides sont ignorées.
module Labo.Cli.Csv

open System.Text

type Ligne =
    { /// numéro de la ligne du fichier où commence l'enregistrement (1 = l'en-tête)
      numero: int
      champs: string list }

type ErreurCsv =
    | GuillemetNonFerme of ligne: int
    | GuillemetDansChamp of ligne: int
    | FichierVide

/// Découpe un texte en enregistrements, chacun avec le numéro de sa première ligne.
let analyser (texte: string) : Result<Ligne list, ErreurCsv> =
    let t = texte.TrimStart '﻿'
    let lignes = ResizeArray<Ligne>()
    let champs = ResizeArray<string>()
    let champ = StringBuilder()
    let mutable ligne = 1
    let mutable debut = 1
    let mutable i = 0
    let mutable entreGuillemets = false
    let mutable champCite = false
    let mutable erreur = None

    let finChamp () =
        champs.Add(champ.ToString())
        champ.Clear() |> ignore
        champCite <- false

    let finLigne () =
        finChamp ()

        if not (champs.Count = 1 && champs[0] = "") then
            lignes.Add { numero = debut; champs = List.ofSeq champs }

        champs.Clear()

    while erreur.IsNone && i < t.Length do
        let c = t[i]

        if entreGuillemets then
            if c = '"' then
                if i + 1 < t.Length && t[i + 1] = '"' then
                    champ.Append '"' |> ignore
                    i <- i + 1
                else
                    entreGuillemets <- false
            else
                if c = '\n' then ligne <- ligne + 1
                champ.Append c |> ignore
        else
            match c with
            | '"' when champ.Length = 0 && not champCite ->
                entreGuillemets <- true
                champCite <- true
            | '"' -> erreur <- Some(GuillemetDansChamp ligne)
            | ',' -> finChamp ()
            | '\r' -> ()
            | '\n' ->
                finLigne ()
                ligne <- ligne + 1
                debut <- ligne
            | c -> champ.Append c |> ignore

        i <- i + 1

    match erreur with
    | Some e -> Error e
    | None when entreGuillemets -> Error(GuillemetNonFerme debut)
    | None ->
        if champ.Length > 0 || champs.Count > 0 then finLigne ()
        if lignes.Count = 0 then Error FichierVide else Ok(List.ofSeq lignes)

/// Écrit un champ, entre guillemets seulement s'il le faut.
let citer (v: string) =
    if v.IndexOfAny [| ','; '"'; '\n'; '\r' |] >= 0 then
        "\"" + v.Replace("\"", "\"\"") + "\""
    else
        v
