// Les formes des données du jeu. Les valeurs sont dans Generated/, produites depuis data/ par
// `labo generate` ; ce fichier-ci est écrit à la main.
namespace Labo.Data

type Source =
    { id: string
      titre: string
      url: string
      producteur: string
      genre: string
      methode: string
      limites: string
      /// quand la source réunit plusieurs documents (sources-documents.csv) : titre, url
      documents: (string * string) list }

type Jauge =
    { id: string
      libelle: string
      ancre: string
      /// la jauge mesure le but (Liberté, Moyens libérés, Égalité, Principes) et n'entre pas dans le score
      horsScore: bool
      /// au-dessous, le modèle n'est pas viable (jugement) ; seulement pour les jauges vitales
      seuilVital: float option
      /// seuil « solide » qui pondère la légitimité : pays fermé ou entrouvert, pays ouvert, France
      solide: float option
      solideOuvert: float option
      solideFrance: float option
      aide: string }

/// La valeur d'une jauge pour un pays, aujourd'hui, avec sa date et son rythme de révision.
type ValeurPays =
    { jauge: string
      valeur: float option
      texte: string
      date: string
      revoir: string }

type GenreLevier =
    | Choix
    | OuiNon
    | Curseur
    /// des options qui règlent un curseur ou appliquent un préréglage nommé
    | Prereglage

type Levier =
    { id: string
      titre: string
      ancre: string
      genre: GenreLevier
      /// champ de l'état écrit par le levier : `c.<nom>` (un choix) ou `s.<nom>` (un curseur) ; vide pour un préréglage pur
      champ: string
      /// l'option que le moteur suppose quand le levier n'est pas touché ; pour un curseur, sa valeur
      defaut: string option
      min: float option
      max: float option
      pas: float option }

type Ecriture =
    /// `s.supply=829` : un autre champ que l'option écrit
    | Champ of champ: string * valeur: string
    /// `@paliers` : un préréglage défini dans le moteur
    | Nomme of nom: string

type OptionLevier =
    { levier: string
      rang: int
      id: string
      libelle: string
      note: string
      /// la valeur écrite dans le champ du levier
      valeur: string
      ecritAussi: Ecriture list }

type Statut =
    | Sourcé
    | Hypothèse
    | Jugement
    /// écrit sans source vérifiée : la source reste à trouver
    | DeMémoire
    /// recopié de l'ancien moteur, d'où il vient reste à documenter
    | ADocumenter

type Nature =
    | Contrainte
    | Egalite
    | Danger
    | Capacite
    | Cout
    | Concentration
    | Dependance
    | Dissuasion
    | Privation

/// `coord=plateforme|ia` ou `relais!=relais` ; `accueil=*` : n'importe quelle option.
type Atome =
    { levier: string
      egal: bool
      /// vide = n'importe quelle option (`*`)
      options: string list }

type Poids =
    | Plein
    /// `monnaie!=sans:0.5` : ce facteur quand la condition est vraie, 1 sinon
    | Si of condition: Atome list * facteur: float
    /// `@risque-gang` : un facteur calculé par le moteur
    | Facteur of nom: string

type Courbe = { nom: string; args: string list }

type Lien =
    { id: string
      jauge: string
      effet: float
      statut: Statut
      /// la lentille théorique dont relève le lien (data/lentilles.csv)
      lentille: string
      /// qui a choisi cette lentille : `auteur`, `assistant` (une proposition à vérifier), ou un contributeur
      lentillePar: string
      nature: Nature
      raison: string
      source: string option
      contreSource: string option
      /// toutes les conditions doivent être vraies ; vide = toujours
      quand: Atome list
      poids: Poids
      /// le lien dépend d'un curseur ou d'une valeur calculée : une fonction du moteur
      courbe: Courbe option }

/// Une lentille théorique : le cadre dont relève un lien ou un coefficient. Le joueur pourra en
/// changer ou en superposer (CHANTIERS.md).
type Lentille =
    { id: string
      nom: string
      penseurs: string
      description: string
      /// qui a proposé cette lentille : `auteur`, `assistant`, ou un contributeur
      proposePar: string }

/// Un curseur de l'état (`s.<id>`) : sa valeur par défaut dans le moteur, et dans l'état de départ.
type Curseur =
    { id: string
      defaut: float option
      depart: float option
      description: string }

/// Une attaque du stress test de la société sans police.
type Attaque =
    { id: string
      attaque: string
      /// tient, fissure, casse ou ouvert
      verdict: string
      verdictAvant: string option
      pourquoi: string }

type Maturite = { id: string; libelle: string; rang: int }

