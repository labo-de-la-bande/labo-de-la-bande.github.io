/// Parler au relais en tant que collaborateur du dépôt : le jeton de `gh` (déjà connecté) prouve à
/// GitHub qui l'on est, et le relais vérifie auprès de GitHub que ce compte peut écrire dans le
/// dépôt. Aucune clé à se transmettre : un collaborateur ajouté sur GitHub peut relire tout de suite.
module Labo.Cli.Moderation

open System
open System.Net.Http
open System.Text
open System.Text.Json
open CliWrap
open CliWrap.Buffered

/// L'adresse du relais (la même que dans src/Labo.Interface/Relais.fs), ou LABO_RELAIS.
let adresse () =
    match Environment.GetEnvironmentVariable "LABO_RELAIS" with
    | null
    | "" -> "https://labo-relais.colet-patrice.workers.dev"
    | a -> a.TrimEnd '/'

let depot = "labo-de-la-bande/labo-de-la-bande.github.io"

/// Lance `gh` avec ces arguments ; sa sortie, ou son message d'erreur.
let gh (args: string list) =
    let r =
        Cli.Wrap("gh").WithArguments(args).WithValidation(CommandResultValidation.None).ExecuteBufferedAsync().Task.Result

    if r.ExitCode = 0 then Ok(r.StandardOutput.Trim()) else Error(r.StandardError.Trim())

let jeton () =
    gh [ "auth"; "token" ]
    |> Result.mapError (fun e -> $"gh n'est pas connecté (gh auth login) : {e}")

/// Un appel au relais avec le jeton GitHub : le code HTTP et le texte de la réponse.
let appeler (methode: HttpMethod) (chemin: string) (corps: string option) =
    jeton ()
    |> Result.bind (fun j ->
        use client = new HttpClient()
        use req = new HttpRequestMessage(methode, adresse () + chemin)
        req.Headers.Add("Authorization", "Bearer " + j)
        req.Headers.Add("User-Agent", "labo-cli")

        match corps with
        | Some c -> req.Content <- new StringContent(c, Encoding.UTF8, "application/json")
        | None -> ()

        let rep = client.Send req
        let texte = rep.Content.ReadAsStringAsync().Result

        if rep.IsSuccessStatusCode then
            Ok texte
        else
            let message =
                try JsonDocument.Parse(texte).RootElement.GetProperty("erreur").GetString() with _ -> texte

            Error $"le relais répond {int rep.StatusCode} : {message}")
