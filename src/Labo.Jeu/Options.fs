/// Appliquer une option d'un levier à l'état du modèle.
module Labo.Jeu.Options

open Labo.Data
open Labo.Engine

let levierDe = Tables.levierDe
let optionsDe = Tables.optionsDe

let estNombre (s: string) =
    System.Text.RegularExpressions.Regex.IsMatch(s, "^-?[0-9]+(\\.[0-9]+)?$")

/// La valeur qu'une option écrit dans le champ de son levier.
let valeurOption (levier: Levier) (o: OptionLevier) =
    match levier.genre with
    | OuiNon -> Bool(o.valeur = "oui")
    | _ when estNombre o.valeur -> Num(float o.valeur)
    | _ -> Txt o.valeur

let ecrireChamp (champ: string) (v: Val) (e: Etat) =
    match champ.Split '.', v with
    | [| "c"; k |], v -> { e with c = e.c |> Map.add k v }
    | [| "s"; k |], Num n -> { e with s = e.s |> Map.add k n }
    | _ -> failwith $"écriture impossible : {champ}"

/// Les préréglages nommés (`@paliers`, `@sans-paliers`), définis à partir des données.
let prereglage nom (e: Etat) =
    match nom with
    | "paliers" ->
        // toutes les parades qui ne sont ni adoptées d'office ni automatiques, et le protocole d'essai
        let par =
            Modele.parades |> List.filter (fun p -> not p.adoptee && p.automatique.IsNone) |> List.map (fun p -> p.id, Num 1.0)

        let proto =
            Modele.curseurs
            |> List.filter (fun c -> c.id.StartsWith "pr")
            |> List.map (fun c -> c.id, Num(Moteur.curseur c.id e))

        { e with c = e.c |> Map.add "par" (Obj(Map par)) |> Map.add "proto" (Obj(Map proto)) }
    | "sans-paliers" -> { e with c = e.c |> Map.add "par" (Obj Map.empty) |> Map.add "palNo" (Num 1.0) }
    | autre -> failwith $"préréglage inconnu {autre}"

type ErreurOption =
    | LevierInconnu of levier: string
    | OptionInconnue of levier: string * option: string

/// Applique l'option `option` du levier `levier`.
let appliquer levier option (e: Etat) =
    match levierDe.TryFind levier with
    | None -> Error(LevierInconnu levier)
    | Some lv ->
        match optionsDe.TryFind levier |> Option.defaultValue [] |> List.tryFind (fun o -> o.id = option) with
        | None -> Error(OptionInconnue(levier, option))
        | Some o ->
            let e = if lv.champ = "" then e else ecrireChamp lv.champ (valeurOption lv o) e

            o.ecritAussi
            |> List.fold
                (fun e ec ->
                    match ec with
                    | Champ(c, v) -> ecrireChamp c (if estNombre v then Num(float v) else Txt v) e
                    | Nomme n -> prereglage n e)
                e
            |> Ok
