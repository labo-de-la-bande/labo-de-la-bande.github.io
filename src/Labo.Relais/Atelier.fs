/// L'atelier des données : les tables qu'un collaborateur invité peut modifier sans compte GitHub,
/// par le relais, dans le dépôt séparé `atelier` (docs/atelier.md). Partagé par le relais (qui
/// applique les modifications), la CLI (qui les fusionne dans le dépôt du jeu) et la page de
/// l'atelier. Code pur : il tourne en .NET et, compilé par Fable, dans le navigateur et le Worker.
///
/// Garde-fous : seules les tables et les colonnes listées ici se modifient ; une valeur est une
/// ligne de texte courte, sans caractère de contrôle ; l'en-tête doit être exactement celui
/// attendu ; la provenance de la ligne devient l'identifiant du collaborateur.
module Labo.Relais.Atelier

open System
open System.Text.RegularExpressions

/// Une table ouverte à l'atelier. L'en-tête doit être celui de tools/Labo.Cli/Schema.fs (le
/// doctor le vérifie).
type Table =
    { chemin: string
      titre: string
      aide: string
      entete: string list
      /// la colonne qui identifie une ligne
      cle: string
      /// les colonnes qu'un collaborateur peut écrire (en plus de la clé, pour une ligne nouvelle)
      modifiables: string list
      /// la colonne où le relais écrit l'identifiant du collaborateur
      provenance: string }

let tables =
    [ { chemin = "data/jeu/abolitions.csv"
        titre = "Ce que la société voulait abolir"
        aide =
          "Chaque ligne est une chose qu'une société sans prison ni police pourrait garder quand même. La condition (quand) s'écrit comme dans les liens : levier=option, par exemple bande=gardes."
        entete = [ "id"; "famille"; "palier"; "libelle_fr"; "quand"; "calcul"; "raison_fr"; "provenance" ]
        cle = "id"
        modifiables = [ "famille"; "palier"; "libelle_fr"; "quand"; "raison_fr" ]
        provenance = "provenance" }
      { chemin = "data/jeu/types-partie.csv"
        titre = "Les types de partie et leurs enjeux"
        aide =
          "Les interdits disent ce qui fait perdre la partie : des familles ou des identifiants de la table ci-dessus, séparés par |."
        entete = [ "id"; "titre_fr"; "description_fr"; "interdits"; "provenance" ]
        cle = "id"
        modifiables = [ "titre_fr"; "description_fr"; "interdits" ]
        provenance = "provenance" } ]

let tableDe (chemin: string) = tables |> List.tryFind (fun t -> t.chemin = chemin)

// ---------- CSV (RFC 4180) ----------

/// Une ligne du fichier : ses champs, et son texte d'origine (réécrit tel quel si elle ne change pas).
type Ligne = { champs: string list; brut: string }

/// Lit un CSV : l'en-tête, puis les lignes. Les guillemets doublés, les champs entre guillemets
/// (virgules et retours à la ligne compris) ; \r\n ou \n.
let lire (texte: string) : Result<Ligne list, string> =
    let lignes = ResizeArray<Ligne>()
    let champs = ResizeArray<string>()
    let champ = Text.StringBuilder()
    let mutable debut = 0
    let mutable i = 0
    let mutable guillemets = false
    let mutable erreur = None
    let n = texte.Length

    let finirLigne fin =
        champs.Add(champ.ToString())
        champ.Clear() |> ignore
        let brut = texte.Substring(debut, fin - debut)
        if not (champs.Count = 1 && champs[0] = "" && brut.Trim() = "") then
            lignes.Add { champs = List.ofSeq champs; brut = brut.TrimEnd('\r') }
        champs.Clear()

    while i < n && erreur.IsNone do
        let c = texte[i]

        if guillemets then
            if c = '"' then
                if i + 1 < n && texte[i + 1] = '"' then
                    champ.Append '"' |> ignore
                    i <- i + 1
                else
                    guillemets <- false
            else
                champ.Append c |> ignore
        else
            match c with
            | '"' when champ.Length = 0 -> guillemets <- true
            | '"' -> erreur <- Some $"guillemet au milieu d'un champ (caractère {i})"
            | ',' ->
                champs.Add(champ.ToString())
                champ.Clear() |> ignore
            | '\n' ->
                finirLigne i
                debut <- i + 1
            | '\r' -> ()
            | c -> champ.Append c |> ignore

        i <- i + 1

    match erreur with
    | Some e -> Error e
    | None when guillemets -> Error "guillemet non fermé"
    | None ->
        if debut < n then finirLigne n
        Ok(List.ofSeq lignes)

/// Un champ écrit en CSV : entre guillemets seulement s'il le faut.
let champCsv (s: string) =
    if s.IndexOfAny [| ','; '"'; '\n'; '\r' |] >= 0 then "\"" + s.Replace("\"", "\"\"") + "\"" else s

let ligneCsv (champs: string list) = champs |> List.map champCsv |> String.concat ","

/// Réécrit le fichier : les lignes inchangées gardent leur texte d'origine ; un \n à la fin.
let ecrire (lignes: Ligne list) = (lignes |> List.map _.brut |> String.concat "\n") + "\n"

let nouvelleLigne champs = { champs = champs; brut = ligneCsv champs }

// ---------- Les modifications ----------

type Modification =
    /// change des champs d'une ligne existante ; `avant` : les valeurs que le collaborateur a vues
    | Modifier of cle: string * champs: (string * string) list * avant: (string * string) list
    /// ajoute une ligne (la clé comprise)
    | Ajouter of champs: (string * string) list

