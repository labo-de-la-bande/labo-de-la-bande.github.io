/// Lit data/ en valeurs typées et vérifie tout ce qui peut l'être avant de générer : formes des
/// colonnes, identifiants uniques, références entre fichiers, mini-langage des liens.
module Labo.Cli.Validate

open System
open System.Globalization
open System.Text.RegularExpressions
open Labo.Data
open Labo.Cli.Schema

type Donnees =
    { sources: Source list
      jauges: Jauge list
      jaugesFrance: ValeurPays list
      leviers: Levier list
      options: OptionLevier list
      liens: Lien list }

type Avertissement =
    | ReferenceNecessaire of fichier: string * ligne: int * colonne: string

let decrireAvertissement =
    function
    | ReferenceNecessaire(f, l, c) -> $"data/{f}:{l} : colonne {c} : [réf:?], une source reste à trouver"

/// Les courbes que le moteur sait calculer, et le nombre d'arguments de chacune.
/// À tenir à jour avec le moteur F# (étape 3) ; le doctor lira alors la liste du moteur lui-même.
let courbesConnues =
    Map
        [ "risque-evite", 1
          "baisse-violences-couple", 0
          "duree-vie-commune", 0
          "rendement-eleve", 0
          "tirage-sous-moitie", 1
          "tirage-au-dessus-moitie", 1
          "revocation-facile", 0
          "mandat-court", 0
          "socle", 2
          "revocation", 0
          "mandat-inexperience", 0 ]

/// Les facteurs nommés (`@nom` dans la colonne poids) et les préréglages (`@nom` dans ecrit_aussi).
let facteursConnus = set [ "risque-gang" ]
let prereglagesConnus = set [ "paliers"; "sans-paliers" ]

let reId = Regex "^[a-z0-9][a-z0-9_-]*$"
let reIdPointe = Regex "^[a-z0-9][a-z0-9_.-]*$"
let reChamp = Regex "^[cs]\\.[A-Za-z_][A-Za-z0-9_]*$"
let reRef = Regex "\\[réf:([^\\]]*)\\]"

