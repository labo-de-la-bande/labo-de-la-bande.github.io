/// Le F# généré depuis data/ dans src/Labo.Data/Generated/. Le texte est déterministe (fins de
/// ligne `\n`, lignes dans l'ordre des CSV) : un fichier est à jour quand il est, à l'octet près,
/// ce que `labo generate` écrirait.
module Labo.Cli.Generate

open System
open System.Globalization
open System.IO
open System.Text
open Labo.Data
open Labo.Cli.Validate

let chaine (s: string) =
    let b = StringBuilder "\""

    for c in s do
        match c with
        | '\\' -> b.Append "\\\\"
        | '"' -> b.Append "\\\""
        | '\n' -> b.Append "\\n"
        | '\r' -> b.Append "\\r"
        | '\t' -> b.Append "\\t"
        | c -> b.Append c
        |> ignore

    b.Append('"').ToString()

let nombre (v: float) =
    let s = v.ToString("R", CultureInfo.InvariantCulture)
    if s.Contains '.' || s.Contains 'E' then s else s + ".0"

let option f =
    function
    | None -> "None"
    | Some v -> $"Some {f v}"

let liste f xs =
    match xs with
    | [] -> "[]"
    | xs -> "[ " + String.Join("; ", xs |> List.map f) + " ]"

let booleen b = if b then "true" else "false"

/// Un enregistrement F#, un champ par ligne, indenté sous `[`.
let enregistrement (champs: (string * string) list) =
    let corps =
        champs
        |> List.mapi (fun i (k, v) -> (if i = 0 then "      { " else "        ") + $"{k} = {v}")

    String.Join("\n", corps) + " }"

let fichier (sources: string list) (moduleNom: string) (ouvertures: string list) (corps: string) =
    let entete =
        $"""// GÉNÉRÉ depuis {String.Join(", ", sources |> List.map (fun s -> "data/" + s))} par `labo generate`.
// Ne pas éditer : changer les données et régénérer.
module {moduleNom}
"""

    let opens = String.Join("", ouvertures |> List.map (fun o -> $"\nopen {o}"))
    entete + opens + "\n\n" + corps + "\n"

let valeursListe (nom: string) (typeNom: string) (items: string list) =
    if items.IsEmpty then
        $"let {nom} : {typeNom} list = []"
    else
        // un tableau converti en liste : une longue liste littérale s'imbrique élément par élément
        // et fait déborder la pile de Fable ; un tableau reste plat
        $"let {nom} : {typeNom} list =\n    List.ofArray [|\n" + String.Join("\n", items) + "\n    |]"

let atome (a: Atome) =
    $"{{ levier = {chaine a.levier}; egal = {booleen a.egal}; options = {liste chaine a.options} }}"

let sourcesFs (d: Donnees) =
    d.sources
    |> List.map (fun s ->
        enregistrement
            [ "id", chaine s.id
              "titre", chaine s.titre
              "url", chaine s.url
              "producteur", chaine s.producteur
              "genre", chaine s.genre
              "methode", chaine s.methode
              "limites", chaine s.limites
              "doi", chaine s.doi
              "verifieLe", chaine s.verifieLe
              "revoir", chaine s.revoir
              "vigilance", chaine s.vigilance
              "documents", liste (fun (t, u) -> $"({chaine t}, {chaine u})") s.documents ])
    |> valeursListe "tous" "Source"
    |> fichier [ "sources.csv"; "sources-documents.csv" ] "Labo.Data.Sources" [ "Labo.Data" ]

let jaugesFs (d: Donnees) =
    d.jauges
    |> List.map (fun j ->
        enregistrement
            [ "id", chaine j.id
              "libelle", chaine j.libelle
              "ancre", chaine j.ancre
              "horsScore", booleen j.horsScore
              "seuilVital", option nombre j.seuilVital
              "solide", option nombre j.solide
              "solideOuvert", option nombre j.solideOuvert
              "solideFrance", option nombre j.solideFrance
              "aide", chaine j.aide ])
    |> valeursListe "tous" "Jauge"
    |> fichier [ "jauges.csv" ] "Labo.Data.Jauges" [ "Labo.Data" ]

