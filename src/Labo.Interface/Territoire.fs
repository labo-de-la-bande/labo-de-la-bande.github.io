/// Le territoire, en formes simples : la coupe de vallée de Geddes, de la montagne à la mer.
/// Chaque étage montre une jauge : les champs la nourriture, la forêt les ressources, les fenêtres
/// de la ville le niveau de vie, les bateaux les échanges, les cellules l'égalité. Le soleil avance
/// avec les révolutions. Des images dessinées remplaceront ces formes (docs/bible.md, § 8).
module Labo.Interface.Territoire

open Browser
open Browser.Types
open Fable.Core
open Fable.Core.JsInterop
open Labo.Engine

let couleur (nom: string) =
    let style: obj = window?getComputedStyle (document.documentElement)
    let v = (string (style?getPropertyValue (nom))).Trim()
    if v = "" then "#888" else v

let valeur (r: Moteur.Resultat) id =
    (r.jauges |> List.find (fun j -> j.id = id)).valeur.Milieu

/// Un nombre pseudo-aléatoire stable (les mêmes arbres à chaque dessin).
let bruit (i: int) =
    let x = sin (float i * 12.9898) * 43758.5453
    x - floor x

let dessiner (canvas: HTMLCanvasElement) (r: Moteur.Resultat) (revolution: int) (ouverture: int) (alerte: string option) =
    let dpr = window.devicePixelRatio
    let largeur = canvas.clientWidth
    let hauteur = canvas.clientHeight
    canvas.width <- largeur * dpr
    canvas.height <- hauteur * dpr
    let ctx = canvas.getContext_2d ()
    ctx.setTransform(dpr, 0.0, 0.0, dpr, 0.0, 0.0)
    let w, h = largeur, hauteur

    // le ciel, et le soleil qui avance d'une révolution à l'autre
    let ciel = ctx.createLinearGradient(0.0, 0.0, 0.0, h)
    ciel.addColorStop(0.0, couleur "--ciel-haut")
    ciel.addColorStop(1.0, couleur "--ciel-bas")
    ctx.fillStyle <- U3.Case2 ciel
    ctx.fillRect(0.0, 0.0, w, h)
    let angle = System.Math.PI * (0.15 + 0.7 * (float (revolution % 12) / 11.0))
    let sx, sy = w * 0.5 - cos angle * w * 0.42, h * 0.55 - sin angle * h * 0.45
    ctx.beginPath ()
    ctx.arc(sx, sy, 16.0, 0.0, 2.0 * System.Math.PI)
    ctx.fillStyle <- U3.Case1(couleur "--soleil")
    ctx.fill ()

    // le relief : montagne, forêt, champs, village, ville, mer
    let sol x =
        let t = x / w

        if t < 0.18 then h * (0.30 + 0.5 * (t / 0.18) ** 1.4)
        elif t < 0.80 then h * (0.80 - 0.04 * sin (t * 18.0))
        else h * 0.84

    ctx.beginPath ()
    ctx.moveTo(0.0, h)

    for i in 0..200 do
        let x = w * float i / 200.0
        ctx.lineTo(x, sol x)

    ctx.lineTo(w, h)
    ctx.closePath ()
    ctx.fillStyle <- U3.Case1(couleur "--terre")
    ctx.fill ()

    // la mer
    ctx.fillStyle <- U3.Case1(couleur "--mer")
    ctx.fillRect(w * 0.82, h * 0.84, w * 0.18, h * 0.16)

    // la forêt : autant d'arbres que de ressources couvertes
    let arbres = int (valeur r "ressources" * 22.0)

    for i in 0 .. arbres - 1 do
        let x = w * (0.10 + 0.20 * bruit i)
        let y = sol x
        ctx.beginPath ()
        ctx.moveTo(x, y - 18.0)
        ctx.lineTo(x - 6.0, y)
        ctx.lineTo(x + 6.0, y)
        ctx.closePath ()
        ctx.fillStyle <- U3.Case1(couleur "--foret")
        ctx.fill ()

    // les champs : leur vert dit la nourriture
    let nourriture = valeur r "nourriture"

    for i in 0..5 do
        let x = w * (0.31 + 0.045 * float i)
        ctx.globalAlpha <- 0.35 + 0.65 * nourriture
        ctx.fillStyle <- U3.Case1(if i % 2 = 0 then couleur "--champ" else couleur "--champ-2")
        ctx.fillRect(x, sol x - 6.0, w * 0.04, 8.0)

    ctx.globalAlpha <- 1.0

    // le village et la ville : fenêtres allumées selon le niveau de vie
    let niveau = valeur r "niveau-de-vie"

    for i in 0..9 do
        let x = w * (0.58 + 0.022 * float i)
        let haut = if i < 4 then 18.0 else 26.0 + 22.0 * bruit (i + 40)
        let y = sol x - haut
        ctx.fillStyle <- U3.Case1(couleur "--maison")
        ctx.fillRect(x, y, w * 0.018, haut)

        for k in 0 .. int (haut / 9.0) - 1 do
            if bruit (i * 7 + k) < niveau then
                ctx.fillStyle <- U3.Case1(couleur "--fenetre")
                ctx.fillRect(x + 3.0, y + 4.0 + 9.0 * float k, 4.0, 4.0)

    // les cellules : des cercles d'habitants, plus pleins quand l'égalité monte
    let egalite = valeur r "egalite"

    for i in 0..4 do
        let x = w * (0.36 + 0.09 * float i)
        let y = h * 0.885
        ctx.beginPath ()
        ctx.arc(x, y, 8.0, 0.0, 2.0 * System.Math.PI)
        ctx.lineWidth <- 2.0
        ctx.strokeStyle <- U3.Case1(couleur "--cellule")
        ctx.stroke ()
        ctx.globalAlpha <- egalite
        ctx.fillStyle <- U3.Case1(couleur "--cellule")
        ctx.fill ()
        ctx.globalAlpha <- 1.0

    // les bateaux : seulement si le pays échange
    if ouverture > 0 then
        for i in 0 .. ouverture do
            let x = w * (0.85 + 0.04 * float i)
            let y = h * 0.88 + 4.0 * bruit i
            ctx.fillStyle <- U3.Case1(couleur "--bateau")
            ctx.beginPath ()
            ctx.moveTo(x - 10.0, y)
            ctx.lineTo(x + 10.0, y)
            ctx.lineTo(x + 6.0, y + 6.0)
            ctx.lineTo(x - 6.0, y + 6.0)
            ctx.closePath ()
            ctx.fill ()

    // les noms des étages
    ctx.fillStyle <- U3.Case1(couleur "--encre-douce")
    ctx.font <- "12px 'Atkinson Hyperlegible', system-ui, sans-serif"

    for (t, nom) in [ 0.05, "Montagne"; 0.18, "Forêt"; 0.36, "Champs"; 0.58, "Village"; 0.68, "Ville"; 0.86, "Mer" ] do
        ctx.fillText(nom, w * t, h * 0.99)

    // ce qui vient d'arriver
    match alerte with
    | Some t ->
        ctx.font <- "600 15px 'Bricolage Grotesque', system-ui, sans-serif"
        ctx.fillStyle <- U3.Case1(couleur "--encre")
        ctx.fillText(t, 14.0, 26.0)
    | None -> ()
