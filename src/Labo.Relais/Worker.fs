/// Le relais du Labo (Cloudflare Worker) : les idées des joueurs sans compte GitHub, et les prénoms
/// qu'ils proposent. Voir docs/relais.md.
///
/// Pour les joueurs (anti-spam Turnstile, débit limité, depuis le site du jeu seulement) :
///   POST /idees                 une idée : elle attend la relecture d'un collaborateur
///   GET  /idees/<code>          où en est une idée (le code de suivi du joueur)
///   POST /idees/<code>/oublier  efface l'adresse mail du joueur
///   POST /prenoms               un prénom proposé pour un pays et une décennie : un compte de plus
///
/// Pour les collaborateurs du dépôt GitHub (en-tête `Authorization: Bearer <jeton gh>` : le relais
/// demande à GitHub si ce compte peut écrire dans le dépôt, et n'en garde rien) :
///   GET  /moderation/idees                     les idées à relire, et celles publiées à suivre
///   POST /moderation/idees/<code>/publier      { ticket, url } : publiée par ce collaborateur
///   POST /moderation/idees/<code>/ecarter      { raison }
///   POST /moderation/idees/<code>/statut       { statut, raison } : à l'étude, intégrée, écartée
///   GET  /moderation/prenoms                   les comptes de prénoms proposés
///   GET  /moderation/atelier/invitations       les invités de l'atelier
///   POST /moderation/atelier/invitations       { id, nom } : une invitation ; le code n'est rendu qu'une fois
///   POST /moderation/atelier/invitations/<id>/retirer
///
/// Pour les invités de l'atelier (en-tête `Authorization: Bearer atl_…`, depuis le site du jeu ;
/// 20 essais ratés par heure et par IP, 60 modifications par heure et par invité) :
///   GET  /atelier/moi                          qui est l'invité
///   GET  /atelier/tables                       les tables ouvertes, lues dans le dépôt de l'atelier
///   POST /atelier/modifications                une modification : un commit dans le dépôt de l'atelier
///
/// Ce que garde le KV (`IDEES`) : `idee:<code>` (statut, ticket public, adresse facultative, la
/// proposition tant qu'elle n'est pas publiée ; le statut aussi dans les métadonnées),
/// `debit:<empreinte de l'IP>:<heure>` (compteur), `decl:<pays>:<décennie>:<usage>:<prénom>` (le
/// nombre de propositions, dans les métadonnées).
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
          "Access-Control-Allow-Headers" ==> "Content-Type, Authorization"
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
      publique: int option
      url: string option
      mail: string option
      raison: string option
      /// la proposition, gardée jusqu'à sa publication
      proposition: Proposition option
      /// le collaborateur qui l'a relue (compte GitHub)
      par: string option }

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
                      publique = optionEntier o "publique"
                      url = optionTexte o "url"
                      mail = optionTexte o "mail"
                      raison = optionTexte o "raison"
                      proposition = (match o?proposition with null -> None | p -> Some(propositionDe p))
                      par = optionTexte o "par" }))
    }

let ecrireSuivi (env: obj) (code: string) (s: Suivi) =
    let o =
        createObj
            [ "statut" ==> codeStatut s.statut
              "publique" ==> Option.toObj (s.publique |> Option.map box)
              "url" ==> Option.toObj s.url
              "mail" ==> Option.toObj s.mail
              "raison" ==> Option.toObj s.raison
              "proposition" ==> Option.toObj (s.proposition |> Option.map enJson)
              "par" ==> Option.toObj s.par ]

    // un an après la dernière nouvelle, le suivi s'efface de lui-même
    let options = createObj [ "expirationTtl" ==> 31536000; "metadata" ==> createObj [ "statut" ==> codeStatut s.statut ] ]
    attendre ((kv env).put ("idee:" + code, JS.JSON.stringify o, options))

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
                        let code = uuid ()

                        let s =
                            { statut = ARelire; publique = None; url = None; mail = p.mail; raison = None
                              proposition = Some { p with mail = None }; par = None }

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


// ---------- Les collaborateurs ----------

