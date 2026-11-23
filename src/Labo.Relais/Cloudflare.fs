/// Ce dont le relais a besoin du moteur JavaScript de Cloudflare Workers : requêtes, réponses,
/// stockage clé-valeur (KV), Web Crypto. Le strict nécessaire, déclaré pour Fable.
module Labo.Relais.Cloudflare

open Fable.Core
open Fable.Core.JsInterop

type Headers =
    abstract get: string -> string option

type Request =
    abstract method: string
    abstract url: string
    abstract headers: Headers
    abstract text: unit -> JS.Promise<string>

type Response =
    abstract status: int
    abstract ok: bool
    abstract text: unit -> JS.Promise<string>

/// Un espace KV : des textes rangés par clé, avec une durée de vie facultative.
type Kv =
    abstract get: string -> JS.Promise<string option>
    abstract put: string * string * obj -> JS.Promise<unit>
    abstract delete: string -> JS.Promise<unit>
    /// les clés qui commencent par un préfixe, par pages : { keys: [{ name, metadata }], list_complete, cursor }
    abstract list: obj -> JS.Promise<obj>

[<Emit("fetch($0, $1)")>]
let fetch (url: string) (init: obj) : JS.Promise<Response> = jsNative

[<Emit("new Response($0, $1)")>]
let nouvelleReponse (corps: string) (init: obj) : Response = jsNative

[<Emit("crypto.randomUUID()")>]
let uuid () : string = jsNative

[<Emit("crypto.subtle.digest('SHA-256', $0)")>]
let empreinteBrute (donnees: byte[]) : JS.Promise<obj> = jsNative

[<Emit("new Uint8Array($0)")>]
let enOctets (tampon: obj) : byte[] = jsNative

let attendre (p: JS.Promise<'T>) = Async.AwaitPromise p

let utf8 (s: string) = System.Text.Encoding.UTF8.GetBytes s

let hex (octets: byte[]) =
    octets |> Array.map (fun b -> (int b).ToString("x2")) |> String.concat ""

/// SHA-256 d'un texte, en hexadécimal.
let sha256 (s: string) =
    async {
        let! b = attendre (empreinteBrute (utf8 s))
        return hex (enOctets b)
    }

/// Une variable ou un secret de l'environnement du Worker (vide si absent).
let variable (env: obj) (nom: string) : string =
    match env?(nom) with
    | null -> ""
    | v -> string v

let kv (env: obj) : Kv = env?IDEES
