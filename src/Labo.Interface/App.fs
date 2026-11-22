/// L'écran du jeu : un état, des messages, une vue recréée à chaque changement.
module Labo.Interface.App

open Browser
open Browser.Types
open Labo.Data
open Labo.Engine
open Labo.Jeu
open Labo.Jeu.Partie
open Labo.Interface.Html
open Labo.Interface.Textes

module F = Labo.Data.Pays.Fr

type Ecran =
    | Accueil
    | EnJeu
    | Fin

type Modele =
    { ecran: Ecran
      partie: Partie
      /// la situation en cours d'examen et les choix envisagés (pour l'aperçu)
      envisage: (string * Choix list) option
      /// la révolution dont on a déjà lu les nouvelles
      nouvellesLues: int
      /// les répliques choisies par le joueur dans la scène de chaque situation
      choixScene: Map<string, string list>
      /// les situations où le joueur a déjà proposé son idée
      ideesProposees: Set<string>
      /// le mode apprentissage : Labo indique l'option qui remonte le plus le maillon faible
      apprentissage: bool
      /// la dernière décision qui a une scène de réaction : situation, option
      reaction: (string * string) option
      /// le dernier revirement, pour la carte qui le raconte : situation, choix repris
      revirement: (string * Choix list * (string * float) list) option
      /// les situations où le joueur a ouvert le champ de son idée
      ideeOuverte: Set<string>
      /// l'habitant dont la fiche est ouverte sous le paysage
      habitantOuvert: string option
      /// la région choisie à l'accueil pour la prochaine partie
      region: string
      /// la tranche où commence la prochaine partie (les suivantes viennent ensuite)
      depart: string
      /// la partie sauvegardée dans le navigateur, si elle se relit
      sauvegarde: Partie option
      /// un message à afficher (code copié, code illisible…)
      note: string option
      erreur: string option }

type Message =
    | Commencer
    | Envisager of situation: string * choix: Choix list
    | Decider
    | RevolutionSuivante
    | LireNouvelles
    | Recommencer
    | Repondre of cle: string * situation: string * replique: string
    | BasculerApprentissage
    | FermerReaction
    | RevenirSur of situation: string
    | FermerRevirement
    | RevenirDansScene of cle: string
    | OuvrirIdee of situation: string
    | ProposerIdee of situation: string * texte: string
    | ChoisirRegion of region: string
    | ChoisirDepart of tranche: string
    | OuvrirHabitant of habitant: string
    | Reprendre
    | ChargerCode of code: string
    | CopierCode

let graineDuJour () = int64 (System.DateTime.Now.Ticks % 2147483647L)

/// Le parcours d'une partie qui commence à une tranche : celle-ci et toutes les suivantes.
let parcoursDepuis tranche =
    match parcoursComplet |> List.skipWhile ((<>) tranche) with
    | [] -> parcoursComplet
    | p -> p

let nouvellePartie region depart =
    match Journal.commencer { region = region; graine = graineDuJour (); hasard = true; parcours = parcoursDepuis depart } with
    | Ok p -> p
    | Error e -> failwith e

let cleSauvegarde = "labo-partie"

/// La partie sauvegardée dans ce navigateur (le stockage peut être indisponible : rien ne casse).
let lireSauvegarde () =
    try
        match Browser.WebStorage.localStorage.getItem cleSauvegarde with
        | null -> None
        | code -> match Journal.decoder code with Ok p -> Some p | Error _ -> None
    with _ ->
        None

let ecrireSauvegarde (p: Partie) =
    try Browser.WebStorage.localStorage.setItem (cleSauvegarde, Journal.encoder p) with _ -> ()

/// Les répliques déjà choisies dans chaque scène, relues dans le journal (pour une partie reprise).
let parcoursDesScenes (p: Partie) =
    p.actions
    |> List.choose (function
        | Action.Repondre(s, id) ->
            match Scene.repliqueDe.TryFind id with
            | Some r -> Some((if r.moment = Avant then "" else "apres:") + s, id)
            | None -> None
        | _ -> None)
    |> List.groupBy fst
    |> List.map (fun (cle, l) -> cle, l |> List.map snd)
    |> Map.ofList

let initial () =
    { ecran = Accueil
      partie = nouvellePartie "france" (List.head parcoursComplet)
      envisage = None
      nouvellesLues = 0
      choixScene = Map.empty
      ideesProposees = Set.empty
      apprentissage = false
      reaction = None
      revirement = None
      ideeOuverte = Set.empty
      habitantOuvert = None
      region = "france"
      depart = List.head parcoursComplet
      sauvegarde = lireSauvegarde ()
      note = None
      erreur = None }

/// La première décision qui attend pendant cette révolution.
let decisionCourante (p: Partie) = ouvertes p |> List.tryFind estDecision

/// Les choix par défaut proposés quand une situation s'ouvre.
let choixParDefaut (s: Situation) =
    match s.genre with
    | Paris -> [ { levier = "paris"; option = "aucun" } ]
    | Prevention -> s.leviers |> List.map (fun l -> { levier = l; option = "non" })
    | _ -> []

/// Joue une action du joueur : elle entre au journal de la partie.
let agir (a: Action) (m: Modele) =
    match Journal.jouer a m.partie with
    | Ok p -> Ok { m with partie = p; erreur = None }
    | Error e -> Error { m with erreur = Some e }

let resultat =
    function
    | Ok m
    | Error m -> m

/// Une partie reprise ou chargée : l'écran se remet dans l'état qu'elle décrit.
let surPartie (p: Partie) (m: Modele) =
    { m with
        partie = p
        ecran = (if terminee p then Fin else EnJeu)
        envisage = None
        nouvellesLues = p.revolution
        choixScene = parcoursDesScenes p
        ideesProposees = idees p |> List.map (fun (_, s, _) -> s) |> Set.ofList
        reaction = None
        revirement = None
        erreur = None
        note = None }