/// Le compte GitHub derrière le jeton, s'il peut écrire dans le dépôt public (collaborateur). Le
/// jeton ne sert qu'à poser la question à GitHub : il n'est ni gardé ni écrit nulle part.
let collaborateur (env: obj) (req: Request) =
    async {
        match req.headers.get "Authorization" with
        | Some a when a.StartsWith "Bearer " && a.Length > 7 ->
            let jeton = a.Substring 7
            let! (s1, moi) = GitHub.appeler jeton "GET" "/user" None

            if s1 <> 200 then
                return Error "GitHub ne reconnaît pas ce jeton"
            else
                let login = string moi?login
                let depot = variable env "DEPOT_PUBLIC"
                let! (s2, droit) = GitHub.appeler jeton "GET" $"/repos/{depot}/collaborators/{login}/permission" None

                match s2, (if s2 = 200 then string droit?permission else "") with
                | 200, ("admin" | "maintain" | "write") -> return Ok login
                | _ -> return Error $"{login} n'est pas collaborateur du dépôt"
        | _ -> return Error "il faut un jeton GitHub (gh auth token)"
    }

let lireJson (req: Request) =
    async {
        let! corps = lireCorps req
        return (match corps with Some t -> (try JS.JSON.parse t with _ -> createObj []) | None -> createObj [])
    }

/// Les idées à relire, et celles publiées dont le sort reste à suivre.
let idees (env: obj) (req: Request) (_: string) =
    async {
        let liste = ResizeArray<obj>()
        let mutable curseur: string option = None
        let mutable fini = false

        while not fini do
            let options =
                createObj
                    [ "prefix" ==> "idee:"
                      match curseur with
                      | Some c -> "cursor" ==> c
                      | None -> () ]

            let! page = attendre ((kv env).list options)

            for k in unbox<obj[]> page?keys do
                let code = (string k?name).Substring 5
                let statut = match k?metadata with null -> None | m -> statutDe (string m?statut)

                if statut |> Option.exists (estFinal >> not) then
                    match! lireSuivi env code with
                    | Some s ->
                        liste.Add(
                            createObj
                                [ "code" ==> code
                                  "statut" ==> codeStatut s.statut
                                  "ticket" ==> Option.toObj (s.publique |> Option.map box)
                                  "url" ==> Option.toObj s.url
                                  "par" ==> Option.toObj s.par
                                  "mail" ==> s.mail.IsSome
                                  "proposition" ==> Option.toObj (s.proposition |> Option.map enJson) ]
                        )
                    | None -> ()

            if unbox<bool> page?list_complete then fini <- true else curseur <- Some(string page?cursor)

        return repondre env req 200 (liste.ToArray())
    }

let publier (env: obj) (req: Request) (login: string) (code: string) =
    async {
        let! o = lireJson req

        match! lireSuivi env code with
        | Some s when s.statut = ARelire ->
            match o?ticket with
            | null -> return erreur env req 400 "il manque le numéro du ticket public"
            | n ->
                let s = { s with publique = Some(unbox<int> n); url = optionTexte o "url"; proposition = None; par = Some login }
                do! avancer env code s Publiee None
                return repondre env req 200 (createObj [ "statut" ==> codeStatut Publiee ])
        | Some _ -> return erreur env req 409 "cette idée n'est plus à relire"
        | None -> return erreur env req 404 "aucune idée avec ce code"
    }

let ecarter (env: obj) (req: Request) (login: string) (code: string) =
    async {
        let! o = lireJson req

        match! lireSuivi env code with
        | Some s when not (estFinal s.statut) ->
            do! avancer env code { s with proposition = None; par = Some login } Ecartee (optionTexte o "raison")
            return repondre env req 200 (createObj [ "statut" ==> codeStatut Ecartee ])
        | Some _ -> return erreur env req 409 "cette idée a déjà sa réponse"
        | None -> return erreur env req 404 "aucune idée avec ce code"
    }

/// Le sort d'une idée publiée : à l'étude, intégrée, écartée (suivi du ticket public).
let changerStatut (env: obj) (req: Request) (login: string) (code: string) =
    async {
        let! o = lireJson req

        let! suivi = lireSuivi env code

        match suivi, statutDe (texteDe o "statut") with
        | Some s, Some statut when s.publique.IsSome && not (estFinal s.statut) && statut <> s.statut && (statut = ALEtude || estFinal statut) ->
            do! avancer env code { s with par = Some login } statut (optionTexte o "raison")
            return repondre env req 200 (createObj [ "statut" ==> codeStatut statut ])
        | Some _, _ -> return repondre env req 200 (createObj [ "statut" ==> "inchange" ])
        | None, _ -> return erreur env req 404 "aucune idée avec ce code"
    }

