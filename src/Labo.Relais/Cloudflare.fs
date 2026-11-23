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

[<Emit("crypto.subtle.importKey($0, $1, $2, false, $3)")>]
let importerCle (format: string) (cle: byte[]) (algorithme: obj) (usages: string[]) : JS.Promise<obj> = jsNative

[<Emit("crypto.subtle.sign($0, $1, $2)")>]
let signerBrut (algorithme: obj) (cle: obj) (donnees: byte[]) : JS.Promise<obj> = jsNative

[<Emit("crypto.subtle.digest('SHA-256', $0)")>]
let empreinteBrute (donnees: byte[]) : JS.Promise<obj> = jsNative

[<Emit("new Uint8Array($0)")>]
let enOctets (tampon: obj) : byte[] = jsNative

let attendre (p: JS.Promise<'T>) = Async.AwaitPromise p

let utf8 (s: string) = System.Text.Encoding.UTF8.GetBytes s

let hex (octets: byte[]) =
    octets |> Array.map (fun b -> (int b).ToString("x2")) |> String.concat ""

let base64url (octets: byte[]) =
    System.Convert.ToBase64String(octets).Replace('+', '-').Replace('/', '_').TrimEnd('=')

/// SHA-256 d'un texte, en hexadécimal.
let sha256 (s: string) =
    async {
        let! b = attendre (empreinteBrute (utf8 s))
        return hex (enOctets b)
    }

/// HMAC-SHA256 d'un texte avec une clé, en hexadécimal (signature des webhooks GitHub).
let hmac (cle: string) (s: string) =
    async {
        let algo = createObj [ "name" ==> "HMAC"; "hash" ==> "SHA-256" ]
        let! k = attendre (importerCle "raw" (utf8 cle) algo [| "sign" |])
        let! b = attendre (signerBrut algo k (utf8 s))
        return hex (enOctets b)
    }

/// Égalité en temps constant, pour comparer des signatures.
let memes (a: string) (b: string) =
    a.Length = b.Length && (Seq.zip a b |> Seq.fold (fun acc (x, y) -> acc ||| (int x ^^^ int y)) 0) = 0

/// Signe un texte en RS256 avec une clé privée PKCS#8 au format PEM (jeton de l'application GitHub).
let signerRs256 (pem: string) (s: string) =
    async {
        let corps =
            pem.Split('\n')
            |> Array.map _.Trim()
            |> Array.filter (fun l -> l <> "" && not (l.StartsWith "-----"))
            |> String.concat ""

        let algo = createObj [ "name" ==> "RSASSA-PKCS1-v1_5"; "hash" ==> "SHA-256" ]
        let! k = attendre (importerCle "pkcs8" (System.Convert.FromBase64String corps) algo [| "sign" |])
        let! b = attendre (signerBrut algo k (utf8 s))
        return base64url (enOctets b)
    }

/// Une variable ou un secret de l'environnement du Worker (vide si absent).
let variable (env: obj) (nom: string) : string =
    match env?(nom) with
    | null -> ""
    | v -> string v

let kv (env: obj) : Kv = env?IDEES
