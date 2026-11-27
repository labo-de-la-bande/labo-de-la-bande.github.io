// GÉNÉRÉ depuis data/coefficients.csv par `labo generate`.
// Ne pas éditer : changer les données et régénérer.
module Labo.Data.Coefficients

open Labo.Data

/// Écart de revenus au départ, quand une monnaie revient (1 à 3) (Jugement, a-classer)
let mutable monnaieEcartDepart = 3.0

/// Fonte de la monnaie fondante au départ (Jugement, a-classer)
let mutable monnaieFonteDepart = 2.0

/// Plafond de patrimoine au départ (Jugement, a-classer)
let mutable monnaiePlafondDepart = 10.0

/// Nombre de besoins couverts par le socle au départ (Jugement, a-classer)
let mutable monnaieSocleDepart = 3.0

/// Années du deuxième horizon de temps (Jugement, a-classer)
let mutable horizon10Ans = 10.0

/// Années du troisième horizon de temps (Jugement, a-classer)
let mutable horizon30Ans = 30.0

/// Risque ajouté par pari actif, par degré d'immaturité (éprouvé 0, expérimental 3) (Jugement, a-classer)
let mutable parisRisqueParMaturite = 7.0

/// Sans échanges ciblés, les usines d'engrais au gaz importé s'arrêtent : les rendements retombent comme en bio (%) (Hypothèse, materialisme-historique)
let mutable engraisSansGazPerteRendement = 25.0

/// Surface minimale pour nourrir une personne, même avec tous les paris (hectares) (Hypothèse, materialisme-historique)
let mutable besoinMinimum = 0.15

/// Électricité retirée par l'ammoniac fabriqué par électrolyse (TWh par an) (Hypothèse, economie-ecologique)
let mutable nh3Electricite = 28.0

/// Électricité consommée par une planification par IA lourde (TWh par an) (Hypothèse, economie-ecologique)
let mutable coordIaElectricite = 5.0

/// Valeur exportée par TWh d'électricité ajouté par les paris (Md€) (Hypothèse, materialisme-historique)
let mutable exportElectriciteParTwh = 0.057

/// Valeur exportée par million de personnes nourries au-delà de la population (Md€) (Hypothèse, materialisme-historique)
let mutable exportNourritureParMillion = 0.2

/// Nombre de besoins couverts par le socle à partir duquel la nourriture en fait partie (Jugement, materialisme-historique)
let mutable accesSocleNourriture = 2.0

/// Nombre de besoins de base (logement, nourriture, soins…) (Jugement, materialisme-historique)
let mutable accesBesoins = 5.0

/// Personne n'est obligé de vivre en couple (Jugement, feminisme-materialiste)
let mutable patriarcatVieSeule = -0.1

/// Recul quand le socle atteint tout le monde : chacune a de quoi vivre sans dépendre d'un conjoint (Jugement, feminisme-materialiste)
let mutable patriarcatSocle = 0.2

/// Part atteinte par le socle au-dessous de laquelle il ne fait pas reculer le patriarcat (%) (Jugement, feminisme-materialiste)
let mutable patriarcatSoclePlancher = 90.0

/// Écart entre ce plancher et un socle qui atteint tout le monde (points) (Jugement, feminisme-materialiste)
let mutable patriarcatSoclePlage = 10.0

/// Héritage maximum (Jugement, feminisme-materialiste)
let mutable patriarcatHeritageLfi = -0.05

/// Tous les enfants à parts égales (Jugement, feminisme-materialiste)
let mutable patriarcatHeritageEgaux = -0.1

/// Retour aux communs (Jugement, feminisme-materialiste)
let mutable patriarcatHeritageCommuns = -0.15

/// À la famille choisie (Jugement, feminisme-materialiste)
let mutable patriarcatHeritageChoisie = -0.03

/// Libre choix du défunt (Jugement, feminisme-materialiste)
let mutable patriarcatHeritageLibre = 0.1

/// Des foyers choisis reconnus comme les couples (Jugement, feminisme-materialiste)
let mutable patriarcatFoyersChoisis = -0.05

/// Le soin des enfants et des repas confié à des services collectifs (Jugement, feminisme-materialiste)
let mutable patriarcatSoinServices = -0.03

/// Le soin des enfants et des repas partagé par toute la cellule (Jugement, feminisme-materialiste)
let mutable patriarcatSoinPartage = -0.1

/// Recul maximal par le tirage au sort paritaire, quand tous les conseils sont tirés au sort (Jugement, feminisme-materialiste)
let mutable patriarcatTirage = 0.1

/// Des cellules mobilisées contre les violences (Jugement, feminisme-materialiste)
let mutable patriarcatCellulesMobilisees = -0.15

/// Des clergés réservés aux hommes reconnus (Jugement, feminisme-materialiste)
let mutable patriarcatCultesReconnus = 0.05

/// Effet au-dessous duquel une structure n'est pas comptée (Jugement, feminisme-materialiste)
let mutable patriarcatSeuil = 0.005

/// Recul du patriarcat qui donne la baisse maximale des violences dans le couple (Jugement, feminisme-materialiste)
let mutable violencesReculPlein = 0.7

/// Baisse maximale des violences dans le couple : la moitié en 30 ans (les normes changent lentement, même dans les pays les plus égalitaires) (Jugement, feminisme-materialiste)
let mutable violencesBaisseMax = 0.5

/// Part de cette baisse acquise aujourd'hui (Jugement, feminisme-materialiste)
let mutable violencesPartAujourdhui = 0.3

/// Part de cette baisse acquise dans 10 ans (Jugement, feminisme-materialiste)
let mutable violencesPart10Ans = 0.6

/// Part de cette baisse acquise dans 30 ans (Jugement, feminisme-materialiste)
let mutable violencesPart30Ans = 1.0

/// Sans police : homicides +16 %, comme lors des grèves de police au Brésil (chiffre encore sans source vérifiée) (ADocumenter, realisme)
let mutable crimePessimistePolice = 1.16

/// Sans prison : homicides en hausse (haut de fourchette) (Hypothèse, realisme)
let mutable crimePessimistePrison = 1.05

/// La prévention qui marche (bas de fourchette) (Hypothèse, abolitionnisme)
let mutable crimeOptimistePrevention = 0.85

/// Le socle qui marche (bas de fourchette) (Hypothèse, abolitionnisme)
let mutable crimeOptimisteSocle = 0.9

/// Les réparations qui marchent (bas de fourchette) (Hypothèse, abolitionnisme)
let mutable crimeOptimisteReparations = 0.95

/// Part des homicides commis dans le couple (138 sur 976 en 2024) (ADocumenter, feminisme-materialiste)
let mutable crimePartCouple = 0.14

/// Baisse des homicides avec des gardes renforcées face au gang, au prorata du risque de gang (Hypothèse, realisme)
let mutable crimeGardes = 0.05

/// Baisse des homicides avec la médiation et l'aide pour quitter le gang, au prorata du risque de gang (Hypothèse, abolitionnisme)
let mutable crimeMediation = 0.02

/// Baisse des homicides avec l'appel aux cellules voisines, au prorata du risque de gang (Hypothèse, abolitionnisme)
let mutable crimeVoisins = 0.01

/// Des armes dans les foyers avec une milice : haut de fourchette (Hypothèse, abolitionnisme)
let mutable crimeMiliceHaut = 1.1

/// Des armes dans les foyers avec une milice : bas de fourchette (Hypothèse, abolitionnisme)
let mutable crimeMiliceBas = 1.05

/// Risque accepté face au meurtre par calcul froid (Hypothèse, abolitionnisme)
let mutable crimeCalculFroid = 1.02

/// Sans monnaie, ou avec un petit écart de revenus : moins de crimes (bas de fourchette) (Hypothèse, materialisme-historique)
let mutable crimePeuDEcart = 0.95

/// Écart de revenus jusqu'auquel il compte comme petit (Jugement, materialisme-historique)
let mutable crimeEcartMax = 3.0

/// Homicides (100 = aujourd'hui) qui vident la jauge : 50 % correspond à aujourd'hui (Jugement, a-classer)
let mutable crimeJaugePlage = 200.0

/// Nombre minimal d'unités au sommet de la pyramide des cellules (Jugement, loi-d-airain)
let mutable pyramideSommetMinimum = 10.0

/// Risque de prise du pouvoir avec les réglages de référence (Jugement, loi-d-airain)
let mutable pouvoirBase = 30.0

/// Risque ajouté par mois de mandat au-delà de la référence (Jugement, loi-d-airain)
let mutable pouvoirMandat = 1.5

/// Durée de mandat de référence (mois) (Jugement, loi-d-airain)
let mutable pouvoirMandatReference = 12.0

/// Risque ajouté par point de conseillers élus plutôt que tirés au sort (Jugement, democratie-directe)
let mutable pouvoirTirage = 0.25

/// Risque ajouté par point de base nécessaire pour révoquer au-delà de la référence (Jugement, democratie-directe)
let mutable pouvoirRevocation = 0.6

/// Part de la base de référence pour révoquer (%) (Jugement, democratie-directe)
let mutable pouvoirRevocationReference = 20.0

/// Risque ajouté par domaine décidé au national au-delà de la référence (Jugement, loi-d-airain)
let mutable pouvoirDomaines = 5.0

/// Nombre de domaines nationaux de référence (Jugement, loi-d-airain)
let mutable pouvoirDomainesReference = 3.0

/// Risque ajouté par niveau de la pyramide au-delà de la référence (Jugement, loi-d-airain)
let mutable pouvoirNiveau = 6.0

/// Nombre de niveaux de référence (Jugement, loi-d-airain)
let mutable pouvoirNiveauxReference = 3.0

/// Risque ajouté quand le sommet compte trop peu d'unités (Jugement, loi-d-airain)
let mutable pouvoirSommetPetit = 15.0

/// Risque ajouté par une petite armée professionnelle (Jugement, loi-d-airain)
let mutable pouvoirArmee = 15.0

/// Risque ajouté par des gardes renforcées, au prorata du risque de gang (Jugement, loi-d-airain)
let mutable pouvoirGardes = 5.0

/// Risque ajouté par point d'écart de revenus, avec une monnaie fondante ou plafonnée (Jugement, materialisme-historique)
let mutable pouvoirMonnaieEcart = 2.0

/// Risque ajouté par point de plafond de patrimoine au-delà de la référence (Jugement, materialisme-historique)
let mutable pouvoirMonnaiePlafond = 0.4

/// Plafond de patrimoine de référence (Jugement, materialisme-historique)
let mutable pouvoirMonnaiePlafondReference = 10.0

/// Risque de prise du pouvoir au-delà duquel les délégués qui s'emparent du pouvoir font fissurer le stress test (Jugement, loi-d-airain)
let mutable stressSeuilPouvoir = 60.0

