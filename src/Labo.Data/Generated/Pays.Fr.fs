// GÉNÉRÉ depuis data/pays/fr/jauges.csv, data/pays/fr/constantes.csv, data/pays/fr/ressources.csv, data/pays/fr/ressources-options.csv, data/pays/fr/importations.csv, data/pays/fr/exportations.csv par `labo generate`.
// Ne pas éditer : changer les données et régénérer.
module Labo.Data.Pays.Fr

open Labo.Data

let jauges : ValeurPays list =
    [
      { jauge = "securite"
        valeur = Some 0.7777777777777778
        texte = "5 attaques sur 9 tiennent, avec police et prison (jugement)"
        date = "oct. 2026"
        revoir = "jugement, à revoir si le stress test change" }
      { jauge = "criminalite"
        valeur = Some 0.5
        texte = "975 homicides en 2025"
        date = "2024"
        revoir = "chaque année (bilan SSMSI, début d’année)" }
      { jauge = "liberte"
        valeur = Some 0.3355
        texte = "90 020 détenus en prison et au plus 1 959 personnes en centre de rétention à un instant donné (le nombre de places en 2024 ; 40 592 personnes y sont passées dans l’année, dont 24 364 en outre-mer) : environ 133 personnes enfermées pour 100 000 habitants"
        date = "sept. 2026"
        revoir = "chaque mois pour les prisons, chaque année pour la rétention (rapport des associations)" }
      { jauge = "moyens-liberes"
        valeur = Some 0.0
        texte = "29,5 Md€ par an pour les prisons et la police"
        date = "2025"
        revoir = "chaque année (loi de finances)" }
      { jauge = "niveau-de-vie"
        valeur = Some 0.846
        texte = "21,5 MWh par personne en moyenne, mais 15,4 % de la population sous le seuil de pauvreté et un écart de 3,5 entre les 10 % les plus aisés et les 10 % les plus modestes (Insee, 2023)"
        date = "2023"
        revoir = "chaque année (Insee, automne)" }
      { jauge = "nourriture"
        valeur = Some 0.88
        texte = "les terres couvrent 129 % des besoins avec engrais et gazole importés, mais 12 % de la population dit ne pas avoir assez à manger (Crédoc, 2024) et les Restos du Cœur servent 161 millions de repas par an, contre 8,5 millions en 1985"
        date = "2024"
        revoir = "chaque année (Crédoc) ; repas des Restos du Cœur à chaque campagne" }
      { jauge = "ressources"
        valeur = Some 0.9
        texte = "9 ressources sur 10 couvertes, surtout par import"
        date = "oct. 2026"
        revoir = "jugement, à revoir si les importations changent" }
      { jauge = "echanges"
        valeur = Some 0.88
        texte = "les exportations couvrent 88 % des importations (déficit de 81 Md€ en 2024)"
        date = "2024"
        revoir = "chaque année (douanes, février)" }
      { jauge = "pouvoirs"
        valeur = Some 0.45
        texte = "jugement : élections, presse libre et justice indépendante en droit, mais ni tirage au sort ni révocation ; et une justice à deux vitesses constatée depuis les gilets jaunes (2018-2019), puis aux émeutes de 2023 et en octobre 2026 : manifestants jugés en comparution immédiate en deux jours, plaintes contre des policiers en enquête sans date de fin, et aucun bilan officiel de leurs suites (valeur d’octobre 2026, à revoir : la situation bouge vite)"
        date = "oct. 2026"
        revoir = "à chaque grand événement, et au moins tous les trois mois" }
      { jauge = "echelle"
        valeur = Some 1.0
        texte = "le système actuel fonctionne déjà à cette échelle"
        date = "oct. 2026"
        revoir = "sans objet" }
      { jauge = "paris"
        valeur = Some 1.0
        texte = "aucun pari"
        date = "oct. 2026"
        revoir = "sans objet" }
      { jauge = "egalite"
        valeur = Some 0.35
        texte = "jugement : à Mayotte, 77 % des habitants vivent sous le seuil de pauvreté national, contre 14 % en métropole (Insee) ; un jeune homme perçu comme noir ou arabe a 20 fois plus de risques d’être contrôlé (Défenseur des droits) ; 83 % des affaires de violences sexuelles ne sont pas poursuivies, comme pour les autres atteintes à la personne, et la part des viols non poursuivis est passée de 82 % à 92 % entre 2012 et 2019 (IPP)"
        date = "oct. 2026"
        revoir = "chaque année, et à chaque nouvelle enquête (Insee Mayotte, Défenseur des droits, IPP)" }
      { jauge = "principes"
        valeur = Some 0.15
        texte = "jugement : prisons, police, armée et monnaie"
        date = "oct. 2026"
        revoir = "jugement" }
    ]

