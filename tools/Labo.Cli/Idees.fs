/// `labo idees` : relire les idées envoyées sans compte GitHub, pour les collaborateurs du dépôt.
///
///   labo idees                          les idées à relire, et celles publiées à suivre
///   labo idees publier <code>           ouvre le ticket public (avec ton compte gh) et prévient le joueur
///   labo idees ecarter <code> "raison"  écarte l'idée ; le joueur voit la raison sur son lien de suivi
///   labo idees suivre                   reporte le sort des tickets publiés (à l'étude, intégrée,
///                                       écartée) sur le suivi des joueurs
///
/// Le code d'une idée suffit à la désigner : ses huit premiers caractères suffisent.
module Labo.Cli.Idees

open System
open System.Net.Http
open System.Text.Json
open Labo.Relais.Proposition

type Idee =
    { code: string
      statut: string
      ticket: int option
      proposition: Proposition option }

let lireIdees (json: string) =
    use doc = JsonDocument.Parse json

    let texte (e: JsonElement) (k: string) =
        match e.TryGetProperty k with
        | true, v when v.ValueKind = JsonValueKind.String -> v.GetString()
        | _ -> ""

    [ for e in doc.RootElement.EnumerateArray() ->
          { code = texte e "code"
            statut = texte e "statut"
            ticket =
              match e.TryGetProperty "ticket" with
              | true, v when v.ValueKind = JsonValueKind.Number -> Some(v.GetInt32())
              | _ -> None
            proposition =
              match e.TryGetProperty "proposition" with
              | true, p when p.ValueKind = JsonValueKind.Object ->
                  Some
                      { situation = texte p "situation"
                        idee = texte p "idee"
                        appuis = texte p "appuis"
                        provenance = texte p "provenance"
                        citation = texte p "citation"
                        mail = None }
              | _ -> None } ]

let lister () =
    Moderation.appeler HttpMethod.Get "/moderation/idees" None |> Result.map lireIdees

/// L'idée désignée par son code, ou par le début de son code.
let trouver (debut: string) =
    lister ()
    |> Result.bind (fun l ->
        match l |> List.filter (fun i -> i.code.StartsWith debut) with
        | [ i ] -> Ok i
        | [] -> Error $"aucune idée en cours avec le code {debut}"
        | _ -> Error $"plusieurs idées commencent par {debut} : donne plus de caractères")

let afficher () =
    match lister () with
    | Error e ->
        eprintfn "%s" e
        1
    | Ok l ->
        let aRelire = l |> List.filter (fun i -> i.statut = "a-relire")
        let suivies = l |> List.filter (fun i -> i.statut <> "a-relire")
        printfn "%d idée(s) à relire.\n" aRelire.Length

        for i in aRelire do
            match i.proposition with
            | Some p ->
                printfn "── %s ──" (i.code.Substring(0, 8))
                printfn "Situation : %s" (if p.situation = "" then "(non précisée)" else p.situation)
                printfn "Idée : %s" p.idee
                if p.appuis <> "" then printfn "Appuis : %s" p.appuis
                printfn "D'où vient ce texte : %s" (libelleProvenance p.provenance)
                printfn "À citer comme : %s\n" (signature p)
            | None -> ()

        if not suivies.IsEmpty then
            printfn "Publiées, à suivre (labo idees suivre) : %s"
                (suivies |> List.map (fun i -> $"""{i.code.Substring(0, 8)} (ticket n° {i.ticket |> Option.map string |> Option.defaultValue "?"}, {i.statut})""") |> String.concat " ; ")

        printfn "\nlabo idees publier <code> · labo idees ecarter <code> \"raison\""
        0

let publier (debut: string) =
    let resultat =
        trouver debut
        |> Result.bind (fun i ->
            match i.statut, i.proposition with
            | "a-relire", Some p ->
                Moderation.gh
                    [ "issue"; "create"; "--repo"; Moderation.depot
                      "--title"; titre p
                      "--body"; corpsPublic p false
                      "--label"; "idée"; "--label"; "via le jeu" ]
                |> Result.bind (fun url ->
                    let n = url.Substring(url.LastIndexOf '/' + 1)
                    let corps = JsonSerializer.Serialize {| ticket = int n; url = url |}
                    Moderation.appeler HttpMethod.Post $"/moderation/idees/{i.code}/publier" (Some corps) |> Result.map (fun _ -> url))
            | _ -> Error "cette idée n'est plus à relire")

    match resultat with
    | Ok url ->
        printfn "Publiée : %s (le joueur le voit sur son lien de suivi)" url
        0
    | Error e ->
        eprintfn "%s" e
        1

let ecarter (debut: string) (raison: string) =
    let resultat =
        trouver debut
        |> Result.bind (fun i ->
            let corps = JsonSerializer.Serialize {| raison = raison |}
            Moderation.appeler HttpMethod.Post $"/moderation/idees/{i.code}/ecarter" (Some corps))

    match resultat with
    | Ok _ ->
        printfn "Écartée ; le joueur voit la raison sur son lien de suivi."
        0
    | Error e ->
        eprintfn "%s" e
        1

/// Le sort d'un ticket public : les étiquettes « à l'étude », « intégrée », « écartée », ou sa
/// fermeture (« not planned » : écartée ; sinon intégrée). La raison : le dernier commentaire.
let sortDuTicket (n: int) =
    Moderation.gh [ "issue"; "view"; string n; "--repo"; Moderation.depot; "--json"; "state,stateReason,labels,comments" ]
    |> Result.map (fun json ->
        use doc = JsonDocument.Parse json
        let r = doc.RootElement
        let etiquettes = [ for e in r.GetProperty("labels").EnumerateArray() -> e.GetProperty("name").GetString() ]
        let commentaires = [ for c in r.GetProperty("comments").EnumerateArray() -> c.GetProperty("body").GetString() ]

        let statut =
            match etiquettes |> List.tryPick statutDeEtiquette with
            | Some s -> Some s
            | None when r.GetProperty("state").GetString() = "CLOSED" ->
                Some(if r.GetProperty("stateReason").GetString() = "NOT_PLANNED" then Ecartee else Integree)
            | None -> None

        statut, List.tryLast commentaires)

let suivre () =
    match lister () with
    | Error e ->
        eprintfn "%s" e
        1
    | Ok l ->
        let mut = ref 0

        for i in l |> List.filter (fun i -> i.ticket.IsSome) do
            match sortDuTicket i.ticket.Value with
            | Ok(Some statut, raison) when codeStatut statut <> i.statut ->
                let corps = JsonSerializer.Serialize {| statut = codeStatut statut; raison = (if statut = Ecartee then defaultArg raison "" else "") |}

                match Moderation.appeler HttpMethod.Post $"/moderation/idees/{i.code}/statut" (Some corps) with
                | Ok _ ->
                    mut.Value <- mut.Value + 1
                    printfn "Ticket n° %d : %s" i.ticket.Value (libelleStatut statut)
                | Error e -> eprintfn "Ticket n° %d : %s" i.ticket.Value e
            | Ok _ -> ()
            | Error e -> eprintfn "Ticket n° %d : %s" i.ticket.Value e

        printfn "%d suivi(s) mis à jour." mut.Value
        0

let executer (args: string list) =
    match args with
    | [] -> afficher ()
    | [ "publier"; code ] -> publier code
    | [ "ecarter"; code; raison ] -> ecarter code raison
    | [ "suivre" ] -> suivre ()
    | _ ->
        eprintfn "labo idees [publier <code> | ecarter <code> \"raison\" | suivre]"
        1