/// Baisse des violences dans le couple à partir de laquelle l'attaque « violences cachées » tient (Jugement, feminisme-materialiste)
let mutable stressSeuilViolences = 0.3

/// Ce que vaut une attaque qui fissure (une attaque qui tient vaut 1) (Jugement, a-classer)
let mutable securiteFissure = 0.5

/// Place de la monnaie à la frontière (Jugement, materialisme-historique)
let mutable principesMonnaieFrontiere = 0.25

/// Place des bons de travail (Jugement, materialisme-historique)
let mutable principesMonnaieBons = 0.25

/// Place de la monnaie fondante (Jugement, materialisme-historique)
let mutable principesMonnaieFondante = 0.6

/// Place de la monnaie plafonnée (Jugement, materialisme-historique)
let mutable principesMonnaiePlafond = 1.0

/// Ce que la monnaie retire aux principes au plus (Jugement, materialisme-historique)
let mutable principesMonnaiePoids = 0.35

/// Ce qu'une armée retire aux principes (Jugement, abolitionnisme)
let mutable principesArmee = 0.15

/// Ce que des gardes renforcées contre un gang retirent aux principes : une police, quel que soit le risque (décision de l'auteur, 26 novembre 2026 : « les gardes coûtent en principes » ; autant qu'une armée, proposition de l'assistant). Avant : 0,1 au prorata du risque de gang (Jugement, abolitionnisme)
let mutable principesGardes = 0.15

/// Ce que des gardes armés d'intervention contre les violences dans le couple retirent aux principes : une police, comme les gardes contre un gang (décision de l'auteur, 26 novembre 2026 ; autant qu'une armée, proposition de l'assistant). Avant : rien (Jugement, abolitionnisme)
let mutable principesGardesViolence = 0.15

/// Ce que l'exil (meurtre de sang-froid) retire aux principes : une exclusion sans retour, qui exporte le danger (décision de l'auteur, 26 novembre 2026 ; montant proposé par l'assistant) (Jugement, abolitionnisme)
let mutable principesExil = 0.1

/// Ce que l'éloignement renouvelé par la charte retire aux principes : une exclusion limitée dans le temps (décision de l'auteur, 26 novembre 2026 ; montant proposé par l'assistant) (Jugement, abolitionnisme)
let mutable principesEloignement = 0.05

/// Ce que la vidéosurveillance algorithmique encadrée retire aux principes : une surveillance policière par les machines (décision de l'auteur, 26 novembre 2026 ; montant proposé par l'assistant) (Jugement, abolitionnisme)
let mutable principesSurveillanceEncadree = 0.1

/// Ce que la surveillance généralisée avec reconnaissance faciale retire aux principes : une police par les machines, autant que des gardes (décision de l'auteur, 26 novembre 2026 ; montant proposé par l'assistant) (Jugement, abolitionnisme)
let mutable principesSurveillanceGeneralisee = 0.15

/// Ce qu'une milice armée retire aux principes, citoyenne face à une invasion ou fédérale face à une sécession : une force armée (décision de l'auteur, 26 novembre 2026 ; montant proposé par l'assistant) (Jugement, abolitionnisme)
let mutable principesMilice = 0.1

/// Ce que retire chaque domaine national au-delà de la référence (Jugement, mutuellisme)
let mutable principesDomaines = 0.03

/// Ce que la centralisation retire au plus (Jugement, mutuellisme)
let mutable principesDomainesMax = 0.15

/// Ce que l'héritage libre retire aux principes (Jugement, materialisme-historique)
let mutable principesHeritageLibre = 0.1

/// Ce que la grande industrie exportatrice retire aux principes, pays entrouvert (Jugement, ecologie-politique)
let mutable principesIndustrie = 0.1

/// Part du budget des prisons et de la police reprise par les gardes, la médiation et les soins (Hypothèse, abolitionnisme)
let mutable moyensCouts = 0.4

/// Part reprise en plus par des gardes renforcées, au prorata du risque de gang (Hypothèse, abolitionnisme)
let mutable moyensGardes = 0.1

/// Liberté du modèle sans contrainte ajoutée : personne en prison, mises à l'abri de 30 jours au plus (Jugement, abolitionnisme)
let mutable liberteBase = 0.9

/// Ce que retirent des gardes renforcées, au prorata du risque de gang (Jugement, abolitionnisme)
let mutable liberteGardes = 0.15

/// Ce que retire un service de milice obligatoire (Jugement, abolitionnisme)
let mutable liberteMilice = 0.05

/// Ce que retire l'exil pour les meurtres de sang-froid (Jugement, abolitionnisme)
let mutable liberteExil = 0.05

/// Ce que retirent des éloignements renouvelés (Jugement, abolitionnisme)
let mutable liberteEloignement = 0.05

/// Ce que retire un engagement de durée contre formation (Jugement, mutuellisme)
let mutable liberteEngagement = 0.1

/// Égalité du modèle au départ : 35 comme la France, plus 10 sans contrôles au faciès ni enfermement (Jugement, intersectionnalite)
let mutable egaliteDepart = 0.45

/// Près de 0 ou de 1, une jauge ralentit au lieu d'être écrasée par la borne (Jugement, a-classer)
let mutable borneSouple = 0.03

/// Effet maximal de l'appartenance par la vie commune (durée la plus courte) (Jugement, anthropologie-comparee)
let mutable courbeAccueilMax = 4.0

/// Baisse de cet effet entre la durée la plus courte et la plus longue (Jugement, anthropologie-comparee)
let mutable courbeAccueilBaisse = 3.0

/// Durée de vie commune au-delà de laquelle l'effet ne baisse plus (mois) (Jugement, anthropologie-comparee)
let mutable courbeAccueilDureeMax = 36.0

/// Perte de rendement à partir de laquelle le travail des champs pèse sur les foyers (%) (Jugement, feminisme-materialiste)
let mutable courbeRendementSeuil = 20.0

/// Part de tirés au sort sans effet (%) (Jugement, democratie-directe)
let mutable courbeTiragePivot = 50.0

/// Part de la base au-dessous de laquelle la révocation devient facile (%) (Jugement, democratie-directe)
let mutable courbeRevocationFacileSeuil = 15.0

/// Points de base par unité d'effet (Jugement, democratie-directe)
let mutable courbeRevocationFacilePas = 5.0

/// Effet maximal d'une révocation facile (Jugement, democratie-directe)
let mutable courbeRevocationFacileMax = 2.0

/// Durée de mandat au-dessous de laquelle il devient court (mois) (Jugement, loi-d-airain)
let mutable courbeMandatCourtSeuil = 6.0

/// Mois par unité d'effet (Jugement, loi-d-airain)
let mutable courbeMandatCourtPas = 3.0

/// Part atteinte par le socle sans effet (%) (Jugement, materialisme-historique)
let mutable courbeSoclePivot = 99.0

/// Points de socle par unité d'effet, au-dessous du pivot (Jugement, materialisme-historique)
let mutable courbeSoclePas = 9.0

/// Part de la base pour révoquer sans effet (%) (Jugement, democratie-directe)
let mutable courbeRevocationPivot = 20.0

/// Points par unité d'effet, au-dessous du pivot (Jugement, democratie-directe)
let mutable courbeRevocationPasBas = 10.0

/// Effet par unité, au-dessous du pivot (Jugement, democratie-directe)
let mutable courbeRevocationEffetBas = 3.0

/// Points par unité d'effet, au-dessus du pivot (Jugement, democratie-directe)
let mutable courbeRevocationPasHaut = 20.0

/// Effet par unité, au-dessus du pivot (Jugement, democratie-directe)
let mutable courbeRevocationEffetHaut = 2.0

/// Durée de mandat au-dessous de laquelle les délégués manquent d'expérience (mois) (Jugement, loi-d-airain)
let mutable courbeMandatPivot = 12.0

/// Mois par unité d'effet (Jugement, loi-d-airain)
let mutable courbeMandatPas = 9.0

/// Effet par unité (Jugement, loi-d-airain)
let mutable courbeMandatEffet = 2.0

/// Jauge Échelle quand une faiblesse au moins reste sans parade suffisante (Jugement, a-classer)
let mutable echelleReduit = 0.33

/// Jauge Échelle quand les faiblesses ont leurs parades, sans protocole d'essai adopté (Jugement, a-classer)
let mutable echelleSurLePapier = 0.66

/// Nombre de parades qui suffisent pour une faiblesse (Jugement, a-classer)
let mutable paradesSuffisantes = 2.0

/// Écart de revenus à partir duquel une monnaie permet de mieux payer le travail rare (Jugement, ordre-spontane)
let mutable paradeEcartMinimum = 2.0

/// Effet (en points) au-dessous duquel un lien n'est pas compté (Jugement, a-classer)
let mutable lienSeuil = 0.05

/// Points d'effet par unité de baisse des violences dans le couple (Jugement, feminisme-materialiste)
let mutable courbeViolencesFacteur = 10.0

/// Ce que vaut une ressource partiellement couverte (une ressource couverte vaut 1) (Jugement, a-classer)
let mutable ressourcesPartielle = 0.5

/// Nombre de situations au plus qui se présentent à chaque révolution ; 100 pour l'instant, décision de l'auteur (22 novembre 2026) : tout se décide avant que l'année passe, en attendant un décompte du temps (CHANTIERS, « Le temps du jeu ») (Jugement, a-classer)
let mutable jeuSituationsParRevolution = 100.0

/// Confiance gagnée par un habitant quand on choisit l'option qu'il soutient (Jugement, a-classer)
let mutable jeuReputationPour = 1.0

/// Confiance perdue par un habitant quand on choisit l'option qu'il redoute (Jugement, a-classer)
let mutable jeuReputationContre = 1.0

/// Confiance perdue quand l'option qu'un habitant soutenait n'est pas choisie (Jugement, a-classer)
let mutable jeuReputationEspoirDecu = 0.5

/// Confiance gagnée quand l'option qu'un habitant redoutait est écartée (Jugement, a-classer)
let mutable jeuReputationCrainteEvitee = 0.5

/// Nombre d'événements au plus à chaque révolution (Jugement, a-classer)
let mutable jeuEvenementsParRevolution = 1.0

/// Révolutions qu'il faut à une décision pour produire tout son effet (une part chaque révolution, le reste ensuite) (Jugement, a-classer)
let mutable jeuInertieRevolutions = 2.0

/// Revenir sur une décision : ce que perdent ceux qui l'approuvaient, en multiple de ce qu'ils avaient gagné (un espoir repris fait plus mal qu'un refus) (Jugement, a-classer)
let mutable jeuRevirementDeception = 1.5

