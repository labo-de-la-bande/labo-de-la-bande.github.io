/// `labo rumeurs telecharger | chercher <sujet>` : des fausses nouvelles déjà vérifiées par des
/// vérificateurs de faits, dans le fichier que Google publie chaque jour sous licence CC BY (Data
/// Commons, format ClaimReview : environ 100 000 vérifications, surtout en anglais, quelques
/// centaines en français). Sans compte, sans clé.
///
/// Le résultat va dans une pile à trier, `.tmp/rumeurs-<sujet>.csv`, jamais directement dans le
/// jeu : quelqu'un choisit ce qui entre dans `data/jeu/rumeurs.csv`, avec la source qui réfute
/// chaque rumeur.
module Labo.Cli.Rumeurs

open System
open System.Globalization
open System.IO
open System.Net.Http
open System.Text
open System.Text.Json

let adresse = "https://storage.googleapis.com/datacommons-feeds/claimreview/latest/data.json"
let fichierLocal () = Repo.path [ ".tmp"; "rumeurs"; "data.json" ]

/// Une vérification : l'affirmation, qui l'a dite, quand, et ce qu'en dit le vérificateur.
type Trouvee =
    { affirmation: string
      qui: string
      quand: string
      verdict: string
      explication: string
      verificateur: string
      url: string
      verifieeLe: string }

let texte (e: JsonElement) (cle: string) =
    match e.ValueKind with
    | JsonValueKind.Object ->
        match e.TryGetProperty cle with
        | true, v when v.ValueKind = JsonValueKind.String -> v.GetString()
        | _ -> ""
    | _ -> ""

let enfant (e: JsonElement) (cle: string) =
    match e.ValueKind with
    | JsonValueKind.Object ->
        match e.TryGetProperty cle with
        | true, v -> v
        | _ -> JsonElement()
    | _ -> JsonElement()

let trouvee (it: JsonElement) =
    let claim = enfant it "itemReviewed"
    let note = enfant it "reviewRating"

    { affirmation = texte it "claimReviewed"
      qui = texte (enfant claim "author") "name"
      quand = texte claim "datePublished"
      verdict = texte note "alternateName"
      explication = texte note "ratingExplanation"
      verificateur = (texte (enfant it "author") "name").Trim()
      url = texte it "url"
      verifieeLe = texte it "datePublished" }

/// Toutes les vérifications d'un fichier au format DataFeed de ClaimReview.
let lireFlux (racine: JsonElement) =
    match racine.ValueKind, enfant racine "dataFeedElement" with
    | JsonValueKind.Object, elements when elements.ValueKind = JsonValueKind.Array ->
        [ for el in elements.EnumerateArray() do
              let items = enfant el "item"

              if items.ValueKind = JsonValueKind.Array then
                  for it in items.EnumerateArray() do
                      if texte it "claimReviewed" <> "" then yield trouvee it ]
    | _ -> []

/// Sans accents ni majuscules, pour chercher un mot.
let normaliser (s: string) =
    let d = s.ToLowerInvariant().Normalize NormalizationForm.FormD

    String(d |> Seq.filter (fun c -> CharUnicodeInfo.GetUnicodeCategory c <> UnicodeCategory.NonSpacingMark) |> Array.ofSeq)

/// Le fichier ne dit pas la langue : on la reconnaît à ses petits mots.
let marqueurs =
    Map
        [ "fr", [ "les"; "des"; "est"; "une"; "pour"; "pas"; "dans"; "qui"; "sur"; "du" ]
          "en", [ "the"; "is"; "are"; "and"; "of"; "to"; "was"; "that"; "for"; "with" ] ]

let langueDe (t: Trouvee) =
    let mots = (normaliser t.affirmation).Split([| ' '; ','; '.'; '\''; '’'; '"' |], StringSplitOptions.RemoveEmptyEntries) |> Set.ofArray
    let score l = marqueurs[l] |> List.filter mots.Contains |> List.length

    match score "fr", score "en" with
    | f, e when f >= 2 && f > e -> "fr"
    | f, e when e >= 2 && e > f -> "en"
    | _ -> ""

let correspond (sujet: string) (t: Trouvee) =
    let s = normaliser sujet
    (normaliser t.affirmation).Contains s || (normaliser t.explication).Contains s

let colonnes = [ "affirmation"; "qui"; "quand"; "verdict"; "explication"; "verificateur"; "url"; "verifiee_le" ]

