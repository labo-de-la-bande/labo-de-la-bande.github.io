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
      liens: Lien list
      lentilles: Lentille list
      curseurs: Curseur list
      attaques: Attaque list
      maturites: Maturite list
      paris: Pari list
      faiblesses: Faiblesse list
      parades: Parade list
      prevention: Prevention list
      coefficients: Coefficient list
      constantesFrance: Constante list
      ressourcesFrance: RessourcePays list
      ressourcesOptionsFrance: OptionRessource list
      importationsFrance: Importation list
      exportationsFrance: Exportation list
      tranches: Tranche list
      situations: Situation list
      chiffres: Chiffre list
      voix: Voix list
      habitants: Habitant list
      evenements: Evenement list
      repliques: Replique list }

type Avertissement =
    | ReferenceNecessaire of fichier: string * ligne: int * colonne: string
    /// des lignes dont la lentille n'est pas encore choisie
    | AClasser of fichier: string * nombre: int
    /// des lignes classées par un assistant : des propositions, à vérifier
    | ParUnAssistant of fichier: string * nombre: int
    /// des lignes « de mémoire » ou « à documenter »
    | SansSource of fichier: string * statut: string * nombre: int

let decrireAvertissement =
    function
    | ReferenceNecessaire(f, l, c) -> $"data/{f}:{l} : colonne {c} : [réf:?], une source reste à trouver"
    | AClasser(f, n) -> $"data/{f} : {n} ligne(s) à classer dans une lentille"
    | ParUnAssistant(f, n) -> $"data/{f} : {n} ligne(s) classée(s) par un assistant, à vérifier"
    | SansSource(f, s, n) -> $"data/{f} : {n} ligne(s) « {s} »"

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

        let statut (r: Rangee) =
            l.Parmi
                (r, "statut")
                [ "sourcé", Sourcé
                  "hypothèse", Hypothèse
                  "jugement", Jugement
                  "de mémoire", DeMémoire
                  "à documenter", ADocumenter ]
            |> Option.defaultValue Jugement

        /// `a|b|c` : des sources connues ; `?` marque une source à trouver
        let listeSources (r: Rangee) col =
            if r[col] = "" then
                []
            else
                [ for s in r[col].Split '|' do
                      if s = "?" then l.Avertir(ReferenceNecessaire(r.fichier, r.numero, col))
                      elif not (idsSources.Contains s) then l.Erreur(Reference(r.fichier, r.numero, col, s, "sources.csv"))
                      else yield s ]

        /// `auteur`, `assistant`, ou l'identifiant d'un contributeur
        let provenance (r: Rangee) col =
            if not (reId.IsMatch r[col]) then
                l.Valeur r col "auteur, assistant, ou l'identifiant d'un contributeur"

            r[col]

        let oui (r: Rangee) col =
            l.Parmi (r, col) [ "oui", true; "non", false ] |> Option.defaultValue false

        let entierOpt (r: Rangee) col = if r[col] = "" then None else Some(l.Entier(r, col))
        let texteOpt (r: Rangee) col = if r[col] = "" then None else Some r[col]

        let couvertures = [ "manq"; "nv"; "part"; "imp"; "ok" ] |> List.map (fun c -> c, c)

        let compterSansSource fichier (statuts: Statut list) =
            for s, nom in [ DeMémoire, "de mémoire"; ADocumenter, "à documenter" ] do
                match statuts |> List.filter ((=) s) |> List.length with
                | 0 -> ()
                | n -> l.Avertir(SansSource(fichier, nom, n))

        // lentilles
        let rlen = t Schema.lentilles
        l.Uniques rlen "id"

        let lentilles =
            rlen
            |> List.map (fun r ->
                { Lentille.id = l.Id(r, "id")
                  nom = texte (r, "nom_fr")
                  penseurs = r["penseurs_fr"]
                  description = texte (r, "description_fr")
                  proposePar = provenance r "propose_par" })

        let idsLentilles = lentilles |> List.map _.id |> Set.ofList

        let refLentille (r: Rangee) col =
            if not (idsLentilles.Contains r[col]) then
                l.Erreur(Reference(r.fichier, r.numero, col, r[col], "lentilles.csv"))

            r[col]

        let compterAClasser fichier (ids: string list) =
            match ids |> List.filter ((=) "a-classer") |> List.length with
            | 0 -> ()
            | n -> l.Avertir(AClasser(fichier, n))

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
                  seuilVital = l.NombreOpt(r, "seuil_vital")
                  solide = l.NombreOpt(r, "solide")
                  solideOuvert = l.NombreOpt(r, "solide_ouvert")
                  solideFrance = l.NombreOpt(r, "solide_france")
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
                  statut = statut r
                  lentille = refLentille r "lentille"
                  lentillePar = provenance r "lentille_par"
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

        compterAClasser Schema.liens.chemin (liens |> List.map _.lentille)
        match liens |> List.filter (fun x -> x.lentillePar = "assistant") |> List.length with
        | 0 -> ()
        | n -> l.Avertir(ParUnAssistant(Schema.liens.chemin, n))

        compterSansSource Schema.liens.chemin (liens |> List.map _.statut)

        for j in jauges do
            if not j.horsScore && (j.solide.IsNone || j.solideOuvert.IsNone || j.solideFrance.IsNone) then
                l.Erreur(Incomplet(Schema.jauges.chemin, $"la jauge {j.id} entre dans le score : solide, solide_ouvert et solide_france sont requis"))

        // curseurs
        let rc = t Schema.curseurs
        l.Uniques rc "id"

        let curseurs =
            rc
            |> List.map (fun r ->
                if r["defaut"] = "" && r["depart"] = "" then
                    l.Valeur r "defaut" "une valeur par défaut ou une valeur de départ"

                { Curseur.id = r["id"]
                  defaut = l.NombreOpt(r, "defaut")
                  depart = l.NombreOpt(r, "depart")
                  description = texte (r, "description_fr") })

        let idsCurseurs = curseurs |> List.map _.id |> Set.ofList

        for v in leviers do
            if v.champ.StartsWith "s." && not (idsCurseurs.Contains(v.champ.Substring 2)) then
                l.Erreur(Incomplet(Schema.leviers.chemin, $"le levier {v.id} règle {v.champ}, absent de curseurs.csv"))

        for o in options do
            for ec in o.ecritAussi do
                match ec with
                | Champ(c, _) when c.StartsWith "s." && not (idsCurseurs.Contains(c.Substring 2)) ->
                    l.Erreur(Incomplet(Schema.options.chemin, $"l'option {o.levier}={o.id} écrit {c}, absent de curseurs.csv"))
                | _ -> ()

        // stress test
        let rs = t Schema.stress
        l.Uniques rs "id"
        let verdicts = [ "tient"; "fissure"; "casse"; "ouvert" ] |> List.map (fun v -> v, v)

        let attaques =
            rs
            |> List.map (fun r ->
                { Attaque.id = l.Id(r, "id")
                  attaque = texte (r, "attaque_fr")
                  verdict = l.Parmi (r, "verdict") verdicts |> Option.defaultValue "ouvert"
                  verdictAvant =
                    if r["verdict_avant"] = "" then None else l.Parmi (r, "verdict_avant") verdicts
                  pourquoi = texte (r, "pourquoi_fr") })

        // ressources du test de fermeture (France)
        let rr = t Schema.ressourcesFrance
        l.Uniques rr "id"

        let refLevier (r: Rangee) col =
            if not (levierDe.ContainsKey r[col]) then
                l.Erreur(Reference(r.fichier, r.numero, col, r[col], "leviers.csv"))

            r[col]

        let ressourcesFrance =
            rr
            |> List.map (fun r ->
                let importee = entierOpt r "importee_des"

                match importee with
                | Some n when n < 1 || n > 3 -> l.Valeur r "importee_des" "un niveau d'ouverture de 1 à 3, ou vide"
                | _ -> ()

                { RessourcePays.id = l.Id(r, "id")
                  nom = texte (r, "nom_fr")
                  metropole = l.Parmi (r, "metropole") couvertures |> Option.defaultValue "manq"
                  franceEntiere = l.Parmi (r, "france_entiere") couvertures |> Option.defaultValue "manq"
                  importeeDes = importee
                  regleePar = if r["reglee_par"] = "" then None else Some(refLevier r "reglee_par")
                  pourquoi = texte (r, "pourquoi_fr") })

        let idsRessources = ressourcesFrance |> List.map _.id |> Set.ofList

        let ressourcesOptionsFrance =
            t Schema.ressourcesOptionsFrance
            |> List.map (fun r ->
                let lev = refLevier r "levier"

                if not (optionsDe.TryFind lev |> Option.defaultValue [] |> List.exists (fun o -> o.id = r["option"])) then
                    l.Erreur(Reference(r.fichier, r.numero, "option", lev + "=" + r["option"], "options.csv"))

                { OptionRessource.levier = lev
                  option = r["option"]
                  couverture = l.Parmi (r, "couverture") couvertures |> Option.defaultValue "manq"
                  importe = oui r "importe"
                  cout = texte (r, "cout_fr")
                  effet = texte (r, "effet_fr") })

        for res in ressourcesFrance do
            match res.regleePar with
            | Some lev ->
                for o in optionsDe.TryFind lev |> Option.defaultValue [] do
                    if not (ressourcesOptionsFrance |> List.exists (fun x -> x.levier = lev && x.option = o.id)) then
                        l.Erreur(Incomplet(Schema.ressourcesOptionsFrance.chemin, $"pas de couverture pour {lev}={o.id}"))
            | None -> ()

        // échanges (France)
        let importationsFrance =
            t Schema.importationsFrance
            |> List.map (fun r ->
                { Importation.ouverture = l.Entier(r, "ouverture")
                  rang = l.Entier(r, "rang")
                  libelle = texte (r, "libelle_fr")
                  valeur = l.Nombre(r, "valeur")
                  note = texte (r, "note_fr") })

        let calculsExportation = set [ "export-electricite"; "export-nourriture" ]
        let rx = t Schema.exportationsFrance
        l.Uniques rx "id"

        let exportationsFrance =
            rx
            |> List.map (fun r ->
                let valeur =
                    match r["valeur"], r["calcul"] with
                    | "", c when calculsExportation.Contains c -> Calcul c
                    | "", c ->
                        l.Valeur r "calcul" $"""l'un des calculs du moteur : {String.Join(", ", calculsExportation)}"""
                        Fixe 0.0
                    | _, "" -> Fixe(l.Nombre(r, "valeur"))
                    | _ ->
                        l.Valeur r "valeur" "une valeur ou un calcul, pas les deux"
                        Fixe 0.0

                { Exportation.id = l.Id(r, "id")
                  libelle = texte (r, "libelle_fr")
                  parDefaut = oui r "par_defaut"
                  valeur = valeur
                  note = texte (r, "note_fr") })

        // paris
        let rm = t Schema.maturites
        l.Uniques rm "id"

        let maturites =
            rm
            |> List.map (fun r ->
                { Maturite.id = l.Id(r, "id")
                  libelle = texte (r, "libelle_fr")
                  rang = l.Entier(r, "rang") })

        let idsMaturites = maturites |> List.map _.id |> Set.ofList
        let rp = t Schema.paris
        l.Uniques rp "id"

        let paris =
            rp
            |> List.map (fun r ->
                if not (idsMaturites.Contains r["maturite"]) then
                    l.Erreur(Reference(r.fichier, r.numero, "maturite", r["maturite"], "maturites.csv"))

                let couvre =
                    match r["couvre"], r["couverture"] with
                    | "", "" -> None
                    | res, c ->
                        if not (idsRessources.Contains res) then
                            l.Erreur(Reference(r.fichier, r.numero, "couvre", res, "pays/fr/ressources.csv"))

                        Some(res, l.Parmi (r, "couverture") couvertures |> Option.defaultValue "manq")

                { Pari.id = l.Id(r, "id")
                  categorie = texte (r, "categorie_fr")
                  titre = texte (r, "titre_fr")
                  maturite = r["maturite"]
                  pretDans = l.Entier(r, "pret_dans")
                  confort = l.Nombre(r, "confort")
                  energie = l.Nombre(r, "energie")
                  rendement = l.Nombre(r, "rendement")
                  besoin = l.Nombre(r, "besoin")
                  captation = l.Nombre(r, "captation")
                  couvre = couvre
                  avecMonnaie = oui r "avec_monnaie"
                  statut = statut r
                  sources = listeSources r "sources"
                  description = texte (r, "description_fr")
                  effet = texte (r, "effet_fr")
                  echec = texte (r, "echec_fr") })

        compterSansSource Schema.paris.chemin (paris |> List.map _.statut)

        // passage à l'échelle
        let rfa = t Schema.faiblesses
        l.Uniques rfa "id"

        let faiblesses =
            rfa
            |> List.map (fun r ->
                { Faiblesse.id = l.Id(r, "id")
                  titre = texte (r, "titre_fr")
                  probleme = texte (r, "probleme_fr")
                  fait = texte (r, "fait_fr")
                  statut = statut r
                  sources = listeSources r "sources" })

        let idsFaiblesses = faiblesses |> List.map _.id |> Set.ofList

        let parades =
            t Schema.parades
            |> List.map (fun r ->
                if not (idsFaiblesses.Contains r["faiblesse"]) then
                    l.Erreur(Reference(r.fichier, r.numero, "faiblesse", r["faiblesse"], "faiblesses.csv"))

                { Parade.faiblesse = r["faiblesse"]
                  rang = l.Entier(r, "rang")
                  id = l.Id(r, "id")
                  libelle = texte (r, "libelle_fr")
                  adoptee = oui r "adoptee"
                  captation = l.Nombre(r, "captation")
                  automatique =
                    if r["automatique"] = "" then
                        None
                    else
                        l.Parmi
                            (r, "automatique")
                            [ "monnaie-plafonnee", MonnaiePlafonnee
                              "monnaie-avec-ecart", MonnaieAvecEcart
                              "protocole-adopte", ProtocoleAdopte ]
                  automatiqueTexte = texte (r, "automatique_fr") })

        // prévention
        let prevention =
            t Schema.prevention
            |> List.map (fun r ->
                let lev = refLevier r "levier"

                match levierDe.TryFind lev with
                | Some v when v.genre <> OuiNon -> l.Valeur r "levier" "un levier oui-non"
                | _ -> ()

                { Prevention.levier = lev
                  facteur = l.Nombre(r, "facteur")
                  statut = statut r
                  sources = listeSources r "sources"
                  raison = texte (r, "raison_fr")
                  contre = texte (r, "contre_fr")
                  contreSource = refSource r "contre_source" })

        // coefficients du modèle et constantes de la France
        let rco = t Schema.coefficients
        l.Uniques rco "id"

        let coefficients =
            rco
            |> List.map (fun r ->
                { Coefficient.id = l.Id(r, "id")
                  valeur = l.Nombre(r, "valeur")
                  statut = statut r
                  lentille = refLentille r "lentille"
                  lentillePar = provenance r "lentille_par"
                  sources = listeSources r "sources"
                  raison = texte (r, "raison_fr") })

        compterAClasser Schema.coefficients.chemin (coefficients |> List.map _.lentille)

        match coefficients |> List.filter (fun x -> x.lentillePar = "assistant") |> List.length with
        | 0 -> ()
        | n -> l.Avertir(ParUnAssistant(Schema.coefficients.chemin, n))

        compterSansSource Schema.coefficients.chemin (coefficients |> List.map _.statut)

        let rcf = t Schema.constantesFrance
        l.Uniques rcf "id"

        let constantesFrance =
            rcf
            |> List.map (fun r ->
                { Constante.id = l.Id(r, "id")
                  valeur = l.Nombre(r, "valeur")
                  unite = r["unite"]
                  statut = statut r
                  sources = listeSources r "sources"
                  description = texte (r, "description_fr") })

        compterSansSource Schema.constantesFrance.chemin (constantesFrance |> List.map _.statut)

        // le jeu : habitants, tranches, situations, chiffres, voix
        let rh = t Schema.habitants
        l.Uniques rh "id"

        let habitants =
            rh
            |> List.map (fun r ->
                { Habitant.id = l.Id(r, "id")
                  nom = texte (r, "nom")
                  portrait = texte (r, "portrait_fr")
                  provenance = provenance r "provenance" })

        let idsHabitants = habitants |> List.map _.id |> Set.ofList
        let rt = t Schema.tranches
        l.Uniques rt "id"

        let tranches =
            rt
            |> List.map (fun r ->
                let jauges = if r["jauges"] = "" then [] else List.ofArray (r["jauges"].Split '|')

                for j in jauges do
                    if not (idsJauges.Contains j) then
                        l.Erreur(Reference(r.fichier, r.numero, "jauges", j, "jauges.csv"))

                { Tranche.id = l.Id(r, "id")
                  rang = l.Entier(r, "rang")
                  titre = texte (r, "titre_fr")
                  sousTitre = texte (r, "sous_titre_fr")
                  couleur = r["couleur"]
                  embleme = r["embleme"]
                  jauges = jauges
                  provenance = provenance r "provenance" })

        let idsTranches = tranches |> List.map _.id |> Set.ofList
        let idsParis = paris |> List.map _.id |> Set.ofList
        let rsit = t Schema.situations
        l.Uniques rsit "id"

        let situations =
            rsit
            |> List.map (fun r ->
                if not (idsTranches.Contains r["tranche"]) then
                    l.Erreur(Reference(r.fichier, r.numero, "tranche", r["tranche"], "jeu/tranches.csv"))

                let leviersSit = if r["leviers"] = "" then [] else List.ofArray (r["leviers"].Split '|')
                let parisSit = if r["paris"] = "" then [] else List.ofArray (r["paris"].Split '|')

                for b in parisSit do
                    if not (idsParis.Contains b) then
                        l.Erreur(Reference(r.fichier, r.numero, "paris", b, "paris.csv"))

                for v in leviersSit do
                    if not (levierDe.ContainsKey v) then
                        l.Erreur(Reference(r.fichier, r.numero, "leviers", v, "leviers.csv"))

                let genre =
                    l.Parmi (r, "genre") [ "constat", Constat; "dilemme", Dilemme; "prevention", Prevention; "bilan", Bilan; "paris", Paris ]
                    |> Option.defaultValue Constat

                match genre, leviersSit with
                | Dilemme, [ _ ] -> ()
                | Dilemme, _ -> l.Valeur r "leviers" "un seul levier pour un dilemme"
                | Prevention, [] -> l.Valeur r "leviers" "au moins un levier pour la prévention"
                | (Constat | Bilan | Paris), _ :: _ -> l.Valeur r "leviers" "aucun levier pour un constat, un bilan ou des paris"
                | _ -> ()

                let quand =
                    match analyserQuand r["quand"] with
                    | Ok a ->
                        verifierAtomes r "quand" a
                        a
                    | Error m ->
                        l.Valeur r "quand" m
                        []

                { Situation.id = r["id"]
                  tranche = r["tranche"]
                  rang = l.Entier(r, "rang")
                  genre = genre
                  leviers = leviersSit
                  paris = parisSit
                  titre = texte (r, "titre_fr")
                  texte = texte (r, "texte_fr")
                  embleme = r["embleme"]
                  quand = quand
                  provenance = provenance r "provenance" })

        let situationDe = situations |> List.map (fun s -> s.id, s) |> Map.ofList

        let refSituation (r: Rangee) =
            if not (situationDe.ContainsKey r["situation"]) then
                l.Erreur(Reference(r.fichier, r.numero, "situation", r["situation"], "jeu/situations.csv"))

            r["situation"]

        let chiffres =
            t Schema.chiffres
            |> List.map (fun r ->
                { Chiffre.situation = refSituation r
                  rang = l.Entier(r, "rang")
                  valeur = texte (r, "valeur_fr")
                  legende = texte (r, "legende_fr")
                  sources = listeSources r "sources"
                  provenance = provenance r "provenance" })

        let voix =
            t Schema.voix
            |> List.map (fun r ->
                let sit = refSituation r

                if not (idsHabitants.Contains r["habitant"]) then
                    l.Erreur(Reference(r.fichier, r.numero, "habitant", r["habitant"], "jeu/habitants.csv"))

                match situationDe.TryFind sit with
                | Some s ->
                    let ids = s.leviers |> List.collect (fun v -> optionsDe.TryFind v |> Option.defaultValue []) |> List.map _.id

                    if not (List.contains r["option"] ids) then
                        l.Erreur(Reference(r.fichier, r.numero, "option", r["option"], $"les options de la situation {sit}"))
                | None -> ()

                { Voix.situation = sit
                  rang = l.Entier(r, "rang")
                  habitant = r["habitant"]
                  precision = texte (r, "precision_fr")
                  texte = texte (r, "texte_fr")
                  option = r["option"]
                  position = l.Parmi (r, "position") [ "pour", Pour; "contre", Contre; "reserve", Reserve ] |> Option.defaultValue Reserve
                  provenance = provenance r "provenance"
                  positionPar = provenance r "position_par" })

        match voix |> List.filter (fun v -> v.positionPar = "assistant") |> List.length with
        | 0 -> ()
        | n -> l.Avertir(ParUnAssistant(Schema.voix.chemin, n))

        for s in situations do
            match s.genre, s.paris with
            | Paris, [] -> l.Erreur(Incomplet(Schema.situations.chemin, $"la situation {s.id} propose des paris : la colonne paris est vide"))
            | Paris, _ -> ()
            | _, _ :: _ -> l.Erreur(Incomplet(Schema.situations.chemin, $"la situation {s.id} n'est pas une situation de paris : la colonne paris doit rester vide"))
            | _ -> ()

        // événements
        let rev = t Schema.evenements
        l.Uniques rev "id"

        let liste (texte: string) = if texte.Trim() = "" then [] else texte.Split '|' |> Array.map _.Trim() |> List.ofArray
        let nombreDe (s: string) = Double.TryParse(s, NumberStyles.Float, CultureInfo.InvariantCulture)

        let evenements =
            rev
            |> List.map (fun r ->
                let tranche =
                    if r["tranche"] = "" then
                        None
                    else
                        if not (idsTranches.Contains r["tranche"]) then
                            l.Erreur(Reference(r.fichier, r.numero, "tranche", r["tranche"], "jeu/tranches.csv"))

                        Some r["tranche"]

                let quand =
                    match analyserQuand r["quand"] with
                    | Ok a ->
                        verifierAtomes r "quand" a
                        a
                    | Error m ->
                        l.Valeur r "quand" m
                        []

                let siJauge =
                    r["si_jauge"].Split('&', StringSplitOptions.RemoveEmptyEntries ||| StringSplitOptions.TrimEntries)
                    |> List.ofArray
                    |> List.choose (fun c ->
                        let m = Regex.Match(c, "^([a-z-]+)\\s*([<>])\\s*([0-9.]+)$")

                        match m.Success, (if m.Success then nombreDe m.Groups[3].Value else (false, 0.0)) with
                        | true, (true, seuil) when idsJauges.Contains m.Groups[1].Value ->
                            Some(m.Groups[1].Value, (if m.Groups[2].Value = "<" then Inferieur else Superieur), seuil)
                        | _ ->
                            l.Valeur r "si_jauge" "jauge<seuil ou jauge>seuil (seuil entre 0 et 1), séparés par &"
                            None)

                let effets =
                    liste r["effets"]
                    |> List.choose (fun x ->
                        match x.Split ':' with
                        | [| j; p; d |] when idsJauges.Contains j ->
                            match nombreDe p, Int32.TryParse d with
                            | (true, points), (true, duree) when duree >= 1 -> Some { jauge = j; points = points; duree = duree }
                            | _ ->
                                l.Valeur r "effets" "jauge:points:durée (durée en révolutions, au moins 1)"
                                None
                        | _ ->
                            l.Valeur r "effets" "jauge:points:durée, séparés par |"
                            None)

                let reputation =
                    liste r["reputation"]
                    |> List.choose (fun x ->
                        match x.Split ':' with
                        | [| h; d |] when idsHabitants.Contains h ->
                            match nombreDe d with
                            | true, v -> Some(h, v)
                            | _ ->
                                l.Valeur r "reputation" "habitant:points"
                                None
                        | _ ->
                            l.Valeur r "reputation" "habitant:points, séparés par | (habitants de jeu/habitants.csv)"
                            None)

                let probabilite = l.Nombre(r, "probabilite")

                if probabilite < 0.0 || probabilite > 1.0 then
                    l.Valeur r "probabilite" "une probabilité entre 0 et 1"

                { Evenement.id = l.Id(r, "id")
                  tranche = tranche
                  titre = texte (r, "titre_fr")
                  texte = texte (r, "texte_fr")
                  quand = quand
                  siJauge = siJauge
                  probabilite = probabilite
                  uneFois = oui r "une_fois"
                  effets = effets
                  reputation = reputation
                  statut = statut r
                  sources = listeSources r "sources"
                  provenance = provenance r "provenance" })

        match evenements |> List.filter (fun x -> x.provenance = "assistant") |> List.length with
        | 0 -> ()
        | n -> l.Avertir(ParUnAssistant(Schema.evenements.chemin, n))

        // les scènes : des répliques avant une décision
        let rr = t Schema.repliques
        l.Uniques rr "id"
        let idsLiens = liens |> List.map _.id |> Set.ofList

        let repliques =
            rr
            |> List.map (fun r ->
                let sit = refSituation r

                let nature =
                    l.Parmi (r, "nature") [ "dit", Dit; "paraphrase", Paraphrase; "rappel", Rappel; "joueur", Joueur ]
                    |> Option.defaultValue Dit

                let loc = r["locuteur"]

                // chaque locuteur a sa règle : un habitant dit, une lentille paraphrase, Labo rappelle
                match nature with
                | Dit when not (idsHabitants.Contains loc) -> l.Valeur r "locuteur" "un habitant de jeu/habitants.csv (nature « dit »)"
                | Paraphrase when not (idsLentilles.Contains loc) -> l.Valeur r "locuteur" "une lentille de lentilles.csv (nature « paraphrase »)"
                | Rappel when loc <> "labo" -> l.Valeur r "locuteur" "labo (nature « rappel »)"
                | Joueur when loc <> "joueur" -> l.Valeur r "locuteur" "joueur (nature « joueur »)"
                | _ -> ()

                let lien =
                    if r["lien"] = "" then
                        None
                    else
                        if not (idsLiens.Contains r["lien"]) then
                            l.Erreur(Reference(r.fichier, r.numero, "lien", r["lien"], "liens.csv"))

                        Some r["lien"]

                let sources = listeSources r "sources"

                if nature = Paraphrase && sources.IsEmpty && lien.IsNone then
                    l.Valeur r "sources" "une source ou un lien : une lentille ne parle pas sans référence"

                let moment = l.Parmi (r, "moment") [ "avant", Avant; "apres", Apres ] |> Option.defaultValue Avant
                let option = if r["option"] = "" then None else Some r["option"]

                match moment, option, situationDe.TryFind sit with
                | Apres, None, _ -> l.Valeur r "option" "l'option qui déclenche la réaction (moment « apres »)"
                | Avant, Some _, _ -> l.Valeur r "option" "vide avant la décision"
                | Apres, Some o, Some s ->
                    let possibles = (s.leviers |> List.collect (fun v -> optionsDe.TryFind v |> Option.defaultValue []) |> List.map _.id) @ s.paris @ [ "aucun" ]
                    if not (List.contains o possibles) then l.Erreur(Reference(r.fichier, r.numero, "option", o, $"les options de la situation {sit}"))
                | _ -> ()

                let pointsHabitants col =
                    liste r[col]
                    |> List.choose (fun x ->
                        match x.Split ':' with
                        | [| h; d |] when idsHabitants.Contains h ->
                            match nombreDe d with
                            | true, v -> Some(h, v)
                            | _ -> l.Valeur r col "habitant:points"; None
                        | _ -> l.Valeur r col "habitant:points, séparés par |"; None)

                let siConfiance =
                    liste r["si_confiance"]
                    |> List.choose (fun c ->
                        let m = Regex.Match(c, "^([a-z-]+)\\s*([<>])\\s*(-?[0-9.]+)$")

                        match m.Success, (if m.Success then nombreDe m.Groups[3].Value else (false, 0.0)) with
                        | true, (true, seuil) when idsHabitants.Contains m.Groups[1].Value ->
                            Some(m.Groups[1].Value, (if m.Groups[2].Value = "<" then Inferieur else Superieur), seuil)
                        | _ -> l.Valeur r "si_confiance" "habitant>seuil ou habitant<seuil, séparés par |"; None)

                let confiance = pointsHabitants "confiance"

                if not confiance.IsEmpty && nature <> Joueur then
                    l.Valeur r "confiance" "vide : seule une réplique du joueur change la confiance"

                { Replique.situation = sit
                  id = l.Id(r, "id")
                  rang = l.Entier(r, "rang")
                  moment = moment
                  option = option
                  locuteur = loc
                  nature = nature
                  texte = texte (r, "texte_fr")
                  sources = sources
                  lien = lien
                  suite = liste r["suite"]
                  action =
                    if r["action"] = "" then None
                    else l.Parmi (r, "action") [ "idee", Idee; "decider", Decider ]
                  siConfiance = siConfiance
                  confiance = confiance
                  provenance = provenance r "provenance" })

        let repliqueDe = repliques |> List.map (fun x -> x.id, x) |> Map.ofList

        for x in repliques do
            for s in x.suite do
                match repliqueDe.TryFind s with
                | None -> l.Erreur(Incomplet(Schema.repliques.chemin, $"la réplique {x.id} renvoie à {s}, qui n'existe pas"))
                | Some y when y.situation <> x.situation ->
                    l.Erreur(Incomplet(Schema.repliques.chemin, $"la réplique {x.id} renvoie à {s}, d'une autre situation"))
                | _ -> ()

            let suivantes = x.suite |> List.choose repliqueDe.TryFind

            if suivantes.Length > 1 && suivantes |> List.exists (fun y -> y.nature = Joueur) && suivantes |> List.exists (fun y -> y.nature <> Joueur) then
                l.Erreur(Incomplet(Schema.repliques.chemin, $"la réplique {x.id} mêle des répliques du joueur (un choix) et d'autres (un tirage au sort)"))

            if x.suite.IsEmpty && x.moment = Avant && x.action <> Some Decider then
                l.Erreur(Incomplet(Schema.repliques.chemin, $"la réplique {x.id} termine la scène : action « decider » attendue"))

        match repliques |> List.filter (fun x -> x.provenance = "assistant") |> List.length with
        | 0 -> ()
        | n -> l.Avertir(ParUnAssistant(Schema.repliques.chemin, n))

        match l.Erreurs with
        | [] ->
            Ok(
                { sources = sources
                  jauges = jauges
                  jaugesFrance = jaugesFrance
                  leviers = leviers
                  options = options
                  liens = liens
                  lentilles = lentilles
                  curseurs = curseurs
                  attaques = attaques
                  maturites = maturites
                  paris = paris
                  faiblesses = faiblesses
                  parades = parades
                  prevention = prevention
                  coefficients = coefficients
                  constantesFrance = constantesFrance
                  ressourcesFrance = ressourcesFrance
                  ressourcesOptionsFrance = ressourcesOptionsFrance
                  importationsFrance = importationsFrance
                  exportationsFrance = exportationsFrance
                  tranches = tranches
                  situations = situations
                  chiffres = chiffres
                  voix = voix
                  habitants = habitants
                  evenements = evenements
                  repliques = repliques },
                l.Avertissements
            )
        | e -> Error e
