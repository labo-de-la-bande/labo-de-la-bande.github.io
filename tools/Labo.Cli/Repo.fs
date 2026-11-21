/// Où est le dépôt : le dossier qui contient Labo.slnx, en remontant depuis le dossier courant.
module Labo.Cli.Repo

open System.IO

let root =
    lazy
        (let rec up (d: DirectoryInfo) =
            if isNull d then
                failwith "Labo.slnx introuvable : lancer la CLI depuis le dépôt labo-de-la-bande."
            elif File.Exists(Path.Combine(d.FullName, "Labo.slnx")) then
                d.FullName
            else
                up d.Parent

         up (DirectoryInfo(Directory.GetCurrentDirectory())))

let path (parts: string list) = Path.Combine(Array.ofList (root.Value :: parts))

/// Chemin relatif au dépôt, pour les messages.
let rel (p: string) = Path.GetRelativePath(root.Value, p)

/// Écrit un texte en UTF-8 sans BOM, fins de ligne `\n`.
let writeText (p: string) (text: string) =
    Directory.CreateDirectory(Path.GetDirectoryName p) |> ignore
    File.WriteAllText(p, text.Replace("\r\n", "\n"), System.Text.UTF8Encoding false)