let ecrire (chemin: string) (ts: Trouvee list) =
    let ligne (t: Trouvee) =
        [ t.affirmation; t.qui; t.quand; t.verdict; t.explication; t.verificateur; t.url; t.verifieeLe ]
        |> List.map (fun v -> Csv.citer (v.Replace("\r", " ").Replace("\n", " ")))
        |> String.concat ","

    Repo.writeText chemin ((String.concat "," colonnes :: List.map ligne ts |> String.concat "\n") + "\n")

let telecharger () =
    let chemin = fichierLocal ()
    Directory.CreateDirectory(Path.GetDirectoryName chemin) |> ignore
    use h = new HttpClient(Timeout = TimeSpan.FromMinutes 10.0)
    printfn "Téléchargement des vérifications de faits (Data Commons, environ 200 Mo)…"
    use flux = h.GetStreamAsync(adresse).Result
    use sortie = File.Create chemin
    flux.CopyTo sortie
    printfn "→ %s (%d Mo)" (Repo.rel chemin) (sortie.Length / 1_000_000L)

let usage =
    """labo rumeurs telecharger
labo rumeurs chercher <sujet> [--langue fr|en|toutes] [--max 300]
labo rumeurs flux [sujet]
labo rumeurs archives [sujet]

  telecharger : le fichier des vérifications de faits que Google publie chaque jour (Data
  Commons, licence CC BY, environ 200 Mo) dans .tmp/rumeurs/, sans compte ni clé.
  chercher : les vérifications dont l'affirmation ou l'explication contient le sujet, dans
  .tmp/rumeurs-<sujet>.csv, une pile à trier. Rien n'entre dans le jeu sans être choisi.
  flux : les derniers articles des vérificateurs français (Les Décodeurs, CheckNews, Les Surligneurs,
  France Info), ajoutés sans doublon à .tmp/rumeurs/flux.csv ; avec un sujet, ceux qui en parlent.
  archives : tout l'historique d'après les plans des sites (Les Surligneurs, France Info, CheckNews),
  dans la même pile ; le titre est deviné d'après l'adresse. Une demi-seconde entre deux requêtes.
"""

let valeur (args: string list) nom defaut =
    match args |> List.skipWhile ((<>) nom) with
    | _ :: v :: _ -> v
    | _ -> defaut

let chercher sujet (options: string list) =
    let chemin = fichierLocal ()

    if not (File.Exists chemin) then
        eprintfn "%s manque : lancer d'abord `labo rumeurs telecharger`." (Repo.rel chemin)
        1
    else
        let langue = valeur options "--langue" "fr"
        let maximum = int (valeur options "--max" "300")
        use doc = JsonDocument.Parse(File.OpenRead chemin)

        let ts =
            lireFlux doc.RootElement
            |> List.filter (fun t -> (langue = "toutes" || langueDe t = langue) && correspond sujet t)
            |> List.sortByDescending _.verifieeLe
            |> List.truncate maximum

        let nom = String(normaliser sujet |> Seq.map (fun c -> if Char.IsLetterOrDigit c then c else '-') |> Array.ofSeq)
        let sortie = Repo.path [ ".tmp"; $"rumeurs-{nom}.csv" ]
        ecrire sortie ts
        printfn "%d vérification(s) sur « %s » (langue : %s) → %s" ts.Length sujet langue (Repo.rel sortie)

        for (v, n) in ts |> List.countBy _.verificateur |> List.sortByDescending snd |> List.truncate 8 do
            printfn "  %4d  %s" n (if v = "" then "(vérificateur inconnu)" else v)

        0


// ---------- Les flux d'actualité des vérificateurs français ----------

/// Les flux lus par `labo rumeurs flux`. L'AFP (factuel.afp.com) refuse les accès automatiques : on
/// ne la lit pas. Le Monde, Libération, Les Surligneurs et France Info publient un flux RSS.
let fluxFrancais =
    [ "Les Décodeurs (Le Monde)", "https://www.lemonde.fr/les-decodeurs/rss_full.xml"
      "CheckNews (Libération)", "https://www.liberation.fr/arc/outboundfeeds/rss/category/checknews/?outputType=xml"
      "Les Surligneurs", "https://www.lessurligneurs.eu/feed/"
      "Vrai ou fake (France Info)", "https://www.francetvinfo.fr/vrai-ou-fake.rss" ]

/// Un article de vérification annoncé par un flux : son titre dit souvent la rumeur et le verdict.
type Article =
    { verificateur: string
      titre: string
      resume: string
      date: string
      lien: string }

let sansBalises (s: string) =
    Text.RegularExpressions.Regex.Replace(s, "<[^>]+>", " ")
    |> Net.WebUtility.HtmlDecode
    |> fun t -> Text.RegularExpressions.Regex.Replace(t, "\\s+", " ").Trim()

