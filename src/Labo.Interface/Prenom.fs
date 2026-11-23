/// La page « Proposer un prénom » (jeu/prenom.html) : un prénom qu'on donnait dans un pays à une
/// génération, et à qui il se donne (aux filles, aux garçons, aux deux). Le joueur y arrive depuis
/// la fiche d'un habitant à qui il a donné ce prénom : la page est préremplie (décennie, origine,
/// sexe du personnage). C'est un témoignage sur des prénoms, pas une donnée sur le joueur. Le relais
/// n'en garde que des comptes ; un prénom n'entre dans une banque qu'au-delà d'un seuil de
/// propositions, et après relecture (docs/relais.md).
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
    | Ok _ when not accord -> message "erreur" [ texte "Coche la case : ce prénom se donnait vraiment dans ce pays, à cette génération." ]
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
                      para "aide" [ texte "Il entrera dans la banque de ce pays quand assez de personnes l'auront proposé pour la même génération, et après une relecture." ] ]
            | Choice1Of2(_, j) -> message "erreur" [ texte (string j?erreur) ]
            | Choice2Of2 _ -> message "erreur" [ texte "Le relais ne répond pas. Réessaie plus tard." ]
        }
        |> Async.StartImmediate

let page () =
    // préremplie depuis la fiche d'un habitant : prénom, décennie, origine (`ici` ou un groupe de pays), sexe
    let origine = parametre "origine"
    let decennieDonnee = parametre "decennie"
    let usageDonne = parametre "usage"

    let codes =
        match Labo.Data.Pays.Fr.origines |> List.tryFind (fun o -> o.id = origine) with
        | Some o -> o.pays
        | None when origine = "ici" -> [ "fr" ]
        | None -> Labo.Data.Prenoms.paysDuMonde |> List.map fst

    let pays =
        codes
        |> List.map (fun code -> code, nomPays (code.ToUpperInvariant()))
        |> List.sortBy (fun (_, n) -> n.Normalize(System.Text.NormalizationForm.FormD))

    let option (valeur: string) (libelle: string) (choisie: bool) =
        el "option" ((if choisie then [ "selected", "" ] else []) @ [ "value", valeur ]) [ texte libelle ]

    div
        "page"
        [ el
              "header"
              [ "class", "tete" ]
              [ el "a" [ "href", "./"; "class", "lien-discret" ] [ texte "← Retour au jeu" ]
                h1 [ texte "Proposer un prénom" ]
                para
                    "accroche"
                    [ texte "Les habitants du jeu reçoivent un prénom de leur génération et de leur pays de naissance. Pour beaucoup de pays, aucune statistique publique ne dit quels prénoms on y donnait, décennie par décennie : les prénoms que vous proposez aident à le savoir." ] ]
          (if not ouvert then
               para "carte discret" [ texte "L'envoi ouvre bientôt." ]
           else
               el
                   "section"
                   [ "class", "carte" ]
                   [ champ "prenom" "Le prénom" "Comme il s'écrit." (entree "prenom" "text" (parametre "prenom") [ "maxlength", "40"; "autocomplete", "off" ])
                     champ
                         "decennie"
                         "Donné dans les années"
                         "La décennie de naissance de celles et ceux qui le portent."
                         (el "select" [ "id", "decennie" ] [ option "" "—" (decennieDonnee = ""); yield! decennies |> List.map (fun d -> option (string d) $"années {d}" (decennieDonnee = string d)) ])
                     champ
                         "pays"
                         "Dans quel pays"
                         (if pays.Length > 1 && origine <> "" then "Le personnage est né dans l'un de ces pays : lequel ?" else "")
                         (el "select" [ "id", "pays" ] [ option "" "—" (pays.Length > 1); yield! pays |> List.map (fun (c, n) -> option c n (pays.Length = 1)) ])
                     el
                         "fieldset"
                         [ "class", "champ" ]
                         [ el "legend" [] [ texte "Là-bas, ce prénom se donne plutôt…" ]
                           para "aide" [ texte "Ça dit à quels personnages il peut aller." ]
                           yield!
                               usages
                               |> List.map (fun (code, libelle) ->
                                   el
                                       "label"
                                       [ "class", "choix" ]
                                       [ el "input" ([ "type", "radio"; "name", "usage"; "value", code; "id", "usage-" + code ] @ (if code = usageDonne then [ "checked", "" ] else [])) []
                                         texte (" " + libelle) ]) ]
                     el
                         "label"
                         [ "class", "choix" ]
                         [ el "input" [ "type", "checkbox"; "id", "accord" ] []
                           texte " Ce prénom se donnait vraiment dans ce pays, à cette génération : je le sais de première main, ou par mon entourage. (Il ne sert qu'après plusieurs propositions concordantes et une relecture.)" ]
                     div "piege" [ el "label" [ "for", "site" ] [ texte "Ne pas remplir" ]; entree "site" "text" "" [ "tabindex", "-1"; "autocomplete", "off" ] ]
                     el "div" [ "id", "turnstile" ] []
                     div "actions" [ bouton "principal" envoyer [ texte "Envoyer" ] ] ])
          el "div" [ "id", "message"; "aria-live", "polite" ] [] ]

let demarrer () =
    monter (unbox (document.getElementById "prenom-page")) (page ())

    if ouvert then
        chargerTurnstile ()

demarrer ()