let constantes : Constante list =
    [
      { id = "population-france-entiere"
        valeur = 69700000.0
        unite = "habitants"
        statut = ADocumenter
        sources = []
        description = "Population, outre-mer compris (valeur de l'ancien moteur)" }
      { id = "population-metropole"
        valeur = 66850000.0
        unite = "habitants"
        statut = ADocumenter
        sources = []
        description = "Population de la métropole (valeur de l'ancien moteur)" }
      { id = "population-aujourdhui"
        valeur = 69.1
        unite = "millions"
        statut = Sourcé
        sources = [ "insee26" ]
        description = "Population en 2026 : base des projections" }
      { id = "population-dans-10-ans"
        valeur = 69.8
        unite = "millions"
        statut = Sourcé
        sources = [ "insee26" ]
        description = "Population projetée dans 10 ans" }
      { id = "population-dans-30-ans"
        valeur = 67.55
        unite = "millions"
        statut = Sourcé
        sources = [ "insee26" ]
        description = "Population projetée dans 30 ans" }
      { id = "ecart-d9-d1"
        valeur = 3.49
        unite = "rapport"
        statut = Sourcé
        sources = [ "insee23" ]
        description = "Écart de niveau de vie entre les 10 % les plus aisés et les 10 % les plus modestes" }
      { id = "pauvrete"
        valeur = 0.154
        unite = "part"
        statut = Sourcé
        sources = [ "insee23" ]
        description = "Part de la population sous le seuil de pauvreté (2023)" }
      { id = "faim"
        valeur = 0.12
        unite = "part"
        statut = Sourcé
        sources = [ "credoc" ]
        description = "Part de la population qui dit ne pas avoir assez à manger (2024)" }
      { id = "energie-aujourdhui"
        valeur = 1496.0
        unite = "TWh"
        statut = ADocumenter
        sources = []
        description = "Énergie disponible aujourd'hui : la référence du niveau de vie" }
      { id = "surface-agricole"
        valeur = 27.9
        unite = "millions d'hectares"
        statut = ADocumenter
        sources = []
        description = "Surface agricole cultivée" }
      { id = "perte-terres-facteur"
        valeur = 44.0
        unite = ""
        statut = ADocumenter
        sources = []
        description = "Facteur de la perte de terres (perte × 44 / 1 000), sens à documenter" }
      { id = "importations-aujourdhui"
        valeur = 698.9
        unite = "Md€"
        statut = Sourcé
        sources = [ "douanes" ]
        description = "Importations en 2024" }
      { id = "exportations-aujourdhui"
        valeur = 617.9
        unite = "Md€"
        statut = Sourcé
        sources = [ "douanes" ]
        description = "Exportations en 2024" }
      { id = "budget-police-prisons"
        valeur = 29.5
        unite = "Md€"
        statut = Sourcé
        sources = [ "budget-ap"; "budget-pn" ]
        description = "Budget des prisons et de la police (2025)" }
      { id = "export-electricite-2024"
        valeur = 5.1
        unite = "Md€"
        statut = ADocumenter
        sources = []
        description = "Exportations d'électricité en 2024 (SDES selon l'ancien moteur)" }
      { id = "tendance-energie"
        valeur = 0.993
        unite = "facteur par an"
        statut = ADocumenter
        sources = []
        description = "Consommation d'énergie : −0,7 % par an depuis 2012, corrigée du climat (SDES selon l'ancien moteur)" }
      { id = "tendance-energie-decalage"
        valeur = 3.0
        unite = "ans"
        statut = ADocumenter
        sources = []
        description = "Années écoulées depuis la dernière mesure de la tendance de l'énergie" }
      { id = "terres-perdues-par-an"
        valeur = 0.06
        unite = "millions d'hectares"
        statut = ADocumenter
        sources = []
        description = "Terres agricoles perdues chaque année (60 000 ha)" }
      { id = "besoin-par-personne"
        valeur = 0.31
        unite = "hectares"
        statut = ADocumenter
        sources = []
        description = "Surface cultivée nécessaire pour nourrir une personne aujourd'hui" }
      { id = "croissance-aide-alimentaire"
        valeur = 1.0385
        unite = "facteur par an"
        statut = Sourcé
        sources = [ "restos" ]
        description = "Repas servis par les Restos du Cœur : +3,9 % par an depuis 20 ans" }
    ]

