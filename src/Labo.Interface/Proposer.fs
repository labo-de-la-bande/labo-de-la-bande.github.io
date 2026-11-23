/// La page « Proposer une idée » (jeu/proposer.html) : préremplie depuis le jeu, elle envoie l'idée
/// sans compte par le relais, ou ouvre le formulaire GitHub pour qui a un compte, et montre où en
/// sont les propositions envoyées depuis ce navigateur. Construite une fois (pas de recréation de
/// l'écran, qui effacerait le widget anti-spam) ; seuls le message et la liste des suivis changent.
module Labo.Interface.Proposer

open Browser
open Browser.Types
open Fable.Core
open Fable.Core.JsInterop
open Labo.Interface.Html
open Labo.Interface.Relais
open Labo.Relais.Proposition
open Labo.Interface.Formulaire

// ---------- Le suivi ----------

let rec afficherSuivis () =
    match lireEnvoyees () with
    | [] -> monterDans "suivis" Rien
    | envoyees ->
        monterDans
            "suivis"
            (el
                "section"
                [ "class", "carte"; "id", "mes-propositions" ]
                [ h2 "" [ texte "Mes propositions" ]
                  para "aide" [ texte "Celles envoyées sans compte depuis ce navigateur. Le code de suivi suffit pour les retrouver ailleurs : garde le lien." ]
                  el "ul" [ "class", "suivis" ] [ for e in envoyees -> el "li" [ "id", "suivi-" + e.code ] [ texte "…" ] ] ])

        for e in envoyees do
            async {
                let! r = appeler "GET" ("/idees/" + e.code) None |> Async.Catch

                let contenu =
                    match r with
                    | Choice1Of2(200, j) ->
                        [ div "suivi-tete" [ el "strong" [] [ texte (if e.debut = "" then "Proposition" else e.debut) ]; span "puce" [ texte (string j?libelle) ] ]
                          (match j?ticket with
                           | null -> Rien
                           | t -> para "" [ lienExterne (string t) "Le ticket public (lisible sans compte)" ])
                          (match j?raison with
                           | null -> Rien
                           | t -> para "" [ texte ("Pourquoi : " + string t) ])
                          para
                              "discret"
                              [ texte $"Envoyée le {e.date} · "
                                el "a" [ "href", "#suivi=" + e.code ] [ texte "lien de suivi" ] ]
                          (if unbox<bool> j?mail then
                               bouton "lien-discret" (fun () -> oublier e.code) [ texte "Ne plus m'écrire (effacer mon adresse)" ]
                           else
                               Rien) ]
                    | Choice1Of2(_, j) -> [ texte $"{e.debut} : {string j?erreur}" ]
                    | Choice2Of2 _ -> [ texte $"{e.debut} : le relais ne répond pas pour l'instant." ]

                monterDans ("suivi-" + e.code) (div "" contenu)
            }
            |> Async.StartImmediate

and oublier (code: string) =
    async {
        let! _ = appeler "POST" ("/idees/" + code + "/oublier") None |> Async.Catch
        afficherSuivis ()
    }
    |> Async.StartImmediate

// ---------- L'envoi ----------

let lireFormulaire () =
    let provenance =
        provenances
        |> List.tryPick (fun (code, _) ->
            match document.getElementById ("provenance-" + code) with
            | null -> None
            | e when unbox<bool> e?``checked`` -> Some code
            | _ -> None)
        |> Option.defaultValue ""

    { situation = valeur "situation"
      idee = valeur "idee"
      appuis = valeur "appuis"
      provenance = provenance
      citation = valeur "citation"
      mail = Some(valeur "mail") }

let ouvrirGitHub () =
    let p = lireFormulaire ()
    window.``open`` (lienGitHub p.situation p.idee p.appuis p.citation, "_blank") |> ignore

let envoyer () =
    match valider (lireFormulaire ()) with
    | Error refus -> message "erreur" [ texte ("À corriger : " + (refus |> List.map decrireRefus |> String.concat " ; ") + ".") ]
    | Ok p when jetonTurnstile = "" -> message "erreur" [ texte "La vérification anti-spam n'est pas encore passée : attends la coche, puis réessaie." ]
    | Ok p ->
        let cle = valeur "cle"
        garderConfiance cle
        message "" [ texte "Envoi…" ]

        async {
            let corps =
                createObj
                    [ "situation" ==> p.situation
                      "idee" ==> p.idee
                      "appuis" ==> p.appuis
                      "provenance" ==> p.provenance
                      "citation" ==> p.citation
                      "mail" ==> defaultArg p.mail ""
                      "cle" ==> cle
                      "site" ==> valeur "site"
                      "jeton" ==> jetonTurnstile ]

            let! r = appeler "POST" "/idees" (Some corps) |> Async.Catch
            jetonTurnstile <- ""
            reinitialiserTurnstile ()

            match r with
            | Choice1Of2(201, j) ->
                let code = string j?code
                let debut = if p.idee.Length > 50 then p.idee.Substring(0, 50) + "…" else p.idee
                garder { code = code; date = System.DateTime.Now.ToString("dd/MM/yyyy"); debut = debut }

                message
                    "bon"
                    [ para "" [ el "strong" [] [ texte (if string j?statut = "publiee" then "Merci ! Ta proposition est publiée." else "Merci ! Ta proposition est reçue : elle sera relue avant d'être publiée.") ] ]
                      para "" [ texte "Pour suivre où elle en est, garde ce lien : "; el "a" [ "href", "#suivi=" + code ] [ texte "suivre ma proposition" ] ]
                      (if p.mail.IsSome then para "aide" [ texte "Tu recevras un mail à chaque étape." ] else Rien) ]

                afficherSuivis ()
            | Choice1Of2(_, j) -> message "erreur" [ texte (string j?erreur) ]
            | Choice2Of2 _ -> message "erreur" [ texte "Le relais ne répond pas. Réessaie plus tard, ou passe par GitHub si tu as un compte." ]
        }
        |> Async.StartImmediate