/// Revenir sur une décision : ce que regagnent ceux qui la redoutaient, en part de ce qu'ils avaient perdu (soulagés, mais méfiants) (Jugement, a-classer)
let mutable jeuRevirementSoulagement = 0.5

/// Ressenti au-delà duquel un habitant sans avis sur une question réagit quand même à la décision : somme des variations, en points sur 100, des jauges qui comptent pour lui, chacune multipliée par son poids. Mesuré le 22 novembre 2026 sur 20 parties complètes jouées au hasard (2 880 ressentis) : médiane 0,1, 75e centile 6, 90e centile 18 ; 25 ne retient que les décisions qui touchent vraiment (moins d'une sur dix) (Jugement, a-classer)
let mutable jeuRessentiSeuil = 25.0

/// Confiance gagnée ou perdue par un habitant quand une décision touche nettement ce qui compte pour lui (ressenti au-delà du seuil), qu'il ait eu un avis ou non (Jugement, a-classer)
let mutable jeuReputationRessenti = 0.25

/// Ce qu'on vit pèse plus : chaque point perdu sur une jauge qui compte pour un habitant augmente son poids relatif de 5 % (la direction vient de l'aversion à la perte ; l'ampleur est un réglage de jeu) (Jugement, a-classer)
let mutable jeuSensibilisationPerte = 0.05

/// Un point gagné apaise deux fois moins qu'un point perdu ne sensibilise (sens de l'aversion à la perte ; le rapport exact est un réglage de jeu) (Jugement, a-classer)
let mutable jeuApaisementGain = 0.025

/// Part de la sensibilité qui retombe à chaque révolution : on s'adapte avec le temps, mais pas d'un coup ni complètement (Jugement, a-classer)
let mutable jeuApaisementRevolution = 0.3

/// Sensibilité au plus : un attachement pèse au plus trois fois son poids de départ (Jugement, a-classer)
let mutable jeuSensibilitePlafond = 2.0

/// La confiance colore le regard : chaque point de confiance d'un habitant réduit de 10 % ce qu'une décision qui le gêne lui fait perdre ; chaque point de méfiance l'augmente d'autant (direction : justice procédurale ; ampleur : réglage de jeu, la confiance allant d'environ −6 à +5 sur 20 parties mesurées le 22 novembre 2026) (Jugement, a-classer)
let mutable jeuConfianceAttenuation = 0.1

/// Même en pleine confiance, une décision qui gêne un habitant lui coûte au moins 40 % de son effet : la confiance n'efface pas tout (Jugement, a-classer)
let mutable jeuConfiancePlancher = 0.4

/// En pleine méfiance, une décision qui gêne un habitant lui coûte au plus 160 % de son effet (Jugement, a-classer)
let mutable jeuMefiancePlafond = 1.6

/// La vie avance : jusqu'à cet âge, ce qu'on vit marque davantage (les « années impressionnables » : les opinions changent surtout à la fin de l'adolescence et au début de l'âge adulte) (Jugement, a-classer)
let mutable jeuAgeImpressionnable = 25.0

/// Jusqu'à l'âge impressionnable, une perte ou un gain vécus sensibilisent une fois et demie plus (direction : krosnick1989 ; ampleur : réglage de jeu). Aucun effet n'est appliqué au grand âge : les travaux divergent, et ce serait un stéréotype (Jugement, a-classer)
let mutable jeuMalleabiliteJeunes = 1.5

/// Nombre de paris qu'on peut lancer à la fois quand la question se présente : plusieurs, pour que des paris compatibles se cumulent ; pas tous, pour que le choix garde un sens (on ne peut pas tout entreprendre en même temps) (Jugement, a-classer)
let mutable jeuParisParOccasion = 3.0

/// Nombre de déclarations au moins pour qu'un prénom déclaré par des joueurs entre dans une banque : protège les prénoms très rares (l'Insee ne publie pas non plus ceux donnés moins de trois fois) et écarte les plaisanteries isolées (Jugement, a-classer)
let mutable prenomsJoueursSeuil = 3.0

/// Nombre de prénoms déclarés différents (même groupe de pays, même décennie, même usage) à partir duquel ils servent seuls au tirage ; en dessous, ils s'ajoutent aux prénoms les plus portés (INE), pour qu'un seul prénom déclaré ne soit pas donné à tout le monde (Jugement, a-classer)
let mutable prenomsJoueursMinimum = 5.0