type Lecteur() =
    let erreurs = ResizeArray<Erreur>()
    let avertissements = ResizeArray<Avertissement>()
    member _.Erreurs = List.ofSeq erreurs
    member _.Avertissements = List.ofSeq avertissements
    member _.Erreur e = erreurs.Add e
    member _.Avertir a = avertissements.Add a

    member l.Valeur (r: Rangee) col attendu =
        l.Erreur(Valeur(r.fichier, r.numero, col, r[col], attendu))

    member l.Id(r: Rangee, col) =
        if not (reId.IsMatch r[col]) then
            l.Valeur r col "un identifiant : minuscules, chiffres, - et _"

        r[col]

    member l.Nombre(r: Rangee, col) =
        match Double.TryParse(r[col], NumberStyles.Float, CultureInfo.InvariantCulture) with
        | true, v -> v
        | _ ->
            l.Valeur r col "un nombre, avec un point décimal"
            0.0

    member l.NombreOpt(r: Rangee, col) =
        if r[col] = "" then None else Some(l.Nombre(r, col))

    member l.Entier(r: Rangee, col) =
        match Int32.TryParse r[col] with
        | true, v -> v
        | _ ->
            l.Valeur r col "un nombre entier"
            0

    member l.Parmi (r: Rangee, col) (choix: (string * 'T) list) =
        match List.tryFind (fun (k, _) -> k = r[col]) choix with
        | Some(_, v) -> Some v
        | None ->
            let liste = String.Join(", ", choix |> List.map fst)
            l.Valeur r col $"l'une des valeurs : {liste}"
            None

    /// Un texte : les marqueurs [réf:id] doivent viser une source connue.
    member l.Texte (sources: Set<string>) (r: Rangee, col) =
        for m in reRef.Matches r[col] do
            match m.Groups[1].Value with
            | "?" -> l.Avertir(ReferenceNecessaire(r.fichier, r.numero, col))
            | id when not (sources.Contains id) ->
                l.Erreur(Reference(r.fichier, r.numero, col, id, "sources.csv"))
            | _ -> ()

        r[col]

    member l.Uniques (rangees: Rangee list) col =
        rangees
        |> List.fold
            (fun (vus: Set<string>) r ->
                if vus.Contains r[col] then l.Erreur(Doublon(r.fichier, r.numero, col, r[col]))
                vus.Add r[col])
            Set.empty
        |> ignore

/// `coord=plateforme|ia & relais!=relais` ; vide = toujours.
let analyserQuand (texte: string) : Result<Atome list, string> =
    if texte.Trim() = "" then
        Ok []
    else
        let atomes =
            texte.Split '&'
            |> Array.map (fun a ->
                let m = Regex.Match(a.Trim(), "^([a-z0-9_-]+)(!=|=)(\\*|[a-z0-9_-]+(\\|[a-z0-9_-]+)*)$")

                if not m.Success then
                    Error $"« {a.Trim()} » n'est pas de la forme levier=option, levier!=option, levier=a|b ou levier=*"
                else
                    let opts = if m.Groups[3].Value = "*" then [] else List.ofArray (m.Groups[3].Value.Split '|')

                    Ok
                        { levier = m.Groups[1].Value
                          egal = (m.Groups[2].Value = "=")
                          options = opts })
            |> List.ofArray

        match atomes |> List.choose (function Error e -> Some e | Ok _ -> None) with
        | e :: _ -> Error e
        | [] -> Ok(atomes |> List.choose (function Ok a -> Some a | Error _ -> None))

let analyserPoids (texte: string) : Result<Poids, string> =
    if texte = "" then
        Ok Plein
    elif texte.StartsWith "@" then
        let nom = texte.Substring 1
        if facteursConnus.Contains nom then Ok(Facteur nom) else Error $"facteur inconnu « {nom} »"
    else
        let i = texte.LastIndexOf ':'

        if i < 0 then
            Error "vide, @facteur, ou condition:nombre"
        else
            match
                analyserQuand (texte.Substring(0, i)),
                Double.TryParse(texte.Substring(i + 1), NumberStyles.Float, CultureInfo.InvariantCulture)
            with
            | Ok c, (true, f) -> Ok(Si(c, f))
            | Error e, _ -> Error e
            | _, _ -> Error "le facteur après « : » doit être un nombre"

let analyserCourbe (texte: string) : Result<Courbe option, string> =
    if texte = "" then
        Ok None
    else
        let m = Regex.Match(texte, "^([a-z0-9-]+)(\\(([^)]*)\\))?$")

        if not m.Success then
            Error "nom ou nom(arg,arg)"
        else
            let nom = m.Groups[1].Value

            let args =
                if m.Groups[3].Success && m.Groups[3].Value <> "" then
                    m.Groups[3].Value.Split ',' |> Array.map _.Trim() |> List.ofArray
                else
                    []

            match courbesConnues.TryFind nom with
            | None -> Error $"courbe inconnue du moteur « {nom} »"
            | Some n when n <> args.Length -> Error $"« {nom} » prend {n} argument(s), pas {args.Length}"
            | Some _ -> Ok(Some { nom = nom; args = args })

let analyserEcritures (texte: string) : Result<Ecriture list, string> =
    if texte.Trim() = "" then
        Ok []
    else
        let parts =
            texte.Split ';'
            |> Array.map (fun p ->
                let p = p.Trim()

                if p.StartsWith "@" then
                    let nom = p.Substring 1
                    if prereglagesConnus.Contains nom then Ok(Nomme nom) else Error $"préréglage inconnu « {nom} »"
                else
                    match p.Split '=' with
                    | [| c; v |] when reChamp.IsMatch c && v <> "" -> Ok(Champ(c, v))
                    | _ -> Error $"« {p} » n'est pas de la forme c.champ=valeur, s.champ=valeur ou @prereglage")
            |> List.ofArray

        match parts |> List.choose (function Error e -> Some e | Ok _ -> None) with
        | e :: _ -> Error e
        | [] -> Ok(parts |> List.choose (function Ok e -> Some e | Error _ -> None))

let charger (dossier: string) : Result<Donnees * Avertissement list, Erreur list> =
    let tables = Schema.tous |> List.map (fun f -> f.chemin, lire dossier f) |> Map.ofList

    let illisibles =
        tables |> Map.toList |> List.collect (fun (_, r) -> match r with Error e -> e | Ok _ -> [])

    if not illisibles.IsEmpty then
        Error illisibles
    else
        let t (f: Fichier) = match tables[f.chemin] with Ok r -> r | Error _ -> []
        let l = Lecteur()

        // sources
        let docs = t sourcesDocuments
        let rs = t Schema.sources
        l.Uniques rs "id"
        let idsSources = rs |> List.map (fun r -> r["id"]) |> Set.ofList

        for d in docs do
            if not (idsSources.Contains d["source"]) then
                l.Erreur(Reference(d.fichier, d.numero, "source", d["source"], "sources.csv"))

            l.Entier(d, "rang") |> ignore

        let sources =
            rs
            |> List.map (fun r ->
                let documents =
                    docs
                    |> List.filter (fun d -> d["source"] = r["id"])
                    |> List.sortBy (fun d -> l.Entier(d, "rang"))
                    |> List.map (fun d -> d["titre"], d["url"])

                if r["url"] = "" && documents.IsEmpty then
                    l.Valeur r "url" "une adresse, ou des lignes dans sources-documents.csv"

                { Source.id = l.Id(r, "id")
                  titre = r["titre"]
                  url = r["url"]
                  producteur = r["producteur"]
                  genre = r["type"]
                  methode = r["methode"]
                  limites = r["limites"]
                  documents = documents })

        let texte = l.Texte idsSources

        // jauges
        let rj = t Schema.jauges
        l.Uniques rj "id"

        let jauges =
            rj
            |> List.map (fun r ->
                { Jauge.id = l.Id(r, "id")
                  libelle = texte (r, "libelle_fr")
                  ancre = r["ancre"]
                  horsScore = l.Parmi (r, "hors_score") [ "oui", true; "non", false ] |> Option.defaultValue false
                  aide = texte (r, "aide_fr") })

        let idsJauges = jauges |> List.map _.id |> Set.ofList

        let refJauge (r: Rangee) col =
            if not (idsJauges.Contains r[col]) then
                l.Erreur(Reference(r.fichier, r.numero, col, r[col], "jauges.csv"))

            r[col]

        let rf = t jaugesFrance
        l.Uniques rf "jauge"

        let jaugesFrance =
            rf
            |> List.map (fun r ->
                { ValeurPays.jauge = refJauge r "jauge"
                  valeur = l.NombreOpt(r, "valeur")
                  texte = texte (r, "texte_fr")
                  date = r["date"]
                  revoir = texte (r, "revoir_fr") })

        let sansValeur = idsJauges - (jaugesFrance |> List.map _.jauge |> Set.ofList)

        if not sansValeur.IsEmpty then
            l.Erreur(Incomplet(Schema.jaugesFrance.chemin, $"""jauges sans valeur France : {String.Join(", ", sansValeur)}"""))

        // leviers et options
        let rl = t Schema.leviers
        l.Uniques rl "id"

        let genres =
            [ "choix", Choix; "oui-non", OuiNon; "curseur", Curseur; "prereglage", Prereglage ]

        let leviers =
            rl
            |> List.map (fun r ->
                let genre = l.Parmi (r, "genre") genres |> Option.defaultValue Choix

                if r["champ"] <> "" && not (reChamp.IsMatch r["champ"]) then
                    l.Valeur r "champ" "c.nom, s.nom, ou vide"

                if r["champ"] = "" && genre <> Prereglage then
                    l.Valeur r "champ" "un champ (seul un préréglage peut s'en passer)"

                if genre = Curseur then
                    for c in [ "min"; "max"; "pas" ] do
                        if r[c] = "" then l.Valeur r c "un nombre pour un curseur"

                { Levier.id = l.Id(r, "id")
                  titre = texte (r, "titre_fr")
                  ancre = r["ancre"]
                  genre = genre
                  champ = r["champ"]
                  defaut = if r["defaut"] = "" then None else Some r["defaut"]
                  min = l.NombreOpt(r, "min")
                  max = l.NombreOpt(r, "max")
                  pas = l.NombreOpt(r, "pas") })

        let levierDe = leviers |> List.map (fun v -> v.id, v) |> Map.ofList
        let ro = t Schema.options

        let options =
            ro
            |> List.map (fun r ->
                if not (levierDe.ContainsKey r["levier"]) then
                    l.Erreur(Reference(r.fichier, r.numero, "levier", r["levier"], "leviers.csv"))

                let ecritures =
                    match analyserEcritures r["ecrit_aussi"] with
                    | Ok e -> e
                    | Error m ->
                        l.Valeur r "ecrit_aussi" m
                        []

                { OptionLevier.levier = r["levier"]
                  rang = l.Entier(r, "rang")
                  id = l.Id(r, "id")
                  libelle = texte (r, "libelle_fr")
                  note = texte (r, "note_fr")
                  valeur = r["valeur"]
                  ecritAussi = ecritures })

        let optionsDe =
            options |> List.groupBy _.levier |> List.map (fun (k, os) -> k, os) |> Map.ofList

        for (k, os) in Map.toList optionsDe do
            let ids = os |> List.map _.id

            if ids.Length <> (Set.ofList ids).Count then
                l.Erreur(Incomplet(Schema.options.chemin, $"le levier {k} a deux options de même id"))

            if (os |> List.map _.rang |> List.sort) <> [ 0 .. os.Length - 1 ] then
                l.Erreur(Incomplet(Schema.options.chemin, $"les rangs des options de {k} ne vont pas de 0 à {os.Length - 1} sans trou"))

        for v in leviers do
            let os = optionsDe.TryFind v.id |> Option.defaultValue []

            match v.genre with
            | Curseur when not os.IsEmpty ->
                l.Erreur(Incomplet(Schema.options.chemin, $"le curseur {v.id} ne prend pas d'options"))
            | Choix
            | OuiNon
            | Prereglage when os.IsEmpty -> l.Erreur(Incomplet(Schema.options.chemin, $"le levier {v.id} n'a aucune option"))
            | _ -> ()

            match v.genre, v.defaut with
            | (Choix | OuiNon | Prereglage), Some d when not (os |> List.exists (fun o -> o.id = d)) ->
                l.Erreur(Incomplet(Schema.leviers.chemin, $"le défaut « {d} » du levier {v.id} n'est pas une de ses options"))
            | _ -> ()

        // liens
        let rk = t Schema.liens
        l.Uniques rk "id"

        let verifierAtomes (r: Rangee) col (atomes: Atome list) =
            for a in atomes do
                match levierDe.TryFind a.levier with
                | None -> l.Erreur(Reference(r.fichier, r.numero, col, a.levier, "leviers.csv"))
                | Some _ ->
                    let os = optionsDe.TryFind a.levier |> Option.defaultValue [] |> List.map _.id |> Set.ofList

                    for o in a.options do
                        if not (os.Contains o) then
                            l.Erreur(Reference(r.fichier, r.numero, col, $"{a.levier}={o}", "options.csv"))

        let refSource (r: Rangee) col =
            match r[col] with
            | "" -> None
            | s ->
                if not (idsSources.Contains s) then
                    l.Erreur(Reference(r.fichier, r.numero, col, s, "sources.csv"))

                Some s

        let liens =
            rk
            |> List.map (fun r ->
                if not (reIdPointe.IsMatch r["id"]) then
                    l.Valeur r "id" "un identifiant : minuscules, chiffres, ., - et _"

                let quand =
                    match analyserQuand r["quand"] with
                    | Ok a ->
                        verifierAtomes r "quand" a
                        a
                    | Error m ->
                        l.Valeur r "quand" m
                        []

                let poids =
                    match analyserPoids r["poids"] with
                    | Ok p ->
                        (match p with
                         | Si(c, _) -> verifierAtomes r "poids" c
                         | _ -> ())

                        p
                    | Error m ->
                        l.Valeur r "poids" m
                        Plein

                let courbe =
                    match analyserCourbe r["courbe"] with
                    | Ok c -> c
                    | Error m ->
                        l.Valeur r "courbe" m
                        None

                if quand.IsEmpty && courbe.IsNone then
                    l.Valeur r "quand" "une condition, ou une courbe (sinon le lien serait toujours actif)"

                { Lien.id = r["id"]
                  jauge = refJauge r "jauge"
                  effet = l.Nombre(r, "effet")
                  statut =
                    l.Parmi (r, "statut") [ "sourcé", Sourcé; "hypothèse", Hypothèse; "jugement", Jugement ]
                    |> Option.defaultValue Jugement
                  nature =
                    l.Parmi
                        (r, "nature")
                        [ "contrainte", Contrainte
                          "égalité", Egalite
                          "danger", Danger
                          "capacité", Capacite
                          "coût", Cout
                          "concentration", Concentration
                          "dépendance", Dependance
                          "dissuasion", Dissuasion
                          "privation", Privation ]
                    |> Option.defaultValue Contrainte
                  raison = texte (r, "raison_fr")
                  source = refSource r "source"
                  contreSource = refSource r "contre_source"
                  quand = quand
                  poids = poids
                  courbe = courbe })

        match l.Erreurs with
        | [] ->
            Ok(
                { sources = sources
                  jauges = jauges
                  jaugesFrance = jaugesFrance
                  leviers = leviers
                  options = options
                  liens = liens },
                l.Avertissements
            )
        | e -> Error e
