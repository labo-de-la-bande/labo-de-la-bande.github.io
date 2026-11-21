/// `labo servir [dossier] [port]` : sert les fichiers d'un dossier en local (par défaut jeu/ sur le
/// port 8080), pour ouvrir le jeu dans un navigateur. Les modules JavaScript ne se chargent pas
/// depuis un fichier ouvert directement. Bibliothèque standard .NET seulement ; pour le
/// développement, jamais en production.
module Labo.Cli.Servir

open System
open System.IO
open System.Net

let typeDe (chemin: string) =
    match Path.GetExtension(chemin).ToLowerInvariant() with
    | ".html" -> "text/html; charset=utf-8"
    | ".js" -> "text/javascript; charset=utf-8"
    | ".css" -> "text/css; charset=utf-8"
    | ".json" -> "application/json; charset=utf-8"
    | ".svg" -> "image/svg+xml"
    | ".png" -> "image/png"
    | ".jpg"
    | ".jpeg" -> "image/jpeg"
    | ".webp" -> "image/webp"
    | _ -> "application/octet-stream"

let servir (dossier: string) (port: int) =
    let racine = Path.GetFullPath dossier
    use ecoute = new HttpListener()
    ecoute.Prefixes.Add $"http://localhost:{port}/"
    ecoute.Start()
    printfn "Le jeu : http://localhost:%d/ (dossier %s). Ctrl+C pour arrêter." port (Repo.rel racine)

    while true do
        let ctx = ecoute.GetContext()
        let demande = Uri.UnescapeDataString(ctx.Request.Url.AbsolutePath.TrimStart '/')
        let chemin = Path.GetFullPath(Path.Combine(racine, (if demande = "" || demande.EndsWith "/" then demande + "index.html" else demande)))
        let reponse = ctx.Response

        if chemin.StartsWith racine && File.Exists chemin then
            let octets = File.ReadAllBytes chemin
            reponse.ContentType <- typeDe chemin
            reponse.Headers.Add("Cache-Control", "no-store")
            reponse.OutputStream.Write(octets, 0, octets.Length)
        else
            reponse.StatusCode <- 404

        reponse.Close()