/// De quoi lire et écrire chaque valeur (labo sensibilite).
let reglages : (string * (unit -> float) * (float -> unit)) list =
    [ "monnaie-ecart-depart", (fun () -> monnaieEcartDepart), (fun v -> monnaieEcartDepart <- v)
      "monnaie-fonte-depart", (fun () -> monnaieFonteDepart), (fun v -> monnaieFonteDepart <- v)
      "monnaie-plafond-depart", (fun () -> monnaiePlafondDepart), (fun v -> monnaiePlafondDepart <- v)
      "monnaie-socle-depart", (fun () -> monnaieSocleDepart), (fun v -> monnaieSocleDepart <- v)
      "horizon-10-ans", (fun () -> horizon10Ans), (fun v -> horizon10Ans <- v)
      "horizon-30-ans", (fun () -> horizon30Ans), (fun v -> horizon30Ans <- v)
      "paris-risque-par-maturite", (fun () -> parisRisqueParMaturite), (fun v -> parisRisqueParMaturite <- v)
      "engrais-sans-gaz-perte-rendement", (fun () -> engraisSansGazPerteRendement), (fun v -> engraisSansGazPerteRendement <- v)
      "besoin-minimum", (fun () -> besoinMinimum), (fun v -> besoinMinimum <- v)
      "nh3-electricite", (fun () -> nh3Electricite), (fun v -> nh3Electricite <- v)
      "coord-ia-electricite", (fun () -> coordIaElectricite), (fun v -> coordIaElectricite <- v)
      "export-electricite-par-twh", (fun () -> exportElectriciteParTwh), (fun v -> exportElectriciteParTwh <- v)
      "export-nourriture-par-million", (fun () -> exportNourritureParMillion), (fun v -> exportNourritureParMillion <- v)
      "acces-socle-nourriture", (fun () -> accesSocleNourriture), (fun v -> accesSocleNourriture <- v)
      "acces-besoins", (fun () -> accesBesoins), (fun v -> accesBesoins <- v)
      "patriarcat-vie-seule", (fun () -> patriarcatVieSeule), (fun v -> patriarcatVieSeule <- v)
      "patriarcat-socle", (fun () -> patriarcatSocle), (fun v -> patriarcatSocle <- v)
      "patriarcat-socle-plancher", (fun () -> patriarcatSoclePlancher), (fun v -> patriarcatSoclePlancher <- v)
      "patriarcat-socle-plage", (fun () -> patriarcatSoclePlage), (fun v -> patriarcatSoclePlage <- v)
      "patriarcat-heritage-lfi", (fun () -> patriarcatHeritageLfi), (fun v -> patriarcatHeritageLfi <- v)
      "patriarcat-heritage-egaux", (fun () -> patriarcatHeritageEgaux), (fun v -> patriarcatHeritageEgaux <- v)
      "patriarcat-heritage-communs", (fun () -> patriarcatHeritageCommuns), (fun v -> patriarcatHeritageCommuns <- v)
      "patriarcat-heritage-choisie", (fun () -> patriarcatHeritageChoisie), (fun v -> patriarcatHeritageChoisie <- v)
      "patriarcat-heritage-libre", (fun () -> patriarcatHeritageLibre), (fun v -> patriarcatHeritageLibre <- v)
      "patriarcat-foyers-choisis", (fun () -> patriarcatFoyersChoisis), (fun v -> patriarcatFoyersChoisis <- v)
      "patriarcat-soin-services", (fun () -> patriarcatSoinServices), (fun v -> patriarcatSoinServices <- v)
      "patriarcat-soin-partage", (fun () -> patriarcatSoinPartage), (fun v -> patriarcatSoinPartage <- v)
      "patriarcat-tirage", (fun () -> patriarcatTirage), (fun v -> patriarcatTirage <- v)
      "patriarcat-cellules-mobilisees", (fun () -> patriarcatCellulesMobilisees), (fun v -> patriarcatCellulesMobilisees <- v)
      "patriarcat-cultes-reconnus", (fun () -> patriarcatCultesReconnus), (fun v -> patriarcatCultesReconnus <- v)
      "patriarcat-seuil", (fun () -> patriarcatSeuil), (fun v -> patriarcatSeuil <- v)
      "violences-recul-plein", (fun () -> violencesReculPlein), (fun v -> violencesReculPlein <- v)
      "violences-baisse-max", (fun () -> violencesBaisseMax), (fun v -> violencesBaisseMax <- v)
      "violences-part-aujourdhui", (fun () -> violencesPartAujourdhui), (fun v -> violencesPartAujourdhui <- v)
      "violences-part-10-ans", (fun () -> violencesPart10Ans), (fun v -> violencesPart10Ans <- v)
      "violences-part-30-ans", (fun () -> violencesPart30Ans), (fun v -> violencesPart30Ans <- v)
      "crime-pessimiste-police", (fun () -> crimePessimistePolice), (fun v -> crimePessimistePolice <- v)
      "crime-pessimiste-prison", (fun () -> crimePessimistePrison), (fun v -> crimePessimistePrison <- v)
      "crime-optimiste-prevention", (fun () -> crimeOptimistePrevention), (fun v -> crimeOptimistePrevention <- v)
      "crime-optimiste-socle", (fun () -> crimeOptimisteSocle), (fun v -> crimeOptimisteSocle <- v)
      "crime-optimiste-reparations", (fun () -> crimeOptimisteReparations), (fun v -> crimeOptimisteReparations <- v)
      "crime-part-couple", (fun () -> crimePartCouple), (fun v -> crimePartCouple <- v)
      "crime-gardes", (fun () -> crimeGardes), (fun v -> crimeGardes <- v)
      "crime-mediation", (fun () -> crimeMediation), (fun v -> crimeMediation <- v)
      "crime-voisins", (fun () -> crimeVoisins), (fun v -> crimeVoisins <- v)
      "crime-milice-haut", (fun () -> crimeMiliceHaut), (fun v -> crimeMiliceHaut <- v)
      "crime-milice-bas", (fun () -> crimeMiliceBas), (fun v -> crimeMiliceBas <- v)
      "crime-calcul-froid", (fun () -> crimeCalculFroid), (fun v -> crimeCalculFroid <- v)
      "crime-peu-d-ecart", (fun () -> crimePeuDEcart), (fun v -> crimePeuDEcart <- v)
      "crime-ecart-max", (fun () -> crimeEcartMax), (fun v -> crimeEcartMax <- v)
      "crime-jauge-plage", (fun () -> crimeJaugePlage), (fun v -> crimeJaugePlage <- v)
      "pyramide-sommet-minimum", (fun () -> pyramideSommetMinimum), (fun v -> pyramideSommetMinimum <- v)
      "pouvoir-base", (fun () -> pouvoirBase), (fun v -> pouvoirBase <- v)
      "pouvoir-mandat", (fun () -> pouvoirMandat), (fun v -> pouvoirMandat <- v)
      "pouvoir-mandat-reference", (fun () -> pouvoirMandatReference), (fun v -> pouvoirMandatReference <- v)
      "pouvoir-tirage", (fun () -> pouvoirTirage), (fun v -> pouvoirTirage <- v)
      "pouvoir-revocation", (fun () -> pouvoirRevocation), (fun v -> pouvoirRevocation <- v)
      "pouvoir-revocation-reference", (fun () -> pouvoirRevocationReference), (fun v -> pouvoirRevocationReference <- v)
      "pouvoir-domaines", (fun () -> pouvoirDomaines), (fun v -> pouvoirDomaines <- v)
      "pouvoir-domaines-reference", (fun () -> pouvoirDomainesReference), (fun v -> pouvoirDomainesReference <- v)
      "pouvoir-niveau", (fun () -> pouvoirNiveau), (fun v -> pouvoirNiveau <- v)
      "pouvoir-niveaux-reference", (fun () -> pouvoirNiveauxReference), (fun v -> pouvoirNiveauxReference <- v)
      "pouvoir-sommet-petit", (fun () -> pouvoirSommetPetit), (fun v -> pouvoirSommetPetit <- v)
      "pouvoir-armee", (fun () -> pouvoirArmee), (fun v -> pouvoirArmee <- v)
      "pouvoir-gardes", (fun () -> pouvoirGardes), (fun v -> pouvoirGardes <- v)
      "pouvoir-monnaie-ecart", (fun () -> pouvoirMonnaieEcart), (fun v -> pouvoirMonnaieEcart <- v)
      "pouvoir-monnaie-plafond", (fun () -> pouvoirMonnaiePlafond), (fun v -> pouvoirMonnaiePlafond <- v)
      "pouvoir-monnaie-plafond-reference", (fun () -> pouvoirMonnaiePlafondReference), (fun v -> pouvoirMonnaiePlafondReference <- v)
      "stress-seuil-pouvoir", (fun () -> stressSeuilPouvoir), (fun v -> stressSeuilPouvoir <- v)
      "stress-seuil-violences", (fun () -> stressSeuilViolences), (fun v -> stressSeuilViolences <- v)
      "securite-fissure", (fun () -> securiteFissure), (fun v -> securiteFissure <- v)
      "principes-monnaie-frontiere", (fun () -> principesMonnaieFrontiere), (fun v -> principesMonnaieFrontiere <- v)
      "principes-monnaie-bons", (fun () -> principesMonnaieBons), (fun v -> principesMonnaieBons <- v)
      "principes-monnaie-fondante", (fun () -> principesMonnaieFondante), (fun v -> principesMonnaieFondante <- v)
      "principes-monnaie-plafond", (fun () -> principesMonnaiePlafond), (fun v -> principesMonnaiePlafond <- v)
      "principes-monnaie-poids", (fun () -> principesMonnaiePoids), (fun v -> principesMonnaiePoids <- v)
      "principes-armee", (fun () -> principesArmee), (fun v -> principesArmee <- v)
      "principes-gardes", (fun () -> principesGardes), (fun v -> principesGardes <- v)
      "principes-gardes-violence", (fun () -> principesGardesViolence), (fun v -> principesGardesViolence <- v)
      "principes-exil", (fun () -> principesExil), (fun v -> principesExil <- v)
      "principes-eloignement", (fun () -> principesEloignement), (fun v -> principesEloignement <- v)
      "principes-surveillance-encadree", (fun () -> principesSurveillanceEncadree), (fun v -> principesSurveillanceEncadree <- v)
      "principes-surveillance-generalisee", (fun () -> principesSurveillanceGeneralisee), (fun v -> principesSurveillanceGeneralisee <- v)
      "principes-milice", (fun () -> principesMilice), (fun v -> principesMilice <- v)
      "principes-domaines", (fun () -> principesDomaines), (fun v -> principesDomaines <- v)
      "principes-domaines-max", (fun () -> principesDomainesMax), (fun v -> principesDomainesMax <- v)
      "principes-heritage-libre", (fun () -> principesHeritageLibre), (fun v -> principesHeritageLibre <- v)
      "principes-industrie", (fun () -> principesIndustrie), (fun v -> principesIndustrie <- v)
      "moyens-couts", (fun () -> moyensCouts), (fun v -> moyensCouts <- v)
      "moyens-gardes", (fun () -> moyensGardes), (fun v -> moyensGardes <- v)
      "liberte-base", (fun () -> liberteBase), (fun v -> liberteBase <- v)
      "liberte-gardes", (fun () -> liberteGardes), (fun v -> liberteGardes <- v)
      "liberte-milice", (fun () -> liberteMilice), (fun v -> liberteMilice <- v)
      "liberte-exil", (fun () -> liberteExil), (fun v -> liberteExil <- v)
      "liberte-eloignement", (fun () -> liberteEloignement), (fun v -> liberteEloignement <- v)
      "liberte-engagement", (fun () -> liberteEngagement), (fun v -> liberteEngagement <- v)
      "egalite-depart", (fun () -> egaliteDepart), (fun v -> egaliteDepart <- v)
      "borne-souple", (fun () -> borneSouple), (fun v -> borneSouple <- v)
      "courbe-accueil-max", (fun () -> courbeAccueilMax), (fun v -> courbeAccueilMax <- v)
      "courbe-accueil-baisse", (fun () -> courbeAccueilBaisse), (fun v -> courbeAccueilBaisse <- v)
      "courbe-accueil-duree-max", (fun () -> courbeAccueilDureeMax), (fun v -> courbeAccueilDureeMax <- v)
      "courbe-rendement-seuil", (fun () -> courbeRendementSeuil), (fun v -> courbeRendementSeuil <- v)
      "courbe-tirage-pivot", (fun () -> courbeTiragePivot), (fun v -> courbeTiragePivot <- v)
      "courbe-revocation-facile-seuil", (fun () -> courbeRevocationFacileSeuil), (fun v -> courbeRevocationFacileSeuil <- v)
      "courbe-revocation-facile-pas", (fun () -> courbeRevocationFacilePas), (fun v -> courbeRevocationFacilePas <- v)
      "courbe-revocation-facile-max", (fun () -> courbeRevocationFacileMax), (fun v -> courbeRevocationFacileMax <- v)
      "courbe-mandat-court-seuil", (fun () -> courbeMandatCourtSeuil), (fun v -> courbeMandatCourtSeuil <- v)
      "courbe-mandat-court-pas", (fun () -> courbeMandatCourtPas), (fun v -> courbeMandatCourtPas <- v)
      "courbe-socle-pivot", (fun () -> courbeSoclePivot), (fun v -> courbeSoclePivot <- v)
      "courbe-socle-pas", (fun () -> courbeSoclePas), (fun v -> courbeSoclePas <- v)
      "courbe-revocation-pivot", (fun () -> courbeRevocationPivot), (fun v -> courbeRevocationPivot <- v)
      "courbe-revocation-pas-bas", (fun () -> courbeRevocationPasBas), (fun v -> courbeRevocationPasBas <- v)
      "courbe-revocation-effet-bas", (fun () -> courbeRevocationEffetBas), (fun v -> courbeRevocationEffetBas <- v)
      "courbe-revocation-pas-haut", (fun () -> courbeRevocationPasHaut), (fun v -> courbeRevocationPasHaut <- v)
      "courbe-revocation-effet-haut", (fun () -> courbeRevocationEffetHaut), (fun v -> courbeRevocationEffetHaut <- v)
      "courbe-mandat-pivot", (fun () -> courbeMandatPivot), (fun v -> courbeMandatPivot <- v)
      "courbe-mandat-pas", (fun () -> courbeMandatPas), (fun v -> courbeMandatPas <- v)
      "courbe-mandat-effet", (fun () -> courbeMandatEffet), (fun v -> courbeMandatEffet <- v)
      "echelle-reduit", (fun () -> echelleReduit), (fun v -> echelleReduit <- v)
      "echelle-sur-le-papier", (fun () -> echelleSurLePapier), (fun v -> echelleSurLePapier <- v)
      "parades-suffisantes", (fun () -> paradesSuffisantes), (fun v -> paradesSuffisantes <- v)
      "parade-ecart-minimum", (fun () -> paradeEcartMinimum), (fun v -> paradeEcartMinimum <- v)
      "lien-seuil", (fun () -> lienSeuil), (fun v -> lienSeuil <- v)
      "courbe-violences-facteur", (fun () -> courbeViolencesFacteur), (fun v -> courbeViolencesFacteur <- v)
      "ressources-partielle", (fun () -> ressourcesPartielle), (fun v -> ressourcesPartielle <- v)
      "jeu-situations-par-revolution", (fun () -> jeuSituationsParRevolution), (fun v -> jeuSituationsParRevolution <- v)
      "jeu-reputation-pour", (fun () -> jeuReputationPour), (fun v -> jeuReputationPour <- v)
      "jeu-reputation-contre", (fun () -> jeuReputationContre), (fun v -> jeuReputationContre <- v)
      "jeu-reputation-espoir-decu", (fun () -> jeuReputationEspoirDecu), (fun v -> jeuReputationEspoirDecu <- v)
      "jeu-reputation-crainte-evitee", (fun () -> jeuReputationCrainteEvitee), (fun v -> jeuReputationCrainteEvitee <- v)
      "jeu-evenements-par-revolution", (fun () -> jeuEvenementsParRevolution), (fun v -> jeuEvenementsParRevolution <- v)
      "jeu-inertie-revolutions", (fun () -> jeuInertieRevolutions), (fun v -> jeuInertieRevolutions <- v)
      "jeu-revirement-deception", (fun () -> jeuRevirementDeception), (fun v -> jeuRevirementDeception <- v)
      "jeu-revirement-soulagement", (fun () -> jeuRevirementSoulagement), (fun v -> jeuRevirementSoulagement <- v)
      "jeu-ressenti-seuil", (fun () -> jeuRessentiSeuil), (fun v -> jeuRessentiSeuil <- v)
      "jeu-reputation-ressenti", (fun () -> jeuReputationRessenti), (fun v -> jeuReputationRessenti <- v)
      "jeu-sensibilisation-perte", (fun () -> jeuSensibilisationPerte), (fun v -> jeuSensibilisationPerte <- v)
      "jeu-apaisement-gain", (fun () -> jeuApaisementGain), (fun v -> jeuApaisementGain <- v)
      "jeu-apaisement-revolution", (fun () -> jeuApaisementRevolution), (fun v -> jeuApaisementRevolution <- v)
      "jeu-sensibilite-plafond", (fun () -> jeuSensibilitePlafond), (fun v -> jeuSensibilitePlafond <- v)
      "jeu-confiance-attenuation", (fun () -> jeuConfianceAttenuation), (fun v -> jeuConfianceAttenuation <- v)
      "jeu-confiance-plancher", (fun () -> jeuConfiancePlancher), (fun v -> jeuConfiancePlancher <- v)
      "jeu-mefiance-plafond", (fun () -> jeuMefiancePlafond), (fun v -> jeuMefiancePlafond <- v)
      "jeu-age-impressionnable", (fun () -> jeuAgeImpressionnable), (fun v -> jeuAgeImpressionnable <- v)
      "jeu-malleabilite-jeunes", (fun () -> jeuMalleabiliteJeunes), (fun v -> jeuMalleabiliteJeunes <- v)
      "jeu-paris-par-occasion", (fun () -> jeuParisParOccasion), (fun v -> jeuParisParOccasion <- v)
      "prenoms-joueurs-seuil", (fun () -> prenomsJoueursSeuil), (fun v -> prenomsJoueursSeuil <- v)
      "prenoms-joueurs-minimum", (fun () -> prenomsJoueursMinimum), (fun v -> prenomsJoueursMinimum <- v) ]

