/// Une idée envoyée sans compte GitHub : ce que le joueur écrit, ce qu'on en vérifie, ce qu'on en
/// publie, et où elle en est. Pur : aucun appel au réseau, validé par les tests .NET.
module Labo.Relais.Proposition

open System.Text.RegularExpressions

/// Où en est une proposition.
type Statut =
    /// reçue, attend la relecture (dépôt privé)
    | ARelire
    /// relue et publiée : un ticket public existe
    | Publiee
    | ALEtude
    | Integree
    | Ecartee

let codeStatut =
    function
    | ARelire -> "a-relire"
    | Publiee -> "publiee"
    | ALEtude -> "a-l-etude"
    | Integree -> "integree"
    | Ecartee -> "ecartee"

let statutDe =
    function
    | "a-relire" -> Some ARelire
    | "publiee" -> Some Publiee
    | "a-l-etude" -> Some ALEtude
    | "integree" -> Some Integree
    | "ecartee" -> Some Ecartee
    | _ -> None

let libelleStatut =
    function
    | ARelire -> "reçue, en attente de relecture"
    | Publiee -> "relue et publiée"
    | ALEtude -> "à l'étude"
    | Integree -> "intégrée au jeu"
    | Ecartee -> "écartée"

/// Après une décision finale, on n'écrit plus au joueur : son adresse est effacée.
let estFinal s = s = Integree || s = Ecartee

/// Les étiquettes du dépôt public qui font avancer une proposition publiée.
let statutDeEtiquette =
    function
    | "à l'étude" -> Some ALEtude
    | "intégrée" -> Some Integree
    | "écartée" -> Some Ecartee
    | _ -> None

/// L'étiquette du dépôt de relecture qui publie une proposition.
let etiquettePublier = "publier"

/// Les réponses à « D'où vient ce texte ? », comme dans le formulaire GitHub (idee.yml).
let provenances =
    [ "moi", "Je l'ai écrit moi-même"
      "assistant", "Je l'ai écrit avec l'aide d'un assistant (IA)"
      "recopie", "Il est recopié d'ailleurs (la source est dans les appuis)" ]

type Proposition =
    { situation: string
      idee: string
      appuis: string
      provenance: string
      citation: string
      mail: string option }

type Refus =
    | IdeeTropCourte
    | TropLong of champ: string * maximum: int
    | ProvenanceInconnue
    | MailInvalide

let decrireRefus =
    function
    | IdeeTropCourte -> "l'idée est trop courte"
    | TropLong(c, n) -> $"le champ « {c} » dépasse {n} caractères"
    | ProvenanceInconnue -> "dis d'où vient le texte"
    | MailInvalide -> "l'adresse mail ne semble pas valide"

let ideeMinimum = 10

let maximums =
    [ "situation", 200
      "idée", 4000
      "appuis", 4000
      "citation", 60
      "mail", 254 ]

let reMail = Regex("^[^@\\s]+@[^@\\s]+\\.[^@\\s]+$")

/// Nettoie et vérifie une proposition ; tous les refus d'un coup, pour tout corriger en une fois.
let valider (p: Proposition) : Result<Proposition, Refus list> =
    let p =
        { situation = p.situation.Trim()
          idee = p.idee.Trim()
          appuis = p.appuis.Trim()
          provenance = p.provenance.Trim()
          citation = p.citation.Trim()
          mail = p.mail |> Option.map _.Trim() |> Option.filter (fun m -> m <> "") }

    let longueurs =
        [ "situation", p.situation
          "idée", p.idee
          "appuis", p.appuis
          "citation", p.citation
          "mail", defaultArg p.mail "" ]

    let refus =
        [ if p.idee.Length < ideeMinimum then yield IdeeTropCourte
          for (champ, valeur) in longueurs do
              let n = maximums |> List.find (fst >> (=) champ) |> snd
              if valeur.Length > n then yield TropLong(champ, n)
          if not (provenances |> List.exists (fst >> (=) p.provenance)) then yield ProvenanceInconnue
          match p.mail with
          | Some m when not (reMail.IsMatch m) -> yield MailInvalide
          | _ -> () ]

    if refus.IsEmpty then Ok p else Error refus

/// Empêche un texte de joueur de notifier quelqu'un sur GitHub (@nom) : une espace invisible après
/// l'arobase casse la mention sans changer ce qu'on lit.
let neutraliser (t: string) = t.Replace("@", "@​")

/// Un texte de joueur cité dans un ticket : chaque ligne en citation.
let citer (t: string) =
    (neutraliser t).Split('\n') |> Array.map (fun l -> "> " + l.TrimEnd('\r')) |> String.concat "\n"

let titre (p: Proposition) =
    let court = if p.idee.Length > 60 then p.idee.Substring(0, 60).TrimEnd() + "…" else p.idee
    "Idée : " + (neutraliser (court.Replace('\n', ' ')))

let libelleProvenance code =
    provenances |> List.tryFind (fst >> (=) code) |> Option.map snd |> Option.defaultValue code

let signature (p: Proposition) =
    if p.citation = "" then "un joueur" else neutraliser p.citation

/// Le corps commun aux deux tickets : situation, idée, appuis, provenance, signature.
let corps (p: Proposition) =
    [ yield "### La situation du jeu"
      yield (if p.situation = "" then "_non précisée_" else neutraliser p.situation)
      yield ""
      yield "### Son idée"
      yield citer p.idee
      yield ""
      if p.appuis <> "" then
          yield "### Sur quoi elle s'appuie"
          yield citer p.appuis
          yield ""
      yield "### D'où vient ce texte ?"
      yield libelleProvenance p.provenance
      yield ""
      yield "### À citer comme"
      yield signature p ]
    |> String.concat "\n"

