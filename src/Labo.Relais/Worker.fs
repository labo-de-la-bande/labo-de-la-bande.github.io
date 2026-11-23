/// Le relais des idées sans compte GitHub (Cloudflare Worker). Voir docs/relais.md.
///
///   POST /idees                 une proposition (anti-spam Turnstile, débit limité) : un ticket dans
///                               le dépôt de relecture, ou directement public avec une clé de confiance
///   GET  /idees/<code>          où en est une proposition
///   POST /idees/<code>/oublier  efface l'adresse mail du joueur
///   POST /github                webhook de l'application GitHub : publier, écarter, suivre
///   POST /prenoms               un prénom proposé pour un pays et une décennie (anti-spam, débit
///                               limité) : un compte de plus
///   GET  /prenoms/export        les comptes, pour `labo prenoms importer` (jeton EXPORT_CLE)
///
/// Ce que garde le KV (`IDEES`) : `idee:<code>` (statut, numéros des tickets, adresse facultative,
/// la proposition tant qu'elle n'est pas publiée), `prive:<n>` et `public:<n>` (le code d'un ticket),
/// `debit:<empreinte de l'IP>:<heure>` (compteur), `vip:<empreinte de la clé>` (clés de confiance),
/// `decl:<pays>:<décennie>:<usage>:<prénom>` (le nombre de déclarations, dans les métadonnées).
module Labo.Relais.Worker

open Fable.Core
open Fable.Core.JsInterop
open Labo.Relais.Cloudflare
open Labo.Relais.Proposition

// ---------- Réponses ----------

let origineAdmise (env: obj) (req: Request) =
    match req.headers.get "Origin" with
    | Some o when (variable env "ORIGINES").Split(',') |> Array.exists (fun x -> x.Trim() = o) -> Some o
    | _ -> None

let entetes (env: obj) (req: Request) =
    createObj
        [ "Content-Type" ==> "application/json; charset=utf-8"
          "Access-Control-Allow-Methods" ==> "GET, POST, OPTIONS"
          "Access-Control-Allow-Headers" ==> "Content-Type"
          "Vary" ==> "Origin"
          "Cache-Control" ==> "no-store"
          "X-Content-Type-Options" ==> "nosniff"
          "Referrer-Policy" ==> "no-referrer"
          match origineAdmise env req with
          | Some o -> "Access-Control-Allow-Origin" ==> o
          | None -> () ]

let repondre (env: obj) (req: Request) (statut: int) (corps: obj) =
    nouvelleReponse (JS.JSON.stringify corps) (createObj [ "status" ==> statut; "headers" ==> entetes env req ])

let erreur env req statut (message: string) =
    repondre env req statut (createObj [ "erreur" ==> message ])

// ---------- Le suivi d'une proposition ----------

type Suivi =
    { statut: Statut
      prive: int option
      publique: int option
      url: string option
      mail: string option
      raison: string option
      /// la proposition, gardée jusqu'à sa publication
      proposition: Proposition option }

let texteDe (o: obj) (k: string) =
    match o?(k): obj with
    | null -> ""
    | v when jsTypeof v = "string" -> string v
    | _ -> ""

let optionTexte (o: obj) k = match texteDe o k with "" -> None | t -> Some t
let optionEntier (o: obj) (k: string) : int option = match o?(k): obj with null -> None | v -> Some(unbox<int> v)

let propositionDe (o: obj) =
    { situation = texteDe o "situation"
      idee = texteDe o "idee"
      appuis = texteDe o "appuis"
      provenance = texteDe o "provenance"
      citation = texteDe o "citation"
      mail = optionTexte o "mail" }

let enJson (p: Proposition) =
    createObj
        [ "situation" ==> p.situation
          "idee" ==> p.idee
          "appuis" ==> p.appuis
          "provenance" ==> p.provenance
          "citation" ==> p.citation ]

let lireSuivi (env: obj) (code: string) =
    async {
        let! t = attendre ((kv env).get ("idee:" + code))

        return
            t
            |> Option.bind (fun t ->
                let o = JS.JSON.parse t

                statutDe (texteDe o "statut")
                |> Option.map (fun s ->
                    { statut = s
                      prive = optionEntier o "prive"
                      publique = optionEntier o "publique"
                      url = optionTexte o "url"
                      mail = optionTexte o "mail"
                      raison = optionTexte o "raison"
                      proposition = (match o?proposition with null -> None | p -> Some(propositionDe p)) }))
    }

