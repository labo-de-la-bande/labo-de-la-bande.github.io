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