let miseAJour msg (m: Modele) =
    match msg with
    | ChoisirRegion r -> { m with region = r }
    | ChoisirDepart t -> { m with depart = t }
    | OuvrirHabitant h -> { m with habitantOuvert = (if m.habitantOuvert = Some h then None else Some h) }
    | Commencer -> { m with partie = nouvellePartie m.region m.depart; ecran = EnJeu; nouvellesLues = 0; erreur = None; choixScene = Map.empty; ideesProposees = Set.empty }
    | Recommencer -> { initial () with apprentissage = m.apprentissage; region = m.region; depart = m.depart; sauvegarde = None }
    | Reprendre ->
        match m.sauvegarde with
        | Some p -> surPartie p m
        | None -> m
    | ChargerCode code ->
        match Journal.decoder code with
        | Ok p -> surPartie p m
        | Error e -> { m with note = Some(Journal.decrireErreur e) }
    | CopierCode -> m
    | Envisager(s, c) -> { m with envisage = Some(s, c); erreur = None }
    | LireNouvelles -> { m with nouvellesLues = m.partie.revolution }
    | Repondre(cle, s, id) ->
        let deja = m.choixScene.TryFind cle |> Option.defaultValue []

        agir (Action.Repondre(s, id)) m
        |> Result.map (fun m -> { m with choixScene = m.choixScene.Add(cle, deja @ [ id ]) })
        |> resultat
    | BasculerApprentissage -> { m with apprentissage = not m.apprentissage }
    | FermerReaction -> { m with reaction = None }
    | FermerRevirement -> { m with revirement = None }
    | RevenirSur s ->
        let repris = dernierChoix m.partie s |> Option.defaultValue []
        // l'effet du revirement se calcule avant : après, le ressenti de la décision est oublié
        let effets = effetRevirement m.partie situationDe[s] repris

        agir (Action.RevenirSur s) m
        |> Result.map (fun m -> { m with reaction = None; revirement = Some(s, repris, effets); envisage = None })
        |> resultat
    | RevenirDansScene cle ->
        let deja = m.choixScene.TryFind cle |> Option.defaultValue []
        { m with choixScene = m.choixScene.Add(cle, (if deja.IsEmpty then [] else List.take (deja.Length - 1) deja)) }
    | OuvrirIdee s -> { m with ideeOuverte = m.ideeOuverte.Add s }
    | ProposerIdee(s, texte) when texte.Trim() = "" -> { m with ideeOuverte = m.ideeOuverte.Remove s }
    | ProposerIdee(s, texte) ->
        agir (Action.ProposerIdee(s, texte)) m
        |> Result.map (fun m -> { m with ideesProposees = m.ideesProposees.Add s; ideeOuverte = m.ideeOuverte.Remove s })
        |> resultat
    | Decider ->
        match m.envisage with
        | Some(s, c) when not c.IsEmpty ->
            let reaction =
                match c with
                | premier :: _ when situationDe[s].genre = Dilemme -> Some(s, premier.option)
                | _ -> None

            agir (Action.Decider(s, c)) m
            |> Result.map (fun m -> { m with envisage = None; reaction = reaction })
            |> resultat
        | _ -> m
    | RevolutionSuivante ->
        agir Action.FinirRevolution m
        |> Result.map (fun m -> { m with envisage = None; ecran = (if terminee m.partie then Fin else EnJeu) })
        |> resultat

// ---------- La vue ----------

let mutable envoyer: Message -> unit = ignore

let ton (v: float) =
    if v >= 0.66 then "bon"
    elif v >= 0.4 then "moyen"
    else "faible"

/// Une jauge : sa valeur, le repère de la France, et l'aperçu de la décision envisagée.
let jauge (j: Moteur.JaugeCalculee) (france: float) (apres: float option) =
    let v = j.valeur.Milieu
    let pct (x: float) = $"{max 0.0 (min 100.0 (x * 100.0))}%%"

    let ecart =
        match apres with
        | Some a when abs (a - v) >= 0.0005 -> span (if a > v then "ecart hausse" else "ecart baisse") [ texte (signe ((a - v) * 100.0)) ]
        | _ -> Rien

    let aide = (Jauges.tous |> List.find (fun x -> x.id = j.id)).aide

    // un clic sur la jauge déplie ce qu'elle mesure, et la France d'aujourd'hui
    el
        "details"
        [ "class", "jauge" ]
        [ el
              "summary"
              []
              [ div "jauge-tete" [ span "jauge-nom" [ texte (libelleJauge j.id) ]; ecart; span "jauge-valeur" [ texte (nombre (v * 100.0)) ] ]
                el
                    "div"
                    [ "class", "barre"; "title", $"France : {nombre (france * 100.0)}" ]
                    [ el "div" [ "class", "plein " + ton v; "style", style "width" (pct v) ] []
                      (match apres with
                       | Some a -> el "div" [ "class", "fantome"; "style", style "width" (pct a) ] []
                       | None -> Rien)
                      el "div" [ "class", "repere"; "style", style "left" (pct france) ] [] ] ]
          div
              "jauge-aide"
              [ para "" (riche aide)
                para "discret" [ texte $"La France d'aujourd'hui : {nombre (france * 100.0)} (le trait sur la barre)." ] ] ]

let panneauJauges (p: Partie) (apres: Moteur.Resultat option) (extra: Noeud) =
    let r = Moteur.evaluer p.etat

    let groupe titre filtre =
        div
            "groupe"
            [ h3 "petit-titre" [ texte titre ]
              yield!
                  r.jauges
                  |> List.filter filtre
                  |> List.map (fun j ->
                      let a = apres |> Option.map (fun x -> (x.jauges |> List.find (fun y -> y.id = j.id)).valeur.Milieu)
                      jauge j (Js.clamp01 (Moteur.valeurFrance j.id p.etat)) a) ]

    el
        "aside"
        [ "class", "jauges"; "aria-label", "Les jauges" ]
        [ groupe "Les résultats" (fun j -> not j.horsScore)
          groupe "Le but" (fun j -> j.horsScore)
          para "legende" [ texte "Le trait marque la France d’aujourd’hui ; la zone claire, l’effet de la décision envisagée." ]
          extra ]

let entete (m: Modele) (p: Partie) =
    let r = Moteur.evaluer p.etat

    let tranche =
        match trancheCourante p with
        | Some t -> Jeu.tranches |> List.find (fun x -> x.id = t)
        | None -> Jeu.tranches |> List.last

    el
        "header"
        [ "class", "entete" ]
        [ div
              "orbite"
              [ el
                    "div"
                    [ "class", "planete"
                      "style", style "transform" $"rotate({(p.revolution % 12) * 30}deg) translateX(15px)" ]
                    [] ]
          div "titre-revolution" [ span "petit-titre" [ texte "Révolution" ]; span "numero" [ texte (string p.revolution) ] ]
          div "titre-tranche" [ span "embleme" [ texte tranche.embleme ]; texte tranche.titre ]
          (match F.regions |> List.tryFind (fun r -> r.id = p.region) with
           | Some r -> span "puce" [ texte r.nom ]
           | None -> Rien)
          div
              "scores"
              [ span "score" [ texte $"Score {nombre r.score}" ]
                span "france" [ texte $"France {nombre r.scoreFrance}" ]
                span (if r.viable.ok then "puce bon" else "puce faible") [ texte (if r.viable.ok then "viable" else "pas viable") ]
                boutonAvec
                    [ "class", "bascule"; "aria-pressed", (if m.apprentissage then "true" else "false") ]
                    (fun () -> envoyer BasculerApprentissage)
                    [ texte (if m.apprentissage then "Apprentissage : oui" else "Apprentissage : non") ] ] ]

let carteConstat (s: Situation) =
    el
        "article"
        [ "class", "carte constat" ]
        [ h2 "" [ texte s.titre ]
          div
              "chiffres"
              [ for c in Jeu.chiffres |> List.filter (fun c -> c.situation = s.id) do
                    div
                        "chiffre"
                        [ span "chiffre-valeur" [ texte c.valeur ]
                          span
                              "chiffre-legende"
                              [ texte c.legende
                                yield! c.sources |> List.map (fun id -> el "sup" [ "class", "ref"; "title", titreSource id ] [ texte "•" ]) ] ] ]
          para "" (riche s.texte) ]

/// Labo, le robot : il signe ce qui vient d'un assistant.
let labo (phrase: string) =
    div "labo" [ div "labo-tete" [ div "labo-oeil" []; div "labo-oeil" [] ]; para "labo-bulle" [ texte phrase ] ]

let avatar (id: string) (nom: string) =
    el "div" [ "class", "avatar"; "style", style "background" (teinte id); "aria-hidden", "true" ] [ texte (initiales nom) ]

