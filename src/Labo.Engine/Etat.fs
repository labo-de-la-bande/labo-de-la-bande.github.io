/// L'état d'une partie, sous la forme exacte de l'ancien moteur (et des liens de partage `#m=`) :
/// `c` les choix, `s` les curseurs, `mp` les réglages de la monnaie, `p` le périmètre
/// (`om` : France entière, outre-mer compris ; `m` : métropole), `r` la règle des paliers.
namespace Labo.Engine

type Val =
    | Num of float
    | Txt of string
    | Bool of bool
    | Obj of Map<string, Val>
    | Nul

type ReglagesMonnaie =
    { ecart: float
      fonte: float
      plafond: float
      socle: float }

type Etat =
    { c: Map<string, Val>
      s: Map<string, float>
      mp: ReglagesMonnaie
      p: string
      r: bool }

/// Les lectures de l'ancien moteur, avec la sémantique de JavaScript qu'il supposait : une
/// valeur absente vaut `undefined`, `x || d` prend `d` quand `x` est « faux » (0, "", absent),
/// `===` compare type et valeur.
module Js =
    let vrai =
        function
        | Some(Num n) -> n <> 0.0 && not (System.Double.IsNaN n)
        | Some(Txt t) -> t <> ""
        | Some(Bool b) -> b
        | Some(Obj _) -> true
        | Some Nul
        | None -> false

    /// `Math.round` : la moitié va vers +∞ (pas l'arrondi bancaire de .NET).
    let round (x: float) = floor (x + 0.5)

    let clamp01 x = max 0.0 (min 1.0 x)

    /// `clamp` de l'ancien moteur : entre 0 et 100.
    let clamp100 x = max 0.0 (min 100.0 x)

module Etat =
    let choix k (e: Etat) = Map.tryFind k e.c

    /// `choix[k] === n`
    let estNum k n e =
        match choix k e with
        | Some(Num x) -> x = n
        | _ -> false

    /// `choix[k] === "t"`
    let estTxt k t e =
        match choix k e with
        | Some(Txt x) -> x = t
        | _ -> false

    /// `choix[k] >= n` (absent : faux)
    let auMoins k n e =
        match choix k e with
        | Some(Num x) -> x >= n
        | Some(Bool b) -> (if b then 1.0 else 0.0) >= n
        | _ -> false

    /// `choix[k] || d`, pour un nombre
    let nombreOu k d e =
        match choix k e with
        | Some(Num x) when x <> 0.0 -> x
        | _ -> d

    /// `choix[k]` quand c'est un texte
    let texte k e =
        match choix k e with
        | Some(Txt t) -> Some t
        | _ -> None

    /// `choix[k] && choix[k][id]` : une case cochée dans un objet (paris, parades, exportations)
    let coche k id e =
        match choix k e with
        | Some(Obj m) -> Js.vrai (Map.tryFind id m)
        | _ -> false

    let defini k e =
        match choix k e with
        | None
        | Some Nul -> false
        | Some _ -> true