/// `crime-peu-d-ecart` → `crimePeuDEcart` : le nom F# d'un coefficient ou d'une constante.
let nomFs (id: string) =
    let parts = id.Split '-'

    parts[0]
    + String.Join("", parts |> Array.skip 1 |> Array.map (fun p -> if p = "" then "" else string (Char.ToUpperInvariant p[0]) + p.Substring 1))

let statutFs =
    function
    | Sourcé -> "Sourcé"
    | Hypothèse -> "Hypothèse"
    | Jugement -> "Jugement"
    | Témoignage -> "Témoignage"
    | DeMémoire -> "DeMémoire"
    | ADocumenter -> "ADocumenter"

/// Chaque valeur nommée, puis la liste complète (pour l'affichage et la critique).
let valeursNommees (items: (string * float * string) list) =
    String.Join("\n\n", items |> List.map (fun (id, v, doc) -> $"/// {doc.Replace('\n', ' ')}\nlet {nomFs id} = {nombre v}"))

let paysFrFs (d: Donnees) =
    let jauges =
        d.jaugesFrance
        |> List.map (fun v ->
            enregistrement
                [ "jauge", chaine v.jauge
                  "valeur", option nombre v.valeur
                  "texte", chaine v.texte
                  "date", chaine v.date
                  "revoir", chaine v.revoir ])
        |> valeursListe "jauges" "ValeurPays"

    let constantes =
        d.constantesFrance
        |> List.map (fun c ->
            enregistrement
                [ "id", chaine c.id
                  "valeur", nombre c.valeur
                  "unite", chaine c.unite
                  "statut", statutFs c.statut
                  "sources", liste chaine c.sources
                  "description", chaine c.description ])
        |> valeursListe "constantes" "Constante"

    let nommees =
        d.constantesFrance |> List.map (fun c -> c.id, c.valeur, $"{c.description} ({c.unite})") |> valeursNommees

    let ressources =
        d.ressourcesFrance
        |> List.map (fun r ->
            enregistrement
                [ "id", chaine r.id
                  "nom", chaine r.nom
                  "metropole", chaine r.metropole
                  "franceEntiere", chaine r.franceEntiere
                  "importeeDes", option string r.importeeDes
                  "regleePar", option chaine r.regleePar
                  "pourquoi", chaine r.pourquoi ])
        |> valeursListe "ressources" "RessourcePays"

    let options =
        d.ressourcesOptionsFrance
        |> List.map (fun o ->
            enregistrement
                [ "levier", chaine o.levier
                  "option", chaine o.option
                  "couverture", chaine o.couverture
                  "importe", booleen o.importe
                  "cout", chaine o.cout
                  "effet", chaine o.effet ])
        |> valeursListe "ressourcesOptions" "OptionRessource"

    let importations =
        d.importationsFrance
        |> List.map (fun i ->
            enregistrement
                [ "ouverture", string i.ouverture
                  "rang", string i.rang
                  "libelle", chaine i.libelle
                  "valeur", nombre i.valeur
                  "note", chaine i.note ])
        |> valeursListe "importations" "Importation"

    let exportations =
        d.exportationsFrance
        |> List.map (fun x ->
            enregistrement
                [ "id", chaine x.id
                  "libelle", chaine x.libelle
                  "parDefaut", booleen x.parDefaut
                  "valeur",
                  (match x.valeur with
                   | Fixe v -> $"Fixe {nombre v}"
                   | Calcul c -> $"Calcul {chaine c}")
                  "note", chaine x.note ])
        |> valeursListe "exportations" "Exportation"

    // la banque de prénoms en texte compact, lue au démarrage : 17 000 enregistrements écrits un par
    // un alourdiraient beaucoup la page
    let prenoms =
        let lignes = d.prenomsFrance |> List.map (fun p -> $"{p.region};{p.sexe};{p.decennie};{p.prenom};{p.naissances}")

        "/// La banque de prénoms (région;sexe;décennie;prénom;naissances).\nlet prenomsTexte =\n    \"\"\""
        + String.Join("\n", lignes)
        + "\"\"\"\n\nlet prenoms: Prenom list =\n    prenomsTexte.Split('\\n')\n    |> Array.map (fun l ->\n        let c = l.Split(';')\n        { region = c[0]; sexe = c[1]; decennie = int c[2]; prenom = c[3]; naissances = int c[4] })\n    |> List.ofArray"

    let regions =
        d.regionsFrance
        |> List.map (fun r ->
            enregistrement
                [ "id", chaine r.id
                  "nom", chaine r.nom
                  "outreMer", booleen r.outreMer
                  "departements", liste chaine r.departements ])
        |> valeursListe "regions" "Region"

    let origines =
        d.originesFrance
        |> List.map (fun o ->
            enregistrement
                [ "id", chaine o.id
                  "nom", chaine o.nom
                  "effectif", nombre o.effectif
                  "ages", liste nombre o.ages
                  "pays", liste chaine o.pays
                  "statut", statutFs o.statut
                  "sources", liste chaine o.sources ])
        |> valeursListe "origines" "Origine"

    String.Join("\n\n", [ jauges; constantes; nommees; ressources; options; importations; exportations; regions; origines; prenoms ])
    |> fichier
        [ "pays/fr/jauges.csv"
          "pays/fr/constantes.csv"
          "pays/fr/ressources.csv"
          "pays/fr/ressources-options.csv"
          "pays/fr/importations.csv"
          "pays/fr/exportations.csv"
          "pays/fr/regions.csv"
          "pays/fr/origines.csv"
          "pays/fr/prenoms.csv" ]
        "Labo.Data.Pays.Fr"
        [ "Labo.Data" ]

