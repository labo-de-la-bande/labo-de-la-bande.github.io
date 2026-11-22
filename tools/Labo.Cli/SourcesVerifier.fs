/// `labo sources verifier` : va sur internet et rend un rapport, sans rien modifier.
/// - chaque adresse répond-elle encore ? (une page disparue, un accès refusé)
/// - pour chaque DOI, Crossref connaît-il un avis qui le met à jour : rétractation, correction,
///   expression de préoccupation… (les données de Retraction Watch y sont intégrées)
/// Ce qui en sort est une liste à lire : c'est une personne qui décide quoi changer.
module Labo.Cli.SourcesVerifier

open System
open System.IO
open System.Net
open System.Net.Http
open System.Text
open System.Text.Json
open System.Threading

type Etat =
    | Repond
    | Refuse of code: int
    | Introuvable of code: int
    /// une redirection que le client ne suit pas (vers http, par exemple)
    | Redirige of code: int
    | Injoignable of raison: string

let client =
    let h = new HttpClientHandler(AllowAutoRedirect = true, AutomaticDecompression = DecompressionMethods.All)
    let c = new HttpClient(h, Timeout = TimeSpan.FromSeconds 20.0)
    // un nom de client générique : rien qui désigne le projet ni la personne
    c.DefaultRequestHeaders.UserAgent.ParseAdd "Mozilla/5.0 (compatible; labo-sources/1.0)"
    c

let verifierAdresse (url: string) =
    task {
        try
            use! r = client.GetAsync(url, HttpCompletionOption.ResponseHeadersRead)
            let code = int r.StatusCode

            return
                if r.IsSuccessStatusCode then Repond
                elif code = 404 || code = 410 then Introuvable code
                elif code >= 300 && code < 400 then Redirige code
                else Refuse code
        with e ->
            return Injoignable(e.GetType().Name)
    }

/// Les avis Crossref qui mettent à jour un DOI : (type, date, DOI de l'avis).
let avisCrossref (doi: string) =
    task {
        try
            let url = "https://api.crossref.org/works?filter=updates:" + Uri.EscapeDataString doi + "&rows=20"
            let! texte = client.GetStringAsync url
            use j = JsonDocument.Parse texte

            return
                Ok
                    [ for item in j.RootElement.GetProperty("message").GetProperty("items").EnumerateArray() do
                          let avis = match item.TryGetProperty "DOI" with | true, v -> v.GetString() | _ -> ""

                          match item.TryGetProperty "update-to" with
                          | true, maj ->
                              for u in maj.EnumerateArray() do
                                  let cible = match u.TryGetProperty "DOI" with | true, v -> v.GetString() | _ -> ""

                                  if String.Equals(cible, doi, StringComparison.OrdinalIgnoreCase) then
                                      let genre = match u.TryGetProperty "type" with | true, v -> v.GetString() | _ -> "mise à jour"

                                      let date =
                                          match u.TryGetProperty "updated" with
                                          | true, d ->
                                              match d.TryGetProperty "date-parts" with
                                              | true, p -> String.Join("-", p[0].EnumerateArray() |> Seq.map (fun x -> string (x.GetInt32())))
                                              | _ -> ""
                                          | _ -> ""

                                      yield genre, date, avis
                          | _ -> () ]
        with e ->
            return Error(e.GetType().Name)
    }

let executer () =
    match Validate.charger (Repo.path [ "data" ]) with
    | Error es ->
        for e in es do eprintfn "✗ %s" (Schema.decrire e)
        1
    | Ok(d, _) ->
        let adresses =
            [ for s in d.sources do
                  if s.url <> "" then yield s.id, s.url
                  for (_, u) in s.documents do
                      if u <> "" then yield s.id, u ]

        printfn "%d adresses et %d DOI à vérifier…" adresses.Length (d.sources |> List.filter (fun s -> s.doi <> "") |> List.length)

        // huit à la fois, pour ne pas assaillir les sites
        let limite = new SemaphoreSlim(8)

        let resultats =
            adresses
            |> List.map (fun (id, url) ->
                task {
                    do! limite.WaitAsync()

                    try
                        let! e = verifierAdresse url
                        return id, url, e
                    finally
                        limite.Release() |> ignore
                })
            |> Tasks.Task.WhenAll
            |> fun t -> t.Result

        let avis =
            [ for s in d.sources do
                  if s.doi <> "" then
                      yield s.id, s.doi, (avisCrossref s.doi).Result ]

        let rapport = StringBuilder()
        let ligne (t: string) = rapport.AppendLine t |> ignore
        ligne ("# Vérification des sources (" + DateTime.Today.ToString("yyyy-MM-dd") + ")")
        ligne ""
        ligne "Rien n'a été modifié : ce rapport est à lire, une personne décide."
        ligne ""

        let section titre (lignes: string list) =
            ligne $"## {titre} ({lignes.Length})"
            ligne ""
            for l in lignes do ligne ("- " + l)
            ligne ""

        section "Rétractations, corrections et autres avis (Crossref)"
            [ for (id, doi, r) in avis do
                  match r with
                  | Ok [] -> ()
                  | Ok l -> for (g, date, a) in l do yield $"`{id}` ({doi}) : {g}, {date}, avis {a}"
                  | Error e -> yield $"`{id}` ({doi}) : Crossref n'a pas répondu ({e})" ]

        section "Pages introuvables"
            [ for (id, url, e) in resultats do
                  match e with
                  | Introuvable c -> yield $"`{id}` : {url} ({c})"
                  | _ -> () ]

        section "Sites injoignables"
            [ for (id, url, e) in resultats do
                  match e with
                  | Injoignable r -> yield $"`{id}` : {url} ({r})"
                  | _ -> () ]

        section "Redirections à suivre à la main"
            [ for (id, url, e) in resultats do
                  match e with
                  | Redirige c -> yield $"`{id}` : {url} ({c})"
                  | _ -> () ]

        section "Accès refusé à un programme (souvent un site qui bloque les robots : à vérifier à la main)"
            [ for (id, url, e) in resultats do
                  match e with
                  | Refuse c -> yield $"`{id}` : {url} ({c})"
                  | _ -> () ]

        let n = resultats |> Array.filter (fun (_, _, e) -> e = Repond) |> Array.length
        ligne $"{n} adresses sur {resultats.Length} répondent normalement."

        let sortie = Repo.path [ ".tmp"; "sources-rapport.md" ]
        Directory.CreateDirectory(Path.GetDirectoryName sortie) |> ignore
        File.WriteAllText(sortie, rapport.ToString())
        printfn "%s" (rapport.ToString())
        printfn "rapport : %s" (Repo.rel sortie)
        0