/// Population, outre-mer compris (valeur de l'ancien moteur) (habitants)
let populationFranceEntiere = 69700000.0

/// Population de la métropole (valeur de l'ancien moteur) (habitants)
let populationMetropole = 66850000.0

/// Population en 2026 : base des projections (millions)
let populationAujourdhui = 69.1

/// Population projetée dans 10 ans (millions)
let populationDans10Ans = 69.8

/// Population projetée dans 30 ans (millions)
let populationDans30Ans = 67.55

/// Écart de niveau de vie entre les 10 % les plus aisés et les 10 % les plus modestes (rapport)
let ecartD9D1 = 3.49

/// Part de la population sous le seuil de pauvreté (2023) (part)
let pauvrete = 0.154

/// Part de la population qui dit ne pas avoir assez à manger (2024) (part)
let faim = 0.12

/// Énergie disponible aujourd'hui : la référence du niveau de vie (TWh)
let energieAujourdhui = 1496.0

/// Surface agricole cultivée (millions d'hectares)
let surfaceAgricole = 27.9

/// Facteur de la perte de terres (perte × 44 / 1 000), sens à documenter ()
let perteTerresFacteur = 44.0

/// Importations en 2024 (Md€)
let importationsAujourdhui = 698.9

/// Exportations en 2024 (Md€)
let exportationsAujourdhui = 617.9

/// Budget des prisons et de la police (2025) (Md€)
let budgetPolicePrisons = 29.5

/// Exportations d'électricité en 2024 (SDES selon l'ancien moteur) (Md€)
let exportElectricite2024 = 5.1

/// Consommation d'énergie : −0,7 % par an depuis 2012, corrigée du climat (SDES selon l'ancien moteur) (facteur par an)
let tendanceEnergie = 0.993

/// Années écoulées depuis la dernière mesure de la tendance de l'énergie (ans)
let tendanceEnergieDecalage = 3.0

/// Terres agricoles perdues chaque année (60 000 ha) (millions d'hectares)
let terresPerduesParAn = 0.06

/// Surface cultivée nécessaire pour nourrir une personne aujourd'hui (hectares)
let besoinParPersonne = 0.31

/// Repas servis par les Restos du Cœur : +3,9 % par an depuis 20 ans (facteur par an)
let croissanceAideAlimentaire = 1.0385