let ecrireSuivi (env: obj) (code: string) (s: Suivi) =
    let o =
        createObj
            [ "statut" ==> codeStatut s.statut
              "prive" ==> Option.toObj (s.prive |> Option.map box)
              "publique" ==> Option.toObj (s.publique |> Option.map box)
              "url" ==> Option.toObj s.url
              "mail" ==> Option.toObj s.mail
              "raison" ==> Option.toObj s.raison
              "proposition" ==> Option.toObj (s.proposition |> Option.map enJson) ]

    // un an après la dernière nouvelle, le suivi s'efface de lui-même
    attendre ((kv env).put ("idee:" + code, JS.JSON.stringify o, createObj [ "expirationTtl" ==> 31536000 ]))

let pageSuivi (env: obj) (code: string) = variable env "PAGE_SUIVI" + "#suivi=" + code

/// Prévient le joueur, s'il a laissé une adresse et qu'un service d'envoi est configuré.
let prevenir (env: obj) (code: string) (s: Suivi) =
    async {
        match s.mail with
        | Some adresse when variable env "BREVO_CLE" <> "" ->
            let objet, texte = mailStatut s.statut s.url s.raison (pageSuivi env code)

            let corps =
                createObj
                    [ "sender" ==> createObj [ "name" ==> "Le Labo de la Bande"; "email" ==> variable env "MAIL_EXPEDITEUR" ]
                      "to" ==> [| createObj [ "email" ==> adresse ] |]
                      "subject" ==> objet
                      "textContent" ==> texte ]

            let init =
                createObj
                    [ "method" ==> "POST"
                      "headers" ==> createObj [ "api-key" ==> variable env "BREVO_CLE"; "Content-Type" ==> "application/json"; "Accept" ==> "application/json" ]
                      "body" ==> JS.JSON.stringify corps ]

            let! _ = attendre (fetch "https://api.brevo.com/v3/smtp/email" init)
            ()
        | _ -> ()
    }

/// Change le statut, prévient le joueur, et efface son adresse après une décision finale.
let avancer (env: obj) (code: string) (s: Suivi) (statut: Statut) (raison: string option) =
    async {
        let s = { s with statut = statut; raison = raison |> Option.orElse s.raison }
        do! prevenir env code s
        let s = if estFinal statut then { s with mail = None } else s
        do! ecrireSuivi env code s
    }

// ---------- Recevoir une proposition ----------

/// Au plus `LIMITE_PAR_HEURE` propositions par heure et par adresse IP (dont on ne garde que
/// l'empreinte, une heure).
let debitDepasse (env: obj) (req: Request) =
    async {
        let ip = req.headers.get "CF-Connecting-IP" |> Option.defaultValue "inconnue"
        let! empreinte = sha256 ("debit:" + ip)
        let heure = System.DateTimeOffset.UtcNow.ToUnixTimeSeconds() / 3600L
        let cle = $"debit:{empreinte}:{heure}"
        let! t = attendre ((kv env).get cle)
        let n = t |> Option.map int |> Option.defaultValue 0
        let limite = match System.Int32.TryParse(variable env "LIMITE_PAR_HEURE") with | true, l -> l | _ -> 5

        if n >= limite then
            return true
        else
            do! attendre ((kv env).put (cle, string (n + 1), createObj [ "expirationTtl" ==> 3600 ]))
            return false
    }

/// Le corps d'une requête, s'il reste petit (une proposition tient en quelques kilo-octets).
let corpsMaximum = 20000

let lireCorps (req: Request) =
    async {
        match req.headers.get "Content-Length" |> Option.bind (fun l -> match System.Int32.TryParse l with | true, n -> Some n | _ -> None) with
        | Some n when n > corpsMaximum -> return None
        | _ ->
            let! texte = attendre (req.text ())
            return (if texte.Length > corpsMaximum then None else Some texte)
    }