/// Le ticket public. `deConfiance` : envoyée avec une clé de confiance, publiée sans relecture.
let corpsPublic (p: Proposition) (deConfiance: bool) =
    let origine =
        if deConfiance then
            "_Envoyée depuis le jeu, sans compte GitHub, par une personne de confiance (publiée sans relecture)._"
        else
            "_Envoyée depuis le jeu, sans compte GitHub, et relue avant publication._"

    origine + "\n\n" + corps p

/// Le ticket du dépôt de relecture (privé) : ajouter l'étiquette `publier` le publie ; le fermer
/// sans elle l'écarte, et le dernier commentaire dit pourquoi au joueur.
let corpsRelecture (p: Proposition) (code: string) =
    [ corps p
      ""
      "---"
      $"Pour publier : étiquette `{etiquettePublier}`. Pour écarter : fermer le ticket ; le dernier commentaire est envoyé au joueur comme raison."
      (if p.mail.IsSome then "Le joueur a laissé une adresse pour être prévenu (elle n'est pas dans ce ticket)." else "Le joueur n'a pas laissé d'adresse.")
      $"Code de suivi : `{code}`" ]
    |> String.concat "\n"

/// Le message envoyé au joueur quand sa proposition change de statut.
let mailStatut (statut: Statut) (ticket: string option) (raison: string option) (pageSuivi: string) =
    let lien = ticket |> Option.map (fun t -> $"\n\nLe ticket public, lisible sans compte : {t}") |> Option.defaultValue ""

    let quoi =
        match statut with
        | ARelire -> "Ta proposition est bien reçue. Elle sera relue avant d'être publiée."
        | Publiee -> "Ta proposition a été relue et publiée."
        | ALEtude -> "Ta proposition est à l'étude."
        | Integree -> "Ta proposition entre dans le jeu. Merci !"
        | Ecartee -> "Ta proposition n'est pas retenue."

    let pourquoi =
        match raison with
        | Some r when r.Trim() <> "" -> "\n\nPourquoi :\n" + r.Trim()
        | _ -> ""

    let fin =
        if estFinal statut then
            "\n\nC'est le dernier message : ton adresse vient d'être effacée."
        else
            $"\n\nOù en est ta proposition, et pour ne plus recevoir ces messages : {pageSuivi}"

    let objet = $"Le Labo de la Bande : ta proposition est {libelleStatut statut}"
    objet, "Bonjour,\n\n" + quoi + pourquoi + lien + fin + "\n\nLe Labo de la Bande"

// ---------- Un prénom déclaré par un joueur ----------

/// Ce qu'un joueur déclare pour les banques de prénoms : son prénom, sa décennie de naissance, son
/// pays de naissance, et l'usage du prénom autour de lui (pas son genre à lui). Le relais n'en
/// garde que des comptes agrégés.
type Declaration =
    { prenom: string
      decennie: int
      pays: string
      /// `f` : donné aux filles, `m` : aux garçons, `fm` : aux deux, `x` : je ne sais pas
      usage: string }

let usages =
    [ "f", "aux filles"
      "m", "aux garçons"
      "fm", "aux deux"
      "x", "je ne sais pas" ]

let decennies = [ 1920..10..2020 ]

/// « MARIE-CLAIRE » ou « marie-claire » → « Marie-Claire » ; une casse mêlée est gardée telle quelle.
let casseDeclaree (p: string) =
    if p = p.ToUpperInvariant() || p = p.ToLowerInvariant() then
        let bas = p.ToLowerInvariant()
        let sb = System.Text.StringBuilder()
        let mutable debut = true

        for c in bas do
            sb.Append(if debut then System.Char.ToUpperInvariant c else c) |> ignore
            debut <- c = '-' || c = ' ' || c = '\'' || c = '’'

        sb.ToString()
    else
        p

let caractereDePrenom (c: char) =
    System.Char.IsLetter c || c = ' ' || c = '-' || c = '\'' || c = '’'

/// Nettoie et vérifie une déclaration.
let validerDeclaration (d: Declaration) : Result<Declaration, string list> =
    let prenom = System.String.Join(" ", d.prenom.Trim().Split(' ', System.StringSplitOptions.RemoveEmptyEntries))
    let pays = d.pays.Trim().ToLowerInvariant()

    let refus =
        [ if prenom.Length < 2 || prenom.Length > 40 then yield "le prénom doit faire entre 2 et 40 caractères"
          if not (prenom |> Seq.forall caractereDePrenom) then yield "le prénom ne peut contenir que des lettres, des espaces, des traits d'union et des apostrophes"
          if not (List.contains d.decennie decennies) then yield "choisis ta décennie de naissance"
          if pays.Length <> 2 || not (pays |> Seq.forall (fun c -> c >= 'a' && c <= 'z')) then yield "choisis ton pays de naissance"
          if not (usages |> List.exists (fst >> (=) d.usage)) then yield "dis à qui ce prénom se donne autour de toi" ]

    if refus.IsEmpty then
        Ok { d with prenom = casseDeclaree prenom; pays = pays }
    else
        Error refus

/// La clé KV d'un compte agrégé : `decl:<pays>:<décennie>:<usage>:<prénom>`.
let cleDeclaration (d: Declaration) = $"decl:{d.pays}:{d.decennie}:{d.usage}:{d.prenom}"

let declarationDeCle (cle: string) =
    match cle.Split(':', 5) with
    | [| "decl"; pays; decennie; usage; prenom |] ->
        match System.Int32.TryParse decennie with
        | true, d -> Some { prenom = prenom; decennie = d; pays = pays; usage = usage }
        | _ -> None
    | _ -> None
