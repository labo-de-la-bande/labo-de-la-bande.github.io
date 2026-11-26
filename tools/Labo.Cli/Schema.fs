/// Les fichiers de data/ : leur en-tête exact, et la lecture d'une table en lignes nommées.
module Labo.Cli.Schema

open System.IO

type Fichier = { chemin: string; entete: string list }

let sources =
    { chemin = "sources.csv"
      entete = [ "id"; "titre"; "url"; "producteur"; "type"; "methode"; "limites"; "doi"; "verifie_le"; "revoir"; "vigilance_fr"; "transposition"; "transposition_par" ] }

let sourcesDocuments =
    { chemin = "sources-documents.csv"
      entete = [ "source"; "rang"; "titre"; "url" ] }

let jauges =
    { chemin = "jauges.csv"
      entete =
        [ "id"
          "libelle_fr"
          "ancre"
          "hors_score"
          "seuil_vital"
          "solide"
          "solide_ouvert"
          "solide_france"
          "aide_fr" ] }

let jaugesFrance =
    { chemin = "pays/fr/jauges.csv"
      entete = [ "jauge"; "valeur"; "texte_fr"; "date"; "revoir_fr" ] }

let leviers =
    { chemin = "leviers.csv"
      entete = [ "id"; "titre_fr"; "ancre"; "genre"; "champ"; "defaut"; "min"; "max"; "pas" ] }

let options =
    { chemin = "options.csv"
      entete = [ "levier"; "rang"; "id"; "libelle_fr"; "note_fr"; "valeur"; "ecrit_aussi" ] }

let liens =
    { chemin = "liens.csv"
      entete =
        [ "id"
          "jauge"
          "effet"
          "statut"
          "lentille"
          "lentille_par"
          "nature"
          "raison_fr"
          "source"
          "contre_source"
          "quand"
          "poids"
          "courbe" ] }

let fichier chemin (entete: string) =
    { chemin = chemin
      entete = entete.Split ',' |> List.ofArray }

let lentilles = fichier "lentilles.csv" "id,nom_fr,penseurs_fr,description_fr,propose_par"
let curseurs = fichier "curseurs.csv" "id,defaut,depart,description_fr"
let stress = fichier "stress.csv" "id,attaque_fr,verdict,verdict_avant,pourquoi_fr"
let maturites = fichier "maturites.csv" "id,libelle_fr,rang"
let quiz = fichier "quiz.csv" "id,affirmation_fr,juste,explication_fr,origine_fr,sources,statut"
let monnaieEpreuves = fichier "monnaie-epreuves.csv" "id,libelle_fr,verdict,note_fr"
let monnaies = fichier "monnaies.csv" "monnaie,court_fr,distance_fr"
let monnaieVerdicts = fichier "monnaie-verdicts.csv" "monnaie,epreuve,verdict,note_fr"
let monnaieCouts = fichier "monnaie-couts.csv" "monnaie,rang,texte_fr"
let heritage = fichier "heritage.csv" "option,description_fr,egalite_fr"
let heritageMonnaies = fichier "heritage-monnaies.csv" "heritage,monnaie,texte_fr"

let paris =
    fichier
        "paris.csv"
        "id,categorie_fr,titre_fr,maturite,pret_dans,confort,energie,rendement,besoin,captation,couvre,couverture,avec_monnaie,statut,sources,description_fr,effet_fr,echec_fr"

let faiblesses = fichier "faiblesses.csv" "id,titre_fr,probleme_fr,fait_fr,statut,sources"
let parades = fichier "parades.csv" "faiblesse,rang,id,libelle_fr,adoptee,captation,automatique,automatique_fr"
let prevention = fichier "prevention.csv" "levier,facteur,statut,sources,raison_fr,contre_fr,contre_source"
let coefficients = fichier "coefficients.csv" "id,valeur,statut,lentille,lentille_par,sources,raison_fr"
let constantesFrance = fichier "pays/fr/constantes.csv" "id,valeur,unite,statut,sources,description_fr"

let ressourcesFrance =
    fichier "pays/fr/ressources.csv" "id,nom_fr,metropole,france_entiere,importee_des,reglee_par,pourquoi_fr"

