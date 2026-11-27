/// La page de l'atelier des données (jeu/atelier.html) : un collaborateur invité entre son code,
/// choisit une table, cherche une ligne, la modifie (ou en ajoute une, là où c'est permis), en
/// disant pourquoi. Le relais écrit chaque modification comme un commit dans le dépôt de l'atelier
/// (docs/atelier.md). Le code d'invitation reste dans l'onglet (sessionStorage), jamais dans
/// l'adresse.
module Labo.Interface.AtelierPage

open Browser
open Browser.Types
open Fable.Core
open Fable.Core.JsInterop
open Labo.Interface.Html
open Labo.Interface.Formulaire
open Labo.Relais.Atelier

let historique = "https://github.com/labo-de-la-bande/atelier/commits/main"
let cleCode = "labo-atelier-code"

type Edition =
    | Aucune
    | Ligne of chemin: string * cle: string
    | Nouvelle of chemin: string

type Etat =
    { code: string option
      nom: string
      tables: (Table * Ligne list) list
      /// la table ouverte (son chemin)
      choisie: string option
      /// les groupes dépliés : chemin de la table, valeur du groupe
      ouverts: Set<string * string>
      recherche: string
      edition: Edition
      occupe: bool
      note: (string * string) option }

let vide =
    { code = None
      nom = ""
      tables = []
      choisie = None
      ouverts = Set.empty
      recherche = ""
      edition = Aucune
      occupe = false
      note = None }

let mutable etat =
    { vide with code = (try Option.ofObj (window.sessionStorage.getItem cleCode) with _ -> None) }

[<Emit("fetch($0, $1)")>]
let private fetch (url: string) (init: obj) : JS.Promise<obj> = jsNative

/// Un appel au relais avec le code d'invitation : le code HTTP et la réponse JSON.
let appeler (methode: string) (chemin: string) (corps: obj option) =
    async {
        let entetes =
            createObj
                [ "Authorization" ==> ("Bearer " + defaultArg etat.code "")
                  if corps.IsSome then "Content-Type" ==> "application/json" ]

        let init =
            createObj
                [ "method" ==> methode
                  "headers" ==> entetes
                  if corps.IsSome then "body" ==> JS.JSON.stringify corps.Value ]

        let! r = Async.AwaitPromise(fetch (Relais.adresse + chemin) init)
        let! json = Async.AwaitPromise(r?json () : JS.Promise<obj>)
        return (unbox<int> r?status), json
    }

let libelleColonne (col: string) =
    match col with
    | "libelle_fr" -> "libellé"
    | "raison_fr" -> "raison"
    | "titre_fr" -> "titre"
    | "sous_titre_fr" -> "sous-titre"
    | "description_fr" -> "description"
    | "texte_fr" -> "texte"
    | "precision_fr" -> "précision"
    | "portrait_fr" -> "portrait"
    | "role_fr" -> "rôle"
    | "acces_fr" -> "accès"
    | c -> c

let idChamp col = "champ-" + col
let valeurDe (t: Table) (l: Ligne) col = l.champs[t.entete |> List.findIndex ((=) col)]

/// Une saisie sur une ligne : les retours à la ligne d'une zone de texte deviennent des espaces.
let saisie id = (valeur id).Replace("\r", "").Replace("\n", " ")

// ---------- La recherche (sans redessiner : le champ garde la main) ----------

let normaliser (s: string) = s.ToLowerInvariant()

/// Cache les lignes qui ne contiennent pas le texte cherché ; ouvre les groupes qui en ont.
let filtrer () =
    let q = normaliser (etat.recherche.Trim())
    let lignes = document.querySelectorAll "#lignes li[data-cherche]"

    for i in 0 .. lignes.length - 1 do
        let li = lignes.item i :?> HTMLElement
        li.hidden <- q <> "" && not ((li.getAttribute "data-cherche").Contains q)

    let groupes = document.querySelectorAll "#lignes details"

    for i in 0 .. groupes.length - 1 do
        let d = groupes.item i :?> HTMLElement
        let visibles = d.querySelectorAll "li[data-cherche]:not([hidden])"
        d.hidden <- q <> "" && visibles.length = 0
        if q <> "" && visibles.length > 0 then d?``open`` <- true

    match document.getElementById "compte" with
    | null -> ()
    | c ->
        let n = (document.querySelectorAll "#lignes li[data-cherche]:not([hidden])").length
        c.textContent <- if q = "" then "" else $"{n} ligne(s)"

