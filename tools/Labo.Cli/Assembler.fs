/// `labo assembler` : réunit les modules JavaScript produits par Fable (jeu/js/) en un seul script,
/// puis en une page autonome (jeu/autonome.html) qui n'a besoin d'aucun autre fichier. Utile là où
/// une page ne peut pas charger ses propres modules (une page claude.ai, une pièce jointe).
///
/// Ne lit que les formes que Fable produit : `import { a, b as c } from "…"`, `import * as N from
/// "…"`, `import D from "…"`, `import D, { … } from "…"`, et `export function|class|const|let|var
/// nom`, `export default nom;`, `export default function nom(`. Ordre d'évaluation des modules
/// JavaScript (dépendances d'abord, en profondeur) ; les exports passent par des accesseurs ; un
/// import qui vise un module encore en cours d'évaluation (un cycle) est relié dès que ce module a
/// fini, comme le ferait le navigateur. Une forme inconnue arrête l'assemblage.
module Labo.Cli.Assembler

open System
open System.IO
open System.Text
open System.Text.RegularExpressions

type ErreurAssemblage =
    | FormeInconnue of fichier: string * ligne: string
    | ModuleIntrouvable of depuis: string * chemin: string

let decrire =
    function
    | FormeInconnue(f, l) -> $"{f} : forme d'import ou d'export inconnue : {l}"
    | ModuleIntrouvable(d, c) -> $"{d} importe {c}, introuvable"

let reImport = Regex("^import\\s+(.*?)\\s+from\\s+\"([^\"]+)\";\\s*$")
let reExport = Regex("^export\\s+(async\\s+function\\*?|function\\*?|class|const|let|var)\\s+([A-Za-z_$][\\w$]*)")
let reExportDefautNom = Regex("^export\\s+default\\s+([A-Za-z_$][\\w$]*)\\s*;\\s*$")
let reExportDefautFonction = Regex("^export\\s+default\\s+(function\\*?|class)\\s+([A-Za-z_$][\\w$]*)")

/// `a, b as c` → [("a", "a"); ("b", "c")] : nom exporté, nom local.
let noms (liste: string) =
    liste.Split(',', StringSplitOptions.RemoveEmptyEntries ||| StringSplitOptions.TrimEntries)
    |> Array.map (fun n ->
        match n.Split(" as ", StringSplitOptions.TrimEntries) with
        | [| exporte; local |] -> exporte, local
        | _ -> n, n)
    |> List.ofArray

let chaine (s: string) = "\"" + s.Replace("\\", "\\\\").Replace("\"", "\\\"") + "\""