let tous : Coefficient list =
    List.ofArray [|
      { id = "monnaie-ecart-depart"
        valeur = 3.0
        statut = Jugement
        lentille = "a-classer"
        lentillePar = "assistant"
        sources = []
        raison = "Écart de revenus au départ, quand une monnaie revient (1 à 3)" }
      { id = "monnaie-fonte-depart"
        valeur = 2.0
        statut = Jugement
        lentille = "a-classer"
        lentillePar = "assistant"
        sources = []
        raison = "Fonte de la monnaie fondante au départ" }
      { id = "monnaie-plafond-depart"
        valeur = 10.0
        statut = Jugement
        lentille = "a-classer"
        lentillePar = "assistant"
        sources = []
        raison = "Plafond de patrimoine au départ" }
      { id = "monnaie-socle-depart"
        valeur = 3.0
        statut = Jugement
        lentille = "a-classer"
        lentillePar = "assistant"
        sources = []
        raison = "Nombre de besoins couverts par le socle au départ" }
      { id = "horizon-10-ans"
        valeur = 10.0
        statut = Jugement
        lentille = "a-classer"
        lentillePar = "assistant"
        sources = []
        raison = "Années du deuxième horizon de temps" }
      { id = "horizon-30-ans"
        valeur = 30.0
        statut = Jugement
        lentille = "a-classer"
        lentillePar = "assistant"
        sources = []
        raison = "Années du troisième horizon de temps" }
      { id = "paris-risque-par-maturite"
        valeur = 7.0
        statut = Jugement
        lentille = "a-classer"
        lentillePar = "assistant"
        sources = []
        raison = "Risque ajouté par pari actif, par degré d'immaturité (éprouvé 0, expérimental 3)" }
      { id = "engrais-sans-gaz-perte-rendement"
        valeur = 25.0
        statut = Hypothèse
        lentille = "materialisme-historique"
        lentillePar = "assistant"
        sources = []
        raison = "Sans échanges ciblés, les usines d'engrais au gaz importé s'arrêtent : les rendements retombent comme en bio (%)" }
      { id = "besoin-minimum"
        valeur = 0.15
        statut = Hypothèse
        lentille = "materialisme-historique"
        lentillePar = "assistant"
        sources = []
        raison = "Surface minimale pour nourrir une personne, même avec tous les paris (hectares)" }
      { id = "nh3-electricite"
        valeur = 28.0
        statut = Hypothèse
        lentille = "economie-ecologique"
        lentillePar = "assistant"
        sources = []
        raison = "Électricité retirée par l'ammoniac fabriqué par électrolyse (TWh par an)" }
      { id = "coord-ia-electricite"
        valeur = 5.0
        statut = Hypothèse
        lentille = "economie-ecologique"
        lentillePar = "assistant"
        sources = []
        raison = "Électricité consommée par une planification par IA lourde (TWh par an)" }
      { id = "export-electricite-par-twh"
        valeur = 0.057
        statut = Hypothèse
        lentille = "materialisme-historique"
        lentillePar = "assistant"
        sources = []
        raison = "Valeur exportée par TWh d'électricité ajouté par les paris (Md€)" }
      { id = "export-nourriture-par-million"
        valeur = 0.2
        statut = Hypothèse
        lentille = "materialisme-historique"
        lentillePar = "assistant"
        sources = []
        raison = "Valeur exportée par million de personnes nourries au-delà de la population (Md€)" }
      { id = "acces-socle-nourriture"
        valeur = 2.0
        statut = Jugement
        lentille = "materialisme-historique"
        lentillePar = "assistant"
        sources = []
        raison = "Nombre de besoins couverts par le socle à partir duquel la nourriture en fait partie" }
      { id = "acces-besoins"
        valeur = 5.0
        statut = Jugement
        lentille = "materialisme-historique"
        lentillePar = "assistant"
        sources = []
        raison = "Nombre de besoins de base (logement, nourriture, soins…)" }
      { id = "patriarcat-vie-seule"
        valeur = -0.1
        statut = Jugement
        lentille = "feminisme-materialiste"
        lentillePar = "assistant"
        sources = [ "heise"; "sanday" ]
        raison = "Personne n'est obligé de vivre en couple" }
      { id = "patriarcat-socle"
        valeur = 0.2
        statut = Jugement
        lentille = "feminisme-materialiste"
        lentillePar = "assistant"
        sources = [ "heise"; "sanday" ]
        raison = "Recul quand le socle atteint tout le monde : chacune a de quoi vivre sans dépendre d'un conjoint" }
      { id = "patriarcat-socle-plancher"
        valeur = 90.0
        statut = Jugement
        lentille = "feminisme-materialiste"
        lentillePar = "assistant"
        sources = []
        raison = "Part atteinte par le socle au-dessous de laquelle il ne fait pas reculer le patriarcat (%)" }
      { id = "patriarcat-socle-plage"
        valeur = 10.0
        statut = Jugement
        lentille = "feminisme-materialiste"
        lentillePar = "assistant"
        sources = []
        raison = "Écart entre ce plancher et un socle qui atteint tout le monde (points)" }
      { id = "patriarcat-heritage-lfi"
        valeur = -0.05
        statut = Jugement
        lentille = "feminisme-materialiste"
        lentillePar = "assistant"
        sources = [ "heise"; "sanday" ]
        raison = "Héritage maximum" }
      { id = "patriarcat-heritage-egaux"
        valeur = -0.1
        statut = Jugement
        lentille = "feminisme-materialiste"
        lentillePar = "assistant"
        sources = [ "heise"; "sanday" ]
        raison = "Tous les enfants à parts égales" }
      { id = "patriarcat-heritage-communs"
        valeur = -0.15
        statut = Jugement
        lentille = "feminisme-materialiste"
        lentillePar = "assistant"
        sources = [ "heise"; "sanday" ]
        raison = "Retour aux communs" }
      { id = "patriarcat-heritage-choisie"
        valeur = -0.03
        statut = Jugement
        lentille = "feminisme-materialiste"
        lentillePar = "assistant"
        sources = [ "heise"; "sanday" ]
        raison = "À la famille choisie" }
      { id = "patriarcat-heritage-libre"
        valeur = 0.1
        statut = Jugement
        lentille = "feminisme-materialiste"
        lentillePar = "assistant"
        sources = [ "heise"; "sanday" ]
        raison = "Libre choix du défunt" }
      { id = "patriarcat-foyers-choisis"
        valeur = -0.05
        statut = Jugement
        lentille = "feminisme-materialiste"
        lentillePar = "assistant"
        sources = []
        raison = "Des foyers choisis reconnus comme les couples" }
      { id = "patriarcat-soin-services"
        valeur = -0.03
        statut = Jugement
        lentille = "feminisme-materialiste"
        lentillePar = "assistant"
        sources = []
        raison = "Le soin des enfants et des repas confié à des services collectifs" }
      { id = "patriarcat-soin-partage"
        valeur = -0.1
        statut = Jugement
        lentille = "feminisme-materialiste"
        lentillePar = "assistant"
        sources = []
        raison = "Le soin des enfants et des repas partagé par toute la cellule" }
      { id = "patriarcat-tirage"
        valeur = 0.1
        statut = Jugement
        lentille = "feminisme-materialiste"
        lentillePar = "assistant"
        sources = []
        raison = "Recul maximal par le tirage au sort paritaire, quand tous les conseils sont tirés au sort" }
      { id = "patriarcat-cellules-mobilisees"
        valeur = -0.15
        statut = Jugement
        lentille = "feminisme-materialiste"
        lentillePar = "assistant"
        sources = []
        raison = "Des cellules mobilisées contre les violences" }
      { id = "patriarcat-cultes-reconnus"
        valeur = 0.05
        statut = Jugement
        lentille = "feminisme-materialiste"
        lentillePar = "assistant"
        sources = []
        raison = "Des clergés réservés aux hommes reconnus" }
      { id = "patriarcat-seuil"
        valeur = 0.005
        statut = Jugement
        lentille = "feminisme-materialiste"
        lentillePar = "assistant"
        sources = []
        raison = "Effet au-dessous duquel une structure n'est pas comptée" }
      { id = "violences-recul-plein"
        valeur = 0.7
        statut = Jugement
        lentille = "feminisme-materialiste"
        lentillePar = "assistant"
        sources = []
        raison = "Recul du patriarcat qui donne la baisse maximale des violences dans le couple" }
      { id = "violences-baisse-max"
        valeur = 0.5
        statut = Jugement
        lentille = "feminisme-materialiste"
        lentillePar = "assistant"
        sources = [ "nordic" ]
        raison = "Baisse maximale des violences dans le couple : la moitié en 30 ans (les normes changent lentement, même dans les pays les plus égalitaires)" }
      { id = "violences-part-aujourdhui"
        valeur = 0.3
        statut = Jugement
        lentille = "feminisme-materialiste"
        lentillePar = "assistant"
        sources = [ "nordic" ]
        raison = "Part de cette baisse acquise aujourd'hui" }
      { id = "violences-part-10-ans"
        valeur = 0.6
        statut = Jugement
        lentille = "feminisme-materialiste"
        lentillePar = "assistant"
        sources = [ "nordic" ]
        raison = "Part de cette baisse acquise dans 10 ans" }
      { id = "violences-part-30-ans"
        valeur = 1.0
        statut = Jugement
        lentille = "feminisme-materialiste"
        lentillePar = "assistant"
        sources = [ "nordic" ]
        raison = "Part de cette baisse acquise dans 30 ans" }
      { id = "crime-pessimiste-police"
        valeur = 1.16
        statut = ADocumenter
        lentille = "realisme"
        lentillePar = "assistant"
        sources = []
        raison = "Sans police : homicides +16 %, comme lors des grèves de police au Brésil (chiffre encore sans source vérifiée)" }
      { id = "crime-pessimiste-prison"
        valeur = 1.05
        statut = Hypothèse
        lentille = "realisme"
        lentillePar = "assistant"
        sources = []
        raison = "Sans prison : homicides en hausse (haut de fourchette)" }
      { id = "crime-optimiste-prevention"
        valeur = 0.85
        statut = Hypothèse
        lentille = "abolitionnisme"
        lentillePar = "assistant"
        sources = []
        raison = "La prévention qui marche (bas de fourchette)" }
      { id = "crime-optimiste-socle"
        valeur = 0.9
        statut = Hypothèse
        lentille = "abolitionnisme"
        lentillePar = "assistant"
        sources = []
        raison = "Le socle qui marche (bas de fourchette)" }
      { id = "crime-optimiste-reparations"
        valeur = 0.95
        statut = Hypothèse
        lentille = "abolitionnisme"
        lentillePar = "assistant"
        sources = []
        raison = "Les réparations qui marchent (bas de fourchette)" }
      { id = "crime-part-couple"
        valeur = 0.14
        statut = ADocumenter
        lentille = "feminisme-materialiste"
        lentillePar = "assistant"
        sources = []
        raison = "Part des homicides commis dans le couple (138 sur 976 en 2024)" }
      { id = "crime-gardes"
        valeur = 0.05
        statut = Hypothèse
        lentille = "realisme"
        lentillePar = "assistant"
        sources = []
        raison = "Baisse des homicides avec des gardes renforcées face au gang, au prorata du risque de gang" }
      { id = "crime-mediation"
        valeur = 0.02
        statut = Hypothèse
        lentille = "abolitionnisme"
        lentillePar = "assistant"
        sources = [ "bam" ]
        raison = "Baisse des homicides avec la médiation et l'aide pour quitter le gang, au prorata du risque de gang" }
      { id = "crime-voisins"
        valeur = 0.01
        statut = Hypothèse
        lentille = "abolitionnisme"
        lentillePar = "assistant"
        sources = []
        raison = "Baisse des homicides avec l'appel aux cellules voisines, au prorata du risque de gang" }
      { id = "crime-milice-haut"
        valeur = 1.1
        statut = Hypothèse
        lentille = "abolitionnisme"
        lentillePar = "assistant"
        sources = [ "campbell" ]
        raison = "Des armes dans les foyers avec une milice : haut de fourchette" }
      { id = "crime-milice-bas"
        valeur = 1.05
        statut = Hypothèse
        lentille = "abolitionnisme"
        lentillePar = "assistant"
        sources = [ "campbell" ]
        raison = "Des armes dans les foyers avec une milice : bas de fourchette" }
      { id = "crime-calcul-froid"
        valeur = 1.02
        statut = Hypothèse
        lentille = "abolitionnisme"
        lentillePar = "assistant"
        sources = []
        raison = "Risque accepté face au meurtre par calcul froid" }
      { id = "crime-peu-d-ecart"
        valeur = 0.95
        statut = Hypothèse
        lentille = "materialisme-historique"
        lentillePar = "assistant"
        sources = []
        raison = "Sans monnaie, ou avec un petit écart de revenus : moins de crimes (bas de fourchette)" }
      { id = "crime-ecart-max"
        valeur = 3.0
        statut = Jugement
        lentille = "materialisme-historique"
        lentillePar = "assistant"
        sources = []
        raison = "Écart de revenus jusqu'auquel il compte comme petit" }
      { id = "crime-jauge-plage"
        valeur = 200.0
        statut = Jugement
        lentille = "a-classer"
        lentillePar = "assistant"
        sources = []
        raison = "Homicides (100 = aujourd'hui) qui vident la jauge : 50 % correspond à aujourd'hui" }
      { id = "pyramide-sommet-minimum"
        valeur = 10.0
        statut = Jugement
        lentille = "loi-d-airain"
        lentillePar = "assistant"
        sources = []
        raison = "Nombre minimal d'unités au sommet de la pyramide des cellules" }
      { id = "pouvoir-base"
        valeur = 30.0
        statut = Jugement
        lentille = "loi-d-airain"
        lentillePar = "assistant"
        sources = []
        raison = "Risque de prise du pouvoir avec les réglages de référence" }
      { id = "pouvoir-mandat"
        valeur = 1.5
        statut = Jugement
        lentille = "loi-d-airain"
        lentillePar = "assistant"
        sources = []
        raison = "Risque ajouté par mois de mandat au-delà de la référence" }
      { id = "pouvoir-mandat-reference"
        valeur = 12.0
        statut = Jugement
        lentille = "loi-d-airain"
        lentillePar = "assistant"
        sources = []
        raison = "Durée de mandat de référence (mois)" }
      { id = "pouvoir-tirage"
        valeur = 0.25
        statut = Jugement
        lentille = "democratie-directe"
        lentillePar = "assistant"
        sources = []
        raison = "Risque ajouté par point de conseillers élus plutôt que tirés au sort" }
      { id = "pouvoir-revocation"
        valeur = 0.6
        statut = Jugement
        lentille = "democratie-directe"
        lentillePar = "assistant"
        sources = []
        raison = "Risque ajouté par point de base nécessaire pour révoquer au-delà de la référence" }
      { id = "pouvoir-revocation-reference"
        valeur = 20.0
        statut = Jugement
        lentille = "democratie-directe"
        lentillePar = "assistant"
        sources = []
        raison = "Part de la base de référence pour révoquer (%)" }
      { id = "pouvoir-domaines"
        valeur = 5.0
        statut = Jugement
        lentille = "loi-d-airain"
        lentillePar = "assistant"
        sources = []
        raison = "Risque ajouté par domaine décidé au national au-delà de la référence" }
      { id = "pouvoir-domaines-reference"
        valeur = 3.0
        statut = Jugement
        lentille = "loi-d-airain"
        lentillePar = "assistant"
        sources = []
        raison = "Nombre de domaines nationaux de référence" }
      { id = "pouvoir-niveau"
        valeur = 6.0
        statut = Jugement
        lentille = "loi-d-airain"
        lentillePar = "assistant"
        sources = []
        raison = "Risque ajouté par niveau de la pyramide au-delà de la référence" }
      { id = "pouvoir-niveaux-reference"
        valeur = 3.0
        statut = Jugement
        lentille = "loi-d-airain"
        lentillePar = "assistant"
        sources = []
        raison = "Nombre de niveaux de référence" }
      { id = "pouvoir-sommet-petit"
        valeur = 15.0
        statut = Jugement
        lentille = "loi-d-airain"
        lentillePar = "assistant"
        sources = []
        raison = "Risque ajouté quand le sommet compte trop peu d'unités" }
      { id = "pouvoir-armee"
        valeur = 15.0
        statut = Jugement
        lentille = "loi-d-airain"
        lentillePar = "assistant"
        sources = []
        raison = "Risque ajouté par une petite armée professionnelle" }
      { id = "pouvoir-gardes"
        valeur = 5.0
        statut = Jugement
        lentille = "loi-d-airain"
        lentillePar = "assistant"
        sources = []
        raison = "Risque ajouté par des gardes renforcées, au prorata du risque de gang" }
      { id = "pouvoir-monnaie-ecart"
        valeur = 2.0
        statut = Jugement
        lentille = "materialisme-historique"
        lentillePar = "assistant"
        sources = []
        raison = "Risque ajouté par point d'écart de revenus, avec une monnaie fondante ou plafonnée" }
      { id = "pouvoir-monnaie-plafond"
        valeur = 0.4
        statut = Jugement
        lentille = "materialisme-historique"
        lentillePar = "assistant"
        sources = []
        raison = "Risque ajouté par point de plafond de patrimoine au-delà de la référence" }
      { id = "pouvoir-monnaie-plafond-reference"
        valeur = 10.0
        statut = Jugement
        lentille = "materialisme-historique"
        lentillePar = "assistant"
        sources = []
        raison = "Plafond de patrimoine de référence" }
      { id = "stress-seuil-pouvoir"
        valeur = 60.0
        statut = Jugement
        lentille = "loi-d-airain"
        lentillePar = "assistant"
        sources = []
        raison = "Risque de prise du pouvoir au-delà duquel les délégués qui s'emparent du pouvoir font fissurer le stress test" }
      { id = "stress-seuil-violences"
        valeur = 0.3
        statut = Jugement
        lentille = "feminisme-materialiste"
        lentillePar = "assistant"
        sources = []
        raison = "Baisse des violences dans le couple à partir de laquelle l'attaque « violences cachées » tient" }
      { id = "securite-fissure"
        valeur = 0.5
        statut = Jugement
        lentille = "a-classer"
        lentillePar = "assistant"
        sources = []
        raison = "Ce que vaut une attaque qui fissure (une attaque qui tient vaut 1)" }
      { id = "principes-monnaie-frontiere"
        valeur = 0.25
        statut = Jugement
        lentille = "materialisme-historique"
        lentillePar = "assistant"
        sources = []
        raison = "Place de la monnaie à la frontière" }
      { id = "principes-monnaie-bons"
        valeur = 0.25
        statut = Jugement
        lentille = "materialisme-historique"
        lentillePar = "assistant"
        sources = []
        raison = "Place des bons de travail" }
      { id = "principes-monnaie-fondante"
        valeur = 0.6
        statut = Jugement
        lentille = "materialisme-historique"
        lentillePar = "assistant"
        sources = []
        raison = "Place de la monnaie fondante" }
      { id = "principes-monnaie-plafond"
        valeur = 1.0
        statut = Jugement
        lentille = "materialisme-historique"
        lentillePar = "assistant"
        sources = []
        raison = "Place de la monnaie plafonnée" }
      { id = "principes-monnaie-poids"
        valeur = 0.35
        statut = Jugement
        lentille = "materialisme-historique"
        lentillePar = "assistant"
        sources = []
        raison = "Ce que la monnaie retire aux principes au plus" }
      { id = "principes-armee"
        valeur = 0.15
        statut = Jugement
        lentille = "abolitionnisme"
        lentillePar = "assistant"
        sources = []
        raison = "Ce qu'une armée retire aux principes" }
      { id = "principes-gardes"
        valeur = 0.15
        statut = Jugement
        lentille = "abolitionnisme"
        lentillePar = "assistant"
        sources = []
        raison = "Ce que des gardes renforcées contre un gang retirent aux principes : une police, quel que soit le risque (décision de l'auteur, 26 novembre 2026 : « les gardes coûtent en principes » ; autant qu'une armée, proposition de l'assistant). Avant : 0,1 au prorata du risque de gang" }
      { id = "principes-gardes-violence"
        valeur = 0.15
        statut = Jugement
        lentille = "abolitionnisme"
        lentillePar = "assistant"
        sources = []
        raison = "Ce que des gardes armés d'intervention contre les violences dans le couple retirent aux principes : une police, comme les gardes contre un gang (décision de l'auteur, 26 novembre 2026 ; autant qu'une armée, proposition de l'assistant). Avant : rien" }
      { id = "principes-exil"
        valeur = 0.1
        statut = Jugement
        lentille = "abolitionnisme"
        lentillePar = "assistant"
        sources = []
        raison = "Ce que l'exil (meurtre de sang-froid) retire aux principes : une exclusion sans retour, qui exporte le danger (décision de l'auteur, 26 novembre 2026 ; montant proposé par l'assistant)" }
      { id = "principes-eloignement"
        valeur = 0.05
        statut = Jugement
        lentille = "abolitionnisme"
        lentillePar = "assistant"
        sources = []
        raison = "Ce que l'éloignement renouvelé par la charte retire aux principes : une exclusion limitée dans le temps (décision de l'auteur, 26 novembre 2026 ; montant proposé par l'assistant)" }
      { id = "principes-surveillance-encadree"
        valeur = 0.1
        statut = Jugement
        lentille = "abolitionnisme"
        lentillePar = "assistant"
        sources = []
        raison = "Ce que la vidéosurveillance algorithmique encadrée retire aux principes : une surveillance policière par les machines (décision de l'auteur, 26 novembre 2026 ; montant proposé par l'assistant)" }
      { id = "principes-surveillance-generalisee"
        valeur = 0.15
        statut = Jugement
        lentille = "abolitionnisme"
        lentillePar = "assistant"
        sources = []
        raison = "Ce que la surveillance généralisée avec reconnaissance faciale retire aux principes : une police par les machines, autant que des gardes (décision de l'auteur, 26 novembre 2026 ; montant proposé par l'assistant)" }
      { id = "principes-milice"
        valeur = 0.1
        statut = Jugement
        lentille = "abolitionnisme"
        lentillePar = "assistant"
        sources = []
        raison = "Ce qu'une milice armée retire aux principes, citoyenne face à une invasion ou fédérale face à une sécession : une force armée (décision de l'auteur, 26 novembre 2026 ; montant proposé par l'assistant)" }
      { id = "principes-domaines"
        valeur = 0.03
        statut = Jugement
        lentille = "mutuellisme"
        lentillePar = "assistant"
        sources = []
        raison = "Ce que retire chaque domaine national au-delà de la référence" }
      { id = "principes-domaines-max"
        valeur = 0.15
        statut = Jugement
        lentille = "mutuellisme"
        lentillePar = "assistant"
        sources = []
        raison = "Ce que la centralisation retire au plus" }
      { id = "principes-heritage-libre"
        valeur = 0.1
        statut = Jugement
        lentille = "materialisme-historique"
        lentillePar = "assistant"
        sources = []
        raison = "Ce que l'héritage libre retire aux principes" }
      { id = "principes-industrie"
        valeur = 0.1
        statut = Jugement
        lentille = "ecologie-politique"
        lentillePar = "assistant"
        sources = []
        raison = "Ce que la grande industrie exportatrice retire aux principes, pays entrouvert" }
      { id = "moyens-couts"
        valeur = 0.4
        statut = Hypothèse
        lentille = "abolitionnisme"
        lentillePar = "assistant"
        sources = []
        raison = "Part du budget des prisons et de la police reprise par les gardes, la médiation et les soins" }
      { id = "moyens-gardes"
        valeur = 0.1
        statut = Hypothèse
        lentille = "abolitionnisme"
        lentillePar = "assistant"
        sources = []
        raison = "Part reprise en plus par des gardes renforcées, au prorata du risque de gang" }
      { id = "liberte-base"
        valeur = 0.9
        statut = Jugement
        lentille = "abolitionnisme"
        lentillePar = "assistant"
        sources = []
        raison = "Liberté du modèle sans contrainte ajoutée : personne en prison, mises à l'abri de 30 jours au plus" }
      { id = "liberte-gardes"
        valeur = 0.15
        statut = Jugement
        lentille = "abolitionnisme"
        lentillePar = "assistant"
        sources = []
        raison = "Ce que retirent des gardes renforcées, au prorata du risque de gang" }
      { id = "liberte-milice"
        valeur = 0.05
        statut = Jugement
        lentille = "abolitionnisme"
        lentillePar = "assistant"
        sources = []
        raison = "Ce que retire un service de milice obligatoire" }
      { id = "liberte-exil"
        valeur = 0.05
        statut = Jugement
        lentille = "abolitionnisme"
        lentillePar = "assistant"
        sources = []
        raison = "Ce que retire l'exil pour les meurtres de sang-froid" }
      { id = "liberte-eloignement"
        valeur = 0.05
        statut = Jugement
        lentille = "abolitionnisme"
        lentillePar = "assistant"
        sources = []
        raison = "Ce que retirent des éloignements renouvelés" }
      { id = "liberte-engagement"
        valeur = 0.1
        statut = Jugement
        lentille = "mutuellisme"
        lentillePar = "assistant"
        sources = []
        raison = "Ce que retire un engagement de durée contre formation" }
      { id = "egalite-depart"
        valeur = 0.45
        statut = Jugement
        lentille = "intersectionnalite"
        lentillePar = "assistant"
        sources = []
        raison = "Égalité du modèle au départ : 35 comme la France, plus 10 sans contrôles au faciès ni enfermement" }
      { id = "borne-souple"
        valeur = 0.03
        statut = Jugement
        lentille = "a-classer"
        lentillePar = "assistant"
        sources = []
        raison = "Près de 0 ou de 1, une jauge ralentit au lieu d'être écrasée par la borne" }
      { id = "courbe-accueil-max"
        valeur = 4.0
        statut = Jugement
        lentille = "anthropologie-comparee"
        lentillePar = "assistant"
        sources = []
        raison = "Effet maximal de l'appartenance par la vie commune (durée la plus courte)" }
      { id = "courbe-accueil-baisse"
        valeur = 3.0
        statut = Jugement
        lentille = "anthropologie-comparee"
        lentillePar = "assistant"
        sources = []
        raison = "Baisse de cet effet entre la durée la plus courte et la plus longue" }
      { id = "courbe-accueil-duree-max"
        valeur = 36.0
        statut = Jugement
        lentille = "anthropologie-comparee"
        lentillePar = "assistant"
        sources = []
        raison = "Durée de vie commune au-delà de laquelle l'effet ne baisse plus (mois)" }
      { id = "courbe-rendement-seuil"
        valeur = 20.0
        statut = Jugement
        lentille = "feminisme-materialiste"
        lentillePar = "assistant"
        sources = []
        raison = "Perte de rendement à partir de laquelle le travail des champs pèse sur les foyers (%)" }
      { id = "courbe-tirage-pivot"
        valeur = 50.0
        statut = Jugement
        lentille = "democratie-directe"
        lentillePar = "assistant"
        sources = []
        raison = "Part de tirés au sort sans effet (%)" }
      { id = "courbe-revocation-facile-seuil"
        valeur = 15.0
        statut = Jugement
        lentille = "democratie-directe"
        lentillePar = "assistant"
        sources = []
        raison = "Part de la base au-dessous de laquelle la révocation devient facile (%)" }
      { id = "courbe-revocation-facile-pas"
        valeur = 5.0
        statut = Jugement
        lentille = "democratie-directe"
        lentillePar = "assistant"
        sources = []
        raison = "Points de base par unité d'effet" }
      { id = "courbe-revocation-facile-max"
        valeur = 2.0
        statut = Jugement
        lentille = "democratie-directe"
        lentillePar = "assistant"
        sources = []
        raison = "Effet maximal d'une révocation facile" }
      { id = "courbe-mandat-court-seuil"
        valeur = 6.0
        statut = Jugement
        lentille = "loi-d-airain"
        lentillePar = "assistant"
        sources = []
        raison = "Durée de mandat au-dessous de laquelle il devient court (mois)" }
      { id = "courbe-mandat-court-pas"
        valeur = 3.0
        statut = Jugement
        lentille = "loi-d-airain"
        lentillePar = "assistant"
        sources = []
        raison = "Mois par unité d'effet" }
      { id = "courbe-socle-pivot"
        valeur = 99.0
        statut = Jugement
        lentille = "materialisme-historique"
        lentillePar = "assistant"
        sources = []
        raison = "Part atteinte par le socle sans effet (%)" }
      { id = "courbe-socle-pas"
        valeur = 9.0
        statut = Jugement
        lentille = "materialisme-historique"
        lentillePar = "assistant"
        sources = []
        raison = "Points de socle par unité d'effet, au-dessous du pivot" }
      { id = "courbe-revocation-pivot"
        valeur = 20.0
        statut = Jugement
        lentille = "democratie-directe"
        lentillePar = "assistant"
        sources = []
        raison = "Part de la base pour révoquer sans effet (%)" }
      { id = "courbe-revocation-pas-bas"
        valeur = 10.0
        statut = Jugement
        lentille = "democratie-directe"
        lentillePar = "assistant"
        sources = []
        raison = "Points par unité d'effet, au-dessous du pivot" }
      { id = "courbe-revocation-effet-bas"
        valeur = 3.0
        statut = Jugement
        lentille = "democratie-directe"
        lentillePar = "assistant"
        sources = []
        raison = "Effet par unité, au-dessous du pivot" }
      { id = "courbe-revocation-pas-haut"
        valeur = 20.0
        statut = Jugement
        lentille = "democratie-directe"
        lentillePar = "assistant"
        sources = []
        raison = "Points par unité d'effet, au-dessus du pivot" }
      { id = "courbe-revocation-effet-haut"
        valeur = 2.0
        statut = Jugement
        lentille = "democratie-directe"
        lentillePar = "assistant"
        sources = []
        raison = "Effet par unité, au-dessus du pivot" }
      { id = "courbe-mandat-pivot"
        valeur = 12.0
        statut = Jugement
        lentille = "loi-d-airain"
        lentillePar = "assistant"
        sources = []
        raison = "Durée de mandat au-dessous de laquelle les délégués manquent d'expérience (mois)" }
      { id = "courbe-mandat-pas"
        valeur = 9.0
        statut = Jugement
        lentille = "loi-d-airain"
        lentillePar = "assistant"
        sources = []
        raison = "Mois par unité d'effet" }
      { id = "courbe-mandat-effet"
        valeur = 2.0
        statut = Jugement
        lentille = "loi-d-airain"
        lentillePar = "assistant"
        sources = []
        raison = "Effet par unité" }
      { id = "echelle-reduit"
        valeur = 0.33
        statut = Jugement
        lentille = "a-classer"
        lentillePar = "assistant"
        sources = []
        raison = "Jauge Échelle quand une faiblesse au moins reste sans parade suffisante" }
      { id = "echelle-sur-le-papier"
        valeur = 0.66
        statut = Jugement
        lentille = "a-classer"
        lentillePar = "assistant"
        sources = []
        raison = "Jauge Échelle quand les faiblesses ont leurs parades, sans protocole d'essai adopté" }
      { id = "parades-suffisantes"
        valeur = 2.0
        statut = Jugement
        lentille = "a-classer"
        lentillePar = "assistant"
        sources = []
        raison = "Nombre de parades qui suffisent pour une faiblesse" }
      { id = "parade-ecart-minimum"
        valeur = 2.0
        statut = Jugement
        lentille = "ordre-spontane"
        lentillePar = "assistant"
        sources = []
        raison = "Écart de revenus à partir duquel une monnaie permet de mieux payer le travail rare" }
      { id = "lien-seuil"
        valeur = 0.05
        statut = Jugement
        lentille = "a-classer"
        lentillePar = "assistant"
        sources = []
        raison = "Effet (en points) au-dessous duquel un lien n'est pas compté" }
      { id = "courbe-violences-facteur"
        valeur = 10.0
        statut = Jugement
        lentille = "feminisme-materialiste"
        lentillePar = "assistant"
        sources = []
        raison = "Points d'effet par unité de baisse des violences dans le couple" }
      { id = "ressources-partielle"
        valeur = 0.5
        statut = Jugement
        lentille = "a-classer"
        lentillePar = "assistant"
        sources = []
        raison = "Ce que vaut une ressource partiellement couverte (une ressource couverte vaut 1)" }
      { id = "jeu-situations-par-revolution"
        valeur = 100.0
        statut = Jugement
        lentille = "a-classer"
        lentillePar = "assistant"
        sources = []
        raison = "Nombre de situations au plus qui se présentent à chaque révolution ; 100 pour l'instant, décision de l'auteur (22 novembre 2026) : tout se décide avant que l'année passe, en attendant un décompte du temps (CHANTIERS, « Le temps du jeu »)" }
      { id = "jeu-reputation-pour"
        valeur = 1.0
        statut = Jugement
        lentille = "a-classer"
        lentillePar = "assistant"
        sources = []
        raison = "Confiance gagnée par un habitant quand on choisit l'option qu'il soutient" }
      { id = "jeu-reputation-contre"
        valeur = 1.0
        statut = Jugement
        lentille = "a-classer"
        lentillePar = "assistant"
        sources = []
        raison = "Confiance perdue par un habitant quand on choisit l'option qu'il redoute" }
      { id = "jeu-reputation-espoir-decu"
        valeur = 0.5
        statut = Jugement
        lentille = "a-classer"
        lentillePar = "assistant"
        sources = []
        raison = "Confiance perdue quand l'option qu'un habitant soutenait n'est pas choisie" }
      { id = "jeu-reputation-crainte-evitee"
        valeur = 0.5
        statut = Jugement
        lentille = "a-classer"
        lentillePar = "assistant"
        sources = []
        raison = "Confiance gagnée quand l'option qu'un habitant redoutait est écartée" }
      { id = "jeu-evenements-par-revolution"
        valeur = 1.0
        statut = Jugement
        lentille = "a-classer"
        lentillePar = "assistant"
        sources = []
        raison = "Nombre d'événements au plus à chaque révolution" }
      { id = "jeu-inertie-revolutions"
        valeur = 2.0
        statut = Jugement
        lentille = "a-classer"
        lentillePar = "assistant"
        sources = []
        raison = "Révolutions qu'il faut à une décision pour produire tout son effet (une part chaque révolution, le reste ensuite)" }
      { id = "jeu-revirement-deception"
        valeur = 1.5
        statut = Jugement
        lentille = "a-classer"
        lentillePar = "assistant"
        sources = []
        raison = "Revenir sur une décision : ce que perdent ceux qui l'approuvaient, en multiple de ce qu'ils avaient gagné (un espoir repris fait plus mal qu'un refus)" }
      { id = "jeu-revirement-soulagement"
        valeur = 0.5
        statut = Jugement
        lentille = "a-classer"
        lentillePar = "assistant"
        sources = []
        raison = "Revenir sur une décision : ce que regagnent ceux qui la redoutaient, en part de ce qu'ils avaient perdu (soulagés, mais méfiants)" }
      { id = "jeu-ressenti-seuil"
        valeur = 25.0
        statut = Jugement
        lentille = "a-classer"
        lentillePar = "assistant"
        sources = []
        raison = "Ressenti au-delà duquel un habitant sans avis sur une question réagit quand même à la décision : somme des variations, en points sur 100, des jauges qui comptent pour lui, chacune multipliée par son poids. Mesuré le 22 novembre 2026 sur 20 parties complètes jouées au hasard (2 880 ressentis) : médiane 0,1, 75e centile 6, 90e centile 18 ; 25 ne retient que les décisions qui touchent vraiment (moins d'une sur dix)" }
      { id = "jeu-reputation-ressenti"
        valeur = 0.25
        statut = Jugement
        lentille = "a-classer"
        lentillePar = "assistant"
        sources = []
        raison = "Confiance gagnée ou perdue par un habitant quand une décision touche nettement ce qui compte pour lui (ressenti au-delà du seuil), qu'il ait eu un avis ou non" }
      { id = "jeu-sensibilisation-perte"
        valeur = 0.05
        statut = Jugement
        lentille = "a-classer"
        lentillePar = "assistant"
        sources = [ "kahneman1979"; "brown2024" ]
        raison = "Ce qu'on vit pèse plus : chaque point perdu sur une jauge qui compte pour un habitant augmente son poids relatif de 5 % (la direction vient de l'aversion à la perte ; l'ampleur est un réglage de jeu)" }
      { id = "jeu-apaisement-gain"
        valeur = 0.025
        statut = Jugement
        lentille = "a-classer"
        lentillePar = "assistant"
        sources = [ "kahneman1979"; "brown2024" ]
        raison = "Un point gagné apaise deux fois moins qu'un point perdu ne sensibilise (sens de l'aversion à la perte ; le rapport exact est un réglage de jeu)" }
      { id = "jeu-apaisement-revolution"
        valeur = 0.3
        statut = Jugement
        lentille = "a-classer"
        lentillePar = "assistant"
        sources = [ "lucas2007" ]
        raison = "Part de la sensibilité qui retombe à chaque révolution : on s'adapte avec le temps, mais pas d'un coup ni complètement" }
      { id = "jeu-sensibilite-plafond"
        valeur = 2.0
        statut = Jugement
        lentille = "a-classer"
        lentillePar = "assistant"
        sources = []
        raison = "Sensibilité au plus : un attachement pèse au plus trois fois son poids de départ" }
      { id = "jeu-confiance-attenuation"
        valeur = 0.1
        statut = Jugement
        lentille = "a-classer"
        lentillePar = "assistant"
        sources = [ "tyler2006" ]
        raison = "La confiance colore le regard : chaque point de confiance d'un habitant réduit de 10 % ce qu'une décision qui le gêne lui fait perdre ; chaque point de méfiance l'augmente d'autant (direction : justice procédurale ; ampleur : réglage de jeu, la confiance allant d'environ −6 à +5 sur 20 parties mesurées le 22 novembre 2026)" }
      { id = "jeu-confiance-plancher"
        valeur = 0.4
        statut = Jugement
        lentille = "a-classer"
        lentillePar = "assistant"
        sources = []
        raison = "Même en pleine confiance, une décision qui gêne un habitant lui coûte au moins 40 % de son effet : la confiance n'efface pas tout" }
      { id = "jeu-mefiance-plafond"
        valeur = 1.6
        statut = Jugement
        lentille = "a-classer"
        lentillePar = "assistant"
        sources = []
        raison = "En pleine méfiance, une décision qui gêne un habitant lui coûte au plus 160 % de son effet" }
      { id = "jeu-age-impressionnable"
        valeur = 25.0
        statut = Jugement
        lentille = "a-classer"
        lentillePar = "assistant"
        sources = [ "krosnick1989" ]
        raison = "La vie avance : jusqu'à cet âge, ce qu'on vit marque davantage (les « années impressionnables » : les opinions changent surtout à la fin de l'adolescence et au début de l'âge adulte)" }
      { id = "jeu-malleabilite-jeunes"
        valeur = 1.5
        statut = Jugement
        lentille = "a-classer"
        lentillePar = "assistant"
        sources = [ "krosnick1989" ]
        raison = "Jusqu'à l'âge impressionnable, une perte ou un gain vécus sensibilisent une fois et demie plus (direction : krosnick1989 ; ampleur : réglage de jeu). Aucun effet n'est appliqué au grand âge : les travaux divergent, et ce serait un stéréotype" }
      { id = "jeu-paris-par-occasion"
        valeur = 3.0
        statut = Jugement
        lentille = "a-classer"
        lentillePar = "assistant"
        sources = []
        raison = "Nombre de paris qu'on peut lancer à la fois quand la question se présente : plusieurs, pour que des paris compatibles se cumulent ; pas tous, pour que le choix garde un sens (on ne peut pas tout entreprendre en même temps)" }
      { id = "prenoms-joueurs-seuil"
        valeur = 3.0
        statut = Jugement
        lentille = "a-classer"
        lentillePar = "assistant"
        sources = []
        raison = "Nombre de déclarations au moins pour qu'un prénom déclaré par des joueurs entre dans une banque : protège les prénoms très rares (l'Insee ne publie pas non plus ceux donnés moins de trois fois) et écarte les plaisanteries isolées" }
      { id = "prenoms-joueurs-minimum"
        valeur = 5.0
        statut = Jugement
        lentille = "a-classer"
        lentillePar = "assistant"
        sources = []
        raison = "Nombre de prénoms déclarés différents (même groupe de pays, même décennie, même usage) à partir duquel ils servent seuls au tirage ; en dessous, ils s'ajoutent aux prénoms les plus portés (INE), pour qu'un seul prénom déclaré ne soit pas donné à tout le monde" }
    |]