/// Les articles d'un flux RSS ; la date en AAAA-MM-JJ quand elle se lit.
let lireRss verificateur (xml: string) =
    let doc = Xml.Linq.XDocument.Parse xml
    let champ (e: Xml.Linq.XElement) (nom: string) = match e.Element(Xml.Linq.XName.Get nom) with null -> "" | x -> x.Value

    [ for item in doc.Descendants(Xml.Linq.XName.Get "item") do
          let date =
              match DateTimeOffset.TryParse(champ item "pubDate", CultureInfo.InvariantCulture, DateTimeStyles.None) with
              | true, d -> d.ToString("yyyy-MM-dd")
              | _ -> ""

          { verificateur = verificateur
            titre = sansBalises (champ item "title")
            resume = sansBalises (champ item "description")
            date = date
            lien = (champ item "link").Trim() } ]

let colonnesArticles = [ "verificateur"; "titre"; "resume"; "date"; "lien" ]

let lirePile (chemin: string) =
    if not (File.Exists chemin) then
        []
    else
        match Csv.analyser (File.ReadAllText chemin) with
        | Ok(_ :: corps) ->
            corps
            |> List.choose (fun l ->
                match l.champs with
                | [ v; t; r; d; li ] -> Some { verificateur = v; titre = t; resume = r; date = d; lien = li }
                | _ -> None)
        | _ -> []

let ecrireArticles (chemin: string) (xs: Article list) =
    let ligne (a: Article) = [ a.verificateur; a.titre; a.resume; a.date; a.lien ] |> List.map Csv.citer |> String.concat ","
    Repo.writeText chemin ((String.concat "," colonnesArticles :: List.map ligne xs |> String.concat "\n") + "\n")

let flux (sujet: string option) =
    let pile = Repo.path [ ".tmp"; "rumeurs"; "flux.csv" ]
    let avant = lirePile pile
    use h = new HttpClient(Timeout = TimeSpan.FromSeconds 30.0)
    h.DefaultRequestHeaders.UserAgent.ParseAdd "labo-de-la-bande (outil de recherche)"

    let lus =
        fluxFrancais
        |> List.collect (fun (nom, url) ->
            try
                let xs = lireRss nom (h.GetStringAsync(url).Result)
                printfn "  %3d  %s" xs.Length nom
                xs
            with e ->
                eprintfn "  ---  %s : illisible (%s)" nom (e.GetBaseException().Message)
                [])

    let connus = avant |> List.map _.lien |> Set.ofList
    let nouveaux = lus |> List.filter (fun a -> a.lien <> "" && not (connus.Contains a.lien)) |> List.distinctBy _.lien
    let tous = (avant @ nouveaux) |> List.sortByDescending _.date
    ecrireArticles pile tous
    printfn "%d nouvel(s) article(s), %d dans la pile → %s" nouveaux.Length tous.Length (Repo.rel pile)

    match sujet with
    | None -> ()
    | Some s ->
        let choisis = tous |> List.filter (fun a -> (normaliser (a.titre + " " + a.resume)).Contains(normaliser s))
        let nom = String(normaliser s |> Seq.map (fun c -> if Char.IsLetterOrDigit c then c else '-') |> Array.ofSeq)
        let sortie = Repo.path [ ".tmp"; $"rumeurs-flux-{nom}.csv" ]
        ecrireArticles sortie choisis
        printfn "%d sur « %s » → %s" choisis.Length s (Repo.rel sortie)

    0

// ---------- Les archives : les plans des sites (sitemaps) ----------

/// Une adresse sans fragment ni paramètres de suivi, pour reconnaître un même article.
let lienPropre (u: string) =
    let u = match u.IndexOf '#' with -1 -> u | i -> u.Substring(0, i)
    if u.Contains "?outputType" then u.Substring(0, u.IndexOf '?') else u

/// Le titre deviné d'après l'adresse : le dernier segment, sans identifiant final.
let titreDepuisAdresse (u: string) =
    let segment = (lienPropre u).TrimEnd('/').Split('/') |> Array.last
    let sansExt = Text.RegularExpressions.Regex.Replace(segment, "(_\\d+)?\\.html?$", "")
    let sansId = Text.RegularExpressions.Regex.Replace(sansExt, "-(\\d{8}_[A-Z0-9]+|\\d+-?\\d*)$", "")
    let t = sansId.Replace('-', ' ').Trim()
    if t = "" then u else string (Char.ToUpperInvariant t[0]) + t.Substring 1