// ---------- L'écran ----------

let rec afficher () =
    monter (unbox (document.getElementById "atelier")) (page ())
    filtrer ()

and changer f =
    etat <- f etat
    afficher ()

and page () =
    div
        "page"
        [ div
              "tete"
              [ h1 [ texte "L'atelier des données" ]
                para "accroche" [ texte "Les tables du jeu, ouvertes aux personnes invitées. Chaque changement va dans une salle d'attente publique, avec ton nom et ta raison ; l'auteur relit avant que rien n'entre dans le jeu." ] ]
          (match etat.note with
           | Some(cls, t) -> el "div" [ "class", "message " + cls; "role", "status" ] [ texte t ]
           | None -> Rien)
          (match etat.code with
           | None -> entrer ()
           | Some _ when etat.tables.IsEmpty -> para "discret" [ texte (if etat.occupe then "Chargement…" else "") ]
           | Some _ -> atelier ()) ]

and entrer () =
    el
        "section"
        [ "class", "carte" ]
        [ h2 "" [ texte "Entrer" ]
          champ "code" "Ton code d'invitation" "Il commence par atl_. Garde-le pour toi : il signe tes changements." (entree "code" "password" "" [ "autocomplete", "off"; "spellcheck", "false" ])
          div "actions" [ boutonAvec [ "class", "principal"; (if etat.occupe then "disabled" else "data-libre"), "" ] connecter [ texte "Entrer" ] ] ]

and atelier () =
    div
        "atelier"
        [ div
              "carte"
              [ div
                    "suivi-tete"
                    [ para "" [ texte $"Bonjour {etat.nom}." ]
                      bouton "lien-discret" quitter [ texte "Quitter" ] ]
                para "aide" [ lienExterne historique "L'historique de l'atelier : qui a changé quoi, et pourquoi"; texte " (public)." ] ]
          sommaire ()
          (match etat.choisie |> Option.bind (fun c -> etat.tables |> List.tryFind (fun (t, _) -> t.chemin = c)) with
           | Some tl -> vueTable tl
           | None -> para "discret" [ texte "Choisis une table." ]) ]

/// Les tables, en deux familles : les enjeux de la partie, puis les textes.
and sommaire () =
    let onglet (t: Table, lignes: Ligne list) =
        let actif = etat.choisie = Some t.chemin

        boutonAvec
            [ "class", (if actif then "onglet actif" else "onglet")
              if actif then "aria-current", "true" ]
            (fun () -> changer (fun e -> { e with choisie = Some t.chemin; edition = Aucune; recherche = ""; note = None }))
            [ texte t.titre; span "nombre" [ texte (string lignes.Length) ] ]

    let famille titre tables =
        if List.isEmpty tables then Rien
        else div "famille" [ para "discret" [ texte titre ]; div "onglets" (tables |> List.map onglet) ]

    el
        "nav"
        [ "class", "carte sommaire"; "aria-label", "Les tables" ]
        [ famille "Le jeu : les défis, et ce que la société garde" (etat.tables |> List.filter (fun (t, _) -> t.ajout))
          famille "Les textes" (etat.tables |> List.filter (fun (t, _) -> not t.ajout)) ]

