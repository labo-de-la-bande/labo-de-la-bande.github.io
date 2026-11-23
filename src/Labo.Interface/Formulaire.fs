/// Ce que partagent les pages à formulaire (proposer une idée, donner son prénom) : lire un champ,
/// monter un morceau de page, le widget anti-spam Turnstile. Ces pages sont construites une fois :
/// recréer l'écran effacerait le widget.
module Labo.Interface.Formulaire

open Browser
open Browser.Types
open Fable.Core
open Fable.Core.JsInterop
open Labo.Interface.Html
open Labo.Interface.Relais

let parametre (nom: string) =
    let recherche = window.location.search.TrimStart('?')

    recherche.Split('&')
    |> Array.tryPick (fun p ->
        match p.Split('=', 2) with
        | [| k; v |] when k = nom -> Some(System.Uri.UnescapeDataString(v.Replace("+", " ")))
        | _ -> None)
    |> Option.defaultValue ""

let valeur (id: string) =
    match document.getElementById id with
    | null -> ""
    | e -> string e?value

let mutable jetonTurnstile = ""

[<Emit("window.turnstile && window.turnstile.reset()")>]
let reinitialiserTurnstile () : unit = jsNative

[<Emit("window.turnstile.render($0, $1)")>]
let afficherTurnstile (cible: string) (options: obj) : unit = jsNative

let champ (id: string) (libelle: string) (aide: string) (saisie: Noeud) =
    div
        "champ"
        [ el "label" [ "for", id ] [ texte libelle ]
          (if aide = "" then Rien else para "aide" [ texte aide ])
          saisie ]

let entree id typ (v: string) attrs = el "input" ([ "id", id; "type", typ; "value", v ] @ attrs) []
let zone id (v: string) lignes = el "textarea" [ "id", id; "rows", string lignes ] [ texte v ]

let monterDans (id: string) (n: Noeud) =
    match document.getElementById id with
    | null -> ()
    | e -> monter (unbox e) n

let message (cls: string) (enfants: Noeud list) = monterDans "message" (div ("message " + cls) enfants)

let lienExterne (url: string) (t: string) = el "a" [ "href", url; "target", "_blank"; "rel", "noopener" ] [ texte t ]


/// Charge le widget anti-spam dans l'élément `#turnstile`, pour une action (`idee`, `prenom`) que le
/// relais vérifie ; son jeton arrive dans `jetonTurnstile`.
let chargerTurnstile (action: string) =
    window?laboTurnstile <-
        fun () ->
            afficherTurnstile
                "#turnstile"
                (createObj
                    [ "sitekey" ==> cleTurnstile
                      "action" ==> action
                      "language" ==> "fr"
                      "callback" ==> (fun (t: string) -> jetonTurnstile <- t)
                      "expired-callback" ==> (fun () -> jetonTurnstile <- "") ])

    let s = document.createElement "script"
    s.setAttribute ("src", "https://challenges.cloudflare.com/turnstile/v0/api.js?render=explicit&onload=laboTurnstile")
    s.setAttribute ("async", "")
    document.head.appendChild s |> ignore