let ressourcesOptionsFrance =
    fichier "pays/fr/ressources-options.csv" "levier,option,couverture,importe,cout_fr,effet_fr"

let importationsFrance = fichier "pays/fr/importations.csv" "ouverture,rang,libelle_fr,valeur,note_fr"
let exportationsFrance = fichier "pays/fr/exportations.csv" "id,libelle_fr,par_defaut,valeur,calcul,note_fr"
let prenomsFrance = fichier "pays/fr/prenoms.csv" "region,sexe,decennie,prenom,naissances"
let regionsFrance = fichier "pays/fr/regions.csv" "id,nom_fr,outre_mer,departements,code_insee"

let prenomsPays = fichier "prenoms/pays.csv" "pays,nom_fr,sexe,prenom,pour_mille"
let prenomsJoueurs = fichier "prenoms/joueurs.csv" "pays,decennie,usage,prenom,declarations,valide_par"
let paysDuMonde = fichier "pays-du-monde.csv" "code,nom_en"

let originesFrance =
    fichier "pays/fr/origines.csv" "id,nom_fr,effectif_milliers,age_0_17,age_18_44,age_45_64,age_65_plus,pays,statut,sources,raison_fr"
let actionsDefense =
    fichier "defense/actions.csv" "scenario,id,rang,famille,titre_fr,cout_fr,precedent_fr,sources,provenance,valide_par"
let mesuresDefense =
    fichier "defense/mesures.csv" "scenario,id,rang,titre_fr,precedent_fr,sources,statut,provenance,valide_par"
let convoitisesFrance =
    fichier "pays/fr/convoitises.csv" "region,id,nature,texte_fr,sources,statut,date,revoir,provenance,valide_par"

let tranches = fichier "jeu/tranches.csv" "id,rang,titre_fr,sous_titre_fr,couleur,embleme,jauges,provenance"
let situations = fichier "jeu/situations.csv" "id,tranche,rang,genre,leviers,paris,titre_fr,texte_fr,embleme,quand,provenance"
let chiffres = fichier "jeu/chiffres.csv" "situation,rang,valeur_fr,legende_fr,sources,provenance"

let voix =
    fichier "jeu/voix.csv" "situation,rang,habitant,precision_fr,texte_fr,option,position,provenance,position_par"

let habitants = fichier "jeu/habitants.csv" "id,nom,age,sexe,portrait_fr,provenance,naissance"

let repliques =
    fichier "jeu/repliques.csv" "situation,id,rang,moment,option,locuteur,nature,texte_fr,sources,lien,suite,action,si_confiance,confiance,provenance"

let attitudes = fichier "jeu/attitudes.csv" "habitant,humeur,rang,texte_fr,provenance"
let antecedents = fichier "jeu/antecedents.csv" "habitant,role_fr,texte_fr,provenance"
let typesPartie = fichier "jeu/types-partie.csv" "id,titre_fr,description_fr,provenance"
let roles = fichier "jeu/roles.csv" "type,habitant,role,depuis,jusqua,titre_fr,description_fr,acces_fr,provenance"
let attachements = fichier "jeu/attachements.csv" "habitant,jauge,poids,raison_fr,provenance,valide_par"
let idees = fichier "jeu/idees.csv" "id,ticket,titre_fr,par,statut,levier,option,reponse_fr"
let rumeurs = fichier "jeu/rumeurs.csv" "id,theme,affirmation_fr,qui_fr,quand,ou_fr,verdict,correction_fr,sources,statut,provenance"

let evenements =
    fichier
        "jeu/evenements.csv"
        "id,tranche,titre_fr,texte_fr,quand,si_jauge,probabilite,une_fois,effets,reputation,statut,sources,provenance"