let reIdentifiant = Regex "^[a-z0-9][a-z0-9-]{0,59}$"
let longueurMaximum = 1000
let raisonMaximum = 500

/// Une valeur acceptable : une ligne de texte courte, sans caractère de contrôle.
let valeurPropre (v: string) =
    if v.Length > longueurMaximum then Error $"trop long ({v.Length} caractères, {longueurMaximum} au plus)"
    elif v |> Seq.exists (fun c -> Char.IsControl c) then Error "une seule ligne, sans caractère de contrôle"
    else Ok(v.Trim())

let raisonPropre (r: string) =
    let r = r.Trim()
    if r = "" then Error "dis en une phrase pourquoi ce changement"
    elif r.Length > raisonMaximum then Error $"raison trop longue ({raisonMaximum} caractères au plus)"
    elif r |> Seq.exists Char.IsControl then Error "la raison tient sur une ligne"
    else Ok r

/// Vérifie chaque champ : une colonne permise, une valeur propre.
let private champsPropres (table: Table) (permises: string list) (champs: (string * string) list) =
    let erreurs = ResizeArray<string>()

    let propres =
        champs
        |> List.choose (fun (col, v) ->
            if not (List.contains col permises) then
                erreurs.Add $"la colonne « {col} » ne se modifie pas dans l'atelier"
                None
            else
                match valeurPropre v with
                | Ok v -> Some(col, v)
                | Error e ->
                    erreurs.Add $"{col} : {e}"
                    None)

    if (champs |> List.map fst |> List.distinct).Length <> champs.Length then erreurs.Add "une colonne en double"
    if erreurs.Count = 0 then Ok propres else Error(String.concat " ; " erreurs)

/// Applique une modification au texte d'une table ; `qui` (l'identifiant du collaborateur) devient
/// la provenance de la ligne. Rend le nouveau texte et un résumé pour le message du commit.
let appliquer (table: Table) (texte: string) (qui: string) (m: Modification) : Result<string * string, string> =
    let index col = table.entete |> List.findIndex ((=) col)
    let valeurDe (l: Ligne) col = l.champs[index col]

    if not (reIdentifiant.IsMatch qui) then
        Error "identifiant de collaborateur invalide"
    else
        match lire texte with
        | Error e -> Error $"{table.chemin} illisible : {e}"
        | Ok [] -> Error $"{table.chemin} est vide"
        | Ok(entete :: lignes) when entete.champs <> table.entete -> Error $"{table.chemin} : l'en-tête n'est pas celui attendu"
        | Ok(entete :: lignes) when lignes |> List.exists (fun l -> l.champs.Length <> table.entete.Length) ->
            Error $"{table.chemin} : une ligne n'a pas le bon nombre de colonnes"
        | Ok(entete :: lignes) ->
            match m with
            | Modifier(cle, champs, avant) ->
                match lignes |> List.tryFindIndex (fun l -> valeurDe l table.cle = cle) with
                | None -> Error $"aucune ligne « {cle} »"
                | Some i ->
                    champsPropres table table.modifiables champs
                    |> Result.bind (fun champs ->
                        let l = lignes[i]

                        // quelqu'un a changé la ligne depuis que le collaborateur l'a lue
                        match avant |> List.tryFind (fun (col, v) -> List.contains col table.entete && valeurDe l col <> v) with
                        | Some(col, _) -> Error $"« {cle} » a changé entre-temps ({col}) : recharge la page"
                        | None ->
                            let changes = champs |> List.filter (fun (col, v) -> valeurDe l col <> v)

                            if changes.IsEmpty then
                                Error "rien ne change"
                            else
                                let nouveaux =
                                    table.entete
                                    |> List.map (fun col ->
                                        if col = table.provenance then qui
                                        else match List.tryFind (fun (c, _) -> c = col) changes with Some(_, v) -> v | None -> valeurDe l col)

                                let lignes = lignes |> List.mapi (fun j x -> if j = i then nouvelleLigne nouveaux else x)
                                let quoi = changes |> List.map fst |> String.concat ", "
                                Ok(ecrire (entete :: lignes), $"{cle} : {quoi}"))
            | Ajouter champs ->
                champsPropres table (table.cle :: table.modifiables) champs
                |> Result.bind (fun champs ->
                    match champs |> List.tryFind (fun (c, _) -> c = table.cle) with
                    | None -> Error $"il manque l'identifiant ({table.cle})"
                    | Some(_, cle) when not (reIdentifiant.IsMatch cle) -> Error "identifiant : minuscules, chiffres et tirets (60 caractères au plus)"
                    | Some(_, cle) when lignes |> List.exists (fun l -> valeurDe l table.cle = cle) -> Error $"« {cle} » existe déjà"
                    | Some(_, cle) ->
                        let nouveaux =
                            table.entete
                            |> List.map (fun col ->
                                if col = table.provenance then qui
                                else match List.tryFind (fun (c, _) -> c = col) champs with Some(_, v) -> v | None -> "")

                        Ok(ecrire (entete :: lignes @ [ nouvelleLigne nouveaux ]), $"{cle} : nouvelle ligne"))

// ---------- Les invitations ----------

let prefixeCode = "atl_"

/// Un code d'invitation a la bonne forme : le préfixe et 43 caractères base64url (256 bits).
let formeCode (code: string) = Regex.IsMatch(code, "^atl_[A-Za-z0-9_-]{43}$")
