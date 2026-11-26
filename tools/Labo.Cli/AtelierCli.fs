/// `labo atelier` : l'atelier des données, où des collaborateurs invités modifient des tables sans
/// compte GitHub, par le relais, dans le dépôt séparé `labo-de-la-bande/atelier` (docs/atelier.md).
///
///   labo atelier inviter <id> <nom…>   une invitation : le code s'affiche une seule fois
///   labo atelier invitations           les invités
///   labo atelier retirer <id>          retire une invitation (l'accès est coupé en une minute au plus)
///   labo atelier cle <fichier.pem>     pose la clé privée de l'application GitHub sur le relais
///   labo atelier fusionner             verse dans data/ ce que l'atelier a changé depuis le dernier envoi
///   labo atelier envoyer               met les tables de data/ dans l'atelier (après fusion)
///
/// Les invitations passent par le relais (jeton gh d'un collaborateur du dépôt) ; la fusion et
/// l'envoi parlent au dépôt de l'atelier avec `gh`. Rien d'autre que les tables listées dans
/// src/Labo.Relais/Atelier.fs ne vient de l'atelier.
module Labo.Cli.AtelierCli

open System
open System.IO
open System.Net.Http
open System.Security.Cryptography
open System.Text.Json
open System.Text.Json.Nodes
open CliWrap
open CliWrap.Buffered
open Labo.Relais.Atelier

let depotAtelier = "labo-de-la-bande/atelier"
let page = "https://labo-de-la-bande.github.io/jeu/atelier.html"

/// La copie du dernier envoi, dans l'atelier : la base de la fusion à trois.
let cheminBase (chemin: string) = ".base/" + chemin

// ---------- Le dépôt de l'atelier, par gh ----------

let gh (args: string list) (entree: string option) =
    let c = Cli.Wrap("gh").WithArguments(args).WithValidation(CommandResultValidation.None)

    let c =
        match entree with
        | Some e -> c.WithStandardInputPipe(PipeSource.FromString e)
        | None -> c

    let r = c.ExecuteBufferedAsync().Task.Result
    r.ExitCode, r.StandardOutput, r.StandardError

/// Le texte d'un fichier de l'atelier et son empreinte git, ou rien s'il n'existe pas.
let lireDistant (chemin: string) : Result<(string * string) option, string> =
    let code, sortie, erreur = gh [ "api"; $"repos/{depotAtelier}/contents/{chemin}?ref=main" ] None

    if code = 0 then
        use doc = JsonDocument.Parse sortie
        let b64 = doc.RootElement.GetProperty("content").GetString().Replace("\n", "")
        Ok(Some(Text.Encoding.UTF8.GetString(Convert.FromBase64String b64), doc.RootElement.GetProperty("sha").GetString()))
    elif sortie.Contains "\"Not Found\"" || erreur.Contains "404" then
        Ok None
    else
        Error $"gh : {erreur.Trim()}"

let ecrireDistant (chemin: string) (texte: string) (sha: string option) (message: string) =
    let corps = JsonObject()
    corps["message"] <- JsonValue.Create message
    corps["content"] <- JsonValue.Create(Convert.ToBase64String(Text.Encoding.UTF8.GetBytes texte))
    corps["branch"] <- JsonValue.Create "main"

    match sha with
    | Some s -> corps["sha"] <- JsonValue.Create s
    | None -> ()

    let code, _, erreur = gh [ "api"; "-X"; "PUT"; $"repos/{depotAtelier}/contents/{chemin}"; "--input"; "-" ] (Some(corps.ToJsonString()))
    if code = 0 then Ok() else Error $"gh : {erreur.Trim()}"

// ---------- La fusion à trois ----------

type Fusion =
    { texte: string
      changements: string list
      conflits: string list }