let panneauConseil (a: Apercu) =
    let parAssistant = a.conseil |> List.exists (fun av -> av.arguments |> List.exists (fun x -> x.lien.lentillePar = "assistant"))

    div
        "conseil"
        [ h3 "petit-titre" [ texte "Le conseil" ]
          (if a.conseil.IsEmpty then para "discret" [ texte "Aucune lentille n’a rien à dire de ce choix." ] else Rien)
          yield!
              a.conseil
              |> List.truncate 4
              |> List.map (fun av ->
                  div
                      "avis"
                      [ avatar av.lentille.id av.lentille.nom
                        div
                            "avis-corps"
                            [ div "avis-nom" [ texte av.lentille.nom ]
                              yield!
                                  av.arguments
                                  |> List.truncate 2
                                  |> List.map (fun x ->
                                      let etat = if x.avant = 0.0 then "" elif x.apres = 0.0 then " (ne joue plus)" else ""

                                      para
                                          "argument"
                                          [ span (if x.Ecart >= 0.0 then "ecart hausse" else "ecart baisse") [ texte (signe x.Ecart) ]
                                            texte $" {libelleJauge x.lien.jauge}{etat} : "
                                            yield! riche x.lien.raison
                                            span "statut" [ texte $" ({x.lien.statut})" ] ]) ] ])
          (if parAssistant then
               labo "Les arguments sont rangés par lentille : une proposition, à vérifier."
           else
               Rien) ]

let panneauVoix (a: Apercu) =
    if a.voix.IsEmpty && a.reputation |> List.forall (fun (_, d) -> d = 0.0) then
        Rien
    else
        div
            "voix"
            [ h3 "petit-titre" [ texte "Les habitants" ]
              yield!
                  a.voix
                  |> List.map (fun v ->
                      div
                          "bulle"
                          [ avatar v.habitant (nomHabitant v.habitant)
                            para "" [ el "strong" [] [ texte (nomHabitant v.habitant) ]; texte $" : « {v.texte} »" ] ])
              div
                  "confiances"
                  [ for (h, d) in a.reputation |> List.filter (fun (_, d) -> d <> 0.0) do
                        span (if d > 0.0 then "puce bon" else "puce faible") [ texte $"{nomHabitant h} {signe d}" ] ] ]

/// Qui parle : son nom, et son avatar.
let locuteur (r: Replique) =
    match r.nature with
    | Joueur -> "Toi", el "div" [ "class", "avatar joueur"; "aria-hidden", "true" ] [ texte "Toi" ]
    | Rappel -> "Labo", div "labo-tete petite" [ div "labo-oeil" []; div "labo-oeil" [] ]
    | Paraphrase ->
        let l = Modele.lentilles |> List.find (fun x -> x.id = r.locuteur)
        l.nom, avatar l.id l.nom
    | Dit ->
        let n = nomHabitant r.locuteur
        n, avatar r.locuteur n
    | Souvenir ->
        // la personne parle depuis un autre contexte : on le dit à côté de son nom
        let n = nomHabitant r.locuteur
        let passe = Jeu.antecedents |> List.tryFind (fun a -> a.habitant = r.locuteur) |> Option.map _.role
        (match passe with Some p -> n + ", " + p | None -> n), avatar r.locuteur n

let lienDe id = Liens.tous |> List.tryFind (fun l -> l.id = id)

/// D'où vient une réplique : la source ou le lien du modèle qu'elle paraphrase ou rappelle.
let origine (r: Replique) =
    let sources = r.sources |> List.map titreSource

    let lien =
        r.lien |> Option.bind lienDe |> Option.map (fun l -> $"lien du modèle : {libelleJauge l.jauge} {signe l.effet}, {l.statut}")

    let tout = sources @ Option.toList lien

    match r.nature, tout with
    | Paraphrase, _ -> span "origine" [ texte ("paraphrase" + (if tout.IsEmpty then "" else " · " + String.concat " · " tout)) ]
    | Rappel, _ :: _ -> span "origine" [ texte ("rappel du modèle · " + String.concat " · " tout) ]
    | Rappel, [] -> span "origine" [ texte "rappel du modèle" ]
    | Souvenir, _ -> span "origine" [ texte ("souvenir d'un personnage de fiction · faits réels : " + String.concat " · " tout) ]
    | _ -> Rien

/// Le ticket GitHub d'une idée de joueur.
let urlTicket (n: int) =
    $"https://github.com/labo-de-la-bande/labo-de-la-bande.github.io/issues/{n}"

/// L'idée de joueur qu'une option porte, si elle en porte une.
let ideeDe levier option =
    Jeu.idees |> List.tryFind (fun i -> i.statut = Integree && i.levier = levier && i.option = option)

/// Où trouver une idée intégrée : la situation et la tranche.
let ouTrouver (i: IdeeJoueur) =
    match Jeu.situations |> List.tryFind (fun s -> s.leviers |> List.contains i.levier) with
    | Some s ->
        let t = Jeu.tranches |> List.find (fun t -> t.id = s.tranche)
        $"« {s.titre} » ({t.titre})"
    | None -> ""

/// Les idées des joueurs déjà intégrées au jeu, avec leur ticket et où les trouver.
let listeIdeesIntegrees () =
    match Jeu.idees |> List.filter (fun i -> i.statut = Integree) with
    | [] -> Rien
    | l ->
        div
            "idees-integrees"
            [ h3 "petit-titre" [ texte "Les idées des joueurs, déjà dans le jeu" ]
              el
                  "ul"
                  []
                  [ for i in l do
                        el
                            "li"
                            []
                            [ el "strong" [] [ texte (Tables.optionsDe[i.levier] |> List.find (fun o -> o.id = i.option)).libelle ]
                              texte $" : à la question {ouTrouver i}, proposée par {i.par}. "
                              el "a" [ "href", urlTicket i.ticket; "target", "_blank"; "rel", "noopener" ] [ texte $"Ticket n° {i.ticket}" ] ] ] ]

let lienIdee (s: Situation) (idee: string) =
    let titre = System.Uri.EscapeDataString("Idée : " + (if idee.Length > 60 then idee.Substring(0, 60) + "…" else idee))
    let situation = System.Uri.EscapeDataString s.titre
    let texteIdee = System.Uri.EscapeDataString idee
    $"https://github.com/labo-de-la-bande/labo-de-la-bande.github.io/issues/new?template=idee.yml&title={titre}&situation={situation}&idee={texteIdee}"

