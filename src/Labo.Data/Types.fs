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
      nature: Nature
      raison: string
      source: string option
      contreSource: string option
      /// toutes les conditions doivent être vraies ; vide = toujours
      quand: Atome list
      poids: Poids
      /// le lien dépend d'un curseur ou d'une valeur calculée : une fonction du moteur
      courbe: Courbe option }