/// Un module transformé en fonction `(__e, __imp) => { … }`.
let transformer (racine: string) (fichier: string) : Result<string * string list, ErreurAssemblage> =
    let cle = Path.GetRelativePath(racine, fichier).Replace('\\', '/')
    let dossier = Path.GetDirectoryName fichier
    let exports = ResizeArray<string * string>()
    let locaux = ResizeArray<string>()
    let liaisons = ResizeArray<string>()
    let deps = ResizeArray<string>()
    let corps = StringBuilder()
    let mutable erreur = None

    for ligne in File.ReadAllLines fichier do
        if erreur.IsNone then
            let mi = reImport.Match ligne

            if ligne.StartsWith "import " then
                if not mi.Success then
                    erreur <- Some(FormeInconnue(cle, ligne))
                else
                    let cible = Path.GetFullPath(Path.Combine(dossier, mi.Groups[2].Value))

                    if not (File.Exists cible) then
                        erreur <- Some(ModuleIntrouvable(cle, mi.Groups[2].Value))
                    else
                        let ccle = Path.GetRelativePath(racine, cible).Replace('\\', '/')
                        deps.Add cible
                        let clause = mi.Groups[1].Value.Trim()
                        let affectations = ResizeArray<string>()

                        let mDefautEtNoms = Regex.Match(clause, "^([A-Za-z_$][\\w$]*)\\s*,\\s*\\{(.*)\\}$")
                        let mNoms = Regex.Match(clause, "^\\{(.*)\\}$")
                        let mEspace = Regex.Match(clause, "^\\*\\s+as\\s+([A-Za-z_$][\\w$]*)$")
                        let mDefaut = Regex.Match(clause, "^([A-Za-z_$][\\w$]*)$")

                        let ajouterNoms (l: string) =
                            for (exporte, local) in noms l do
                                locaux.Add local
                                affectations.Add $"{local} = m.{exporte};"

                        if mDefautEtNoms.Success then
                            locaux.Add mDefautEtNoms.Groups[1].Value
                            affectations.Add $"{mDefautEtNoms.Groups[1].Value} = m.default;"
                            ajouterNoms mDefautEtNoms.Groups[2].Value
                        elif mNoms.Success then
                            ajouterNoms mNoms.Groups[1].Value
                        elif mEspace.Success then
                            // un espace de noms : l'objet des exports, dont les accesseurs restent vivants
                            locaux.Add mEspace.Groups[1].Value
                            affectations.Add $"{mEspace.Groups[1].Value} = m;"
                        elif mDefaut.Success then
                            locaux.Add mDefaut.Groups[1].Value
                            affectations.Add $"{mDefaut.Groups[1].Value} = m.default;"
                        else
                            erreur <- Some(FormeInconnue(cle, ligne))

                        liaisons.Add("__imp(" + chaine ccle + ", m => { " + String.Join(" ", affectations) + " });")

                    corps.AppendLine() |> ignore
            elif ligne.StartsWith "export " then
                let me = reExport.Match ligne
                let md = reExportDefautNom.Match ligne
                let mf = reExportDefautFonction.Match ligne

                if me.Success then
                    exports.Add(me.Groups[2].Value, me.Groups[2].Value)
                    corps.AppendLine(ligne.Substring("export ".Length)) |> ignore
                elif md.Success then
                    exports.Add("default", md.Groups[1].Value)
                    corps.AppendLine() |> ignore
                elif mf.Success then
                    exports.Add("default", mf.Groups[2].Value)
                    corps.AppendLine(ligne.Substring(ligne.IndexOf mf.Groups[1].Value)) |> ignore
                else
                    erreur <- Some(FormeInconnue(cle, ligne))
            else
                corps.AppendLine ligne |> ignore

    match erreur with
    | Some e -> Error e
    | None ->
        let accesseurs =
            exports
            |> Seq.map (fun (nom, local) -> $"Object.defineProperty(__e, {chaine nom}, {{ enumerable: true, get: () => {local} }});")

        let declarations = if locaux.Count = 0 then "" else "let " + String.Join(", ", Seq.distinct locaux) + ";"

        let texte =
            "__def(" + chaine cle + ", function (__e, __imp) {\n" + String.Join("\n", accesseurs) + "\n" + declarations + "\n"
            + String.Join("\n", liaisons) + "\n" + corps.ToString() + "});\n"

        Ok(texte, List.ofSeq deps)

/// Le runtime : définir, charger (en profondeur), relier.
let runtime =
    """"use strict";
const __mods = {};
function __def(cle, fn) { __mods[cle] = { fn, e: {}, commence: false, fini: false, attente: [] }; }
function __charger(cle) {
  const m = __mods[cle];
  if (!m) throw new Error("module absent : " + cle);
  if (!m.commence) {
    m.commence = true;
    m.fn(m.e, __imp);
    m.fini = true;
    for (const w of m.attente) w(m.e);
    m.attente = [];
  }
  return m;
}
function __imp(cle, lier) { const m = __charger(cle); if (m.fini) lier(m.e); else m.attente.push(lier); }
"""

