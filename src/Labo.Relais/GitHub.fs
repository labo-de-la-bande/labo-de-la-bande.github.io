/// Parler à GitHub au nom de l'application du projet : un jeton d'installation, puis les tickets.
module Labo.Relais.GitHub

open Fable.Core
open Fable.Core.JsInterop
open Labo.Relais.Cloudflare

let api = "https://api.github.com"

let entetes (jeton: string) =
    createObj
        [ "Authorization" ==> ("Bearer " + jeton)
          "Accept" ==> "application/vnd.github+json"
          "X-GitHub-Api-Version" ==> "2022-11-28"
          "User-Agent" ==> "labo-de-la-bande-relais"
          "Content-Type" ==> "application/json" ]

/// Un appel à l'API : le code HTTP et la réponse JSON (null si vide).
let appeler (jeton: string) (methode: string) (chemin: string) (corps: obj option) =
    async {
        let init =
            createObj
                [ "method" ==> methode
                  "headers" ==> entetes jeton
                  if corps.IsSome then "body" ==> JS.JSON.stringify corps.Value ]

        let! r = attendre (fetch (api + chemin) init)
        let! texte = attendre (r.text ())
        return r.status, (if texte = "" then null else JS.JSON.parse texte)
    }

/// Le jeton d'installation de l'application : un JWT signé avec sa clé privée, échangé contre un
/// jeton valable une heure, limité aux dépôts où l'application est installée.
let jeton (env: obj) =
    async {
        let maintenant = System.DateTimeOffset.UtcNow.ToUnixTimeSeconds()
        let entete = base64url (utf8 """{"alg":"RS256","typ":"JWT"}""")
        let charge = base64url (utf8 (sprintf """{"iat":%d,"exp":%d,"iss":"%s"}""" (maintenant - 60L) (maintenant + 540L) (variable env "GITHUB_APP_ID")))
        let! signature = signerRs256 (variable env "GITHUB_APP_CLE") (entete + "." + charge)
        let jwt = entete + "." + charge + "." + signature
        let! (statut, json) = appeler jwt "POST" $"""/app/installations/{variable env "GITHUB_INSTALLATION_ID"}/access_tokens""" None

        if statut <> 201 then
            return failwith $"GitHub refuse le jeton d'installation ({statut})"
        else
            return (string json?token)
    }

/// Ouvre un ticket ; rend son numéro et son adresse.
let creerTicket jeton (depot: string) (titre: string) (corps: string) (etiquettes: string list) =
    async {
        let! (statut, json) =
            appeler jeton "POST" $"/repos/{depot}/issues" (Some(createObj [ "title" ==> titre; "body" ==> corps; "labels" ==> List.toArray etiquettes ]))

        if statut <> 201 then
            return failwith $"GitHub refuse le ticket dans {depot} ({statut})"
        else
            return (int json?number, string json?html_url)
    }

let commenter jeton (depot: string) (n: int) (texte: string) =
    appeler jeton "POST" $"/repos/{depot}/issues/{n}/comments" (Some(createObj [ "body" ==> texte ])) |> Async.Ignore

let fermer jeton (depot: string) (n: int) =
    appeler jeton "PATCH" $"/repos/{depot}/issues/{n}" (Some(createObj [ "state" ==> "closed"; "state_reason" ==> "completed" ])) |> Async.Ignore

/// Le dernier commentaire écrit par une personne (pas par un robot) : la raison d'une décision.
let dernierCommentaire jeton (depot: string) (n: int) =
    async {
        let! (statut, json) = appeler jeton "GET" $"/repos/{depot}/issues/{n}/comments?per_page=100" None

        if statut <> 200 then
            return None
        else
            let commentaires: obj[] = unbox json

            return
                commentaires
                |> Array.filter (fun c -> string c?user?``type`` <> "Bot")
                |> Array.tryLast
                |> Option.map (fun c -> string c?body)
    }
