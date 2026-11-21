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
      erreur: string option }

type Message =
    | Commencer
    | Envisager of situation: string * choix: Choix list
    | Decider
    | RevolutionSuivante
    | LireNouvelles
    | Recommencer

let graineDuJour () = int64 (System.DateTime.Now.Ticks % 2147483647L)

let nouvellePartie () =
    match nouvelleAvec (graineDuJour ()) true parcoursComplet with
    | Ok p -> p
    | Error e -> failwith (decrire e)

let initial () =
    { ecran = Accueil
      partie = nouvellePartie ()
      envisage = None
      nouvellesLues = 0
      erreur = None }

/// La première décision qui attend pendant cette révolution.
let decisionCourante (p: Partie) = ouvertes p |> List.tryFind estDecision

/// Les choix par défaut proposés quand une situation s'ouvre.
let choixParDefaut (s: Situation) =
    match s.genre with
    | Paris -> [ { levier = "paris"; option = "aucun" } ]
    | Prevention -> s.leviers |> List.map (fun l -> { levier = l; option = "non" })
    | _ -> []

let miseAJour msg (m: Modele) =
    match msg with
    | Commencer -> { m with ecran = EnJeu; nouvellesLues = 0; erreur = None }
    | Recommencer -> { initial () with ecran = EnJeu }
    | Envisager(s, c) -> { m with envisage = Some(s, c); erreur = None }
    | LireNouvelles -> { m with nouvellesLues = m.partie.revolution }
    | Decider ->
        match m.envisage with
        | Some(s, c) when not c.IsEmpty ->
            match decider m.partie s c with
            | Ok p -> { m with partie = p; envisage = None; erreur = None }
            | Error e -> { m with erreur = Some(decrire e) }
        | _ -> m
    | RevolutionSuivante ->
        match finirRevolution m.partie with
        | Ok p when terminee p -> { m with partie = p; ecran = Fin; envisage = None }
        | Ok p -> { m with partie = p; envisage = None }
        | Error e -> { m with erreur = Some(decrire e) }

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

    div
        "jauge"
        [ div "jauge-tete" [ span "jauge-nom" [ texte (libelleJauge j.id) ]; ecart; span "jauge-valeur" [ texte (nombre (v * 100.0)) ] ]
          el
              "div"
              [ "class", "barre"; "title", $"France : {nombre (france * 100.0)}" ]
              [ el "div" [ "class", "plein " + ton v; "style", style "width" (pct v) ] []
                (match apres with
                 | Some a -> el "div" [ "class", "fantome"; "style", style "width" (pct a) ] []
                 | None -> Rien)
                el "div" [ "class", "repere"; "style", style "left" (pct france) ] [] ] ]

let panneauJauges (p: Partie) (apres: Moteur.Resultat option) =
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
          para "legende" [ texte "Le trait marque la France d’aujourd’hui ; la zone claire, l’effet de la décision envisagée." ] ]

let entete (p: Partie) =
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
          div
              "scores"
              [ span "score" [ texte $"Score {nombre r.score}" ]
                span "france" [ texte $"France {nombre r.scoreFrance}" ]
                span (if r.viable.ok then "puce bon" else "puce faible") [ texte (if r.viable.ok then "viable" else "pas viable") ] ] ]

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
               labo "C’est moi qui ai rangé ces arguments par lentille. C’est une proposition : vérifie-la."
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