/// Le jeton Turnstile du formulaire est-il valide, émis sur une page du jeu (HOTES) et pour cette
/// action (`idee` ou `prenom`) ? (le widget anti-spam de Cloudflare)
let humain (env: obj) (req: Request) (action: string) (jetonTurnstile: string) =
    async {
        let corps =
            createObj
                [ "secret" ==> variable env "TURNSTILE_SECRET"
                  "response" ==> jetonTurnstile
                  "remoteip" ==> (req.headers.get "CF-Connecting-IP" |> Option.toObj) ]

        let init = createObj [ "method" ==> "POST"; "headers" ==> createObj [ "Content-Type" ==> "application/json" ]; "body" ==> JS.JSON.stringify corps ]
        let! r = attendre (fetch "https://challenges.cloudflare.com/turnstile/v0/siteverify" init)
        let! t = attendre (r.text ())
        let rep = JS.JSON.parse t
        let hotes = (variable env "HOTES").Split(',') |> Array.map _.Trim()

        // la raison d'un refus, sans rien de personnel : les codes d'erreur de Cloudflare, ou ce que
        // le relais vérifie en plus (l'hôte et l'action)
        if rep?success <> box true then
            let codes = match rep?``error-codes`` with null -> [||] | c -> unbox<string[]> c
            return Error(if codes.Length = 0 then "refusé" else String.concat ", " codes)
        elif not (Array.contains (string rep?hostname) hotes) then
            return Error $"hôte inattendu ({string rep?hostname})"
        elif string rep?action <> action then
            return Error $"action inattendue ({string rep?action}, attendue : {action})"
        else
            return Ok()
    }

let etiquettesPubliques = [ "idée"; "via le jeu" ]

let recevoir (env: obj) (req: Request) =
    async {
        let! corps = lireCorps req
        let o = match corps with Some texte -> (try JS.JSON.parse texte with _ -> createObj []) | None -> createObj []

        // le champ piège, invisible pour une personne : un robot le remplit
        if corps.IsNone then
            return erreur env req 413 "proposition trop longue"
        elif texteDe o "site" <> "" then
            return erreur env req 400 "proposition refusée"
        elif (origineAdmise env req).IsNone then
            return erreur env req 403 "origine inconnue"
        else
            let! trop = debitDepasse env req

            if trop then
                return erreur env req 429 "trop de propositions en peu de temps : réessaie dans une heure"
            else
                let! verif = humain env req "idee" (texteDe o "jeton")

                match verif with
                | Error raison -> return erreur env req 400 $"la vérification anti-spam a échoué ({raison}) : recharge la page et réessaie"
                | Ok() ->
                    match valider (propositionDe o) with
                    | Error refus -> return erreur env req 400 (refus |> List.map decrireRefus |> String.concat " ; ")
                    | Ok p ->
                        let cle = texteDe o "cle"
                        let! empreinte = sha256 ("vip:" + cle)
                        let! vip = if cle = "" then async { return None } else attendre ((kv env).get ("vip:" + empreinte))

                        if cle <> "" && vip.IsNone then
                            return erreur env req 403 "cette clé de confiance n'est pas reconnue"
                        else
                            let code = uuid ()
                            let! jeton = GitHub.jeton env

                            if vip.IsSome then
                                let! (n, url) = GitHub.creerTicket jeton (variable env "DEPOT_PUBLIC") (titre p) (corpsPublic p true) etiquettesPubliques
                                do! attendre ((kv env).put ($"public:{n}", code, createObj []))

                                let s =
                                    { statut = Publiee; prive = None; publique = Some n; url = Some url
                                      mail = p.mail; raison = None; proposition = None }

                                do! ecrireSuivi env code s
                                return repondre env req 201 (createObj [ "code" ==> code; "statut" ==> codeStatut Publiee; "ticket" ==> url ])
                            else
                                let! (n, _) = GitHub.creerTicket jeton (variable env "DEPOT_RELECTURE") (titre p) (corpsRelecture p code) [ "à relire" ]
                                do! attendre ((kv env).put ($"prive:{n}", code, createObj []))

                                let s =
                                    { statut = ARelire; prive = Some n; publique = None; url = None
                                      mail = p.mail; raison = None; proposition = Some { p with mail = None } }

                                do! ecrireSuivi env code s
                                return repondre env req 201 (createObj [ "code" ==> code; "statut" ==> codeStatut ARelire ])
    }

let consulter (env: obj) (req: Request) (code: string) =
    async {
        match! lireSuivi env code with
        | None -> return erreur env req 404 "aucune proposition avec ce code"
        | Some s ->
            return
                repondre env req 200 (createObj
                    [ "statut" ==> codeStatut s.statut
                      "libelle" ==> libelleStatut s.statut
                      "ticket" ==> Option.toObj s.url
                      "raison" ==> Option.toObj s.raison
                      "mail" ==> s.mail.IsSome ])
    }