/// Les banques de prénoms par pays, en texte compact (pays;nom;sexe;prénom;poids).
let prenomsFs (d: Donnees) =
    let lignes =
        d.prenomsPays
        |> List.map (fun p -> String.concat ";" [ p.pays; p.nomPays; p.sexe; p.prenom; p.poids.ToString(System.Globalization.CultureInfo.InvariantCulture) ])

    "/// Les prénoms les plus portés dans chaque pays (pays;nom du pays;sexe;prénom;pour mille).\nlet prenomsTexte =\n    \"\"\""
    + String.Join("\n", lignes)
    + "\"\"\"\n\nlet prenoms: PrenomPays list =\n    prenomsTexte.Split('\\n')\n    |> Array.map (fun l ->\n        let c = l.Split(';')\n        { pays = c[0]; nomPays = c[1]; sexe = c[2]; prenom = c[3]; poids = float c[4] })\n    |> List.ofArray"
    |> fun pays ->
        let joueurs =
            d.prenomsJoueurs
            |> List.map (fun p ->
                enregistrement
                    [ "pays", chaine p.pays
                      "decennie", string p.decennie
                      "usage", chaine p.usage
                      "prenom", chaine p.prenom
                      "declarations", string p.declarations
                      "valide", booleen p.valide ])
            |> valeursListe "joueurs" "PrenomJoueur"

        let monde =
            "/// Les pays du monde (code ISO, nom anglais) : la page en affiche le nom dans la langue du joueur.\nlet paysDuMonde : (string * string) list =\n    [ "
            + String.Join("\n      ", d.paysDuMonde |> List.map (fun (c, n) -> $"{chaine c}, {chaine n}"))
            + " ]"

        String.Join("\n\n", [ pays; joueurs; monde ])
    |> fichier [ "prenoms/pays.csv"; "prenoms/joueurs.csv"; "pays-du-monde.csv" ] "Labo.Data.Prenoms" [ "Labo.Data" ]