let ressources : RessourcePays list =
    [
      { id = "cereales"
        nom = "Blé et céréales"
        metropole = "part"
        franceEntiere = "part"
        importeeDes = Some 2
        regleePar = None
        pourquoi = "Partielle sans gazole ni engrais importés." }
      { id = "legumes"
        nom = "Légumes et fruits tempérés"
        metropole = "part"
        franceEntiere = "part"
        importeeDes = Some 3
        regleePar = None
        pourquoi = "Légumes frais 86 %, fruits tempérés 80 % d’auto-approvisionnement." }
      { id = "proteines"
        nom = "Protéines pour l’élevage"
        metropole = "part"
        franceEntiere = "part"
        importeeDes = Some 2
        regleePar = None
        pourquoi = "Environ 3,6 millions de tonnes de soja importées par an." }
      { id = "electricite"
        nom = "Électricité"
        metropole = "part"
        franceEntiere = "part"
        importeeDes = Some 1
        regleePar = None
        pourquoi = "L’uranium est entièrement importé." }
      { id = "tropicaux"
        nom = "Fruits tropicaux et riz"
        metropole = "manq"
        franceEntiere = "part"
        importeeDes = Some 2
        regleePar = None
        pourquoi = "Antilles et Réunion pour les fruits. La Guyane produisait du riz (15 000 t en 2006), mais en importe aujourd’hui environ 131 000 t par an." }
      { id = "engrais"
        nom = "Engrais"
        metropole = "manq"
        franceEntiere = "manq"
        importeeDes = Some 2
        regleePar = Some "engrais"
        pourquoi = "Plus de 80 % importés en 2022." }
      { id = "petrole-gaz"
        nom = "Pétrole et gaz"
        metropole = "manq"
        franceEntiere = "manq"
        importeeDes = Some 2
        regleePar = Some "energie"
        pourquoi = "Pas de production notable, outre-mer compris." }
      { id = "metaux"
        nom = "Métaux critiques"
        metropole = "manq"
        franceEntiere = "part"
        importeeDes = Some 1
        regleePar = None
        pourquoi = "Nickel et cobalt de Nouvelle-Calédonie ; lithium et terres rares toujours absents." }
      { id = "medicaments"
        nom = "Médicaments"
        metropole = "manq"
        franceEntiere = "manq"
        importeeDes = Some 1
        regleePar = Some "medic"
        pourquoi = "Principes actifs surtout importés d’Asie." }
      { id = "eau-terres"
        nom = "Eau, terres, forêt"
        metropole = "nv"
        franceEntiere = "nv"
        importeeDes = None
        regleePar = None
        pourquoi = "La Guyane ajoute une très grande forêt tropicale (environ 8 millions d’hectares, de mémoire)." }
    ]

let ressourcesOptions : OptionRessource list =
    [
      { levier = "engrais"
        option = "organique"
        couverture = "part"
        importe = false
        cout = "Rendements plus bas, de l’ordre de 20 à 25 % comme en bio, et des légumineuses qui prennent de la surface. Phosphore et potasse à recycler (fumiers, composts, boues)."
        effet = "Perte de rendement réglée à 25 % dans la table Calculs ; engrais « partiel » dans le test de fermeture." }
      { levier = "engrais"
        option = "moitie"
        couverture = "manq"
        importe = true
        cout = "Inspiré de l’objectif public de réduire de moitié la consommation d’engrais azotés. Les usines actuelles tournent au gaz importé : sans échanges ciblés ou ouverture, elles s’arrêtent et les rendements retombent comme en bio."
        effet = "Perte de rendement réglée à 12 % si du gaz peut être importé (échanges ciblés ou ouvert) ; sinon 25 %." }
      { levier = "engrais"
        option = "electrique"
        couverture = "part"
        importe = false
        cout = "Environ 28 TWh d’électricité par an, retirés de l’énergie disponible : environ 2,1 millions de tonnes d’azote de synthèse, et 10 à 12 MWh par tonne d’ammoniac. Les perspectives industrielles restent lointaines ; phosphore et potasse restent à trouver."
        effet = "Perte de rendement réglée à 5 %, et 28 TWh retirés de l’énergie disponible (hypothèse)." }
      { levier = "energie"
        option = "renouvelables"
        couverture = "part"
        importe = false
        cout = "Faisabilité contestée (intermittence) ; panneaux, éoliennes et batteries demandent des métaux importés."
        effet = "Énergie disponible réglée à 776 TWh dans la table Calculs." }
      { levier = "energie"
        option = "nucleaire-stocks"
        couverture = "part"
        importe = false
        cout = "Les stocks d’uranium tiennent une dizaine d’années ; ensuite, il faut importer ou construire des réacteurs à neutrons rapides, qui n’existent pas en service en France."
        effet = "Énergie disponible réglée à 829 TWh (repère : scénario ADEME le plus sobre) ; à revoir dans une dizaine d’années." }
      { levier = "energie"
        option = "uranium-importe"
        couverture = "manq"
        importe = true
        cout = "Le système s’ouvre : il faut une monnaie à la frontière et des partenaires."
        effet = "Énergie disponible réglée à 1 062 TWh ; dépend des importations." }
      { levier = "medic"
        option = "produire"
        couverture = "part"
        importe = false
        cout = "Des années de construction d’usines ; la chimie de base et certains précurseurs restent importés. Demande une coordination nationale de la santé."
        effet = "Ajoute la santé aux domaines décidés au national (table Politique)." }
      { levier = "medic"
        option = "principes-importes"
        couverture = "manq"
        importe = true
        cout = "La dépendance demeure ; il faut une monnaie à la frontière."
        effet = "Demande une monnaie, au moins à la frontière." }
      { levier = "medic"
        option = "liste-reduite"
        couverture = "manq"
        importe = true
        cout = "Certains traitements (cancers, maladies rares) deviennent introuvables : la mortalité augmente."
        effet = "Les stocks s’épuisent : il faudra importer un jour." }
    ]

