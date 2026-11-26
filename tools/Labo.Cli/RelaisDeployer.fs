/// `labo relais deployer | secret <NOM>` : met le relais des idées (src/Labo.Relais, compilé par
/// Fable dans js/) sur Cloudflare Workers par l'API de Cloudflare, sans wrangler (un outil Node) :
/// le projet n'utilise ni Node ni Python. La configuration reste dans src/Labo.Relais/wrangler.toml.
///
/// Il faut CLOUDFLARE_API_TOKEN (droit d'écrire les Workers) ; CLOUDFLARE_ACCOUNT_ID si le jeton
/// ne permet pas de lire la liste des comptes.
module Labo.Cli.RelaisDeployer

open System
open System.IO
open System.Net.Http
open System.Net.Http.Headers
open System.Text
open System.Text.Json
open System.Text.Json.Nodes
open System.Text.RegularExpressions

type Config =
    { nom: string
      principal: string
      compatibilite: string
      vars: (string * string) list
      /// liaison → identifiant du stockage
      kv: (string * string) list }

/// Le sous-ensemble de TOML qu'utilise wrangler.toml : `cle = "valeur"`, `[vars]`, `[[kv_namespaces]]`.
let lireConfig (texte: string) =
    let lignes =
        texte.Split '\n'
        |> Array.map (fun l -> l.Trim())
        |> Array.filter (fun l -> l <> "" && not (l.StartsWith "#"))

    let valeur (l: string) =
        let m = Regex.Match(l, "^([A-Za-z_][A-Za-z0-9_]*)\\s*=\\s*\"(.*)\"$")
        if m.Success then Some(m.Groups[1].Value, m.Groups[2].Value) else None

    let mutable section = ""
    let racine = Collections.Generic.Dictionary<string, string>()
    let vars = ResizeArray<string * string>()
    let kv = ResizeArray<Collections.Generic.Dictionary<string, string>>()

    for l in lignes do
        if l = "[[kv_namespaces]]" then
            section <- "kv"
            kv.Add(Collections.Generic.Dictionary())
        elif l.StartsWith "[" then
            section <- l.Trim('[', ']')
        else
            match valeur l, section with
            | Some(k, v), "" -> racine[k] <- v
            | Some(k, v), "vars" -> vars.Add(k, v)
            | Some(k, v), "kv" -> kv[kv.Count - 1][k] <- v
            | Some _, s -> failwith $"wrangler.toml : section [{s}] inconnue de labo"
            | None, _ -> failwith $"wrangler.toml : ligne illisible : {l}"

    let exige k =
        match racine.TryGetValue k with
        | true, v -> v
        | _ -> failwith $"wrangler.toml : {k} manque"

    { nom = exige "name"
      principal = exige "main"
      compatibilite = exige "compatibility_date"
      vars = List.ofSeq vars
      kv = kv |> Seq.map (fun d -> d["binding"], d["id"]) |> List.ofSeq }

let dossier () = Repo.path [ "src"; "Labo.Relais" ]