/// Verse dans `local` ce que `atelier` a changé depuis `base`, ligne par ligne et champ par champ.
/// Un champ changé des deux côtés, différemment, reste local et devient un conflit à régler à la main.
let fusionner3 (t: Table) (base': string) (local: string) (atelier: string) : Result<Fusion, string> =
    let lireTable nom texte =
        match lire texte with
        | Error e -> Error $"{t.chemin} ({nom}) : {e}"
        | Ok [] -> Error $"{t.chemin} ({nom}) : vide"
        | Ok(e :: _) when e.champs <> t.entete -> Error $"{t.chemin} ({nom}) : l'en-tête n'est pas celui attendu"
        | Ok(e :: lignes) -> Ok(e, lignes)

    match lireTable "base" base', lireTable "local" local, lireTable "atelier" atelier with
    | Error e, _, _
    | _, Error e, _
    | _, _, Error e -> Error e
    | Ok(_, b), Ok(entete, o), Ok(_, a) ->
        let ic = t.entete |> List.findIndex ((=) t.cle)
        let ip = t.entete |> List.findIndex ((=) t.provenance)
        let cle (l: Ligne) = l.champs[ic]
        let parCle (ls: Ligne list) = ls |> List.map (fun l -> cle l, l) |> Map.ofList
        let mb, mo, ma = parCle b, parCle o, parCle a
        let changements = ResizeArray<string>()
        let conflits = ResizeArray<string>()
        let par (l: Ligne) = l.champs[ip]

        let gardees =
            o
            |> List.choose (fun lo ->
                let k = cle lo

                match mb.TryFind k, ma.TryFind k with
                | Some lb, Some la when la.champs = lb.champs -> Some lo
                | Some lb, Some la when lo.champs = lb.champs ->
                    changements.Add $"{k} : modifiée par {par la}"
                    Some la
                | Some _, Some la when lo.champs = la.champs -> Some lo
                | Some lb, Some la ->
                    // champ par champ ; la provenance suit les champs repris de l'atelier
                    let mutable repris = false

                    let champs =
                        List.init t.entete.Length (fun i ->
                            let vb, vo, va = lb.champs[i], lo.champs[i], la.champs[i]

                            if i = ip then vo
                            elif va = vb || vo = va then vo
                            elif vo = vb then
                                repris <- true
                                va
                            else
                                conflits.Add $"{k} : {t.entete[i]} changé ici (« {vo} ») et dans l'atelier par {par la} (« {va} ») ; la version d'ici est gardée"
                                vo)

                    let champs = if repris then champs |> List.mapi (fun i v -> if i = ip then par la else v) else champs
                    if repris then changements.Add $"{k} : modifiée en partie par {par la}"
                    Some(if champs = lo.champs then lo else nouvelleLigne champs)
                | Some lb, None when lo.champs = lb.champs ->
                    changements.Add $"{k} : retirée dans l'atelier"
                    None
                | Some _, None ->
                    conflits.Add $"{k} : retirée dans l'atelier mais changée ici ; gardée"
                    Some lo
                | None, Some la when la.champs <> lo.champs ->
                    conflits.Add $"{k} : ajoutée ici et dans l'atelier, différemment ; la version d'ici est gardée"
                    Some lo
                | _ -> Some lo)

        let ajoutees =
            a
            |> List.filter (fun la -> not (mo.ContainsKey(cle la)))
            |> List.choose (fun la ->
                let k = cle la

                match mb.TryFind k with
                | None ->
                    changements.Add $"{k} : ajoutée par {par la}"
                    Some la
                | Some lb when lb.champs = la.champs -> None
                | Some _ ->
                    conflits.Add $"{k} : retirée ici mais changée dans l'atelier par {par la} ; reste retirée"
                    None)

        Ok
            { texte = ecrire (entete :: gardees @ ajoutees)
              changements = List.ofSeq changements
              conflits = List.ofSeq conflits }

// ---------- Les commandes ----------

let fichierLocal (t: Table) = Repo.path (t.chemin.Split '/' |> List.ofArray)

let lireLocal (t: Table) = File.ReadAllText(fichierLocal t).Replace("\r\n", "\n")

/// Pour chaque table : la base, la version de l'atelier, et la fusion avec data/.
let fusions () =
    tables
    |> List.map (fun t ->
        match lireDistant t.chemin, lireDistant (cheminBase t.chemin) with
        | Error e, _
        | _, Error e -> t, Error e
        | Ok None, _
        | _, Ok None -> t, Error "pas encore dans l'atelier : labo atelier envoyer"
        | Ok(Some(a, _)), Ok(Some(b, _)) -> t, fusionner3 t b (lireLocal t) a)

let fusionner (generer: unit -> int) =
    let resultats = fusions ()
    let mutable ok = true
    let mutable change = false

    for (t, r) in resultats do
        match r with
        | Error e ->
            eprintfn "✗ %s : %s" t.chemin e
            ok <- false
        | Ok f ->
            for c in f.changements do printfn "  %s · %s" t.chemin c
            for c in f.conflits do eprintfn "  ! %s · %s" t.chemin c

            if f.texte <> lireLocal t then
                File.WriteAllText(fichierLocal t, f.texte)
                change <- true

    if not ok then
        1
    elif not change then
        printfn "Rien de nouveau dans l'atelier."
        0
    else
        printfn "\nÉcrit dans data/. Puis : relire (git diff data/), commiter, et labo atelier envoyer."
        generer ()

let envoyer () =
    let readme =
        "# L'atelier des données du Labo de la Bande\n\nCe dépôt n'est qu'une salle d'attente. Des collaborateurs invités y modifient des tables du jeu depuis la page de l'atelier (https://labo-de-la-bande.github.io/jeu/atelier.html), par le relais du projet ; l'auteur relit et verse ces modifications dans le dépôt du jeu (`labo atelier fusionner`). Chaque commit dit qui a changé quoi, et pourquoi.\n\n`.base/` garde la version du dernier envoi : la base de la fusion.\n"

    let etapes =
        tables
        |> List.map (fun t ->
            let local = lireLocal t

            match lireDistant t.chemin, lireDistant (cheminBase t.chemin) with
            | Error e, _
            | _, Error e -> Error $"{t.chemin} : {e}"
            | Ok distant, Ok base' ->
                // l'atelier a-t-il des changements que data/ n'a pas encore ?
                let enAttente =
                    match distant, base' with
                    | Some(a, _), Some(b, _) ->
                        match fusionner3 t b local a with
                        | Ok f -> f.texte <> local
                        | Error _ -> true
                    | Some(a, _), None -> a <> local
                    | None, _ -> false

                if enAttente then
                    Error $"{t.chemin} : l'atelier a des changements pas encore versés : labo atelier fusionner, puis commiter"
                else
                    Ok(t, local, distant |> Option.map snd, base' |> Option.map snd, distant |> Option.map fst, base' |> Option.map fst))

    match etapes |> List.tryPick (function Error e -> Some e | Ok _ -> None) with
    | Some e ->
        eprintfn "✗ %s" e
        1
    | None ->
        let commit = match Moderation.gh [ "api"; "user"; "--jq"; ".login" ] with Ok l -> l | Error _ -> "?"
        let mutable echec = None

        let ecrire chemin texte sha message =
            if echec.IsNone then
                match ecrireDistant chemin texte sha message with
                | Ok() -> printfn "  écrit %s" chemin
                | Error e -> echec <- Some $"{chemin} : {e}"

        match lireDistant "README.md" with
        | Ok None -> ecrire "README.md" readme None "L'atelier des données"
        | _ -> ()

        for e in etapes do
            match e with
            | Ok(t, local, sha, shaBase, actuel, actuelBase) ->
                if actuel <> Some local then ecrire t.chemin local sha $"Envoi depuis le dépôt du jeu ({commit})"
                if actuelBase <> Some local then ecrire (cheminBase t.chemin) local shaBase $"Base de la fusion ({commit})"
            | Error _ -> ()

        match echec with
        | Some e ->
            eprintfn "✗ %s" e
            1
        | None ->
            printfn "L'atelier a les tables de data/."
            0

let inviter (id: string) (nom: string) =
    let corps = JsonObject()
    corps["id"] <- JsonValue.Create id
    corps["nom"] <- JsonValue.Create nom

    match Moderation.appeler HttpMethod.Post "/moderation/atelier/invitations" (Some(corps.ToJsonString())) with
    | Error e ->
        eprintfn "✗ %s" e
        1
    | Ok texte ->
        use doc = JsonDocument.Parse texte
        let code = doc.RootElement.GetProperty("code").GetString()
        printfn "Invitation pour %s (%s). Le code ne s'affichera plus : transmets-le à cette personne seule, par un canal privé.\n" nom id
        printfn "  %s\n" code
        printfn "La page de l'atelier : %s" page
        0

let invitations () =
    match Moderation.appeler HttpMethod.Get "/moderation/atelier/invitations" None with
    | Error e ->
        eprintfn "✗ %s" e
        1
    | Ok texte ->
        use doc = JsonDocument.Parse texte
        let liste = doc.RootElement.EnumerateArray() |> List.ofSeq

        if liste.IsEmpty then printfn "Aucune invitation."

        for e in liste do
            printfn "  %-20s %s" (e.GetProperty("id").GetString()) (e.GetProperty("nom").GetString())

        0

let retirer (id: string) =
    match Moderation.appeler HttpMethod.Post $"/moderation/atelier/invitations/{id}/retirer" (Some "{}") with
    | Error e ->
        eprintfn "✗ %s" e
        1
    | Ok _ ->
        printfn "Invitation de %s retirée : son code ne marchera plus d'ici une minute (propagation chez Cloudflare)." id
        0

/// La clé privée que GitHub donne (PEM, PKCS#1 ou PKCS#8), convertie en PKCS#8 base64 pour Web Crypto,
/// et posée comme secret du relais ; jamais affichée.
let cle (fichier: string) =
    use rsa = RSA.Create()
    rsa.ImportFromPem(File.ReadAllText fichier)
    let pkcs8 = Convert.ToBase64String(rsa.ExportPkcs8PrivateKey())
    RelaisDeployer.poserSecretValeur "ATELIER_CLE" pkcs8
    printfn "Efface maintenant %s : la clé n'est plus utile ailleurs (GitHub peut en générer une autre)." fichier
    0

let usage =
    """labo atelier inviter <id> <nom…> | invitations | retirer <id> | cle <fichier.pem> | fusionner | envoyer
  (docs/atelier.md)
"""

let executer (generer: unit -> int) (args: string list) =
    try
        match args with
        | "inviter" :: id :: (_ :: _ as nom) -> inviter id (String.concat " " nom)
        | [ "invitations" ] -> invitations ()
        | [ "retirer"; id ] -> retirer id
        | [ "cle"; fichier ] -> cle fichier
        | [ "fusionner" ] -> fusionner generer
        | [ "envoyer" ] -> envoyer ()
        | _ ->
            eprintf "%s" usage
            2
    with e ->
        eprintfn "%s" e.Message
        1