/// Les adresses (et leur date de modification) d'un plan de site ou d'un index de plans.
let lireSitemap (xml: string) =
    let doc = Xml.Linq.XDocument.Parse xml
    let ns = Xml.Linq.XNamespace.Get "http://www.sitemaps.org/schemas/sitemap/0.9"

    let entree (e: Xml.Linq.XElement) =
        let loc = match e.Element(ns + "loc") with null -> "" | x -> x.Value.Trim()
        let mod' = match e.Element(ns + "lastmod") with null -> "" | x -> x.Value.Trim()
        loc, (if mod'.Length >= 10 then mod'.Substring(0, 10) else mod')

    let plans = doc.Descendants(ns + "sitemap") |> Seq.map entree |> List.ofSeq
    let pages = doc.Descendants(ns + "url") |> Seq.map entree |> List.ofSeq
    plans, pages

/// Les archives lues par `labo rumeurs archives` : un index de plans, quels plans garder, quels
/// articles garder. Le Monde publie un plan par jour depuis 1945 : trop de requêtes, son flux suffit.
let archivesFrancaises =
    [ "Les Surligneurs", "https://lessurligneurs.eu/sitemap_index.xml", (fun (p: string) -> p.Contains "post-sitemap"), (fun (u: string) -> not (u.Contains "/wp-content/"))
      "Vrai ou fake (France Info)", "https://www.franceinfo.fr/sitemap_index.xml", (fun (p: string) -> p.EndsWith "-article.xml"), (fun (u: string) -> u.Contains "/vrai-ou-fake/")
      "CheckNews (Libération)", "https://www.liberation.fr/arc/outboundfeeds/sitemap.xml?outputType=xml", (fun (_: string) -> true), (fun (u: string) -> u.Contains "/checknews/") ]

let archives (sujet: string option) =
    let pile = Repo.path [ ".tmp"; "rumeurs"; "flux.csv" ]
    let avant = lirePile pile
    use h = new HttpClient(Timeout = TimeSpan.FromSeconds 60.0)
    h.DefaultRequestHeaders.UserAgent.ParseAdd "labo-de-la-bande (outil de recherche)"
    // une demi-seconde entre deux requêtes : ne pas charger les sites
    let lire (url: string) =
        Threading.Thread.Sleep 500
        h.GetStringAsync(url).Result

    let lus =
        archivesFrancaises
        |> List.collect (fun (nom, index, garderPlan, garderArticle) ->
            try
                let plans, _ = lireSitemap (lire index)
                let plans = plans |> List.map fst |> List.filter garderPlan
                printf "  %s : %d plan(s)…" nom plans.Length

                let articles =
                    plans
                    |> List.collect (fun p ->
                        try snd (lireSitemap (lire p)) with _ -> [])
                    |> List.filter (fst >> garderArticle)
                    |> List.map (fun (u, d) ->
                        { verificateur = nom
                          titre = titreDepuisAdresse u
                          resume = "(titre deviné d'après l'adresse)"
                          date = d
                          lien = lienPropre u })

                printfn " %d article(s)" articles.Length
                articles
            with e ->
                eprintfn "  %s : illisible (%s)" nom (e.GetBaseException().Message)
                [])

    let connus = avant |> List.map (fun a -> lienPropre a.lien) |> Set.ofList
    let nouveaux = lus |> List.filter (fun a -> not (connus.Contains a.lien)) |> List.distinctBy _.lien
    let tous = (avant @ nouveaux) |> List.sortByDescending _.date
    ecrireArticles pile tous
    printfn "%d nouvel(s) article(s), %d dans la pile → %s" nouveaux.Length tous.Length (Repo.rel pile)

    match sujet with
    | None -> ()
    | Some s ->
        let choisis = tous |> List.filter (fun a -> (normaliser (a.titre + " " + a.resume)).Contains(normaliser s))
        let nom = String(normaliser s |> Seq.map (fun c -> if Char.IsLetterOrDigit c then c else '-') |> Array.ofSeq)
        let sortie = Repo.path [ ".tmp"; $"rumeurs-flux-{nom}.csv" ]
        ecrireArticles sortie choisis
        printfn "%d sur « %s » → %s" choisis.Length s (Repo.rel sortie)

    0

let executer (args: string list) =
    try
        match args with
        | [ "telecharger" ] -> telecharger (); 0
        | "chercher" :: sujet :: options when not (sujet.StartsWith "--") -> chercher sujet options
        | [ "flux" ] -> flux None
        | [ "flux"; sujet ] -> flux (Some sujet)
        | [ "archives" ] -> archives None
        | [ "archives"; sujet ] -> archives (Some sujet)
        | _ ->
            eprintf "%s" usage
            2
    with e ->
        eprintfn "%s" e.Message
        1
