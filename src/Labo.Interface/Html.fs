/// Un petit constructeur de page : on décrit l'écran comme un arbre de nœuds, et on le recrée à
/// chaque changement. Assez pour un jeu à tours, sans bibliothèque d'interface.
module Labo.Interface.Html

open Browser
open Browser.Types

type Noeud =
    | Element of tag: string * attributs: (string * string) list * evenements: (string * (Event -> unit)) list * enfants: Noeud list
    | Texte of string
    | Rien

let rec creer (n: Noeud) : Node option =
    match n with
    | Rien -> None
    | Texte t -> Some(document.createTextNode t :> Node)
    | Element(tag, attrs, evs, enfants) ->
        let el =
            if tag = "svg" || tag = "circle" || tag = "path" then
                document.createElementNS("http://www.w3.org/2000/svg", tag)
            else
                document.createElement tag

        for (k, v) in attrs do
            el.setAttribute(k, v)

        for (e, f) in evs do
            el.addEventListener(e, f)

        for enfant in enfants do
            match creer enfant with
            | Some x -> el.appendChild x |> ignore
            | None -> ()

        Some(el :> Node)

/// Remplace le contenu d'un conteneur par l'arbre donné.
let monter (conteneur: HTMLElement) (n: Noeud) =
    conteneur.innerHTML <- ""

    match creer n with
    | Some x -> conteneur.appendChild x |> ignore
    | None -> ()

let el tag attrs enfants = Element(tag, attrs, [], enfants)
let div cls enfants = Element("div", [ "class", cls ], [], enfants)
let span cls enfants = Element("span", [ "class", cls ], [], enfants)
let texte t = Texte t
let para cls enfants = Element("p", [ "class", cls ], [], enfants)
let h1 enfants = Element("h1", [], [], enfants)
let h2 cls enfants = Element("h2", [ "class", cls ], [], enfants)
let h3 cls enfants = Element("h3", [ "class", cls ], [], enfants)

let bouton cls (action: unit -> unit) enfants =
    Element("button", [ "class", cls; "type", "button" ], [ "click", (fun _ -> action ()) ], enfants)

let boutonAvec attrs (action: unit -> unit) enfants =
    Element("button", ("type", "button") :: attrs, [ "click", (fun _ -> action ()) ], enfants)

let style (k: string) (v: string) = k + ":" + v + ";"