let carteDecision (m: Modele) (s: Situation) =
    let choix =
        match m.envisage with
        | Some(id, c) when id = s.id -> c
        | _ -> choixParDefaut s

    let apercu =
        if choix.IsEmpty then None
        else match apercu m.partie s.id choix with Ok a -> Some a | Error _ -> None

    let option (cle: string) (libelle: string) (note: string) (choisi: bool) (nouveaux: Choix list) =
        boutonAvec
            [ "class", (if choisi then "option choisie" else "option"); "aria-pressed", (if choisi then "true" else "false") ]
            (fun () -> envoyer (Envisager(s.id, nouveaux)))
            [ span "option-libelle" (riche libelle); (if note = "" then Rien else span "option-note" (riche note)) ]

    let options =
        match s.genre with
        | Dilemme ->
            let levier = List.exactlyOne s.leviers

            Tables.optionsDe[levier]
            |> List.map (fun o ->
                option o.id o.libelle o.note (choix = [ { levier = levier; option = o.id } ]) [ { levier = levier; option = o.id } ])
        | Paris ->
            [ yield!
                  s.paris
                  |> List.map (fun id ->
                      let b = Modele.paris |> List.find (fun x -> x.id = id)
                      let quand = if b.pretDans = 0 then "agit tout de suite" else $"agit dans {b.pretDans} révolutions"
                      option b.id b.titre $"{quand}. {b.effet}" (choix = [ { levier = "paris"; option = id } ]) [ { levier = "paris"; option = id } ])
              option "aucun" "Aucun pari" "" (choix = [ { levier = "paris"; option = "aucun" } ]) [ { levier = "paris"; option = "aucun" } ] ]
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
                                      (if ev.provenance = "assistant" then labo "J’ai écrit cet événement ; il attend d’être relu." else Rien) ]
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
        [ entete p
          div
              "plateau"
              [ el "main" [ "class", "scene" ]
                    [ el "canvas" [ "id", "territoire"; "class", "territoire"; "aria-label", "Le territoire, de la montagne à la mer" ] []
                      (match m.erreur with Some e -> para "erreur" [ texte e ] | None -> Rien)
                      yield! ouvertes p |> List.filter (fun s -> s.genre = Constat) |> List.map carteConstat
                      (match courante with
                       | Some s -> carteDecision m s
                       | None -> carteFinRevolution p) ]
                panneauJauges p apresEnvisage ]
          nouvelles m ]

let vueAccueil () =
    div
        "accueil"
        [ h1 [ texte "Le Labo de la Bande" ]
          para "accroche" [ texte "Une société sans prison, sans police et autant que possible sans monnaie, révolution solaire après révolution. Chaque choix a un prix, et chaque chiffre dit d’où il vient." ]
          labo "Je suis Labo. Je propose, tu vérifies."
          div "actions" [ bouton "principal" (fun () -> envoyer Commencer) [ texte "Commencer une partie" ] ] ]

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
          div "actions" [ bouton "principal" (fun () -> envoyer Recommencer) [ texte "Rejouer" ] ] ]

let vue (m: Modele) =
    match m.ecran with
    | Accueil -> vueAccueil ()
    | EnJeu -> vueJeu m
    | Fin -> vueFin m

// ---------- La boucle ----------

/// `#jouer` ouvre directement une partie ; `#apercu` montre aussi le conseil sur la première
/// option de la première décision (pour vérifier l'écran sans cliquer).
let depart () =
    let m = initial ()

    match window.location.hash with
    | "#jouer" -> { m with ecran = EnJeu }
    | "#apercu" ->
        let m = { m with ecran = EnJeu }

        match decisionCourante m.partie with
        | Some s when s.genre = Dilemme ->
            let l = List.exactlyOne s.leviers
            { m with envisage = Some(s.id, [ { levier = l; option = (Tables.optionsDe[l] |> List.item 1).id } ]) }
        | _ -> m
    | _ -> m

let mutable modele = depart ()
let racine: HTMLElement = unbox (document.getElementById "jeu")

let dessinerTerritoire () =
    match document.getElementById "territoire" with
    | null -> ()
    | c ->
        let p = modele.partie
        Territoire.dessiner (c :?> HTMLCanvasElement) (Moteur.evaluer p.etat) p.revolution (Moteur.ouverture p.etat) (alerte p)

let rendre () =
    monter racine (vue modele)
    dessinerTerritoire ()

envoyer <-
    fun msg ->
        modele <- miseAJour msg modele
        rendre ()

window.addEventListener ("resize", fun _ -> dessinerTerritoire ())
rendre ()
