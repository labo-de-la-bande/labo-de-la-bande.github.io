/// La vérification du moteur compilé en JavaScript : la page jeu/verifier.html rejoue les états de
/// l'oracle (oracle/*.json) avec le moteur tel que Fable l'a compilé, et écrit le résultat dans la
/// page. `labo verifier-js` l'ouvre dans Chrome sans écran et lit ce résultat. Le même contrôle que
/// tests/Labo.Tests/OracleTests.fs, mais sur le code qui tournera dans le navigateur.
module Labo.Interface.Verifier

open Browser
open Fable.Core
open Fable.Core.JsInterop
open Labo.Engine

let tolerance = 1.5e-6
let typeDe (o: obj) : string = emitJsExpr o "typeof $0"
let estTableau (o: obj) : bool = emitJsExpr o "Array.isArray($0)"
let cles (o: obj) : string array = emitJsExpr o "Object.keys($0)"

let rec valeur (o: obj) =
    match typeDe o with
    | "number" -> Num(unbox<float> o)
    | "string" -> Txt(unbox<string> o)
    | "boolean" -> Bool(unbox<bool> o)
    | "object" when not (isNull o) -> Obj(cles o |> Array.map (fun k -> k, valeur o?(k)) |> Map.ofArray)
    | _ -> Nul

let etatDe (o: obj) : Etat =
    let c = o?c
    let s = o?s
    let mp = o?mp

    { c = cles c |> Array.map (fun k -> k, valeur c?(k)) |> Map.ofArray
      s = cles s |> Array.map (fun k -> k, unbox<float> s?(k)) |> Map.ofArray
      mp =
        { ecart = mp?ecart
          fonte = mp?fonte
          plafond = mp?plafond
          socle = mp?socle }
      p = o?p
      r = o?r }

let idDeLibelle = Labo.Data.Jauges.tous |> List.map (fun j -> j.libelle, j.id) |> Map.ofList

let ecarts (o: obj) =
    let r = Moteur.evaluer (etatDe o?etat)
    let e = ResizeArray<string>()

    let nombre nom (attendu: float) (obtenu: float) =
        if not (abs (attendu - obtenu) <= tolerance) then
            e.Add $"{nom} : attendu {attendu}, obtenu {obtenu}"

    nombre "score" o?SC r.score
    nombre "légitimité" o?LG r.legitimite
    nombre "fidélité" o?FI r.fidelite
    nombre "score France" o?FR r.scoreFrance
    nombre "légitimité France" o?FLG r.legitimiteFrance
    nombre "fidélité France" o?FFI r.fideliteFrance
    if unbox<bool> o?VI <> r.viable.ok then e.Add "viable"
    if unbox<bool> o?FVI <> r.viableFrance.ok then e.Add "viable (France)"

    for g in unbox<obj array> o?G do
        let id = idDeLibelle[unbox<string> g?(0)]
        let j = r.jauges |> List.find (fun x -> x.id = id)
        let v: obj = g?(1)

        match estTableau v, j.valeur with
        | false, Moteur.Valeur x -> nombre id (unbox v) x
        | true, Moteur.Fourchette(a, b) ->
            nombre (id + " (bas)") v?(0) a
            nombre (id + " (haut)") v?(1) b
        | _ -> e.Add(id + " : forme différente")

    let attendus = unbox<obj array> o?LK |> Array.map (fun l -> unbox<int> l?(0), unbox<float> l?(1)) |> Map.ofArray
    let obtenus = r.liens |> List.map (fun l -> l.rang, l.effet) |> Map.ofList

    if Set.ofSeq attendus.Keys <> Set.ofSeq obtenus.Keys then
        e.Add "liens actifs différents"
    else
        for KeyValue(i, a) in attendus do
            nombre $"lien {i}" a obtenus[i]

    List.ofSeq e

let ecrire (texte: string) =
    let pre = document.getElementById "resultat"
    pre.textContent <- texte
    pre.setAttribute("data-fini", "oui")

let lots = [ "golden"; "large"; "etendu" ]

let rec verifier (restants: string list) (bilan: string list) =
    match restants with
    | [] -> ecrire (String.concat "\n" (List.rev bilan))
    | lot :: suite ->
        let promesse: obj = window?fetch ($"../oracle/{lot}.json")

        promesse?``then``(fun reponse -> reponse?json ())?``then``(fun (d: obj) ->
            let resultats = unbox<obj array> d?resultats

            let fautes =
                resultats
                |> Array.mapi (fun i o -> i, ecarts o)
                |> Array.filter (fun (_, e) -> not e.IsEmpty)

            let ligne =
                if fautes.Length = 0 then
                    $"OK {lot} : {resultats.Length} états identiques"
                else
                    let (i, e) = fautes[0]
                    $"ÉCART {lot} : {fautes.Length} état(s) sur {resultats.Length} diffèrent ; état {i} : " + String.concat " ; " (List.truncate 5 e)

            verifier suite (ligne :: bilan))?catch(fun err -> ecrire $"ERREUR {lot} : {err}")
        |> ignore

verifier lots []