let coefficientsFs (d: Donnees) =
    let nommees =
        d.coefficients |> List.map (fun c -> c.id, c.valeur, $"{c.raison} ({statutFs c.statut}, {c.lentille})") |> valeursNommees

    let tous =
        d.coefficients
        |> List.map (fun c ->
            enregistrement
                [ "id", chaine c.id
                  "valeur", nombre c.valeur
                  "statut", statutFs c.statut
                  "lentille", chaine c.lentille
                  "lentillePar", chaine c.lentillePar
                  "sources", liste chaine c.sources
                  "raison", chaine c.raison ])
        |> valeursListe "tous" "Coefficient"

    fichier [ "coefficients.csv" ] "Labo.Data.Coefficients" [ "Labo.Data" ] (nommees + "\n\n" + tous)

let modeleFs (d: Donnees) =
    let lentilles =
        d.lentilles
        |> List.map (fun l ->
            enregistrement
                [ "id", chaine l.id
                  "nom", chaine l.nom
                  "penseurs", chaine l.penseurs
                  "description", chaine l.description
                  "proposePar", chaine l.proposePar ])
        |> valeursListe "lentilles" "Lentille"

    let curseurs =
        d.curseurs
        |> List.map (fun c ->
            enregistrement
                [ "id", chaine c.id
                  "defaut", option nombre c.defaut
                  "depart", option nombre c.depart
                  "description", chaine c.description ])
        |> valeursListe "curseurs" "Curseur"

    let attaques =
        d.attaques
        |> List.map (fun a ->
            enregistrement
                [ "id", chaine a.id
                  "attaque", chaine a.attaque
                  "verdict", chaine a.verdict
                  "verdictAvant", option chaine a.verdictAvant
                  "pourquoi", chaine a.pourquoi ])
        |> valeursListe "attaques" "Attaque"

    let maturites =
        d.maturites
        |> List.map (fun m -> enregistrement [ "id", chaine m.id; "libelle", chaine m.libelle; "rang", string m.rang ])
        |> valeursListe "maturites" "Maturite"

    let paris =
        d.paris
        |> List.map (fun p ->
            enregistrement
                [ "id", chaine p.id
                  "categorie", chaine p.categorie
                  "titre", chaine p.titre
                  "maturite", chaine p.maturite
                  "pretDans", string p.pretDans
                  "confort", nombre p.confort
                  "energie", nombre p.energie
                  "rendement", nombre p.rendement
                  "besoin", nombre p.besoin
                  "captation", nombre p.captation
                  "couvre", option (fun (r, c) -> $"({chaine r}, {chaine c})") p.couvre
                  "avecMonnaie", booleen p.avecMonnaie
                  "statut", statutFs p.statut
                  "sources", liste chaine p.sources
                  "description", chaine p.description
                  "effet", chaine p.effet
                  "echec", chaine p.echec ])
        |> valeursListe "paris" "Pari"

    let faiblesses =
        d.faiblesses
        |> List.map (fun f ->
            enregistrement
                [ "id", chaine f.id
                  "titre", chaine f.titre
                  "probleme", chaine f.probleme
                  "fait", chaine f.fait
                  "statut", statutFs f.statut
                  "sources", liste chaine f.sources ])
        |> valeursListe "faiblesses" "Faiblesse"

    let parades =
        d.parades
        |> List.map (fun p ->
            enregistrement
                [ "faiblesse", chaine p.faiblesse
                  "rang", string p.rang
                  "id", chaine p.id
                  "libelle", chaine p.libelle
                  "adoptee", booleen p.adoptee
                  "captation", nombre p.captation
                  "automatique", option (fun a -> $"%A{a}") p.automatique
                  "automatiqueTexte", chaine p.automatiqueTexte ])
        |> valeursListe "parades" "Parade"

    let prevention =
        d.prevention
        |> List.map (fun p ->
            enregistrement
                [ "levier", chaine p.levier
                  "facteur", nombre p.facteur
                  "statut", statutFs p.statut
                  "sources", liste chaine p.sources
                  "raison", chaine p.raison
                  "contre", chaine p.contre
                  "contreSource", option chaine p.contreSource ])
        |> valeursListe "prevention" "Prevention"

    String.Join("\n\n", [ lentilles; curseurs; attaques; maturites; paris; faiblesses; parades; prevention ])
    |> fichier
        [ "lentilles.csv"
          "curseurs.csv"
          "stress.csv"
          "maturites.csv"
          "paris.csv"
          "faiblesses.csv"
          "parades.csv"
          "prevention.csv" ]
        "Labo.Data.Modele"
        [ "Labo.Data" ]

