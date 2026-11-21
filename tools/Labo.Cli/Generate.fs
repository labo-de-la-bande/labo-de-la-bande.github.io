/// Le F# généré depuis data/ dans src/Labo.Data/Generated/. Le texte est déterministe (fins de
/// ligne `\n`, lignes dans l'ordre des CSV) : un fichier est à jour quand il est, à l'octet près,
/// ce que `labo generate` écrirait.
module Labo.Cli.Generate

open System
open System.Globalization
open System.IO
open System.Text
open Labo.Data
open Labo.Cli.Validate

let chaine (s: string) =
    let b = StringBuilder "\""

    for c in s do
        match c with
        | '\\' -> b.Append "\\\\"
        | '"' -> b.Append "\\\""
        | '\n' -> b.Append "\\n"
        | '\r' -> b.Append "\\r"
        | '\t' -> b.Append "\\t"
        | c -> b.Append c
        |> ignore

    b.Append('"').ToString()

let nombre (v: float) =
    let s = v.ToString("R", CultureInfo.InvariantCulture)
    if s.Contains '.' || s.Contains 'E' then s else s + ".0"

let option f =
    function
    | None -> "None"
    | Some v -> $"Some {f v}"

let liste f xs =
    match xs with
    | [] -> "[]"
    | xs -> "[ " + String.Join("; ", xs |> List.map f) + " ]"

let booleen b = if b then "true" else "false"

/// Un enregistrement F#, un champ par ligne, indenté sous `[`.
let enregistrement (champs: (string * string) list) =
    let corps =
        champs
        |> List.mapi (fun i (k, v) -> (if i = 0 then "      { " else "        ") + $"{k} = {v}")

    String.Join("\n", corps) + " }"

let fichier (sources: string list) (moduleNom: string) (ouvertures: string list) (corps: string) =
    let entete =
        $"""// GÉNÉRÉ depuis {String.Join(", ", sources |> List.map (fun s -> "data/" + s))} par `labo generate`.
// Ne pas éditer : changer les données et régénérer.
module {moduleNom}
"""

    let opens = String.Join("", ouvertures |> List.map (fun o -> $"\nopen {o}"))
    entete + opens + "\n\n" + corps + "\n"

let valeursListe (nom: string) (typeNom: string) (items: string list) =
    if items.IsEmpty then
        $"let {nom} : {typeNom} list = []"
    else
        $"let {nom} : {typeNom} list =\n    [\n" + String.Join("\n", items) + "\n    ]"

let atome (a: Atome) =
    $"{{ levier = {chaine a.levier}; egal = {booleen a.egal}; options = {liste chaine a.options} }}"

let sourcesFs (d: Donnees) =
    d.sources
    |> List.map (fun s ->
        enregistrement
            [ "id", chaine s.id
              "titre", chaine s.titre
              "url", chaine s.url
              "producteur", chaine s.producteur
              "genre", chaine s.genre
              "methode", chaine s.methode
              "limites", chaine s.limites
              "documents", liste (fun (t, u) -> $"({chaine t}, {chaine u})") s.documents ])
    |> valeursListe "tous" "Source"
    |> fichier [ "sources.csv"; "sources-documents.csv" ] "Labo.Data.Sources" [ "Labo.Data" ]

let jaugesFs (d: Donnees) =
    d.jauges
    |> List.map (fun j ->
        enregistrement
            [ "id", chaine j.id
              "libelle", chaine j.libelle
              "ancre", chaine j.ancre
              "horsScore", booleen j.horsScore
              "aide", chaine j.aide ])
    |> valeursListe "tous" "Jauge"
    |> fichier [ "jauges.csv" ] "Labo.Data.Jauges" [ "Labo.Data" ]

let paysFrFs (d: Donnees) =
    d.jaugesFrance
    |> List.map (fun v ->
        enregistrement
            [ "jauge", chaine v.jauge
              "valeur", option nombre v.valeur
              "texte", chaine v.texte
              "date", chaine v.date
              "revoir", chaine v.revoir ])
    |> valeursListe "jauges" "ValeurPays"
    |> fichier [ "pays/fr/jauges.csv" ] "Labo.Data.Pays.Fr" [ "Labo.Data" ]

let genre =
    function
    | Choix -> "Choix"
    | OuiNon -> "OuiNon"
    | Curseur -> "Curseur"
    | Prereglage -> "Prereglage"

let ecriture =
    function
    | Champ(c, v) -> $"Champ({chaine c}, {chaine v})"
    | Nomme n -> $"Nomme {chaine n}"

let leviersFs (d: Donnees) =
    let leviers =
        d.leviers
        |> List.map (fun v ->
            enregistrement
                [ "id", chaine v.id
                  "titre", chaine v.titre
                  "ancre", chaine v.ancre
                  "genre", genre v.genre
                  "champ", chaine v.champ
                  "defaut", option chaine v.defaut
                  "min", option nombre v.min
                  "max", option nombre v.max
                  "pas", option nombre v.pas ])
        |> valeursListe "tous" "Levier"

    let options =
        d.options
        |> List.map (fun o ->
            enregistrement
                [ "levier", chaine o.levier
                  "rang", string o.rang
                  "id", chaine o.id
                  "libelle", chaine o.libelle
                  "note", chaine o.note
                  "valeur", chaine o.valeur
                  "ecritAussi", liste ecriture o.ecritAussi ])
        |> valeursListe "options" "OptionLevier"

    fichier [ "leviers.csv"; "options.csv" ] "Labo.Data.Leviers" [ "Labo.Data" ] (leviers + "\n\n" + options)

let statut =
    function
    | Sourcé -> "Sourcé"
    | Hypothèse -> "Hypothèse"
    | Jugement -> "Jugement"

let poids =
    function
    | Plein -> "Plein"
    | Si(c, f) -> $"Si({liste atome c}, {nombre f})"
    | Facteur n -> $"Facteur {chaine n}"

let liensFs (d: Donnees) =
    d.liens
    |> List.map (fun l ->
        enregistrement
            [ "id", chaine l.id
              "jauge", chaine l.jauge
              "effet", nombre l.effet
              "statut", statut l.statut
              "nature", $"%A{l.nature}"
              "raison", chaine l.raison
              "source", option chaine l.source
              "contreSource", option chaine l.contreSource
              "quand", liste atome l.quand
              "poids", poids l.poids
              "courbe", option (fun (c: Courbe) -> $"{{ nom = {chaine c.nom}; args = {liste chaine c.args} }}") l.courbe ])
    |> valeursListe "tous" "Lien"
    |> fichier [ "liens.csv" ] "Labo.Data.Liens" [ "Labo.Data" ]

/// Chaque fichier généré : chemin relatif au dépôt, contenu.
let sorties (d: Donnees) =
    [ "src/Labo.Data/Generated/Sources.fs", sourcesFs d
      "src/Labo.Data/Generated/Jauges.fs", jaugesFs d
      "src/Labo.Data/Generated/Pays.Fr.fs", paysFrFs d
      "src/Labo.Data/Generated/Leviers.fs", leviersFs d
      "src/Labo.Data/Generated/Liens.fs", liensFs d ]

type Etat =
    | AJour
    | Obsolete
    | Absent

let etat (chemin: string) (contenu: string) =
    let p = Repo.path [ chemin ]

    if not (File.Exists p) then Absent
    elif File.ReadAllText p = contenu then AJour
    else Obsolete