/// Réunit l'entrée et tout ce qu'elle importe en un seul script.
let assembler (racine: string) (entree: string) =
    let vus = Collections.Generic.HashSet<string>()
    let morceaux = ResizeArray<string>()

    let rec visiter (f: string) =
        if vus.Add f then
            match transformer racine f with
            | Error e -> Error e
            | Ok(texte, deps) ->
                morceaux.Add texte

                deps
                |> List.fold (fun acc d -> acc |> Result.bind (fun () -> visiter d)) (Ok())
        else
            Ok()

    visiter (Path.GetFullPath entree)
    |> Result.map (fun () ->
        let cle = Path.GetRelativePath(racine, Path.GetFullPath entree).Replace('\\', '/')
        "(function () {\n" + runtime + String.Join("", morceaux) + $"__charger({chaine cle});\n}})();\n")

/// Remplace, dans une page de jeu/, la balise du module `entree` par le script assemblé.
let pageAutonome (page: string) (entree: string) (script: string) =
    let html = File.ReadAllText(Repo.path [ "jeu"; page ])
    // `</script>` dans le script fermerait la balise : on le neutralise
    let sur = script.Replace("</script", "<\\/script")
    let balise = $"<script type=\"module\" src=\"\\./js/{Regex.Escape entree}\"></script>"
    let contenu = "\n" + sur
    let autonome = Regex.Replace(html, balise, (fun _ -> "<script>" + contenu + "</script>"))

    // une page qui déclare une politique de sécurité (CSP) n'autorise que son propre script, par
    // son empreinte : un script injecté ailleurs ne s'exécuterait pas
    let empreinte =
        "'sha256-" + Convert.ToBase64String(Security.Cryptography.SHA256.HashData(Encoding.UTF8.GetBytes contenu)) + "'"

    let autonome = autonome.Replace("script-src 'self'", "script-src 'self' " + empreinte)
    if autonome.Contains "<script>\n" |> not then Error $"jeu/{page} : la balise du module {entree} est introuvable" else Ok autonome

/// `labo assembler` : jeu/js/App.js → jeu/labo.js et jeu/autonome.html (la page du jeu, script
/// inclus) ; jeu/js/Proposer.js → jeu/proposer-autonome.html (la page « Proposer une idée ») ;
/// jeu/js/Prenom.js → jeu/prenom-autonome.html (la page « Mon prénom »).
let executer () =
    let racine = Repo.path [ "jeu"; "js" ]

    let proposer =
        assembler racine (Path.Combine(racine, "Proposer.js"))
        |> Result.mapError decrire
        |> Result.bind (pageAutonome "proposer.html" "Proposer.js")

    let prenom =
        assembler racine (Path.Combine(racine, "Prenom.js"))
        |> Result.mapError decrire
        |> Result.bind (pageAutonome "prenom.html" "Prenom.js")

    let atelier =
        assembler racine (Path.Combine(racine, "AtelierPage.js"))
        |> Result.mapError decrire
        |> Result.bind (pageAutonome "atelier.html" "AtelierPage.js")

    match assembler racine (Path.Combine(racine, "App.js")) |> Result.mapError decrire, proposer, prenom, atelier with
    | Error e, _, _, _
    | _, Error e, _, _
    | _, _, Error e, _
    | _, _, _, Error e ->
        eprintfn "%s" e
        1
    | Ok script, Ok pageProposer, Ok pagePrenom, Ok pageAtelier ->
        let sortie = Repo.path [ "jeu"; "labo.js" ]
        Repo.writeText sortie script

        match pageAutonome "index.html" "App.js" script with
        | Error e ->
            eprintfn "%s" e
            1
        | Ok autonome ->
            Repo.writeText (Repo.path [ "jeu"; "autonome.html" ]) autonome
            Repo.writeText (Repo.path [ "jeu"; "proposer-autonome.html" ]) pageProposer
            Repo.writeText (Repo.path [ "jeu"; "prenom-autonome.html" ]) pagePrenom
            Repo.writeText (Repo.path [ "jeu"; "atelier-autonome.html" ]) pageAtelier
            printfn "écrit %s (%d Ko), jeu/autonome.html, jeu/proposer-autonome.html, jeu/prenom-autonome.html et jeu/atelier-autonome.html" (Repo.rel sortie) (script.Length / 1024)
            0