/// Tous les comptes de prénoms proposés, page après page (KV rend au plus 1000 clés à la fois).
let comptesPrenoms (env: obj) (req: Request) (_: string) =
    async {
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

/// Une route réservée aux collaborateurs.
let reserve (env: obj) (req: Request) (action: obj -> Request -> string -> Async<Response>) =
    async {
        match! collaborateur env req with
        | Error e -> return erreur env req 403 e
        | Ok login -> return! action env req login
    }

// ---------- L'atelier des données (docs/atelier.md) ----------

[<Emit("(() => { const o = new Uint8Array(32); crypto.getRandomValues(o); return btoa(Array.from(o, b => String.fromCharCode(b)).join('')).replace(/\\+/g, '-').replace(/\\//g, '_').replace(/=+$/, ''); })()")>]
let aleatoire256 () : string = jsNative

/// L'empreinte d'un code d'invitation : le KV ne garde jamais le code lui-même.
let empreinteCode (code: string) = sha256 ("atelier:" + code)

/// Une invitation : l'identifiant du collaborateur (sa provenance dans les CSV) et son nom.
type Invite = { id: string; nom: string }

/// Au plus 20 essais de code ratés par heure et par adresse IP (empreinte seulement).
let echecsDepasses (env: obj) (req: Request) (compter: bool) =
    async {
        let ip = req.headers.get "CF-Connecting-IP" |> Option.defaultValue "inconnue"
        let! e = sha256 ("atelier-echec:" + ip)
        let cle = $"atelier:echec:{e}:{System.DateTimeOffset.UtcNow.ToUnixTimeSeconds() / 3600L}"
        let! t = attendre ((kv env).get cle)
        let n = t |> Option.map int |> Option.defaultValue 0

        if compter then
            do! attendre ((kv env).put (cle, string (n + 1), createObj [ "expirationTtl" ==> 3600 ]))

        return n >= 20
    }

/// Le collaborateur derrière le code d'invitation (`Authorization: Bearer atl_…`).
let invite (env: obj) (req: Request) =
    async {
        let! bloque = echecsDepasses env req false

        if bloque then
            return Error(429, "trop d'essais ratés : réessaie dans une heure")
        elif (origineAdmise env req).IsNone then
            return Error(403, "origine inconnue")
        else
            match req.headers.get "Authorization" with
            | Some a when a.StartsWith "Bearer " && Atelier.formeCode (a.Substring 7) ->
                let! h = empreinteCode (a.Substring 7)
                let! t = attendre ((kv env).get ("atelier:invite:" + h))

                match t with
                | Some t ->
                    let o = JS.JSON.parse t
                    return Ok { id = texteDe o "id"; nom = texteDe o "nom" }
                | None ->
                    let! _ = echecsDepasses env req true
                    return Error(403, "code d'invitation inconnu ou retiré")
            | _ ->
                let! _ = echecsDepasses env req true
                return Error(403, "il faut un code d'invitation")
    }

/// Une route de l'atelier : réservée aux invités.
let pourInvite (env: obj) (req: Request) (action: Invite -> Async<Response>) =
    async {
        match! invite env req with
        | Error(statut, e) -> return erreur env req statut e
        | Ok i -> return! action i
    }

/// Au plus 60 modifications par heure et par invité.
let modificationsDepassees (env: obj) (i: Invite) =
    async {
        let cle = $"atelier:debit:{i.id}:{System.DateTimeOffset.UtcNow.ToUnixTimeSeconds() / 3600L}"
        let! t = attendre ((kv env).get cle)
        let n = t |> Option.map int |> Option.defaultValue 0

        if n >= 60 then
            return true
        else
            do! attendre ((kv env).put (cle, string (n + 1), createObj [ "expirationTtl" ==> 3600 ]))
            return false
    }

let depotAtelier (env: obj) = variable env "DEPOT_ATELIER"

let jetonAtelier (env: obj) =
    match variable env "ATELIER_APP_ID", variable env "ATELIER_INSTALLATION", variable env "ATELIER_CLE" with
    | "", _, _
    | _, "", _
    | _, _, "" -> async { return Error "l'atelier n'est pas encore branché sur GitHub" }
    | app, inst, cle -> GitHub.jetonAtelier app inst cle (variable env "DEPOT_ATELIER")

/// Le texte d'une table dans le dépôt de l'atelier, et son empreinte git (pour réécrire sans écraser).
let lireTable (env: obj) (jeton: string) (chemin: string) =
    async {
        let! (s, r) = GitHub.appeler jeton "GET" $"/repos/{depotAtelier env}/contents/{chemin}?ref=main" None
        return (if s = 200 then Ok(GitHub.depuisContenu (string r?content), string r?sha) else Error $"{chemin} illisible dans l'atelier ({s})")
    }

/// Qui est l'invité (la page le montre).
let moi (env: obj) (req: Request) =
    pourInvite env req (fun i -> async { return repondre env req 200 (createObj [ "id" ==> i.id; "nom" ==> i.nom ]) })

/// Les tables de l'atelier, telles qu'elles sont dans le dépôt de l'atelier.
let tablesAtelier (env: obj) (req: Request) =
    pourInvite env req (fun _ ->
        async {
            match! jetonAtelier env with
            | Error e -> return erreur env req 503 e
            | Ok jeton ->
                let resultats = ResizeArray<obj>()
                let mutable probleme = None

                for t in Atelier.tables do
                    match! lireTable env jeton t.chemin with
                    | Ok(texte, _) -> resultats.Add(createObj [ "chemin" ==> t.chemin; "contenu" ==> texte ])
                    | Error e -> probleme <- Some e

                match probleme with
                | Some e -> return erreur env req 502 e
                | None -> return repondre env req 200 (resultats.ToArray())
        })

let paires (o: obj) : (string * string) list =
    match o with
    | null -> []
    | o when jsTypeof o = "object" ->
        [ for k in JS.Constructors.Object.keys o do
              match o?(k): obj with
              | v when jsTypeof v = "string" -> yield k, string v
              | _ -> () ]
    | _ -> []

/// Une modification d'un invité : { chemin, genre: modifier|ajouter, cle, champs, avant, raison }.
/// Le relais relit la table dans l'atelier, applique, et écrit un commit signé du nom de l'invité.
let modifier (env: obj) (req: Request) =
    pourInvite env req (fun i ->
        async {
            let! o = lireJson req
            let! trop = modificationsDepassees env i

            let modification =
                match texteDe o "genre" with
                | "modifier" -> Ok(Atelier.Modifier(texteDe o "cle", paires o?champs, paires o?avant))
                | "ajouter" -> Ok(Atelier.Ajouter(paires o?champs))
                | _ -> Error "genre : modifier ou ajouter"

            match trop, Atelier.tableDe (texteDe o "chemin"), modification, Atelier.raisonPropre (texteDe o "raison") with
            | true, _, _, _ -> return erreur env req 429 "beaucoup de modifications en une heure : reprends plus tard"
            | _, None, _, _ -> return erreur env req 400 "cette table n'est pas ouverte à l'atelier"
            | _, _, Error e, _
            | _, _, _, Error e -> return erreur env req 400 e
            | false, Some table, Ok m, Ok raison ->
                match! jetonAtelier env with
                | Error e -> return erreur env req 503 e
                | Ok jeton ->
                    // deux essais : si quelqu'un écrit la même table au même moment, on relit
                    let rec essayer n =
                        async {
                            match! lireTable env jeton table.chemin with
                            | Error e -> return erreur env req 502 e
                            | Ok(texte, sha) ->
                                match Atelier.appliquer table texte i.id m with
                                | Error e -> return erreur env req 409 e
                                | Ok(nouveau, resume) ->
                                    let message = $"Atelier : {resume} ({i.nom})\n\n{raison}\n\nProposé par {i.nom} ({i.id}) depuis l'atelier."

                                    let corps =
                                        createObj
                                            [ "message" ==> message
                                              "content" ==> GitHub.enBase64 nouveau
                                              "sha" ==> sha
                                              "branch" ==> "main" ]

                                    let! (s, r) = GitHub.appeler jeton "PUT" $"/repos/{depotAtelier env}/contents/{table.chemin}" (Some corps)

                                    match s with
                                    | 200 | 201 -> return repondre env req 200 (createObj [ "commit" ==> string r?commit?html_url; "resume" ==> resume ])
                                    | 409 when n > 0 -> return! essayer (n - 1)
                                    | _ -> return erreur env req 502 $"GitHub refuse l'écriture ({s})"
                        }

                    return! essayer 1
        })

/// Crée une invitation (collaborateur du dépôt) : { id, nom }. Le code n'est rendu qu'une fois.
let inviter (env: obj) (req: Request) (login: string) =
    async {
        let! o = lireJson req
        let id = texteDe o "id"
        let nom = (texteDe o "nom").Trim()

        if not (Atelier.reIdentifiant.IsMatch id) || id = "auteur" || id = "assistant" then
            return erreur env req 400 "identifiant : minuscules, chiffres et tirets (ni auteur ni assistant)"
        elif nom = "" || nom.Length > 80 || nom |> Seq.exists System.Char.IsControl then
            return erreur env req 400 "un nom, sur une ligne (80 caractères au plus)"
        else
            let! existantes = attendre ((kv env).list (createObj [ "prefix" ==> "atelier:invite:" ]))

            if unbox<obj[]> existantes?keys |> Array.exists (fun k -> k?metadata <> null && string k?metadata?id = id) then
                return erreur env req 409 $"« {id} » a déjà une invitation : retire-la d'abord"
            else
                let code = Atelier.prefixeCode + aleatoire256 ()
                let! h = empreinteCode code
                let valeur = JS.JSON.stringify (createObj [ "id" ==> id; "nom" ==> nom; "par" ==> login; "cree" ==> System.DateTime.UtcNow.ToString("yyyy-MM-dd") ])
                do! attendre ((kv env).put ("atelier:invite:" + h, valeur, createObj [ "metadata" ==> createObj [ "id" ==> id; "nom" ==> nom ] ]))
                return repondre env req 201 (createObj [ "id" ==> id; "nom" ==> nom; "code" ==> code ])
    }

let invitations (env: obj) (req: Request) (_: string) =
    async {
        let! page = attendre ((kv env).list (createObj [ "prefix" ==> "atelier:invite:" ]))

        let liste =
            unbox<obj[]> page?keys
            |> Array.choose (fun k -> match k?metadata with null -> None | m -> Some(createObj [ "id" ==> m?id; "nom" ==> m?nom ]))

        return repondre env req 200 liste
    }

let retirer (env: obj) (req: Request) (id: string) =
    async {
        let! page = attendre ((kv env).list (createObj [ "prefix" ==> "atelier:invite:" ]))
        let cles = unbox<obj[]> page?keys |> Array.filter (fun k -> k?metadata <> null && string k?metadata?id = id)

        for k in cles do
            do! attendre ((kv env).delete (string k?name))

        return (if cles.Length = 0 then erreur env req 404 $"aucune invitation pour « {id} »" else repondre env req 200 (createObj [ "retire" ==> id ]))
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
            | "POST", [ "prenoms" ] -> return! declarer env req
            | "GET", [ "moderation"; "idees" ] -> return! reserve env req idees
            | "POST", [ "moderation"; "idees"; code; "publier" ] -> return! reserve env req (fun e r l -> publier e r l code)
            | "POST", [ "moderation"; "idees"; code; "ecarter" ] -> return! reserve env req (fun e r l -> ecarter e r l code)
            | "POST", [ "moderation"; "idees"; code; "statut" ] -> return! reserve env req (fun e r l -> changerStatut e r l code)
            | "GET", [ "moderation"; "prenoms" ] -> return! reserve env req comptesPrenoms
            | "GET", [ "atelier"; "moi" ] -> return! moi env req
            | "GET", [ "atelier"; "tables" ] -> return! tablesAtelier env req
            | "POST", [ "atelier"; "modifications" ] -> return! modifier env req
            | "GET", [ "moderation"; "atelier"; "invitations" ] -> return! reserve env req invitations
            | "POST", [ "moderation"; "atelier"; "invitations" ] -> return! reserve env req inviter
            | "POST", [ "moderation"; "atelier"; "invitations"; id; "retirer" ] -> return! reserve env req (fun e r _ -> retirer e r id)
            | _ -> return erreur env req 404 "rien ici"
        with e ->
            JS.console.error e.Message
            return erreur env req 500 "le relais a un problème : réessaie plus tard"
    }

exportDefault (createObj [ "fetch" ==> System.Func<Request, obj, obj, JS.Promise<Response>>(fun req env _ -> Async.StartAsPromise(traiter req env)) ])