and vueTable (t: Table, lignes: Ligne list) =
    let item (l: Ligne) =
        el "li" [ "data-cherche", normaliser (String.concat " " l.champs) ] [ vueLigne t l (cleDe t l.champs) ]

    let corps =
        match t.groupe with
        | None -> el "ul" [ "class", "suivis" ] (lignes |> List.map item)
        | Some g ->
            let groupes = lignes |> List.groupBy (fun l -> valeurDe t l g)

            let groupe (v: string, ls: Ligne list) =
                let edite =
                    match etat.edition with
                    | Ligne(c, k) when c = t.chemin -> ls |> List.exists (fun l -> cleDe t l.champs = k)
                    | _ -> false

                let ouvert = edite || etat.ouverts.Contains(t.chemin, v)

                // se souvenir des groupes dépliés, sans redessiner
                let basculer (ev: Event) =
                    let o: bool = ev.target?``open``
                    etat <- { etat with ouverts = (if o then etat.ouverts.Add(t.chemin, v) else etat.ouverts.Remove(t.chemin, v)) }

                Element(
                    "details",
                    [ "class", "groupe"; if ouvert then "open", "" ],
                    [ "toggle", basculer ],
                    [ el "summary" [] [ texte (if v = "" then "(sans " + g + ")" else v); span "nombre" [ texte (string ls.Length) ] ]
                      el "ul" [ "class", "suivis" ] (ls |> List.map item) ]
                )

            div "groupes" (groupes |> List.map groupe)

    el
        "section"
        [ "class", "carte" ]
        [ h2 "" [ texte t.titre ]
          para "aide" [ texte t.aide ]
          div
              "recherche"
              [ Element(
                    "input",
                    [ "id", "recherche"; "type", "search"; "value", etat.recherche; "placeholder", "Chercher un mot, un habitant, une situation…"; "aria-label", "Chercher dans la table" ],
                    [ "input",
                      (fun ev ->
                          etat <- { etat with recherche = string ev.target?value }
                          filtrer ()) ],
                    []
                )
                span "discret" [ el "span" [ "id", "compte"; "aria-live", "polite" ] [] ] ]
          div "" [ el "div" [ "id", "lignes" ] [ corps ] ]
          (if not t.ajout then Rien
           else
               match etat.edition with
               | Nouvelle c when c = t.chemin -> formulaire t None
               | _ -> div "actions" [ bouton "secondaire" (fun () -> changer (fun e -> { e with edition = Nouvelle t.chemin; note = None })) [ texte "Ajouter une ligne" ] ]) ]

and vueLigne (t: Table) (l: Ligne) (cle: string) =
    match etat.edition with
    | Ligne(c, k) when c = t.chemin && k = cle -> formulaire t (Some l)
    | _ ->
        div
            ""
            [ div
                  "suivi-tete"
                  [ el "strong" [] [ texte cle ]
                    yield!
                        t.contexte
                        |> List.choose (fun col -> match valeurDe t l col with "" -> None | v -> Some(span "puce discrete" [ texte (libelleColonne col + " : " + v) ]))
                    span "puce" [ texte ("par " + valeurDe t l t.provenance) ] ]
              yield!
                  t.modifiables
                  |> List.choose (fun col ->
                      match valeurDe t l col with
                      | "" -> None
                      | v -> Some(para "" [ span "discret" [ texte (libelleColonne col + " : ") ]; texte v ]))
              bouton "lien-discret" (fun () -> changer (fun e -> { e with edition = Ligne(t.chemin, cle); note = None })) [ texte "Modifier" ] ]

/// Le formulaire d'une ligne : existante (ses colonnes modifiables) ou nouvelle (avec la clé).
and formulaire (t: Table) (ligne: Ligne option) =
    let valeurDe col =
        match ligne with
        | Some l -> valeurDe t l col
        | None -> ""

    let colonnes = (if ligne.IsNone then [ List.head t.cle ] else []) @ t.modifiables

    div
        "champ edition"
        [ yield!
              colonnes
              |> List.map (fun col ->
                  let v = valeurDe col
                  let long = v.Length > 80 || col = "texte_fr" || col = "description_fr" || col = "raison_fr"
                  let saisie = if long then zone (idChamp col) v (max 3 (v.Length / 70 + 1)) else entree (idChamp col) "text" v [ "maxlength", string longueurMaximum ]
                  champ (idChamp col) (libelleColonne col) "" saisie)
          champ "raison" "Pourquoi ce changement ?" "Une phrase : elle accompagne ton changement dans l'historique." (entree "raison" "text" "" [ "maxlength", string raisonMaximum ])
          div
              "actions"
              [ bouton "secondaire" (fun () -> changer (fun e -> { e with edition = Aucune })) [ texte "Annuler" ]
                boutonAvec [ "class", "principal"; (if etat.occupe then "disabled" else "data-libre"), "" ] (fun () -> envoyer t ligne colonnes) [ texte "Enregistrer" ] ] ]

// ---------- Les actions ----------