let genre =
    function
    | Choix -> "Choix"
    | OuiNon -> "OuiNon"
    | Curseur -> "Curseur"
    | Prereglage -> "Prereglage"

let ecriture =
    function
    | Champ(c, v) -> $"Champ({chaine c}, {chaine v})"
    | Nomme n -> $"Nomme {chaine n}"

let leviersFs (d: Donnees) =
    let leviers =
        d.leviers
        |> List.map (fun v ->
            enregistrement
                [ "id", chaine v.id
                  "titre", chaine v.titre
                  "ancre", chaine v.ancre
                  "genre", genre v.genre
                  "champ", chaine v.champ
                  "defaut", option chaine v.defaut
                  "min", option nombre v.min
                  "max", option nombre v.max
                  "pas", option nombre v.pas ])
        |> valeursListe "tous" "Levier"

    let options =
        d.options
        |> List.map (fun o ->
            enregistrement
                [ "levier", chaine o.levier
                  "rang", string o.rang
                  "id", chaine o.id
                  "libelle", chaine o.libelle
                  "note", chaine o.note
                  "valeur", chaine o.valeur
                  "ecritAussi", liste ecriture o.ecritAussi ])
        |> valeursListe "options" "OptionLevier"

    fichier [ "leviers.csv"; "options.csv" ] "Labo.Data.Leviers" [ "Labo.Data" ] (leviers + "\n\n" + options)

let statut =
    function
    | Sourcé -> "Sourcé"
    | Hypothèse -> "Hypothèse"
    | Jugement -> "Jugement"
    | Témoignage -> "Témoignage"
    | DeMémoire -> "DeMémoire"
    | ADocumenter -> "ADocumenter"

let poids =
    function
    | Plein -> "Plein"
    | Si(c, f) -> $"Si({liste atome c}, {nombre f})"
    | Facteur n -> $"Facteur {chaine n}"

let liensFs (d: Donnees) =
    d.liens
    |> List.map (fun l ->
        enregistrement
            [ "id", chaine l.id
              "jauge", chaine l.jauge
              "effet", nombre l.effet
              "statut", statut l.statut
              "lentille", chaine l.lentille
              "lentillePar", chaine l.lentillePar
              "nature", $"%A{l.nature}"
              "raison", chaine l.raison
              "source", option chaine l.source
              "contreSource", option chaine l.contreSource
              "quand", liste atome l.quand
              "poids", poids l.poids
              "courbe", option (fun (c: Courbe) -> $"{{ nom = {chaine c.nom}; args = {liste chaine c.args} }}") l.courbe ])
    |> valeursListe "tous" "Lien"
    |> fichier [ "liens.csv" ] "Labo.Data.Liens" [ "Labo.Data" ]

