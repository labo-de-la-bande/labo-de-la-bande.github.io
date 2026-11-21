/// Le moteur F# contre l'oracle : les réponses de l'ancien moteur JavaScript, figées dans
/// oracle/*.json (`labo oracle capture`). Chaque état est rejoué ; score, légitimité, fidélité,
/// viabilité, scores de la France, chaque jauge et chaque lien actif doivent coïncider.
module Labo.Tests.OracleTests

open System.IO
open System.Text.Json
open Expecto
open Labo.Engine
open Labo.Cli

/// L'oracle arrondit au millionième : on tolère un arrondi de part et d'autre.
let tolerance = 1.5e-6

let rec valeur (j: JsonElement) =
    match j.ValueKind with
    | JsonValueKind.Number -> Num(j.GetDouble())
    | JsonValueKind.String -> Txt(j.GetString())
    | JsonValueKind.True -> Bool true
    | JsonValueKind.False -> Bool false
    | JsonValueKind.Object -> Obj(j.EnumerateObject() |> Seq.map (fun p -> p.Name, valeur p.Value) |> Map.ofSeq)
    | _ -> Nul

let etat (j: JsonElement) : Etat =
    let mp = j.GetProperty "mp"

    { c = j.GetProperty("c").EnumerateObject() |> Seq.map (fun p -> p.Name, valeur p.Value) |> Map.ofSeq
      s = j.GetProperty("s").EnumerateObject() |> Seq.map (fun p -> p.Name, p.Value.GetDouble()) |> Map.ofSeq
      mp =
        { ecart = mp.GetProperty("ecart").GetDouble()
          fonte = mp.GetProperty("fonte").GetDouble()
          plafond = mp.GetProperty("plafond").GetDouble()
          socle = mp.GetProperty("socle").GetDouble() }
      p = j.GetProperty("p").GetString()
      r = j.GetProperty("r").GetBoolean() }

let idDeLibelle = Labo.Data.Jauges.tous |> List.map (fun j -> j.libelle, j.id) |> Map.ofList

/// Les écarts d'un état, décrits ; vide quand tout coïncide.
let ecarts (o: JsonElement) =
    let r = Moteur.evaluer (etat (o.GetProperty "etat"))
    let e = ResizeArray<string>()
    let nombre nom (attendu: float) (obtenu: float) =
        if not (abs (attendu - obtenu) <= tolerance) then e.Add $"{nom} : attendu {attendu}, obtenu {obtenu}"

    nombre "score" (o.GetProperty("SC").GetDouble()) r.score
    nombre "légitimité" (o.GetProperty("LG").GetDouble()) r.legitimite
    nombre "fidélité" (o.GetProperty("FI").GetDouble()) r.fidelite
    nombre "score France" (o.GetProperty("FR").GetDouble()) r.scoreFrance
    nombre "légitimité France" (o.GetProperty("FLG").GetDouble()) r.legitimiteFrance
    nombre "fidélité France" (o.GetProperty("FFI").GetDouble()) r.fideliteFrance
    if o.GetProperty("VI").GetBoolean() <> r.viable.ok then e.Add "viable"
    if o.GetProperty("FVI").GetBoolean() <> r.viableFrance.ok then e.Add "viable (France)"

    for g in o.GetProperty("G").EnumerateArray() do
        let id = idDeLibelle[g[0].GetString()]
        let j = r.jauges |> List.find (fun x -> x.id = id)

        let v = g[1]

        match v.ValueKind, j.valeur with
        | JsonValueKind.Number, Moteur.Valeur x -> nombre id (v.GetDouble()) x
        | JsonValueKind.Array, Moteur.Fourchette(a, b) ->
            nombre (id + " (bas)") (v.Item(0).GetDouble()) a
            nombre (id + " (haut)") (v.Item(1).GetDouble()) b
        | _ -> e.Add $"{id} : forme différente"

    let attendus = o.GetProperty("LK").EnumerateArray() |> Seq.map (fun l -> l[0].GetInt32(), l[1].GetDouble()) |> List.ofSeq
    let obtenus = r.liens |> List.map (fun l -> l.rang, l.effet) |> List.sortBy id
    let rangs l = l |> List.map fst |> Set.ofList

    for i in Set.difference (rangs attendus) (rangs obtenus) do
        e.Add $"lien {Labo.Data.Liens.tous[i].id} actif dans l'oracle, pas ici"

    for i in Set.difference (rangs obtenus) (rangs attendus) do
        e.Add $"lien {Labo.Data.Liens.tous[i].id} actif ici, pas dans l'oracle"

    for (i, a) in attendus do
        match obtenus |> List.tryFind (fun (k, _) -> k = i) with
        | Some(_, b) -> nombre $"lien {Labo.Data.Liens.tous[i].id}" a b
        | None -> ()

    List.ofSeq e

let lot nom =
    test $"oracle {nom}" {
        use doc = JsonDocument.Parse(File.ReadAllText(Repo.path [ "oracle"; $"{nom}.json" ]))
        let resultats = doc.RootElement.GetProperty("resultats").EnumerateArray() |> List.ofSeq

        let fautes =
            resultats
            |> List.indexed
            |> List.choose (fun (i, o) ->
                match ecarts o with
                | [] -> None
                | e -> Some(i, o.GetProperty("etat").GetRawText(), e))

        if not fautes.IsEmpty then
            let detail =
                fautes
                |> List.truncate 3
                |> List.map (fun (i, etat, e) -> $"état {i} {etat}\n    " + System.String.Join("\n    ", e |> List.truncate 8))

            failtest ($"{fautes.Length} état(s) sur {resultats.Length} diffèrent :\n" + System.String.Join("\n", detail))
    }

let tests = testList "moteur" [ lot "golden"; lot "large"; lot "etendu" ]