and charger () =
    async {
        changer (fun e -> { e with occupe = true })
        let! qui = appeler "GET" "/atelier/moi" None |> Async.Catch

        match qui with
        | Choice1Of2(200, j) ->
            let! r = appeler "GET" "/atelier/tables" None |> Async.Catch

            match r with
            | Choice1Of2(200, liste) ->
                let recues =
                    unbox<obj[]> liste
                    |> Array.choose (fun o ->
                        match tableDe (string o?chemin), lire (string o?contenu) with
                        | Some t, Ok(_ :: lignes) -> Some(t, lignes)
                        | _ -> None)
                    |> List.ofArray

                // l'ordre de Atelier.tables, pas celui de la réponse
                let tables = tables |> List.choose (fun t -> recues |> List.tryFind (fun (r, _) -> r.chemin = t.chemin))

                let choisie =
                    match etat.choisie with
                    | Some c when tables |> List.exists (fun (t, _) -> t.chemin = c) -> Some c
                    | _ -> tables |> List.tryHead |> Option.map (fun (t, _) -> t.chemin)

                changer (fun e -> { e with nom = string j?nom; tables = tables; choisie = choisie; occupe = false })
            | Choice1Of2(_, j) -> changer (fun e -> { e with occupe = false; note = Some("erreur", string j?erreur) })
            | Choice2Of2 _ -> changer (fun e -> { e with occupe = false; note = Some("erreur", "Le relais ne répond pas pour l'instant.") })
        | Choice1Of2(_, j) ->
            try window.sessionStorage.removeItem cleCode with _ -> ()
            changer (fun e -> { e with code = None; occupe = false; note = Some("erreur", string j?erreur) })
        | Choice2Of2 _ -> changer (fun e -> { e with occupe = false; note = Some("erreur", "Le relais ne répond pas pour l'instant.") })
    }
    |> Async.StartImmediate

and connecter () =
    let code = (valeur "code").Trim()

    if not (formeCode code) then
        changer (fun e -> { e with note = Some("erreur", "Ce n'est pas un code d'invitation : il commence par atl_ et compte 47 caractères.") })
    else
        try window.sessionStorage.setItem (cleCode, code) with _ -> ()
        etat <- { etat with code = Some code; note = None }
        charger ()

and quitter () =
    try window.sessionStorage.removeItem cleCode with _ -> ()
    changer (fun _ -> vide)

and envoyer (t: Table) (ligne: Ligne option) (colonnes: string list) =
    let champs = colonnes |> List.map (fun col -> col, saisie (idChamp col))
    let raison = valeur "raison"

    let corps =
        match ligne with
        | Some l ->
            let avant = colonnes |> List.map (fun col -> col, valeurDe t l col)
            let modifies = champs |> List.filter (fun (c, v) -> List.tryFind (fun (c2, _) -> c2 = c) avant |> Option.map snd <> Some v)

            createObj
                [ "chemin" ==> t.chemin
                  "genre" ==> "modifier"
                  "cle" ==> cleDe t l.champs
                  "champs" ==> createObj [ for (c, v) in modifies -> c ==> v ]
                  "avant" ==> createObj [ for (c, v) in avant do if List.exists (fun (c2, _) -> c2 = c) modifies then yield c ==> v ]
                  "raison" ==> raison ]
        | None ->
            createObj
                [ "chemin" ==> t.chemin
                  "genre" ==> "ajouter"
                  "champs" ==> createObj [ for (c, v) in champs do if v.Trim() <> "" then yield c ==> v ]
                  "raison" ==> raison ]

    async {
        changer (fun e -> { e with occupe = true })
        let! r = appeler "POST" "/atelier/modifications" (Some corps) |> Async.Catch

        match r with
        | Choice1Of2(200, j) ->
            etat <- { etat with edition = Aucune; occupe = false; note = Some("bon", $"Enregistré dans l'atelier ({string j?resume}). Merci.") }
            charger ()
        | Choice1Of2(_, j) -> changer (fun e -> { e with occupe = false; note = Some("erreur", string j?erreur) })
        | Choice2Of2 _ -> changer (fun e -> { e with occupe = false; note = Some("erreur", "Le relais ne répond pas pour l'instant.") })
    }
    |> Async.StartImmediate

let demarrer () =
    afficher ()
    if etat.code.IsSome then charger ()

demarrer ()