let jeuFs (d: Donnees) =
    let tranches =
        d.tranches
        |> List.map (fun t ->
            enregistrement
                [ "id", chaine t.id
                  "rang", string t.rang
                  "titre", chaine t.titre
                  "sousTitre", chaine t.sousTitre
                  "couleur", chaine t.couleur
                  "embleme", chaine t.embleme
                  "jauges", liste chaine t.jauges
                  "provenance", chaine t.provenance ])
        |> valeursListe "tranches" "Tranche"

    let situations =
        d.situations
        |> List.map (fun s ->
            enregistrement
                [ "id", chaine s.id
                  "tranche", chaine s.tranche
                  "rang", string s.rang
                  "genre", $"%A{s.genre}"
                  "leviers", liste chaine s.leviers
                  "paris", liste chaine s.paris
                  "titre", chaine s.titre
                  "texte", chaine s.texte
                  "embleme", chaine s.embleme
                  "quand", liste atome s.quand
                  "provenance", chaine s.provenance ])
        |> valeursListe "situations" "Situation"

    let chiffres =
        d.chiffres
        |> List.map (fun c ->
            enregistrement
                [ "situation", chaine c.situation
                  "rang", string c.rang
                  "valeur", chaine c.valeur
                  "legende", chaine c.legende
                  "sources", liste chaine c.sources
                  "provenance", chaine c.provenance ])
        |> valeursListe "chiffres" "Chiffre"

    let voix =
        d.voix
        |> List.map (fun v ->
            enregistrement
                [ "situation", chaine v.situation
                  "rang", string v.rang
                  "habitant", chaine v.habitant
                  "precision", chaine v.precision
                  "texte", chaine v.texte
                  "option", chaine v.option
                  "position", $"%A{v.position}"
                  "provenance", chaine v.provenance
                  "positionPar", chaine v.positionPar ])
        |> valeursListe "voix" "Voix"

    let habitants =
        d.habitants
        |> List.map (fun h ->
            enregistrement
                [ "id", chaine h.id
                  "nom", chaine h.nom
                  "age", string h.age
                  "sexe", chaine h.sexe
                  "portrait", chaine h.portrait
                  "provenance", chaine h.provenance
                  "naissance", chaine h.naissance ])
        |> valeursListe "habitants" "Habitant"

    let evenements =
        d.evenements
        |> List.map (fun x ->
            enregistrement
                [ "id", chaine x.id
                  "tranche", option chaine x.tranche
                  "titre", chaine x.titre
                  "texte", chaine x.texte
                  "quand", liste atome x.quand
                  "siJauge", liste (fun (j, c, s) -> $"({chaine j}, %A{c}, {nombre s})") x.siJauge
                  "probabilite", nombre x.probabilite
                  "uneFois", booleen x.uneFois
                  "effets", liste (fun (f: EffetTemps) -> $"{{ jauge = {chaine f.jauge}; points = {nombre f.points}; duree = {f.duree} }}") x.effets
                  "reputation", liste (fun (h, v) -> $"({chaine h}, {nombre v})") x.reputation
                  "statut", statutFs x.statut
                  "sources", liste chaine x.sources
                  "provenance", chaine x.provenance ])
        |> valeursListe "evenements" "Evenement"

    let repliques =
        d.repliques
        |> List.map (fun x ->
            enregistrement
                [ "situation", chaine x.situation
                  "id", chaine x.id
                  "rang", string x.rang
                  "moment", $"%A{x.moment}"
                  "option", option chaine x.option
                  "locuteur", chaine x.locuteur
                  "nature", $"%A{x.nature}"
                  "texte", chaine x.texte
                  "sources", liste chaine x.sources
                  "lien", option chaine x.lien
                  "suite", liste chaine x.suite
                  "action", option (fun a -> $"%A{a}") x.action
                  "siConfiance", liste (fun (h, c, s) -> $"({chaine h}, %A{c}, {nombre s})") x.siConfiance
                  "confiance", liste (fun (h, v) -> $"({chaine h}, {nombre v})") x.confiance
                  "provenance", chaine x.provenance ])
        |> valeursListe "repliques" "Replique"

    let attitudes =
        d.attitudes
        |> List.map (fun a ->
            enregistrement
                [ "habitant", chaine a.habitant
                  "humeur", $"%A{a.humeur}"
                  "rang", string a.rang
                  "texte", chaine a.texte
                  "provenance", chaine a.provenance ])
        |> valeursListe "attitudes" "Attitude"

    let antecedents =
        d.antecedents
        |> List.map (fun x ->
            enregistrement
                [ "habitant", chaine x.habitant
                  "role", chaine x.role
                  "texte", chaine x.texte
                  "provenance", chaine x.provenance ])
        |> valeursListe "antecedents" "Antecedent"

    let typesPartie =
        d.typesPartie
        |> List.map (fun x ->
            enregistrement
                [ "id", chaine x.id
                  "titre", chaine x.titre
                  "description", chaine x.description
                  "provenance", chaine x.provenance ])
        |> valeursListe "typesPartie" "TypePartie"

    let roles =
        d.roles
        |> List.map (fun x ->
            enregistrement
                [ "typePartie", chaine x.typePartie
                  "habitant", chaine x.habitant
                  "role", chaine x.role
                  "depuis", string x.depuis
                  "jusqua", option string x.jusqua
                  "titre", chaine x.titre
                  "description", chaine x.description
                  "acces", chaine x.acces
                  "provenance", chaine x.provenance ])
        |> valeursListe "roles" "Role"

    let attachements =
        d.attachements
        |> List.map (fun a ->
            enregistrement
                [ "habitant", chaine a.habitant
                  "jauge", chaine a.jauge
                  "poids", nombre a.poids
                  "raison", chaine a.raison
                  "provenance", chaine a.provenance
                  "validePar", chaine a.validePar ])
        |> valeursListe "attachements" "Attachement"

    let idees =
        d.idees
        |> List.map (fun i ->
            enregistrement
                [ "id", chaine i.id
                  "ticket", string i.ticket
                  "titre", chaine i.titre
                  "par", chaine i.par
                  "statut", $"%A{i.statut}"
                  "levier", chaine i.levier
                  "option", chaine i.option
                  "reponse", chaine i.reponse ])
        |> valeursListe "idees" "IdeeJoueur"

    String.Join("\n\n", [ habitants; tranches; situations; chiffres; voix; evenements; repliques; attitudes; idees; attachements; typesPartie; roles; antecedents ])
    |> fichier
        [ "jeu/habitants.csv"
          "jeu/tranches.csv"
          "jeu/situations.csv"
          "jeu/chiffres.csv"
          "jeu/voix.csv"
          "jeu/evenements.csv"
          "jeu/repliques.csv"
          "jeu/attitudes.csv"
          "jeu/idees.csv"
          "jeu/attachements.csv"
          "jeu/types-partie.csv"
          "jeu/roles.csv"
          "jeu/antecedents.csv" ]
        "Labo.Data.Jeu"
        [ "Labo.Data" ]

/// Chaque fichier généré : chemin relatif au dépôt, contenu.
let sorties (d: Donnees) =
    [ "src/Labo.Data/Generated/Sources.fs", sourcesFs d
      "src/Labo.Data/Generated/Jauges.fs", jaugesFs d
      "src/Labo.Data/Generated/Prenoms.fs", prenomsFs d
      "src/Labo.Data/Generated/Pays.Fr.fs", paysFrFs d
      "src/Labo.Data/Generated/Leviers.fs", leviersFs d
      "src/Labo.Data/Generated/Liens.fs", liensFs d
      "src/Labo.Data/Generated/Modele.fs", modeleFs d
      "src/Labo.Data/Generated/Coefficients.fs", coefficientsFs d
      "src/Labo.Data/Generated/Jeu.fs", jeuFs d ]

type Etat =
    | AJour
    | Obsolete
    | Absent

let etat (chemin: string) (contenu: string) =
    let p = Repo.path [ chemin ]

    if not (File.Exists p) then Absent
    elif File.ReadAllText p = contenu then AJour
    else Obsolete