/// Les modules compilés : le chemin relatif au dossier du point d'entrée, avec des `/`.
let modules (c: Config) =
    let principal = Path.Combine(dossier (), c.principal)
    let base' = Path.GetDirectoryName principal

    if not (File.Exists principal) then
        failwith $"{Repo.rel principal} manque : lancer d'abord `dotnet fable src/Labo.Relais -o src/Labo.Relais/js`."

    Directory.GetFiles(base', "*.js", SearchOption.AllDirectories)
    |> Array.map (fun f -> Path.GetRelativePath(base', f).Replace('\\', '/'), f)
    |> Array.sortBy fst
    |> List.ofArray

let metadonnees (c: Config) =
    let liaisons = JsonArray()

    for (k, v) in c.vars do
        liaisons.Add(JsonObject(dict [ "type", JsonValue.Create "plain_text" :> JsonNode; "name", JsonValue.Create k; "text", JsonValue.Create v ]))

    for (b, id) in c.kv do
        liaisons.Add(JsonObject(dict [ "type", JsonValue.Create "kv_namespace" :> JsonNode; "name", JsonValue.Create b; "namespace_id", JsonValue.Create id ]))

    JsonObject(
        dict
            [ "main_module", JsonValue.Create(Path.GetFileName c.principal) :> JsonNode
              "compatibility_date", JsonValue.Create c.compatibilite
              "bindings", liaisons
              // les secrets (posés par `labo relais secret`) ne sont jamais dans le dépôt : on les garde
              "keep_bindings", JsonArray(JsonValue.Create "secret_text") ]
    )

let api = "https://api.cloudflare.com/client/v4"

let client (jeton: string) =
    let h = new HttpClient()
    h.DefaultRequestHeaders.Authorization <- AuthenticationHeaderValue("Bearer", jeton)
    h

/// La réponse de Cloudflare : `success`, sinon les messages d'erreur.
let lire (r: HttpResponseMessage) =
    let corps = r.Content.ReadAsStringAsync().Result

    let json = try JsonNode.Parse corps with _ -> null

    if not (isNull json) && json["success"] <> null && json["success"].GetValue<bool>() then
        json
    else
        let erreurs =
            if isNull json || isNull json["errors"] then corps
            else json["errors"].AsArray() |> Seq.map (fun e -> $"""{e["code"]} {e["message"]}""") |> String.concat " ; "

        failwith $"Cloudflare ({int r.StatusCode}) : {erreurs}"

let compte (h: HttpClient) =
    match Environment.GetEnvironmentVariable "CLOUDFLARE_ACCOUNT_ID" with
    | null
    | "" ->
        let r = lire (h.GetAsync($"{api}/accounts").Result)

        match r["result"].AsArray() |> List.ofSeq with
        | [ a ] -> a["id"].GetValue<string>()
        | [] -> failwith "aucun compte visible avec ce jeton : donner CLOUDFLARE_ACCOUNT_ID"
        | _ -> failwith "plusieurs comptes visibles avec ce jeton : donner CLOUDFLARE_ACCOUNT_ID"
    | id -> id

let jeton () =
    match Environment.GetEnvironmentVariable "CLOUDFLARE_API_TOKEN" with
    | null
    | "" -> failwith "CLOUDFLARE_API_TOKEN manque (un jeton qui peut écrire les Workers)"
    | t -> t

let deployer essai =
    let c = lireConfig (File.ReadAllText(Path.Combine(dossier (), "wrangler.toml")))
    let ms = modules c
    let meta = metadonnees c

    printfn "Relais %s : %d modules, point d'entrée %s" c.nom ms.Length (Path.GetFileName c.principal)
    printfn "Liaisons : %s" (String.Join(", ", (c.vars |> List.map fst) @ (c.kv |> List.map fst)))

    if essai then
        printfn "%s" (meta.ToJsonString(JsonSerializerOptions(WriteIndented = true)))
        printfn "Essai : rien n'est envoyé."
    else
        use h = client (jeton ())
        let a = compte h
        use corps = new MultipartFormDataContent()
        let m = new StringContent(meta.ToJsonString(), Encoding.UTF8, "application/json")
        corps.Add(m, "metadata")

        for (nom, f) in ms do
            let p = new ByteArrayContent(File.ReadAllBytes f)
            p.Headers.ContentType <- MediaTypeHeaderValue "application/javascript+module"
            corps.Add(p, nom, nom)

        lire (h.PutAsync($"{api}/accounts/{a}/workers/scripts/{c.nom}", corps).Result) |> ignore
        printfn "Relais déployé."

/// Pose un secret du relais ; la valeur est lue sur l'entrée standard, jamais en argument.
let rec poserSecret (nom: string) =
    if Console.IsInputRedirected |> not then
        eprintf "Valeur de %s (puis Entrée) : " nom

    let valeur = Console.ReadLine()

    if String.IsNullOrWhiteSpace valeur then
        failwith "valeur vide : rien n'est posé"

    poserSecretValeur nom valeur

/// Pose un secret du relais avec une valeur déjà connue du programme (jamais affichée).
and poserSecretValeur (nom: string) (valeur: string) =
    let c = lireConfig (File.ReadAllText(Path.Combine(dossier (), "wrangler.toml")))
    use h = client (jeton ())
    let a = compte h

    let json =
        JsonObject(dict [ "name", JsonValue.Create nom :> JsonNode; "text", JsonValue.Create(valeur.Trim()); "type", JsonValue.Create "secret_text" ])

    use corps = new StringContent(json.ToJsonString(), Encoding.UTF8, "application/json")
    lire (h.PutAsync($"{api}/accounts/{a}/workers/scripts/{c.nom}/secrets", corps).Result) |> ignore
    printfn "Secret %s posé sur %s." nom c.nom

let executer (args: string list) =
    try
        match args with
        | [ "deployer" ] -> deployer false; 0
        | [ "deployer"; "--essai" ] -> deployer true; 0
        | [ "secret"; nom ] -> poserSecret nom; 0
        | _ ->
            eprintfn "labo relais deployer [--essai] | labo relais secret <NOM>"
            2
    with e ->
        eprintfn "%s" e.Message
        1
