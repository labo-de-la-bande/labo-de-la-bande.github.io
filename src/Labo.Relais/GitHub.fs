/// Poser une question à l'API de GitHub avec le jeton d'un collaborateur (vérifier qu'il peut écrire
/// dans le dépôt). Le jeton n'est ni gardé ni journalisé.
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

// ---------- L'application GitHub de l'atelier ----------
// Le relais écrit dans le dépôt `atelier` (et nulle part ailleurs : l'application n'est installée que
// sur lui, avec le seul droit « Contents »). Sa clé privée est le secret ATELIER_CLE (PKCS#8 en
// base64, posé par `labo atelier cle`) ; chaque requête obtient un jeton d'une heure.

[<Emit("Uint8Array.from(atob($0), c => c.charCodeAt(0))")>]
let private depuisBase64 (s: string) : byte[] = jsNative

[<Emit("btoa(Array.from(new Uint8Array($0), b => String.fromCharCode(b)).join(''))")>]
let private versBase64 (octets: obj) : string = jsNative

[<Emit("new TextEncoder().encode($0)")>]
let private encoder (s: string) : byte[] = jsNative

[<Emit("new TextDecoder('utf-8', { fatal: true }).decode($0)")>]
let private decoder (octets: byte[]) : string = jsNative

[<Emit("crypto.subtle.importKey('pkcs8', $0, { name: 'RSASSA-PKCS1-v1_5', hash: 'SHA-256' }, false, ['sign'])")>]
let private importerCle (der: byte[]) : JS.Promise<obj> = jsNative

[<Emit("crypto.subtle.sign('RSASSA-PKCS1-v1_5', $0, $1)")>]
let private signer (cle: obj) (donnees: byte[]) : JS.Promise<obj> = jsNative

let private base64url (s: string) = s.Replace('+', '-').Replace('/', '_').TrimEnd('=')

/// Un texte en base64 (UTF-8), comme l'API des contenus de GitHub l'attend.
let enBase64 (s: string) = versBase64 (encoder s)

/// Le texte d'un contenu de GitHub (base64 coupé en lignes, UTF-8).
let depuisContenu (b64: string) = decoder (depuisBase64 (b64.Replace("\n", "")))

/// Le jeton JWT de l'application (dix minutes au plus, recul d'une minute pour les horloges).
let jwt (appId: string) (clePkcs8: string) =
    async {
        let maintenant = System.DateTimeOffset.UtcNow.ToUnixTimeSeconds()
        let tete = base64url (enBase64 """{"alg":"RS256","typ":"JWT"}""")
        let charge = base64url (enBase64 (JS.JSON.stringify (createObj [ "iat" ==> maintenant - 60L; "exp" ==> maintenant + 540L; "iss" ==> appId ])))
        let! cle = attendre (importerCle (depuisBase64 clePkcs8))
        let! signature = attendre (signer cle (encoder (tete + "." + charge)))
        return tete + "." + charge + "." + base64url (versBase64 signature)
    }

/// Un jeton d'installation d'une heure, réduit au dépôt de l'atelier et au droit « Contents ».
let jetonAtelier (appId: string) (installation: string) (clePkcs8: string) (depot: string) =
    async {
        let! j = jwt appId clePkcs8
        let nom = depot.Substring(depot.IndexOf '/' + 1)
        let corps = createObj [ "repositories" ==> [| nom |]; "permissions" ==> createObj [ "contents" ==> "write" ] ]
        let! (s, r) = appeler j "POST" $"/app/installations/{installation}/access_tokens" (Some corps)
        return (if s = 201 then Ok(string r?token) else Error $"GitHub refuse le jeton de l'atelier ({s})")
    }