/// Une scène : avant la décision, ou la réaction à l'option choisie. Les répliques déjà dites,
/// puis les choix du joueur.
let panneauScene (m: Modele) (s: Situation) moment (option: string option) =
    let ctx = contexteScene m.partie
    let cle = (if moment = Avant then "" else "apres:") + s.id

    match Scene.entree ctx s.id moment option with
    | None -> Rien
    | Some _ ->
        let choisis = m.choixScene.TryFind cle |> Option.defaultValue []
        let lignes, ouverts = Scene.rejouer ctx s.id moment option choisis
        let ideeAttendue = lignes |> List.exists (fun r -> r.action = Some ActionReplique.Idee) && not (m.ideesProposees.Contains s.id)

        // après « j'ai une autre idée », la suite attend que l'idée soit proposée
        let visibles =
            if ideeAttendue then
                let i = lignes |> List.findIndex (fun r -> r.action = Some ActionReplique.Idee)
                lignes |> List.truncate (i + 1)
            else
                lignes

        let bulle (r: Replique) =
            let nom, av = locuteur r

            div
                (if r.nature = Joueur then "replique du-joueur" else "replique")
                [ av
                  div "replique-corps" [ div "replique-nom" [ texte nom ]; para "" (riche r.texte); origine r ] ]

        let ideesIci = idees m.partie |> List.filter (fun (_, sit, _) -> sit = s.id) |> List.map (fun (_, _, t) -> t)
        let champIdee = ideeAttendue || (moment = Avant && m.ideeOuverte.Contains s.id)

        el
            "section"
            [ "class", "scene-dialogue"; "aria-label", "La conversation" ]
            [ yield! visibles |> List.map bulle
              yield!
                  ideesIci
                  |> List.map (fun t ->
                      div
                          "idee-proposee"
                          [ para "" [ el "strong" [] [ texte "Ton idée : " ]; texte t ]
                            el "a" [ "href", lienIdee s t; "target", "_blank"; "rel", "noopener" ] [ texte "L’envoyer au projet (formulaire GitHub)" ] ])
              (if champIdee then
                   div
                       "idee"
                       [ listeIdeesIntegrees ()
                         el "label" [ "for", "idee-" + s.id ] [ texte "Ton idée, en quelques phrases : ce qu’il faudrait faire, et ce que ça changerait pour les gens." ]
                         el "textarea" [ "id", "idee-" + s.id; "rows", "3" ] []
                         div
                             "actions"
                             [ bouton
                                   "principal"
                                   (fun () ->
                                       let zone = document.getElementById ("idee-" + s.id)
                                       let texteSaisi: string = Fable.Core.JsInterop.(?) zone "value"
                                       envoyer (ProposerIdee(s.id, texteSaisi)))
                                   [ texte "Proposer" ] ] ]
               elif not ouverts.IsEmpty then
                   div
                       "repliques-joueur"
                       [ for o in ouverts do
                             bouton "replique-choix" (fun () -> envoyer (Repondre(cle, s.id, o.id))) [ texte (nommer o.texte) ] ]
               else
                   Rien)
              div
                  "scene-outils"
                  [ (if choisis.IsEmpty then Rien
                     else bouton "lien-discret" (fun () -> envoyer (RevenirDansScene cle)) [ texte "← Revenir en arrière" ])
                    (if moment = Avant && not champIdee then
                         bouton "lien-discret" (fun () -> envoyer (OuvrirIdee s.id)) [ texte "J’ai une idée" ]
                     else
                         Rien) ]
              (if visibles |> List.exists (fun r -> r.provenance = "assistant") then
                   labo "Ces répliques sont des brouillons, à réécrire."
               else
                   Rien) ]

/// Le mode apprentissage : Labo indique l'option qui remonte le plus le maillon faible, ou le score.
let conseilLabo (m: Modele) (s: Situation) =
    if not m.apprentissage then
        Rien
    else
        match conseilApprentissage m.partie s.id with
        | Some c when c.gain > 0.0005 ->
            let libelle =
                match c.choix with
                | [ { levier = "paris"; option = b } ] -> (Modele.paris |> List.find (fun x -> x.id = b)).titre
                | [ ch ] -> (Tables.optionsDe[ch.levier] |> List.find (fun o -> o.id = ch.option)).libelle
                | _ -> ""

            let critere =
                match c.jauge with
                | Some j -> $"pour remonter {libelleJauge j}, ton maillon faible, « {libelle} » fait le plus ({signe c.gain} points à terme)"
                | None -> $"pour le score, « {libelle} » fait le plus ({signe c.gain} points à terme)"

            labo $"Mode apprentissage : {critere}. Ce n’est qu’un calcul : les principes, l’égalité et la confiance restent à toi."
        | Some _ -> labo "Mode apprentissage : aucune option ne remonte ton maillon faible ni le score ici. Choisis selon tes principes."
        | None -> Rien

let carteDecision (m: Modele) (s: Situation) =
    let choix =
        match m.envisage with
        | Some(id, c) when id = s.id -> c
        | _ -> choixParDefaut s

    let apercu =
        if choix.IsEmpty then None
        else match apercu m.partie s.id choix with Ok a -> Some a | Error _ -> None

    let optionAvec (badge: Noeud) (cle: string) (libelle: string) (note: string) (choisi: bool) (nouveaux: Choix list) =
        boutonAvec
            [ "class", (if choisi then "option choisie" else "option"); "aria-pressed", (if choisi then "true" else "false") ]
            (fun () -> envoyer (Envisager(s.id, nouveaux)))
            [ badge; span "option-libelle" (riche libelle); (if note = "" then Rien else span "option-note" (riche note)) ]

    let option = optionAvec Rien

    let options =
        match s.genre with
        | Dilemme ->
            let levier = List.exactlyOne s.leviers

            Tables.optionsDe[levier]
            |> List.map (fun o ->
                let badge =
                    match ideeDe levier o.id with
                    | Some i -> span "badge-idee" [ texte $"💡 Idée de joueur · ticket n° {i.ticket}" ]
                    | None -> Rien

                optionAvec badge o.id o.libelle o.note (choix = [ { levier = levier; option = o.id } ]) [ { levier = levier; option = o.id } ])
        | Paris ->
            // plusieurs paris à la fois, jusqu'à la limite ; « aucun pari » vide la sélection
            let choisis = choix |> List.filter (fun c -> c.option <> "aucun")
            let limite = int Coefficients.jeuParisParOccasion

            [ yield!
                  s.paris
                  |> List.map (fun id ->
                      let b = Modele.paris |> List.find (fun x -> x.id = id)
                      let quand = if b.pretDans = 0 then "agit tout de suite" else $"agit dans {b.pretDans} révolutions"
                      let coche = choisis |> List.exists (fun c -> c.option = id)

                      let nouveaux =
                          if coche then
                              match choisis |> List.filter (fun c -> c.option <> id) with
                              | [] -> [ { levier = "paris"; option = "aucun" } ]
                              | reste -> reste
                          elif choisis.Length >= limite then choix
                          else choisis @ [ { levier = "paris"; option = id } ]

                      option b.id b.titre $"{quand}. {b.effet}" coche nouveaux)
              option "aucun" "Aucun pari" "" choisis.IsEmpty [ { levier = "paris"; option = "aucun" } ] ]
        | Prevention ->
            s.leviers
            |> List.map (fun l ->
                let oui = choix |> List.contains { levier = l; option = "oui" }

                let bascule =
                    choix |> List.map (fun c -> if c.levier = l then { c with option = (if oui then "non" else "oui") } else c)

                option l Tables.levierDe[l].titre (if oui then "Oui" else "Non") oui bascule)
        | _ -> []

    el
        "article"
        [ "class", "carte decision" ]
        [ div "decision-tete" [ span "embleme grand" [ texte s.embleme ]; h2 "" [ texte s.titre ] ]
          para "" (riche s.texte)
          panneauScene m s Avant None
          conseilLabo m s
          (if s.genre = Paris then
               para "discret" [ texte $"Tu peux cocher plusieurs paris, jusqu'à {int Coefficients.jeuParisParOccasion} ; chacun mûrit à son rythme, et leurs effets s'additionnent." ]
           else
               Rien)
          div "options" options
          (match apercu with
           | Some a ->
               div
                   "apercu"
                   [ para
                         "score-apercu"
                         [ texte $"Score : {nombre a.avant.score} → {nombre a.aussitot.score} aussitôt, {nombre a.aTerme.score} à terme" ]
                     panneauVoix a
                     panneauConseil a ]
           | None -> para "discret" [ texte "Choisis une option pour entendre le conseil et les habitants." ])
          div
              "actions"
              [ boutonAvec
                    [ "class", "principal"; (if choix.IsEmpty then "disabled" else "data-ok"), "" ]
                    (fun () ->
                        envoyer (Envisager(s.id, choix))
                        envoyer Decider)
                    [ texte "Décider" ] ] ]