// ---------- La page ----------

let page () =
    div
        "page"
        [ el
              "header"
              [ "class", "tete" ]
              [ el "a" [ "href", "./"; "class", "lien-discret" ] [ texte "← Retour au jeu" ]
                h1 [ texte "Proposer une idée" ]
                para "accroche" [ texte "Ton idée entre dans le jeu comme une proposition : elle tient en théorie et attend sa preuve. Une personne la relit avant qu'elle pèse sur les scores, et dit ce qu'elle devient." ] ]
          el
              "section"
              [ "class", "carte" ]
              [ champ "situation" "La situation du jeu" "Remplie si tu viens du jeu." (entree "situation" "text" (parametre "situation") [])
                champ "idee" "Ton idée" "Ce qu'il faudrait faire, et ce que ça changerait pour les gens." (zone "idee" (parametre "idee") 5)
                champ "appuis" "Sur quoi elle s'appuie (si possible)" "Un exemple réel, une étude, un auteur, avec un lien si tu l'as. Sans appui, l'idée reste une proposition, et c'est très bien." (zone "appuis" "" 3)
                el
                    "fieldset"
                    [ "class", "champ" ]
                    [ el "legend" [] [ texte "D'où vient ce texte ?" ]
                      para "aide" [ texte "Pour que le jeu affiche honnêtement d'où viennent les idées. Aucune réponse n'est mal vue." ]
                      yield!
                          provenances
                          |> List.map (fun (code, libelle) ->
                              el "label" [ "class", "choix" ] [ el "input" [ "type", "radio"; "name", "provenance"; "id", "provenance-" + code ] []; texte (" " + libelle) ]) ]
                champ "citation" "Comment te citer dans le jeu (facultatif)" "Un pseudonyme. Vide, l'idée sera signée « un joueur »." (entree "citation" "text" "" [ "maxlength", "60" ]) ]
          (if ouvert && ideesSansCompte then
               el
                   "section"
                   [ "class", "carte" ]
                   [ h2 "" [ texte "Envoyer sans compte" ]
                     champ
                         "mail"
                         "Ton adresse mail (facultatif)"
                         "Pour être prévenu quand ta proposition avance. Elle ne sert qu'à ça, n'apparaît dans aucun ticket, et elle est effacée dès que la proposition est intégrée ou écartée, ou quand tu le demandes."
                         (entree "mail" "email" "" [ "autocomplete", "email" ])
                     el
                         "details"
                         [ "class", "champ" ]
                         [ el "summary" [] [ texte "J'ai une clé de confiance" ]
                           para "aide" [ texte "Donnée par l'équipe aux personnes de confiance : ta proposition est publiée sans attendre la relecture. Elle reste dans ce navigateur." ]
                           entree "cle" "text" (lireConfiance ()) [ "autocomplete", "off" ] ]
                     // le piège à robots : invisible pour une personne, un robot le remplit
                     div "piege" [ el "label" [ "for", "site" ] [ texte "Ne pas remplir" ]; entree "site" "text" "" [ "tabindex", "-1"; "autocomplete", "off" ] ]
                     el "div" [ "id", "turnstile" ] []
                     div "actions" [ bouton "principal" envoyer [ texte "Envoyer sans compte" ] ] ]
           else
               para "carte discret" [ texte "L'envoi sans compte ouvre bientôt. En attendant, il faut un compte GitHub." ])
          el
              "section"
              [ "class", "carte" ]
              [ h2 "" [ texte "J'ai un compte GitHub" ]
                para "aide" [ texte "Le ticket s'ouvre directement, sans relecture préalable, avec ce que tu as écrit ici." ]
                div "actions" [ bouton "secondaire" ouvrirGitHub [ texte "Ouvrir le ticket sur GitHub" ] ] ]
          el "div" [ "id", "message"; "aria-live", "polite" ] []
          el "div" [ "id", "suivis" ] [] ]

let demarrer () =
    monter (unbox (document.getElementById "proposer")) (page ())

    // un lien de suivi (#suivi=<code>) : la proposition rejoint la liste de ce navigateur
    let h = window.location.hash

    if h.StartsWith "#suivi=" then
        let code = h.Substring 7

        if not (lireEnvoyees () |> List.exists (fun e -> e.code = code)) then
            garder { code = code; date = System.DateTime.Now.ToString("dd/MM/yyyy"); debut = "" }

    if ouvert && ideesSansCompte then
        afficherSuivis ()

        chargerTurnstile ()

    window.addEventListener ("hashchange", fun _ -> if window.location.hash.StartsWith "#suivi=" then window.location.reload ())

demarrer ()