let importations : Importation list =
    [
      { ouverture = 1
        rang = 0
        libelle = "Médicaments essentiels"
        valeur = 5.0
        note = "hypothèse : environ 30 % des 16,8 Md€ de médicaments importés en 2023" }
      { ouverture = 1
        rang = 1
        libelle = "Métaux critiques"
        valeur = 3.0
        note = "hypothèse" }
      { ouverture = 1
        rang = 2
        libelle = "Uranium"
        valeur = 1.0
        note = "hypothèse" }
      { ouverture = 2
        rang = 0
        libelle = "Engrais"
        valeur = 4.0
        note = "le marché français des engrais pèse près de 5 Md€, dont 80 % de l’azote importé" }
      { ouverture = 2
        rang = 1
        libelle = "Pétrole et gaz en quantité limitée"
        valeur = 15.0
        note = "hypothèse : un quart de la facture de 2024 (61,2 Md€)" }
      { ouverture = 2
        rang = 2
        libelle = "Fruits tropicaux, riz et soja"
        valeur = 4.0
        note = "les oléagineux, surtout le soja, coûtent 1,8 Md€ net (2021) ; le reste en hypothèse" }
    ]

let exportations : Exportation list =
    [
      { id = "elec"
        libelle = "Électricité excédentaire"
        parDefaut = true
        valeur = Calcul "export-electricite"
        note = "5,1 Md€ en 2024 (SDES), plus l’excédent apporté par les paris" }
      { id = "food"
        libelle = "Excédent de nourriture"
        parDefaut = true
        valeur = Calcul "export-nourriture"
        note = "hypothèse : 0,2 Md€ par million de personnes nourries au-delà de la population" }
      { id = "vin"
        libelle = "Vins, spiritueux et fromages"
        parDefaut = false
        valeur = Fixe 15.6
        note = "vins et spiritueux : 15,6 Md€ en 2024" }
      { id = "savoir"
        libelle = "Savoir-faire et services (ingénierie, médecine, formation)"
        parDefaut = false
        valeur = Fixe 5.0
        note = "hypothèse" }
      { id = "accueil"
        libelle = "Accueil de visiteurs"
        parDefaut = false
        valeur = Fixe 10.0
        note = "hypothèse" }
      { id = "bois"
        libelle = "Bois et produits de la forêt"
        parDefaut = false
        valeur = Fixe 1.0
        note = "hypothèse" }
      { id = "industrie"
        libelle = "Une partie de l’industrie exportatrice actuelle (aéronautique, luxe, pharmacie)"
        parDefaut = false
        valeur = Fixe 30.0
        note = "hypothèse ; demande de grandes entreprises, ce qui s’écarte des principes" }
    ]