/// La réaction à la dernière décision, quand elle a une scène.
/// Ceux qui ne parlent pas dans la réaction : un geste selon leur humeur.
/// Le libellé d'une option d'une situation (cherchée dans les leviers de la situation).
let libelleOptionDe (s: Situation) option =
    s.leviers
    |> List.tryPick (fun l -> Tables.optionsDe.TryFind l |> Option.bind (List.tryFind (fun o -> o.id = option)))
    |> Option.map _.libelle
    |> Option.defaultValue option

/// Ce que la décision fait à ce qui compte pour un habitant : l'attachement le plus touché.
let plusTouche (p: Partie) (s: Situation) habitant =
    let deltas = p.ressentis.TryFind s.id |> Option.defaultValue []

    Jeu.attachements
    |> List.filter (fun a -> a.habitant = habitant)
    |> List.map (fun a -> a, (deltas |> List.tryFind (fun (j, _) -> j = a.jauge) |> Option.map snd |> Option.defaultValue 0.0))
    |> List.sortByDescending (fun (a, d) -> abs (poidsEffectif p a * d))
    |> List.tryHead
    |> Option.filter (fun (_, d) -> abs d >= 0.05)

/// Pourquoi un habitant réagit ainsi à une décision : son avis d'avant, ce que tu as choisi, et ce
/// que la décision fait à ce qui compte pour lui.
let raisonHumeur (p: Partie) (s: Situation) (choix: Choix list) habitant =
    let n = nomHabitant habitant
    let choisi = choix |> List.exists (fun c -> Some c.option = (Journal.avisDe s.id habitant |> Option.map _.option))
    let r = p.vecus.TryFind s.id |> Option.bind (fun m -> m.TryFind habitant) |> Option.defaultValue 0.0
    let net = abs r >= Coefficients.jeuRessentiSeuil

    let touche =
        match plusTouche p s habitant with
        | Some(a, d) -> $"{a.raison} ({libelleJauge a.jauge} {signe d})"
        | None -> ""

    let avis =
        match Journal.avisDe s.id habitant with
        | None -> None
        | Some v ->
            let o = libelleOptionDe s v.option
            let cite = $" (« {nommer v.texte} »)"

            Some(
                match v.position, choisi with
                | Pour, true -> $"{n} défendait « {o} »{cite} : c’est ce que tu as choisi."
                | Pour, false -> $"{n} défendait « {o} »{cite} : tu as choisi autre chose."
                | Contre, true -> $"{n} redoutait « {o} »{cite} : c’est ce que tu as choisi."
                | Contre, false -> $"{n} redoutait « {o} »{cite} : tu l’as écartée."
                | Position.Reserve, true -> $"{n} avait des réserves sur « {o} »{cite} : tu l’as choisie."
                | Position.Reserve, false -> $"{n} avait des réserves sur « {o} »{cite} : tu l’as écartée."
            )

    match avis, net with
    | None, true when r > 0.0 -> $"{n} n’avait rien dit sur cette question, mais elle améliore ce qui compte à ses yeux : {touche}."
    | None, true -> $"{n} n’avait rien dit sur cette question, mais elle abîme ce qui compte à ses yeux : {touche}."
    | None, false -> $"{n} n’avait rien dit sur cette question, et elle touche peu ce qui compte à ses yeux."
    | Some a, true when r > 0.0 -> $"{a} Et ce qui compte à ses yeux s’en trouve mieux : {touche}."
    | Some a, true -> $"{a} Et ce qui compte à ses yeux en pâtit : {touche}."
    | Some a, false -> a

/// Le détail « pourquoi ? » sous un geste.
let pourquoi (p: Partie) (s: Situation) (choix: Choix list) habitant =
    let delta = effetDecision p s choix |> List.tryFind (fun (h, _) -> h = habitant) |> Option.map snd |> Option.defaultValue 0.0

    el
        "details"
        [ "class", "pourquoi" ]
        [ el "summary" [] [ texte "Pourquoi ?" ]
          para "" [ texte (raisonHumeur p s choix habitant) ]
          (if delta = 0.0 then Rien else para "discret" [ texte $"Sa confiance en toi : {signe delta}." ])
          (if delta >= 0.0 then
               Rien
           else
               // la confiance qu'il avait avant cette décision a coloré la déception (tyler2006)
               let avant = (p.reputation.TryFind habitant |> Option.defaultValue 0.0) - delta
               let f = facteurConfiance { p with reputation = p.reputation.Add(habitant, avant) } habitant

               if f < 0.99 then
                   para "discret" [ texte $"Comme {nomHabitant habitant} te faisait confiance, la déception a pesé moins (×{nombre f})." ]
               elif f > 1.01 then
                   para "discret" [ texte $"Comme {nomHabitant habitant} se méfiait déjà, la déception a pesé plus (×{nombre f})." ]
               else
                   Rien) ]

let panneauAttitudes (m: Modele) (s: Situation) option =
    let ctx = contexteScene m.partie
    let choisis = m.choixScene.TryFind("apres:" + s.id) |> Option.defaultValue []
    let lignes, _ = Scene.rejouer ctx s.id Apres (Some option) choisis
    let parlent = lignes |> List.filter (fun r -> r.nature = Dit || r.nature = Souvenir) |> List.map _.locuteur |> Set.ofList
    let choix = s.leviers |> List.map (fun l -> { levier = l; option = option })

    let libelleHumeur =
        function
        | Approuve -> "approuve"
        | Desapprouve -> "désapprouve"
        | Decu -> "déception"
        | Soulage -> "soulagement"
        | Dubitatif -> "doute"
        | Neutre -> ""

    let ton =
        function
        | Approuve
        | Soulage -> "puce bon"
        | Desapprouve
        | Decu -> "puce faible"
        | _ -> "puce"

    match attitudes m.partie s choix parlent with
    | [] -> Rien
    | lot ->
        div
            "attitudes"
            [ h3 "petit-titre" [ texte (if parlent.IsEmpty then "Les habitants" else "Les autres") ]
              yield!
                  lot
                  |> List.map (fun (h, humeur, a) ->
                      div
                          "attitude"
                          [ avatar h.id (nomHabitant h.id)
                            para "" [ texte (nommer a.texte) ]
                            (if libelleHumeur humeur = "" then Rien else span (ton humeur) [ texte (libelleHumeur humeur) ])
                            pourquoi m.partie s choix h.id ]) ]

let carteReaction (m: Modele) =
    match m.reaction with
    | None -> Rien
    | Some(id, option) ->
        let s = situationDe[id]

        let libelle =
            s.leviers
            |> List.tryPick (fun l -> Tables.optionsDe[l] |> List.tryFind (fun o -> o.id = option))
            |> Option.map _.libelle
            |> Option.defaultValue option

        el
            "article"
            [ "class", "carte reaction" ]
            [ h2 "" [ texte $"Après ta décision : {libelle}" ]
              panneauScene m s Apres (Some option)
              panneauAttitudes m s option
              div
                  "actions"
                  [ bouton "secondaire" (fun () -> envoyer (RevenirSur id)) [ texte "Revenir sur ma décision" ]
                    bouton "principal" (fun () -> envoyer FermerReaction) [ texte "Continuer" ] ] ]