/// Un pari sur une solution en développement.
type Pari =
    { id: string
      categorie: string
      titre: string
      maturite: string
      /// années avant d'être prêt
      pretDans: int
      confort: float
      energie: float
      rendement: float
      besoin: float
      captation: float
      /// la ressource du test de fermeture qu'il couvre, et jusqu'où
      couvre: (string * string) option
      /// ne joue qu'avec une monnaie fondante ou plafonnée
      avecMonnaie: bool
      statut: Statut
      sources: string list
      description: string
      effet: string
      echec: string }

/// Une faiblesse du passage à l'échelle.
type Faiblesse =
    { id: string
      titre: string
      probleme: string
      fait: string
      statut: Statut
      sources: string list }

type Automatique =
    | MonnaiePlafonnee
    /// monnaie fondante ou plafonnée, avec un écart de revenus d'au moins 2
    | MonnaieAvecEcart
    | ProtocoleAdopte

type Parade =
    { faiblesse: string
      rang: int
      id: string
      libelle: string
      adoptee: bool
      captation: float
      automatique: Automatique option
      automatiqueTexte: string }

/// Un moyen de prévention du gang : il multiplie la probabilité du scénario.
type Prevention =
    { levier: string
      facteur: float
      statut: Statut
      sources: string list
      raison: string
      contre: string
      contreSource: string option }

/// Un coefficient du modèle : un nombre qui était écrit dans une formule.
type Coefficient =
    { id: string
      valeur: float
      statut: Statut
      lentille: string
      lentillePar: string
      sources: string list
      raison: string }

/// Une constante d'un pays : un fait, avec son unité et sa source.
type Constante =
    { id: string
      valeur: float
      unite: string
      statut: Statut
      sources: string list
      description: string }

/// Une ressource du test de fermeture, pour un pays.
type RessourcePays =
    { id: string
      nom: string
      /// couverture : manq, nv, part, imp ou ok
      metropole: string
      franceEntiere: string
      /// niveau d'ouverture à partir duquel elle est importée
      importeeDes: int option
      /// le levier de la table des ressources qui manquent qui la règle
      regleePar: string option
      pourquoi: string }

type OptionRessource =
    { levier: string
      option: string
      couverture: string
      importe: bool
      cout: string
      effet: string }

type Importation =
    { ouverture: int
      rang: int
      libelle: string
      valeur: float
      note: string }

type ValeurExportation =
    | Fixe of float
    /// calculée par le moteur
    | Calcul of nom: string

type Exportation =
    { id: string
      libelle: string
      parDefaut: bool
      valeur: ValeurExportation
      note: string }

// ---------- Le jeu (data/jeu/) ----------

/// Une tranche du jeu : un parcours de situations autour de quelques jauges.
type Tranche =
    { id: string
      rang: int
      titre: string
      sousTitre: string
      couleur: string
      embleme: string
      jauges: string list
      provenance: string }

type GenreSituation =
    /// des chiffres réels et sourcés, sans décision
    | Constat
    /// une décision : les options d'un levier
    | Dilemme
    /// plusieurs leviers oui-non à cocher (la prévention)
    | Prevention
    /// le bilan de la tranche
    | Bilan
    /// miser sur un pari parmi quelques-uns
    | Paris

type Situation =
    { id: string
      tranche: string
      rang: int
      genre: GenreSituation
      leviers: string list
      /// les paris proposés (genre Paris)
      paris: string list
      titre: string
      texte: string
      embleme: string
      /// quand la situation peut se présenter (mini-langage des liens) ; vide = toujours
      quand: Atome list
      provenance: string }

type Chiffre =
    { situation: string
      rang: int
      valeur: string
      legende: string
      sources: string list
      provenance: string }

type Position =
    | Pour
    | Contre
    | Reserve

/// Ce qu'un habitant dit d'une option.
type Voix =
    { situation: string
      rang: int
      habitant: string
      precision: string
      texte: string
      option: string
      position: Position
      provenance: string
      /// qui a lu la position dans le texte : `auteur`, `assistant`…
      positionPar: string }

type Habitant =
    { id: string
      nom: string
      portrait: string
      provenance: string }

/// Un effet qui dure : des points sur une jauge pendant quelques révolutions.
type EffetTemps =
    { jauge: string
      points: float
      duree: int }

type Comparaison =
    | Inferieur
    | Superieur

/// Ce qui arrive sans qu'on l'ait décidé : déclenché par l'état de la société, ou par le hasard.
type Evenement =
    { id: string
      /// la tranche où il peut survenir ; aucune = toutes
      tranche: string option
      titre: string
      texte: string
      /// conditions sur les leviers (mini-langage des liens)
      quand: Atome list
      /// conditions sur les jauges : jauge, comparaison, seuil (entre 0 et 1)
      siJauge: (string * Comparaison * float) list
      /// chance de survenir à chaque révolution où les conditions sont réunies
      probabilite: float
      uneFois: bool
      effets: EffetTemps list
      reputation: (string * float) list
      statut: Statut
      sources: string list
      provenance: string }
