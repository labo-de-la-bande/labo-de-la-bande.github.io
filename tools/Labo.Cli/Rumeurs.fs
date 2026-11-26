/// `labo rumeurs chercher <sujet>` : cherche des fausses nouvelles déjà vérifiées par les
/// vérificateurs de faits (AFP, Libération, Les Décodeurs, Politifact…), dans la base que Google
/// tient à partir de leurs publications (Fact Check Tools API). Le résultat va dans une pile à
/// trier, `.tmp/rumeurs-<sujet>.csv`, jamais directement dans le jeu : quelqu'un choisit ce qui
/// entre dans `data/jeu/rumeurs.csv`, avec la source qui réfute chaque rumeur.
///
/// Il faut une clé (gratuite) dans GOOGLE_FACT_CHECK_KEY ; elle n'est jamais écrite ni affichée.
module Labo.Cli.Rumeurs

open System
open System.IO
open System.Net.Http
open System.Text.Json.Nodes

let api = "https://factchecktools.googleapis.com/v1alpha1/claims:search"

/// Une vérification trouvée : l'affirmation, qui l'a dite, quand, et ce qu'en dit le vérificateur.
type Trouvee =
    { affirmation: string
      qui: string
      quand: string
      verdict: string
      verificateur: string
      titre: string
      url: string
      verifieeLe: string }

let texte (n: JsonNode) (cle: string) =
    match n with
    | null -> ""
    | n ->
        match n[cle] with
        | null -> ""
        | v -> v.GetValue<string>()

/// La date au format AAAA-MM-JJ (la base donne une date et une heure).
let jour (s: string) = if s.Length >= 10 then s.Substring(0, 10) else s

let lirePage (json: JsonNode) =
    match json["claims"] with
    | null -> []
    | claims ->
        [ for c in claims.AsArray() do
              let revues = match c["claimReview"] with null -> [] | r -> List.ofSeq (r.AsArray())

              for r in revues do
                  { affirmation = texte c "text"
                    qui = texte c "claimant"
                    quand = jour (texte c "claimDate")
                    verdict = texte r "textualRating"
                    verificateur = texte r["publisher"] "name"
                    titre = texte r "title"
                    url = texte r "url"
                    verifieeLe = jour (texte r "reviewDate") } ]

let chercher (cle: string) sujet langue site maximum =
    use h = new HttpClient()

    let rec pages (jeton: string option) (acc: Trouvee list) =
        if acc.Length >= maximum then
            acc
        else
            let parametres =
                [ "query", sujet
                  "languageCode", langue
                  "pageSize", "50"
                  "key", cle ]
                @ (match site with Some s -> [ "reviewPublisherSiteFilter", s ] | None -> [])
                @ (match jeton with Some j -> [ "pageToken", j ] | None -> [])

            let requete =
                parametres |> List.map (fun (k, v) -> k + "=" + Uri.EscapeDataString v) |> String.concat "&"

            let r = h.GetAsync(api + "?" + requete).Result
            let corps = r.Content.ReadAsStringAsync().Result

            if not r.IsSuccessStatusCode then
                // le message de Google, jamais l'adresse appelée (elle contient la clé)
                let message =
                    try texte (JsonNode.Parse(corps)["error"]) "message" with _ -> ""

                failwith $"la base de vérifications a refusé la demande ({int r.StatusCode}) : {message}"

            let json = JsonNode.Parse corps
            let acc = acc @ lirePage json

            match texte json "nextPageToken" with
            | "" -> acc
            | suivant -> pages (Some suivant) acc

    pages None [] |> List.truncate maximum

let colonnes = [ "affirmation"; "qui"; "quand"; "verdict"; "verificateur"; "titre"; "url"; "verifiee_le" ]

let ecrire (chemin: string) (ts: Trouvee list) =
    let ligne (t: Trouvee) =
        [ t.affirmation; t.qui; t.quand; t.verdict; t.verificateur; t.titre; t.url; t.verifieeLe ]
        |> List.map Csv.citer
        |> String.concat ","

    Repo.writeText chemin ((String.concat "," colonnes :: List.map ligne ts |> String.concat "\n") + "\n")

let usage =
    """labo rumeurs chercher <sujet> [--langue fr] [--site afp.com] [--max 200]

  Cherche des affirmations déjà vérifiées (AFP, Libération, Les Décodeurs…) sur un sujet, et les
  range dans .tmp/rumeurs-<sujet>.csv, une pile à trier : rien n'entre dans le jeu sans être choisi.
  Clé gratuite (Google Cloud, « Fact Check Tools API ») dans GOOGLE_FACT_CHECK_KEY.
"""

let valeur (args: string list) nom defaut =
    match args |> List.skipWhile ((<>) nom) with
    | _ :: v :: _ -> v
    | _ -> defaut

let executer (args: string list) =
    match args with
    | "chercher" :: sujet :: options when not (sujet.StartsWith "--") ->
        match Environment.GetEnvironmentVariable "GOOGLE_FACT_CHECK_KEY" with
        | null
        | "" ->
            eprintfn "GOOGLE_FACT_CHECK_KEY manque : une clé gratuite de Google Cloud (« Fact Check Tools API »)."
            1
        | cle ->
            try
                let langue = valeur options "--langue" "fr"
                let site = match valeur options "--site" "" with "" -> None | s -> Some s
                let maximum = int (valeur options "--max" "200")
                let ts = chercher cle sujet langue site maximum
                let nom = String(sujet.ToLowerInvariant() |> Seq.map (fun c -> if Char.IsLetterOrDigit c then c else '-') |> Array.ofSeq)
                let chemin = Repo.path [ ".tmp"; $"rumeurs-{nom}.csv" ]
                ecrire chemin ts
                printfn "%d vérification(s) sur « %s » (%s) → %s" ts.Length sujet langue (Repo.rel chemin)

                for (v, n) in ts |> List.countBy _.verificateur |> List.sortByDescending snd |> List.truncate 8 do
                    printfn "  %4d  %s" n (if v = "" then "(vérificateur inconnu)" else v)

                0
            with e ->
                eprintfn "%s" e.Message
                1
    | _ ->
        eprintf "%s" usage
        2