let oublier (env: obj) (req: Request) (code: string) =
    async {
        match! lireSuivi env code with
        | None -> return erreur env req 404 "aucune proposition avec ce code"
        | Some s ->
            do! ecrireSuivi env code { s with mail = None }
            return repondre env req 200 (createObj [ "mail" ==> false ])
    }

// ---------- Le webhook GitHub ----------

let etiquettes (ticket: obj) : string list =
    match ticket?labels with
    | null -> []
    | l -> (unbox<obj[]> l) |> Array.map (fun e -> string e?name) |> List.ofArray

/// Dans le dépôt de relecture : l'étiquette `publier` publie, fermer sans elle écarte.
let relu (env: obj) (charge: obj) =
    async {
        let n = int charge?issue?number
        let! code = attendre ((kv env).get $"prive:{n}")

        match code with
        | None -> ()
        | Some code ->
            match! lireSuivi env code with
            | Some s when s.statut = ARelire ->
                let action = string charge?action
                let jeton () = GitHub.jeton env

                if action = "labeled" && string charge?label?name = etiquettePublier then
                    match s.proposition with
                    | None -> ()
                    | Some p ->
                        let! jeton = jeton ()
                        let! (np, url) = GitHub.creerTicket jeton (variable env "DEPOT_PUBLIC") (titre p) (corpsPublic p false) etiquettesPubliques
                        do! attendre ((kv env).put ($"public:{np}", code, createObj []))
                        do! GitHub.commenter jeton (variable env "DEPOT_RELECTURE") n $"Publiée : {url}"
                        do! GitHub.fermer jeton (variable env "DEPOT_RELECTURE") n
                        do! avancer env code { s with publique = Some np; url = Some url; proposition = None } Publiee None
                elif action = "closed" && not (etiquettes charge?issue |> List.contains etiquettePublier) then
                    let! jeton = jeton ()
                    let! raison = GitHub.dernierCommentaire jeton (variable env "DEPOT_RELECTURE") n
                    do! avancer env code { s with proposition = None } Ecartee raison
            | _ -> ()
    }

/// Dans le dépôt public : les étiquettes « à l'étude », « intégrée », « écartée », et la fermeture.
let suivi (env: obj) (charge: obj) =
    async {
        let n = int charge?issue?number
        let! code = attendre ((kv env).get $"public:{n}")

        match code with
        | None -> ()
        | Some code ->
            match! lireSuivi env code with
            | Some s when not (estFinal s.statut) ->
                let nouveau =
                    match string charge?action with
                    | "labeled" -> statutDeEtiquette (string charge?label?name)
                    | "closed" -> Some(if string charge?issue?state_reason = "not_planned" then Ecartee else Integree)
                    | _ -> None

                match nouveau with
                | Some statut when statut <> s.statut ->
                    let! raison =
                        if statut = Ecartee then
                            async {
                                let! jeton = GitHub.jeton env
                                return! GitHub.dernierCommentaire jeton (variable env "DEPOT_PUBLIC") n
                            }
                        else
                            async { return None }

                    do! avancer env code s statut raison
                | _ -> ()
            | _ -> ()
    }

let webhook (env: obj) (req: Request) =
    async {
        let! texte = attendre (req.text ())
        let! attendu = hmac (variable env "WEBHOOK_SECRET") texte
        let recu = req.headers.get "X-Hub-Signature-256" |> Option.defaultValue ""

        if variable env "WEBHOOK_SECRET" = "" || not (memes recu ("sha256=" + attendu)) then
            return erreur env req 401 "signature invalide"
        else
            if req.headers.get "X-GitHub-Event" = Some "issues" then
                let charge = JS.JSON.parse texte
                let depot = string charge?repository?full_name

                if depot = variable env "DEPOT_RELECTURE" then do! relu env charge
                elif depot = variable env "DEPOT_PUBLIC" then do! suivi env charge

            return repondre env req 200 (createObj [ "ok" ==> true ])
    }

// ---------- Les prénoms déclarés ----------

