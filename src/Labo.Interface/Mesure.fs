/// Outil de développement (jeu/mesure.html) : charge le jeu dans un cadre de 390 pixels de large et
/// mesure s'il déborde. Sert à vérifier la mise en page sur téléphone sans navigateur à l'écran.
module Labo.Interface.Mesure

open Browser
open Fable.Core.JsInterop

let mesurer () =
    let f: obj = document.getElementById "f"
    let doc: obj = f?contentDocument
    let large: float = doc?documentElement?scrollWidth
    let visible: float = doc?documentElement?clientWidth
    let fautifs: string = Fable.Core.JsInterop.emitJsExpr doc "Array.from($0.querySelectorAll('*')).filter(e => e.getBoundingClientRect().right > $0.documentElement.clientWidth + 1).slice(0,6).map(e => e.tagName + '.' + e.className).join(' | ')"
    (document.getElementById "m").textContent <- $"largeur {large} / visible {visible} ; débordent : {fautifs}"

window.setTimeout((fun () -> mesurer ()), 2500) |> ignore
