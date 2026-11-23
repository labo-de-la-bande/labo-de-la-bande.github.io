/// `labo cle-confiance <pseudonyme>` : crée une clé de confiance pour le relais des idées. Avec
/// elle, une proposition envoyée sans compte est publiée sans attendre la relecture. Le relais ne
/// garde que l'empreinte de la clé (SHA-256), jamais la clé : elle n'existe que chez la personne.
module Labo.Cli.Confiance

open System
open System.Security.Cryptography
open System.Text

let empreinte (cle: string) =
    SHA256.HashData(Encoding.UTF8.GetBytes("vip:" + cle)) |> Convert.ToHexStringLower

let executer (pseudonyme: string) =
    let cle = Convert.ToBase64String(RandomNumberGenerator.GetBytes 24).Replace('+', '-').Replace('/', '_').TrimEnd('=')
    printfn "Clé de confiance pour %s (à lui transmettre, elle n'est écrite nulle part ailleurs) :" pseudonyme
    printfn ""
    printfn "  %s" cle
    printfn ""
    printfn "Pour l'enregistrer dans le relais, depuis src/Labo.Relais :"
    printfn ""
    printfn "  npx wrangler kv key put --binding IDEES --remote \"vip:%s\" \"%s\"" (empreinte cle) (pseudonyme.Replace("\"", ""))
    printfn ""
    printfn "Pour la retirer : npx wrangler kv key delete --binding IDEES --remote \"vip:%s\"" (empreinte cle)
    0
