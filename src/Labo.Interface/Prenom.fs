/// La page « Mon prénom » (jeu/prenom.html) : un joueur donne son prénom, sa décennie et son pays
/// de naissance, et l'usage du prénom autour de lui, pour remplir les banques de prénoms par pays.
/// On ne demande pas son genre à lui : seulement à qui ce prénom se donne. Le relais n'en garde
/// que des comptes agrégés ; un prénom n'entre dans une banque qu'au-delà d'un seuil de
/// déclarations, et après validation (docs/relais.md).
module Labo.Interface.Prenom

open Browser
open Fable.Core
open Fable.Core.JsInterop
open Labo.Interface.Html
open Labo.Interface.Relais
open Labo.Interface.Formulaire
open Labo.Relais.Proposition

/// Le nom d'un pays dans la langue de la page, d'après les données CLDR du navigateur.
[<Emit("(function (c) { try { return new Intl.DisplayNames(['fr'], { type: 'region' }).of(c) || c } catch (e) { return c } })($0)")>]
let nomPays (codeMajuscule: string) : string = jsNative

let choix (nom: string) (valeur: string) (libelle: string) =
    el "label" [ "class", "choix" ] [ el "input" [ "type", "radio"; "name", nom; "value", valeur; "id", nom + "-" + valeur ] []; texte (" " + libelle) ]

let coche (nom: string) (valeurs: string list) =
    valeurs
    |> List.tryFind (fun v ->
        match document.getElementById (nom + "-" + v) with
        | null -> false
        | e -> unbox<bool> e?``checked``)
    |> Option.defaultValue ""

let envoyer () =
    let accord = match document.getElementById "accord" with null -> false | e -> unbox<bool> e?``checked``

    let d =
        { prenom = valeur "prenom"
          decennie = (match System.Int32.TryParse(valeur "decennie") with | true, d -> d | _ -> 0)
          pays = valeur "pays"
          usage = coche "usage" (usages |> List.map fst) }

    match validerDeclaration d with
    | Error refus -> message "erreur" [ texte ("À corriger : " + String.concat " ; " refus + ".") ]
    | Ok _ when not accord -> message "erreur" [ texte "Coche la case d'accord pour que ton prénom puisse servir aux banques." ]
    | Ok _ when jetonTurnstile = "" -> message "erreur" [ texte "La vérification anti-spam n'est pas encore passée : attends la coche, puis réessaie." ]
    | Ok d ->
        message "" [ texte "Envoi…" ]

        async {
            let corps =
                createObj
                    [ "prenom" ==> d.prenom
                      "decennie" ==> string d.decennie
                      "pays" ==> d.pays
                      "usage" ==> d.usage
                      "accord" ==> "oui"
                      "site" ==> valeur "site"
                      "jeton" ==> jetonTurnstile ]

            let! r = appeler "POST" "/prenoms" (Some corps) |> Async.Catch
            jetonTurnstile <- ""
            reinitialiserTurnstile ()

            match r with
            | Choice1Of2(201, _) ->
                message
                    "bon"
                    [ para "" [ el "strong" [] [ texte $"Merci ! « {d.prenom} » est compté." ] ]
                      para "aide" [ texte "Il entrera dans la banque de ton pays quand assez de personnes l'auront déclaré pour la même décennie, et après une relecture. Ton prénom n'est rattaché à rien d'autre : ni adresse, ni compte." ] ]
            | Choice1Of2(_, j) -> message "erreur" [ texte (string j?erreur) ]
            | Choice2Of2 _ -> message "erreur" [ texte "Le relais ne répond pas. Réessaie plus tard." ]
        }
        |> Async.StartImmediate

let page () =
    let pays =
        Labo.Data.Prenoms.paysDuMonde
        |> List.map (fun (code, _) -> code, nomPays (code.ToUpperInvariant()))
        |> List.sortBy (fun (_, n) -> n.Normalize(System.Text.NormalizationForm.FormD))

    div
        "page"
        [ el
              "header"
              [ "class", "tete" ]
              [ el "a" [ "href", "./"; "class", "lien-discret" ] [ texte "← Retour au jeu" ]
                h1 [ texte "Mon prénom" ]
                para
                    "accroche"
                    [ texte "Les habitants du jeu reçoivent un prénom de leur génération et de leur pays de naissance. Pour beaucoup de pays, aucune statistique publique ne dit quels prénoms on y donnait, décennie par décennie : ton prénom aide à le savoir." ] ]
          (if not ouvert then
               para "carte discret" [ texte "L'envoi ouvre bientôt." ]
           else
               el
                   "section"
                   [ "class", "carte" ]
                   [ champ "prenom" "Ton prénom" "Celui que tu portes, comme tu l'écris." (entree "prenom" "text" "" [ "maxlength", "40"; "autocomplete", "given-name" ])
                     champ
                         "decennie"
                         "Ta décennie de naissance"
                         "Seulement la décennie, pas l'année."
                         (el "select" [ "id", "decennie" ] [ el "option" [ "value", "" ] [ texte "—" ]; yield! decennies |> List.map (fun d -> el "option" [ "value", string d ] [ texte $"années {d}" ]) ])
                     champ
                         "pays"
                         "Ton pays de naissance"
                         ""
                         (el "select" [ "id", "pays" ] [ el "option" [ "value", "" ] [ texte "—" ]; yield! pays |> List.map (fun (c, n) -> el "option" [ "value", c ] [ texte n ]) ])
                     el
                         "fieldset"
                         [ "class", "champ" ]
                         [ el "legend" [] [ texte "Autour de toi, ce prénom se donne plutôt…" ]
                           para "aide" [ texte "On ne te demande pas ton genre : seulement à qui ce prénom est donné là où tu as grandi. Ça dit à quels personnages il peut aller." ]
                           yield! usages |> List.map (fun (code, libelle) -> choix "usage" code libelle) ]
                     el
                         "label"
                         [ "class", "choix" ]
                         [ el "input" [ "type", "checkbox"; "id", "accord" ] []
                           texte " J'accepte que mon prénom, ma décennie et mon pays de naissance soient comptés pour les banques de prénoms du jeu. Seuls des totaux sont gardés (« tel prénom, tel pays, telle décennie : tant de personnes »), jamais une fiche à mon nom ; un prénom n'est utilisé qu'à partir de plusieurs déclarations." ]
                     div "piege" [ el "label" [ "for", "site" ] [ texte "Ne pas remplir" ]; entree "site" "text" "" [ "tabindex", "-1"; "autocomplete", "off" ] ]
                     el "div" [ "id", "turnstile" ] []
                     div "actions" [ bouton "principal" envoyer [ texte "Envoyer" ] ] ])
          el "div" [ "id", "message"; "aria-live", "polite" ] [] ]

let demarrer () =
    monter (unbox (document.getElementById "prenom-page")) (page ())

    if ouvert then
        chargerTurnstile ()

demarrer ()