let declarer (env: obj) (req: Request) =
    async {
        let! corps = lireCorps req
        let o = match corps with Some texte -> (try JS.JSON.parse texte with _ -> createObj []) | None -> createObj []

        if corps.IsNone then
            return erreur env req 413 "envoi trop long"
        elif texteDe o "site" <> "" then
            return erreur env req 400 "déclaration refusée"
        elif (origineAdmise env req).IsNone then
            return erreur env req 403 "origine inconnue"
        elif texteDe o "accord" <> "oui" then
            return erreur env req 400 "confirme que ce prénom se donnait vraiment dans ce pays, à cette génération"
        else
            let! trop = debitDepasse env req

            if trop then
                return erreur env req 429 "trop d'envois en peu de temps : réessaie dans une heure"
            else
                let! verif = humain env req "prenom" (texteDe o "jeton")

                match verif with
                | Error raison -> return erreur env req 400 $"la vérification anti-spam a échoué ({raison}) : recharge la page et réessaie"
                | Ok() ->
                    let decennie = match System.Int32.TryParse(texteDe o "decennie") with | true, d -> d | _ -> 0

                    match validerDeclaration { prenom = texteDe o "prenom"; decennie = decennie; pays = texteDe o "pays"; usage = texteDe o "usage" } with
                    | Error refus -> return erreur env req 400 (String.concat " ; " refus)
                    | Ok d ->
                        let cle = cleDeclaration d
                        let! actuel = attendre ((kv env).get cle)
                        let n = (actuel |> Option.map int |> Option.defaultValue 0) + 1
                        do! attendre ((kv env).put (cle, string n, createObj [ "metadata" ==> createObj [ "n" ==> n ] ]))
                        return repondre env req 201 (createObj [ "ok" ==> true ])
    }

/// Tous les comptes, page après page (KV rend au plus 1000 clés à la fois).
let exporter (env: obj) (req: Request) =
    async {
        let attendu = variable env "EXPORT_CLE"
        let recu = req.headers.get "Authorization" |> Option.defaultValue ""

        if attendu = "" || not (memes recu ("Bearer " + attendu)) then
            return erreur env req 401 "jeton d'export invalide"
        else
            let comptes = ResizeArray<obj>()
            let mutable curseur: string option = None
            let mutable fini = false

            while not fini do
                let options =
                    createObj
                        [ "prefix" ==> "decl:"
                          match curseur with
                          | Some c -> "cursor" ==> c
                          | None -> () ]

                let! page = attendre ((kv env).list options)

                for k in unbox<obj[]> page?keys do
                    match declarationDeCle (string k?name) with
                    | Some d ->
                        let n = match k?metadata with null -> 0 | m -> unbox<int> m?n
                        comptes.Add(createObj [ "pays" ==> d.pays; "decennie" ==> d.decennie; "usage" ==> d.usage; "prenom" ==> d.prenom; "declarations" ==> n ])
                    | None -> ()

                if unbox<bool> page?list_complete then fini <- true else curseur <- Some(string page?cursor)

            return repondre env req 200 (comptes.ToArray())
    }

// ---------- L'aiguillage ----------

let traiter (req: Request) (env: obj) =
    async {
        // https://hôte/chemin?requête → les morceaux du chemin
        let sansHote = req.url.Substring(req.url.IndexOf("://") + 3)
        let chemin = if sansHote.Contains "/" then sansHote.Substring(sansHote.IndexOf '/') else "/"
        let chemin = chemin.Split('?').[0]
        let morceaux = chemin.Split('/', System.StringSplitOptions.RemoveEmptyEntries) |> List.ofArray

        try
            match req.method, morceaux with
            | "OPTIONS", _ -> return nouvelleReponse null (createObj [ "status" ==> 204; "headers" ==> entetes env req ])
            | "POST", [ "idees" ] -> return! recevoir env req
            | "GET", [ "idees"; code ] -> return! consulter env req code
            | "POST", [ "idees"; code; "oublier" ] -> return! oublier env req code
            | "POST", [ "github" ] -> return! webhook env req
            | "POST", [ "prenoms" ] -> return! declarer env req
            | "GET", [ "prenoms"; "export" ] -> return! exporter env req
            | _ -> return erreur env req 404 "rien ici"
        with e ->
            JS.console.error e.Message
            return erreur env req 500 "le relais a un problème : réessaie plus tard"
    }

exportDefault (createObj [ "fetch" ==> System.Func<Request, obj, obj, JS.Promise<Response>>(fun req env _ -> Async.StartAsPromise(traiter req env)) ])
