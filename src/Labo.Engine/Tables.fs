/// Les données du moteur, lues dans Labo.Data (généré depuis data/ par `labo generate`). Aucune
/// valeur n'est écrite ici : seulement des accès commodes. Les données de la France viennent de
/// data/pays/fr/.
module Labo.Engine.Tables

open Labo.Data

module C = Labo.Data.Coefficients
module F = Labo.Data.Pays.Fr

let curseursParDefaut =
    Modele.curseurs |> List.choose (fun c -> c.defaut |> Option.map (fun v -> c.id, v)) |> Map.ofList

/// L'état de départ du jeu.
let etatDeDepart: Etat =
    { c = Map.empty
      s =
        Modele.curseurs
        |> List.choose (fun c ->
            match c.depart, c.defaut with
            | Some v, _
            | None, Some v -> Some(c.id, v)
            | None, None -> None)
        |> Map.ofList
      mp =
        { ecart = C.monnaieEcartDepart
          fonte = C.monnaieFonteDepart
          plafond = C.monnaiePlafondDepart
          socle = C.monnaieSocleDepart }
      p = "om"
      r = true }

// ---------- France ----------

let population0 perimetre =
    if perimetre = "om" then F.populationFranceEntiere else F.populationMetropole

/// Population projetée, en millions : aujourd'hui, dans 10 ans, dans 30 ans.
let populationHorizon = [| F.populationAujourdhui; F.populationDans10Ans; F.populationDans30Ans |]
let anneesHorizon = [| 0.0; C.horizon10Ans; C.horizon30Ans |]

// ---------- Leviers ----------

let levierDe = Leviers.tous |> List.map (fun l -> l.id, l) |> Map.ofList

let optionsDe =
    Leviers.options |> List.groupBy _.levier |> List.map (fun (k, os) -> k, os) |> Map.ofList

/// Le nom du champ de l'état écrit par un levier (`c.prev_ete` → `prev_ete`).
let champDuLevier id =
    match levierDe[id].champ.Split '.' with
    | [| _; k |] -> k
    | _ -> failwith $"le levier {id} n'a pas de champ"

// ---------- Test de fermeture ----------

/// Rang d'une couverture : manquant ou non vital, partiel, importé ou couvert.
let rangCouverture =
    function
    | "manq"
    | "nv" -> 0
    | "part" -> 1
    | "imp"
    | "ok" -> 2
    | autre -> failwith $"couverture inconnue {autre}"

/// La couverture qu'apporte l'option de rang `rang` du levier qui règle une ressource.
let couvertureOption levier rang =
    let o = optionsDe[levier] |> List.find (fun o -> o.rang = rang)
    (F.ressourcesOptions |> List.find (fun x -> x.levier = levier && x.option = o.id)).couverture

// ---------- Paris, passage à l'échelle, prévention ----------

let rangMaturite = Modele.maturites |> List.map (fun m -> m.id, m.rang) |> Map.ofList

/// Les parades de chaque faiblesse, dans l'ordre des faiblesses.
let faiblesses =
    Modele.faiblesses |> List.map (fun f -> Modele.parades |> List.filter (fun p -> p.faiblesse = f.id))

/// Champ de l'état, facteur de risque.
let prevention = Modele.prevention |> List.map (fun p -> champDuLevier p.levier, p.facteur)