/// Le libellé d'un choix : l'option d'un levier, ou le pari.
let libelleChoix (choix: Choix list) =
    choix
    |> List.map (fun c ->
        if c.levier = "paris" then
            Modele.paris |> List.tryFind (fun b -> b.id = c.option) |> Option.map _.titre |> Option.defaultValue "aucun pari"
        else
            Tables.optionsDe[c.levier] |> List.tryFind (fun o -> o.id = c.option) |> Option.map _.libelle |> Option.defaultValue c.option)
    |> String.concat ", "

/// Le revirement raconté : ce qu'il coûte en confiance, et les gestes de ceux qu'il touche.
let carteRevirement (m: Modele) =
    match m.revirement with
    | None -> Rien
    | Some(id, choix, tous) ->
        let s = situationDe[id]
        let effets = tous |> List.filter (fun (_, d) -> d <> 0.0)

        let geste h humeur =
            Jeu.attitudes |> List.tryFind (fun a -> a.habitant = h && a.humeur = humeur) |> Option.map _.texte

        el
            "article"
            [ "class", "carte reaction" ]
            [ h2 "" [ texte $"Tu reviens sur ta décision : {libelleChoix choix}" ]
              (if effets.IsEmpty then
                   para "discret" [ texte "Personne ne s’était exprimé sur cette décision." ]
               else
                   div
                       "attitudes"
                       [ for (h, d) in effets do
                             let n = nomHabitant h

                             div
                                 "attitude"
                                 [ avatar h n
                                   para "" [ texte (geste h (if d < 0.0 then Decu else Soulage) |> Option.map nommer |> Option.defaultValue n) ]
                                   span (if d > 0.0 then "puce bon" else "puce faible") [ texte $"{n} {signe d}" ] ] ])
              labo "Revenir sur une décision coûte de la confiance : ceux qu’elle réjouissait sont déçus, ceux qu’elle inquiétait restent méfiants."
              div "actions" [ bouton "principal" (fun () -> envoyer FermerRevirement) [ texte "Continuer" ] ] ]

/// Les histoires des habitants, recalculées seulement quand le journal change.
let mutable private histoiresEnCache: string * Map<string, Journal.Moment list> = "", Map.empty

let histoiresDe (p: Partie) =
    let cle = Journal.encoder p

    if fst histoiresEnCache <> cle then
        histoiresEnCache <- cle, Journal.histoires p

    snd histoiresEnCache

/// Un moment de l'histoire d'un habitant, en une ligne.
let ligneMoment (p: Partie) habitant (mo: Journal.Moment) =
    let s = situationDe[mo.situation]
    let delta = if mo.delta = 0.0 then "" else $" Confiance {signe mo.delta}."

    let quoi =
        match mo.action with
        | Action.Decider(_, c) -> raisonHumeur p s c habitant
        | Action.Repondre _ -> "Tu lui as répondu dans la conversation."
        | Action.RevenirSur _ -> "Tu es revenu sur cette décision."
        | _ -> ""

    el
        "li"
        []
        [ span "discret" [ texte $"Révolution {mo.revolution} · {s.titre}" ]
          para "" [ texte (quoi + delta) ] ]

/// Le rôle d'un habitant : ce qu'il fait, comment il y est arrivé, et ses rôles d'avant.
let ficheRole (p: Partie) habitant =
    match roleDe p habitant with
    | None -> Rien
    | Some r ->
        let avant = rolesDe p habitant |> List.filter (fun x -> x.role <> r.role)

        div
            "fiche-role"
            [ para "" [ el "strong" [] [ texte (r.titre + " : ") ]; texte r.description ]
              para "discret" [ texte r.acces ]
              (if avant.IsEmpty then
                   Rien
               else
                   para "discret" [ texte ("Avant : " + String.concat " ; " (avant |> List.map (fun x -> $"{x.titre} (révolution {x.depuis})"))) ]) ]

/// Ce qui compte pour un habitant aujourd'hui, et ce que la partie y a changé.
let ceQuiCompte (p: Partie) habitant =
    el
        "ul"
        [ "class", "ce-qui-compte" ]
        [ for a in Jeu.attachements |> List.filter (fun a -> a.habitant = habitant) do
              let s = sensibilite p habitant a.jauge
              let facteur = (nombre (1.0 + s)).Replace(",0", "")

              el
                  "li"
                  []
                  [ texte a.raison
                    (if s < 0.05 then Rien
                     else span "puce faible" [ texte $"pèse ×{facteur} depuis tes décisions" ]) ] ]

/// La fiche d'un habitant : qui il est, son rôle, son passé, ce qui compte pour lui, son histoire.
let ficheHabitant (m: Modele) (h: Habitant) =
    let n = nomHabitant h.id
    let moments = (histoiresDe m.partie).TryFind h.id |> Option.defaultValue []

    el
        "section"
        [ "class", "fiche-habitant"; "aria-label", $"La fiche de {n}" ]
        [ div
              "fiche-tete"
              [ avatar h.id n
                span "" [ el "strong" [] [ texte n ]; texte $", {ageDe m.partie h.id} ans" ]
                bouton "lien-discret" (fun () -> envoyer (OuvrirHabitant h.id)) [ texte "Fermer" ] ]
          // l'âge change à chaque révolution : on le retire du portrait, il est déjà dans le titre
          para "discret" [ texte (System.Text.RegularExpressions.Regex.Replace(h.portrait, "^[0-9]+ ans[ ;,]*", "")) ]
          ficheRole m.partie h.id
          (match Jeu.antecedents |> List.tryFind (fun a -> a.habitant = h.id) with
           | Some a -> para "discret" [ el "strong" [] [ texte "Son passé : " ]; texte a.texte ]
           | None -> Rien)
          h3 "petit-titre" [ texte "Ce qui compte pour elle ou lui" ]
          ceQuiCompte m.partie h.id
          (if malleabilite (ageDe m.partie h.id) > 1.0 then
               para "discret" [ texte $"À {ageDe m.partie h.id} ans, ce qu'on vit marque davantage (les « années impressionnables »)." ]
           else
               Rien)
          h3 "petit-titre" [ texte "Ce qu’elle ou il a pensé de tes décisions" ]
          (if moments.IsEmpty then
               para "discret" [ texte "Rien encore : aucune de tes décisions ne l’a touché pour l’instant." ]
           else
               el "ol" [ "class", "histoire" ] (moments |> List.rev |> List.map (ligneMoment m.partie h.id))) ]

/// La cellule, sous le paysage : un portrait par habitant (prénom, rôle, confiance) ; un clic
/// ouvre sa fiche juste en dessous.
let bandeCellule (m: Modele) =
    div
        "bande-cellule"
        [ div
              "portraits"
              [ for h in Jeu.habitants do
                    let n = nomHabitant h.id
                    let c = m.partie.reputation.TryFind h.id |> Option.defaultValue 0.0
                    let ouvert = m.habitantOuvert = Some h.id

                    boutonAvec
                        [ "class", (if ouvert then "portrait ouvert" else "portrait"); "aria-expanded", (if ouvert then "true" else "false") ]
                        (fun () -> envoyer (OuvrirHabitant h.id))
                        [ avatar h.id n
                          span
                              "nom-role"
                              [ el "strong" [] [ texte n ]
                                (match roleDe m.partie h.id with
                                 | Some r -> span "discret role-court" [ texte r.titre ]
                                 | None -> Rien) ]
                          span (if c > 0.0 then "puce bon" elif c < 0.0 then "puce faible" else "puce") [ texte (signe c) ] ] ]
          (match m.habitantOuvert |> Option.bind (fun id -> Jeu.habitants |> List.tryFind (fun h -> h.id = id)) with
           | Some h -> ficheHabitant m h
           | None -> Rien) ]