let tous =
    [ sources
      sourcesDocuments
      jauges
      jaugesFrance
      leviers
      options
      liens
      lentilles
      curseurs
      stress
      maturites
      quiz
      monnaieEpreuves
      monnaies
      monnaieVerdicts
      monnaieCouts
      heritage
      heritageMonnaies
      paris
      faiblesses
      parades
      prevention
      coefficients
      constantesFrance
      ressourcesFrance
      ressourcesOptionsFrance
      importationsFrance
      exportationsFrance
      prenomsFrance
      regionsFrance
      originesFrance
      prenomsPays
      prenomsJoueurs
      paysDuMonde
      attachements
      antecedents
      typesPartie
      roles
      convoitisesFrance
      actionsDefense
      mesuresDefense
      tranches
      idees
      rumeurs
      situations
      chiffres
      voix
      habitants
      evenements
      repliques
      attitudes ]

type Erreur =
    | Illisible of fichier: string * Csv.ErreurCsv
    | Manquant of fichier: string
    | EnTete of fichier: string * attendu: string list * trouve: string list
    | NombreDeChamps of fichier: string * ligne: int * attendu: int * trouve: int
    /// `attendu` dit, pour un humain, ce que la colonne accepte
    | Valeur of fichier: string * ligne: int * colonne: string * valeur: string * attendu: string
    | Doublon of fichier: string * ligne: int * colonne: string * valeur: string
    | Reference of fichier: string * ligne: int * colonne: string * valeur: string * cible: string
    | Incomplet of fichier: string * message: string
    | GenereObsolete of chemin: string
    | GenereAbsent of chemin: string

let decrire =
    function
    | Illisible(f, Csv.GuillemetNonFerme l) -> $"data/{f}:{l} : guillemet ouvert jamais fermé"
    | Illisible(f, Csv.GuillemetDansChamp l) -> $"data/{f}:{l} : guillemet au milieu d'un champ non cité"
    | Illisible(f, Csv.FichierVide) -> $"data/{f} : fichier vide"
    | Manquant f -> $"data/{f} : fichier absent"
    | EnTete(f, a, t) ->
        let j (l: string list) = System.String.Join(",", l)
        $"data/{f}:1 : en-tête inattendu\n    attendu : {j a}\n    trouvé  : {j t}"
    | NombreDeChamps(f, l, a, t) -> $"data/{f}:{l} : {t} champs au lieu de {a}"
    | Valeur(f, l, c, v, a) -> $"data/{f}:{l} : colonne {c} : « {v} » : attendu {a}"
    | Doublon(f, l, c, v) -> $"data/{f}:{l} : colonne {c} : « {v} » déjà utilisé plus haut"
    | Reference(f, l, c, v, cible) -> $"data/{f}:{l} : colonne {c} : « {v} » n'existe pas dans {cible}"
    | Incomplet(f, m) -> $"data/{f} : {m}"
    | GenereObsolete p -> $"{p} : pas à jour avec data/ (lancer `labo generate`)"
    | GenereAbsent p -> $"{p} : absent (lancer `labo generate`)"

/// Une ligne de table, lue par nom de colonne.
type Rangee =
    { fichier: string
      numero: int
      valeurs: Map<string, string> }

    member r.Item(colonne: string) = r.valeurs[colonne]

let lire (dossier: string) (f: Fichier) : Result<Rangee list, Erreur list> =
    let p = Path.Combine(dossier, f.chemin)

    if not (File.Exists p) then
        Error [ Manquant f.chemin ]
    else
        match Csv.analyser (File.ReadAllText p) with
        | Error e -> Error [ Illisible(f.chemin, e) ]
        | Ok(tete :: corps) when tete.champs <> f.entete -> Error [ EnTete(f.chemin, f.entete, tete.champs) ]
        | Ok(_ :: corps) ->
            let n = f.entete.Length

            let erreurs =
                corps
                |> List.filter (fun l -> l.champs.Length <> n)
                |> List.map (fun l -> NombreDeChamps(f.chemin, l.numero, n, l.champs.Length))

            if erreurs.IsEmpty then
                corps
                |> List.map (fun l ->
                    { fichier = f.chemin
                      numero = l.numero
                      valeurs = List.zip f.entete l.champs |> Map.ofList })
                |> Ok
            else
                Error erreurs
        | Ok [] -> Error [ Illisible(f.chemin, Csv.FichierVide) ]
