/// Envoyer une idée au projet : directement sur GitHub (avec un compte), ou par le relais (sans
/// compte, relue avant publication). Le relais : src/Labo.Relais, mise en place dans docs/relais.md.
module Labo.Interface.Relais

open Fable.Core
open Fable.Core.JsInterop

/// L'adresse du relais une fois déployé (vide : l'envoi sans compte n'est pas encore ouvert).
let adresse = "https://labo-relais.colet-patrice.workers.dev"

/// La clé publique du widget anti-spam Turnstile (vide tant que le relais n'est pas déployé).
let cleTurnstile = "0x4AAAAAAGOV9FjpILWIq0Kf"

let ouvert = adresse <> "" && cleTurnstile <> ""

/// Les idées sans compte demandent en plus l'application GitHub qui ouvre les tickets (docs/relais.md,
/// étape 4) : tant qu'elle n'est pas installée, seule la page des prénoms est ouverte.
let ideesSansCompte = false

let depot = "https://github.com/labo-de-la-bande/labo-de-la-bande.github.io"

/// Le formulaire GitHub `idee.yml`, prérempli.
let lienGitHub (situation: string) (idee: string) (appuis: string) (citation: string) =
    let titre = "Idée : " + (if idee.Length > 60 then idee.Substring(0, 60) + "…" else idee)
    let e (s: string) = System.Uri.EscapeDataString s

    $"{depot}/issues/new?template=idee.yml&title={e titre}&situation={e situation}&idee={e idee}&appuis={e appuis}&citation={e citation}"

/// La page qui envoie une idée, préremplie depuis le jeu.
let lienProposer (situation: string) (idee: string) =
    let e (s: string) = System.Uri.EscapeDataString s
    $"proposer.html?situation={e situation}&idee={e idee}"

/// La page qui propose un prénom aux banques : celui que le joueur a donné à un habitant, avec sa
/// décennie de naissance, son origine (`ici` ou un groupe de pays) et son sexe (`f`, `m`, `x`).
let lienProposerPrenom (prenom: string) (decennie: int) (origine: string) (usage: string) =
    let e (s: string) = System.Uri.EscapeDataString s
    $"prenom.html?prenom={e prenom}&decennie={decennie}&origine={e origine}&usage={e usage}"

// ---------- Les propositions de ce navigateur ----------

/// Une proposition envoyée depuis ce navigateur : son code de suivi, sa date, son début.
type Envoyee = { code: string; date: string; debut: string }

let cleStockage = "labo-propositions"
let cleConfiance = "labo-cle-confiance"

let lireEnvoyees () : Envoyee list =
    try
        match Browser.WebStorage.localStorage.getItem cleStockage with
        | null -> []
        | t ->
            (JS.JSON.parse t :?> obj[])
            |> Array.map (fun o -> { code = string o?code; date = string o?date; debut = string o?debut })
            |> List.ofArray
    with _ ->
        []

let garder (e: Envoyee) =
    let tous = e :: (lireEnvoyees () |> List.filter (fun x -> x.code <> e.code))

    let enJs =
        tous |> List.map (fun x -> createObj [ "code" ==> x.code; "date" ==> x.date; "debut" ==> x.debut ]) |> Array.ofList

    try Browser.WebStorage.localStorage.setItem (cleStockage, JS.JSON.stringify enJs) with _ -> ()

let lireConfiance () =
    try
        match Browser.WebStorage.localStorage.getItem cleConfiance with
        | null -> ""
        | t -> t
    with _ ->
        ""

let garderConfiance (cle: string) =
    try
        if cle = "" then Browser.WebStorage.localStorage.removeItem cleConfiance
        else Browser.WebStorage.localStorage.setItem (cleConfiance, cle)
    with _ ->
        ()

// ---------- Parler au relais ----------

[<Emit("fetch($0, $1)")>]
let private fetch (url: string) (init: obj) : JS.Promise<obj> = jsNative

/// Un appel au relais : le code HTTP et la réponse JSON.
let appeler (methode: string) (chemin: string) (corps: obj option) =
    async {
        let init =
            createObj
                [ "method" ==> methode
                  if corps.IsSome then
                      "headers" ==> createObj [ "Content-Type" ==> "application/json" ]
                      "body" ==> JS.JSON.stringify corps.Value ]

        let! r = Async.AwaitPromise(fetch (adresse + chemin) init)
        let! json = Async.AwaitPromise(r?json () : JS.Promise<obj>)
        return (unbox<int> r?status), json
    }