/// Les décisions prises, avec la possibilité de revenir sur chacune.
let panneauDecisions (m: Modele) =
    match decisionsPrises m.partie with
    | [] -> Rien
    | lot ->
        div
            "groupe decisions"
            [ h3 "petit-titre" [ texte "Tes décisions" ]
              yield!
                  lot
                  |> List.map (fun (r, id, choix) ->
                      let s = situationDe[id]

                      div
                          "decision-prise"
                          [ div "" [ div "decision-titre" [ texte s.titre ]; div "discret" [ texte $"{libelleChoix choix} · révolution {r}" ] ]
                            (if estOuverte m.partie id then
                                 span "puce" [ texte "à décider" ]
                             else
                                 bouton "lien-discret" (fun () -> envoyer (RevenirSur id)) [ texte "Revenir dessus" ]) ]) ]

let carteFinRevolution (p: Partie) =
    el
        "article"
        [ "class", "carte fin-revolution" ]
        [ h2 "" [ texte "Les décisions de cette révolution sont prises" ]
          para "" [ texte "Une année passe : les décisions s’installent, des paris mûrissent, la vie suit son cours." ]
          div "actions" [ bouton "principal" (fun () -> envoyer RevolutionSuivante) [ texte "Révolution suivante" ] ] ]

let nouvelles (m: Modele) =
    let n = nouvellesDe m.partie m.partie.revolution

    if n.IsEmpty || m.nouvellesLues >= m.partie.revolution then
        Rien
    else
        div
            "voile"
            [ el
                  "div"
                  [ "class", "carte nouvelles"; "role", "dialog"; "aria-label", "Ce qui s’est passé" ]
                  [ h2 "" [ texte "Ce qui s’est passé" ]
                    yield!
                        n
                        |> List.map (function
                            | Survenu(_, id) ->
                                let ev = Jeu.evenements |> List.find (fun x -> x.id = id)

                                let effets =
                                    ev.effets
                                    |> List.map (fun f -> $"{libelleJauge f.jauge} {signe f.points}" + (if f.duree > 1 then $" pendant {f.duree} révolutions" else ""))

                                div
                                    "nouvelle"
                                    [ h3 "" [ texte ev.titre ]
                                      para "" (riche ev.texte)
                                      (if effets.IsEmpty then Rien else para "discret" [ texte (String.concat " · " effets) ])
                                      (if ev.provenance = "assistant" then labo "Cet événement est une proposition : il attend d’être relu." else Rien) ]
                            | Muri(_, b) ->
                                let pari = Modele.paris |> List.find (fun x -> x.id = b)
                                div "nouvelle" [ h3 "" [ texte $"Ton pari arrive à maturité : {pari.titre}" ]; para "" (riche pari.effet) ]
                            | _ -> Rien)
                    div "actions" [ bouton "principal" (fun () -> envoyer LireNouvelles) [ texte "Continuer" ] ] ] ]

let alerte (p: Partie) =
    nouvellesDe p p.revolution
    |> List.tryPick (function
        | Survenu(_, id) -> Some (Jeu.evenements |> List.find (fun x -> x.id = id)).titre
        | _ -> None)

let vueJeu (m: Modele) =
    let p = m.partie
    let courante = decisionCourante p

    let apresEnvisage =
        match m.envisage, courante with
        | Some(id, c), Some s when id = s.id && not c.IsEmpty ->
            match apercu p id c with
            | Ok a -> Some a.aTerme
            | Error _ -> None
        | _ -> None

    div
        "jeu"
        [ entete m p
          div
              "plateau"
              [ el "main" [ "class", "scene" ]
                    [ el "canvas" [ "id", "territoire"; "class", "territoire"; "aria-label", "Le territoire, de la montagne à la mer" ] []
                      bandeCellule m
                      (match m.erreur with Some e -> para "erreur" [ texte e ] | None -> Rien)
                      carteReaction m
                      carteRevirement m
                      yield! ouvertes p |> List.filter (fun s -> s.genre = Constat) |> List.map carteConstat
                      (match courante with
                       | Some s -> carteDecision m s
                       | None -> carteFinRevolution p) ]
                panneauJauges p apresEnvisage (panneauDecisions m) ]
          nouvelles m ]

/// Copie le code de la partie (le presse-papiers peut être refusé : le code reste affiché).
let copierCode (p: Partie) =
    let code = Journal.encoder p

    try
        Fable.Core.JsInterop.emitJsStatement code "navigator.clipboard.writeText($0).catch(() => {})"
    with _ ->
        ()

/// Le code de la partie, à copier ou à recopier.
let panneauCode (p: Partie) =
    div
        "code-partie"
        [ h3 "petit-titre" [ texte "Le code de ta partie" ]
          para "discret" [ texte "Il contient tes choix : le recopier plus tard, ou le donner à quelqu’un, rejoue exactement la même partie." ]
          el "textarea" [ "id", "code-partie"; "rows", "2"; "readonly", ""; "aria-label", "Le code de la partie" ] [ texte (Journal.encoder p) ]
          div "actions" [ bouton "secondaire" (fun () -> copierCode p) [ texte "Copier le code" ] ] ]

/// Un petit nombre écrit en lettres, avec une majuscule (« Six »).
let enLettres n =
    match n with
    | 1 -> "Une" | 2 -> "Deux" | 3 -> "Trois" | 4 -> "Quatre" | 5 -> "Cinq" | 6 -> "Six"
    | 7 -> "Sept" | 8 -> "Huit" | 9 -> "Neuf" | 10 -> "Dix"
    | n -> string n

