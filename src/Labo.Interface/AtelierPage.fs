/// La page de l'atelier des données (jeu/atelier.html) : un collaborateur invité entre son code,
/// voit les tables ouvertes, modifie une ligne ou en ajoute une, en disant pourquoi. Le relais
/// écrit chaque modification comme un commit dans le dépôt de l'atelier (docs/atelier.md).
/// Le code d'invitation reste dans l'onglet (sessionStorage), jamais dans l'adresse.
module Labo.Interface.AtelierPage

open Browser
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
      edition: Edition
      occupe: bool
      note: (string * string) option }

let mutable etat =
    { code = (try Option.ofObj (window.sessionStorage.getItem cleCode) with _ -> None)
      nom = ""
      tables = []
      edition = Aucune
      occupe = false
      note = None }

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
    | "description_fr" -> "description"
    | c -> c

let idChamp col = "champ-" + col

// ---------- L'écran ----------

let rec afficher () =
    monter (unbox (document.getElementById "atelier")) (page ())

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
          (match etat.code with
           | None -> entrer ()
           | Some _ when etat.tables.IsEmpty -> para "discret" [ texte (if etat.occupe then "Chargement…" else "") ]
           | Some _ -> atelier ())
          (match etat.note with
           | Some(cls, t) -> div ("message " + cls) [ texte t ]
           | None -> Rien) ]

and entrer () =
    el
        "section"
        [ "class", "carte" ]
        [ h2 "" [ texte "Entrer" ]
          champ "code" "Ton code d'invitation" "Il commence par atl_. Garde-le pour toi : il signe tes changements." (entree "code" "password" "" [ "autocomplete", "off"; "spellcheck", "false" ])
          div "actions" [ boutonAvec [ "class", "principal"; (if etat.occupe then "disabled" else "data-libre"), "" ] connecter [ texte "Entrer" ] ] ]

and atelier () =
    div
        ""
        [ div
              "carte"
              [ para "" [ texte $"Bonjour {etat.nom}." ]
                para "aide" [ lienExterne historique "L'historique de l'atelier : qui a changé quoi, et pourquoi"; texte " (public)." ]
                div "actions" [ bouton "secondaire" quitter [ texte "Quitter" ] ] ]
          yield! etat.tables |> List.map vueTable ]

and vueTable (t: Table, lignes: Ligne list) =
    let ic = t.entete |> List.findIndex ((=) t.cle)

    el
        "section"
        [ "class", "carte" ]
        [ h2 "" [ texte t.titre ]
          para "aide" [ texte t.aide ]
          el "ul" [ "class", "suivis" ] [ for l in lignes -> el "li" [] [ vueLigne t l (l.champs[ic]) ] ]
          (match etat.edition with
           | Nouvelle c when c = t.chemin -> formulaire t None
           | _ -> div "actions" [ bouton "secondaire" (fun () -> changer (fun e -> { e with edition = Nouvelle t.chemin; note = None })) [ texte "Ajouter une ligne" ] ]) ]

and vueLigne (t: Table) (l: Ligne) (cle: string) =
    match etat.edition with
    | Ligne(c, k) when c = t.chemin && k = cle -> formulaire t (Some l)
    | _ ->
        div
            ""
            [ div "suivi-tete" [ el "strong" [] [ texte cle ]; span "puce" [ texte ("par " + l.champs[t.entete |> List.findIndex ((=) t.provenance)]) ] ]
              yield!
                  List.zip t.entete l.champs
                  |> List.filter (fun (col, v) -> col <> t.cle && col <> t.provenance && v <> "")
                  |> List.map (fun (col, v) -> para "" [ span "discret" [ texte (libelleColonne col + " : ") ]; texte v ])
              bouton "lien-discret" (fun () -> changer (fun e -> { e with edition = Ligne(t.chemin, cle); note = None })) [ texte "Modifier" ] ]

/// Le formulaire d'une ligne : existante (ses colonnes modifiables) ou nouvelle (avec la clé).
and formulaire (t: Table) (ligne: Ligne option) =
    let valeurDe col =
        match ligne with
        | Some l -> l.champs[t.entete |> List.findIndex ((=) col)]
        | None -> ""

    let colonnes = (if ligne.IsNone then [ t.cle ] else []) @ t.modifiables

    div
        "champ"
        [ yield!
              colonnes
              |> List.map (fun col ->
                  let v = valeurDe col
                  let saisie = if v.Length > 80 then zone (idChamp col) v 3 else entree (idChamp col) "text" v [ "maxlength", string longueurMaximum ]
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
                let tables =
                    unbox<obj[]> liste
                    |> Array.choose (fun o ->
                        match tableDe (string o?chemin), lire (string o?contenu) with
                        | Some t, Ok(_ :: lignes) -> Some(t, lignes)
                        | _ -> None)
                    |> List.ofArray

                changer (fun e -> { e with nom = string j?nom; tables = tables; occupe = false })
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
    changer (fun _ -> { code = None; nom = ""; tables = []; edition = Aucune; occupe = false; note = None })

and envoyer (t: Table) (ligne: Ligne option) (colonnes: string list) =
    let champs = colonnes |> List.map (fun col -> col, valeur (idChamp col))
    let raison = valeur "raison"

    let corps =
        match ligne with
        | Some l ->
            let avant = colonnes |> List.map (fun col -> col, l.champs[t.entete |> List.findIndex ((=) col)])
            let cle = l.champs[t.entete |> List.findIndex ((=) t.cle)]
            let modifies = champs |> List.filter (fun (c, v) -> List.tryFind (fun (c2, _) -> c2 = c) avant |> Option.map snd <> Some v)

            createObj
                [ "chemin" ==> t.chemin
                  "genre" ==> "modifier"
                  "cle" ==> cle
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