let vueAccueil (m: Modele) =
    let nomRegion id =
        if id = "france" then "Sans région (la France entière)"
        else (F.regions |> List.find (fun r -> r.id = id)).nom

    div
        "accueil"
        [ h1 [ texte "Le Labo de la Bande" ]
          para "accroche" [ texte "Une société sans prison, sans police et autant que possible sans monnaie, révolution solaire après révolution. Chaque choix a un prix, et chaque chiffre dit d’où il vient." ]
          (match m.sauvegarde with
           | Some p ->
               div
                   "reprise"
                   [ para "" [ texte $"Une partie t’attend : révolution {p.revolution}, {nomRegion p.region}." ]
                     div "actions" [ bouton "principal" (fun () -> envoyer Reprendre) [ texte "Reprendre ma partie" ] ] ]
           | None -> Rien)
          h2 "" [ texte "Une nouvelle partie" ]
          div
              "lieu"
              [ el "label" [ "for", "region" ] [ texte "Où se trouve ta cellule ?" ]
                Element(
                    "select",
                    [ "id", "region" ],
                    [ "change", (fun e -> envoyer (ChoisirRegion(Fable.Core.JsInterop.(?) e.target "value"))) ],
                    [ for id in "france" :: (F.regions |> List.map _.id) do
                          el "option" ((if id = m.region then [ "selected", "" ] else []) @ [ "value", id ]) [ texte (nomRegion id) ] ]
                ) ]
          para "discret" [ texte "La région change les prénoms de ta cellule : ils sont tirés parmi ceux qu’on donne vraiment là-bas, l’année de naissance de chacun (Insee)." ]
          div
              "lieu"
              [ el "label" [ "for", "depart" ] [ texte "Par où commencer ?" ]
                Element(
                    "select",
                    [ "id", "depart" ],
                    [ "change", (fun e -> envoyer (ChoisirDepart(Fable.Core.JsInterop.(?) e.target "value"))) ],
                    [ for t in Jeu.tranches |> List.sortBy _.rang do
                          el "option" ((if t.id = m.depart then [ "selected", "" ] else []) @ [ "value", t.id ]) [ texte (t.embleme + " " + t.titre) ] ]
                ) ]
          para "discret" [ texte "La partie commence à cette étape et continue avec les suivantes. Commencer au début, c’est le parcours complet." ]
          para "accroche" [ texte $"Tu rejoins une cellule d’une cinquantaine de personnes. {enLettres Jeu.habitants.Length} d’entre elles t’accompagnent toute la partie ; ce que tu décides, elles le vivent, et elles s’en souviennent. Leurs prénoms changent à chaque partie." ]
          labo "Je suis Labo. Je propose, tu vérifies."
          div "actions" [ bouton "principal" (fun () -> envoyer Commencer) [ texte "Commencer une partie" ] ]
          listeIdeesIntegrees ()
          para "discret" [ texte "Une idée en cours de partie ? « J’ai une idée », dans chaque conversation : elle part au projet par un ticket, et sa réponse arrive ici." ]
          el
              "details"
              [ "class", "charger" ]
              [ el "summary" [] [ texte "Charger une partie avec son code" ]
                el "textarea" [ "id", "code-a-charger"; "rows", "2"; "aria-label", "Le code d’une partie" ] []
                div
                    "actions"
                    [ bouton
                          "secondaire"
                          (fun () ->
                              let zone = document.getElementById "code-a-charger"
                              envoyer (ChargerCode(Fable.Core.JsInterop.(?) zone "value")))
                          [ texte "Charger" ] ]
                (match m.note with Some n -> para "erreur" [ texte n ] | None -> Rien) ] ]

let vueFin (m: Modele) =
    let p = m.partie
    let r = Moteur.evaluer p.etat

    div
        "accueil fin"
        [ h1 [ texte $"Après {p.revolution} révolutions" ]
          div
              "questions"
              [ div "question" [ h3 "" [ texte "Viable ?" ]; para "grand-chiffre" [ texte (if r.viable.ok then "Oui" else "Non") ]; para "discret" [ texte (if r.viable.ok then "Aucune jauge vitale sous son seuil." else $"{libelleJauge (fst r.viable.maillon)} sous son seuil vital.") ] ]
                div "question" [ h3 "" [ texte "Fidèle au but ?" ]; para "grand-chiffre" [ texte (nombre r.fidelite) ]; para "discret" [ texte $"La France : {nombre r.fideliteFrance}" ] ]
                div "question" [ h3 "" [ texte "Mieux que la France ?" ]; para "grand-chiffre" [ texte (nombre r.score) ]; para "discret" [ texte $"La France : {nombre r.scoreFrance}" ] ] ]
          h3 "petit-titre" [ texte "La confiance des habitants" ]
          div "confiances" [ for (h, d) in p.reputation |> Map.toList do span (if d >= 0.0 then "puce bon" else "puce faible") [ texte $"{nomHabitant h} {signe d}" ] ]
          panneauCode p
          div "actions" [ bouton "principal" (fun () -> envoyer Recommencer) [ texte "Rejouer" ] ] ]

let vue (m: Modele) =
    match m.ecran with
    | Accueil -> vueAccueil m
    | EnJeu -> vueJeu m
    | Fin -> vueFin m

// ---------- La boucle ----------

/// `#jouer` ouvre directement une partie ; `#apercu` montre aussi le conseil sur la première
/// option de la première décision (pour vérifier l'écran sans cliquer).
let depart () =
    let m = initial ()

    match window.location.hash with
    | "#jouer" -> { m with ecran = EnJeu }
    | h when h.StartsWith "#partie=" ->
        let code = h.Substring 8
        let code = match code.IndexOf "&fiche=" with -1 -> code | i -> code.Substring(0, i)

        match Journal.decoder (System.Uri.UnescapeDataString code) with
        | Ok p -> surPartie p m
        | Error e -> { m with note = Some(Journal.decrireErreur e) }
    | h when h.StartsWith "#depart-" ->
        let t = h.Substring 8
        { m with ecran = EnJeu; depart = t; partie = nouvellePartie m.region t }
    | "#apprentissage" -> { m with ecran = EnJeu; apprentissage = true }
    | "#reaction" ->
        match Journal.jouer (Action.Decider("vivre.monnaie", [ { levier = "monnaie"; option = "sans" } ])) m.partie with
        | Ok p -> { m with ecran = EnJeu; partie = p; reaction = Some("vivre.monnaie", "sans") }
        | Error _ -> m
    | "#revirement" ->
        let choix = [ { levier = "monnaie"; option = "sans" } ]

        match Journal.jouer (Action.Decider("vivre.monnaie", choix)) m.partie with
        | Ok p1 ->
            let effets = effetRevirement p1 situationDe["vivre.monnaie"] choix

            match Journal.jouer (Action.RevenirSur "vivre.monnaie") p1 with
            | Ok p -> { m with ecran = EnJeu; partie = p; revirement = Some("vivre.monnaie", choix, effets) }
            | Error _ -> m
        | Error _ -> m
    | "#apercu" ->
        let m = { m with ecran = EnJeu }

        match decisionCourante m.partie with
        | Some s when s.genre = Dilemme ->
            let l = List.exactlyOne s.leviers
            { m with envisage = Some(s.id, [ { levier = l; option = (Tables.optionsDe[l] |> List.item 1).id } ]) }
        | _ -> m
    | _ -> m

/// `…&fiche=<habitant>` à la fin de l'adresse ouvre la fiche d'un habitant (pour vérifier l'écran sans cliquer).
let ficheDemandee (m: Modele) =
    let h = window.location.hash

    match h.IndexOf "&fiche=" with
    | -1 -> m
    | i -> { m with habitantOuvert = Some(h.Substring(i + 7)) }

let mutable modele = depart () |> ficheDemandee
let racine: HTMLElement = unbox (document.getElementById "jeu")

let dessinerTerritoire () =
    match document.getElementById "territoire" with
    | null -> ()
    | c ->
        let p = modele.partie
        Territoire.dessiner (c :?> HTMLCanvasElement) (Moteur.evaluer p.etat) p.revolution (Moteur.ouverture p.etat) (alerte p)

let rendre () =
    Textes.prenoms <- modele.partie.prenoms
    monter racine (vue modele)
    dessinerTerritoire ()

envoyer <-
    fun msg ->
        modele <- miseAJour msg modele

        // la partie en cours se sauvegarde dans le navigateur après chaque geste
        if modele.ecran <> Accueil then
            ecrireSauvegarde modele.partie

        rendre ()

window.addEventListener ("resize", fun _ -> dessinerTerritoire ())
rendre ()
