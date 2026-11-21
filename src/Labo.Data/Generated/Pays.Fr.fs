// GÉNÉRÉ depuis data/pays/fr/jauges.csv, data/pays/fr/constantes.csv, data/pays/fr/ressources.csv, data/pays/fr/ressources-options.csv, data/pays/fr/importations.csv, data/pays/fr/exportations.csv, data/pays/fr/prenoms.csv par `labo generate`.
// Ne pas éditer : changer les données et régénérer.
module Labo.Data.Pays.Fr

open Labo.Data

let jauges : ValeurPays list =
    List.ofArray [|
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
    |]

let constantes : Constante list =
    List.ofArray [|
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
      { id = "annee-depart"
        valeur = 2026.0
        unite = "année"
        statut = Jugement
        sources = []
        description = "L'année de la révolution 0 : la France d'aujourd'hui" }
    |]

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

/// L'année de la révolution 0 : la France d'aujourd'hui (année)
let anneeDepart = 2026.0

let ressources : RessourcePays list =
    List.ofArray [|
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
    |]

let ressourcesOptions : OptionRessource list =
    List.ofArray [|
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
    |]

let importations : Importation list =
    List.ofArray [|
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
    |]

let exportations : Exportation list =
    List.ofArray [|
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
    |]

let prenoms : Prenom list =
    List.ofArray [|
      { sexe = "f"; decennie = 1900; prenom = "Marie"; naissances = 490609 }
      { sexe = "f"; decennie = 1900; prenom = "Jeanne"; naissances = 150267 }
      { sexe = "f"; decennie = 1900; prenom = "Marguerite"; naissances = 89438 }
      { sexe = "f"; decennie = 1900; prenom = "Germaine"; naissances = 78921 }
      { sexe = "f"; decennie = 1900; prenom = "Louise"; naissances = 67732 }
      { sexe = "f"; decennie = 1900; prenom = "Yvonne"; naissances = 65428 }
      { sexe = "f"; decennie = 1900; prenom = "Madeleine"; naissances = 64909 }
      { sexe = "f"; decennie = 1900; prenom = "Suzanne"; naissances = 61760 }
      { sexe = "f"; decennie = 1900; prenom = "Marcelle"; naissances = 51025 }
      { sexe = "f"; decennie = 1900; prenom = "Marthe"; naissances = 42395 }
      { sexe = "f"; decennie = 1900; prenom = "Maria"; naissances = 36527 }
      { sexe = "f"; decennie = 1900; prenom = "Anne"; naissances = 36492 }
      { sexe = "f"; decennie = 1900; prenom = "Henriette"; naissances = 35370 }
      { sexe = "f"; decennie = 1900; prenom = "Lucienne"; naissances = 34799 }
      { sexe = "f"; decennie = 1900; prenom = "Renée"; naissances = 34643 }
      { sexe = "f"; decennie = 1900; prenom = "Hélène"; naissances = 34422 }
      { sexe = "f"; decennie = 1900; prenom = "Alice"; naissances = 34030 }
      { sexe = "f"; decennie = 1900; prenom = "Georgette"; naissances = 31103 }
      { sexe = "f"; decennie = 1900; prenom = "Josephine"; naissances = 30740 }
      { sexe = "f"; decennie = 1900; prenom = "Berthe"; naissances = 30262 }
      { sexe = "f"; decennie = 1900; prenom = "Lucie"; naissances = 28201 }
      { sexe = "f"; decennie = 1900; prenom = "Eugénie"; naissances = 27889 }
      { sexe = "f"; decennie = 1900; prenom = "Andrée"; naissances = 26143 }
      { sexe = "f"; decennie = 1900; prenom = "Juliette"; naissances = 26049 }
      { sexe = "f"; decennie = 1900; prenom = "Fernande"; naissances = 23033 }
      { sexe = "f"; decennie = 1900; prenom = "Gabrielle"; naissances = 21223 }
      { sexe = "f"; decennie = 1900; prenom = "Raymonde"; naissances = 21012 }
      { sexe = "f"; decennie = 1900; prenom = "Anna"; naissances = 20550 }
      { sexe = "f"; decennie = 1900; prenom = "Denise"; naissances = 20135 }
      { sexe = "f"; decennie = 1900; prenom = "Angèle"; naissances = 19181 }
      { sexe = "f"; decennie = 1900; prenom = "Simonne"; naissances = 18732 }
      { sexe = "f"; decennie = 1900; prenom = "Thérèse"; naissances = 17934 }
      { sexe = "f"; decennie = 1900; prenom = "Odette"; naissances = 17540 }
      { sexe = "f"; decennie = 1900; prenom = "Augustine"; naissances = 17391 }
      { sexe = "f"; decennie = 1900; prenom = "Rose"; naissances = 17284 }
      { sexe = "f"; decennie = 1900; prenom = "Antoinette"; naissances = 16871 }
      { sexe = "f"; decennie = 1900; prenom = "Catherine"; naissances = 15340 }
      { sexe = "f"; decennie = 1900; prenom = "Elise"; naissances = 15328 }
      { sexe = "f"; decennie = 1900; prenom = "Blanche"; naissances = 14950 }
      { sexe = "f"; decennie = 1900; prenom = "Simone"; naissances = 14877 }
      { sexe = "f"; decennie = 1900; prenom = "Léonie"; naissances = 14464 }
      { sexe = "f"; decennie = 1900; prenom = "Françoise"; naissances = 14261 }
      { sexe = "f"; decennie = 1900; prenom = "Cécile"; naissances = 12754 }
      { sexe = "f"; decennie = 1900; prenom = "Charlotte"; naissances = 12481 }
      { sexe = "f"; decennie = 1900; prenom = "Geneviève"; naissances = 12419 }
      { sexe = "f"; decennie = 1900; prenom = "Emilienne"; naissances = 11737 }
      { sexe = "f"; decennie = 1900; prenom = "Marie-Louise"; naissances = 11563 }
      { sexe = "f"; decennie = 1900; prenom = "Adrienne"; naissances = 11434 }
      { sexe = "f"; decennie = 1900; prenom = "Pauline"; naissances = 11265 }
      { sexe = "f"; decennie = 1900; prenom = "Mathilde"; naissances = 11084 }
      { sexe = "f"; decennie = 1900; prenom = "Léontine"; naissances = 10890 }
      { sexe = "f"; decennie = 1900; prenom = "Emilie"; naissances = 10567 }
      { sexe = "f"; decennie = 1900; prenom = "Léa"; naissances = 10380 }
      { sexe = "f"; decennie = 1900; prenom = "Albertine"; naissances = 10092 }
      { sexe = "f"; decennie = 1900; prenom = "Claire"; naissances = 9876 }
      { sexe = "f"; decennie = 1900; prenom = "Elisabeth"; naissances = 9842 }
      { sexe = "f"; decennie = 1900; prenom = "Julie"; naissances = 9539 }
      { sexe = "f"; decennie = 1900; prenom = "Julienne"; naissances = 9325 }
      { sexe = "f"; decennie = 1900; prenom = "Irène"; naissances = 8505 }
      { sexe = "f"; decennie = 1900; prenom = "Amélie"; naissances = 8281 }
      { sexe = "f"; decennie = 1900; prenom = "Emma"; naissances = 8270 }
      { sexe = "f"; decennie = 1900; prenom = "Julia"; naissances = 7829 }
      { sexe = "f"; decennie = 1900; prenom = "Paule"; naissances = 7728 }
      { sexe = "f"; decennie = 1900; prenom = "Paulette"; naissances = 7600 }
      { sexe = "f"; decennie = 1900; prenom = "Aline"; naissances = 7574 }
      { sexe = "f"; decennie = 1900; prenom = "Camille"; naissances = 7455 }
      { sexe = "f"; decennie = 1900; prenom = "Victorine"; naissances = 7006 }
      { sexe = "f"; decennie = 1900; prenom = "Ernestine"; naissances = 6913 }
      { sexe = "f"; decennie = 1900; prenom = "Aimée"; naissances = 6846 }
      { sexe = "f"; decennie = 1900; prenom = "Gilberte"; naissances = 6756 }
      { sexe = "f"; decennie = 1900; prenom = "Rosalie"; naissances = 6672 }
      { sexe = "f"; decennie = 1900; prenom = "Reine"; naissances = 6605 }
      { sexe = "f"; decennie = 1900; prenom = "Valentine"; naissances = 6554 }
      { sexe = "f"; decennie = 1900; prenom = "Olga"; naissances = 6358 }
      { sexe = "f"; decennie = 1900; prenom = "Clémence"; naissances = 6259 }
      { sexe = "f"; decennie = 1900; prenom = "Francine"; naissances = 6114 }
      { sexe = "f"; decennie = 1900; prenom = "Adèle"; naissances = 6107 }
      { sexe = "f"; decennie = 1900; prenom = "Augusta"; naissances = 5794 }
      { sexe = "f"; decennie = 1900; prenom = "Alphonsine"; naissances = 5733 }
      { sexe = "f"; decennie = 1900; prenom = "Mélanie"; naissances = 5580 }
      { sexe = "f"; decennie = 1900; prenom = "Solange"; naissances = 5501 }
      { sexe = "f"; decennie = 1900; prenom = "Andréa"; naissances = 5457 }
      { sexe = "f"; decennie = 1900; prenom = "Isabelle"; naissances = 5286 }
      { sexe = "f"; decennie = 1900; prenom = "Céline"; naissances = 5098 }
      { sexe = "f"; decennie = 1900; prenom = "Clémentine"; naissances = 5008 }
      { sexe = "f"; decennie = 1900; prenom = "Clotilde"; naissances = 4997 }
      { sexe = "f"; decennie = 1900; prenom = "Agnès"; naissances = 4831 }
      { sexe = "f"; decennie = 1900; prenom = "Laure"; naissances = 4795 }
      { sexe = "f"; decennie = 1900; prenom = "Philomene"; naissances = 4771 }
      { sexe = "f"; decennie = 1900; prenom = "Rachel"; naissances = 4407 }
      { sexe = "f"; decennie = 1900; prenom = "Irma"; naissances = 4358 }
      { sexe = "f"; decennie = 1900; prenom = "Celestine"; naissances = 4286 }
      { sexe = "f"; decennie = 1900; prenom = "Felicie"; naissances = 4262 }
      { sexe = "f"; decennie = 1900; prenom = "Rosa"; naissances = 4212 }
      { sexe = "f"; decennie = 1900; prenom = "Caroline"; naissances = 4149 }
      { sexe = "f"; decennie = 1900; prenom = "Elisa"; naissances = 4058 }
      { sexe = "f"; decennie = 1900; prenom = "Alexandrine"; naissances = 3921 }
      { sexe = "f"; decennie = 1900; prenom = "Virginie"; naissances = 3602 }
      { sexe = "f"; decennie = 1900; prenom = "Eva"; naissances = 3475 }
      { sexe = "f"; decennie = 1900; prenom = "Ida"; naissances = 3464 }
      { sexe = "f"; decennie = 1900; prenom = "Sophie"; naissances = 3380 }
      { sexe = "f"; decennie = 1900; prenom = "Bernadette"; naissances = 3316 }
      { sexe = "f"; decennie = 1900; prenom = "Léone"; naissances = 3219 }
      { sexe = "f"; decennie = 1900; prenom = "Laurence"; naissances = 3186 }
      { sexe = "f"; decennie = 1900; prenom = "Claudine"; naissances = 3161 }
      { sexe = "f"; decennie = 1900; prenom = "Marie-Thérèse"; naissances = 3066 }
      { sexe = "f"; decennie = 1900; prenom = "Claudia"; naissances = 2984 }
      { sexe = "f"; decennie = 1900; prenom = "Yvette"; naissances = 2961 }
      { sexe = "f"; decennie = 1900; prenom = "Justine"; naissances = 2863 }
      { sexe = "f"; decennie = 1910; prenom = "Marie"; naissances = 329246 }
      { sexe = "f"; decennie = 1910; prenom = "Jeanne"; naissances = 121277 }
      { sexe = "f"; decennie = 1910; prenom = "Madeleine"; naissances = 66630 }
      { sexe = "f"; decennie = 1910; prenom = "Marguerite"; naissances = 64108 }
      { sexe = "f"; decennie = 1910; prenom = "Yvonne"; naissances = 60613 }
      { sexe = "f"; decennie = 1910; prenom = "Suzanne"; naissances = 59349 }
      { sexe = "f"; decennie = 1910; prenom = "Germaine"; naissances = 53770 }
      { sexe = "f"; decennie = 1910; prenom = "Marcelle"; naissances = 52870 }
      { sexe = "f"; decennie = 1910; prenom = "Louise"; naissances = 45968 }
      { sexe = "f"; decennie = 1910; prenom = "Renée"; naissances = 41686 }
      { sexe = "f"; decennie = 1910; prenom = "Lucienne"; naissances = 38934 }
      { sexe = "f"; decennie = 1910; prenom = "Andrée"; naissances = 36469 }
      { sexe = "f"; decennie = 1910; prenom = "Denise"; naissances = 34185 }
      { sexe = "f"; decennie = 1910; prenom = "Simone"; naissances = 33571 }
      { sexe = "f"; decennie = 1910; prenom = "Raymonde"; naissances = 33506 }
      { sexe = "f"; decennie = 1910; prenom = "Georgette"; naissances = 32144 }
      { sexe = "f"; decennie = 1910; prenom = "Paulette"; naissances = 31941 }
      { sexe = "f"; decennie = 1910; prenom = "Odette"; naissances = 31535 }
      { sexe = "f"; decennie = 1910; prenom = "Marthe"; naissances = 30908 }
      { sexe = "f"; decennie = 1910; prenom = "Henriette"; naissances = 30796 }
      { sexe = "f"; decennie = 1910; prenom = "Simonne"; naissances = 30519 }
      { sexe = "f"; decennie = 1910; prenom = "Anne"; naissances = 29851 }
      { sexe = "f"; decennie = 1910; prenom = "Hélène"; naissances = 29169 }
      { sexe = "f"; decennie = 1910; prenom = "Alice"; naissances = 26162 }
      { sexe = "f"; decennie = 1910; prenom = "Fernande"; naissances = 21981 }
      { sexe = "f"; decennie = 1910; prenom = "Maria"; naissances = 21780 }
      { sexe = "f"; decennie = 1910; prenom = "Juliette"; naissances = 20011 }
      { sexe = "f"; decennie = 1910; prenom = "Josephine"; naissances = 19579 }
      { sexe = "f"; decennie = 1910; prenom = "Geneviève"; naissances = 18735 }
      { sexe = "f"; decennie = 1910; prenom = "Lucie"; naissances = 18488 }
      { sexe = "f"; decennie = 1910; prenom = "Berthe"; naissances = 18393 }
      { sexe = "f"; decennie = 1910; prenom = "Thérèse"; naissances = 16344 }
      { sexe = "f"; decennie = 1910; prenom = "Gabrielle"; naissances = 16040 }
      { sexe = "f"; decennie = 1910; prenom = "Eugénie"; naissances = 15343 }
      { sexe = "f"; decennie = 1910; prenom = "Antoinette"; naissances = 14141 }
      { sexe = "f"; decennie = 1910; prenom = "Anna"; naissances = 13963 }
      { sexe = "f"; decennie = 1910; prenom = "Angèle"; naissances = 13016 }
      { sexe = "f"; decennie = 1910; prenom = "Rose"; naissances = 12520 }
      { sexe = "f"; decennie = 1910; prenom = "Elise"; naissances = 11792 }
      { sexe = "f"; decennie = 1910; prenom = "Emilienne"; naissances = 11468 }
      { sexe = "f"; decennie = 1910; prenom = "Françoise"; naissances = 11188 }
      { sexe = "f"; decennie = 1910; prenom = "Yvette"; naissances = 11040 }
      { sexe = "f"; decennie = 1910; prenom = "Gilberte"; naissances = 10939 }
      { sexe = "f"; decennie = 1910; prenom = "Solange"; naissances = 10471 }
      { sexe = "f"; decennie = 1910; prenom = "Cécile"; naissances = 10443 }
      { sexe = "f"; decennie = 1910; prenom = "Jacqueline"; naissances = 10080 }
      { sexe = "f"; decennie = 1910; prenom = "Charlotte"; naissances = 9986 }
      { sexe = "f"; decennie = 1910; prenom = "Augustine"; naissances = 9919 }
      { sexe = "f"; decennie = 1910; prenom = "Irène"; naissances = 9738 }
      { sexe = "f"; decennie = 1910; prenom = "Marie-Louise"; naissances = 9158 }
      { sexe = "f"; decennie = 1910; prenom = "Elisabeth"; naissances = 9123 }
      { sexe = "f"; decennie = 1910; prenom = "Adrienne"; naissances = 8819 }
      { sexe = "f"; decennie = 1910; prenom = "Paule"; naissances = 8815 }
      { sexe = "f"; decennie = 1910; prenom = "Catherine"; naissances = 8810 }
      { sexe = "f"; decennie = 1910; prenom = "Léonie"; naissances = 8301 }
      { sexe = "f"; decennie = 1910; prenom = "Blanche"; naissances = 8131 }
      { sexe = "f"; decennie = 1910; prenom = "Albertine"; naissances = 7826 }
      { sexe = "f"; decennie = 1910; prenom = "Julienne"; naissances = 7550 }
      { sexe = "f"; decennie = 1910; prenom = "Léa"; naissances = 7224 }
      { sexe = "f"; decennie = 1910; prenom = "Claire"; naissances = 7122 }
      { sexe = "f"; decennie = 1910; prenom = "Mathilde"; naissances = 6986 }
      { sexe = "f"; decennie = 1910; prenom = "Gisèle"; naissances = 6847 }
      { sexe = "f"; decennie = 1910; prenom = "Emilie"; naissances = 6647 }
      { sexe = "f"; decennie = 1910; prenom = "Pauline"; naissances = 6456 }
      { sexe = "f"; decennie = 1910; prenom = "Francine"; naissances = 6398 }
      { sexe = "f"; decennie = 1910; prenom = "Léontine"; naissances = 6115 }
      { sexe = "f"; decennie = 1910; prenom = "Aimée"; naissances = 5893 }
      { sexe = "f"; decennie = 1910; prenom = "Aline"; naissances = 5803 }
      { sexe = "f"; decennie = 1910; prenom = "Reine"; naissances = 5781 }
      { sexe = "f"; decennie = 1910; prenom = "Camille"; naissances = 5701 }
      { sexe = "f"; decennie = 1910; prenom = "Emma"; naissances = 5594 }
      { sexe = "f"; decennie = 1910; prenom = "Andréa"; naissances = 5509 }
      { sexe = "f"; decennie = 1910; prenom = "Bernadette"; naissances = 5334 }
      { sexe = "f"; decennie = 1910; prenom = "Christiane"; naissances = 5297 }
      { sexe = "f"; decennie = 1910; prenom = "Augusta"; naissances = 5046 }
      { sexe = "f"; decennie = 1910; prenom = "Amélie"; naissances = 4937 }
      { sexe = "f"; decennie = 1910; prenom = "Mireille"; naissances = 4862 }
      { sexe = "f"; decennie = 1910; prenom = "Léone"; naissances = 4767 }
      { sexe = "f"; decennie = 1910; prenom = "France"; naissances = 4693 }
      { sexe = "f"; decennie = 1910; prenom = "Julie"; naissances = 4596 }
      { sexe = "f"; decennie = 1910; prenom = "Rolande"; naissances = 4582 }
      { sexe = "f"; decennie = 1910; prenom = "Olga"; naissances = 4553 }
      { sexe = "f"; decennie = 1910; prenom = "Julia"; naissances = 4505 }
      { sexe = "f"; decennie = 1910; prenom = "Agnès"; naissances = 4399 }
      { sexe = "f"; decennie = 1910; prenom = "Pierrette"; naissances = 4363 }
      { sexe = "f"; decennie = 1910; prenom = "Ernestine"; naissances = 4088 }
      { sexe = "f"; decennie = 1910; prenom = "Rosalie"; naissances = 3940 }
      { sexe = "f"; decennie = 1910; prenom = "Victorine"; naissances = 3827 }
      { sexe = "f"; decennie = 1910; prenom = "Adèle"; naissances = 3746 }
      { sexe = "f"; decennie = 1910; prenom = "Lucette"; naissances = 3731 }
      { sexe = "f"; decennie = 1910; prenom = "Clémence"; naissances = 3672 }
      { sexe = "f"; decennie = 1910; prenom = "Marie-Thérèse"; naissances = 3629 }
      { sexe = "f"; decennie = 1910; prenom = "Alphonsine"; naissances = 3620 }
      { sexe = "f"; decennie = 1910; prenom = "Isabelle"; naissances = 3584 }
      { sexe = "f"; decennie = 1910; prenom = "Valentine"; naissances = 3568 }
      { sexe = "f"; decennie = 1910; prenom = "Colette"; naissances = 3485 }
      { sexe = "f"; decennie = 1910; prenom = "Carmen"; naissances = 3375 }
      { sexe = "f"; decennie = 1910; prenom = "Eliane"; naissances = 3299 }
      { sexe = "f"; decennie = 1910; prenom = "Laure"; naissances = 3280 }
      { sexe = "f"; decennie = 1910; prenom = "Clotilde"; naissances = 3243 }
      { sexe = "f"; decennie = 1910; prenom = "Jeannine"; naissances = 3153 }
      { sexe = "f"; decennie = 1910; prenom = "Céline"; naissances = 3077 }
      { sexe = "f"; decennie = 1910; prenom = "Mélanie"; naissances = 3004 }
      { sexe = "f"; decennie = 1910; prenom = "Micheline"; naissances = 2997 }
      { sexe = "f"; decennie = 1910; prenom = "Huguette"; naissances = 2898 }
      { sexe = "f"; decennie = 1910; prenom = "Ida"; naissances = 2894 }
      { sexe = "f"; decennie = 1910; prenom = "Rosa"; naissances = 2862 }
      { sexe = "f"; decennie = 1910; prenom = "Irma"; naissances = 2813 }
      { sexe = "f"; decennie = 1910; prenom = "Philomene"; naissances = 2761 }
      { sexe = "f"; decennie = 1910; prenom = "Rachel"; naissances = 2752 }
      { sexe = "f"; decennie = 1910; prenom = "Clémentine"; naissances = 2736 }
      { sexe = "f"; decennie = 1910; prenom = "Edith"; naissances = 2690 }
      { sexe = "f"; decennie = 1910; prenom = "Odile"; naissances = 2578 }
      { sexe = "f"; decennie = 1910; prenom = "Mauricette"; naissances = 2554 }
      { sexe = "f"; decennie = 1910; prenom = "Yolande"; naissances = 2506 }
      { sexe = "f"; decennie = 1910; prenom = "Celestine"; naissances = 2502 }
      { sexe = "f"; decennie = 1910; prenom = "Felicie"; naissances = 2500 }
      { sexe = "f"; decennie = 1920; prenom = "Marie"; naissances = 322486 }
      { sexe = "f"; decennie = 1920; prenom = "Jeanne"; naissances = 127574 }
      { sexe = "f"; decennie = 1920; prenom = "Jacqueline"; naissances = 81985 }
      { sexe = "f"; decennie = 1920; prenom = "Madeleine"; naissances = 79952 }
      { sexe = "f"; decennie = 1920; prenom = "Suzanne"; naissances = 78364 }
      { sexe = "f"; decennie = 1920; prenom = "Denise"; naissances = 78138 }
      { sexe = "f"; decennie = 1920; prenom = "Paulette"; naissances = 76824 }
      { sexe = "f"; decennie = 1920; prenom = "Yvonne"; naissances = 71462 }
      { sexe = "f"; decennie = 1920; prenom = "Simone"; naissances = 70770 }
      { sexe = "f"; decennie = 1920; prenom = "Odette"; naissances = 70768 }
      { sexe = "f"; decennie = 1920; prenom = "Marcelle"; naissances = 64680 }
      { sexe = "f"; decennie = 1920; prenom = "Renée"; naissances = 64309 }
      { sexe = "f"; decennie = 1920; prenom = "Andrée"; naissances = 61653 }
      { sexe = "f"; decennie = 1920; prenom = "Jeannine"; naissances = 59586 }
      { sexe = "f"; decennie = 1920; prenom = "Marguerite"; naissances = 58671 }
      { sexe = "f"; decennie = 1920; prenom = "Yvette"; naissances = 57919 }
      { sexe = "f"; decennie = 1920; prenom = "Lucienne"; naissances = 49029 }
      { sexe = "f"; decennie = 1920; prenom = "Raymonde"; naissances = 48969 }
      { sexe = "f"; decennie = 1920; prenom = "Germaine"; naissances = 46954 }
      { sexe = "f"; decennie = 1920; prenom = "Georgette"; naissances = 45031 }
      { sexe = "f"; decennie = 1920; prenom = "Louise"; naissances = 37355 }
      { sexe = "f"; decennie = 1920; prenom = "Geneviève"; naissances = 37346 }
      { sexe = "f"; decennie = 1920; prenom = "Hélène"; naissances = 36912 }
      { sexe = "f"; decennie = 1920; prenom = "Simonne"; naissances = 36901 }
      { sexe = "f"; decennie = 1920; prenom = "Thérèse"; naissances = 36799 }
      { sexe = "f"; decennie = 1920; prenom = "Ginette"; naissances = 36354 }
      { sexe = "f"; decennie = 1920; prenom = "Anne"; naissances = 35926 }
      { sexe = "f"; decennie = 1920; prenom = "Gisèle"; naissances = 31926 }
      { sexe = "f"; decennie = 1920; prenom = "Henriette"; naissances = 31824 }
      { sexe = "f"; decennie = 1920; prenom = "Alice"; naissances = 27568 }
      { sexe = "f"; decennie = 1920; prenom = "Christiane"; naissances = 27516 }
      { sexe = "f"; decennie = 1920; prenom = "Marthe"; naissances = 26729 }
      { sexe = "f"; decennie = 1920; prenom = "Micheline"; naissances = 25593 }
      { sexe = "f"; decennie = 1920; prenom = "Monique"; naissances = 24104 }
      { sexe = "f"; decennie = 1920; prenom = "Fernande"; naissances = 22669 }
      { sexe = "f"; decennie = 1920; prenom = "Huguette"; naissances = 22547 }
      { sexe = "f"; decennie = 1920; prenom = "Colette"; naissances = 21700 }
      { sexe = "f"; decennie = 1920; prenom = "Solange"; naissances = 20282 }
      { sexe = "f"; decennie = 1920; prenom = "Juliette"; naissances = 19787 }
      { sexe = "f"; decennie = 1920; prenom = "Josette"; naissances = 19549 }
      { sexe = "f"; decennie = 1920; prenom = "Irène"; naissances = 19452 }
      { sexe = "f"; decennie = 1920; prenom = "Gilberte"; naissances = 19229 }
      { sexe = "f"; decennie = 1920; prenom = "Françoise"; naissances = 18803 }
      { sexe = "f"; decennie = 1920; prenom = "Maria"; naissances = 18596 }
      { sexe = "f"; decennie = 1920; prenom = "Lucette"; naissances = 18426 }
      { sexe = "f"; decennie = 1920; prenom = "Lucie"; naissances = 17197 }
      { sexe = "f"; decennie = 1920; prenom = "Antoinette"; naissances = 16940 }
      { sexe = "f"; decennie = 1920; prenom = "Gabrielle"; naissances = 16437 }
      { sexe = "f"; decennie = 1920; prenom = "Josephine"; naissances = 15528 }
      { sexe = "f"; decennie = 1920; prenom = "Pierrette"; naissances = 15471 }
      { sexe = "f"; decennie = 1920; prenom = "Janine"; naissances = 15436 }
      { sexe = "f"; decennie = 1920; prenom = "Eliane"; naissances = 14776 }
      { sexe = "f"; decennie = 1920; prenom = "Anna"; naissances = 13887 }
      { sexe = "f"; decennie = 1920; prenom = "Rose"; naissances = 13675 }
      { sexe = "f"; decennie = 1920; prenom = "Berthe"; naissances = 13319 }
      { sexe = "f"; decennie = 1920; prenom = "Cécile"; naissances = 13225 }
      { sexe = "f"; decennie = 1920; prenom = "Elise"; naissances = 12753 }
      { sexe = "f"; decennie = 1920; prenom = "Angèle"; naissances = 12212 }
      { sexe = "f"; decennie = 1920; prenom = "Emilienne"; naissances = 12156 }
      { sexe = "f"; decennie = 1920; prenom = "Paule"; naissances = 12010 }
      { sexe = "f"; decennie = 1920; prenom = "Rolande"; naissances = 11130 }
      { sexe = "f"; decennie = 1920; prenom = "Bernadette"; naissances = 10931 }
      { sexe = "f"; decennie = 1920; prenom = "Mireille"; naissances = 10788 }
      { sexe = "f"; decennie = 1920; prenom = "Mauricette"; naissances = 10459 }
      { sexe = "f"; decennie = 1920; prenom = "Francine"; naissances = 10180 }
      { sexe = "f"; decennie = 1920; prenom = "Eugénie"; naissances = 9919 }
      { sexe = "f"; decennie = 1920; prenom = "Jeanine"; naissances = 9589 }
      { sexe = "f"; decennie = 1920; prenom = "Marie-Louise"; naissances = 9426 }
      { sexe = "f"; decennie = 1920; prenom = "Marie-Thérèse"; naissances = 9399 }
      { sexe = "f"; decennie = 1920; prenom = "Liliane"; naissances = 9321 }
      { sexe = "f"; decennie = 1920; prenom = "Elisabeth"; naissances = 8704 }
      { sexe = "f"; decennie = 1920; prenom = "Charlotte"; naissances = 8649 }
      { sexe = "f"; decennie = 1920; prenom = "Arlette"; naissances = 8495 }
      { sexe = "f"; decennie = 1920; prenom = "Yolande"; naissances = 8070 }
      { sexe = "f"; decennie = 1920; prenom = "Reine"; naissances = 8051 }
      { sexe = "f"; decennie = 1920; prenom = "Adrienne"; naissances = 7797 }
      { sexe = "f"; decennie = 1920; prenom = "Claire"; naissances = 7743 }
      { sexe = "f"; decennie = 1920; prenom = "Léone"; naissances = 7248 }
      { sexe = "f"; decennie = 1920; prenom = "Odile"; naissances = 7160 }
      { sexe = "f"; decennie = 1920; prenom = "Catherine"; naissances = 7135 }
      { sexe = "f"; decennie = 1920; prenom = "Aline"; naissances = 6984 }
      { sexe = "f"; decennie = 1920; prenom = "Léa"; naissances = 6837 }
      { sexe = "f"; decennie = 1920; prenom = "Julienne"; naissances = 6782 }
      { sexe = "f"; decennie = 1920; prenom = "Nicole"; naissances = 6774 }
      { sexe = "f"; decennie = 1920; prenom = "Aimée"; naissances = 6771 }
      { sexe = "f"; decennie = 1920; prenom = "Michelle"; naissances = 6314 }
      { sexe = "f"; decennie = 1920; prenom = "Agnès"; naissances = 6196 }
      { sexe = "f"; decennie = 1920; prenom = "Augustine"; naissances = 6041 }
      { sexe = "f"; decennie = 1920; prenom = "Blanche"; naissances = 5978 }
      { sexe = "f"; decennie = 1920; prenom = "Albertine"; naissances = 5963 }
      { sexe = "f"; decennie = 1920; prenom = "Edith"; naissances = 5848 }
      { sexe = "f"; decennie = 1920; prenom = "Mathilde"; naissances = 5670 }
      { sexe = "f"; decennie = 1920; prenom = "Léonie"; naissances = 5460 }
      { sexe = "f"; decennie = 1920; prenom = "Camille"; naissances = 5403 }
      { sexe = "f"; decennie = 1920; prenom = "Carmen"; naissances = 5396 }
      { sexe = "f"; decennie = 1920; prenom = "Andréa"; naissances = 5246 }
      { sexe = "f"; decennie = 1920; prenom = "Claude"; naissances = 4925 }
      { sexe = "f"; decennie = 1920; prenom = "Olga"; naissances = 4806 }
      { sexe = "f"; decennie = 1920; prenom = "Emilie"; naissances = 4783 }
      { sexe = "f"; decennie = 1920; prenom = "Pauline"; naissances = 4418 }
      { sexe = "f"; decennie = 1920; prenom = "Claudine"; naissances = 4317 }
      { sexe = "f"; decennie = 1920; prenom = "Nelly"; naissances = 4250 }
      { sexe = "f"; decennie = 1920; prenom = "Emma"; naissances = 4210 }
      { sexe = "f"; decennie = 1920; prenom = "Augusta"; naissances = 4104 }
      { sexe = "f"; decennie = 1920; prenom = "Isabelle"; naissances = 3954 }
      { sexe = "f"; decennie = 1920; prenom = "Régine"; naissances = 3895 }
      { sexe = "f"; decennie = 1920; prenom = "Léontine"; naissances = 3889 }
      { sexe = "f"; decennie = 1920; prenom = "Louisette"; naissances = 3832 }
      { sexe = "f"; decennie = 1920; prenom = "Annette"; naissances = 3830 }
      { sexe = "f"; decennie = 1920; prenom = "Jeannette"; naissances = 3795 }
      { sexe = "f"; decennie = 1920; prenom = "Anne-Marie"; naissances = 3667 }
      { sexe = "f"; decennie = 1920; prenom = "Amélie"; naissances = 3578 }
      { sexe = "f"; decennie = 1920; prenom = "Josiane"; naissances = 3391 }
      { sexe = "f"; decennie = 1920; prenom = "Adèle"; naissances = 3328 }
      { sexe = "f"; decennie = 1920; prenom = "Claudette"; naissances = 3317 }
      { sexe = "f"; decennie = 1920; prenom = "Ida"; naissances = 3302 }
      { sexe = "f"; decennie = 1930; prenom = "Marie"; naissances = 247784 }
      { sexe = "f"; decennie = 1930; prenom = "Monique"; naissances = 112149 }
      { sexe = "f"; decennie = 1930; prenom = "Jacqueline"; naissances = 108617 }
      { sexe = "f"; decennie = 1930; prenom = "Jeannine"; naissances = 89784 }
      { sexe = "f"; decennie = 1930; prenom = "Yvette"; naissances = 67973 }
      { sexe = "f"; decennie = 1930; prenom = "Denise"; naissances = 66038 }
      { sexe = "f"; decennie = 1930; prenom = "Jeanne"; naissances = 62881 }
      { sexe = "f"; decennie = 1930; prenom = "Paulette"; naissances = 57819 }
      { sexe = "f"; decennie = 1930; prenom = "Simone"; naissances = 55372 }
      { sexe = "f"; decennie = 1930; prenom = "Christiane"; naissances = 55348 }
      { sexe = "f"; decennie = 1930; prenom = "Colette"; naissances = 54270 }
      { sexe = "f"; decennie = 1930; prenom = "Nicole"; naissances = 52255 }
      { sexe = "f"; decennie = 1930; prenom = "Ginette"; naissances = 51941 }
      { sexe = "f"; decennie = 1930; prenom = "Madeleine"; naissances = 47501 }
      { sexe = "f"; decennie = 1930; prenom = "Andrée"; naissances = 47057 }
      { sexe = "f"; decennie = 1930; prenom = "Josette"; naissances = 46962 }
      { sexe = "f"; decennie = 1930; prenom = "Odette"; naissances = 46516 }
      { sexe = "f"; decennie = 1930; prenom = "Thérèse"; naissances = 46230 }
      { sexe = "f"; decennie = 1930; prenom = "Suzanne"; naissances = 44837 }
      { sexe = "f"; decennie = 1930; prenom = "Renée"; naissances = 43451 }
      { sexe = "f"; decennie = 1930; prenom = "Françoise"; naissances = 39484 }
      { sexe = "f"; decennie = 1930; prenom = "Micheline"; naissances = 39413 }
      { sexe = "f"; decennie = 1930; prenom = "Anne"; naissances = 37683 }
      { sexe = "f"; decennie = 1930; prenom = "Yvonne"; naissances = 36438 }
      { sexe = "f"; decennie = 1930; prenom = "Marcelle"; naissances = 35174 }
      { sexe = "f"; decennie = 1930; prenom = "Gisèle"; naissances = 34087 }
      { sexe = "f"; decennie = 1930; prenom = "Raymonde"; naissances = 32738 }
      { sexe = "f"; decennie = 1930; prenom = "Geneviève"; naissances = 31894 }
      { sexe = "f"; decennie = 1930; prenom = "Huguette"; naissances = 31661 }
      { sexe = "f"; decennie = 1930; prenom = "Marguerite"; naissances = 29403 }
      { sexe = "f"; decennie = 1930; prenom = "Liliane"; naissances = 28399 }
      { sexe = "f"; decennie = 1930; prenom = "Hélène"; naissances = 28211 }
      { sexe = "f"; decennie = 1930; prenom = "Lucienne"; naissances = 26965 }
      { sexe = "f"; decennie = 1930; prenom = "Bernadette"; naissances = 26825 }
      { sexe = "f"; decennie = 1930; prenom = "Georgette"; naissances = 26430 }
      { sexe = "f"; decennie = 1930; prenom = "Michelle"; naissances = 25951 }
      { sexe = "f"; decennie = 1930; prenom = "Eliane"; naissances = 25903 }
      { sexe = "f"; decennie = 1930; prenom = "Arlette"; naissances = 24949 }
      { sexe = "f"; decennie = 1930; prenom = "Pierrette"; naissances = 24541 }
      { sexe = "f"; decennie = 1930; prenom = "Janine"; naissances = 24318 }
      { sexe = "f"; decennie = 1930; prenom = "Lucette"; naissances = 22604 }
      { sexe = "f"; decennie = 1930; prenom = "Germaine"; naissances = 20717 }
      { sexe = "f"; decennie = 1930; prenom = "Solange"; naissances = 19034 }
      { sexe = "f"; decennie = 1930; prenom = "Louise"; naissances = 17647 }
      { sexe = "f"; decennie = 1930; prenom = "Jeanine"; naissances = 17622 }
      { sexe = "f"; decennie = 1930; prenom = "Irène"; naissances = 17198 }
      { sexe = "f"; decennie = 1930; prenom = "Henriette"; naissances = 16605 }
      { sexe = "f"; decennie = 1930; prenom = "Mauricette"; naissances = 16156 }
      { sexe = "f"; decennie = 1930; prenom = "Claudine"; naissances = 15966 }
      { sexe = "f"; decennie = 1930; prenom = "Josiane"; naissances = 15596 }
      { sexe = "f"; decennie = 1930; prenom = "Mireille"; naissances = 15359 }
      { sexe = "f"; decennie = 1930; prenom = "Michèle"; naissances = 15274 }
      { sexe = "f"; decennie = 1930; prenom = "Simonne"; naissances = 15075 }
      { sexe = "f"; decennie = 1930; prenom = "Claude"; naissances = 14776 }
      { sexe = "f"; decennie = 1930; prenom = "Marie-Thérèse"; naissances = 14150 }
      { sexe = "f"; decennie = 1930; prenom = "Alice"; naissances = 13837 }
      { sexe = "f"; decennie = 1930; prenom = "Claudette"; naissances = 13561 }
      { sexe = "f"; decennie = 1930; prenom = "Gilberte"; naissances = 13255 }
      { sexe = "f"; decennie = 1930; prenom = "Francine"; naissances = 12851 }
      { sexe = "f"; decennie = 1930; prenom = "Odile"; naissances = 12448 }
      { sexe = "f"; decennie = 1930; prenom = "Marthe"; naissances = 12264 }
      { sexe = "f"; decennie = 1930; prenom = "Annie"; naissances = 11979 }
      { sexe = "f"; decennie = 1930; prenom = "Annick"; naissances = 11958 }
      { sexe = "f"; decennie = 1930; prenom = "Danielle"; naissances = 10996 }
      { sexe = "f"; decennie = 1930; prenom = "Rolande"; naissances = 10886 }
      { sexe = "f"; decennie = 1930; prenom = "Antoinette"; naissances = 10821 }
      { sexe = "f"; decennie = 1930; prenom = "Rose"; naissances = 10645 }
      { sexe = "f"; decennie = 1930; prenom = "Fernande"; naissances = 10615 }
      { sexe = "f"; decennie = 1930; prenom = "Yolande"; naissances = 10006 }
      { sexe = "f"; decennie = 1930; prenom = "Juliette"; naissances = 9835 }
      { sexe = "f"; decennie = 1930; prenom = "Maria"; naissances = 9646 }
      { sexe = "f"; decennie = 1930; prenom = "Cécile"; naissances = 9585 }
      { sexe = "f"; decennie = 1930; prenom = "Gabrielle"; naissances = 8944 }
      { sexe = "f"; decennie = 1930; prenom = "Lucie"; naissances = 8592 }
      { sexe = "f"; decennie = 1930; prenom = "Anna"; naissances = 8464 }
      { sexe = "f"; decennie = 1930; prenom = "Maryse"; naissances = 8299 }
      { sexe = "f"; decennie = 1930; prenom = "Josephine"; naissances = 7833 }
      { sexe = "f"; decennie = 1930; prenom = "Elisabeth"; naissances = 7807 }
      { sexe = "f"; decennie = 1930; prenom = "Paule"; naissances = 7654 }
      { sexe = "f"; decennie = 1930; prenom = "Anne-Marie"; naissances = 7361 }
      { sexe = "f"; decennie = 1930; prenom = "Elise"; naissances = 7117 }
      { sexe = "f"; decennie = 1930; prenom = "Angèle"; naissances = 6978 }
      { sexe = "f"; decennie = 1930; prenom = "Daniele"; naissances = 6929 }
      { sexe = "f"; decennie = 1930; prenom = "Edith"; naissances = 6464 }
      { sexe = "f"; decennie = 1930; prenom = "Aline"; naissances = 6387 }
      { sexe = "f"; decennie = 1930; prenom = "Annette"; naissances = 6038 }
      { sexe = "f"; decennie = 1930; prenom = "Régine"; naissances = 6034 }
      { sexe = "f"; decennie = 1930; prenom = "Jeannette"; naissances = 6018 }
      { sexe = "f"; decennie = 1930; prenom = "Agnès"; naissances = 5920 }
      { sexe = "f"; decennie = 1930; prenom = "Louisette"; naissances = 5796 }
      { sexe = "f"; decennie = 1930; prenom = "Emilienne"; naissances = 5789 }
      { sexe = "f"; decennie = 1930; prenom = "Reine"; naissances = 5713 }
      { sexe = "f"; decennie = 1930; prenom = "Claire"; naissances = 5633 }
      { sexe = "f"; decennie = 1930; prenom = "Marie-Louise"; naissances = 5549 }
      { sexe = "f"; decennie = 1930; prenom = "Berthe"; naissances = 5367 }
      { sexe = "f"; decennie = 1930; prenom = "Catherine"; naissances = 5357 }
      { sexe = "f"; decennie = 1930; prenom = "Chantal"; naissances = 4903 }
      { sexe = "f"; decennie = 1930; prenom = "Léone"; naissances = 4780 }
      { sexe = "f"; decennie = 1930; prenom = "Nelly"; naissances = 4729 }
      { sexe = "f"; decennie = 1930; prenom = "Marinette"; naissances = 4562 }
      { sexe = "f"; decennie = 1930; prenom = "Jocelyne"; naissances = 4498 }
      { sexe = "f"; decennie = 1930; prenom = "Nicolle"; naissances = 4448 }
      { sexe = "f"; decennie = 1930; prenom = "Charlotte"; naissances = 4190 }
      { sexe = "f"; decennie = 1930; prenom = "Ghislaine"; naissances = 4128 }
      { sexe = "f"; decennie = 1930; prenom = "Carmen"; naissances = 4069 }
      { sexe = "f"; decennie = 1930; prenom = "Aimée"; naissances = 3878 }
      { sexe = "f"; decennie = 1930; prenom = "Eugénie"; naissances = 3714 }
      { sexe = "f"; decennie = 1930; prenom = "France"; naissances = 3537 }
      { sexe = "f"; decennie = 1930; prenom = "Adrienne"; naissances = 3457 }
      { sexe = "f"; decennie = 1930; prenom = "Brigitte"; naissances = 3449 }
      { sexe = "f"; decennie = 1930; prenom = "Eveline"; naissances = 3385 }
      { sexe = "f"; decennie = 1930; prenom = "Lydie"; naissances = 3333 }
      { sexe = "f"; decennie = 1930; prenom = "Evelyne"; naissances = 3170 }
      { sexe = "f"; decennie = 1930; prenom = "Léa"; naissances = 3146 }
      { sexe = "f"; decennie = 1930; prenom = "Viviane"; naissances = 3139 }
      { sexe = "f"; decennie = 1930; prenom = "Julienne"; naissances = 3118 }
      { sexe = "f"; decennie = 1930; prenom = "Maryvonne"; naissances = 3108 }
      { sexe = "f"; decennie = 1940; prenom = "Marie"; naissances = 265690 }
      { sexe = "f"; decennie = 1940; prenom = "Monique"; naissances = 139592 }
      { sexe = "f"; decennie = 1940; prenom = "Nicole"; naissances = 120135 }
      { sexe = "f"; decennie = 1940; prenom = "Françoise"; naissances = 108065 }
      { sexe = "f"; decennie = 1940; prenom = "Jacqueline"; naissances = 95968 }
      { sexe = "f"; decennie = 1940; prenom = "Christiane"; naissances = 88577 }
      { sexe = "f"; decennie = 1940; prenom = "Danielle"; naissances = 83212 }
      { sexe = "f"; decennie = 1940; prenom = "Annie"; naissances = 72017 }
      { sexe = "f"; decennie = 1940; prenom = "Michèle"; naissances = 71878 }
      { sexe = "f"; decennie = 1940; prenom = "Chantal"; naissances = 52918 }
      { sexe = "f"; decennie = 1940; prenom = "Josette"; naissances = 51965 }
      { sexe = "f"; decennie = 1940; prenom = "Colette"; naissances = 49183 }
      { sexe = "f"; decennie = 1940; prenom = "Anne"; naissances = 45979 }
      { sexe = "f"; decennie = 1940; prenom = "Jeannine"; naissances = 44009 }
      { sexe = "f"; decennie = 1940; prenom = "Michelle"; naissances = 43919 }
      { sexe = "f"; decennie = 1940; prenom = "Annick"; naissances = 43445 }
      { sexe = "f"; decennie = 1940; prenom = "Claudine"; naissances = 43317 }
      { sexe = "f"; decennie = 1940; prenom = "Josiane"; naissances = 42721 }
      { sexe = "f"; decennie = 1940; prenom = "Liliane"; naissances = 39322 }
      { sexe = "f"; decennie = 1940; prenom = "Martine"; naissances = 38725 }
      { sexe = "f"; decennie = 1940; prenom = "Yvette"; naissances = 37890 }
      { sexe = "f"; decennie = 1940; prenom = "Denise"; naissances = 37504 }
      { sexe = "f"; decennie = 1940; prenom = "Geneviève"; naissances = 37270 }
      { sexe = "f"; decennie = 1940; prenom = "Daniele"; naissances = 36443 }
      { sexe = "f"; decennie = 1940; prenom = "Bernadette"; naissances = 35673 }
      { sexe = "f"; decennie = 1940; prenom = "Mireille"; naissances = 35199 }
      { sexe = "f"; decennie = 1940; prenom = "Eliane"; naissances = 30764 }
      { sexe = "f"; decennie = 1940; prenom = "Thérèse"; naissances = 29732 }
      { sexe = "f"; decennie = 1940; prenom = "Simone"; naissances = 29536 }
      { sexe = "f"; decennie = 1940; prenom = "Gisèle"; naissances = 28365 }
      { sexe = "f"; decennie = 1940; prenom = "Andrée"; naissances = 28246 }
      { sexe = "f"; decennie = 1940; prenom = "Ginette"; naissances = 26942 }
      { sexe = "f"; decennie = 1940; prenom = "Paulette"; naissances = 26695 }
      { sexe = "f"; decennie = 1940; prenom = "Jeanne"; naissances = 25958 }
      { sexe = "f"; decennie = 1940; prenom = "Madeleine"; naissances = 25401 }
      { sexe = "f"; decennie = 1940; prenom = "Micheline"; naissances = 25360 }
      { sexe = "f"; decennie = 1940; prenom = "Arlette"; naissances = 25114 }
      { sexe = "f"; decennie = 1940; prenom = "Jocelyne"; naissances = 24719 }
      { sexe = "f"; decennie = 1940; prenom = "Marie-Thérèse"; naissances = 24654 }
      { sexe = "f"; decennie = 1940; prenom = "Renée"; naissances = 24225 }
      { sexe = "f"; decennie = 1940; prenom = "Anne-Marie"; naissances = 24196 }
      { sexe = "f"; decennie = 1940; prenom = "Hélène"; naissances = 23458 }
      { sexe = "f"; decennie = 1940; prenom = "Suzanne"; naissances = 22242 }
      { sexe = "f"; decennie = 1940; prenom = "Maryse"; naissances = 21505 }
      { sexe = "f"; decennie = 1940; prenom = "Evelyne"; naissances = 21315 }
      { sexe = "f"; decennie = 1940; prenom = "Francine"; naissances = 20752 }
      { sexe = "f"; decennie = 1940; prenom = "Catherine"; naissances = 19955 }
      { sexe = "f"; decennie = 1940; prenom = "Elisabeth"; naissances = 19643 }
      { sexe = "f"; decennie = 1940; prenom = "Odile"; naissances = 19515 }
      { sexe = "f"; decennie = 1940; prenom = "Huguette"; naissances = 19039 }
      { sexe = "f"; decennie = 1940; prenom = "Pierrette"; naissances = 18868 }
      { sexe = "f"; decennie = 1940; prenom = "Brigitte"; naissances = 18553 }
      { sexe = "f"; decennie = 1940; prenom = "Janine"; naissances = 18454 }
      { sexe = "f"; decennie = 1940; prenom = "Raymonde"; naissances = 17636 }
      { sexe = "f"; decennie = 1940; prenom = "Claudette"; naissances = 17552 }
      { sexe = "f"; decennie = 1940; prenom = "Marie-Claude"; naissances = 17251 }
      { sexe = "f"; decennie = 1940; prenom = "Odette"; naissances = 16812 }
      { sexe = "f"; decennie = 1940; prenom = "Marcelle"; naissances = 16295 }
      { sexe = "f"; decennie = 1940; prenom = "Joëlle"; naissances = 16235 }
      { sexe = "f"; decennie = 1940; prenom = "Marie-France"; naissances = 15965 }
      { sexe = "f"; decennie = 1940; prenom = "Claude"; naissances = 15890 }
      { sexe = "f"; decennie = 1940; prenom = "Dominique"; naissances = 15465 }
      { sexe = "f"; decennie = 1940; prenom = "Marguerite"; naissances = 14884 }
      { sexe = "f"; decennie = 1940; prenom = "Solange"; naissances = 14823 }
      { sexe = "f"; decennie = 1940; prenom = "Lucette"; naissances = 14642 }
      { sexe = "f"; decennie = 1940; prenom = "Maryvonne"; naissances = 14569 }
      { sexe = "f"; decennie = 1940; prenom = "Yvonne"; naissances = 14491 }
      { sexe = "f"; decennie = 1940; prenom = "Mauricette"; naissances = 13528 }
      { sexe = "f"; decennie = 1940; prenom = "Jeanine"; naissances = 12981 }
      { sexe = "f"; decennie = 1940; prenom = "Nadine"; naissances = 12915 }
      { sexe = "f"; decennie = 1940; prenom = "Lucienne"; naissances = 12597 }
      { sexe = "f"; decennie = 1940; prenom = "Viviane"; naissances = 12000 }
      { sexe = "f"; decennie = 1940; prenom = "Georgette"; naissances = 11935 }
      { sexe = "f"; decennie = 1940; prenom = "Yolande"; naissances = 11166 }
      { sexe = "f"; decennie = 1940; prenom = "Régine"; naissances = 11035 }
      { sexe = "f"; decennie = 1940; prenom = "Edith"; naissances = 10662 }
      { sexe = "f"; decennie = 1940; prenom = "Irène"; naissances = 10643 }
      { sexe = "f"; decennie = 1940; prenom = "Ghislaine"; naissances = 10415 }
      { sexe = "f"; decennie = 1940; prenom = "Sylviane"; naissances = 9993 }
      { sexe = "f"; decennie = 1940; prenom = "Christine"; naissances = 9812 }
      { sexe = "f"; decennie = 1940; prenom = "Nelly"; naissances = 9238 }
      { sexe = "f"; decennie = 1940; prenom = "Annette"; naissances = 9022 }
      { sexe = "f"; decennie = 1940; prenom = "Marie-Claire"; naissances = 7732 }
      { sexe = "f"; decennie = 1940; prenom = "Rose"; naissances = 7701 }
      { sexe = "f"; decennie = 1940; prenom = "Henriette"; naissances = 7431 }
      { sexe = "f"; decennie = 1940; prenom = "Agnès"; naissances = 7326 }
      { sexe = "f"; decennie = 1940; prenom = "Noëlle"; naissances = 7104 }
      { sexe = "f"; decennie = 1940; prenom = "Aline"; naissances = 6938 }
      { sexe = "f"; decennie = 1940; prenom = "Germaine"; naissances = 6898 }
      { sexe = "f"; decennie = 1940; prenom = "Béatrice"; naissances = 6758 }
      { sexe = "f"; decennie = 1940; prenom = "Marie-Françoise"; naissances = 6682 }
      { sexe = "f"; decennie = 1940; prenom = "Louise"; naissances = 6655 }
      { sexe = "f"; decennie = 1940; prenom = "Gilberte"; naissances = 6631 }
      { sexe = "f"; decennie = 1940; prenom = "Marie-José"; naissances = 6584 }
      { sexe = "f"; decennie = 1940; prenom = "Rolande"; naissances = 6382 }
      { sexe = "f"; decennie = 1940; prenom = "Claire"; naissances = 6351 }
      { sexe = "f"; decennie = 1940; prenom = "Cécile"; naissances = 6326 }
      { sexe = "f"; decennie = 1940; prenom = "France"; naissances = 6090 }
      { sexe = "f"; decennie = 1940; prenom = "Sylvie"; naissances = 5958 }
      { sexe = "f"; decennie = 1940; prenom = "Gabrielle"; naissances = 5704 }
      { sexe = "f"; decennie = 1940; prenom = "Antoinette"; naissances = 5512 }
      { sexe = "f"; decennie = 1940; prenom = "Alice"; naissances = 5426 }
      { sexe = "f"; decennie = 1940; prenom = "Marthe"; naissances = 5339 }
      { sexe = "f"; decennie = 1940; prenom = "Marie-Louise"; naissances = 5173 }
      { sexe = "f"; decennie = 1940; prenom = "Louisette"; naissances = 5093 }
      { sexe = "f"; decennie = 1940; prenom = "Eveline"; naissances = 4952 }
      { sexe = "f"; decennie = 1940; prenom = "Roselyne"; naissances = 4836 }
      { sexe = "f"; decennie = 1940; prenom = "Claudie"; naissances = 4757 }
      { sexe = "f"; decennie = 1940; prenom = "Paule"; naissances = 4599 }
      { sexe = "f"; decennie = 1940; prenom = "Lydie"; naissances = 4354 }
      { sexe = "f"; decennie = 1940; prenom = "Sylvette"; naissances = 4325 }
      { sexe = "f"; decennie = 1940; prenom = "Jeannette"; naissances = 4288 }
      { sexe = "f"; decennie = 1940; prenom = "Nicolle"; naissances = 4283 }
      { sexe = "f"; decennie = 1940; prenom = "Fernande"; naissances = 4210 }
      { sexe = "f"; decennie = 1940; prenom = "Marie-Jeanne"; naissances = 4129 }
      { sexe = "f"; decennie = 1940; prenom = "Anny"; naissances = 4108 }
      { sexe = "f"; decennie = 1940; prenom = "Marie-Christine"; naissances = 4078 }
      { sexe = "f"; decennie = 1940; prenom = "Roseline"; naissances = 4024 }
      { sexe = "f"; decennie = 1940; prenom = "Marinette"; naissances = 4016 }
      { sexe = "f"; decennie = 1940; prenom = "Juliette"; naissances = 3916 }
      { sexe = "f"; decennie = 1950; prenom = "Marie"; naissances = 217042 }
      { sexe = "f"; decennie = 1950; prenom = "Martine"; naissances = 191479 }
      { sexe = "f"; decennie = 1950; prenom = "Françoise"; naissances = 126336 }
      { sexe = "f"; decennie = 1950; prenom = "Chantal"; naissances = 114643 }
      { sexe = "f"; decennie = 1950; prenom = "Catherine"; naissances = 112540 }
      { sexe = "f"; decennie = 1950; prenom = "Brigitte"; naissances = 105751 }
      { sexe = "f"; decennie = 1950; prenom = "Monique"; naissances = 92938 }
      { sexe = "f"; decennie = 1950; prenom = "Dominique"; naissances = 91121 }
      { sexe = "f"; decennie = 1950; prenom = "Annie"; naissances = 82665 }
      { sexe = "f"; decennie = 1950; prenom = "Nicole"; naissances = 82430 }
      { sexe = "f"; decennie = 1950; prenom = "Christine"; naissances = 78194 }
      { sexe = "f"; decennie = 1950; prenom = "Sylvie"; naissances = 73028 }
      { sexe = "f"; decennie = 1950; prenom = "Patricia"; naissances = 65981 }
      { sexe = "f"; decennie = 1950; prenom = "Evelyne"; naissances = 63271 }
      { sexe = "f"; decennie = 1950; prenom = "Michèle"; naissances = 62572 }
      { sexe = "f"; decennie = 1950; prenom = "Christiane"; naissances = 62353 }
      { sexe = "f"; decennie = 1950; prenom = "Claudine"; naissances = 55842 }
      { sexe = "f"; decennie = 1950; prenom = "Jacqueline"; naissances = 53642 }
      { sexe = "f"; decennie = 1950; prenom = "Jocelyne"; naissances = 49960 }
      { sexe = "f"; decennie = 1950; prenom = "Annick"; naissances = 48964 }
      { sexe = "f"; decennie = 1950; prenom = "Nadine"; naissances = 48174 }
      { sexe = "f"; decennie = 1950; prenom = "Joëlle"; naissances = 47945 }
      { sexe = "f"; decennie = 1950; prenom = "Josiane"; naissances = 46341 }
      { sexe = "f"; decennie = 1950; prenom = "Danielle"; naissances = 44621 }
      { sexe = "f"; decennie = 1950; prenom = "Elisabeth"; naissances = 42246 }
      { sexe = "f"; decennie = 1950; prenom = "Bernadette"; naissances = 41063 }
      { sexe = "f"; decennie = 1950; prenom = "Anne"; naissances = 40057 }
      { sexe = "f"; decennie = 1950; prenom = "Maryse"; naissances = 39094 }
      { sexe = "f"; decennie = 1950; prenom = "Marie-Christine"; naissances = 37091 }
      { sexe = "f"; decennie = 1950; prenom = "Anne-Marie"; naissances = 35277 }
      { sexe = "f"; decennie = 1950; prenom = "Isabelle"; naissances = 35049 }
      { sexe = "f"; decennie = 1950; prenom = "Béatrice"; naissances = 33733 }
      { sexe = "f"; decennie = 1950; prenom = "Mireille"; naissances = 33602 }
      { sexe = "f"; decennie = 1950; prenom = "Colette"; naissances = 33150 }
      { sexe = "f"; decennie = 1950; prenom = "Véronique"; naissances = 32686 }
      { sexe = "f"; decennie = 1950; prenom = "Geneviève"; naissances = 31600 }
      { sexe = "f"; decennie = 1950; prenom = "Michelle"; naissances = 30334 }
      { sexe = "f"; decennie = 1950; prenom = "Josette"; naissances = 27232 }
      { sexe = "f"; decennie = 1950; prenom = "Eliane"; naissances = 27000 }
      { sexe = "f"; decennie = 1950; prenom = "Ghislaine"; naissances = 26443 }
      { sexe = "f"; decennie = 1950; prenom = "Odile"; naissances = 25859 }
      { sexe = "f"; decennie = 1950; prenom = "Marie-Claude"; naissances = 24936 }
      { sexe = "f"; decennie = 1950; prenom = "Daniele"; naissances = 24928 }
      { sexe = "f"; decennie = 1950; prenom = "Liliane"; naissances = 24209 }
      { sexe = "f"; decennie = 1950; prenom = "Pascale"; naissances = 24188 }
      { sexe = "f"; decennie = 1950; prenom = "Hélène"; naissances = 23889 }
      { sexe = "f"; decennie = 1950; prenom = "Marie-Thérèse"; naissances = 23628 }
      { sexe = "f"; decennie = 1950; prenom = "Francine"; naissances = 23390 }
      { sexe = "f"; decennie = 1950; prenom = "Sylviane"; naissances = 22715 }
      { sexe = "f"; decennie = 1950; prenom = "Gisèle"; naissances = 22594 }
      { sexe = "f"; decennie = 1950; prenom = "Marie-France"; naissances = 22013 }
      { sexe = "f"; decennie = 1950; prenom = "Régine"; naissances = 21939 }
      { sexe = "f"; decennie = 1950; prenom = "Agnès"; naissances = 20617 }
      { sexe = "f"; decennie = 1950; prenom = "Viviane"; naissances = 20169 }
      { sexe = "f"; decennie = 1950; prenom = "Yvette"; naissances = 19868 }
      { sexe = "f"; decennie = 1950; prenom = "Denise"; naissances = 19676 }
      { sexe = "f"; decennie = 1950; prenom = "Thérèse"; naissances = 18693 }
      { sexe = "f"; decennie = 1950; prenom = "Marie-José"; naissances = 17600 }
      { sexe = "f"; decennie = 1950; prenom = "Jeannine"; naissances = 17540 }
      { sexe = "f"; decennie = 1950; prenom = "Fabienne"; naissances = 16193 }
      { sexe = "f"; decennie = 1950; prenom = "Corinne"; naissances = 15838 }
      { sexe = "f"; decennie = 1950; prenom = "Laurence"; naissances = 15304 }
      { sexe = "f"; decennie = 1950; prenom = "Muriel"; naissances = 14445 }
      { sexe = "f"; decennie = 1950; prenom = "Arlette"; naissances = 14306 }
      { sexe = "f"; decennie = 1950; prenom = "Nelly"; naissances = 14199 }
      { sexe = "f"; decennie = 1950; prenom = "Edith"; naissances = 14050 }
      { sexe = "f"; decennie = 1950; prenom = "Marie-Claire"; naissances = 13892 }
      { sexe = "f"; decennie = 1950; prenom = "Maryvonne"; naissances = 13514 }
      { sexe = "f"; decennie = 1950; prenom = "Marie-Hélène"; naissances = 13299 }
      { sexe = "f"; decennie = 1950; prenom = "Micheline"; naissances = 13066 }
      { sexe = "f"; decennie = 1950; prenom = "Yolande"; naissances = 13063 }
      { sexe = "f"; decennie = 1950; prenom = "Ginette"; naissances = 12737 }
      { sexe = "f"; decennie = 1950; prenom = "Claude"; naissances = 12631 }
      { sexe = "f"; decennie = 1950; prenom = "Pierrette"; naissances = 12198 }
      { sexe = "f"; decennie = 1950; prenom = "Andrée"; naissances = 11984 }
      { sexe = "f"; decennie = 1950; prenom = "Marie-Françoise"; naissances = 11862 }
      { sexe = "f"; decennie = 1950; prenom = "Myriam"; naissances = 11808 }
      { sexe = "f"; decennie = 1950; prenom = "Simone"; naissances = 11778 }
      { sexe = "f"; decennie = 1950; prenom = "Florence"; naissances = 11576 }
      { sexe = "f"; decennie = 1950; prenom = "Claire"; naissances = 11239 }
      { sexe = "f"; decennie = 1950; prenom = "Solange"; naissances = 11174 }
      { sexe = "f"; decennie = 1950; prenom = "Jeanne"; naissances = 10886 }
      { sexe = "f"; decennie = 1950; prenom = "Madeleine"; naissances = 10844 }
      { sexe = "f"; decennie = 1950; prenom = "Renée"; naissances = 10343 }
      { sexe = "f"; decennie = 1950; prenom = "Huguette"; naissances = 10281 }
      { sexe = "f"; decennie = 1950; prenom = "Aline"; naissances = 10105 }
      { sexe = "f"; decennie = 1950; prenom = "Claudette"; naissances = 10032 }
      { sexe = "f"; decennie = 1950; prenom = "Paulette"; naissances = 9864 }
      { sexe = "f"; decennie = 1950; prenom = "Suzanne"; naissances = 9702 }
      { sexe = "f"; decennie = 1950; prenom = "Lydie"; naissances = 9485 }
      { sexe = "f"; decennie = 1950; prenom = "Roselyne"; naissances = 9431 }
      { sexe = "f"; decennie = 1950; prenom = "Raymonde"; naissances = 8338 }
      { sexe = "f"; decennie = 1950; prenom = "Marie-Paule"; naissances = 8128 }
      { sexe = "f"; decennie = 1950; prenom = "Claudie"; naissances = 8053 }
      { sexe = "f"; decennie = 1950; prenom = "Anita"; naissances = 7958 }
      { sexe = "f"; decennie = 1950; prenom = "Maryline"; naissances = 7952 }
      { sexe = "f"; decennie = 1950; prenom = "Noëlle"; naissances = 7864 }
      { sexe = "f"; decennie = 1950; prenom = "Janine"; naissances = 7776 }
      { sexe = "f"; decennie = 1950; prenom = "Irène"; naissances = 7531 }
      { sexe = "f"; decennie = 1950; prenom = "Lucette"; naissances = 7500 }
      { sexe = "f"; decennie = 1950; prenom = "Marie-Noëlle"; naissances = 7431 }
      { sexe = "f"; decennie = 1950; prenom = "Mauricette"; naissances = 7431 }
      { sexe = "f"; decennie = 1950; prenom = "Marguerite"; naissances = 7397 }
      { sexe = "f"; decennie = 1950; prenom = "Cécile"; naissances = 7140 }
      { sexe = "f"; decennie = 1950; prenom = "Marylene"; naissances = 7015 }
      { sexe = "f"; decennie = 1950; prenom = "Nadia"; naissances = 6986 }
      { sexe = "f"; decennie = 1950; prenom = "Annette"; naissances = 6940 }
      { sexe = "f"; decennie = 1950; prenom = "Marie-Pierre"; naissances = 6937 }
      { sexe = "f"; decennie = 1950; prenom = "Sophie"; naissances = 6889 }
      { sexe = "f"; decennie = 1950; prenom = "Marcelle"; naissances = 6832 }
      { sexe = "f"; decennie = 1950; prenom = "Odette"; naissances = 6637 }
      { sexe = "f"; decennie = 1950; prenom = "Jeanine"; naissances = 6366 }
      { sexe = "f"; decennie = 1950; prenom = "Carole"; naissances = 6322 }
      { sexe = "f"; decennie = 1950; prenom = "Marianne"; naissances = 6214 }
      { sexe = "f"; decennie = 1950; prenom = "Armelle"; naissances = 6143 }
      { sexe = "f"; decennie = 1950; prenom = "Yvonne"; naissances = 6061 }
      { sexe = "f"; decennie = 1950; prenom = "Sabine"; naissances = 6016 }
      { sexe = "f"; decennie = 1950; prenom = "Lucienne"; naissances = 5886 }
      { sexe = "f"; decennie = 1950; prenom = "Nathalie"; naissances = 5785 }
      { sexe = "f"; decennie = 1950; prenom = "Sylvette"; naissances = 5705 }
      { sexe = "f"; decennie = 1950; prenom = "Frédérique"; naissances = 5699 }
      { sexe = "f"; decennie = 1950; prenom = "Murielle"; naissances = 5666 }
      { sexe = "f"; decennie = 1950; prenom = "Marlène"; naissances = 5566 }
      { sexe = "f"; decennie = 1950; prenom = "Rose"; naissances = 5532 }
      { sexe = "f"; decennie = 1950; prenom = "Roseline"; naissances = 5503 }
      { sexe = "f"; decennie = 1950; prenom = "Eveline"; naissances = 5367 }
      { sexe = "f"; decennie = 1950; prenom = "Marie-Jeanne"; naissances = 5319 }
      { sexe = "f"; decennie = 1950; prenom = "Maria"; naissances = 5111 }
      { sexe = "f"; decennie = 1950; prenom = "Marie-Line"; naissances = 5101 }
      { sexe = "f"; decennie = 1950; prenom = "Marie-Odile"; naissances = 4686 }
      { sexe = "f"; decennie = 1950; prenom = "Marie-Ange"; naissances = 4658 }
      { sexe = "f"; decennie = 1950; prenom = "Georgette"; naissances = 4645 }
      { sexe = "f"; decennie = 1950; prenom = "Yveline"; naissances = 4557 }
      { sexe = "f"; decennie = 1950; prenom = "Marie-Louise"; naissances = 4520 }
      { sexe = "f"; decennie = 1950; prenom = "Marie-Laure"; naissances = 4464 }
      { sexe = "f"; decennie = 1950; prenom = "Sonia"; naissances = 4340 }
      { sexe = "f"; decennie = 1950; prenom = "Lysiane"; naissances = 4214 }
      { sexe = "f"; decennie = 1950; prenom = "Carmen"; naissances = 4107 }
      { sexe = "f"; decennie = 1950; prenom = "Antoinette"; naissances = 4072 }
      { sexe = "f"; decennie = 1950; prenom = "Marinette"; naissances = 4052 }
      { sexe = "f"; decennie = 1950; prenom = "Caroline"; naissances = 3963 }
      { sexe = "f"; decennie = 1950; prenom = "Gilberte"; naissances = 3857 }
      { sexe = "f"; decennie = 1950; prenom = "Lydia"; naissances = 3786 }
      { sexe = "f"; decennie = 1950; prenom = "Gabrielle"; naissances = 3782 }
      { sexe = "f"; decennie = 1950; prenom = "Marie-Madeleine"; naissances = 3590 }
      { sexe = "f"; decennie = 1950; prenom = "Rolande"; naissances = 3543 }
      { sexe = "f"; decennie = 1950; prenom = "Rose-Marie"; naissances = 3527 }
      { sexe = "f"; decennie = 1960; prenom = "Sylvie"; naissances = 204773 }
      { sexe = "f"; decennie = 1960; prenom = "Nathalie"; naissances = 197659 }
      { sexe = "f"; decennie = 1960; prenom = "Isabelle"; naissances = 186477 }
      { sexe = "f"; decennie = 1960; prenom = "Catherine"; naissances = 163001 }
      { sexe = "f"; decennie = 1960; prenom = "Christine"; naissances = 137657 }
      { sexe = "f"; decennie = 1960; prenom = "Véronique"; naissances = 134241 }
      { sexe = "f"; decennie = 1960; prenom = "Valérie"; naissances = 133880 }
      { sexe = "f"; decennie = 1960; prenom = "Marie"; naissances = 107989 }
      { sexe = "f"; decennie = 1960; prenom = "Corinne"; naissances = 103589 }
      { sexe = "f"; decennie = 1960; prenom = "Patricia"; naissances = 97700 }
      { sexe = "f"; decennie = 1960; prenom = "Laurence"; naissances = 90379 }
      { sexe = "f"; decennie = 1960; prenom = "Martine"; naissances = 74131 }
      { sexe = "f"; decennie = 1960; prenom = "Françoise"; naissances = 68689 }
      { sexe = "f"; decennie = 1960; prenom = "Florence"; naissances = 65134 }
      { sexe = "f"; decennie = 1960; prenom = "Brigitte"; naissances = 62743 }
      { sexe = "f"; decennie = 1960; prenom = "Pascale"; naissances = 61640 }
      { sexe = "f"; decennie = 1960; prenom = "Anne"; naissances = 54707 }
      { sexe = "f"; decennie = 1960; prenom = "Béatrice"; naissances = 54047 }
      { sexe = "f"; decennie = 1960; prenom = "Sandrine"; naissances = 53741 }
      { sexe = "f"; decennie = 1960; prenom = "Fabienne"; naissances = 53590 }
      { sexe = "f"; decennie = 1960; prenom = "Sophie"; naissances = 53436 }
      { sexe = "f"; decennie = 1960; prenom = "Nadine"; naissances = 52040 }
      { sexe = "f"; decennie = 1960; prenom = "Chantal"; naissances = 49589 }
      { sexe = "f"; decennie = 1960; prenom = "Dominique"; naissances = 48972 }
      { sexe = "f"; decennie = 1960; prenom = "Agnès"; naissances = 35403 }
      { sexe = "f"; decennie = 1960; prenom = "Carole"; naissances = 35041 }
      { sexe = "f"; decennie = 1960; prenom = "Elisabeth"; naissances = 34155 }
      { sexe = "f"; decennie = 1960; prenom = "Annie"; naissances = 33289 }
      { sexe = "f"; decennie = 1960; prenom = "Muriel"; naissances = 30872 }
      { sexe = "f"; decennie = 1960; prenom = "Evelyne"; naissances = 30574 }
      { sexe = "f"; decennie = 1960; prenom = "Marie-Christine"; naissances = 27366 }
      { sexe = "f"; decennie = 1960; prenom = "Claudine"; naissances = 26893 }
      { sexe = "f"; decennie = 1960; prenom = "Hélène"; naissances = 26501 }
      { sexe = "f"; decennie = 1960; prenom = "Nicole"; naissances = 25751 }
      { sexe = "f"; decennie = 1960; prenom = "Monique"; naissances = 24762 }
      { sexe = "f"; decennie = 1960; prenom = "Myriam"; naissances = 24268 }
      { sexe = "f"; decennie = 1960; prenom = "Christelle"; naissances = 23219 }
      { sexe = "f"; decennie = 1960; prenom = "Michèle"; naissances = 22755 }
      { sexe = "f"; decennie = 1960; prenom = "Sabine"; naissances = 22588 }
      { sexe = "f"; decennie = 1960; prenom = "Cécile"; naissances = 22151 }
      { sexe = "f"; decennie = 1960; prenom = "Caroline"; naissances = 21077 }
      { sexe = "f"; decennie = 1960; prenom = "Mireille"; naissances = 20868 }
      { sexe = "f"; decennie = 1960; prenom = "Joëlle"; naissances = 20338 }
      { sexe = "f"; decennie = 1960; prenom = "Annick"; naissances = 20320 }
      { sexe = "f"; decennie = 1960; prenom = "Nadia"; naissances = 20225 }
      { sexe = "f"; decennie = 1960; prenom = "Jocelyne"; naissances = 19964 }
      { sexe = "f"; decennie = 1960; prenom = "Anne-Marie"; naissances = 19630 }
      { sexe = "f"; decennie = 1960; prenom = "Frédérique"; naissances = 19620 }
      { sexe = "f"; decennie = 1960; prenom = "Murielle"; naissances = 18553 }
      { sexe = "f"; decennie = 1960; prenom = "Maria"; naissances = 17705 }
      { sexe = "f"; decennie = 1960; prenom = "Karine"; naissances = 17681 }
      { sexe = "f"; decennie = 1960; prenom = "Maryse"; naissances = 17405 }
      { sexe = "f"; decennie = 1960; prenom = "Odile"; naissances = 17099 }
      { sexe = "f"; decennie = 1960; prenom = "Claire"; naissances = 16824 }
      { sexe = "f"; decennie = 1960; prenom = "Jacqueline"; naissances = 16021 }
      { sexe = "f"; decennie = 1960; prenom = "Ghislaine"; naissances = 15893 }
      { sexe = "f"; decennie = 1960; prenom = "Christiane"; naissances = 15450 }
      { sexe = "f"; decennie = 1960; prenom = "Emmanuelle"; naissances = 15099 }
      { sexe = "f"; decennie = 1960; prenom = "Marie-Pierre"; naissances = 15040 }
      { sexe = "f"; decennie = 1960; prenom = "Bernadette"; naissances = 14891 }
      { sexe = "f"; decennie = 1960; prenom = "Régine"; naissances = 14400 }
      { sexe = "f"; decennie = 1960; prenom = "Lydie"; naissances = 14296 }
      { sexe = "f"; decennie = 1960; prenom = "Virginie"; naissances = 14068 }
      { sexe = "f"; decennie = 1960; prenom = "Marie-Hélène"; naissances = 13515 }
      { sexe = "f"; decennie = 1960; prenom = "Danielle"; naissances = 13506 }
      { sexe = "f"; decennie = 1960; prenom = "Maryline"; naissances = 13338 }
      { sexe = "f"; decennie = 1960; prenom = "Josiane"; naissances = 13012 }
      { sexe = "f"; decennie = 1960; prenom = "Geneviève"; naissances = 12870 }
      { sexe = "f"; decennie = 1960; prenom = "Nelly"; naissances = 12537 }
      { sexe = "f"; decennie = 1960; prenom = "Magali"; naissances = 12485 }
      { sexe = "f"; decennie = 1960; prenom = "Sylviane"; naissances = 12354 }
      { sexe = "f"; decennie = 1960; prenom = "Marie-France"; naissances = 11576 }
      { sexe = "f"; decennie = 1960; prenom = "Marie-Laure"; naissances = 11327 }
      { sexe = "f"; decennie = 1960; prenom = "Marie-Claude"; naissances = 11308 }
      { sexe = "f"; decennie = 1960; prenom = "Viviane"; naissances = 10958 }
      { sexe = "f"; decennie = 1960; prenom = "Céline"; naissances = 10852 }
      { sexe = "f"; decennie = 1960; prenom = "Stéphanie"; naissances = 10423 }
      { sexe = "f"; decennie = 1960; prenom = "Michelle"; naissances = 10344 }
      { sexe = "f"; decennie = 1960; prenom = "Corine"; naissances = 10306 }
      { sexe = "f"; decennie = 1960; prenom = "Delphine"; naissances = 9793 }
      { sexe = "f"; decennie = 1960; prenom = "Francine"; naissances = 9727 }
      { sexe = "f"; decennie = 1960; prenom = "Sonia"; naissances = 9468 }
      { sexe = "f"; decennie = 1960; prenom = "Aline"; naissances = 9228 }
      { sexe = "f"; decennie = 1960; prenom = "Edith"; naissances = 8843 }
      { sexe = "f"; decennie = 1960; prenom = "Colette"; naissances = 8817 }
      { sexe = "f"; decennie = 1960; prenom = "Daniele"; naissances = 8634 }
      { sexe = "f"; decennie = 1960; prenom = "Marie-Thérèse"; naissances = 8567 }
      { sexe = "f"; decennie = 1960; prenom = "Marie-Claire"; naissances = 8489 }
      { sexe = "f"; decennie = 1960; prenom = "Gisèle"; naissances = 8169 }
      { sexe = "f"; decennie = 1960; prenom = "Eliane"; naissances = 8150 }
      { sexe = "f"; decennie = 1960; prenom = "Marie-José"; naissances = 8112 }
      { sexe = "f"; decennie = 1960; prenom = "Laure"; naissances = 8087 }
      { sexe = "f"; decennie = 1960; prenom = "Anita"; naissances = 7988 }
      { sexe = "f"; decennie = 1960; prenom = "Benedicte"; naissances = 7854 }
      { sexe = "f"; decennie = 1960; prenom = "Marie-Line"; naissances = 7771 }
      { sexe = "f"; decennie = 1960; prenom = "Cathy"; naissances = 7746 }
      { sexe = "f"; decennie = 1960; prenom = "Marie-Noëlle"; naissances = 7374 }
      { sexe = "f"; decennie = 1960; prenom = "Marielle"; naissances = 7307 }
      { sexe = "f"; decennie = 1960; prenom = "Sandra"; naissances = 7279 }
      { sexe = "f"; decennie = 1960; prenom = "Lydia"; naissances = 7076 }
      { sexe = "f"; decennie = 1960; prenom = "Geraldine"; naissances = 6987 }
      { sexe = "f"; decennie = 1960; prenom = "Thérèse"; naissances = 6922 }
      { sexe = "f"; decennie = 1960; prenom = "Malika"; naissances = 6718 }
      { sexe = "f"; decennie = 1960; prenom = "Liliane"; naissances = 6686 }
      { sexe = "f"; decennie = 1960; prenom = "Christel"; naissances = 6637 }
      { sexe = "f"; decennie = 1960; prenom = "Armelle"; naissances = 6534 }
      { sexe = "f"; decennie = 1960; prenom = "Marie-Françoise"; naissances = 6493 }
      { sexe = "f"; decennie = 1960; prenom = "Marianne"; naissances = 6489 }
      { sexe = "f"; decennie = 1960; prenom = "Denise"; naissances = 5794 }
      { sexe = "f"; decennie = 1960; prenom = "Yolande"; naissances = 5702 }
      { sexe = "f"; decennie = 1960; prenom = "Claudie"; naissances = 5627 }
      { sexe = "f"; decennie = 1960; prenom = "Nadege"; naissances = 5624 }
      { sexe = "f"; decennie = 1960; prenom = "Josette"; naissances = 5432 }
      { sexe = "f"; decennie = 1960; prenom = "Claude"; naissances = 5376 }
      { sexe = "f"; decennie = 1960; prenom = "Katia"; naissances = 5360 }
      { sexe = "f"; decennie = 1960; prenom = "Rachel"; naissances = 5342 }
      { sexe = "f"; decennie = 1960; prenom = "Roselyne"; naissances = 5309 }
      { sexe = "f"; decennie = 1960; prenom = "Solange"; naissances = 5223 }
      { sexe = "f"; decennie = 1960; prenom = "Sylvia"; naissances = 5162 }
      { sexe = "f"; decennie = 1960; prenom = "Marie-Ange"; naissances = 4992 }
      { sexe = "f"; decennie = 1960; prenom = "Blandine"; naissances = 4976 }
      { sexe = "f"; decennie = 1960; prenom = "Christèle"; naissances = 4905 }
      { sexe = "f"; decennie = 1960; prenom = "Marie-Paule"; naissances = 4801 }
      { sexe = "f"; decennie = 1960; prenom = "Fatima"; naissances = 4762 }
      { sexe = "f"; decennie = 1960; prenom = "Yvette"; naissances = 4746 }
      { sexe = "f"; decennie = 1960; prenom = "Jeanne"; naissances = 4305 }
      { sexe = "f"; decennie = 1960; prenom = "Estelle"; naissances = 4273 }
      { sexe = "f"; decennie = 1960; prenom = "Irène"; naissances = 4251 }
      { sexe = "f"; decennie = 1960; prenom = "Carine"; naissances = 4185 }
      { sexe = "f"; decennie = 1960; prenom = "Marylene"; naissances = 4166 }
      { sexe = "f"; decennie = 1960; prenom = "Noëlle"; naissances = 4150 }
      { sexe = "f"; decennie = 1960; prenom = "Pierrette"; naissances = 3747 }
      { sexe = "f"; decennie = 1960; prenom = "Anna"; naissances = 3653 }
      { sexe = "f"; decennie = 1960; prenom = "Mylène"; naissances = 3647 }
      { sexe = "f"; decennie = 1960; prenom = "Carmen"; naissances = 3604 }
      { sexe = "f"; decennie = 1960; prenom = "Marlène"; naissances = 3590 }
      { sexe = "f"; decennie = 1960; prenom = "Jeannine"; naissances = 3564 }
      { sexe = "f"; decennie = 1960; prenom = "Micheline"; naissances = 3514 }
      { sexe = "f"; decennie = 1960; prenom = "Roseline"; naissances = 3490 }
      { sexe = "f"; decennie = 1960; prenom = "Dalila"; naissances = 3394 }
      { sexe = "f"; decennie = 1960; prenom = "Maryvonne"; naissances = 3284 }
      { sexe = "f"; decennie = 1960; prenom = "Barbara"; naissances = 3251 }
      { sexe = "f"; decennie = 1960; prenom = "Suzanne"; naissances = 3183 }
      { sexe = "f"; decennie = 1960; prenom = "Edwige"; naissances = 3154 }
      { sexe = "f"; decennie = 1960; prenom = "Rose"; naissances = 3102 }
      { sexe = "f"; decennie = 1960; prenom = "Guylaine"; naissances = 3093 }
      { sexe = "f"; decennie = 1960; prenom = "Marie-Odile"; naissances = 3088 }
      { sexe = "f"; decennie = 1960; prenom = "Andrée"; naissances = 3086 }
      { sexe = "f"; decennie = 1960; prenom = "France"; naissances = 2971 }
      { sexe = "f"; decennie = 1960; prenom = "Madeleine"; naissances = 2962 }
      { sexe = "f"; decennie = 1960; prenom = "Arlette"; naissances = 2827 }
      { sexe = "f"; decennie = 1960; prenom = "Marie-Agnès"; naissances = 2806 }
      { sexe = "f"; decennie = 1960; prenom = "Farida"; naissances = 2798 }
      { sexe = "f"; decennie = 1960; prenom = "Nora"; naissances = 2754 }
      { sexe = "f"; decennie = 1960; prenom = "Marina"; naissances = 2751 }
      { sexe = "f"; decennie = 1960; prenom = "Simone"; naissances = 2714 }
      { sexe = "f"; decennie = 1960; prenom = "Marie-Anne"; naissances = 2630 }
      { sexe = "f"; decennie = 1960; prenom = "Rose-Marie"; naissances = 2610 }
      { sexe = "f"; decennie = 1960; prenom = "Linda"; naissances = 2609 }
      { sexe = "f"; decennie = 1960; prenom = "Dorothee"; naissances = 2607 }
      { sexe = "f"; decennie = 1960; prenom = "Antoinette"; naissances = 2575 }
      { sexe = "f"; decennie = 1960; prenom = "Marie-Josee"; naissances = 2551 }
      { sexe = "f"; decennie = 1960; prenom = "Marguerite"; naissances = 2537 }
      { sexe = "f"; decennie = 1960; prenom = "Yamina"; naissances = 2494 }
      { sexe = "f"; decennie = 1960; prenom = "Annette"; naissances = 2478 }
      { sexe = "f"; decennie = 1960; prenom = "Renée"; naissances = 2468 }
      { sexe = "f"; decennie = 1960; prenom = "Angelique"; naissances = 2460 }
      { sexe = "f"; decennie = 1960; prenom = "Huguette"; naissances = 2457 }
      { sexe = "f"; decennie = 1960; prenom = "Djamila"; naissances = 2443 }
      { sexe = "f"; decennie = 1960; prenom = "Marilyne"; naissances = 2442 }
      { sexe = "f"; decennie = 1960; prenom = "Clarisse"; naissances = 2428 }
      { sexe = "f"; decennie = 1960; prenom = "Sylvaine"; naissances = 2411 }
      { sexe = "f"; decennie = 1960; prenom = "Lucie"; naissances = 2394 }
      { sexe = "f"; decennie = 1960; prenom = "Yasmina"; naissances = 2379 }
      { sexe = "f"; decennie = 1960; prenom = "Marie-Jeanne"; naissances = 2372 }
      { sexe = "f"; decennie = 1960; prenom = "Fatiha"; naissances = 2365 }
      { sexe = "f"; decennie = 1960; prenom = "Ginette"; naissances = 2363 }
      { sexe = "f"; decennie = 1960; prenom = "Dolores"; naissances = 2361 }
      { sexe = "f"; decennie = 1960; prenom = "Claudette"; naissances = 2298 }
      { sexe = "f"; decennie = 1960; prenom = "Laetitia"; naissances = 2293 }
      { sexe = "f"; decennie = 1960; prenom = "Juliette"; naissances = 2287 }
      { sexe = "f"; decennie = 1960; prenom = "Gaelle"; naissances = 2268 }
      { sexe = "f"; decennie = 1960; prenom = "Alexandra"; naissances = 2195 }
      { sexe = "f"; decennie = 1970; prenom = "Sandrine"; naissances = 146395 }
      { sexe = "f"; decennie = 1970; prenom = "Nathalie"; naissances = 139696 }
      { sexe = "f"; decennie = 1970; prenom = "Stéphanie"; naissances = 135829 }
      { sexe = "f"; decennie = 1970; prenom = "Isabelle"; naissances = 107347 }
      { sexe = "f"; decennie = 1970; prenom = "Céline"; naissances = 97371 }
      { sexe = "f"; decennie = 1970; prenom = "Valérie"; naissances = 96010 }
      { sexe = "f"; decennie = 1970; prenom = "Christelle"; naissances = 95035 }
      { sexe = "f"; decennie = 1970; prenom = "Sophie"; naissances = 86330 }
      { sexe = "f"; decennie = 1970; prenom = "Karine"; naissances = 82064 }
      { sexe = "f"; decennie = 1970; prenom = "Virginie"; naissances = 79154 }
      { sexe = "f"; decennie = 1970; prenom = "Sylvie"; naissances = 64543 }
      { sexe = "f"; decennie = 1970; prenom = "Delphine"; naissances = 64363 }
      { sexe = "f"; decennie = 1970; prenom = "Severine"; naissances = 56646 }
      { sexe = "f"; decennie = 1970; prenom = "Marie"; naissances = 53927 }
      { sexe = "f"; decennie = 1970; prenom = "Véronique"; naissances = 53723 }
      { sexe = "f"; decennie = 1970; prenom = "Laurence"; naissances = 53161 }
      { sexe = "f"; decennie = 1970; prenom = "Sandra"; naissances = 52422 }
      { sexe = "f"; decennie = 1970; prenom = "Catherine"; naissances = 49337 }
      { sexe = "f"; decennie = 1970; prenom = "Anne"; naissances = 46553 }
      { sexe = "f"; decennie = 1970; prenom = "Caroline"; naissances = 46246 }
      { sexe = "f"; decennie = 1970; prenom = "Cécile"; naissances = 45091 }
      { sexe = "f"; decennie = 1970; prenom = "Laetitia"; naissances = 44692 }
      { sexe = "f"; decennie = 1970; prenom = "Florence"; naissances = 43800 }
      { sexe = "f"; decennie = 1970; prenom = "Carole"; naissances = 43475 }
      { sexe = "f"; decennie = 1970; prenom = "Christine"; naissances = 40574 }
      { sexe = "f"; decennie = 1970; prenom = "Corinne"; naissances = 37388 }
      { sexe = "f"; decennie = 1970; prenom = "Magali"; naissances = 36063 }
      { sexe = "f"; decennie = 1970; prenom = "Sonia"; naissances = 32034 }
      { sexe = "f"; decennie = 1970; prenom = "Patricia"; naissances = 31781 }
      { sexe = "f"; decennie = 1970; prenom = "Alexandra"; naissances = 31441 }
      { sexe = "f"; decennie = 1970; prenom = "Hélène"; naissances = 30080 }
      { sexe = "f"; decennie = 1970; prenom = "Emmanuelle"; naissances = 30060 }
      { sexe = "f"; decennie = 1970; prenom = "Carine"; naissances = 28205 }
      { sexe = "f"; decennie = 1970; prenom = "Fabienne"; naissances = 27803 }
      { sexe = "f"; decennie = 1970; prenom = "Aurélie"; naissances = 27001 }
      { sexe = "f"; decennie = 1970; prenom = "Audrey"; naissances = 26464 }
      { sexe = "f"; decennie = 1970; prenom = "Sabrina"; naissances = 26365 }
      { sexe = "f"; decennie = 1970; prenom = "Béatrice"; naissances = 26196 }
      { sexe = "f"; decennie = 1970; prenom = "Angelique"; naissances = 25487 }
      { sexe = "f"; decennie = 1970; prenom = "Sabine"; naissances = 23377 }
      { sexe = "f"; decennie = 1970; prenom = "Nadia"; naissances = 23194 }
      { sexe = "f"; decennie = 1970; prenom = "Myriam"; naissances = 21962 }
      { sexe = "f"; decennie = 1970; prenom = "Vanessa"; naissances = 21929 }
      { sexe = "f"; decennie = 1970; prenom = "Estelle"; naissances = 21655 }
      { sexe = "f"; decennie = 1970; prenom = "Claire"; naissances = 21382 }
      { sexe = "f"; decennie = 1970; prenom = "Laure"; naissances = 18920 }
      { sexe = "f"; decennie = 1970; prenom = "Muriel"; naissances = 18568 }
      { sexe = "f"; decennie = 1970; prenom = "Agnès"; naissances = 18099 }
      { sexe = "f"; decennie = 1970; prenom = "Elisabeth"; naissances = 18034 }
      { sexe = "f"; decennie = 1970; prenom = "Nadege"; naissances = 17463 }
      { sexe = "f"; decennie = 1970; prenom = "Geraldine"; naissances = 17386 }
      { sexe = "f"; decennie = 1970; prenom = "Frédérique"; naissances = 16838 }
      { sexe = "f"; decennie = 1970; prenom = "Julie"; naissances = 15916 }
      { sexe = "f"; decennie = 1970; prenom = "Katia"; naissances = 15487 }
      { sexe = "f"; decennie = 1970; prenom = "Gaelle"; naissances = 15383 }
      { sexe = "f"; decennie = 1970; prenom = "Peggy"; naissances = 14802 }
      { sexe = "f"; decennie = 1970; prenom = "Maria"; naissances = 14712 }
      { sexe = "f"; decennie = 1970; prenom = "Mélanie"; naissances = 13653 }
      { sexe = "f"; decennie = 1970; prenom = "Emilie"; naissances = 13329 }
      { sexe = "f"; decennie = 1970; prenom = "Ingrid"; naissances = 13293 }
      { sexe = "f"; decennie = 1970; prenom = "Elodie"; naissances = 12994 }
      { sexe = "f"; decennie = 1970; prenom = "Natacha"; naissances = 12802 }
      { sexe = "f"; decennie = 1970; prenom = "Pascale"; naissances = 12438 }
      { sexe = "f"; decennie = 1970; prenom = "Sarah"; naissances = 12013 }
      { sexe = "f"; decennie = 1970; prenom = "Lydie"; naissances = 11938 }
      { sexe = "f"; decennie = 1970; prenom = "Françoise"; naissances = 11902 }
      { sexe = "f"; decennie = 1970; prenom = "Rachel"; naissances = 11875 }
      { sexe = "f"; decennie = 1970; prenom = "Nadine"; naissances = 11553 }
      { sexe = "f"; decennie = 1970; prenom = "Fanny"; naissances = 11421 }
      { sexe = "f"; decennie = 1970; prenom = "Magalie"; naissances = 11206 }
      { sexe = "f"; decennie = 1970; prenom = "Christel"; naissances = 11176 }
      { sexe = "f"; decennie = 1970; prenom = "Marie-Laure"; naissances = 11045 }
      { sexe = "f"; decennie = 1970; prenom = "Aline"; naissances = 10965 }
      { sexe = "f"; decennie = 1970; prenom = "Aurore"; naissances = 10936 }
      { sexe = "f"; decennie = 1970; prenom = "Cathy"; naissances = 10582 }
      { sexe = "f"; decennie = 1970; prenom = "Murielle"; naissances = 10204 }
      { sexe = "f"; decennie = 1970; prenom = "Linda"; naissances = 10101 }
      { sexe = "f"; decennie = 1970; prenom = "Martine"; naissances = 9827 }
      { sexe = "f"; decennie = 1970; prenom = "Anne-Sophie"; naissances = 9790 }
      { sexe = "f"; decennie = 1970; prenom = "Séverine"; naissances = 9571 }
      { sexe = "f"; decennie = 1970; prenom = "Barbara"; naissances = 9235 }
      { sexe = "f"; decennie = 1970; prenom = "Maryline"; naissances = 9069 }
      { sexe = "f"; decennie = 1970; prenom = "Dominique"; naissances = 8800 }
      { sexe = "f"; decennie = 1970; prenom = "Nelly"; naissances = 8782 }
      { sexe = "f"; decennie = 1970; prenom = "Benedicte"; naissances = 8769 }
      { sexe = "f"; decennie = 1970; prenom = "Brigitte"; naissances = 8558 }
      { sexe = "f"; decennie = 1970; prenom = "Aude"; naissances = 8524 }
      { sexe = "f"; decennie = 1970; prenom = "Marjorie"; naissances = 8215 }
      { sexe = "f"; decennie = 1970; prenom = "Marina"; naissances = 8034 }
      { sexe = "f"; decennie = 1970; prenom = "Marion"; naissances = 7825 }
      { sexe = "f"; decennie = 1970; prenom = "Maud"; naissances = 7818 }
      { sexe = "f"; decennie = 1970; prenom = "Chantal"; naissances = 7750 }
      { sexe = "f"; decennie = 1970; prenom = "Christèle"; naissances = 7271 }
      { sexe = "f"; decennie = 1970; prenom = "Lydia"; naissances = 6978 }
      { sexe = "f"; decennie = 1970; prenom = "Ludivine"; naissances = 6713 }
      { sexe = "f"; decennie = 1970; prenom = "Marie-Pierre"; naissances = 6632 }
      { sexe = "f"; decennie = 1970; prenom = "Sylvia"; naissances = 6479 }
      { sexe = "f"; decennie = 1970; prenom = "Evelyne"; naissances = 6439 }
      { sexe = "f"; decennie = 1970; prenom = "Fatima"; naissances = 6288 }
      { sexe = "f"; decennie = 1970; prenom = "Malika"; naissances = 6208 }
      { sexe = "f"; decennie = 1970; prenom = "Armelle"; naissances = 6158 }
      { sexe = "f"; decennie = 1970; prenom = "Chrystelle"; naissances = 6135 }
      { sexe = "f"; decennie = 1970; prenom = "Gwenaelle"; naissances = 6127 }
      { sexe = "f"; decennie = 1970; prenom = "Cindy"; naissances = 6054 }
      { sexe = "f"; decennie = 1970; prenom = "Annabelle"; naissances = 6048 }
      { sexe = "f"; decennie = 1970; prenom = "Jennifer"; naissances = 5892 }
      { sexe = "f"; decennie = 1970; prenom = "Marianne"; naissances = 5685 }
      { sexe = "f"; decennie = 1970; prenom = "Amélie"; naissances = 5618 }
      { sexe = "f"; decennie = 1970; prenom = "Anne-Laure"; naissances = 5604 }
      { sexe = "f"; decennie = 1970; prenom = "Marie-Christine"; naissances = 5524 }
      { sexe = "f"; decennie = 1970; prenom = "Charlotte"; naissances = 5519 }
      { sexe = "f"; decennie = 1970; prenom = "Mireille"; naissances = 5475 }
      { sexe = "f"; decennie = 1970; prenom = "Annie"; naissances = 5470 }
      { sexe = "f"; decennie = 1970; prenom = "Marlène"; naissances = 5380 }
      { sexe = "f"; decennie = 1970; prenom = "Adeline"; naissances = 5332 }
      { sexe = "f"; decennie = 1970; prenom = "Jessica"; naissances = 5269 }
      { sexe = "f"; decennie = 1970; prenom = "Elise"; naissances = 5262 }
      { sexe = "f"; decennie = 1970; prenom = "Claudine"; naissances = 5202 }
      { sexe = "f"; decennie = 1970; prenom = "Coralie"; naissances = 5191 }
      { sexe = "f"; decennie = 1970; prenom = "Alice"; naissances = 5101 }
      { sexe = "f"; decennie = 1970; prenom = "Anne-Marie"; naissances = 5085 }
      { sexe = "f"; decennie = 1970; prenom = "Dorothee"; naissances = 5062 }
      { sexe = "f"; decennie = 1970; prenom = "Karen"; naissances = 5041 }
      { sexe = "f"; decennie = 1970; prenom = "Blandine"; naissances = 5027 }
      { sexe = "f"; decennie = 1970; prenom = "Mathilde"; naissances = 4904 }
      { sexe = "f"; decennie = 1970; prenom = "Olivia"; naissances = 4806 }
      { sexe = "f"; decennie = 1970; prenom = "Edwige"; naissances = 4730 }
      { sexe = "f"; decennie = 1970; prenom = "Marie-Hélène"; naissances = 4707 }
      { sexe = "f"; decennie = 1970; prenom = "Mylène"; naissances = 4680 }
      { sexe = "f"; decennie = 1970; prenom = "Joëlle"; naissances = 4618 }
      { sexe = "f"; decennie = 1970; prenom = "Odile"; naissances = 4575 }
      { sexe = "f"; decennie = 1970; prenom = "Betty"; naissances = 4477 }
      { sexe = "f"; decennie = 1970; prenom = "Juliette"; naissances = 4384 }
      { sexe = "f"; decennie = 1970; prenom = "Lucie"; naissances = 4376 }
      { sexe = "f"; decennie = 1970; prenom = "Anita"; naissances = 4301 }
      { sexe = "f"; decennie = 1970; prenom = "Angélique"; naissances = 4197 }
      { sexe = "f"; decennie = 1970; prenom = "Leila"; naissances = 4048 }
      { sexe = "f"; decennie = 1970; prenom = "Michèle"; naissances = 4034 }
      { sexe = "f"; decennie = 1970; prenom = "Samia"; naissances = 4003 }
      { sexe = "f"; decennie = 1970; prenom = "Nora"; naissances = 3949 }
      { sexe = "f"; decennie = 1970; prenom = "Corine"; naissances = 3948 }
      { sexe = "f"; decennie = 1970; prenom = "Samira"; naissances = 3920 }
      { sexe = "f"; decennie = 1970; prenom = "Karima"; naissances = 3873 }
      { sexe = "f"; decennie = 1970; prenom = "Annick"; naissances = 3764 }
      { sexe = "f"; decennie = 1970; prenom = "Ghislaine"; naissances = 3761 }
      { sexe = "f"; decennie = 1970; prenom = "Marilyne"; naissances = 3757 }
      { sexe = "f"; decennie = 1970; prenom = "Amandine"; naissances = 3724 }
      { sexe = "f"; decennie = 1970; prenom = "Géraldine"; naissances = 3693 }
      { sexe = "f"; decennie = 1970; prenom = "Dalila"; naissances = 3689 }
      { sexe = "f"; decennie = 1970; prenom = "Morgane"; naissances = 3617 }
      { sexe = "f"; decennie = 1970; prenom = "Marielle"; naissances = 3575 }
      { sexe = "f"; decennie = 1970; prenom = "Gaëlle"; naissances = 3529 }
      { sexe = "f"; decennie = 1970; prenom = "Cécilia"; naissances = 3522 }
      { sexe = "f"; decennie = 1970; prenom = "Edith"; naissances = 3454 }
      { sexe = "f"; decennie = 1970; prenom = "Nicole"; naissances = 3439 }
      { sexe = "f"; decennie = 1970; prenom = "Elsa"; naissances = 3435 }
      { sexe = "f"; decennie = 1970; prenom = "Deborah"; naissances = 3321 }
      { sexe = "f"; decennie = 1970; prenom = "Maryse"; naissances = 3292 }
      { sexe = "f"; decennie = 1970; prenom = "Marie-Noëlle"; naissances = 3253 }
      { sexe = "f"; decennie = 1970; prenom = "Nadège"; naissances = 3152 }
      { sexe = "f"; decennie = 1970; prenom = "Clarisse"; naissances = 3142 }
      { sexe = "f"; decennie = 1970; prenom = "Astrid"; naissances = 3099 }
      { sexe = "f"; decennie = 1970; prenom = "Monique"; naissances = 3061 }
      { sexe = "f"; decennie = 1970; prenom = "Cynthia"; naissances = 3046 }
      { sexe = "f"; decennie = 1970; prenom = "Jacqueline"; naissances = 2975 }
      { sexe = "f"; decennie = 1970; prenom = "Camille"; naissances = 2970 }
      { sexe = "f"; decennie = 1970; prenom = "Jocelyne"; naissances = 2961 }
      { sexe = "f"; decennie = 1970; prenom = "Marine"; naissances = 2945 }
      { sexe = "f"; decennie = 1970; prenom = "Anna"; naissances = 2926 }
      { sexe = "f"; decennie = 1970; prenom = "Fatiha"; naissances = 2880 }
      { sexe = "f"; decennie = 1970; prenom = "Manuela"; naissances = 2877 }
      { sexe = "f"; decennie = 1970; prenom = "Katy"; naissances = 2869 }
      { sexe = "f"; decennie = 1970; prenom = "Marie-France"; naissances = 2855 }
      { sexe = "f"; decennie = 1970; prenom = "Aurelia"; naissances = 2813 }
      { sexe = "f"; decennie = 1970; prenom = "Viviane"; naissances = 2770 }
      { sexe = "f"; decennie = 1970; prenom = "Régine"; naissances = 2768 }
      { sexe = "f"; decennie = 1970; prenom = "Chrystele"; naissances = 2767 }
      { sexe = "f"; decennie = 1970; prenom = "Sylviane"; naissances = 2749 }
      { sexe = "f"; decennie = 1970; prenom = "Farida"; naissances = 2727 }
      { sexe = "f"; decennie = 1970; prenom = "Célia"; naissances = 2715 }
      { sexe = "f"; decennie = 1970; prenom = "Manuella"; naissances = 2673 }
      { sexe = "f"; decennie = 1970; prenom = "Pauline"; naissances = 2673 }
      { sexe = "f"; decennie = 1970; prenom = "Geneviève"; naissances = 2660 }
      { sexe = "f"; decennie = 1970; prenom = "Danielle"; naissances = 2649 }
      { sexe = "f"; decennie = 1970; prenom = "Tatiana"; naissances = 2643 }
      { sexe = "f"; decennie = 1970; prenom = "Claudia"; naissances = 2617 }
      { sexe = "f"; decennie = 1970; prenom = "Alexia"; naissances = 2615 }
      { sexe = "f"; decennie = 1970; prenom = "Laëtitia"; naissances = 2576 }
      { sexe = "f"; decennie = 1970; prenom = "Laura"; naissances = 2562 }
      { sexe = "f"; decennie = 1970; prenom = "Yasmina"; naissances = 2520 }
      { sexe = "f"; decennie = 1970; prenom = "Noëlle"; naissances = 2473 }
      { sexe = "f"; decennie = 1970; prenom = "Eva"; naissances = 2452 }
      { sexe = "f"; decennie = 1970; prenom = "Pascaline"; naissances = 2441 }
      { sexe = "f"; decennie = 1970; prenom = "Bernadette"; naissances = 2438 }
      { sexe = "f"; decennie = 1970; prenom = "Tania"; naissances = 2438 }
      { sexe = "f"; decennie = 1970; prenom = "Christiane"; naissances = 2318 }
      { sexe = "f"; decennie = 1970; prenom = "Sandy"; naissances = 2314 }
      { sexe = "f"; decennie = 1970; prenom = "Eve"; naissances = 2306 }
      { sexe = "f"; decennie = 1970; prenom = "Marie-Claire"; naissances = 2302 }
      { sexe = "f"; decennie = 1970; prenom = "Francine"; naissances = 2300 }
      { sexe = "f"; decennie = 1980; prenom = "Aurélie"; naissances = 113344 }
      { sexe = "f"; decennie = 1980; prenom = "Emilie"; naissances = 92506 }
      { sexe = "f"; decennie = 1980; prenom = "Céline"; naissances = 84698 }
      { sexe = "f"; decennie = 1980; prenom = "Elodie"; naissances = 84507 }
      { sexe = "f"; decennie = 1980; prenom = "Julie"; naissances = 79686 }
      { sexe = "f"; decennie = 1980; prenom = "Marie"; naissances = 74510 }
      { sexe = "f"; decennie = 1980; prenom = "Stéphanie"; naissances = 71552 }
      { sexe = "f"; decennie = 1980; prenom = "Audrey"; naissances = 71308 }
      { sexe = "f"; decennie = 1980; prenom = "Laetitia"; naissances = 60382 }
      { sexe = "f"; decennie = 1980; prenom = "Virginie"; naissances = 58127 }
      { sexe = "f"; decennie = 1980; prenom = "Sophie"; naissances = 55001 }
      { sexe = "f"; decennie = 1980; prenom = "Mélanie"; naissances = 52021 }
      { sexe = "f"; decennie = 1980; prenom = "Sabrina"; naissances = 47602 }
      { sexe = "f"; decennie = 1980; prenom = "Vanessa"; naissances = 47463 }
      { sexe = "f"; decennie = 1980; prenom = "Caroline"; naissances = 47400 }
      { sexe = "f"; decennie = 1980; prenom = "Marion"; naissances = 40862 }
      { sexe = "f"; decennie = 1980; prenom = "Jennifer"; naissances = 39749 }
      { sexe = "f"; decennie = 1980; prenom = "Jessica"; naissances = 38913 }
      { sexe = "f"; decennie = 1980; prenom = "Amandine"; naissances = 38846 }
      { sexe = "f"; decennie = 1980; prenom = "Delphine"; naissances = 37768 }
      { sexe = "f"; decennie = 1980; prenom = "Claire"; naissances = 35771 }
      { sexe = "f"; decennie = 1980; prenom = "Sandrine"; naissances = 34111 }
      { sexe = "f"; decennie = 1980; prenom = "Cindy"; naissances = 32883 }
      { sexe = "f"; decennie = 1980; prenom = "Cécile"; naissances = 32819 }
      { sexe = "f"; decennie = 1980; prenom = "Pauline"; naissances = 32815 }
      { sexe = "f"; decennie = 1980; prenom = "Laura"; naissances = 31527 }
      { sexe = "f"; decennie = 1980; prenom = "Angelique"; naissances = 30953 }
      { sexe = "f"; decennie = 1980; prenom = "Alexandra"; naissances = 30889 }
      { sexe = "f"; decennie = 1980; prenom = "Christelle"; naissances = 30398 }
      { sexe = "f"; decennie = 1980; prenom = "Nathalie"; naissances = 30177 }
      { sexe = "f"; decennie = 1980; prenom = "Amélie"; naissances = 30079 }
      { sexe = "f"; decennie = 1980; prenom = "Hélène"; naissances = 30038 }
      { sexe = "f"; decennie = 1980; prenom = "Aurore"; naissances = 29905 }
      { sexe = "f"; decennie = 1980; prenom = "Charlotte"; naissances = 28749 }
      { sexe = "f"; decennie = 1980; prenom = "Lucie"; naissances = 28559 }
      { sexe = "f"; decennie = 1980; prenom = "Sandra"; naissances = 28452 }
      { sexe = "f"; decennie = 1980; prenom = "Sarah"; naissances = 28218 }
      { sexe = "f"; decennie = 1980; prenom = "Fanny"; naissances = 27661 }
      { sexe = "f"; decennie = 1980; prenom = "Anne"; naissances = 26393 }
      { sexe = "f"; decennie = 1980; prenom = "Marine"; naissances = 25591 }
      { sexe = "f"; decennie = 1980; prenom = "Camille"; naissances = 24076 }
      { sexe = "f"; decennie = 1980; prenom = "Isabelle"; naissances = 22824 }
      { sexe = "f"; decennie = 1980; prenom = "Anaïs"; naissances = 22797 }
      { sexe = "f"; decennie = 1980; prenom = "Emmanuelle"; naissances = 22588 }
      { sexe = "f"; decennie = 1980; prenom = "Laure"; naissances = 21178 }
      { sexe = "f"; decennie = 1980; prenom = "Adeline"; naissances = 20668 }
      { sexe = "f"; decennie = 1980; prenom = "Sonia"; naissances = 20435 }
      { sexe = "f"; decennie = 1980; prenom = "Magali"; naissances = 19302 }
      { sexe = "f"; decennie = 1980; prenom = "Gaelle"; naissances = 17911 }
      { sexe = "f"; decennie = 1980; prenom = "Severine"; naissances = 17888 }
      { sexe = "f"; decennie = 1980; prenom = "Coralie"; naissances = 17569 }
      { sexe = "f"; decennie = 1980; prenom = "Mathilde"; naissances = 17469 }
      { sexe = "f"; decennie = 1980; prenom = "Karine"; naissances = 16626 }
      { sexe = "f"; decennie = 1980; prenom = "Ludivine"; naissances = 16378 }
      { sexe = "f"; decennie = 1980; prenom = "Florence"; naissances = 16189 }
      { sexe = "f"; decennie = 1980; prenom = "Carole"; naissances = 15889 }
      { sexe = "f"; decennie = 1980; prenom = "Aline"; naissances = 15750 }
      { sexe = "f"; decennie = 1980; prenom = "Elise"; naissances = 14712 }
      { sexe = "f"; decennie = 1980; prenom = "Valérie"; naissances = 14554 }
      { sexe = "f"; decennie = 1980; prenom = "Estelle"; naissances = 14400 }
      { sexe = "f"; decennie = 1980; prenom = "Morgane"; naissances = 13877 }
      { sexe = "f"; decennie = 1980; prenom = "Anne-Sophie"; naissances = 13631 }
      { sexe = "f"; decennie = 1980; prenom = "Nadia"; naissances = 13230 }
      { sexe = "f"; decennie = 1980; prenom = "Myriam"; naissances = 13095 }
      { sexe = "f"; decennie = 1980; prenom = "Justine"; naissances = 12598 }
      { sexe = "f"; decennie = 1980; prenom = "Alice"; naissances = 12216 }
      { sexe = "f"; decennie = 1980; prenom = "Chloé"; naissances = 12195 }
      { sexe = "f"; decennie = 1980; prenom = "Marjorie"; naissances = 12086 }
      { sexe = "f"; decennie = 1980; prenom = "Aude"; naissances = 11952 }
      { sexe = "f"; decennie = 1980; prenom = "Charlène"; naissances = 11831 }
      { sexe = "f"; decennie = 1980; prenom = "Mélissa"; naissances = 11688 }
      { sexe = "f"; decennie = 1980; prenom = "Maud"; naissances = 11585 }
      { sexe = "f"; decennie = 1980; prenom = "Marina"; naissances = 11129 }
      { sexe = "f"; decennie = 1980; prenom = "Carine"; naissances = 10915 }
      { sexe = "f"; decennie = 1980; prenom = "Nadege"; naissances = 10364 }
      { sexe = "f"; decennie = 1980; prenom = "Sylvie"; naissances = 9952 }
      { sexe = "f"; decennie = 1980; prenom = "Clémence"; naissances = 9832 }
      { sexe = "f"; decennie = 1980; prenom = "Magalie"; naissances = 9631 }
      { sexe = "f"; decennie = 1980; prenom = "Anne-Laure"; naissances = 9549 }
      { sexe = "f"; decennie = 1980; prenom = "Catherine"; naissances = 9546 }
      { sexe = "f"; decennie = 1980; prenom = "Linda"; naissances = 9426 }
      { sexe = "f"; decennie = 1980; prenom = "Deborah"; naissances = 9396 }
      { sexe = "f"; decennie = 1980; prenom = "Johanna"; naissances = 9389 }
      { sexe = "f"; decennie = 1980; prenom = "Laurie"; naissances = 9293 }
      { sexe = "f"; decennie = 1980; prenom = "Emeline"; naissances = 9056 }
      { sexe = "f"; decennie = 1980; prenom = "Marlène"; naissances = 9038 }
      { sexe = "f"; decennie = 1980; prenom = "Laurence"; naissances = 8987 }
      { sexe = "f"; decennie = 1980; prenom = "Natacha"; naissances = 8920 }
      { sexe = "f"; decennie = 1980; prenom = "Noemie"; naissances = 8828 }
      { sexe = "f"; decennie = 1980; prenom = "Véronique"; naissances = 8801 }
      { sexe = "f"; decennie = 1980; prenom = "Ingrid"; naissances = 8592 }
      { sexe = "f"; decennie = 1980; prenom = "Ophélie"; naissances = 8573 }
      { sexe = "f"; decennie = 1980; prenom = "Elsa"; naissances = 8350 }
      { sexe = "f"; decennie = 1980; prenom = "Angélique"; naissances = 8101 }
      { sexe = "f"; decennie = 1980; prenom = "Nelly"; naissances = 8052 }
      { sexe = "f"; decennie = 1980; prenom = "Lydie"; naissances = 7915 }
      { sexe = "f"; decennie = 1980; prenom = "Cynthia"; naissances = 7785 }
      { sexe = "f"; decennie = 1980; prenom = "Priscilla"; naissances = 7687 }
      { sexe = "f"; decennie = 1980; prenom = "Geraldine"; naissances = 7640 }
      { sexe = "f"; decennie = 1980; prenom = "Perrine"; naissances = 7284 }
      { sexe = "f"; decennie = 1980; prenom = "Mylène"; naissances = 7171 }
      { sexe = "f"; decennie = 1980; prenom = "Olivia"; naissances = 7113 }
      { sexe = "f"; decennie = 1980; prenom = "Christine"; naissances = 6974 }
      { sexe = "f"; decennie = 1980; prenom = "Juliette"; naissances = 6890 }
      { sexe = "f"; decennie = 1980; prenom = "Léa"; naissances = 6886 }
      { sexe = "f"; decennie = 1980; prenom = "Lucile"; naissances = 6827 }
      { sexe = "f"; decennie = 1980; prenom = "Julia"; naissances = 6773 }
      { sexe = "f"; decennie = 1980; prenom = "Alexia"; naissances = 6485 }
      { sexe = "f"; decennie = 1980; prenom = "Sabine"; naissances = 6318 }
      { sexe = "f"; decennie = 1980; prenom = "Kelly"; naissances = 6305 }
      { sexe = "f"; decennie = 1980; prenom = "Marie-Laure"; naissances = 6252 }
      { sexe = "f"; decennie = 1980; prenom = "Manon"; naissances = 6204 }
      { sexe = "f"; decennie = 1980; prenom = "Floriane"; naissances = 6177 }
      { sexe = "f"; decennie = 1980; prenom = "Elisabeth"; naissances = 6133 }
      { sexe = "f"; decennie = 1980; prenom = "Sandy"; naissances = 6098 }
      { sexe = "f"; decennie = 1980; prenom = "Célia"; naissances = 6065 }
      { sexe = "f"; decennie = 1980; prenom = "Solène"; naissances = 6061 }
      { sexe = "f"; decennie = 1980; prenom = "Rachel"; naissances = 5992 }
      { sexe = "f"; decennie = 1980; prenom = "Leslie"; naissances = 5964 }
      { sexe = "f"; decennie = 1980; prenom = "Cécilia"; naissances = 5958 }
      { sexe = "f"; decennie = 1980; prenom = "Barbara"; naissances = 5931 }
      { sexe = "f"; decennie = 1980; prenom = "Aurelia"; naissances = 5915 }
      { sexe = "f"; decennie = 1980; prenom = "Agnès"; naissances = 5898 }
      { sexe = "f"; decennie = 1980; prenom = "Gaëlle"; naissances = 5897 }
      { sexe = "f"; decennie = 1980; prenom = "Charline"; naissances = 5848 }
      { sexe = "f"; decennie = 1980; prenom = "Marianne"; naissances = 5745 }
      { sexe = "f"; decennie = 1980; prenom = "Patricia"; naissances = 5649 }
      { sexe = "f"; decennie = 1980; prenom = "Fatima"; naissances = 5600 }
      { sexe = "f"; decennie = 1980; prenom = "Blandine"; naissances = 5495 }
      { sexe = "f"; decennie = 1980; prenom = "Dorothee"; naissances = 5404 }
      { sexe = "f"; decennie = 1980; prenom = "Laëtitia"; naissances = 5386 }
      { sexe = "f"; decennie = 1980; prenom = "Tiphaine"; naissances = 5323 }
      { sexe = "f"; decennie = 1980; prenom = "Annabelle"; naissances = 5269 }
      { sexe = "f"; decennie = 1980; prenom = "Clémentine"; naissances = 5233 }
      { sexe = "f"; decennie = 1980; prenom = "Maryline"; naissances = 5229 }
      { sexe = "f"; decennie = 1980; prenom = "Béatrice"; naissances = 5146 }
      { sexe = "f"; decennie = 1980; prenom = "Pamela"; naissances = 5115 }
      { sexe = "f"; decennie = 1980; prenom = "Eva"; naissances = 5113 }
      { sexe = "f"; decennie = 1980; prenom = "Samira"; naissances = 5113 }
      { sexe = "f"; decennie = 1980; prenom = "Katia"; naissances = 5083 }
      { sexe = "f"; decennie = 1980; prenom = "Leila"; naissances = 5066 }
      { sexe = "f"; decennie = 1980; prenom = "Benedicte"; naissances = 4989 }
      { sexe = "f"; decennie = 1980; prenom = "Gwendoline"; naissances = 4875 }
      { sexe = "f"; decennie = 1980; prenom = "Gwenaelle"; naissances = 4758 }
      { sexe = "f"; decennie = 1980; prenom = "Lise"; naissances = 4721 }
      { sexe = "f"; decennie = 1980; prenom = "Agathe"; naissances = 4682 }
      { sexe = "f"; decennie = 1980; prenom = "Cathy"; naissances = 4659 }
      { sexe = "f"; decennie = 1980; prenom = "Séverine"; naissances = 4600 }
      { sexe = "f"; decennie = 1980; prenom = "Frédérique"; naissances = 4598 }
      { sexe = "f"; decennie = 1980; prenom = "Corinne"; naissances = 4591 }
      { sexe = "f"; decennie = 1980; prenom = "Tatiana"; naissances = 4502 }
      { sexe = "f"; decennie = 1980; prenom = "Cyrielle"; naissances = 4445 }
      { sexe = "f"; decennie = 1980; prenom = "Alicia"; naissances = 4442 }
      { sexe = "f"; decennie = 1980; prenom = "Fabienne"; naissances = 4431 }
      { sexe = "f"; decennie = 1980; prenom = "Malika"; naissances = 4421 }
      { sexe = "f"; decennie = 1980; prenom = "Muriel"; naissances = 4389 }
      { sexe = "f"; decennie = 1980; prenom = "Maeva"; naissances = 4182 }
      { sexe = "f"; decennie = 1980; prenom = "Karen"; naissances = 4163 }
      { sexe = "f"; decennie = 1980; prenom = "Lisa"; naissances = 4108 }
      { sexe = "f"; decennie = 1980; prenom = "Samia"; naissances = 4103 }
      { sexe = "f"; decennie = 1980; prenom = "Karima"; naissances = 3997 }
      { sexe = "f"; decennie = 1980; prenom = "Lauriane"; naissances = 3990 }
      { sexe = "f"; decennie = 1980; prenom = "Diane"; naissances = 3947 }
      { sexe = "f"; decennie = 1980; prenom = "Tiffany"; naissances = 3685 }
      { sexe = "f"; decennie = 1980; prenom = "Anissa"; naissances = 3676 }
      { sexe = "f"; decennie = 1980; prenom = "Samantha"; naissances = 3661 }
      { sexe = "f"; decennie = 1980; prenom = "Anna"; naissances = 3568 }
      { sexe = "f"; decennie = 1980; prenom = "Margaux"; naissances = 3479 }
      { sexe = "f"; decennie = 1980; prenom = "Sylvia"; naissances = 3443 }
      { sexe = "f"; decennie = 1980; prenom = "Louise"; naissances = 3401 }
      { sexe = "f"; decennie = 1980; prenom = "Erika"; naissances = 3356 }
      { sexe = "f"; decennie = 1980; prenom = "Lydia"; naissances = 3339 }
      { sexe = "f"; decennie = 1980; prenom = "Betty"; naissances = 3236 }
      { sexe = "f"; decennie = 1980; prenom = "Jeanne"; naissances = 3228 }
      { sexe = "f"; decennie = 1980; prenom = "Roxane"; naissances = 3223 }
      { sexe = "f"; decennie = 1980; prenom = "Rebecca"; naissances = 3216 }
      { sexe = "f"; decennie = 1980; prenom = "Clarisse"; naissances = 3188 }
      { sexe = "f"; decennie = 1980; prenom = "Edwige"; naissances = 3169 }
      { sexe = "f"; decennie = 1980; prenom = "Astrid"; naissances = 3103 }
      { sexe = "f"; decennie = 1980; prenom = "Noémie"; naissances = 3013 }
      { sexe = "f"; decennie = 1980; prenom = "Anne-Lise"; naissances = 3011 }
      { sexe = "f"; decennie = 1980; prenom = "Nora"; naissances = 2974 }
      { sexe = "f"; decennie = 1980; prenom = "Flora"; naissances = 2969 }
      { sexe = "f"; decennie = 1980; prenom = "Nadège"; naissances = 2954 }
      { sexe = "f"; decennie = 1980; prenom = "Clara"; naissances = 2953 }
      { sexe = "f"; decennie = 1980; prenom = "Héloïse"; naissances = 2821 }
      { sexe = "f"; decennie = 1980; prenom = "Priscillia"; naissances = 2811 }
      { sexe = "f"; decennie = 1980; prenom = "Eve"; naissances = 2771 }
      { sexe = "f"; decennie = 1980; prenom = "Elisa"; naissances = 2763 }
      { sexe = "f"; decennie = 1980; prenom = "Nolwenn"; naissances = 2758 }
      { sexe = "f"; decennie = 1980; prenom = "Eugénie"; naissances = 2752 }
      { sexe = "f"; decennie = 1980; prenom = "Alison"; naissances = 2734 }
      { sexe = "f"; decennie = 1980; prenom = "Violaine"; naissances = 2725 }
      { sexe = "f"; decennie = 1980; prenom = "Sophia"; naissances = 2704 }
      { sexe = "f"; decennie = 1980; prenom = "Axelle"; naissances = 2697 }
      { sexe = "f"; decennie = 1980; prenom = "Yasmina"; naissances = 2661 }
      { sexe = "f"; decennie = 1980; prenom = "Melodie"; naissances = 2640 }
      { sexe = "f"; decennie = 1980; prenom = "Émilie"; naissances = 2594 }
      { sexe = "f"; decennie = 1980; prenom = "Anne-Claire"; naissances = 2568 }
      { sexe = "f"; decennie = 1980; prenom = "Marilyne"; naissances = 2556 }
      { sexe = "f"; decennie = 1990; prenom = "Laura"; naissances = 70049 }
      { sexe = "f"; decennie = 1990; prenom = "Marie"; naissances = 69798 }
      { sexe = "f"; decennie = 1990; prenom = "Camille"; naissances = 67192 }
      { sexe = "f"; decennie = 1990; prenom = "Marine"; naissances = 64198 }
      { sexe = "f"; decennie = 1990; prenom = "Manon"; naissances = 63317 }
      { sexe = "f"; decennie = 1990; prenom = "Julie"; naissances = 57384 }
      { sexe = "f"; decennie = 1990; prenom = "Pauline"; naissances = 56186 }
      { sexe = "f"; decennie = 1990; prenom = "Léa"; naissances = 52354 }
      { sexe = "f"; decennie = 1990; prenom = "Anaïs"; naissances = 52128 }
      { sexe = "f"; decennie = 1990; prenom = "Marion"; naissances = 51704 }
      { sexe = "f"; decennie = 1990; prenom = "Sarah"; naissances = 50422 }
      { sexe = "f"; decennie = 1990; prenom = "Elodie"; naissances = 45329 }
      { sexe = "f"; decennie = 1990; prenom = "Mélanie"; naissances = 44448 }
      { sexe = "f"; decennie = 1990; prenom = "Mathilde"; naissances = 44378 }
      { sexe = "f"; decennie = 1990; prenom = "Chloé"; naissances = 43382 }
      { sexe = "f"; decennie = 1990; prenom = "Justine"; naissances = 40387 }
      { sexe = "f"; decennie = 1990; prenom = "Emilie"; naissances = 34360 }
      { sexe = "f"; decennie = 1990; prenom = "Morgane"; naissances = 34328 }
      { sexe = "f"; decennie = 1990; prenom = "Charlotte"; naissances = 34053 }
      { sexe = "f"; decennie = 1990; prenom = "Audrey"; naissances = 33295 }
      { sexe = "f"; decennie = 1990; prenom = "Amandine"; naissances = 33027 }
      { sexe = "f"; decennie = 1990; prenom = "Lucie"; naissances = 32762 }
      { sexe = "f"; decennie = 1990; prenom = "Océane"; naissances = 31445 }
      { sexe = "f"; decennie = 1990; prenom = "Aurélie"; naissances = 29104 }
      { sexe = "f"; decennie = 1990; prenom = "Mélissa"; naissances = 28657 }
      { sexe = "f"; decennie = 1990; prenom = "Amélie"; naissances = 26373 }
      { sexe = "f"; decennie = 1990; prenom = "Claire"; naissances = 26241 }
      { sexe = "f"; decennie = 1990; prenom = "Céline"; naissances = 24176 }
      { sexe = "f"; decennie = 1990; prenom = "Margaux"; naissances = 22778 }
      { sexe = "f"; decennie = 1990; prenom = "Sophie"; naissances = 22518 }
      { sexe = "f"; decennie = 1990; prenom = "Fanny"; naissances = 22385 }
      { sexe = "f"; decennie = 1990; prenom = "Caroline"; naissances = 22292 }
      { sexe = "f"; decennie = 1990; prenom = "Cindy"; naissances = 21543 }
      { sexe = "f"; decennie = 1990; prenom = "Coralie"; naissances = 20937 }
      { sexe = "f"; decennie = 1990; prenom = "Clémence"; naissances = 20878 }
      { sexe = "f"; decennie = 1990; prenom = "Juliette"; naissances = 20477 }
      { sexe = "f"; decennie = 1990; prenom = "Jessica"; naissances = 19472 }
      { sexe = "f"; decennie = 1990; prenom = "Ophélie"; naissances = 19450 }
      { sexe = "f"; decennie = 1990; prenom = "Estelle"; naissances = 19380 }
      { sexe = "f"; decennie = 1990; prenom = "Clara"; naissances = 19126 }
      { sexe = "f"; decennie = 1990; prenom = "Laetitia"; naissances = 19026 }
      { sexe = "f"; decennie = 1990; prenom = "Alice"; naissances = 18697 }
      { sexe = "f"; decennie = 1990; prenom = "Alexandra"; naissances = 18696 }
      { sexe = "f"; decennie = 1990; prenom = "Lisa"; naissances = 18632 }
      { sexe = "f"; decennie = 1990; prenom = "Emma"; naissances = 18451 }
      { sexe = "f"; decennie = 1990; prenom = "Maeva"; naissances = 17814 }
      { sexe = "f"; decennie = 1990; prenom = "Adeline"; naissances = 17267 }
      { sexe = "f"; decennie = 1990; prenom = "Jennifer"; naissances = 16742 }
      { sexe = "f"; decennie = 1990; prenom = "Inès"; naissances = 16346 }
      { sexe = "f"; decennie = 1990; prenom = "Noemie"; naissances = 15926 }
      { sexe = "f"; decennie = 1990; prenom = "Charlène"; naissances = 15914 }
      { sexe = "f"; decennie = 1990; prenom = "Elise"; naissances = 15342 }
      { sexe = "f"; decennie = 1990; prenom = "Louise"; naissances = 14846 }
      { sexe = "f"; decennie = 1990; prenom = "Alexia"; naissances = 14826 }
      { sexe = "f"; decennie = 1990; prenom = "Célia"; naissances = 14452 }
      { sexe = "f"; decennie = 1990; prenom = "Aurore"; naissances = 14414 }
      { sexe = "f"; decennie = 1990; prenom = "Laurie"; naissances = 14131 }
      { sexe = "f"; decennie = 1990; prenom = "Hélène"; naissances = 14049 }
      { sexe = "f"; decennie = 1990; prenom = "Sabrina"; naissances = 13800 }
      { sexe = "f"; decennie = 1990; prenom = "Cécile"; naissances = 13541 }
      { sexe = "f"; decennie = 1990; prenom = "Eva"; naissances = 13452 }
      { sexe = "f"; decennie = 1990; prenom = "Margot"; naissances = 13444 }
      { sexe = "f"; decennie = 1990; prenom = "Stéphanie"; naissances = 13408 }
      { sexe = "f"; decennie = 1990; prenom = "Emeline"; naissances = 13402 }
      { sexe = "f"; decennie = 1990; prenom = "Marina"; naissances = 13384 }
      { sexe = "f"; decennie = 1990; prenom = "Alicia"; naissances = 13382 }
      { sexe = "f"; decennie = 1990; prenom = "Sandra"; naissances = 12127 }
      { sexe = "f"; decennie = 1990; prenom = "Ludivine"; naissances = 12038 }
      { sexe = "f"; decennie = 1990; prenom = "Angelique"; naissances = 11624 }
      { sexe = "f"; decennie = 1990; prenom = "Solène"; naissances = 11582 }
      { sexe = "f"; decennie = 1990; prenom = "Elisa"; naissances = 11376 }
      { sexe = "f"; decennie = 1990; prenom = "Cassandra"; naissances = 11065 }
      { sexe = "f"; decennie = 1990; prenom = "Agathe"; naissances = 10729 }
      { sexe = "f"; decennie = 1990; prenom = "Laure"; naissances = 10601 }
      { sexe = "f"; decennie = 1990; prenom = "Laurine"; naissances = 10458 }
      { sexe = "f"; decennie = 1990; prenom = "Myriam"; naissances = 10393 }
      { sexe = "f"; decennie = 1990; prenom = "Gwendoline"; naissances = 10357 }
      { sexe = "f"; decennie = 1990; prenom = "Johanna"; naissances = 10319 }
      { sexe = "f"; decennie = 1990; prenom = "Julia"; naissances = 10196 }
      { sexe = "f"; decennie = 1990; prenom = "Megane"; naissances = 10123 }
      { sexe = "f"; decennie = 1990; prenom = "Valentine"; naissances = 10112 }
      { sexe = "f"; decennie = 1990; prenom = "Kelly"; naissances = 10104 }
      { sexe = "f"; decennie = 1990; prenom = "Victoria"; naissances = 10053 }
      { sexe = "f"; decennie = 1990; prenom = "Romane"; naissances = 9808 }
      { sexe = "f"; decennie = 1990; prenom = "Alison"; naissances = 9741 }
      { sexe = "f"; decennie = 1990; prenom = "Virginie"; naissances = 9621 }
      { sexe = "f"; decennie = 1990; prenom = "Lucile"; naissances = 9383 }
      { sexe = "f"; decennie = 1990; prenom = "Sonia"; naissances = 9278 }
      { sexe = "f"; decennie = 1990; prenom = "Delphine"; naissances = 9243 }
      { sexe = "f"; decennie = 1990; prenom = "Deborah"; naissances = 9208 }
      { sexe = "f"; decennie = 1990; prenom = "Andréa"; naissances = 9144 }
      { sexe = "f"; decennie = 1990; prenom = "Tiffany"; naissances = 8853 }
      { sexe = "f"; decennie = 1990; prenom = "Jeanne"; naissances = 8784 }
      { sexe = "f"; decennie = 1990; prenom = "Clémentine"; naissances = 8632 }
      { sexe = "f"; decennie = 1990; prenom = "Floriane"; naissances = 8411 }
      { sexe = "f"; decennie = 1990; prenom = "Cynthia"; naissances = 8401 }
      { sexe = "f"; decennie = 1990; prenom = "Samantha"; naissances = 8384 }
      { sexe = "f"; decennie = 1990; prenom = "Charline"; naissances = 8375 }
      { sexe = "f"; decennie = 1990; prenom = "Gaelle"; naissances = 8074 }
      { sexe = "f"; decennie = 1990; prenom = "Elsa"; naissances = 7742 }
      { sexe = "f"; decennie = 1990; prenom = "Anne"; naissances = 7700 }
      { sexe = "f"; decennie = 1990; prenom = "Vanessa"; naissances = 7556 }
      { sexe = "f"; decennie = 1990; prenom = "Jade"; naissances = 7555 }
      { sexe = "f"; decennie = 1990; prenom = "Emmanuelle"; naissances = 7389 }
      { sexe = "f"; decennie = 1990; prenom = "Olivia"; naissances = 7061 }
      { sexe = "f"; decennie = 1990; prenom = "Coline"; naissances = 6984 }
      { sexe = "f"; decennie = 1990; prenom = "Axelle"; naissances = 6874 }
      { sexe = "f"; decennie = 1990; prenom = "Marjorie"; naissances = 6862 }
      { sexe = "f"; decennie = 1990; prenom = "Maud"; naissances = 6604 }
      { sexe = "f"; decennie = 1990; prenom = "Lola"; naissances = 6437 }
      { sexe = "f"; decennie = 1990; prenom = "Salomé"; naissances = 6356 }
      { sexe = "f"; decennie = 1990; prenom = "Perrine"; naissances = 6271 }
      { sexe = "f"; decennie = 1990; prenom = "Aline"; naissances = 6142 }
      { sexe = "f"; decennie = 1990; prenom = "Tiphaine"; naissances = 6142 }
      { sexe = "f"; decennie = 1990; prenom = "Nina"; naissances = 6053 }
      { sexe = "f"; decennie = 1990; prenom = "Anna"; naissances = 5953 }
      { sexe = "f"; decennie = 1990; prenom = "Anne-Sophie"; naissances = 5934 }
      { sexe = "f"; decennie = 1990; prenom = "Anissa"; naissances = 5920 }
      { sexe = "f"; decennie = 1990; prenom = "Aude"; naissances = 5759 }
      { sexe = "f"; decennie = 1990; prenom = "Mylène"; naissances = 5749 }
      { sexe = "f"; decennie = 1990; prenom = "Zoé"; naissances = 5712 }
      { sexe = "f"; decennie = 1990; prenom = "Clarisse"; naissances = 5704 }
      { sexe = "f"; decennie = 1990; prenom = "Wendy"; naissances = 5673 }
      { sexe = "f"; decennie = 1990; prenom = "Léna"; naissances = 5661 }
      { sexe = "f"; decennie = 1990; prenom = "Eloise"; naissances = 5578 }
      { sexe = "f"; decennie = 1990; prenom = "Sandy"; naissances = 5526 }
      { sexe = "f"; decennie = 1990; prenom = "Carla"; naissances = 5497 }
      { sexe = "f"; decennie = 1990; prenom = "Constance"; naissances = 5412 }
      { sexe = "f"; decennie = 1990; prenom = "Héloïse"; naissances = 5400 }
      { sexe = "f"; decennie = 1990; prenom = "Christelle"; naissances = 5392 }
      { sexe = "f"; decennie = 1990; prenom = "Lauriane"; naissances = 5360 }
      { sexe = "f"; decennie = 1990; prenom = "Lise"; naissances = 5277 }
      { sexe = "f"; decennie = 1990; prenom = "Florine"; naissances = 5225 }
      { sexe = "f"; decennie = 1990; prenom = "Fiona"; naissances = 5066 }
      { sexe = "f"; decennie = 1990; prenom = "Melodie"; naissances = 5064 }
      { sexe = "f"; decennie = 1990; prenom = "Roxane"; naissances = 5034 }
      { sexe = "f"; decennie = 1990; prenom = "Sara"; naissances = 5011 }
      { sexe = "f"; decennie = 1990; prenom = "Maelle"; naissances = 5004 }
      { sexe = "f"; decennie = 1990; prenom = "Rachel"; naissances = 4978 }
      { sexe = "f"; decennie = 1990; prenom = "Leila"; naissances = 4895 }
      { sexe = "f"; decennie = 1990; prenom = "Leslie"; naissances = 4856 }
      { sexe = "f"; decennie = 1990; prenom = "Marianne"; naissances = 4743 }
      { sexe = "f"; decennie = 1990; prenom = "Cécilia"; naissances = 4697 }
      { sexe = "f"; decennie = 1990; prenom = "Anaelle"; naissances = 4677 }
      { sexe = "f"; decennie = 1990; prenom = "Flora"; naissances = 4659 }
      { sexe = "f"; decennie = 1990; prenom = "Florence"; naissances = 4641 }
      { sexe = "f"; decennie = 1990; prenom = "Barbara"; naissances = 4525 }
      { sexe = "f"; decennie = 1990; prenom = "Ambre"; naissances = 4473 }
      { sexe = "f"; decennie = 1990; prenom = "Natacha"; naissances = 4441 }
      { sexe = "f"; decennie = 1990; prenom = "Nolwenn"; naissances = 4438 }
      { sexe = "f"; decennie = 1990; prenom = "Isabelle"; naissances = 4434 }
      { sexe = "f"; decennie = 1990; prenom = "Sandrine"; naissances = 4424 }
      { sexe = "f"; decennie = 1990; prenom = "Alizée"; naissances = 4380 }
      { sexe = "f"; decennie = 1990; prenom = "Mélina"; naissances = 4369 }
      { sexe = "f"; decennie = 1990; prenom = "Diane"; naissances = 4286 }
      { sexe = "f"; decennie = 1990; prenom = "Gabrielle"; naissances = 4271 }
      { sexe = "f"; decennie = 1990; prenom = "Adèle"; naissances = 4251 }
      { sexe = "f"; decennie = 1990; prenom = "Blandine"; naissances = 4200 }
      { sexe = "f"; decennie = 1990; prenom = "Nathalie"; naissances = 4096 }
      { sexe = "f"; decennie = 1990; prenom = "Carole"; naissances = 4091 }
      { sexe = "f"; decennie = 1990; prenom = "Tatiana"; naissances = 4074 }
      { sexe = "f"; decennie = 1990; prenom = "Alix"; naissances = 4070 }
      { sexe = "f"; decennie = 1990; prenom = "Nadia"; naissances = 4010 }
      { sexe = "f"; decennie = 1990; prenom = "Linda"; naissances = 3839 }
      { sexe = "f"; decennie = 1990; prenom = "Doriane"; naissances = 3836 }
      { sexe = "f"; decennie = 1990; prenom = "Marlène"; naissances = 3821 }
      { sexe = "f"; decennie = 1990; prenom = "Maureen"; naissances = 3814 }
      { sexe = "f"; decennie = 1990; prenom = "Gwenaelle"; naissances = 3810 }
      { sexe = "f"; decennie = 1990; prenom = "Priscilla"; naissances = 3801 }
      { sexe = "f"; decennie = 1990; prenom = "Coraline"; naissances = 3794 }
      { sexe = "f"; decennie = 1990; prenom = "Lou"; naissances = 3768 }
      { sexe = "f"; decennie = 1990; prenom = "Priscillia"; naissances = 3750 }
      { sexe = "f"; decennie = 1990; prenom = "Melody"; naissances = 3674 }
      { sexe = "f"; decennie = 1990; prenom = "Laurene"; naissances = 3632 }
      { sexe = "f"; decennie = 1990; prenom = "Cyrielle"; naissances = 3579 }
      { sexe = "f"; decennie = 1990; prenom = "Cassandre"; naissances = 3561 }
      { sexe = "f"; decennie = 1990; prenom = "Noémie"; naissances = 3514 }
      { sexe = "f"; decennie = 1990; prenom = "Candice"; naissances = 3504 }
      { sexe = "f"; decennie = 1990; prenom = "Kimberley"; naissances = 3492 }
      { sexe = "f"; decennie = 1990; prenom = "Lucille"; naissances = 3418 }
      { sexe = "f"; decennie = 1990; prenom = "Annabelle"; naissances = 3399 }
      { sexe = "f"; decennie = 1990; prenom = "Karine"; naissances = 3364 }
      { sexe = "f"; decennie = 1990; prenom = "Kenza"; naissances = 3354 }
      { sexe = "f"; decennie = 1990; prenom = "Sophia"; naissances = 3351 }
      { sexe = "f"; decennie = 1990; prenom = "Sofia"; naissances = 3326 }
      { sexe = "f"; decennie = 1990; prenom = "Cloe"; naissances = 3298 }
      { sexe = "f"; decennie = 1990; prenom = "Prescillia"; naissances = 3288 }
      { sexe = "f"; decennie = 1990; prenom = "Anne-Laure"; naissances = 3279 }
      { sexe = "f"; decennie = 1990; prenom = "Magali"; naissances = 3272 }
      { sexe = "f"; decennie = 1990; prenom = "Eugénie"; naissances = 3250 }
      { sexe = "f"; decennie = 1990; prenom = "Eleonore"; naissances = 3209 }
      { sexe = "f"; decennie = 1990; prenom = "Rebecca"; naissances = 3204 }
      { sexe = "f"; decennie = 1990; prenom = "Ingrid"; naissances = 3165 }
      { sexe = "f"; decennie = 1990; prenom = "Oriane"; naissances = 3138 }
      { sexe = "f"; decennie = 1990; prenom = "Astrid"; naissances = 3098 }
      { sexe = "f"; decennie = 1990; prenom = "Josephine"; naissances = 3086 }
      { sexe = "f"; decennie = 1990; prenom = "Yasmine"; naissances = 3049 }
      { sexe = "f"; decennie = 1990; prenom = "Flavie"; naissances = 3041 }
      { sexe = "f"; decennie = 1990; prenom = "Joanna"; naissances = 2994 }
      { sexe = "f"; decennie = 1990; prenom = "Severine"; naissances = 2993 }
      { sexe = "f"; decennie = 2000; prenom = "Léa"; naissances = 77504 }
      { sexe = "f"; decennie = 2000; prenom = "Emma"; naissances = 66921 }
      { sexe = "f"; decennie = 2000; prenom = "Manon"; naissances = 63053 }
      { sexe = "f"; decennie = 2000; prenom = "Chloé"; naissances = 57278 }
      { sexe = "f"; decennie = 2000; prenom = "Camille"; naissances = 53974 }
      { sexe = "f"; decennie = 2000; prenom = "Clara"; naissances = 48824 }
      { sexe = "f"; decennie = 2000; prenom = "Sarah"; naissances = 46925 }
      { sexe = "f"; decennie = 2000; prenom = "Inès"; naissances = 45813 }
      { sexe = "f"; decennie = 2000; prenom = "Océane"; naissances = 41768 }
      { sexe = "f"; decennie = 2000; prenom = "Marie"; naissances = 39894 }
      { sexe = "f"; decennie = 2000; prenom = "Jade"; naissances = 38975 }
      { sexe = "f"; decennie = 2000; prenom = "Anaïs"; naissances = 36079 }
      { sexe = "f"; decennie = 2000; prenom = "Lucie"; naissances = 35912 }
      { sexe = "f"; decennie = 2000; prenom = "Julie"; naissances = 32077 }
      { sexe = "f"; decennie = 2000; prenom = "Mathilde"; naissances = 31806 }
      { sexe = "f"; decennie = 2000; prenom = "Laura"; naissances = 31536 }
      { sexe = "f"; decennie = 2000; prenom = "Eva"; naissances = 30964 }
      { sexe = "f"; decennie = 2000; prenom = "Lola"; naissances = 30435 }
      { sexe = "f"; decennie = 2000; prenom = "Lisa"; naissances = 28868 }
      { sexe = "f"; decennie = 2000; prenom = "Pauline"; naissances = 27927 }
      { sexe = "f"; decennie = 2000; prenom = "Louise"; naissances = 26738 }
      { sexe = "f"; decennie = 2000; prenom = "Juliette"; naissances = 26216 }
      { sexe = "f"; decennie = 2000; prenom = "Romane"; naissances = 26076 }
      { sexe = "f"; decennie = 2000; prenom = "Zoé"; naissances = 25393 }
      { sexe = "f"; decennie = 2000; prenom = "Charlotte"; naissances = 23695 }
      { sexe = "f"; decennie = 2000; prenom = "Célia"; naissances = 23400 }
      { sexe = "f"; decennie = 2000; prenom = "Justine"; naissances = 23367 }
      { sexe = "f"; decennie = 2000; prenom = "Maeva"; naissances = 23363 }
      { sexe = "f"; decennie = 2000; prenom = "Léna"; naissances = 22616 }
      { sexe = "f"; decennie = 2000; prenom = "Marine"; naissances = 22308 }
      { sexe = "f"; decennie = 2000; prenom = "Noemie"; naissances = 21937 }
      { sexe = "f"; decennie = 2000; prenom = "Lilou"; naissances = 21555 }
      { sexe = "f"; decennie = 2000; prenom = "Clémence"; naissances = 21071 }
      { sexe = "f"; decennie = 2000; prenom = "Jeanne"; naissances = 20590 }
      { sexe = "f"; decennie = 2000; prenom = "Emilie"; naissances = 19893 }
      { sexe = "f"; decennie = 2000; prenom = "Maelys"; naissances = 19611 }
      { sexe = "f"; decennie = 2000; prenom = "Elisa"; naissances = 18926 }
      { sexe = "f"; decennie = 2000; prenom = "Lou"; naissances = 18835 }
      { sexe = "f"; decennie = 2000; prenom = "Morgane"; naissances = 18140 }
      { sexe = "f"; decennie = 2000; prenom = "Ambre"; naissances = 17907 }
      { sexe = "f"; decennie = 2000; prenom = "Alice"; naissances = 17843 }
      { sexe = "f"; decennie = 2000; prenom = "Mélissa"; naissances = 17716 }
      { sexe = "f"; decennie = 2000; prenom = "Margaux"; naissances = 17438 }
      { sexe = "f"; decennie = 2000; prenom = "Alicia"; naissances = 17165 }
      { sexe = "f"; decennie = 2000; prenom = "Marion"; naissances = 17091 }
      { sexe = "f"; decennie = 2000; prenom = "Lina"; naissances = 16799 }
      { sexe = "f"; decennie = 2000; prenom = "Elise"; naissances = 15840 }
      { sexe = "f"; decennie = 2000; prenom = "Margot"; naissances = 15761 }
      { sexe = "f"; decennie = 2000; prenom = "Amandine"; naissances = 15738 }
      { sexe = "f"; decennie = 2000; prenom = "Carla"; naissances = 15363 }
      { sexe = "f"; decennie = 2000; prenom = "Maelle"; naissances = 14896 }
      { sexe = "f"; decennie = 2000; prenom = "Louna"; naissances = 14367 }
      { sexe = "f"; decennie = 2000; prenom = "Anna"; naissances = 14002 }
      { sexe = "f"; decennie = 2000; prenom = "Agathe"; naissances = 13434 }
      { sexe = "f"; decennie = 2000; prenom = "Amélie"; naissances = 13047 }
      { sexe = "f"; decennie = 2000; prenom = "Nina"; naissances = 12952 }
      { sexe = "f"; decennie = 2000; prenom = "Elsa"; naissances = 12676 }
      { sexe = "f"; decennie = 2000; prenom = "Julia"; naissances = 12182 }
      { sexe = "f"; decennie = 2000; prenom = "Eloise"; naissances = 11167 }
      { sexe = "f"; decennie = 2000; prenom = "Valentine"; naissances = 11109 }
      { sexe = "f"; decennie = 2000; prenom = "Laurine"; naissances = 10300 }
      { sexe = "f"; decennie = 2000; prenom = "Fanny"; naissances = 10264 }
      { sexe = "f"; decennie = 2000; prenom = "Elodie"; naissances = 10175 }
      { sexe = "f"; decennie = 2000; prenom = "Solène"; naissances = 10149 }
      { sexe = "f"; decennie = 2000; prenom = "Louane"; naissances = 10138 }
      { sexe = "f"; decennie = 2000; prenom = "Yasmine"; naissances = 9959 }
      { sexe = "f"; decennie = 2000; prenom = "Luna"; naissances = 9889 }
      { sexe = "f"; decennie = 2000; prenom = "Anaelle"; naissances = 9842 }
      { sexe = "f"; decennie = 2000; prenom = "Cassandra"; naissances = 9830 }
      { sexe = "f"; decennie = 2000; prenom = "Salomé"; naissances = 9771 }
      { sexe = "f"; decennie = 2000; prenom = "Léonie"; naissances = 9699 }
      { sexe = "f"; decennie = 2000; prenom = "Léane"; naissances = 9497 }
      { sexe = "f"; decennie = 2000; prenom = "Alexia"; naissances = 9154 }
      { sexe = "f"; decennie = 2000; prenom = "Mélanie"; naissances = 9141 }
      { sexe = "f"; decennie = 2000; prenom = "Mélina"; naissances = 8894 }
      { sexe = "f"; decennie = 2000; prenom = "Claire"; naissances = 8834 }
      { sexe = "f"; decennie = 2000; prenom = "Axelle"; naissances = 8795 }
      { sexe = "f"; decennie = 2000; prenom = "Candice"; naissances = 8785 }
      { sexe = "f"; decennie = 2000; prenom = "Coralie"; naissances = 8782 }
      { sexe = "f"; decennie = 2000; prenom = "Victoria"; naissances = 8750 }
      { sexe = "f"; decennie = 2000; prenom = "Lana"; naissances = 8626 }
      { sexe = "f"; decennie = 2000; prenom = "Audrey"; naissances = 8594 }
      { sexe = "f"; decennie = 2000; prenom = "Flavie"; naissances = 8516 }
      { sexe = "f"; decennie = 2000; prenom = "Andréa"; naissances = 8477 }
      { sexe = "f"; decennie = 2000; prenom = "Sara"; naissances = 8339 }
      { sexe = "f"; decennie = 2000; prenom = "Kenza"; naissances = 8227 }
      { sexe = "f"; decennie = 2000; prenom = "Ilona"; naissances = 8192 }
      { sexe = "f"; decennie = 2000; prenom = "Emeline"; naissances = 8144 }
      { sexe = "f"; decennie = 2000; prenom = "Coline"; naissances = 7963 }
      { sexe = "f"; decennie = 2000; prenom = "Cloe"; naissances = 7723 }
      { sexe = "f"; decennie = 2000; prenom = "Lise"; naissances = 7640 }
      { sexe = "f"; decennie = 2000; prenom = "Rose"; naissances = 7607 }
      { sexe = "f"; decennie = 2000; prenom = "Clarisse"; naissances = 7498 }
      { sexe = "f"; decennie = 2000; prenom = "Myriam"; naissances = 7447 }
      { sexe = "f"; decennie = 2000; prenom = "Capucine"; naissances = 7320 }
      { sexe = "f"; decennie = 2000; prenom = "Lily"; naissances = 7269 }
      { sexe = "f"; decennie = 2000; prenom = "Clémentine"; naissances = 7114 }
      { sexe = "f"; decennie = 2000; prenom = "Adèle"; naissances = 7038 }
      { sexe = "f"; decennie = 2000; prenom = "Olivia"; naissances = 7000 }
      { sexe = "f"; decennie = 2000; prenom = "Estelle"; naissances = 6990 }
      { sexe = "f"; decennie = 2000; prenom = "Gabrielle"; naissances = 6971 }
      { sexe = "f"; decennie = 2000; prenom = "Loane"; naissances = 6969 }
      { sexe = "f"; decennie = 2000; prenom = "Héloïse"; naissances = 6832 }
      { sexe = "f"; decennie = 2000; prenom = "Elena"; naissances = 6745 }
      { sexe = "f"; decennie = 2000; prenom = "Sofia"; naissances = 6668 }
      { sexe = "f"; decennie = 2000; prenom = "Alexandra"; naissances = 6467 }
      { sexe = "f"; decennie = 2000; prenom = "Lucile"; naissances = 6398 }
      { sexe = "f"; decennie = 2000; prenom = "Sophie"; naissances = 6358 }
      { sexe = "f"; decennie = 2000; prenom = "Angelina"; naissances = 6116 }
      { sexe = "f"; decennie = 2000; prenom = "Meline"; naissances = 6111 }
      { sexe = "f"; decennie = 2000; prenom = "Faustine"; naissances = 6096 }
      { sexe = "f"; decennie = 2000; prenom = "Charline"; naissances = 5943 }
      { sexe = "f"; decennie = 2000; prenom = "Enora"; naissances = 5876 }
      { sexe = "f"; decennie = 2000; prenom = "Alix"; naissances = 5813 }
      { sexe = "f"; decennie = 2000; prenom = "Caroline"; naissances = 5758 }
      { sexe = "f"; decennie = 2000; prenom = "Marina"; naissances = 5618 }
      { sexe = "f"; decennie = 2000; prenom = "Aurélie"; naissances = 5537 }
      { sexe = "f"; decennie = 2000; prenom = "Ninon"; naissances = 5532 }
      { sexe = "f"; decennie = 2000; prenom = "Elea"; naissances = 5463 }
      { sexe = "f"; decennie = 2000; prenom = "Ophélie"; naissances = 5455 }
      { sexe = "f"; decennie = 2000; prenom = "Jessica"; naissances = 5410 }
      { sexe = "f"; decennie = 2000; prenom = "Assia"; naissances = 5375 }
      { sexe = "f"; decennie = 2000; prenom = "Aurore"; naissances = 5310 }
      { sexe = "f"; decennie = 2000; prenom = "Flora"; naissances = 5273 }
      { sexe = "f"; decennie = 2000; prenom = "Lou-Anne"; naissances = 5254 }
      { sexe = "f"; decennie = 2000; prenom = "Ludivine"; naissances = 5230 }
      { sexe = "f"; decennie = 2000; prenom = "Kelly"; naissances = 5219 }
      { sexe = "f"; decennie = 2000; prenom = "Angèle"; naissances = 5201 }
      { sexe = "f"; decennie = 2000; prenom = "Alizée"; naissances = 5195 }
      { sexe = "f"; decennie = 2000; prenom = "Laure"; naissances = 5170 }
      { sexe = "f"; decennie = 2000; prenom = "Rachel"; naissances = 5079 }
      { sexe = "f"; decennie = 2000; prenom = "Céline"; naissances = 5054 }
      { sexe = "f"; decennie = 2000; prenom = "Anissa"; naissances = 5048 }
      { sexe = "f"; decennie = 2000; prenom = "Constance"; naissances = 5013 }
      { sexe = "f"; decennie = 2000; prenom = "Apolline"; naissances = 4952 }
      { sexe = "f"; decennie = 2000; prenom = "Laetitia"; naissances = 4914 }
      { sexe = "f"; decennie = 2000; prenom = "Mailys"; naissances = 4901 }
      { sexe = "f"; decennie = 2000; prenom = "Leila"; naissances = 4848 }
      { sexe = "f"; decennie = 2000; prenom = "Maya"; naissances = 4839 }
      { sexe = "f"; decennie = 2000; prenom = "Perrine"; naissances = 4784 }
      { sexe = "f"; decennie = 2000; prenom = "Alyssa"; naissances = 4771 }
      { sexe = "f"; decennie = 2000; prenom = "Léana"; naissances = 4741 }
      { sexe = "f"; decennie = 2000; prenom = "Chiara"; naissances = 4709 }
      { sexe = "f"; decennie = 2000; prenom = "Lila"; naissances = 4679 }
      { sexe = "f"; decennie = 2000; prenom = "Imane"; naissances = 4673 }
      { sexe = "f"; decennie = 2000; prenom = "Eleonore"; naissances = 4601 }
      { sexe = "f"; decennie = 2000; prenom = "Victoire"; naissances = 4491 }
      { sexe = "f"; decennie = 2000; prenom = "Lou-Ann"; naissances = 4430 }
      { sexe = "f"; decennie = 2000; prenom = "Thais"; naissances = 4415 }
      { sexe = "f"; decennie = 2000; prenom = "Sabrina"; naissances = 4374 }
      { sexe = "f"; decennie = 2000; prenom = "Cassandre"; naissances = 4337 }
      { sexe = "f"; decennie = 2000; prenom = "Stella"; naissances = 4326 }
      { sexe = "f"; decennie = 2000; prenom = "Laurie"; naissances = 4251 }
      { sexe = "f"; decennie = 2000; prenom = "Orlane"; naissances = 4225 }
      { sexe = "f"; decennie = 2000; prenom = "Aya"; naissances = 4185 }
      { sexe = "f"; decennie = 2000; prenom = "Shana"; naissances = 4175 }
      { sexe = "f"; decennie = 2000; prenom = "Chaima"; naissances = 4171 }
      { sexe = "f"; decennie = 2000; prenom = "Salma"; naissances = 4131 }
      { sexe = "f"; decennie = 2000; prenom = "Cléa"; naissances = 4094 }
      { sexe = "f"; decennie = 2000; prenom = "Emmy"; naissances = 4069 }
      { sexe = "f"; decennie = 2000; prenom = "Garance"; naissances = 4061 }
      { sexe = "f"; decennie = 2000; prenom = "Fiona"; naissances = 4050 }
      { sexe = "f"; decennie = 2000; prenom = "Josephine"; naissances = 4049 }
      { sexe = "f"; decennie = 2000; prenom = "Maud"; naissances = 4023 }
      { sexe = "f"; decennie = 2000; prenom = "Lison"; naissances = 3977 }
      { sexe = "f"; decennie = 2000; prenom = "Enola"; naissances = 3968 }
      { sexe = "f"; decennie = 2000; prenom = "Elina"; naissances = 3923 }
      { sexe = "f"; decennie = 2000; prenom = "Tiphaine"; naissances = 3889 }
      { sexe = "f"; decennie = 2000; prenom = "Selma"; naissances = 3762 }
      { sexe = "f"; decennie = 2000; prenom = "Amel"; naissances = 3747 }
      { sexe = "f"; decennie = 2000; prenom = "Mila"; naissances = 3713 }
      { sexe = "f"; decennie = 2000; prenom = "Cécile"; naissances = 3659 }
      { sexe = "f"; decennie = 2000; prenom = "Lyna"; naissances = 3636 }
      { sexe = "f"; decennie = 2000; prenom = "Oriane"; naissances = 3634 }
      { sexe = "f"; decennie = 2000; prenom = "Nolwenn"; naissances = 3572 }
      { sexe = "f"; decennie = 2000; prenom = "Florine"; naissances = 3569 }
      { sexe = "f"; decennie = 2000; prenom = "Maissa"; naissances = 3568 }
      { sexe = "f"; decennie = 2000; prenom = "Manel"; naissances = 3567 }
      { sexe = "f"; decennie = 2000; prenom = "Gaelle"; naissances = 3566 }
      { sexe = "f"; decennie = 2000; prenom = "Roxane"; naissances = 3551 }
      { sexe = "f"; decennie = 2000; prenom = "Nora"; naissances = 3544 }
      { sexe = "f"; decennie = 2000; prenom = "Diane"; naissances = 3537 }
      { sexe = "f"; decennie = 2000; prenom = "Eve"; naissances = 3535 }
      { sexe = "f"; decennie = 2000; prenom = "Charlène"; naissances = 3502 }
      { sexe = "f"; decennie = 2000; prenom = "Emy"; naissances = 3491 }
      { sexe = "f"; decennie = 2000; prenom = "Noa"; naissances = 3481 }
      { sexe = "f"; decennie = 2000; prenom = "Samantha"; naissances = 3468 }
      { sexe = "f"; decennie = 2000; prenom = "Gwendoline"; naissances = 3462 }
      { sexe = "f"; decennie = 2000; prenom = "Lilia"; naissances = 3437 }
      { sexe = "f"; decennie = 2000; prenom = "Wendy"; naissances = 3427 }
      { sexe = "f"; decennie = 2000; prenom = "Anouk"; naissances = 3385 }
      { sexe = "f"; decennie = 2000; prenom = "Lili"; naissances = 3382 }
      { sexe = "f"; decennie = 2000; prenom = "Amina"; naissances = 3372 }
      { sexe = "f"; decennie = 2000; prenom = "Sirine"; naissances = 3371 }
      { sexe = "f"; decennie = 2000; prenom = "Tess"; naissances = 3365 }
      { sexe = "f"; decennie = 2000; prenom = "Deborah"; naissances = 3292 }
      { sexe = "f"; decennie = 2000; prenom = "Rania"; naissances = 3283 }
      { sexe = "f"; decennie = 2000; prenom = "Hélène"; naissances = 3233 }
      { sexe = "f"; decennie = 2000; prenom = "Tiffany"; naissances = 3223 }
      { sexe = "f"; decennie = 2000; prenom = "Adeline"; naissances = 3215 }
      { sexe = "f"; decennie = 2010; prenom = "Emma"; naissances = 49094 }
      { sexe = "f"; decennie = 2010; prenom = "Jade"; naissances = 42836 }
      { sexe = "f"; decennie = 2010; prenom = "Louise"; naissances = 41654 }
      { sexe = "f"; decennie = 2010; prenom = "Manon"; naissances = 35532 }
      { sexe = "f"; decennie = 2010; prenom = "Chloé"; naissances = 34863 }
      { sexe = "f"; decennie = 2010; prenom = "Lola"; naissances = 32655 }
      { sexe = "f"; decennie = 2010; prenom = "Léa"; naissances = 32341 }
      { sexe = "f"; decennie = 2010; prenom = "Lina"; naissances = 31703 }
      { sexe = "f"; decennie = 2010; prenom = "Camille"; naissances = 30411 }
      { sexe = "f"; decennie = 2010; prenom = "Alice"; naissances = 30157 }
      { sexe = "f"; decennie = 2010; prenom = "Sarah"; naissances = 27098 }
      { sexe = "f"; decennie = 2010; prenom = "Inès"; naissances = 27023 }
      { sexe = "f"; decennie = 2010; prenom = "Zoé"; naissances = 24593 }
      { sexe = "f"; decennie = 2010; prenom = "Léna"; naissances = 23349 }
      { sexe = "f"; decennie = 2010; prenom = "Juliette"; naissances = 23222 }
      { sexe = "f"; decennie = 2010; prenom = "Ambre"; naissances = 23130 }
      { sexe = "f"; decennie = 2010; prenom = "Anna"; naissances = 22340 }
      { sexe = "f"; decennie = 2010; prenom = "Lucie"; naissances = 22243 }
      { sexe = "f"; decennie = 2010; prenom = "Eva"; naissances = 22230 }
      { sexe = "f"; decennie = 2010; prenom = "Romane"; naissances = 21610 }
      { sexe = "f"; decennie = 2010; prenom = "Rose"; naissances = 21516 }
      { sexe = "f"; decennie = 2010; prenom = "Lilou"; naissances = 21329 }
      { sexe = "f"; decennie = 2010; prenom = "Julia"; naissances = 21160 }
      { sexe = "f"; decennie = 2010; prenom = "Louna"; naissances = 20682 }
      { sexe = "f"; decennie = 2010; prenom = "Clara"; naissances = 20541 }
      { sexe = "f"; decennie = 2010; prenom = "Mila"; naissances = 20451 }
      { sexe = "f"; decennie = 2010; prenom = "Jeanne"; naissances = 19749 }
      { sexe = "f"; decennie = 2010; prenom = "Lou"; naissances = 19544 }
      { sexe = "f"; decennie = 2010; prenom = "Nina"; naissances = 18286 }
      { sexe = "f"; decennie = 2010; prenom = "Charlotte"; naissances = 16680 }
      { sexe = "f"; decennie = 2010; prenom = "Inaya"; naissances = 15929 }
      { sexe = "f"; decennie = 2010; prenom = "Margaux"; naissances = 15676 }
      { sexe = "f"; decennie = 2010; prenom = "Agathe"; naissances = 15554 }
      { sexe = "f"; decennie = 2010; prenom = "Léonie"; naissances = 15015 }
      { sexe = "f"; decennie = 2010; prenom = "Lana"; naissances = 14667 }
      { sexe = "f"; decennie = 2010; prenom = "Maëlys"; naissances = 14486 }
      { sexe = "f"; decennie = 2010; prenom = "Anaïs"; naissances = 14483 }
      { sexe = "f"; decennie = 2010; prenom = "Mathilde"; naissances = 14098 }
      { sexe = "f"; decennie = 2010; prenom = "Clémence"; naissances = 14079 }
      { sexe = "f"; decennie = 2010; prenom = "Lisa"; naissances = 13365 }
      { sexe = "f"; decennie = 2010; prenom = "Louane"; naissances = 13312 }
      { sexe = "f"; decennie = 2010; prenom = "Alicia"; naissances = 13238 }
      { sexe = "f"; decennie = 2010; prenom = "Mia"; naissances = 13212 }
      { sexe = "f"; decennie = 2010; prenom = "Sofia"; naissances = 12853 }
      { sexe = "f"; decennie = 2010; prenom = "Margot"; naissances = 12816 }
      { sexe = "f"; decennie = 2010; prenom = "Luna"; naissances = 12594 }
      { sexe = "f"; decennie = 2010; prenom = "Lily"; naissances = 11999 }
      { sexe = "f"; decennie = 2010; prenom = "Marie"; naissances = 11962 }
      { sexe = "f"; decennie = 2010; prenom = "Olivia"; naissances = 11879 }
      { sexe = "f"; decennie = 2010; prenom = "Elsa"; naissances = 11863 }
      { sexe = "f"; decennie = 2010; prenom = "Ines"; naissances = 11762 }
      { sexe = "f"; decennie = 2010; prenom = "Adèle"; naissances = 11485 }
      { sexe = "f"; decennie = 2010; prenom = "Yasmine"; naissances = 11197 }
      { sexe = "f"; decennie = 2010; prenom = "Pauline"; naissances = 11151 }
      { sexe = "f"; decennie = 2010; prenom = "Capucine"; naissances = 11080 }
      { sexe = "f"; decennie = 2010; prenom = "Elise"; naissances = 11054 }
      { sexe = "f"; decennie = 2010; prenom = "Aya"; naissances = 10809 }
      { sexe = "f"; decennie = 2010; prenom = "Elena"; naissances = 10796 }
      { sexe = "f"; decennie = 2010; prenom = "Victoria"; naissances = 10533 }
      { sexe = "f"; decennie = 2010; prenom = "Noémie"; naissances = 10019 }
      { sexe = "f"; decennie = 2010; prenom = "Léana"; naissances = 9994 }
      { sexe = "f"; decennie = 2010; prenom = "Célia"; naissances = 9655 }
      { sexe = "f"; decennie = 2010; prenom = "Elisa"; naissances = 9565 }
      { sexe = "f"; decennie = 2010; prenom = "Giulia"; naissances = 9493 }
      { sexe = "f"; decennie = 2010; prenom = "Laura"; naissances = 9477 }
      { sexe = "f"; decennie = 2010; prenom = "Gabrielle"; naissances = 9214 }
      { sexe = "f"; decennie = 2010; prenom = "Assia"; naissances = 9147 }
      { sexe = "f"; decennie = 2010; prenom = "Océane"; naissances = 9019 }
      { sexe = "f"; decennie = 2010; prenom = "Victoire"; naissances = 8990 }
      { sexe = "f"; decennie = 2010; prenom = "Lise"; naissances = 8939 }
      { sexe = "f"; decennie = 2010; prenom = "Emy"; naissances = 8869 }
      { sexe = "f"; decennie = 2010; prenom = "Valentine"; naissances = 8828 }
      { sexe = "f"; decennie = 2010; prenom = "Julie"; naissances = 8639 }
      { sexe = "f"; decennie = 2010; prenom = "Justine"; naissances = 8628 }
      { sexe = "f"; decennie = 2010; prenom = "Romy"; naissances = 8566 }
      { sexe = "f"; decennie = 2010; prenom = "Lyna"; naissances = 8536 }
      { sexe = "f"; decennie = 2010; prenom = "Nour"; naissances = 8393 }
      { sexe = "f"; decennie = 2010; prenom = "Faustine"; naissances = 8355 }
      { sexe = "f"; decennie = 2010; prenom = "Charlie"; naissances = 8349 }
      { sexe = "f"; decennie = 2010; prenom = "Alix"; naissances = 8237 }
      { sexe = "f"; decennie = 2010; prenom = "Sara"; naissances = 8206 }
      { sexe = "f"; decennie = 2010; prenom = "Lena"; naissances = 8189 }
      { sexe = "f"; decennie = 2010; prenom = "Apolline"; naissances = 8078 }
      { sexe = "f"; decennie = 2010; prenom = "Lya"; naissances = 7993 }
      { sexe = "f"; decennie = 2010; prenom = "Mélina"; naissances = 7884 }
      { sexe = "f"; decennie = 2010; prenom = "Iris"; naissances = 7852 }
      { sexe = "f"; decennie = 2010; prenom = "Maya"; naissances = 7707 }
      { sexe = "f"; decennie = 2010; prenom = "Mya"; naissances = 7399 }
      { sexe = "f"; decennie = 2010; prenom = "Emmy"; naissances = 7298 }
      { sexe = "f"; decennie = 2010; prenom = "Lila"; naissances = 6878 }
      { sexe = "f"; decennie = 2010; prenom = "Soline"; naissances = 6840 }
      { sexe = "f"; decennie = 2010; prenom = "Maëlle"; naissances = 6761 }
      { sexe = "f"; decennie = 2010; prenom = "Lea"; naissances = 6746 }
      { sexe = "f"; decennie = 2010; prenom = "Emilie"; naissances = 6606 }
      { sexe = "f"; decennie = 2010; prenom = "Chloe"; naissances = 6526 }
      { sexe = "f"; decennie = 2010; prenom = "Candice"; naissances = 6479 }
      { sexe = "f"; decennie = 2010; prenom = "Kenza"; naissances = 6356 }
      { sexe = "f"; decennie = 2010; prenom = "Constance"; naissances = 6354 }
      { sexe = "f"; decennie = 2010; prenom = "Salomé"; naissances = 6343 }
      { sexe = "f"; decennie = 2010; prenom = "Eloïse"; naissances = 6320 }
      { sexe = "f"; decennie = 2010; prenom = "Roxane"; naissances = 6319 }
      { sexe = "f"; decennie = 2010; prenom = "Héloïse"; naissances = 6273 }
      { sexe = "f"; decennie = 2010; prenom = "Manel"; naissances = 6242 }
      { sexe = "f"; decennie = 2010; prenom = "Thaïs"; naissances = 6193 }
      { sexe = "f"; decennie = 2010; prenom = "Léane"; naissances = 6192 }
      { sexe = "f"; decennie = 2010; prenom = "Maelys"; naissances = 6175 }
      { sexe = "f"; decennie = 2010; prenom = "Eden"; naissances = 6040 }
      { sexe = "f"; decennie = 2010; prenom = "Zélie"; naissances = 6006 }
      { sexe = "f"; decennie = 2010; prenom = "Enora"; naissances = 5867 }
      { sexe = "f"; decennie = 2010; prenom = "Maria"; naissances = 5691 }
      { sexe = "f"; decennie = 2010; prenom = "Elina"; naissances = 5669 }
      { sexe = "f"; decennie = 2010; prenom = "Livia"; naissances = 5612 }
      { sexe = "f"; decennie = 2010; prenom = "Mélissa"; naissances = 5590 }
      { sexe = "f"; decennie = 2010; prenom = "Théa"; naissances = 5569 }
      { sexe = "f"; decennie = 2010; prenom = "Maryam"; naissances = 5545 }
      { sexe = "f"; decennie = 2010; prenom = "Charline"; naissances = 5493 }
      { sexe = "f"; decennie = 2010; prenom = "Ninon"; naissances = 5449 }
      { sexe = "f"; decennie = 2010; prenom = "Lison"; naissances = 5448 }
      { sexe = "f"; decennie = 2010; prenom = "Eléna"; naissances = 5372 }
      { sexe = "f"; decennie = 2010; prenom = "Marion"; naissances = 5301 }
      { sexe = "f"; decennie = 2010; prenom = "Stella"; naissances = 5185 }
      { sexe = "f"; decennie = 2010; prenom = "Myriam"; naissances = 5173 }
      { sexe = "f"; decennie = 2010; prenom = "Lylou"; naissances = 5129 }
      { sexe = "f"; decennie = 2010; prenom = "Maïssa"; naissances = 5025 }
      { sexe = "f"; decennie = 2010; prenom = "Méline"; naissances = 5007 }
      { sexe = "f"; decennie = 2010; prenom = "Lilia"; naissances = 5006 }
      { sexe = "f"; decennie = 2010; prenom = "Selma"; naissances = 4987 }
      { sexe = "f"; decennie = 2010; prenom = "Amina"; naissances = 4928 }
      { sexe = "f"; decennie = 2010; prenom = "Garance"; naissances = 4906 }
      { sexe = "f"; decennie = 2010; prenom = "Lyana"; naissances = 4898 }
      { sexe = "f"; decennie = 2010; prenom = "Alya"; naissances = 4776 }
      { sexe = "f"; decennie = 2010; prenom = "Louisa"; naissances = 4758 }
      { sexe = "f"; decennie = 2010; prenom = "Nora"; naissances = 4748 }
      { sexe = "f"; decennie = 2010; prenom = "Anaëlle"; naissances = 4730 }
      { sexe = "f"; decennie = 2010; prenom = "Zoe"; naissances = 4670 }
      { sexe = "f"; decennie = 2010; prenom = "Morgane"; naissances = 4620 }
      { sexe = "f"; decennie = 2010; prenom = "Coline"; naissances = 4584 }
      { sexe = "f"; decennie = 2010; prenom = "Sirine"; naissances = 4442 }
      { sexe = "f"; decennie = 2010; prenom = "Diane"; naissances = 4352 }
      { sexe = "f"; decennie = 2010; prenom = "Ilyana"; naissances = 4341 }
      { sexe = "f"; decennie = 2010; prenom = "Andréa"; naissances = 4340 }
      { sexe = "f"; decennie = 2010; prenom = "Alyssa"; naissances = 4307 }
      { sexe = "f"; decennie = 2010; prenom = "Cléa"; naissances = 4193 }
      { sexe = "f"; decennie = 2010; prenom = "Marwa"; naissances = 4188 }
      { sexe = "f"; decennie = 2010; prenom = "Anaé"; naissances = 4111 }
      { sexe = "f"; decennie = 2010; prenom = "Albane"; naissances = 4103 }
      { sexe = "f"; decennie = 2010; prenom = "Hanna"; naissances = 4080 }
      { sexe = "f"; decennie = 2010; prenom = "Ana"; naissances = 4064 }
      { sexe = "f"; decennie = 2010; prenom = "Tess"; naissances = 4005 }
      { sexe = "f"; decennie = 2010; prenom = "Fatima"; naissances = 3987 }
      { sexe = "f"; decennie = 2010; prenom = "Sophia"; naissances = 3977 }
      { sexe = "f"; decennie = 2010; prenom = "Cassandre"; naissances = 3955 }
      { sexe = "f"; decennie = 2010; prenom = "Sasha"; naissances = 3936 }
      { sexe = "f"; decennie = 2010; prenom = "Amandine"; naissances = 3932 }
      { sexe = "f"; decennie = 2010; prenom = "Aliya"; naissances = 3921 }
      { sexe = "f"; decennie = 2010; prenom = "Salma"; naissances = 3908 }
      { sexe = "f"; decennie = 2010; prenom = "Clémentine"; naissances = 3907 }
      { sexe = "f"; decennie = 2010; prenom = "Amélia"; naissances = 3902 }
      { sexe = "f"; decennie = 2010; prenom = "Eline"; naissances = 3895 }
      { sexe = "f"; decennie = 2010; prenom = "Joséphine"; naissances = 3882 }
      { sexe = "f"; decennie = 2010; prenom = "Tessa"; naissances = 3880 }
      { sexe = "f"; decennie = 2010; prenom = "Lily-Rose"; naissances = 3871 }
      { sexe = "f"; decennie = 2010; prenom = "Ava"; naissances = 3863 }
      { sexe = "f"; decennie = 2010; prenom = "Clarisse"; naissances = 3817 }
      { sexe = "f"; decennie = 2010; prenom = "Sophie"; naissances = 3816 }
      { sexe = "f"; decennie = 2010; prenom = "Lucile"; naissances = 3809 }
      { sexe = "f"; decennie = 2010; prenom = "Suzanne"; naissances = 3798 }
      { sexe = "f"; decennie = 2010; prenom = "Jenna"; naissances = 3794 }
      { sexe = "f"; decennie = 2010; prenom = "Amira"; naissances = 3779 }
      { sexe = "f"; decennie = 2010; prenom = "Ella"; naissances = 3739 }
      { sexe = "f"; decennie = 2010; prenom = "Calie"; naissances = 3731 }
      { sexe = "f"; decennie = 2010; prenom = "Axelle"; naissances = 3718 }
      { sexe = "f"; decennie = 2010; prenom = "Asma"; naissances = 3690 }
      { sexe = "f"; decennie = 2010; prenom = "Anouk"; naissances = 3676 }
      { sexe = "f"; decennie = 2010; prenom = "Maëva"; naissances = 3665 }
      { sexe = "f"; decennie = 2010; prenom = "Chiara"; naissances = 3633 }
      { sexe = "f"; decennie = 2010; prenom = "Anais"; naissances = 3632 }
      { sexe = "f"; decennie = 2010; prenom = "Cloé"; naissances = 3632 }
      { sexe = "f"; decennie = 2010; prenom = "Naomie"; naissances = 3601 }
      { sexe = "f"; decennie = 2010; prenom = "Enola"; naissances = 3598 }
      { sexe = "f"; decennie = 2010; prenom = "Shana"; naissances = 3550 }
      { sexe = "f"; decennie = 2010; prenom = "Naomi"; naissances = 3529 }
      { sexe = "f"; decennie = 2010; prenom = "Aliyah"; naissances = 3521 }
      { sexe = "f"; decennie = 2010; prenom = "Dina"; naissances = 3473 }
      { sexe = "f"; decennie = 2010; prenom = "Shanna"; naissances = 3454 }
      { sexe = "f"; decennie = 2010; prenom = "Céleste"; naissances = 3451 }
      { sexe = "f"; decennie = 2010; prenom = "Aïcha"; naissances = 3434 }
      { sexe = "f"; decennie = 2010; prenom = "Leïla"; naissances = 3414 }
      { sexe = "f"; decennie = 2010; prenom = "Elya"; naissances = 3396 }
      { sexe = "f"; decennie = 2010; prenom = "Lara"; naissances = 3386 }
      { sexe = "f"; decennie = 2010; prenom = "Lilya"; naissances = 3383 }
      { sexe = "f"; decennie = 2010; prenom = "Eléonore"; naissances = 3372 }
      { sexe = "f"; decennie = 2010; prenom = "Eléa"; naissances = 3367 }
      { sexe = "f"; decennie = 2010; prenom = "Maeva"; naissances = 3342 }
      { sexe = "f"; decennie = 2010; prenom = "Jasmine"; naissances = 3320 }
      { sexe = "f"; decennie = 2010; prenom = "Mariam"; naissances = 3318 }
      { sexe = "f"; decennie = 2010; prenom = "Khadija"; naissances = 3317 }
      { sexe = "f"; decennie = 2010; prenom = "Rachel"; naissances = 3296 }
      { sexe = "f"; decennie = 2010; prenom = "Leyna"; naissances = 3280 }
      { sexe = "f"; decennie = 2010; prenom = "Mayssa"; naissances = 3247 }
      { sexe = "f"; decennie = 2020; prenom = "Jade"; naissances = 7618 }
      { sexe = "f"; decennie = 2020; prenom = "Louise"; naissances = 7577 }
      { sexe = "f"; decennie = 2020; prenom = "Emma"; naissances = 6685 }
      { sexe = "f"; decennie = 2020; prenom = "Ambre"; naissances = 5764 }
      { sexe = "f"; decennie = 2020; prenom = "Alice"; naissances = 5755 }
      { sexe = "f"; decennie = 2020; prenom = "Rose"; naissances = 5367 }
      { sexe = "f"; decennie = 2020; prenom = "Lina"; naissances = 5110 }
      { sexe = "f"; decennie = 2020; prenom = "Anna"; naissances = 4893 }
      { sexe = "f"; decennie = 2020; prenom = "Mia"; naissances = 4891 }
      { sexe = "f"; decennie = 2020; prenom = "Chloé"; naissances = 4785 }
      { sexe = "f"; decennie = 2020; prenom = "Romy"; naissances = 4647 }
      { sexe = "f"; decennie = 2020; prenom = "Léa"; naissances = 4471 }
      { sexe = "f"; decennie = 2020; prenom = "Julia"; naissances = 4470 }
      { sexe = "f"; decennie = 2020; prenom = "Lou"; naissances = 4375 }
      { sexe = "f"; decennie = 2020; prenom = "Mila"; naissances = 4028 }
      { sexe = "f"; decennie = 2020; prenom = "Léna"; naissances = 4026 }
      { sexe = "f"; decennie = 2020; prenom = "Agathe"; naissances = 3945 }
      { sexe = "f"; decennie = 2020; prenom = "Juliette"; naissances = 3730 }
      { sexe = "f"; decennie = 2020; prenom = "Nina"; naissances = 3721 }
      { sexe = "f"; decennie = 2020; prenom = "Inaya"; naissances = 3712 }
      { sexe = "f"; decennie = 2020; prenom = "Iris"; naissances = 3681 }
      { sexe = "f"; decennie = 2020; prenom = "Zoé"; naissances = 3598 }
      { sexe = "f"; decennie = 2020; prenom = "Inès"; naissances = 3535 }
      { sexe = "f"; decennie = 2020; prenom = "Alba"; naissances = 3491 }
      { sexe = "f"; decennie = 2020; prenom = "Jeanne"; naissances = 3458 }
      { sexe = "f"; decennie = 2020; prenom = "Léonie"; naissances = 3456 }
      { sexe = "f"; decennie = 2020; prenom = "Charlie"; naissances = 3372 }
      { sexe = "f"; decennie = 2020; prenom = "Eva"; naissances = 3363 }
      { sexe = "f"; decennie = 2020; prenom = "Adèle"; naissances = 3220 }
      { sexe = "f"; decennie = 2020; prenom = "Luna"; naissances = 3215 }
      { sexe = "f"; decennie = 2020; prenom = "Victoire"; naissances = 3195 }
      { sexe = "f"; decennie = 2020; prenom = "Lola"; naissances = 3180 }
      { sexe = "f"; decennie = 2020; prenom = "Olivia"; naissances = 3034 }
      { sexe = "f"; decennie = 2020; prenom = "Lucie"; naissances = 2974 }
      { sexe = "f"; decennie = 2020; prenom = "Romane"; naissances = 2972 }
      { sexe = "f"; decennie = 2020; prenom = "Camille"; naissances = 2952 }
      { sexe = "f"; decennie = 2020; prenom = "Margaux"; naissances = 2945 }
      { sexe = "f"; decennie = 2020; prenom = "Victoria"; naissances = 2943 }
      { sexe = "f"; decennie = 2020; prenom = "Alix"; naissances = 2750 }
      { sexe = "f"; decennie = 2020; prenom = "Sofia"; naissances = 2732 }
      { sexe = "f"; decennie = 2020; prenom = "Giulia"; naissances = 2721 }
      { sexe = "f"; decennie = 2020; prenom = "Manon"; naissances = 2711 }
      { sexe = "f"; decennie = 2020; prenom = "Charlotte"; naissances = 2671 }
      { sexe = "f"; decennie = 2020; prenom = "Louna"; naissances = 2605 }
      { sexe = "f"; decennie = 2020; prenom = "Mya"; naissances = 2544 }
      { sexe = "f"; decennie = 2020; prenom = "Nour"; naissances = 2521 }
      { sexe = "f"; decennie = 2020; prenom = "Sarah"; naissances = 2521 }
      { sexe = "f"; decennie = 2020; prenom = "Margot"; naissances = 2479 }
      { sexe = "f"; decennie = 2020; prenom = "Lya"; naissances = 2418 }
      { sexe = "f"; decennie = 2020; prenom = "Lyana"; naissances = 2388 }
      { sexe = "f"; decennie = 2020; prenom = "Capucine"; naissances = 2195 }
      { sexe = "f"; decennie = 2020; prenom = "Emy"; naissances = 2138 }
      { sexe = "f"; decennie = 2020; prenom = "Théa"; naissances = 2109 }
      { sexe = "f"; decennie = 2020; prenom = "Clémence"; naissances = 2103 }
      { sexe = "f"; decennie = 2020; prenom = "Elena"; naissances = 2086 }
      { sexe = "f"; decennie = 2020; prenom = "Alya"; naissances = 1923 }
      { sexe = "f"; decennie = 2020; prenom = "Aya"; naissances = 1907 }
      { sexe = "f"; decennie = 2020; prenom = "Clara"; naissances = 1899 }
      { sexe = "f"; decennie = 2020; prenom = "Lyna"; naissances = 1865 }
      { sexe = "f"; decennie = 2020; prenom = "Ava"; naissances = 1810 }
      { sexe = "f"; decennie = 2020; prenom = "Gabrielle"; naissances = 1807 }
      { sexe = "f"; decennie = 2020; prenom = "Lana"; naissances = 1800 }
      { sexe = "f"; decennie = 2020; prenom = "Yasmine"; naissances = 1790 }
      { sexe = "f"; decennie = 2020; prenom = "Alma"; naissances = 1776 }
      { sexe = "f"; decennie = 2020; prenom = "Lily"; naissances = 1642 }
      { sexe = "f"; decennie = 2020; prenom = "Zélie"; naissances = 1622 }
      { sexe = "f"; decennie = 2020; prenom = "Roxane"; naissances = 1613 }
      { sexe = "f"; decennie = 2020; prenom = "Livia"; naissances = 1600 }
      { sexe = "f"; decennie = 2020; prenom = "Apolline"; naissances = 1592 }
      { sexe = "f"; decennie = 2020; prenom = "Lise"; naissances = 1571 }
      { sexe = "f"; decennie = 2020; prenom = "Alicia"; naissances = 1564 }
      { sexe = "f"; decennie = 2020; prenom = "Assia"; naissances = 1561 }
      { sexe = "f"; decennie = 2020; prenom = "Lila"; naissances = 1560 }
      { sexe = "f"; decennie = 2020; prenom = "Maëlys"; naissances = 1544 }
      { sexe = "f"; decennie = 2020; prenom = "Maya"; naissances = 1526 }
      { sexe = "f"; decennie = 2020; prenom = "Mathilde"; naissances = 1492 }
      { sexe = "f"; decennie = 2020; prenom = "Ella"; naissances = 1482 }
      { sexe = "f"; decennie = 2020; prenom = "Maria"; naissances = 1460 }
      { sexe = "f"; decennie = 2020; prenom = "Thaïs"; naissances = 1458 }
      { sexe = "f"; decennie = 2020; prenom = "Joy"; naissances = 1457 }
      { sexe = "f"; decennie = 2020; prenom = "Valentine"; naissances = 1443 }
      { sexe = "f"; decennie = 2020; prenom = "Anaïs"; naissances = 1408 }
      { sexe = "f"; decennie = 2020; prenom = "Céleste"; naissances = 1406 }
      { sexe = "f"; decennie = 2020; prenom = "Albane"; naissances = 1404 }
      { sexe = "f"; decennie = 2020; prenom = "Léana"; naissances = 1396 }
      { sexe = "f"; decennie = 2020; prenom = "Héloïse"; naissances = 1395 }
      { sexe = "f"; decennie = 2020; prenom = "Lilou"; naissances = 1378 }
      { sexe = "f"; decennie = 2020; prenom = "Constance"; naissances = 1376 }
      { sexe = "f"; decennie = 2020; prenom = "Amélia"; naissances = 1330 }
      { sexe = "f"; decennie = 2020; prenom = "Maëlle"; naissances = 1317 }
      { sexe = "f"; decennie = 2020; prenom = "Salomé"; naissances = 1307 }
      { sexe = "f"; decennie = 2020; prenom = "Marie"; naissances = 1298 }
      { sexe = "f"; decennie = 2020; prenom = "Elise"; naissances = 1291 }
      { sexe = "f"; decennie = 2020; prenom = "Célia"; naissances = 1283 }
      { sexe = "f"; decennie = 2020; prenom = "Emmy"; naissances = 1244 }
      { sexe = "f"; decennie = 2020; prenom = "Noémie"; naissances = 1235 }
      { sexe = "f"; decennie = 2020; prenom = "Diane"; naissances = 1212 }
      { sexe = "f"; decennie = 2020; prenom = "Lisa"; naissances = 1208 }
      { sexe = "f"; decennie = 2020; prenom = "Elsa"; naissances = 1183 }
      { sexe = "f"; decennie = 2020; prenom = "Soline"; naissances = 1176 }
      { sexe = "f"; decennie = 2020; prenom = "Nora"; naissances = 1152 }
      { sexe = "f"; decennie = 2020; prenom = "Garance"; naissances = 1145 }
      { sexe = "f"; decennie = 2020; prenom = "Joséphine"; naissances = 1144 }
      { sexe = "f"; decennie = 2020; prenom = "Faustine"; naissances = 1113 }
      { sexe = "f"; decennie = 2020; prenom = "Nelya"; naissances = 1096 }
      { sexe = "f"; decennie = 2020; prenom = "Mélina"; naissances = 1091 }
      { sexe = "f"; decennie = 2020; prenom = "Andréa"; naissances = 1083 }
      { sexe = "f"; decennie = 2020; prenom = "Suzanne"; naissances = 1081 }
      { sexe = "f"; decennie = 2020; prenom = "Ellie"; naissances = 1039 }
      { sexe = "f"; decennie = 2020; prenom = "Aria"; naissances = 1035 }
      { sexe = "f"; decennie = 2020; prenom = "Tessa"; naissances = 1035 }
      { sexe = "f"; decennie = 2020; prenom = "Maryam"; naissances = 1028 }
      { sexe = "f"; decennie = 2020; prenom = "Louisa"; naissances = 1027 }
      { sexe = "f"; decennie = 2020; prenom = "Aïcha"; naissances = 1026 }
      { sexe = "f"; decennie = 2020; prenom = "Arya"; naissances = 1025 }
      { sexe = "f"; decennie = 2020; prenom = "Eléna"; naissances = 1020 }
      { sexe = "f"; decennie = 2020; prenom = "Amira"; naissances = 1016 }
      { sexe = "f"; decennie = 2020; prenom = "Aliyah"; naissances = 1009 }
      { sexe = "f"; decennie = 2020; prenom = "Hanna"; naissances = 1008 }
      { sexe = "f"; decennie = 2020; prenom = "Manel"; naissances = 1007 }
      { sexe = "f"; decennie = 2020; prenom = "Jennah"; naissances = 1004 }
      { sexe = "f"; decennie = 2020; prenom = "Sara"; naissances = 973 }
      { sexe = "f"; decennie = 2020; prenom = "Candice"; naissances = 957 }
      { sexe = "f"; decennie = 2020; prenom = "Maïssa"; naissances = 953 }
      { sexe = "f"; decennie = 2020; prenom = "Ilyana"; naissances = 952 }
      { sexe = "f"; decennie = 2020; prenom = "Sophia"; naissances = 934 }
      { sexe = "f"; decennie = 2020; prenom = "Judith"; naissances = 912 }
      { sexe = "f"; decennie = 2020; prenom = "Fatima"; naissances = 907 }
      { sexe = "f"; decennie = 2020; prenom = "Élise"; naissances = 906 }
      { sexe = "f"; decennie = 2020; prenom = "Hana"; naissances = 906 }
      { sexe = "f"; decennie = 2020; prenom = "Maddy"; naissances = 905 }
      { sexe = "f"; decennie = 2020; prenom = "Lison"; naissances = 895 }
      { sexe = "f"; decennie = 2020; prenom = "Océane"; naissances = 895 }
      { sexe = "f"; decennie = 2020; prenom = "Emna"; naissances = 889 }
      { sexe = "f"; decennie = 2020; prenom = "Eloïse"; naissances = 886 }
      { sexe = "f"; decennie = 2020; prenom = "Valentina"; naissances = 886 }
      { sexe = "f"; decennie = 2020; prenom = "Amina"; naissances = 884 }
      { sexe = "f"; decennie = 2020; prenom = "Elya"; naissances = 876 }
      { sexe = "f"; decennie = 2020; prenom = "Naya"; naissances = 876 }
      { sexe = "f"; decennie = 2020; prenom = "Cataleya"; naissances = 870 }
      { sexe = "f"; decennie = 2020; prenom = "Elisa"; naissances = 867 }
      { sexe = "f"; decennie = 2020; prenom = "Jenna"; naissances = 864 }
      { sexe = "f"; decennie = 2020; prenom = "Anaëlle"; naissances = 861 }
      { sexe = "f"; decennie = 2020; prenom = "Anaé"; naissances = 857 }
      { sexe = "f"; decennie = 2020; prenom = "Elina"; naissances = 847 }
      { sexe = "f"; decennie = 2020; prenom = "Mariam"; naissances = 846 }
      { sexe = "f"; decennie = 2020; prenom = "Jasmine"; naissances = 845 }
      { sexe = "f"; decennie = 2020; prenom = "Ana"; naissances = 843 }
      { sexe = "f"; decennie = 2020; prenom = "Aliya"; naissances = 832 }
      { sexe = "f"; decennie = 2020; prenom = "Liya"; naissances = 830 }
      { sexe = "f"; decennie = 2020; prenom = "Jana"; naissances = 828 }
      { sexe = "f"; decennie = 2020; prenom = "Stella"; naissances = 813 }
      { sexe = "f"; decennie = 2020; prenom = "Camélia"; naissances = 807 }
      { sexe = "f"; decennie = 2020; prenom = "Pauline"; naissances = 807 }
      { sexe = "f"; decennie = 2020; prenom = "Lara"; naissances = 806 }
      { sexe = "f"; decennie = 2020; prenom = "Kiara"; naissances = 799 }
      { sexe = "f"; decennie = 2020; prenom = "Lilia"; naissances = 798 }
      { sexe = "f"; decennie = 2020; prenom = "Ninon"; naissances = 794 }
      { sexe = "f"; decennie = 2020; prenom = "Fatoumata"; naissances = 789 }
      { sexe = "f"; decennie = 2020; prenom = "Ines"; naissances = 785 }
      { sexe = "f"; decennie = 2020; prenom = "Charline"; naissances = 771 }
      { sexe = "f"; decennie = 2020; prenom = "Selma"; naissances = 769 }
      { sexe = "f"; decennie = 2020; prenom = "Coline"; naissances = 768 }
      { sexe = "f"; decennie = 2020; prenom = "Eline"; naissances = 768 }
      { sexe = "f"; decennie = 2020; prenom = "Calie"; naissances = 764 }
      { sexe = "f"; decennie = 2020; prenom = "Lilya"; naissances = 759 }
      { sexe = "f"; decennie = 2020; prenom = "Louane"; naissances = 745 }
      { sexe = "f"; decennie = 2020; prenom = "Laura"; naissances = 742 }
      { sexe = "f"; decennie = 2020; prenom = "Naomi"; naissances = 731 }
      { sexe = "f"; decennie = 2020; prenom = "Jannah"; naissances = 728 }
      { sexe = "f"; decennie = 2020; prenom = "Cléa"; naissances = 726 }
      { sexe = "f"; decennie = 2020; prenom = "Leyna"; naissances = 725 }
      { sexe = "f"; decennie = 2020; prenom = "Célestine"; naissances = 724 }
      { sexe = "f"; decennie = 2020; prenom = "Myla"; naissances = 724 }
      { sexe = "f"; decennie = 2020; prenom = "Dina"; naissances = 722 }
      { sexe = "f"; decennie = 2020; prenom = "Nélia"; naissances = 711 }
      { sexe = "f"; decennie = 2020; prenom = "Hortense"; naissances = 710 }
      { sexe = "f"; decennie = 2020; prenom = "Neyla"; naissances = 710 }
      { sexe = "f"; decennie = 2020; prenom = "Layana"; naissances = 709 }
      { sexe = "f"; decennie = 2020; prenom = "Eden"; naissances = 707 }
      { sexe = "f"; decennie = 2020; prenom = "Éléna"; naissances = 704 }
      { sexe = "f"; decennie = 2020; prenom = "Éléonore"; naissances = 702 }
      { sexe = "f"; decennie = 2020; prenom = "Kenza"; naissances = 699 }
      { sexe = "f"; decennie = 2020; prenom = "Louison"; naissances = 699 }
      { sexe = "f"; decennie = 2020; prenom = "Khadija"; naissances = 695 }
      { sexe = "f"; decennie = 2020; prenom = "Nélya"; naissances = 687 }
      { sexe = "f"; decennie = 2020; prenom = "Billie"; naissances = 681 }
      { sexe = "f"; decennie = 2020; prenom = "Esmée"; naissances = 673 }
      { sexe = "f"; decennie = 2020; prenom = "Leïla"; naissances = 670 }
      { sexe = "f"; decennie = 2020; prenom = "Augustine"; naissances = 665 }
      { sexe = "f"; decennie = 2020; prenom = "Norah"; naissances = 656 }
      { sexe = "f"; decennie = 2020; prenom = "Eléonore"; naissances = 655 }
      { sexe = "f"; decennie = 2020; prenom = "Ayline"; naissances = 651 }
      { sexe = "f"; decennie = 2020; prenom = "Janna"; naissances = 651 }
      { sexe = "f"; decennie = 2020; prenom = "Justine"; naissances = 647 }
      { sexe = "f"; decennie = 2020; prenom = "Myriam"; naissances = 642 }
      { sexe = "f"; decennie = 2020; prenom = "Sasha"; naissances = 639 }
      { sexe = "f"; decennie = 2020; prenom = "Aminata"; naissances = 638 }
      { sexe = "f"; decennie = 2020; prenom = "Julie"; naissances = 638 }
      { sexe = "f"; decennie = 2020; prenom = "Safiya"; naissances = 638 }
      { sexe = "m"; decennie = 1900; prenom = "Jean"; naissances = 174656 }
      { sexe = "m"; decennie = 1900; prenom = "Louis"; naissances = 104042 }
      { sexe = "m"; decennie = 1900; prenom = "Pierre"; naissances = 94205 }
      { sexe = "m"; decennie = 1900; prenom = "Marcel"; naissances = 90550 }
      { sexe = "m"; decennie = 1900; prenom = "Henri"; naissances = 82962 }
      { sexe = "m"; decennie = 1900; prenom = "André"; naissances = 81953 }
      { sexe = "m"; decennie = 1900; prenom = "Joseph"; naissances = 80809 }
      { sexe = "m"; decennie = 1900; prenom = "René"; naissances = 69339 }
      { sexe = "m"; decennie = 1900; prenom = "Georges"; naissances = 68403 }
      { sexe = "m"; decennie = 1900; prenom = "Paul"; naissances = 59198 }
      { sexe = "m"; decennie = 1900; prenom = "Maurice"; naissances = 54219 }
      { sexe = "m"; decennie = 1900; prenom = "Charles"; naissances = 49101 }
      { sexe = "m"; decennie = 1900; prenom = "François"; naissances = 46859 }
      { sexe = "m"; decennie = 1900; prenom = "Emile"; naissances = 46473 }
      { sexe = "m"; decennie = 1900; prenom = "Albert"; naissances = 42731 }
      { sexe = "m"; decennie = 1900; prenom = "Robert"; naissances = 40323 }
      { sexe = "m"; decennie = 1900; prenom = "Roger"; naissances = 39774 }
      { sexe = "m"; decennie = 1900; prenom = "Lucien"; naissances = 37956 }
      { sexe = "m"; decennie = 1900; prenom = "Léon"; naissances = 34588 }
      { sexe = "m"; decennie = 1900; prenom = "Eugène"; naissances = 34432 }
      { sexe = "m"; decennie = 1900; prenom = "Raymond"; naissances = 33168 }
      { sexe = "m"; decennie = 1900; prenom = "Auguste"; naissances = 28180 }
      { sexe = "m"; decennie = 1900; prenom = "Fernand"; naissances = 27654 }
      { sexe = "m"; decennie = 1900; prenom = "Jules"; naissances = 26906 }
      { sexe = "m"; decennie = 1900; prenom = "Gaston"; naissances = 25708 }
      { sexe = "m"; decennie = 1900; prenom = "Antoine"; naissances = 21270 }
      { sexe = "m"; decennie = 1900; prenom = "Marius"; naissances = 18676 }
      { sexe = "m"; decennie = 1900; prenom = "Alfred"; naissances = 17784 }
      { sexe = "m"; decennie = 1900; prenom = "Ernest"; naissances = 16960 }
      { sexe = "m"; decennie = 1900; prenom = "Victor"; naissances = 16818 }
      { sexe = "m"; decennie = 1900; prenom = "Gabriel"; naissances = 16560 }
      { sexe = "m"; decennie = 1900; prenom = "Julien"; naissances = 15080 }
      { sexe = "m"; decennie = 1900; prenom = "Jacques"; naissances = 14706 }
      { sexe = "m"; decennie = 1900; prenom = "Edouard"; naissances = 13917 }
      { sexe = "m"; decennie = 1900; prenom = "Camille"; naissances = 13853 }
      { sexe = "m"; decennie = 1900; prenom = "Alphonse"; naissances = 13660 }
      { sexe = "m"; decennie = 1900; prenom = "Edmond"; naissances = 11806 }
      { sexe = "m"; decennie = 1900; prenom = "Gustave"; naissances = 11185 }
      { sexe = "m"; decennie = 1900; prenom = "Armand"; naissances = 11179 }
      { sexe = "m"; decennie = 1900; prenom = "Alexandre"; naissances = 10667 }
      { sexe = "m"; decennie = 1900; prenom = "Michel"; naissances = 9972 }
      { sexe = "m"; decennie = 1900; prenom = "Adrien"; naissances = 9906 }
      { sexe = "m"; decennie = 1900; prenom = "Etienne"; naissances = 9830 }
      { sexe = "m"; decennie = 1900; prenom = "Marie"; naissances = 9642 }
      { sexe = "m"; decennie = 1900; prenom = "Félix"; naissances = 9260 }
      { sexe = "m"; decennie = 1900; prenom = "Yves"; naissances = 9045 }
      { sexe = "m"; decennie = 1900; prenom = "Raoul"; naissances = 7703 }
      { sexe = "m"; decennie = 1900; prenom = "Elie"; naissances = 7661 }
      { sexe = "m"; decennie = 1900; prenom = "Aime"; naissances = 7308 }
      { sexe = "m"; decennie = 1900; prenom = "Claude"; naissances = 7278 }
      { sexe = "m"; decennie = 1900; prenom = "Arthur"; naissances = 6649 }
      { sexe = "m"; decennie = 1900; prenom = "Bernard"; naissances = 6572 }
      { sexe = "m"; decennie = 1900; prenom = "Germain"; naissances = 6385 }
      { sexe = "m"; decennie = 1900; prenom = "Gilbert"; naissances = 6240 }
      { sexe = "m"; decennie = 1900; prenom = "Clément"; naissances = 6198 }
      { sexe = "m"; decennie = 1900; prenom = "Francis"; naissances = 6011 }
      { sexe = "m"; decennie = 1900; prenom = "Frédéric"; naissances = 5853 }
      { sexe = "m"; decennie = 1900; prenom = "Adolphe"; naissances = 5594 }
      { sexe = "m"; decennie = 1900; prenom = "Augustin"; naissances = 5588 }
      { sexe = "m"; decennie = 1900; prenom = "Guillaume"; naissances = 5545 }
      { sexe = "m"; decennie = 1900; prenom = "Désiré"; naissances = 5500 }
      { sexe = "m"; decennie = 1900; prenom = "Daniel"; naissances = 5119 }
      { sexe = "m"; decennie = 1900; prenom = "Abel"; naissances = 5092 }
      { sexe = "m"; decennie = 1900; prenom = "Marc"; naissances = 4913 }
      { sexe = "m"; decennie = 1900; prenom = "Ferdinand"; naissances = 4549 }
      { sexe = "m"; decennie = 1900; prenom = "Laurent"; naissances = 4317 }
      { sexe = "m"; decennie = 1900; prenom = "Noël"; naissances = 4168 }
      { sexe = "m"; decennie = 1900; prenom = "Alexis"; naissances = 4164 }
      { sexe = "m"; decennie = 1900; prenom = "Antonin"; naissances = 4033 }
      { sexe = "m"; decennie = 1900; prenom = "Marceau"; naissances = 3944 }
      { sexe = "m"; decennie = 1900; prenom = "Philippe"; naissances = 3881 }
      { sexe = "m"; decennie = 1900; prenom = "Maxime"; naissances = 3875 }
      { sexe = "m"; decennie = 1900; prenom = "Roland"; naissances = 3769 }
      { sexe = "m"; decennie = 1900; prenom = "Constant"; naissances = 3714 }
      { sexe = "m"; decennie = 1900; prenom = "Leopold"; naissances = 3649 }
      { sexe = "m"; decennie = 1900; prenom = "Nicolas"; naissances = 3633 }
      { sexe = "m"; decennie = 1900; prenom = "Claudius"; naissances = 3425 }
      { sexe = "m"; decennie = 1900; prenom = "Emmanuel"; naissances = 3360 }
      { sexe = "m"; decennie = 1900; prenom = "Celestin"; naissances = 3222 }
      { sexe = "m"; decennie = 1900; prenom = "Justin"; naissances = 3107 }
      { sexe = "m"; decennie = 1900; prenom = "Jean-Baptiste"; naissances = 3053 }
      { sexe = "m"; decennie = 1900; prenom = "Theophile"; naissances = 3053 }
      { sexe = "m"; decennie = 1900; prenom = "Vincent"; naissances = 2975 }
      { sexe = "m"; decennie = 1900; prenom = "Dominique"; naissances = 2959 }
      { sexe = "m"; decennie = 1900; prenom = "Octave"; naissances = 2946 }
      { sexe = "m"; decennie = 1910; prenom = "Jean"; naissances = 175346 }
      { sexe = "m"; decennie = 1910; prenom = "André"; naissances = 103775 }
      { sexe = "m"; decennie = 1910; prenom = "Pierre"; naissances = 95573 }
      { sexe = "m"; decennie = 1910; prenom = "Marcel"; naissances = 90674 }
      { sexe = "m"; decennie = 1910; prenom = "Louis"; naissances = 86632 }
      { sexe = "m"; decennie = 1910; prenom = "René"; naissances = 81307 }
      { sexe = "m"; decennie = 1910; prenom = "Henri"; naissances = 75607 }
      { sexe = "m"; decennie = 1910; prenom = "Georges"; naissances = 67239 }
      { sexe = "m"; decennie = 1910; prenom = "Joseph"; naissances = 66398 }
      { sexe = "m"; decennie = 1910; prenom = "Roger"; naissances = 62437 }
      { sexe = "m"; decennie = 1910; prenom = "Maurice"; naissances = 59512 }
      { sexe = "m"; decennie = 1910; prenom = "Paul"; naissances = 56072 }
      { sexe = "m"; decennie = 1910; prenom = "Robert"; naissances = 55058 }
      { sexe = "m"; decennie = 1910; prenom = "Raymond"; naissances = 46677 }
      { sexe = "m"; decennie = 1910; prenom = "Albert"; naissances = 45268 }
      { sexe = "m"; decennie = 1910; prenom = "Charles"; naissances = 40190 }
      { sexe = "m"; decennie = 1910; prenom = "Lucien"; naissances = 38752 }
      { sexe = "m"; decennie = 1910; prenom = "François"; naissances = 36760 }
      { sexe = "m"; decennie = 1910; prenom = "Emile"; naissances = 36619 }
      { sexe = "m"; decennie = 1910; prenom = "Léon"; naissances = 26208 }
      { sexe = "m"; decennie = 1910; prenom = "Fernand"; naissances = 25839 }
      { sexe = "m"; decennie = 1910; prenom = "Eugène"; naissances = 25540 }
      { sexe = "m"; decennie = 1910; prenom = "Jacques"; naissances = 21664 }
      { sexe = "m"; decennie = 1910; prenom = "Gaston"; naissances = 21304 }
      { sexe = "m"; decennie = 1910; prenom = "Auguste"; naissances = 19885 }
      { sexe = "m"; decennie = 1910; prenom = "Jules"; naissances = 17909 }
      { sexe = "m"; decennie = 1910; prenom = "Antoine"; naissances = 16821 }
      { sexe = "m"; decennie = 1910; prenom = "Gabriel"; naissances = 15068 }
      { sexe = "m"; decennie = 1910; prenom = "Alfred"; naissances = 14537 }
      { sexe = "m"; decennie = 1910; prenom = "Marius"; naissances = 14295 }
      { sexe = "m"; decennie = 1910; prenom = "Michel"; naissances = 13518 }
      { sexe = "m"; decennie = 1910; prenom = "Julien"; naissances = 12752 }
      { sexe = "m"; decennie = 1910; prenom = "Victor"; naissances = 12723 }
      { sexe = "m"; decennie = 1910; prenom = "Ernest"; naissances = 12564 }
      { sexe = "m"; decennie = 1910; prenom = "Camille"; naissances = 11632 }
      { sexe = "m"; decennie = 1910; prenom = "Yves"; naissances = 11165 }
      { sexe = "m"; decennie = 1910; prenom = "Edouard"; naissances = 11111 }
      { sexe = "m"; decennie = 1910; prenom = "Bernard"; naissances = 10594 }
      { sexe = "m"; decennie = 1910; prenom = "Alphonse"; naissances = 10409 }
      { sexe = "m"; decennie = 1910; prenom = "Armand"; naissances = 10364 }
      { sexe = "m"; decennie = 1910; prenom = "Edmond"; naissances = 10261 }
      { sexe = "m"; decennie = 1910; prenom = "Gilbert"; naissances = 10092 }
      { sexe = "m"; decennie = 1910; prenom = "Gustave"; naissances = 8204 }
      { sexe = "m"; decennie = 1910; prenom = "Adrien"; naissances = 8096 }
      { sexe = "m"; decennie = 1910; prenom = "Etienne"; naissances = 8093 }
      { sexe = "m"; decennie = 1910; prenom = "Alexandre"; naissances = 8061 }
      { sexe = "m"; decennie = 1910; prenom = "Roland"; naissances = 7383 }
      { sexe = "m"; decennie = 1910; prenom = "Aime"; naissances = 7380 }
      { sexe = "m"; decennie = 1910; prenom = "Francis"; naissances = 7314 }
      { sexe = "m"; decennie = 1910; prenom = "Félix"; naissances = 7038 }
      { sexe = "m"; decennie = 1910; prenom = "Claude"; naissances = 6842 }
      { sexe = "m"; decennie = 1910; prenom = "Guy"; naissances = 6524 }
      { sexe = "m"; decennie = 1910; prenom = "Raoul"; naissances = 6386 }
      { sexe = "m"; decennie = 1910; prenom = "Elie"; naissances = 6257 }
      { sexe = "m"; decennie = 1910; prenom = "Daniel"; naissances = 6193 }
      { sexe = "m"; decennie = 1910; prenom = "Marie"; naissances = 5980 }
      { sexe = "m"; decennie = 1910; prenom = "Marc"; naissances = 5702 }
      { sexe = "m"; decennie = 1910; prenom = "Germain"; naissances = 5589 }
      { sexe = "m"; decennie = 1910; prenom = "Arthur"; naissances = 5049 }
      { sexe = "m"; decennie = 1910; prenom = "Clément"; naissances = 4963 }
      { sexe = "m"; decennie = 1910; prenom = "Gérard"; naissances = 4587 }
      { sexe = "m"; decennie = 1910; prenom = "Désiré"; naissances = 4331 }
      { sexe = "m"; decennie = 1910; prenom = "Noël"; naissances = 4267 }
      { sexe = "m"; decennie = 1910; prenom = "Augustin"; naissances = 4141 }
      { sexe = "m"; decennie = 1910; prenom = "Frédéric"; naissances = 4099 }
      { sexe = "m"; decennie = 1910; prenom = "Adolphe"; naissances = 4047 }
      { sexe = "m"; decennie = 1910; prenom = "Abel"; naissances = 3879 }
      { sexe = "m"; decennie = 1910; prenom = "Guillaume"; naissances = 3807 }
      { sexe = "m"; decennie = 1910; prenom = "Hubert"; naissances = 3742 }
      { sexe = "m"; decennie = 1910; prenom = "Marceau"; naissances = 3586 }
      { sexe = "m"; decennie = 1910; prenom = "Laurent"; naissances = 3555 }
      { sexe = "m"; decennie = 1910; prenom = "Maxime"; naissances = 3537 }
      { sexe = "m"; decennie = 1910; prenom = "Ferdinand"; naissances = 3364 }
      { sexe = "m"; decennie = 1910; prenom = "Alexis"; naissances = 3297 }
      { sexe = "m"; decennie = 1910; prenom = "Philippe"; naissances = 3198 }
      { sexe = "m"; decennie = 1910; prenom = "Emmanuel"; naissances = 3066 }
      { sexe = "m"; decennie = 1910; prenom = "Vincent"; naissances = 2840 }
      { sexe = "m"; decennie = 1910; prenom = "Constant"; naissances = 2814 }
      { sexe = "m"; decennie = 1910; prenom = "Antonin"; naissances = 2801 }
      { sexe = "m"; decennie = 1920; prenom = "Jean"; naissances = 303845 }
      { sexe = "m"; decennie = 1920; prenom = "André"; naissances = 175754 }
      { sexe = "m"; decennie = 1920; prenom = "Pierre"; naissances = 158597 }
      { sexe = "m"; decennie = 1920; prenom = "René"; naissances = 139736 }
      { sexe = "m"; decennie = 1920; prenom = "Roger"; naissances = 127789 }
      { sexe = "m"; decennie = 1920; prenom = "Marcel"; naissances = 121488 }
      { sexe = "m"; decennie = 1920; prenom = "Robert"; naissances = 110277 }
      { sexe = "m"; decennie = 1920; prenom = "Georges"; naissances = 95076 }
      { sexe = "m"; decennie = 1920; prenom = "Louis"; naissances = 93762 }
      { sexe = "m"; decennie = 1920; prenom = "Henri"; naissances = 92930 }
      { sexe = "m"; decennie = 1920; prenom = "Maurice"; naissances = 88680 }
      { sexe = "m"; decennie = 1920; prenom = "Jacques"; naissances = 82039 }
      { sexe = "m"; decennie = 1920; prenom = "Joseph"; naissances = 75442 }
      { sexe = "m"; decennie = 1920; prenom = "Paul"; naissances = 74381 }
      { sexe = "m"; decennie = 1920; prenom = "Raymond"; naissances = 73390 }
      { sexe = "m"; decennie = 1920; prenom = "Lucien"; naissances = 54128 }
      { sexe = "m"; decennie = 1920; prenom = "Michel"; naissances = 53444 }
      { sexe = "m"; decennie = 1920; prenom = "Albert"; naissances = 48950 }
      { sexe = "m"; decennie = 1920; prenom = "François"; naissances = 43306 }
      { sexe = "m"; decennie = 1920; prenom = "Charles"; naissances = 41357 }
      { sexe = "m"; decennie = 1920; prenom = "Bernard"; naissances = 38148 }
      { sexe = "m"; decennie = 1920; prenom = "Emile"; naissances = 37513 }
      { sexe = "m"; decennie = 1920; prenom = "Guy"; naissances = 35125 }
      { sexe = "m"; decennie = 1920; prenom = "Claude"; naissances = 34953 }
      { sexe = "m"; decennie = 1920; prenom = "Gilbert"; naissances = 33355 }
      { sexe = "m"; decennie = 1920; prenom = "Fernand"; naissances = 30696 }
      { sexe = "m"; decennie = 1920; prenom = "Yves"; naissances = 25254 }
      { sexe = "m"; decennie = 1920; prenom = "Roland"; naissances = 24444 }
      { sexe = "m"; decennie = 1920; prenom = "Léon"; naissances = 23746 }
      { sexe = "m"; decennie = 1920; prenom = "Eugène"; naissances = 23619 }
      { sexe = "m"; decennie = 1920; prenom = "Gaston"; naissances = 23377 }
      { sexe = "m"; decennie = 1920; prenom = "Antoine"; naissances = 20431 }
      { sexe = "m"; decennie = 1920; prenom = "Gérard"; naissances = 19714 }
      { sexe = "m"; decennie = 1920; prenom = "Gabriel"; naissances = 18754 }
      { sexe = "m"; decennie = 1920; prenom = "Auguste"; naissances = 16543 }
      { sexe = "m"; decennie = 1920; prenom = "Daniel"; naissances = 16103 }
      { sexe = "m"; decennie = 1920; prenom = "Jules"; naissances = 14575 }
      { sexe = "m"; decennie = 1920; prenom = "Alfred"; naissances = 14438 }
      { sexe = "m"; decennie = 1920; prenom = "Serge"; naissances = 14168 }
      { sexe = "m"; decennie = 1920; prenom = "Francis"; naissances = 13838 }
      { sexe = "m"; decennie = 1920; prenom = "Edmond"; naissances = 13304 }
      { sexe = "m"; decennie = 1920; prenom = "Julien"; naissances = 12972 }
      { sexe = "m"; decennie = 1920; prenom = "Marius"; naissances = 12954 }
      { sexe = "m"; decennie = 1920; prenom = "Camille"; naissances = 12224 }
      { sexe = "m"; decennie = 1920; prenom = "Victor"; naissances = 12027 }
      { sexe = "m"; decennie = 1920; prenom = "Marc"; naissances = 11884 }
      { sexe = "m"; decennie = 1920; prenom = "Edouard"; naissances = 11871 }
      { sexe = "m"; decennie = 1920; prenom = "Armand"; naissances = 11431 }
      { sexe = "m"; decennie = 1920; prenom = "Ernest"; naissances = 11082 }
      { sexe = "m"; decennie = 1920; prenom = "Aime"; naissances = 9963 }
      { sexe = "m"; decennie = 1920; prenom = "Etienne"; naissances = 9696 }
      { sexe = "m"; decennie = 1920; prenom = "Alphonse"; naissances = 9688 }
      { sexe = "m"; decennie = 1920; prenom = "Hubert"; naissances = 8905 }
      { sexe = "m"; decennie = 1920; prenom = "Christian"; naissances = 7891 }
      { sexe = "m"; decennie = 1920; prenom = "Alexandre"; naissances = 7824 }
      { sexe = "m"; decennie = 1920; prenom = "Adrien"; naissances = 7791 }
      { sexe = "m"; decennie = 1920; prenom = "Elie"; naissances = 7407 }
      { sexe = "m"; decennie = 1920; prenom = "Félix"; naissances = 7321 }
      { sexe = "m"; decennie = 1920; prenom = "Gustave"; naissances = 7029 }
      { sexe = "m"; decennie = 1920; prenom = "Raoul"; naissances = 6927 }
      { sexe = "m"; decennie = 1920; prenom = "Germain"; naissances = 6233 }
      { sexe = "m"; decennie = 1920; prenom = "Noël"; naissances = 6075 }
      { sexe = "m"; decennie = 1920; prenom = "Philippe"; naissances = 5841 }
      { sexe = "m"; decennie = 1920; prenom = "Clément"; naissances = 5117 }
      { sexe = "m"; decennie = 1920; prenom = "Denis"; naissances = 4845 }
      { sexe = "m"; decennie = 1920; prenom = "Arthur"; naissances = 4536 }
      { sexe = "m"; decennie = 1920; prenom = "Laurent"; naissances = 4415 }
      { sexe = "m"; decennie = 1920; prenom = "Alain"; naissances = 4384 }
      { sexe = "m"; decennie = 1930; prenom = "Jean"; naissances = 339918 }
      { sexe = "m"; decennie = 1930; prenom = "Michel"; naissances = 152303 }
      { sexe = "m"; decennie = 1930; prenom = "André"; naissances = 145622 }
      { sexe = "m"; decennie = 1930; prenom = "Pierre"; naissances = 142565 }
      { sexe = "m"; decennie = 1930; prenom = "Claude"; naissances = 121710 }
      { sexe = "m"; decennie = 1930; prenom = "Jacques"; naissances = 108864 }
      { sexe = "m"; decennie = 1930; prenom = "René"; naissances = 104101 }
      { sexe = "m"; decennie = 1930; prenom = "Roger"; naissances = 95914 }
      { sexe = "m"; decennie = 1930; prenom = "Robert"; naissances = 87071 }
      { sexe = "m"; decennie = 1930; prenom = "Bernard"; naissances = 81872 }
      { sexe = "m"; decennie = 1930; prenom = "Marcel"; naissances = 80935 }
      { sexe = "m"; decennie = 1930; prenom = "Georges"; naissances = 68003 }
      { sexe = "m"; decennie = 1930; prenom = "Guy"; naissances = 64856 }
      { sexe = "m"; decennie = 1930; prenom = "Maurice"; naissances = 64063 }
      { sexe = "m"; decennie = 1930; prenom = "Henri"; naissances = 63389 }
      { sexe = "m"; decennie = 1930; prenom = "Louis"; naissances = 58094 }
      { sexe = "m"; decennie = 1930; prenom = "Joseph"; naissances = 56753 }
      { sexe = "m"; decennie = 1930; prenom = "Gérard"; naissances = 56589 }
      { sexe = "m"; decennie = 1930; prenom = "Raymond"; naissances = 55168 }
      { sexe = "m"; decennie = 1930; prenom = "Paul"; naissances = 53364 }
      { sexe = "m"; decennie = 1930; prenom = "Gilbert"; naissances = 42020 }
      { sexe = "m"; decennie = 1930; prenom = "Daniel"; naissances = 41747 }
      { sexe = "m"; decennie = 1930; prenom = "François"; naissances = 37966 }
      { sexe = "m"; decennie = 1930; prenom = "Yves"; naissances = 37181 }
      { sexe = "m"; decennie = 1930; prenom = "Lucien"; naissances = 36254 }
      { sexe = "m"; decennie = 1930; prenom = "Albert"; naissances = 32152 }
      { sexe = "m"; decennie = 1930; prenom = "Serge"; naissances = 30611 }
      { sexe = "m"; decennie = 1930; prenom = "Roland"; naissances = 30221 }
      { sexe = "m"; decennie = 1930; prenom = "Christian"; naissances = 26385 }
      { sexe = "m"; decennie = 1930; prenom = "Charles"; naissances = 25402 }
      { sexe = "m"; decennie = 1930; prenom = "Emile"; naissances = 20199 }
      { sexe = "m"; decennie = 1930; prenom = "Francis"; naissances = 19382 }
      { sexe = "m"; decennie = 1930; prenom = "Alain"; naissances = 18398 }
      { sexe = "m"; decennie = 1930; prenom = "Fernand"; naissances = 17952 }
      { sexe = "m"; decennie = 1930; prenom = "Marc"; naissances = 15917 }
      { sexe = "m"; decennie = 1930; prenom = "Antoine"; naissances = 15615 }
      { sexe = "m"; decennie = 1930; prenom = "Gabriel"; naissances = 13469 }
      { sexe = "m"; decennie = 1930; prenom = "Léon"; naissances = 12617 }
      { sexe = "m"; decennie = 1930; prenom = "Hubert"; naissances = 12076 }
      { sexe = "m"; decennie = 1930; prenom = "Jean-Claude"; naissances = 11959 }
      { sexe = "m"; decennie = 1930; prenom = "Eugène"; naissances = 11878 }
      { sexe = "m"; decennie = 1930; prenom = "Gaston"; naissances = 11858 }
      { sexe = "m"; decennie = 1930; prenom = "Philippe"; naissances = 11561 }
      { sexe = "m"; decennie = 1930; prenom = "Jean-Pierre"; naissances = 9315 }
      { sexe = "m"; decennie = 1930; prenom = "Edmond"; naissances = 9246 }
      { sexe = "m"; decennie = 1930; prenom = "Armand"; naissances = 8781 }
      { sexe = "m"; decennie = 1930; prenom = "Edouard"; naissances = 8531 }
      { sexe = "m"; decennie = 1930; prenom = "Alfred"; naissances = 8439 }
      { sexe = "m"; decennie = 1930; prenom = "Auguste"; naissances = 7652 }
      { sexe = "m"; decennie = 1930; prenom = "Julien"; naissances = 7457 }
      { sexe = "m"; decennie = 1930; prenom = "Etienne"; naissances = 7424 }
      { sexe = "m"; decennie = 1930; prenom = "Denis"; naissances = 7160 }
      { sexe = "m"; decennie = 1930; prenom = "Victor"; naissances = 7076 }
      { sexe = "m"; decennie = 1930; prenom = "Camille"; naissances = 7034 }
      { sexe = "m"; decennie = 1930; prenom = "Yvon"; naissances = 6924 }
      { sexe = "m"; decennie = 1930; prenom = "Jules"; naissances = 6791 }
      { sexe = "m"; decennie = 1930; prenom = "Aime"; naissances = 6714 }
      { sexe = "m"; decennie = 1930; prenom = "Noël"; naissances = 6553 }
      { sexe = "m"; decennie = 1930; prenom = "Marius"; naissances = 6347 }
      { sexe = "m"; decennie = 1930; prenom = "Max"; naissances = 6121 }
      { sexe = "m"; decennie = 1930; prenom = "Ernest"; naissances = 5569 }
      { sexe = "m"; decennie = 1930; prenom = "Rémy"; naissances = 4975 }
      { sexe = "m"; decennie = 1930; prenom = "Gilles"; naissances = 4974 }
      { sexe = "m"; decennie = 1930; prenom = "Alphonse"; naissances = 4798 }
      { sexe = "m"; decennie = 1930; prenom = "Alexandre"; naissances = 4784 }
      { sexe = "m"; decennie = 1930; prenom = "Jean-Marie"; naissances = 4761 }
      { sexe = "m"; decennie = 1940; prenom = "Jean"; naissances = 421374 }
      { sexe = "m"; decennie = 1940; prenom = "Michel"; naissances = 241014 }
      { sexe = "m"; decennie = 1940; prenom = "Daniel"; naissances = 143027 }
      { sexe = "m"; decennie = 1940; prenom = "Bernard"; naissances = 139982 }
      { sexe = "m"; decennie = 1940; prenom = "Gérard"; naissances = 137697 }
      { sexe = "m"; decennie = 1940; prenom = "Alain"; naissances = 136151 }
      { sexe = "m"; decennie = 1940; prenom = "Jacques"; naissances = 118903 }
      { sexe = "m"; decennie = 1940; prenom = "Claude"; naissances = 109435 }
      { sexe = "m"; decennie = 1940; prenom = "André"; naissances = 105803 }
      { sexe = "m"; decennie = 1940; prenom = "Pierre"; naissances = 103061 }
      { sexe = "m"; decennie = 1940; prenom = "Christian"; naissances = 96077 }
      { sexe = "m"; decennie = 1940; prenom = "Guy"; naissances = 69738 }
      { sexe = "m"; decennie = 1940; prenom = "Jean-Claude"; naissances = 66390 }
      { sexe = "m"; decennie = 1940; prenom = "René"; naissances = 66336 }
      { sexe = "m"; decennie = 1940; prenom = "Robert"; naissances = 65209 }
      { sexe = "m"; decennie = 1940; prenom = "Jean-Pierre"; naissances = 63109 }
      { sexe = "m"; decennie = 1940; prenom = "Roger"; naissances = 54197 }
      { sexe = "m"; decennie = 1940; prenom = "Georges"; naissances = 49240 }
      { sexe = "m"; decennie = 1940; prenom = "Yves"; naissances = 46674 }
      { sexe = "m"; decennie = 1940; prenom = "Marcel"; naissances = 45238 }
      { sexe = "m"; decennie = 1940; prenom = "Serge"; naissances = 43791 }
      { sexe = "m"; decennie = 1940; prenom = "François"; naissances = 42720 }
      { sexe = "m"; decennie = 1940; prenom = "Henri"; naissances = 41248 }
      { sexe = "m"; decennie = 1940; prenom = "Maurice"; naissances = 39955 }
      { sexe = "m"; decennie = 1940; prenom = "Gilbert"; naissances = 39606 }
      { sexe = "m"; decennie = 1940; prenom = "Raymond"; naissances = 35126 }
      { sexe = "m"; decennie = 1940; prenom = "Francis"; naissances = 34221 }
      { sexe = "m"; decennie = 1940; prenom = "Joseph"; naissances = 33614 }
      { sexe = "m"; decennie = 1940; prenom = "Paul"; naissances = 32213 }
      { sexe = "m"; decennie = 1940; prenom = "Louis"; naissances = 30758 }
      { sexe = "m"; decennie = 1940; prenom = "Philippe"; naissances = 29910 }
      { sexe = "m"; decennie = 1940; prenom = "Patrick"; naissances = 29311 }
      { sexe = "m"; decennie = 1940; prenom = "Roland"; naissances = 28763 }
      { sexe = "m"; decennie = 1940; prenom = "Marc"; naissances = 25482 }
      { sexe = "m"; decennie = 1940; prenom = "Joël"; naissances = 25463 }
      { sexe = "m"; decennie = 1940; prenom = "Jean-Paul"; naissances = 20752 }
      { sexe = "m"; decennie = 1940; prenom = "Lucien"; naissances = 19936 }
      { sexe = "m"; decennie = 1940; prenom = "Jean-Marie"; naissances = 18952 }
      { sexe = "m"; decennie = 1940; prenom = "Dominique"; naissances = 18894 }
      { sexe = "m"; decennie = 1940; prenom = "Jacky"; naissances = 18756 }
      { sexe = "m"; decennie = 1940; prenom = "Albert"; naissances = 17668 }
      { sexe = "m"; decennie = 1940; prenom = "Gilles"; naissances = 16657 }
      { sexe = "m"; decennie = 1940; prenom = "Charles"; naissances = 16375 }
      { sexe = "m"; decennie = 1940; prenom = "Jean-Louis"; naissances = 15825 }
      { sexe = "m"; decennie = 1940; prenom = "Denis"; naissances = 15662 }
      { sexe = "m"; decennie = 1940; prenom = "Richard"; naissances = 12316 }
      { sexe = "m"; decennie = 1940; prenom = "Hubert"; naissances = 10442 }
      { sexe = "m"; decennie = 1940; prenom = "Antoine"; naissances = 10342 }
      { sexe = "m"; decennie = 1940; prenom = "Didier"; naissances = 10038 }
      { sexe = "m"; decennie = 1940; prenom = "Patrice"; naissances = 9944 }
      { sexe = "m"; decennie = 1940; prenom = "Yvon"; naissances = 9478 }
      { sexe = "m"; decennie = 1940; prenom = "Jean-Jacques"; naissances = 9246 }
      { sexe = "m"; decennie = 1940; prenom = "Emile"; naissances = 9245 }
      { sexe = "m"; decennie = 1940; prenom = "Gabriel"; naissances = 9245 }
      { sexe = "m"; decennie = 1940; prenom = "Fernand"; naissances = 8760 }
      { sexe = "m"; decennie = 1940; prenom = "Noël"; naissances = 8655 }
      { sexe = "m"; decennie = 1940; prenom = "Hervé"; naissances = 8413 }
      { sexe = "m"; decennie = 1940; prenom = "Jean-François"; naissances = 8409 }
      { sexe = "m"; decennie = 1940; prenom = "Jackie"; naissances = 7965 }
      { sexe = "m"; decennie = 1940; prenom = "Jean-Michel"; naissances = 6465 }
      { sexe = "m"; decennie = 1940; prenom = "Rémy"; naissances = 6155 }
      { sexe = "m"; decennie = 1950; prenom = "Jean"; naissances = 277581 }
      { sexe = "m"; decennie = 1950; prenom = "Michel"; naissances = 215754 }
      { sexe = "m"; decennie = 1950; prenom = "Alain"; naissances = 199013 }
      { sexe = "m"; decennie = 1950; prenom = "Patrick"; naissances = 183988 }
      { sexe = "m"; decennie = 1950; prenom = "Christian"; naissances = 159559 }
      { sexe = "m"; decennie = 1950; prenom = "Philippe"; naissances = 156317 }
      { sexe = "m"; decennie = 1950; prenom = "Bernard"; naissances = 138453 }
      { sexe = "m"; decennie = 1950; prenom = "Daniel"; naissances = 129181 }
      { sexe = "m"; decennie = 1950; prenom = "Gérard"; naissances = 120054 }
      { sexe = "m"; decennie = 1950; prenom = "Dominique"; naissances = 97775 }
      { sexe = "m"; decennie = 1950; prenom = "Jacques"; naissances = 87766 }
      { sexe = "m"; decennie = 1950; prenom = "Didier"; naissances = 84535 }
      { sexe = "m"; decennie = 1950; prenom = "Jean-Pierre"; naissances = 81981 }
      { sexe = "m"; decennie = 1950; prenom = "Pierre"; naissances = 81393 }
      { sexe = "m"; decennie = 1950; prenom = "Claude"; naissances = 81043 }
      { sexe = "m"; decennie = 1950; prenom = "Serge"; naissances = 71949 }
      { sexe = "m"; decennie = 1950; prenom = "Pascal"; naissances = 62961 }
      { sexe = "m"; decennie = 1950; prenom = "André"; naissances = 62954 }
      { sexe = "m"; decennie = 1950; prenom = "Joël"; naissances = 62618 }
      { sexe = "m"; decennie = 1950; prenom = "Guy"; naissances = 59411 }
      { sexe = "m"; decennie = 1950; prenom = "Jean-Claude"; naissances = 58783 }
      { sexe = "m"; decennie = 1950; prenom = "Gilles"; naissances = 56593 }
      { sexe = "m"; decennie = 1950; prenom = "Marc"; naissances = 56196 }
      { sexe = "m"; decennie = 1950; prenom = "Yves"; naissances = 55715 }
      { sexe = "m"; decennie = 1950; prenom = "François"; naissances = 51396 }
      { sexe = "m"; decennie = 1950; prenom = "Francis"; naissances = 50095 }
      { sexe = "m"; decennie = 1950; prenom = "Jean-Luc"; naissances = 49544 }
      { sexe = "m"; decennie = 1950; prenom = "Thierry"; naissances = 46598 }
      { sexe = "m"; decennie = 1950; prenom = "Patrice"; naissances = 45277 }
      { sexe = "m"; decennie = 1950; prenom = "Denis"; naissances = 42916 }
      { sexe = "m"; decennie = 1950; prenom = "Bruno"; naissances = 41982 }
      { sexe = "m"; decennie = 1950; prenom = "Robert"; naissances = 40093 }
      { sexe = "m"; decennie = 1950; prenom = "René"; naissances = 39108 }
      { sexe = "m"; decennie = 1950; prenom = "Jean-Paul"; naissances = 37489 }
      { sexe = "m"; decennie = 1950; prenom = "Eric"; naissances = 37048 }
      { sexe = "m"; decennie = 1950; prenom = "Gilbert"; naissances = 34870 }
      { sexe = "m"; decennie = 1950; prenom = "Jean-Louis"; naissances = 34042 }
      { sexe = "m"; decennie = 1950; prenom = "Georges"; naissances = 31577 }
      { sexe = "m"; decennie = 1950; prenom = "Jean-Marc"; naissances = 30818 }
      { sexe = "m"; decennie = 1950; prenom = "Jean-Marie"; naissances = 30289 }
      { sexe = "m"; decennie = 1950; prenom = "Roger"; naissances = 30282 }
      { sexe = "m"; decennie = 1950; prenom = "Jacky"; naissances = 28447 }
      { sexe = "m"; decennie = 1950; prenom = "Hervé"; naissances = 28333 }
      { sexe = "m"; decennie = 1950; prenom = "Jean-Michel"; naissances = 28263 }
      { sexe = "m"; decennie = 1950; prenom = "Henri"; naissances = 24953 }
      { sexe = "m"; decennie = 1950; prenom = "Marcel"; naissances = 24711 }
      { sexe = "m"; decennie = 1950; prenom = "Jean-François"; naissances = 24708 }
      { sexe = "m"; decennie = 1950; prenom = "Jean-Jacques"; naissances = 24646 }
      { sexe = "m"; decennie = 1950; prenom = "Roland"; naissances = 23873 }
      { sexe = "m"; decennie = 1950; prenom = "Joseph"; naissances = 23702 }
      { sexe = "m"; decennie = 1950; prenom = "Maurice"; naissances = 20643 }
      { sexe = "m"; decennie = 1950; prenom = "Raymond"; naissances = 19964 }
      { sexe = "m"; decennie = 1950; prenom = "Paul"; naissances = 19054 }
      { sexe = "m"; decennie = 1950; prenom = "Richard"; naissances = 17855 }
      { sexe = "m"; decennie = 1950; prenom = "Louis"; naissances = 16474 }
      { sexe = "m"; decennie = 1950; prenom = "Laurent"; naissances = 15462 }
      { sexe = "m"; decennie = 1950; prenom = "Luc"; naissances = 15311 }
      { sexe = "m"; decennie = 1950; prenom = "Olivier"; naissances = 14390 }
      { sexe = "m"; decennie = 1950; prenom = "Frédéric"; naissances = 14329 }
      { sexe = "m"; decennie = 1950; prenom = "Regis"; naissances = 13871 }
      { sexe = "m"; decennie = 1950; prenom = "Lionel"; naissances = 13823 }
      { sexe = "m"; decennie = 1950; prenom = "Jean-Yves"; naissances = 13482 }
      { sexe = "m"; decennie = 1950; prenom = "Hubert"; naissances = 12982 }
      { sexe = "m"; decennie = 1950; prenom = "Charles"; naissances = 12544 }
      { sexe = "m"; decennie = 1950; prenom = "Antoine"; naissances = 11661 }
      { sexe = "m"; decennie = 1950; prenom = "José"; naissances = 11597 }
      { sexe = "m"; decennie = 1950; prenom = "Lucien"; naissances = 11413 }
      { sexe = "m"; decennie = 1950; prenom = "Yvon"; naissances = 10532 }
      { sexe = "m"; decennie = 1950; prenom = "Bertrand"; naissances = 10427 }
      { sexe = "m"; decennie = 1950; prenom = "Noël"; naissances = 9995 }
      { sexe = "m"; decennie = 1950; prenom = "Rémy"; naissances = 9807 }
      { sexe = "m"; decennie = 1950; prenom = "Yannick"; naissances = 9731 }
      { sexe = "m"; decennie = 1950; prenom = "Albert"; naissances = 9372 }
      { sexe = "m"; decennie = 1950; prenom = "Christophe"; naissances = 8467 }
      { sexe = "m"; decennie = 1950; prenom = "Xavier"; naissances = 8464 }
      { sexe = "m"; decennie = 1960; prenom = "Philippe"; naissances = 229584 }
      { sexe = "m"; decennie = 1960; prenom = "Eric"; naissances = 175153 }
      { sexe = "m"; decennie = 1960; prenom = "Pascal"; naissances = 173629 }
      { sexe = "m"; decennie = 1960; prenom = "Thierry"; naissances = 172085 }
      { sexe = "m"; decennie = 1960; prenom = "Patrick"; naissances = 135744 }
      { sexe = "m"; decennie = 1960; prenom = "Christophe"; naissances = 130186 }
      { sexe = "m"; decennie = 1960; prenom = "Laurent"; naissances = 124173 }
      { sexe = "m"; decennie = 1960; prenom = "Jean"; naissances = 122728 }
      { sexe = "m"; decennie = 1960; prenom = "Alain"; naissances = 113364 }
      { sexe = "m"; decennie = 1960; prenom = "Bruno"; naissances = 105633 }
      { sexe = "m"; decennie = 1960; prenom = "Didier"; naissances = 105556 }
      { sexe = "m"; decennie = 1960; prenom = "Frédéric"; naissances = 99405 }
      { sexe = "m"; decennie = 1960; prenom = "Michel"; naissances = 95505 }
      { sexe = "m"; decennie = 1960; prenom = "Dominique"; naissances = 84420 }
      { sexe = "m"; decennie = 1960; prenom = "Stéphane"; naissances = 83397 }
      { sexe = "m"; decennie = 1960; prenom = "Christian"; naissances = 82850 }
      { sexe = "m"; decennie = 1960; prenom = "Olivier"; naissances = 81632 }
      { sexe = "m"; decennie = 1960; prenom = "Franck"; naissances = 69371 }
      { sexe = "m"; decennie = 1960; prenom = "Gilles"; naissances = 61935 }
      { sexe = "m"; decennie = 1960; prenom = "Marc"; naissances = 56381 }
      { sexe = "m"; decennie = 1960; prenom = "Daniel"; naissances = 56345 }
      { sexe = "m"; decennie = 1960; prenom = "Pierre"; naissances = 56188 }
      { sexe = "m"; decennie = 1960; prenom = "Hervé"; naissances = 54494 }
      { sexe = "m"; decennie = 1960; prenom = "François"; naissances = 53735 }
      { sexe = "m"; decennie = 1960; prenom = "Jean-Luc"; naissances = 49421 }
      { sexe = "m"; decennie = 1960; prenom = "Patrice"; naissances = 47364 }
      { sexe = "m"; decennie = 1960; prenom = "Denis"; naissances = 46831 }
      { sexe = "m"; decennie = 1960; prenom = "Fabrice"; naissances = 45632 }
      { sexe = "m"; decennie = 1960; prenom = "Bernard"; naissances = 44094 }
      { sexe = "m"; decennie = 1960; prenom = "Jean-Pierre"; naissances = 42709 }
      { sexe = "m"; decennie = 1960; prenom = "Serge"; naissances = 42154 }
      { sexe = "m"; decennie = 1960; prenom = "Jean-Marc"; naissances = 38322 }
      { sexe = "m"; decennie = 1960; prenom = "Claude"; naissances = 36473 }
      { sexe = "m"; decennie = 1960; prenom = "Joël"; naissances = 35120 }
      { sexe = "m"; decennie = 1960; prenom = "Jacques"; naissances = 33669 }
      { sexe = "m"; decennie = 1960; prenom = "Jean-Michel"; naissances = 33361 }
      { sexe = "m"; decennie = 1960; prenom = "Jean-François"; naissances = 32844 }
      { sexe = "m"; decennie = 1960; prenom = "Yves"; naissances = 31022 }
      { sexe = "m"; decennie = 1960; prenom = "Gérard"; naissances = 30933 }
      { sexe = "m"; decennie = 1960; prenom = "David"; naissances = 30923 }
      { sexe = "m"; decennie = 1960; prenom = "Vincent"; naissances = 30386 }
      { sexe = "m"; decennie = 1960; prenom = "Emmanuel"; naissances = 28843 }
      { sexe = "m"; decennie = 1960; prenom = "Jean-Claude"; naissances = 27480 }
      { sexe = "m"; decennie = 1960; prenom = "Francis"; naissances = 25611 }
      { sexe = "m"; decennie = 1960; prenom = "André"; naissances = 24464 }
      { sexe = "m"; decennie = 1960; prenom = "Xavier"; naissances = 23253 }
      { sexe = "m"; decennie = 1960; prenom = "Nicolas"; naissances = 23159 }
      { sexe = "m"; decennie = 1960; prenom = "Sylvain"; naissances = 22486 }
      { sexe = "m"; decennie = 1960; prenom = "Guy"; naissances = 22407 }
      { sexe = "m"; decennie = 1960; prenom = "Lionel"; naissances = 22137 }
      { sexe = "m"; decennie = 1960; prenom = "Richard"; naissances = 19704 }
      { sexe = "m"; decennie = 1960; prenom = "Jerome"; naissances = 19354 }
      { sexe = "m"; decennie = 1960; prenom = "Jean-Paul"; naissances = 18955 }
      { sexe = "m"; decennie = 1960; prenom = "Jean-Louis"; naissances = 18475 }
      { sexe = "m"; decennie = 1960; prenom = "Luc"; naissances = 18346 }
      { sexe = "m"; decennie = 1960; prenom = "Bertrand"; naissances = 18218 }
      { sexe = "m"; decennie = 1960; prenom = "José"; naissances = 17672 }
      { sexe = "m"; decennie = 1960; prenom = "Yannick"; naissances = 17314 }
      { sexe = "m"; decennie = 1960; prenom = "Jean-Marie"; naissances = 16757 }
      { sexe = "m"; decennie = 1960; prenom = "Jean-Philippe"; naissances = 16280 }
      { sexe = "m"; decennie = 1960; prenom = "Benoît"; naissances = 16264 }
      { sexe = "m"; decennie = 1960; prenom = "Regis"; naissances = 15868 }
      { sexe = "m"; decennie = 1960; prenom = "Joseph"; naissances = 14874 }
      { sexe = "m"; decennie = 1960; prenom = "Robert"; naissances = 14795 }
      { sexe = "m"; decennie = 1960; prenom = "Georges"; naissances = 13540 }
      { sexe = "m"; decennie = 1960; prenom = "Antoine"; naissances = 13246 }
      { sexe = "m"; decennie = 1960; prenom = "Jean-Yves"; naissances = 13244 }
      { sexe = "m"; decennie = 1960; prenom = "Jean-Jacques"; naissances = 13029 }
      { sexe = "m"; decennie = 1960; prenom = "Jean-Christophe"; naissances = 12516 }
      { sexe = "m"; decennie = 1960; prenom = "René"; naissances = 12256 }
      { sexe = "m"; decennie = 1960; prenom = "Jacky"; naissances = 11878 }
      { sexe = "m"; decennie = 1960; prenom = "Ludovic"; naissances = 11468 }
      { sexe = "m"; decennie = 1960; prenom = "Henri"; naissances = 11284 }
      { sexe = "m"; decennie = 1960; prenom = "Arnaud"; naissances = 11127 }
      { sexe = "m"; decennie = 1960; prenom = "Gilbert"; naissances = 11107 }
      { sexe = "m"; decennie = 1960; prenom = "Paul"; naissances = 10311 }
      { sexe = "m"; decennie = 1960; prenom = "Loïc"; naissances = 9742 }
      { sexe = "m"; decennie = 1960; prenom = "Roger"; naissances = 9599 }
      { sexe = "m"; decennie = 1960; prenom = "Fabien"; naissances = 9267 }
      { sexe = "m"; decennie = 1960; prenom = "Roland"; naissances = 9197 }
      { sexe = "m"; decennie = 1960; prenom = "Hubert"; naissances = 8807 }
      { sexe = "m"; decennie = 1960; prenom = "William"; naissances = 8703 }
      { sexe = "m"; decennie = 1960; prenom = "Charles"; naissances = 8600 }
      { sexe = "m"; decennie = 1960; prenom = "Sébastien"; naissances = 8567 }
      { sexe = "m"; decennie = 1960; prenom = "Yann"; naissances = 8399 }
      { sexe = "m"; decennie = 1960; prenom = "Rémy"; naissances = 8283 }
      { sexe = "m"; decennie = 1960; prenom = "Marcel"; naissances = 8094 }
      { sexe = "m"; decennie = 1960; prenom = "Martial"; naissances = 7964 }
      { sexe = "m"; decennie = 1960; prenom = "Manuel"; naissances = 7668 }
      { sexe = "m"; decennie = 1960; prenom = "Louis"; naissances = 7467 }
      { sexe = "m"; decennie = 1960; prenom = "Rémi"; naissances = 7390 }
      { sexe = "m"; decennie = 1960; prenom = "Maurice"; naissances = 6963 }
      { sexe = "m"; decennie = 1960; prenom = "Noël"; naissances = 6515 }
      { sexe = "m"; decennie = 1960; prenom = "Alexandre"; naissances = 6435 }
      { sexe = "m"; decennie = 1960; prenom = "Raymond"; naissances = 6423 }
      { sexe = "m"; decennie = 1960; prenom = "Guillaume"; naissances = 6336 }
      { sexe = "m"; decennie = 1960; prenom = "Hugues"; naissances = 6171 }
      { sexe = "m"; decennie = 1960; prenom = "Antonio"; naissances = 6130 }
      { sexe = "m"; decennie = 1960; prenom = "Etienne"; naissances = 5887 }
      { sexe = "m"; decennie = 1960; prenom = "Gérald"; naissances = 5851 }
      { sexe = "m"; decennie = 1960; prenom = "Raphaël"; naissances = 5810 }
      { sexe = "m"; decennie = 1960; prenom = "Mohamed"; naissances = 5774 }
      { sexe = "m"; decennie = 1960; prenom = "Cyrille"; naissances = 5673 }
      { sexe = "m"; decennie = 1960; prenom = "Yvan"; naissances = 4969 }
      { sexe = "m"; decennie = 1960; prenom = "Damien"; naissances = 4923 }
      { sexe = "m"; decennie = 1960; prenom = "Yvon"; naissances = 4863 }
      { sexe = "m"; decennie = 1960; prenom = "Jean-Charles"; naissances = 4838 }
      { sexe = "m"; decennie = 1960; prenom = "Florent"; naissances = 4748 }
      { sexe = "m"; decennie = 1960; prenom = "Jean-Noël"; naissances = 4736 }
      { sexe = "m"; decennie = 1960; prenom = "Albert"; naissances = 4578 }
      { sexe = "m"; decennie = 1970; prenom = "Christophe"; naissances = 162890 }
      { sexe = "m"; decennie = 1970; prenom = "David"; naissances = 159450 }
      { sexe = "m"; decennie = 1970; prenom = "Stéphane"; naissances = 159100 }
      { sexe = "m"; decennie = 1970; prenom = "Sébastien"; naissances = 152125 }
      { sexe = "m"; decennie = 1970; prenom = "Frédéric"; naissances = 139153 }
      { sexe = "m"; decennie = 1970; prenom = "Laurent"; naissances = 124307 }
      { sexe = "m"; decennie = 1970; prenom = "Olivier"; naissances = 115316 }
      { sexe = "m"; decennie = 1970; prenom = "Nicolas"; naissances = 95691 }
      { sexe = "m"; decennie = 1970; prenom = "Jerome"; naissances = 91593 }
      { sexe = "m"; decennie = 1970; prenom = "Eric"; naissances = 74900 }
      { sexe = "m"; decennie = 1970; prenom = "Philippe"; naissances = 71591 }
      { sexe = "m"; decennie = 1970; prenom = "Franck"; naissances = 67652 }
      { sexe = "m"; decennie = 1970; prenom = "Fabrice"; naissances = 60232 }
      { sexe = "m"; decennie = 1970; prenom = "Vincent"; naissances = 57626 }
      { sexe = "m"; decennie = 1970; prenom = "Cedric"; naissances = 54371 }
      { sexe = "m"; decennie = 1970; prenom = "Thierry"; naissances = 53403 }
      { sexe = "m"; decennie = 1970; prenom = "Arnaud"; naissances = 50278 }
      { sexe = "m"; decennie = 1970; prenom = "Emmanuel"; naissances = 49041 }
      { sexe = "m"; decennie = 1970; prenom = "Ludovic"; naissances = 48379 }
      { sexe = "m"; decennie = 1970; prenom = "Pascal"; naissances = 48364 }
      { sexe = "m"; decennie = 1970; prenom = "Guillaume"; naissances = 42064 }
      { sexe = "m"; decennie = 1970; prenom = "Sylvain"; naissances = 40480 }
      { sexe = "m"; decennie = 1970; prenom = "Bruno"; naissances = 40387 }
      { sexe = "m"; decennie = 1970; prenom = "Julien"; naissances = 38660 }
      { sexe = "m"; decennie = 1970; prenom = "Alexandre"; naissances = 38528 }
      { sexe = "m"; decennie = 1970; prenom = "Jean"; naissances = 37658 }
      { sexe = "m"; decennie = 1970; prenom = "Michael"; naissances = 37141 }
      { sexe = "m"; decennie = 1970; prenom = "Cyril"; naissances = 35840 }
      { sexe = "m"; decennie = 1970; prenom = "Xavier"; naissances = 33881 }
      { sexe = "m"; decennie = 1970; prenom = "François"; naissances = 33177 }
      { sexe = "m"; decennie = 1970; prenom = "Benoît"; naissances = 32626 }
      { sexe = "m"; decennie = 1970; prenom = "Fabien"; naissances = 32386 }
      { sexe = "m"; decennie = 1970; prenom = "Patrick"; naissances = 32375 }
      { sexe = "m"; decennie = 1970; prenom = "Pierre"; naissances = 30639 }
      { sexe = "m"; decennie = 1970; prenom = "Hervé"; naissances = 29359 }
      { sexe = "m"; decennie = 1970; prenom = "Gregory"; naissances = 29231 }
      { sexe = "m"; decennie = 1970; prenom = "Mickael"; naissances = 28626 }
      { sexe = "m"; decennie = 1970; prenom = "Yannick"; naissances = 28506 }
      { sexe = "m"; decennie = 1970; prenom = "Lionel"; naissances = 27743 }
      { sexe = "m"; decennie = 1970; prenom = "Yann"; naissances = 27206 }
      { sexe = "m"; decennie = 1970; prenom = "Marc"; naissances = 26980 }
      { sexe = "m"; decennie = 1970; prenom = "Patrice"; naissances = 26469 }
      { sexe = "m"; decennie = 1970; prenom = "Didier"; naissances = 26145 }
      { sexe = "m"; decennie = 1970; prenom = "Michel"; naissances = 23915 }
      { sexe = "m"; decennie = 1970; prenom = "Alain"; naissances = 23018 }
      { sexe = "m"; decennie = 1970; prenom = "Thomas"; naissances = 22939 }
      { sexe = "m"; decennie = 1970; prenom = "Gilles"; naissances = 22715 }
      { sexe = "m"; decennie = 1970; prenom = "Damien"; naissances = 21883 }
      { sexe = "m"; decennie = 1970; prenom = "Jean-François"; naissances = 21558 }
      { sexe = "m"; decennie = 1970; prenom = "Dominique"; naissances = 21174 }
      { sexe = "m"; decennie = 1970; prenom = "Anthony"; naissances = 20209 }
      { sexe = "m"; decennie = 1970; prenom = "Christian"; naissances = 19592 }
      { sexe = "m"; decennie = 1970; prenom = "Samuel"; naissances = 18571 }
      { sexe = "m"; decennie = 1970; prenom = "Denis"; naissances = 18377 }
      { sexe = "m"; decennie = 1970; prenom = "Cyrille"; naissances = 18036 }
      { sexe = "m"; decennie = 1970; prenom = "Mathieu"; naissances = 18012 }
      { sexe = "m"; decennie = 1970; prenom = "Daniel"; naissances = 17961 }
      { sexe = "m"; decennie = 1970; prenom = "Bertrand"; naissances = 17692 }
      { sexe = "m"; decennie = 1970; prenom = "Raphaël"; naissances = 17224 }
      { sexe = "m"; decennie = 1970; prenom = "Loïc"; naissances = 16386 }
      { sexe = "m"; decennie = 1970; prenom = "Richard"; naissances = 15569 }
      { sexe = "m"; decennie = 1970; prenom = "Antoine"; naissances = 14442 }
      { sexe = "m"; decennie = 1970; prenom = "Jean-Philippe"; naissances = 14063 }
      { sexe = "m"; decennie = 1970; prenom = "José"; naissances = 13564 }
      { sexe = "m"; decennie = 1970; prenom = "Jean-Michel"; naissances = 13042 }
      { sexe = "m"; decennie = 1970; prenom = "Regis"; naissances = 12863 }
      { sexe = "m"; decennie = 1970; prenom = "Jean-Marc"; naissances = 12835 }
      { sexe = "m"; decennie = 1970; prenom = "Gérald"; naissances = 12046 }
      { sexe = "m"; decennie = 1970; prenom = "Benjamin"; naissances = 12017 }
      { sexe = "m"; decennie = 1970; prenom = "Florent"; naissances = 11883 }
      { sexe = "m"; decennie = 1970; prenom = "Karim"; naissances = 11831 }
      { sexe = "m"; decennie = 1970; prenom = "Matthieu"; naissances = 11747 }
      { sexe = "m"; decennie = 1970; prenom = "Tony"; naissances = 11337 }
      { sexe = "m"; decennie = 1970; prenom = "Jérôme"; naissances = 11330 }
      { sexe = "m"; decennie = 1970; prenom = "Romain"; naissances = 11300 }
      { sexe = "m"; decennie = 1970; prenom = "William"; naissances = 10933 }
      { sexe = "m"; decennie = 1970; prenom = "Jean-Christophe"; naissances = 10791 }
      { sexe = "m"; decennie = 1970; prenom = "Joël"; naissances = 10647 }
      { sexe = "m"; decennie = 1970; prenom = "Manuel"; naissances = 10636 }
      { sexe = "m"; decennie = 1970; prenom = "Jean-Pierre"; naissances = 10524 }
      { sexe = "m"; decennie = 1970; prenom = "Jean-Luc"; naissances = 9991 }
      { sexe = "m"; decennie = 1970; prenom = "Mohamed"; naissances = 9949 }
      { sexe = "m"; decennie = 1970; prenom = "Claude"; naissances = 9599 }
      { sexe = "m"; decennie = 1970; prenom = "Serge"; naissances = 9471 }
      { sexe = "m"; decennie = 1970; prenom = "Yves"; naissances = 8127 }
      { sexe = "m"; decennie = 1970; prenom = "Romuald"; naissances = 8098 }
      { sexe = "m"; decennie = 1970; prenom = "Rodolphe"; naissances = 7927 }
      { sexe = "m"; decennie = 1970; prenom = "Eddy"; naissances = 7838 }
      { sexe = "m"; decennie = 1970; prenom = "Cédric"; naissances = 7807 }
      { sexe = "m"; decennie = 1970; prenom = "Luc"; naissances = 7438 }
      { sexe = "m"; decennie = 1970; prenom = "Alexis"; naissances = 7326 }
      { sexe = "m"; decennie = 1970; prenom = "Jacques"; naissances = 7281 }
      { sexe = "m"; decennie = 1970; prenom = "Renaud"; naissances = 7018 }
      { sexe = "m"; decennie = 1970; prenom = "Bernard"; naissances = 6935 }
      { sexe = "m"; decennie = 1970; prenom = "Maxime"; naissances = 6921 }
      { sexe = "m"; decennie = 1970; prenom = "Mikael"; naissances = 6690 }
      { sexe = "m"; decennie = 1970; prenom = "Aurélien"; naissances = 6672 }
      { sexe = "m"; decennie = 1970; prenom = "Charles"; naissances = 6658 }
      { sexe = "m"; decennie = 1970; prenom = "Yoann"; naissances = 6541 }
      { sexe = "m"; decennie = 1970; prenom = "Gael"; naissances = 6477 }
      { sexe = "m"; decennie = 1970; prenom = "Jeremy"; naissances = 6430 }
      { sexe = "m"; decennie = 1970; prenom = "Willy"; naissances = 6404 }
      { sexe = "m"; decennie = 1970; prenom = "Jean-Marie"; naissances = 6245 }
      { sexe = "m"; decennie = 1970; prenom = "Jean-Baptiste"; naissances = 6205 }
      { sexe = "m"; decennie = 1970; prenom = "Erwan"; naissances = 6191 }
      { sexe = "m"; decennie = 1970; prenom = "Farid"; naissances = 6171 }
      { sexe = "m"; decennie = 1970; prenom = "Georges"; naissances = 6148 }
      { sexe = "m"; decennie = 1970; prenom = "Jimmy"; naissances = 6148 }
      { sexe = "m"; decennie = 1970; prenom = "Rémi"; naissances = 6080 }
      { sexe = "m"; decennie = 1970; prenom = "Steve"; naissances = 6009 }
      { sexe = "m"; decennie = 1970; prenom = "Jean-Claude"; naissances = 5982 }
      { sexe = "m"; decennie = 1970; prenom = "Johann"; naissances = 5944 }
      { sexe = "m"; decennie = 1970; prenom = "Rachid"; naissances = 5942 }
      { sexe = "m"; decennie = 1970; prenom = "Paul"; naissances = 5821 }
      { sexe = "m"; decennie = 1970; prenom = "Carlos"; naissances = 5759 }
      { sexe = "m"; decennie = 1970; prenom = "Antonio"; naissances = 5727 }
      { sexe = "m"; decennie = 1970; prenom = "Gaëtan"; naissances = 5663 }
      { sexe = "m"; decennie = 1970; prenom = "Francis"; naissances = 5648 }
      { sexe = "m"; decennie = 1970; prenom = "Martial"; naissances = 5549 }
      { sexe = "m"; decennie = 1970; prenom = "Joseph"; naissances = 5427 }
      { sexe = "m"; decennie = 1970; prenom = "Rémy"; naissances = 5424 }
      { sexe = "m"; decennie = 1970; prenom = "Gérard"; naissances = 5375 }
      { sexe = "m"; decennie = 1970; prenom = "Freddy"; naissances = 5302 }
      { sexe = "m"; decennie = 1970; prenom = "André"; naissances = 5273 }
      { sexe = "m"; decennie = 1970; prenom = "Stephan"; naissances = 5066 }
      { sexe = "m"; decennie = 1970; prenom = "Etienne"; naissances = 5062 }
      { sexe = "m"; decennie = 1970; prenom = "Jean-Louis"; naissances = 5002 }
      { sexe = "m"; decennie = 1970; prenom = "Jean-Paul"; naissances = 4960 }
      { sexe = "m"; decennie = 1970; prenom = "Yvan"; naissances = 4858 }
      { sexe = "m"; decennie = 1970; prenom = "Johan"; naissances = 4643 }
      { sexe = "m"; decennie = 1970; prenom = "Jean-Yves"; naissances = 4593 }
      { sexe = "m"; decennie = 1970; prenom = "Hugues"; naissances = 4567 }
      { sexe = "m"; decennie = 1970; prenom = "Jeremie"; naissances = 4547 }
      { sexe = "m"; decennie = 1970; prenom = "Brice"; naissances = 4539 }
      { sexe = "m"; decennie = 1970; prenom = "Yohann"; naissances = 4500 }
      { sexe = "m"; decennie = 1970; prenom = "Jean-Charles"; naissances = 4496 }
      { sexe = "m"; decennie = 1970; prenom = "Johnny"; naissances = 4492 }
      { sexe = "m"; decennie = 1970; prenom = "Dimitri"; naissances = 4486 }
      { sexe = "m"; decennie = 1970; prenom = "Jonathan"; naissances = 4410 }
      { sexe = "m"; decennie = 1970; prenom = "Wilfried"; naissances = 4394 }
      { sexe = "m"; decennie = 1970; prenom = "Boris"; naissances = 4386 }
      { sexe = "m"; decennie = 1970; prenom = "Kamel"; naissances = 4242 }
      { sexe = "m"; decennie = 1970; prenom = "Grégory"; naissances = 4181 }
      { sexe = "m"; decennie = 1970; prenom = "Gabriel"; naissances = 4138 }
      { sexe = "m"; decennie = 1970; prenom = "Jacky"; naissances = 4013 }
      { sexe = "m"; decennie = 1970; prenom = "Florian"; naissances = 3940 }
      { sexe = "m"; decennie = 1970; prenom = "Michaël"; naissances = 3887 }
      { sexe = "m"; decennie = 1970; prenom = "Mehdi"; naissances = 3864 }
      { sexe = "m"; decennie = 1970; prenom = "Antony"; naissances = 3836 }
      { sexe = "m"; decennie = 1970; prenom = "Frank"; naissances = 3835 }
      { sexe = "m"; decennie = 1970; prenom = "Victor"; naissances = 3811 }
      { sexe = "m"; decennie = 1970; prenom = "Ronan"; naissances = 3789 }
      { sexe = "m"; decennie = 1970; prenom = "Guy"; naissances = 3785 }
      { sexe = "m"; decennie = 1970; prenom = "Ali"; naissances = 3721 }
      { sexe = "m"; decennie = 1970; prenom = "Samir"; naissances = 3680 }
      { sexe = "m"; decennie = 1970; prenom = "Louis"; naissances = 3596 }
      { sexe = "m"; decennie = 1970; prenom = "Robert"; naissances = 3589 }
      { sexe = "m"; decennie = 1970; prenom = "Miguel"; naissances = 3511 }
      { sexe = "m"; decennie = 1970; prenom = "Edouard"; naissances = 3472 }
      { sexe = "m"; decennie = 1970; prenom = "Henri"; naissances = 3439 }
      { sexe = "m"; decennie = 1970; prenom = "Rudy"; naissances = 3391 }
      { sexe = "m"; decennie = 1970; prenom = "Jean-Jacques"; naissances = 3373 }
      { sexe = "m"; decennie = 1970; prenom = "John"; naissances = 3368 }
      { sexe = "m"; decennie = 1970; prenom = "Alban"; naissances = 3346 }
      { sexe = "m"; decennie = 1970; prenom = "Mickaël"; naissances = 3249 }
      { sexe = "m"; decennie = 1970; prenom = "Mathias"; naissances = 3209 }
      { sexe = "m"; decennie = 1970; prenom = "Jean-Noël"; naissances = 3087 }
      { sexe = "m"; decennie = 1970; prenom = "Noël"; naissances = 3063 }
      { sexe = "m"; decennie = 1970; prenom = "Ahmed"; naissances = 3059 }
      { sexe = "m"; decennie = 1970; prenom = "Simon"; naissances = 3015 }
      { sexe = "m"; decennie = 1970; prenom = "Grégoire"; naissances = 2992 }
      { sexe = "m"; decennie = 1970; prenom = "Wilfrid"; naissances = 2957 }
      { sexe = "m"; decennie = 1970; prenom = "Hubert"; naissances = 2925 }
      { sexe = "m"; decennie = 1970; prenom = "Clément"; naissances = 2877 }
      { sexe = "m"; decennie = 1970; prenom = "Abdelkader"; naissances = 2863 }
      { sexe = "m"; decennie = 1970; prenom = "Teddy"; naissances = 2850 }
      { sexe = "m"; decennie = 1970; prenom = "Gwenael"; naissances = 2827 }
      { sexe = "m"; decennie = 1970; prenom = "Thibaut"; naissances = 2821 }
      { sexe = "m"; decennie = 1970; prenom = "Davy"; naissances = 2778 }
      { sexe = "m"; decennie = 1970; prenom = "Paulo"; naissances = 2756 }
      { sexe = "m"; decennie = 1970; prenom = "Dany"; naissances = 2725 }
      { sexe = "m"; decennie = 1970; prenom = "Mustapha"; naissances = 2713 }
      { sexe = "m"; decennie = 1970; prenom = "Roland"; naissances = 2698 }
      { sexe = "m"; decennie = 1970; prenom = "Said"; naissances = 2667 }
      { sexe = "m"; decennie = 1970; prenom = "Nordine"; naissances = 2601 }
      { sexe = "m"; decennie = 1970; prenom = "Ghislain"; naissances = 2598 }
      { sexe = "m"; decennie = 1970; prenom = "Lilian"; naissances = 2593 }
      { sexe = "m"; decennie = 1970; prenom = "Aymeric"; naissances = 2586 }
      { sexe = "m"; decennie = 1970; prenom = "Steeve"; naissances = 2519 }
      { sexe = "m"; decennie = 1970; prenom = "Jean-Sébastien"; naissances = 2515 }
      { sexe = "m"; decennie = 1970; prenom = "Mourad"; naissances = 2498 }
      { sexe = "m"; decennie = 1970; prenom = "René"; naissances = 2478 }
      { sexe = "m"; decennie = 1970; prenom = "Rodrigue"; naissances = 2472 }
      { sexe = "m"; decennie = 1970; prenom = "Karl"; naissances = 2441 }
      { sexe = "m"; decennie = 1970; prenom = "Adrien"; naissances = 2396 }
      { sexe = "m"; decennie = 1970; prenom = "Pierre-Yves"; naissances = 2374 }
      { sexe = "m"; decennie = 1970; prenom = "Luis"; naissances = 2336 }
      { sexe = "m"; decennie = 1970; prenom = "Baptiste"; naissances = 2326 }
      { sexe = "m"; decennie = 1970; prenom = "James"; naissances = 2318 }
      { sexe = "m"; decennie = 1970; prenom = "Pierrick"; naissances = 2274 }
      { sexe = "m"; decennie = 1980; prenom = "Nicolas"; naissances = 146107 }
      { sexe = "m"; decennie = 1980; prenom = "Julien"; naissances = 145913 }
      { sexe = "m"; decennie = 1980; prenom = "Sébastien"; naissances = 102098 }
      { sexe = "m"; decennie = 1980; prenom = "Guillaume"; naissances = 84002 }
      { sexe = "m"; decennie = 1980; prenom = "Alexandre"; naissances = 74083 }
      { sexe = "m"; decennie = 1980; prenom = "David"; naissances = 73188 }
      { sexe = "m"; decennie = 1980; prenom = "Romain"; naissances = 70576 }
      { sexe = "m"; decennie = 1980; prenom = "Thomas"; naissances = 68373 }
      { sexe = "m"; decennie = 1980; prenom = "Anthony"; naissances = 67363 }
      { sexe = "m"; decennie = 1980; prenom = "Vincent"; naissances = 62056 }
      { sexe = "m"; decennie = 1980; prenom = "Jonathan"; naissances = 58838 }
      { sexe = "m"; decennie = 1980; prenom = "Mathieu"; naissances = 58136 }
      { sexe = "m"; decennie = 1980; prenom = "Cedric"; naissances = 56498 }
      { sexe = "m"; decennie = 1980; prenom = "Christophe"; naissances = 56249 }
      { sexe = "m"; decennie = 1980; prenom = "Jeremy"; naissances = 53696 }
      { sexe = "m"; decennie = 1980; prenom = "Damien"; naissances = 52750 }
      { sexe = "m"; decennie = 1980; prenom = "Mickael"; naissances = 49729 }
      { sexe = "m"; decennie = 1980; prenom = "Jerome"; naissances = 48317 }
      { sexe = "m"; decennie = 1980; prenom = "Benjamin"; naissances = 46773 }
      { sexe = "m"; decennie = 1980; prenom = "Frédéric"; naissances = 46515 }
      { sexe = "m"; decennie = 1980; prenom = "Pierre"; naissances = 46169 }
      { sexe = "m"; decennie = 1980; prenom = "Kevin"; naissances = 43934 }
      { sexe = "m"; decennie = 1980; prenom = "Ludovic"; naissances = 43169 }
      { sexe = "m"; decennie = 1980; prenom = "Olivier"; naissances = 41810 }
      { sexe = "m"; decennie = 1980; prenom = "Maxime"; naissances = 41713 }
      { sexe = "m"; decennie = 1980; prenom = "Fabien"; naissances = 41454 }
      { sexe = "m"; decennie = 1980; prenom = "Stéphane"; naissances = 40065 }
      { sexe = "m"; decennie = 1980; prenom = "Arnaud"; naissances = 39606 }
      { sexe = "m"; decennie = 1980; prenom = "Benoît"; naissances = 38589 }
      { sexe = "m"; decennie = 1980; prenom = "Sylvain"; naissances = 37459 }
      { sexe = "m"; decennie = 1980; prenom = "Aurélien"; naissances = 35656 }
      { sexe = "m"; decennie = 1980; prenom = "Gregory"; naissances = 35338 }
      { sexe = "m"; decennie = 1980; prenom = "Laurent"; naissances = 34813 }
      { sexe = "m"; decennie = 1980; prenom = "Florian"; naissances = 34715 }
      { sexe = "m"; decennie = 1980; prenom = "Antoine"; naissances = 31365 }
      { sexe = "m"; decennie = 1980; prenom = "Matthieu"; naissances = 31145 }
      { sexe = "m"; decennie = 1980; prenom = "François"; naissances = 30027 }
      { sexe = "m"; decennie = 1980; prenom = "Loïc"; naissances = 29998 }
      { sexe = "m"; decennie = 1980; prenom = "Cyril"; naissances = 29823 }
      { sexe = "m"; decennie = 1980; prenom = "Michael"; naissances = 28797 }
      { sexe = "m"; decennie = 1980; prenom = "Jean"; naissances = 28600 }
      { sexe = "m"; decennie = 1980; prenom = "Adrien"; naissances = 27603 }
      { sexe = "m"; decennie = 1980; prenom = "Clément"; naissances = 24388 }
      { sexe = "m"; decennie = 1980; prenom = "Florent"; naissances = 23870 }
      { sexe = "m"; decennie = 1980; prenom = "Emmanuel"; naissances = 22436 }
      { sexe = "m"; decennie = 1980; prenom = "Xavier"; naissances = 22135 }
      { sexe = "m"; decennie = 1980; prenom = "Rémi"; naissances = 20821 }
      { sexe = "m"; decennie = 1980; prenom = "Yannick"; naissances = 20471 }
      { sexe = "m"; decennie = 1980; prenom = "Franck"; naissances = 20082 }
      { sexe = "m"; decennie = 1980; prenom = "Yann"; naissances = 19494 }
      { sexe = "m"; decennie = 1980; prenom = "Alexis"; naissances = 18642 }
      { sexe = "m"; decennie = 1980; prenom = "Yoann"; naissances = 18544 }
      { sexe = "m"; decennie = 1980; prenom = "Marc"; naissances = 18144 }
      { sexe = "m"; decennie = 1980; prenom = "Philippe"; naissances = 17869 }
      { sexe = "m"; decennie = 1980; prenom = "Eric"; naissances = 17485 }
      { sexe = "m"; decennie = 1980; prenom = "Samuel"; naissances = 17184 }
      { sexe = "m"; decennie = 1980; prenom = "Fabrice"; naissances = 16398 }
      { sexe = "m"; decennie = 1980; prenom = "Raphaël"; naissances = 15876 }
      { sexe = "m"; decennie = 1980; prenom = "Mohamed"; naissances = 15796 }
      { sexe = "m"; decennie = 1980; prenom = "Simon"; naissances = 14301 }
      { sexe = "m"; decennie = 1980; prenom = "Bruno"; naissances = 14216 }
      { sexe = "m"; decennie = 1980; prenom = "Lionel"; naissances = 13855 }
      { sexe = "m"; decennie = 1980; prenom = "Christopher"; naissances = 13766 }
      { sexe = "m"; decennie = 1980; prenom = "Quentin"; naissances = 13676 }
      { sexe = "m"; decennie = 1980; prenom = "Tony"; naissances = 13339 }
      { sexe = "m"; decennie = 1980; prenom = "Jean-Baptiste"; naissances = 12780 }
      { sexe = "m"; decennie = 1980; prenom = "Gaëtan"; naissances = 12633 }
      { sexe = "m"; decennie = 1980; prenom = "Rémy"; naissances = 12531 }
      { sexe = "m"; decennie = 1980; prenom = "Dimitri"; naissances = 12379 }
      { sexe = "m"; decennie = 1980; prenom = "Geoffrey"; naissances = 12091 }
      { sexe = "m"; decennie = 1980; prenom = "Karim"; naissances = 12062 }
      { sexe = "m"; decennie = 1980; prenom = "Charles"; naissances = 11825 }
      { sexe = "m"; decennie = 1980; prenom = "Baptiste"; naissances = 11772 }
      { sexe = "m"; decennie = 1980; prenom = "Johan"; naissances = 11671 }
      { sexe = "m"; decennie = 1980; prenom = "Jeremie"; naissances = 11605 }
      { sexe = "m"; decennie = 1980; prenom = "Mehdi"; naissances = 11181 }
      { sexe = "m"; decennie = 1980; prenom = "Thibault"; naissances = 10992 }
      { sexe = "m"; decennie = 1980; prenom = "Paul"; naissances = 10928 }
      { sexe = "m"; decennie = 1980; prenom = "Jimmy"; naissances = 10903 }
      { sexe = "m"; decennie = 1980; prenom = "Pascal"; naissances = 10548 }
      { sexe = "m"; decennie = 1980; prenom = "William"; naissances = 10503 }
      { sexe = "m"; decennie = 1980; prenom = "Thierry"; naissances = 10474 }
      { sexe = "m"; decennie = 1980; prenom = "Brice"; naissances = 9833 }
      { sexe = "m"; decennie = 1980; prenom = "Bertrand"; naissances = 9514 }
      { sexe = "m"; decennie = 1980; prenom = "Yohann"; naissances = 9350 }
      { sexe = "m"; decennie = 1980; prenom = "Steve"; naissances = 9307 }
      { sexe = "m"; decennie = 1980; prenom = "Thibaut"; naissances = 9136 }
      { sexe = "m"; decennie = 1980; prenom = "Bastien"; naissances = 8870 }
      { sexe = "m"; decennie = 1980; prenom = "Michel"; naissances = 8464 }
      { sexe = "m"; decennie = 1980; prenom = "Gael"; naissances = 8459 }
      { sexe = "m"; decennie = 1980; prenom = "Morgan"; naissances = 8383 }
      { sexe = "m"; decennie = 1980; prenom = "Teddy"; naissances = 8319 }
      { sexe = "m"; decennie = 1980; prenom = "Etienne"; naissances = 8312 }
      { sexe = "m"; decennie = 1980; prenom = "Yohan"; naissances = 8236 }
      { sexe = "m"; decennie = 1980; prenom = "Patrick"; naissances = 8227 }
      { sexe = "m"; decennie = 1980; prenom = "Daniel"; naissances = 7955 }
      { sexe = "m"; decennie = 1980; prenom = "Renaud"; naissances = 7928 }
      { sexe = "m"; decennie = 1980; prenom = "Jean-François"; naissances = 7902 }
      { sexe = "m"; decennie = 1980; prenom = "Cédric"; naissances = 7873 }
      { sexe = "m"; decennie = 1980; prenom = "Gilles"; naissances = 7837 }
      { sexe = "m"; decennie = 1980; prenom = "Hervé"; naissances = 7746 }
      { sexe = "m"; decennie = 1980; prenom = "Jean-Philippe"; naissances = 7653 }
      { sexe = "m"; decennie = 1980; prenom = "Jérémy"; naissances = 7426 }
      { sexe = "m"; decennie = 1980; prenom = "Rudy"; naissances = 7378 }
      { sexe = "m"; decennie = 1980; prenom = "Steven"; naissances = 7268 }
      { sexe = "m"; decennie = 1980; prenom = "Jérôme"; naissances = 7039 }
      { sexe = "m"; decennie = 1980; prenom = "Denis"; naissances = 6853 }
      { sexe = "m"; decennie = 1980; prenom = "Johann"; naissances = 6791 }
      { sexe = "m"; decennie = 1980; prenom = "Manuel"; naissances = 6768 }
      { sexe = "m"; decennie = 1980; prenom = "Cyrille"; naissances = 6766 }
      { sexe = "m"; decennie = 1980; prenom = "Richard"; naissances = 6701 }
      { sexe = "m"; decennie = 1980; prenom = "Mikael"; naissances = 6625 }
      { sexe = "m"; decennie = 1980; prenom = "Samir"; naissances = 6496 }
      { sexe = "m"; decennie = 1980; prenom = "Gabriel"; naissances = 6406 }
      { sexe = "m"; decennie = 1980; prenom = "Mickaël"; naissances = 6319 }
      { sexe = "m"; decennie = 1980; prenom = "Eddy"; naissances = 6257 }
      { sexe = "m"; decennie = 1980; prenom = "Edouard"; naissances = 6238 }
      { sexe = "m"; decennie = 1980; prenom = "Alain"; naissances = 6096 }
      { sexe = "m"; decennie = 1980; prenom = "Patrice"; naissances = 6073 }
      { sexe = "m"; decennie = 1980; prenom = "Valentin"; naissances = 5679 }
      { sexe = "m"; decennie = 1980; prenom = "Romuald"; naissances = 5673 }
      { sexe = "m"; decennie = 1980; prenom = "Louis"; naissances = 5522 }
      { sexe = "m"; decennie = 1980; prenom = "Christian"; naissances = 5509 }
      { sexe = "m"; decennie = 1980; prenom = "Erwan"; naissances = 5504 }
      { sexe = "m"; decennie = 1980; prenom = "Hugo"; naissances = 5385 }
      { sexe = "m"; decennie = 1980; prenom = "Didier"; naissances = 5384 }
      { sexe = "m"; decennie = 1980; prenom = "Rachid"; naissances = 5274 }
      { sexe = "m"; decennie = 1980; prenom = "Victor"; naissances = 5261 }
      { sexe = "m"; decennie = 1980; prenom = "Johnny"; naissances = 5254 }
      { sexe = "m"; decennie = 1980; prenom = "Jean-Christophe"; naissances = 5239 }
      { sexe = "m"; decennie = 1980; prenom = "Martin"; naissances = 5166 }
      { sexe = "m"; decennie = 1980; prenom = "Jordan"; naissances = 5135 }
      { sexe = "m"; decennie = 1980; prenom = "Yoan"; naissances = 5126 }
      { sexe = "m"; decennie = 1980; prenom = "Boris"; naissances = 5011 }
      { sexe = "m"; decennie = 1980; prenom = "Grégory"; naissances = 4959 }
      { sexe = "m"; decennie = 1980; prenom = "Joël"; naissances = 4855 }
      { sexe = "m"; decennie = 1980; prenom = "Antony"; naissances = 4810 }
      { sexe = "m"; decennie = 1980; prenom = "Grégoire"; naissances = 4788 }
      { sexe = "m"; decennie = 1980; prenom = "Axel"; naissances = 4731 }
      { sexe = "m"; decennie = 1980; prenom = "Luc"; naissances = 4701 }
      { sexe = "m"; decennie = 1980; prenom = "Wilfried"; naissances = 4674 }
      { sexe = "m"; decennie = 1980; prenom = "Steeve"; naissances = 4638 }
      { sexe = "m"; decennie = 1980; prenom = "Alban"; naissances = 4610 }
      { sexe = "m"; decennie = 1980; prenom = "Dominique"; naissances = 4587 }
      { sexe = "m"; decennie = 1980; prenom = "Farid"; naissances = 4559 }
      { sexe = "m"; decennie = 1980; prenom = "Aymeric"; naissances = 4540 }
      { sexe = "m"; decennie = 1980; prenom = "Arthur"; naissances = 4519 }
      { sexe = "m"; decennie = 1980; prenom = "Regis"; naissances = 4456 }
      { sexe = "m"; decennie = 1980; prenom = "Mathias"; naissances = 4400 }
      { sexe = "m"; decennie = 1980; prenom = "Pierrick"; naissances = 4365 }
      { sexe = "m"; decennie = 1980; prenom = "Camille"; naissances = 4302 }
      { sexe = "m"; decennie = 1980; prenom = "Freddy"; naissances = 4263 }
      { sexe = "m"; decennie = 1980; prenom = "Gérald"; naissances = 4258 }
      { sexe = "m"; decennie = 1980; prenom = "Jean-Charles"; naissances = 4207 }
      { sexe = "m"; decennie = 1980; prenom = "Tristan"; naissances = 4188 }
      { sexe = "m"; decennie = 1980; prenom = "Vivien"; naissances = 4092 }
      { sexe = "m"; decennie = 1980; prenom = "Kévin"; naissances = 3992 }
      { sexe = "m"; decennie = 1980; prenom = "Lucas"; naissances = 3866 }
      { sexe = "m"; decennie = 1980; prenom = "Nabil"; naissances = 3833 }
      { sexe = "m"; decennie = 1980; prenom = "Ali"; naissances = 3798 }
      { sexe = "m"; decennie = 1980; prenom = "Willy"; naissances = 3724 }
      { sexe = "m"; decennie = 1980; prenom = "Kamel"; naissances = 3721 }
      { sexe = "m"; decennie = 1980; prenom = "Thibaud"; naissances = 3697 }
      { sexe = "m"; decennie = 1980; prenom = "Joseph"; naissances = 3589 }
      { sexe = "m"; decennie = 1980; prenom = "Jean-Michel"; naissances = 3558 }
      { sexe = "m"; decennie = 1980; prenom = "Rodolphe"; naissances = 3455 }
      { sexe = "m"; decennie = 1980; prenom = "Geoffroy"; naissances = 3443 }
      { sexe = "m"; decennie = 1980; prenom = "Robin"; naissances = 3435 }
      { sexe = "m"; decennie = 1980; prenom = "Michaël"; naissances = 3393 }
      { sexe = "m"; decennie = 1980; prenom = "Emilien"; naissances = 3327 }
      { sexe = "m"; decennie = 1980; prenom = "Ronan"; naissances = 3326 }
      { sexe = "m"; decennie = 1980; prenom = "Yves"; naissances = 3326 }
      { sexe = "m"; decennie = 1980; prenom = "John"; naissances = 3279 }
      { sexe = "m"; decennie = 1980; prenom = "Maxence"; naissances = 3273 }
      { sexe = "m"; decennie = 1980; prenom = "Jacques"; naissances = 3243 }
      { sexe = "m"; decennie = 1980; prenom = "Davy"; naissances = 3195 }
      { sexe = "m"; decennie = 1980; prenom = "Ahmed"; naissances = 3128 }
      { sexe = "m"; decennie = 1980; prenom = "Sofiane"; naissances = 3086 }
      { sexe = "m"; decennie = 1980; prenom = "Jean-Marie"; naissances = 2985 }
      { sexe = "m"; decennie = 1980; prenom = "Joffrey"; naissances = 2969 }
      { sexe = "m"; decennie = 1980; prenom = "Flavien"; naissances = 2939 }
      { sexe = "m"; decennie = 1980; prenom = "Alan"; naissances = 2934 }
      { sexe = "m"; decennie = 1980; prenom = "Mourad"; naissances = 2913 }
      { sexe = "m"; decennie = 1980; prenom = "Jean-Marc"; naissances = 2908 }
      { sexe = "m"; decennie = 1980; prenom = "Hakim"; naissances = 2873 }
      { sexe = "m"; decennie = 1980; prenom = "Dorian"; naissances = 2870 }
      { sexe = "m"; decennie = 1980; prenom = "Youssef"; naissances = 2826 }
      { sexe = "m"; decennie = 1980; prenom = "Claude"; naissances = 2824 }
      { sexe = "m"; decennie = 1980; prenom = "Amaury"; naissances = 2823 }
      { sexe = "m"; decennie = 1980; prenom = "Charly"; naissances = 2803 }
      { sexe = "m"; decennie = 1980; prenom = "Martial"; naissances = 2717 }
      { sexe = "m"; decennie = 1980; prenom = "José"; naissances = 2702 }
      { sexe = "m"; decennie = 1980; prenom = "Pierre-Yves"; naissances = 2683 }
      { sexe = "m"; decennie = 1980; prenom = "André"; naissances = 2622 }
      { sexe = "m"; decennie = 1980; prenom = "Gauthier"; naissances = 2596 }
      { sexe = "m"; decennie = 1980; prenom = "Timothée"; naissances = 2556 }
      { sexe = "m"; decennie = 1980; prenom = "Henri"; naissances = 2550 }
      { sexe = "m"; decennie = 1980; prenom = "Ghislain"; naissances = 2540 }
      { sexe = "m"; decennie = 1980; prenom = "Said"; naissances = 2528 }
      { sexe = "m"; decennie = 1980; prenom = "Emeric"; naissances = 2416 }
      { sexe = "m"; decennie = 1990; prenom = "Thomas"; naissances = 93176 }
      { sexe = "m"; decennie = 1990; prenom = "Alexandre"; naissances = 84360 }
      { sexe = "m"; decennie = 1990; prenom = "Kevin"; naissances = 83984 }
      { sexe = "m"; decennie = 1990; prenom = "Nicolas"; naissances = 83458 }
      { sexe = "m"; decennie = 1990; prenom = "Maxime"; naissances = 78020 }
      { sexe = "m"; decennie = 1990; prenom = "Julien"; naissances = 69512 }
      { sexe = "m"; decennie = 1990; prenom = "Quentin"; naissances = 62821 }
      { sexe = "m"; decennie = 1990; prenom = "Anthony"; naissances = 60279 }
      { sexe = "m"; decennie = 1990; prenom = "Romain"; naissances = 60170 }
      { sexe = "m"; decennie = 1990; prenom = "Florian"; naissances = 59544 }
      { sexe = "m"; decennie = 1990; prenom = "Antoine"; naissances = 57626 }
      { sexe = "m"; decennie = 1990; prenom = "Guillaume"; naissances = 56522 }
      { sexe = "m"; decennie = 1990; prenom = "Clément"; naissances = 52623 }
      { sexe = "m"; decennie = 1990; prenom = "Benjamin"; naissances = 50754 }
      { sexe = "m"; decennie = 1990; prenom = "Alexis"; naissances = 49199 }
      { sexe = "m"; decennie = 1990; prenom = "Pierre"; naissances = 48955 }
      { sexe = "m"; decennie = 1990; prenom = "Valentin"; naissances = 46740 }
      { sexe = "m"; decennie = 1990; prenom = "Jeremy"; naissances = 43298 }
      { sexe = "m"; decennie = 1990; prenom = "Vincent"; naissances = 42536 }
      { sexe = "m"; decennie = 1990; prenom = "Mathieu"; naissances = 38942 }
      { sexe = "m"; decennie = 1990; prenom = "Lucas"; naissances = 38553 }
      { sexe = "m"; decennie = 1990; prenom = "Adrien"; naissances = 38537 }
      { sexe = "m"; decennie = 1990; prenom = "Dylan"; naissances = 36802 }
      { sexe = "m"; decennie = 1990; prenom = "Jordan"; naissances = 34271 }
      { sexe = "m"; decennie = 1990; prenom = "Hugo"; naissances = 32177 }
      { sexe = "m"; decennie = 1990; prenom = "Jonathan"; naissances = 31958 }
      { sexe = "m"; decennie = 1990; prenom = "Sébastien"; naissances = 29429 }
      { sexe = "m"; decennie = 1990; prenom = "Damien"; naissances = 29417 }
      { sexe = "m"; decennie = 1990; prenom = "Paul"; naissances = 29202 }
      { sexe = "m"; decennie = 1990; prenom = "Théo"; naissances = 27678 }
      { sexe = "m"; decennie = 1990; prenom = "David"; naissances = 26632 }
      { sexe = "m"; decennie = 1990; prenom = "Baptiste"; naissances = 26117 }
      { sexe = "m"; decennie = 1990; prenom = "Loïc"; naissances = 26086 }
      { sexe = "m"; decennie = 1990; prenom = "Corentin"; naissances = 25112 }
      { sexe = "m"; decennie = 1990; prenom = "Louis"; naissances = 23340 }
      { sexe = "m"; decennie = 1990; prenom = "Mickael"; naissances = 22443 }
      { sexe = "m"; decennie = 1990; prenom = "Aurélien"; naissances = 22218 }
      { sexe = "m"; decennie = 1990; prenom = "Matthieu"; naissances = 22202 }
      { sexe = "m"; decennie = 1990; prenom = "Arthur"; naissances = 22037 }
      { sexe = "m"; decennie = 1990; prenom = "Rémi"; naissances = 22030 }
      { sexe = "m"; decennie = 1990; prenom = "Simon"; naissances = 21589 }
      { sexe = "m"; decennie = 1990; prenom = "Axel"; naissances = 20581 }
      { sexe = "m"; decennie = 1990; prenom = "Victor"; naissances = 20470 }
      { sexe = "m"; decennie = 1990; prenom = "Arnaud"; naissances = 20218 }
      { sexe = "m"; decennie = 1990; prenom = "Thibault"; naissances = 20162 }
      { sexe = "m"; decennie = 1990; prenom = "Bastien"; naissances = 19898 }
      { sexe = "m"; decennie = 1990; prenom = "Raphaël"; naissances = 18360 }
      { sexe = "m"; decennie = 1990; prenom = "Christopher"; naissances = 17921 }
      { sexe = "m"; decennie = 1990; prenom = "Fabien"; naissances = 17896 }
      { sexe = "m"; decennie = 1990; prenom = "Florent"; naissances = 17727 }
      { sexe = "m"; decennie = 1990; prenom = "Benoît"; naissances = 17628 }
      { sexe = "m"; decennie = 1990; prenom = "Steven"; naissances = 17349 }
      { sexe = "m"; decennie = 1990; prenom = "Léo"; naissances = 16322 }
      { sexe = "m"; decennie = 1990; prenom = "Cedric"; naissances = 16319 }
      { sexe = "m"; decennie = 1990; prenom = "François"; naissances = 16105 }
      { sexe = "m"; decennie = 1990; prenom = "Gaëtan"; naissances = 15833 }
      { sexe = "m"; decennie = 1990; prenom = "Nathan"; naissances = 15378 }
      { sexe = "m"; decennie = 1990; prenom = "Ludovic"; naissances = 15371 }
      { sexe = "m"; decennie = 1990; prenom = "Robin"; naissances = 15059 }
      { sexe = "m"; decennie = 1990; prenom = "Cyril"; naissances = 13712 }
      { sexe = "m"; decennie = 1990; prenom = "Samuel"; naissances = 13699 }
      { sexe = "m"; decennie = 1990; prenom = "Yann"; naissances = 13545 }
      { sexe = "m"; decennie = 1990; prenom = "Jean"; naissances = 13331 }
      { sexe = "m"; decennie = 1990; prenom = "Charles"; naissances = 13246 }
      { sexe = "m"; decennie = 1990; prenom = "Sylvain"; naissances = 13242 }
      { sexe = "m"; decennie = 1990; prenom = "Mehdi"; naissances = 13077 }
      { sexe = "m"; decennie = 1990; prenom = "Thibaut"; naissances = 12974 }
      { sexe = "m"; decennie = 1990; prenom = "William"; naissances = 12932 }
      { sexe = "m"; decennie = 1990; prenom = "Dimitri"; naissances = 12922 }
      { sexe = "m"; decennie = 1990; prenom = "Rémy"; naissances = 12732 }
      { sexe = "m"; decennie = 1990; prenom = "Mohamed"; naissances = 12438 }
      { sexe = "m"; decennie = 1990; prenom = "Jerome"; naissances = 12375 }
      { sexe = "m"; decennie = 1990; prenom = "Tristan"; naissances = 12343 }
      { sexe = "m"; decennie = 1990; prenom = "Maxence"; naissances = 11998 }
      { sexe = "m"; decennie = 1990; prenom = "Geoffrey"; naissances = 11990 }
      { sexe = "m"; decennie = 1990; prenom = "Martin"; naissances = 11642 }
      { sexe = "m"; decennie = 1990; prenom = "Yoann"; naissances = 11641 }
      { sexe = "m"; decennie = 1990; prenom = "Christophe"; naissances = 11631 }
      { sexe = "m"; decennie = 1990; prenom = "Olivier"; naissances = 11588 }
      { sexe = "m"; decennie = 1990; prenom = "Jimmy"; naissances = 11375 }
      { sexe = "m"; decennie = 1990; prenom = "Tom"; naissances = 11270 }
      { sexe = "m"; decennie = 1990; prenom = "Jason"; naissances = 11121 }
      { sexe = "m"; decennie = 1990; prenom = "Stéphane"; naissances = 10984 }
      { sexe = "m"; decennie = 1990; prenom = "Bryan"; naissances = 10873 }
      { sexe = "m"; decennie = 1990; prenom = "Jean-Baptiste"; naissances = 10840 }
      { sexe = "m"; decennie = 1990; prenom = "Tony"; naissances = 10717 }
      { sexe = "m"; decennie = 1990; prenom = "Dorian"; naissances = 10600 }
      { sexe = "m"; decennie = 1990; prenom = "Morgan"; naissances = 10403 }
      { sexe = "m"; decennie = 1990; prenom = "Etienne"; naissances = 10199 }
      { sexe = "m"; decennie = 1990; prenom = "Gabriel"; naissances = 9776 }
      { sexe = "m"; decennie = 1990; prenom = "Xavier"; naissances = 9693 }
      { sexe = "m"; decennie = 1990; prenom = "Marc"; naissances = 9565 }
      { sexe = "m"; decennie = 1990; prenom = "Enzo"; naissances = 9323 }
      { sexe = "m"; decennie = 1990; prenom = "Gregory"; naissances = 9243 }
      { sexe = "m"; decennie = 1990; prenom = "Frédéric"; naissances = 9201 }
      { sexe = "m"; decennie = 1990; prenom = "Jules"; naissances = 8608 }
      { sexe = "m"; decennie = 1990; prenom = "Franck"; naissances = 8284 }
      { sexe = "m"; decennie = 1990; prenom = "Kévin"; naissances = 8167 }
      { sexe = "m"; decennie = 1990; prenom = "Erwan"; naissances = 8094 }
      { sexe = "m"; decennie = 1990; prenom = "Emmanuel"; naissances = 8075 }
      { sexe = "m"; decennie = 1990; prenom = "Laurent"; naissances = 7960 }
      { sexe = "m"; decennie = 1990; prenom = "Michael"; naissances = 7957 }
      { sexe = "m"; decennie = 1990; prenom = "Mathias"; naissances = 7688 }
      { sexe = "m"; decennie = 1990; prenom = "Allan"; naissances = 7627 }
      { sexe = "m"; decennie = 1990; prenom = "Yohan"; naissances = 7612 }
      { sexe = "m"; decennie = 1990; prenom = "Tanguy"; naissances = 7399 }
      { sexe = "m"; decennie = 1990; prenom = "Johan"; naissances = 7392 }
      { sexe = "m"; decennie = 1990; prenom = "Brice"; naissances = 7303 }
      { sexe = "m"; decennie = 1990; prenom = "Sofiane"; naissances = 7293 }
      { sexe = "m"; decennie = 1990; prenom = "Teddy"; naissances = 7051 }
      { sexe = "m"; decennie = 1990; prenom = "Alex"; naissances = 7026 }
      { sexe = "m"; decennie = 1990; prenom = "Aymeric"; naissances = 7004 }
      { sexe = "m"; decennie = 1990; prenom = "Grégoire"; naissances = 6893 }
      { sexe = "m"; decennie = 1990; prenom = "Alan"; naissances = 6743 }
      { sexe = "m"; decennie = 1990; prenom = "Brandon"; naissances = 6741 }
      { sexe = "m"; decennie = 1990; prenom = "Jérémy"; naissances = 6341 }
      { sexe = "m"; decennie = 1990; prenom = "Yanis"; naissances = 6332 }
      { sexe = "m"; decennie = 1990; prenom = "Joris"; naissances = 6286 }
      { sexe = "m"; decennie = 1990; prenom = "Jeremie"; naissances = 6282 }
      { sexe = "m"; decennie = 1990; prenom = "Marvin"; naissances = 6102 }
      { sexe = "m"; decennie = 1990; prenom = "Philippe"; naissances = 5940 }
      { sexe = "m"; decennie = 1990; prenom = "Edouard"; naissances = 5764 }
      { sexe = "m"; decennie = 1990; prenom = "Antonin"; naissances = 5713 }
      { sexe = "m"; decennie = 1990; prenom = "Thibaud"; naissances = 5650 }
      { sexe = "m"; decennie = 1990; prenom = "Jessy"; naissances = 5592 }
      { sexe = "m"; decennie = 1990; prenom = "Karim"; naissances = 5558 }
      { sexe = "m"; decennie = 1990; prenom = "Killian"; naissances = 5551 }
      { sexe = "m"; decennie = 1990; prenom = "Yohann"; naissances = 5455 }
      { sexe = "m"; decennie = 1990; prenom = "Luc"; naissances = 5369 }
      { sexe = "m"; decennie = 1990; prenom = "Charly"; naissances = 5363 }
      { sexe = "m"; decennie = 1990; prenom = "Camille"; naissances = 5316 }
      { sexe = "m"; decennie = 1990; prenom = "Yannick"; naissances = 5168 }
      { sexe = "m"; decennie = 1990; prenom = "Yoan"; naissances = 5047 }
      { sexe = "m"; decennie = 1990; prenom = "Gauthier"; naissances = 5038 }
      { sexe = "m"; decennie = 1990; prenom = "Eric"; naissances = 5017 }
      { sexe = "m"; decennie = 1990; prenom = "Gael"; naissances = 4875 }
      { sexe = "m"; decennie = 1990; prenom = "Mathis"; naissances = 4805 }
      { sexe = "m"; decennie = 1990; prenom = "Timothée"; naissances = 4804 }
      { sexe = "m"; decennie = 1990; prenom = "Bruno"; naissances = 4679 }
      { sexe = "m"; decennie = 1990; prenom = "Matthias"; naissances = 4675 }
      { sexe = "m"; decennie = 1990; prenom = "Rudy"; naissances = 4631 }
      { sexe = "m"; decennie = 1990; prenom = "Flavien"; naissances = 4630 }
      { sexe = "m"; decennie = 1990; prenom = "Steve"; naissances = 4503 }
      { sexe = "m"; decennie = 1990; prenom = "Yassine"; naissances = 4409 }
      { sexe = "m"; decennie = 1990; prenom = "Félix"; naissances = 4353 }
      { sexe = "m"; decennie = 1990; prenom = "Daniel"; naissances = 4230 }
      { sexe = "m"; decennie = 1990; prenom = "Amaury"; naissances = 4209 }
      { sexe = "m"; decennie = 1990; prenom = "Pierrick"; naissances = 4206 }
      { sexe = "m"; decennie = 1990; prenom = "Samy"; naissances = 4158 }
      { sexe = "m"; decennie = 1990; prenom = "Adam"; naissances = 4128 }
      { sexe = "m"; decennie = 1990; prenom = "Sacha"; naissances = 4123 }
      { sexe = "m"; decennie = 1990; prenom = "Alban"; naissances = 4106 }
      { sexe = "m"; decennie = 1990; prenom = "Brian"; naissances = 4045 }
      { sexe = "m"; decennie = 1990; prenom = "Eddy"; naissances = 4016 }
      { sexe = "m"; decennie = 1990; prenom = "Lilian"; naissances = 3959 }
      { sexe = "m"; decennie = 1990; prenom = "Emilien"; naissances = 3943 }
      { sexe = "m"; decennie = 1990; prenom = "Yannis"; naissances = 3894 }
      { sexe = "m"; decennie = 1990; prenom = "Mael"; naissances = 3862 }
      { sexe = "m"; decennie = 1990; prenom = "Michel"; naissances = 3842 }
      { sexe = "m"; decennie = 1990; prenom = "Antony"; naissances = 3813 }
      { sexe = "m"; decennie = 1990; prenom = "Andy"; naissances = 3728 }
      { sexe = "m"; decennie = 1990; prenom = "Yacine"; naissances = 3607 }
      { sexe = "m"; decennie = 1990; prenom = "Joseph"; naissances = 3551 }
      { sexe = "m"; decennie = 1990; prenom = "Samir"; naissances = 3514 }
      { sexe = "m"; decennie = 1990; prenom = "Boris"; naissances = 3349 }
      { sexe = "m"; decennie = 1990; prenom = "Julian"; naissances = 3333 }
      { sexe = "m"; decennie = 1990; prenom = "Mike"; naissances = 3333 }
      { sexe = "m"; decennie = 1990; prenom = "Joffrey"; naissances = 3314 }
      { sexe = "m"; decennie = 1990; prenom = "Richard"; naissances = 3222 }
      { sexe = "m"; decennie = 1990; prenom = "Amine"; naissances = 3212 }
      { sexe = "m"; decennie = 1990; prenom = "Augustin"; naissances = 3166 }
      { sexe = "m"; decennie = 1990; prenom = "Manuel"; naissances = 3161 }
      { sexe = "m"; decennie = 1990; prenom = "Lionel"; naissances = 3114 }
      { sexe = "m"; decennie = 1990; prenom = "Ronan"; naissances = 3110 }
      { sexe = "m"; decennie = 1990; prenom = "Sami"; naissances = 3103 }
      { sexe = "m"; decennie = 1990; prenom = "Nabil"; naissances = 3099 }
      { sexe = "m"; decennie = 1990; prenom = "Gautier"; naissances = 3080 }
      { sexe = "m"; decennie = 1990; prenom = "Melvin"; naissances = 3042 }
      { sexe = "m"; decennie = 1990; prenom = "Bilal"; naissances = 3034 }
      { sexe = "m"; decennie = 1990; prenom = "Maximilien"; naissances = 3024 }
      { sexe = "m"; decennie = 1990; prenom = "Ryan"; naissances = 3022 }
      { sexe = "m"; decennie = 1990; prenom = "Renaud"; naissances = 3021 }
      { sexe = "m"; decennie = 1990; prenom = "Justin"; naissances = 2965 }
      { sexe = "m"; decennie = 1990; prenom = "Denis"; naissances = 2960 }
      { sexe = "m"; decennie = 1990; prenom = "Elie"; naissances = 2933 }
      { sexe = "m"; decennie = 1990; prenom = "Nassim"; naissances = 2898 }
      { sexe = "m"; decennie = 1990; prenom = "Ali"; naissances = 2849 }
      { sexe = "m"; decennie = 1990; prenom = "Mickaël"; naissances = 2848 }
      { sexe = "m"; decennie = 1990; prenom = "Kenny"; naissances = 2845 }
      { sexe = "m"; decennie = 1990; prenom = "Johann"; naissances = 2840 }
      { sexe = "m"; decennie = 1990; prenom = "Donovan"; naissances = 2835 }
      { sexe = "m"; decennie = 1990; prenom = "Fabrice"; naissances = 2830 }
      { sexe = "m"; decennie = 1990; prenom = "Youssef"; naissances = 2800 }
      { sexe = "m"; decennie = 1990; prenom = "Johnny"; naissances = 2799 }
      { sexe = "m"; decennie = 1990; prenom = "Wilfried"; naissances = 2789 }
      { sexe = "m"; decennie = 1990; prenom = "Vivien"; naissances = 2774 }
      { sexe = "m"; decennie = 1990; prenom = "Rayan"; naissances = 2765 }
      { sexe = "m"; decennie = 1990; prenom = "Walid"; naissances = 2750 }
      { sexe = "m"; decennie = 1990; prenom = "Emeric"; naissances = 2731 }
      { sexe = "m"; decennie = 1990; prenom = "Stanislas"; naissances = 2704 }
      { sexe = "m"; decennie = 2000; prenom = "Lucas"; naissances = 77773 }
      { sexe = "m"; decennie = 2000; prenom = "Enzo"; naissances = 67437 }
      { sexe = "m"; decennie = 2000; prenom = "Thomas"; naissances = 65624 }
      { sexe = "m"; decennie = 2000; prenom = "Théo"; naissances = 63033 }
      { sexe = "m"; decennie = 2000; prenom = "Hugo"; naissances = 59706 }
      { sexe = "m"; decennie = 2000; prenom = "Nathan"; naissances = 51493 }
      { sexe = "m"; decennie = 2000; prenom = "Maxime"; naissances = 49555 }
      { sexe = "m"; decennie = 2000; prenom = "Mathis"; naissances = 47522 }
      { sexe = "m"; decennie = 2000; prenom = "Clément"; naissances = 45370 }
      { sexe = "m"; decennie = 2000; prenom = "Louis"; naissances = 44872 }
      { sexe = "m"; decennie = 2000; prenom = "Alexandre"; naissances = 43176 }
      { sexe = "m"; decennie = 2000; prenom = "Antoine"; naissances = 42596 }
      { sexe = "m"; decennie = 2000; prenom = "Tom"; naissances = 40719 }
      { sexe = "m"; decennie = 2000; prenom = "Léo"; naissances = 39809 }
      { sexe = "m"; decennie = 2000; prenom = "Quentin"; naissances = 39761 }
      { sexe = "m"; decennie = 2000; prenom = "Alexis"; naissances = 39665 }
      { sexe = "m"; decennie = 2000; prenom = "Paul"; naissances = 36270 }
      { sexe = "m"; decennie = 2000; prenom = "Arthur"; naissances = 35319 }
      { sexe = "m"; decennie = 2000; prenom = "Baptiste"; naissances = 34981 }
      { sexe = "m"; decennie = 2000; prenom = "Romain"; naissances = 34655 }
      { sexe = "m"; decennie = 2000; prenom = "Nicolas"; naissances = 33745 }
      { sexe = "m"; decennie = 2000; prenom = "Raphaël"; naissances = 32058 }
      { sexe = "m"; decennie = 2000; prenom = "Valentin"; naissances = 31708 }
      { sexe = "m"; decennie = 2000; prenom = "Jules"; naissances = 31274 }
      { sexe = "m"; decennie = 2000; prenom = "Yanis"; naissances = 30779 }
      { sexe = "m"; decennie = 2000; prenom = "Mathéo"; naissances = 30697 }
      { sexe = "m"; decennie = 2000; prenom = "Axel"; naissances = 29937 }
      { sexe = "m"; decennie = 2000; prenom = "Julien"; naissances = 28181 }
      { sexe = "m"; decennie = 2000; prenom = "Gabriel"; naissances = 27857 }
      { sexe = "m"; decennie = 2000; prenom = "Noah"; naissances = 25243 }
      { sexe = "m"; decennie = 2000; prenom = "Benjamin"; naissances = 25208 }
      { sexe = "m"; decennie = 2000; prenom = "Pierre"; naissances = 25077 }
      { sexe = "m"; decennie = 2000; prenom = "Mattéo"; naissances = 24800 }
      { sexe = "m"; decennie = 2000; prenom = "Maxence"; naissances = 24290 }
      { sexe = "m"; decennie = 2000; prenom = "Adrien"; naissances = 23669 }
      { sexe = "m"; decennie = 2000; prenom = "Dylan"; naissances = 23226 }
      { sexe = "m"; decennie = 2000; prenom = "Florian"; naissances = 23008 }
      { sexe = "m"; decennie = 2000; prenom = "Ethan"; naissances = 22494 }
      { sexe = "m"; decennie = 2000; prenom = "Mathieu"; naissances = 22063 }
      { sexe = "m"; decennie = 2000; prenom = "Victor"; naissances = 21708 }
      { sexe = "m"; decennie = 2000; prenom = "Corentin"; naissances = 21286 }
      { sexe = "m"; decennie = 2000; prenom = "Evan"; naissances = 20977 }
      { sexe = "m"; decennie = 2000; prenom = "Anthony"; naissances = 20077 }
      { sexe = "m"; decennie = 2000; prenom = "Kevin"; naissances = 20022 }
      { sexe = "m"; decennie = 2000; prenom = "Adam"; naissances = 19134 }
      { sexe = "m"; decennie = 2000; prenom = "Samuel"; naissances = 18815 }
      { sexe = "m"; decennie = 2000; prenom = "Mael"; naissances = 18581 }
      { sexe = "m"; decennie = 2000; prenom = "Simon"; naissances = 18458 }
      { sexe = "m"; decennie = 2000; prenom = "Mohamed"; naissances = 18096 }
      { sexe = "m"; decennie = 2000; prenom = "Guillaume"; naissances = 16748 }
      { sexe = "m"; decennie = 2000; prenom = "Rayan"; naissances = 16380 }
      { sexe = "m"; decennie = 2000; prenom = "Martin"; naissances = 16004 }
      { sexe = "m"; decennie = 2000; prenom = "Bastien"; naissances = 15676 }
      { sexe = "m"; decennie = 2000; prenom = "Tristan"; naissances = 15653 }
      { sexe = "m"; decennie = 2000; prenom = "Kylian"; naissances = 15421 }
      { sexe = "m"; decennie = 2000; prenom = "Sacha"; naissances = 15292 }
      { sexe = "m"; decennie = 2000; prenom = "Noa"; naissances = 15213 }
      { sexe = "m"; decennie = 2000; prenom = "Dorian"; naissances = 15169 }
      { sexe = "m"; decennie = 2000; prenom = "Noe"; naissances = 14745 }
      { sexe = "m"; decennie = 2000; prenom = "Killian"; naissances = 14181 }
      { sexe = "m"; decennie = 2000; prenom = "Esteban"; naissances = 13770 }
      { sexe = "m"; decennie = 2000; prenom = "Vincent"; naissances = 13466 }
      { sexe = "m"; decennie = 2000; prenom = "Thibault"; naissances = 13252 }
      { sexe = "m"; decennie = 2000; prenom = "Nolan"; naissances = 13235 }
      { sexe = "m"; decennie = 2000; prenom = "Robin"; naissances = 13113 }
      { sexe = "m"; decennie = 2000; prenom = "Mathys"; naissances = 13026 }
      { sexe = "m"; decennie = 2000; prenom = "Jeremy"; naissances = 13013 }
      { sexe = "m"; decennie = 2000; prenom = "Mathias"; naissances = 12861 }
      { sexe = "m"; decennie = 2000; prenom = "Erwan"; naissances = 12585 }
      { sexe = "m"; decennie = 2000; prenom = "Loïc"; naissances = 12387 }
      { sexe = "m"; decennie = 2000; prenom = "Rémi"; naissances = 12266 }
      { sexe = "m"; decennie = 2000; prenom = "Matthieu"; naissances = 12053 }
      { sexe = "m"; decennie = 2000; prenom = "Antonin"; naissances = 11970 }
      { sexe = "m"; decennie = 2000; prenom = "Matéo"; naissances = 11747 }
      { sexe = "m"; decennie = 2000; prenom = "Damien"; naissances = 11715 }
      { sexe = "m"; decennie = 2000; prenom = "Titouan"; naissances = 11661 }
      { sexe = "m"; decennie = 2000; prenom = "Gabin"; naissances = 11606 }
      { sexe = "m"; decennie = 2000; prenom = "Jordan"; naissances = 11279 }
      { sexe = "m"; decennie = 2000; prenom = "William"; naissances = 10804 }
      { sexe = "m"; decennie = 2000; prenom = "Lorenzo"; naissances = 10596 }
      { sexe = "m"; decennie = 2000; prenom = "Lilian"; naissances = 10582 }
      { sexe = "m"; decennie = 2000; prenom = "Aurélien"; naissances = 10401 }
      { sexe = "m"; decennie = 2000; prenom = "David"; naissances = 10378 }
      { sexe = "m"; decennie = 2000; prenom = "Yann"; naissances = 10349 }
      { sexe = "m"; decennie = 2000; prenom = "Mehdi"; naissances = 10348 }
      { sexe = "m"; decennie = 2000; prenom = "Ryan"; naissances = 9898 }
      { sexe = "m"; decennie = 2000; prenom = "Lenny"; naissances = 9738 }
      { sexe = "m"; decennie = 2000; prenom = "Bryan"; naissances = 9600 }
      { sexe = "m"; decennie = 2000; prenom = "Timeo"; naissances = 9564 }
      { sexe = "m"; decennie = 2000; prenom = "Amine"; naissances = 9308 }
      { sexe = "m"; decennie = 2000; prenom = "Jean"; naissances = 9119 }
      { sexe = "m"; decennie = 2000; prenom = "Charles"; naissances = 8633 }
      { sexe = "m"; decennie = 2000; prenom = "Thibaut"; naissances = 8387 }
      { sexe = "m"; decennie = 2000; prenom = "Sofiane"; naissances = 8318 }
      { sexe = "m"; decennie = 2000; prenom = "Gaëtan"; naissances = 8046 }
      { sexe = "m"; decennie = 2000; prenom = "Marius"; naissances = 7779 }
      { sexe = "m"; decennie = 2000; prenom = "Alex"; naissances = 7725 }
      { sexe = "m"; decennie = 2000; prenom = "Tony"; naissances = 7636 }
      { sexe = "m"; decennie = 2000; prenom = "Jonathan"; naissances = 7616 }
      { sexe = "m"; decennie = 2000; prenom = "Eliott"; naissances = 7585 }
      { sexe = "m"; decennie = 2000; prenom = "Luca"; naissances = 6883 }
      { sexe = "m"; decennie = 2000; prenom = "Arnaud"; naissances = 6718 }
      { sexe = "m"; decennie = 2000; prenom = "Alan"; naissances = 6697 }
      { sexe = "m"; decennie = 2000; prenom = "Sébastien"; naissances = 6547 }
      { sexe = "m"; decennie = 2000; prenom = "Ilyes"; naissances = 6500 }
      { sexe = "m"; decennie = 2000; prenom = "Diego"; naissances = 6383 }
      { sexe = "m"; decennie = 2000; prenom = "Yassine"; naissances = 6350 }
      { sexe = "m"; decennie = 2000; prenom = "Augustin"; naissances = 6157 }
      { sexe = "m"; decennie = 2000; prenom = "Bilal"; naissances = 6098 }
      { sexe = "m"; decennie = 2000; prenom = "Oscar"; naissances = 6047 }
      { sexe = "m"; decennie = 2000; prenom = "Kilian"; naissances = 6017 }
      { sexe = "m"; decennie = 2000; prenom = "Steven"; naissances = 5994 }
      { sexe = "m"; decennie = 2000; prenom = "Allan"; naissances = 5962 }
      { sexe = "m"; decennie = 2000; prenom = "Logan"; naissances = 5908 }
      { sexe = "m"; decennie = 2000; prenom = "Etienne"; naissances = 5906 }
      { sexe = "m"; decennie = 2000; prenom = "Tanguy"; naissances = 5859 }
      { sexe = "m"; decennie = 2000; prenom = "Benoît"; naissances = 5845 }
      { sexe = "m"; decennie = 2000; prenom = "Gaspard"; naissances = 5830 }
      { sexe = "m"; decennie = 2000; prenom = "Mickael"; naissances = 5828 }
      { sexe = "m"; decennie = 2000; prenom = "Loris"; naissances = 5799 }
      { sexe = "m"; decennie = 2000; prenom = "Dimitri"; naissances = 5784 }
      { sexe = "m"; decennie = 2000; prenom = "Ilan"; naissances = 5732 }
      { sexe = "m"; decennie = 2000; prenom = "Rémy"; naissances = 5718 }
      { sexe = "m"; decennie = 2000; prenom = "Kyllian"; naissances = 5694 }
      { sexe = "m"; decennie = 2000; prenom = "Timothée"; naissances = 5683 }
      { sexe = "m"; decennie = 2000; prenom = "Jason"; naissances = 5666 }
      { sexe = "m"; decennie = 2000; prenom = "Liam"; naissances = 5652 }
      { sexe = "m"; decennie = 2000; prenom = "Yannis"; naissances = 5621 }
      { sexe = "m"; decennie = 2000; prenom = "Fabien"; naissances = 5590 }
      { sexe = "m"; decennie = 2000; prenom = "Alban"; naissances = 5589 }
      { sexe = "m"; decennie = 2000; prenom = "Emilien"; naissances = 5571 }
      { sexe = "m"; decennie = 2000; prenom = "Nassim"; naissances = 5567 }
      { sexe = "m"; decennie = 2000; prenom = "Elias"; naissances = 5559 }
      { sexe = "m"; decennie = 2000; prenom = "Aymeric"; naissances = 5556 }
      { sexe = "m"; decennie = 2000; prenom = "Aaron"; naissances = 5544 }
      { sexe = "m"; decennie = 2000; prenom = "Johan"; naissances = 5527 }
      { sexe = "m"; decennie = 2000; prenom = "Malo"; naissances = 5510 }
      { sexe = "m"; decennie = 2000; prenom = "Morgan"; naissances = 5488 }
      { sexe = "m"; decennie = 2000; prenom = "Noam"; naissances = 5389 }
      { sexe = "m"; decennie = 2000; prenom = "Rayane"; naissances = 5350 }
      { sexe = "m"; decennie = 2000; prenom = "Melvin"; naissances = 5311 }
      { sexe = "m"; decennie = 2000; prenom = "Ayoub"; naissances = 5289 }
      { sexe = "m"; decennie = 2000; prenom = "Timothe"; naissances = 5236 }
      { sexe = "m"; decennie = 2000; prenom = "Matthias"; naissances = 5221 }
      { sexe = "m"; decennie = 2000; prenom = "François"; naissances = 5206 }
      { sexe = "m"; decennie = 2000; prenom = "Samy"; naissances = 5166 }
      { sexe = "m"; decennie = 2000; prenom = "Florent"; naissances = 5159 }
      { sexe = "m"; decennie = 2000; prenom = "Rafael"; naissances = 5157 }
      { sexe = "m"; decennie = 2000; prenom = "Younes"; naissances = 5098 }
      { sexe = "m"; decennie = 2000; prenom = "Eliot"; naissances = 4980 }
      { sexe = "m"; decennie = 2000; prenom = "Lukas"; naissances = 4969 }
      { sexe = "m"; decennie = 2000; prenom = "Julian"; naissances = 4908 }
      { sexe = "m"; decennie = 2000; prenom = "Yohan"; naissances = 4888 }
      { sexe = "m"; decennie = 2000; prenom = "Grégoire"; naissances = 4853 }
      { sexe = "m"; decennie = 2000; prenom = "Joris"; naissances = 4811 }
      { sexe = "m"; decennie = 2000; prenom = "Téo"; naissances = 4774 }
      { sexe = "m"; decennie = 2000; prenom = "Gael"; naissances = 4744 }
      { sexe = "m"; decennie = 2000; prenom = "Amaury"; naissances = 4725 }
      { sexe = "m"; decennie = 2000; prenom = "Gregory"; naissances = 4723 }
      { sexe = "m"; decennie = 2000; prenom = "Ewen"; naissances = 4665 }
      { sexe = "m"; decennie = 2000; prenom = "Yacine"; naissances = 4616 }
      { sexe = "m"; decennie = 2000; prenom = "Ismael"; naissances = 4615 }
      { sexe = "m"; decennie = 2000; prenom = "Hamza"; naissances = 4613 }
      { sexe = "m"; decennie = 2000; prenom = "Ruben"; naissances = 4567 }
      { sexe = "m"; decennie = 2000; prenom = "Ugo"; naissances = 4544 }
      { sexe = "m"; decennie = 2000; prenom = "Kenzo"; naissances = 4531 }
      { sexe = "m"; decennie = 2000; prenom = "Pablo"; naissances = 4529 }
      { sexe = "m"; decennie = 2000; prenom = "Justin"; naissances = 4524 }
      { sexe = "m"; decennie = 2000; prenom = "Félix"; naissances = 4523 }
      { sexe = "m"; decennie = 2000; prenom = "Joseph"; naissances = 4492 }
      { sexe = "m"; decennie = 2000; prenom = "Cedric"; naissances = 4490 }
      { sexe = "m"; decennie = 2000; prenom = "Christopher"; naissances = 4485 }
      { sexe = "m"; decennie = 2000; prenom = "Gauthier"; naissances = 4445 }
      { sexe = "m"; decennie = 2000; prenom = "Loan"; naissances = 4410 }
      { sexe = "m"; decennie = 2000; prenom = "Luka"; naissances = 4368 }
      { sexe = "m"; decennie = 2000; prenom = "Walid"; naissances = 4344 }
      { sexe = "m"; decennie = 2000; prenom = "Camille"; naissances = 4305 }
      { sexe = "m"; decennie = 2000; prenom = "Emmanuel"; naissances = 4270 }
      { sexe = "m"; decennie = 2000; prenom = "Nael"; naissances = 4260 }
      { sexe = "m"; decennie = 2000; prenom = "Yoann"; naissances = 4245 }
      { sexe = "m"; decennie = 2000; prenom = "Jimmy"; naissances = 4134 }
      { sexe = "m"; decennie = 2000; prenom = "Sami"; naissances = 4126 }
      { sexe = "m"; decennie = 2000; prenom = "Elouan"; naissances = 4120 }
      { sexe = "m"; decennie = 2000; prenom = "Brice"; naissances = 4088 }
      { sexe = "m"; decennie = 2000; prenom = "Marc"; naissances = 4088 }
      { sexe = "m"; decennie = 2000; prenom = "Matis"; naissances = 4078 }
      { sexe = "m"; decennie = 2000; prenom = "Ludovic"; naissances = 4039 }
      { sexe = "m"; decennie = 2000; prenom = "Nino"; naissances = 4013 }
      { sexe = "m"; decennie = 2000; prenom = "Ewan"; naissances = 4001 }
      { sexe = "m"; decennie = 2000; prenom = "Ali"; naissances = 3997 }
      { sexe = "m"; decennie = 2000; prenom = "Louka"; naissances = 3908 }
      { sexe = "m"; decennie = 2000; prenom = "Ibrahim"; naissances = 3891 }
      { sexe = "m"; decennie = 2000; prenom = "Luc"; naissances = 3843 }
      { sexe = "m"; decennie = 2000; prenom = "Cyril"; naissances = 3794 }
      { sexe = "m"; decennie = 2000; prenom = "Joshua"; naissances = 3781 }
      { sexe = "m"; decennie = 2000; prenom = "Andy"; naissances = 3752 }
      { sexe = "m"; decennie = 2000; prenom = "Wassim"; naissances = 3745 }
      { sexe = "m"; decennie = 2000; prenom = "Nathanael"; naissances = 3734 }
      { sexe = "m"; decennie = 2000; prenom = "Olivier"; naissances = 3721 }
      { sexe = "m"; decennie = 2000; prenom = "Yoan"; naissances = 3659 }
      { sexe = "m"; decennie = 2010; prenom = "Gabriel"; naissances = 52341 }
      { sexe = "m"; decennie = 2010; prenom = "Lucas"; naissances = 51285 }
      { sexe = "m"; decennie = 2010; prenom = "Louis"; naissances = 46881 }
      { sexe = "m"; decennie = 2010; prenom = "Nathan"; naissances = 43754 }
      { sexe = "m"; decennie = 2010; prenom = "Jules"; naissances = 43564 }
      { sexe = "m"; decennie = 2010; prenom = "Hugo"; naissances = 43454 }
      { sexe = "m"; decennie = 2010; prenom = "Adam"; naissances = 41136 }
      { sexe = "m"; decennie = 2010; prenom = "Léo"; naissances = 40853 }
      { sexe = "m"; decennie = 2010; prenom = "Ethan"; naissances = 39729 }
      { sexe = "m"; decennie = 2010; prenom = "Arthur"; naissances = 38875 }
      { sexe = "m"; decennie = 2010; prenom = "Raphaël"; naissances = 38224 }
      { sexe = "m"; decennie = 2010; prenom = "Enzo"; naissances = 36329 }
      { sexe = "m"; decennie = 2010; prenom = "Paul"; naissances = 32199 }
      { sexe = "m"; decennie = 2010; prenom = "Nolan"; naissances = 31971 }
      { sexe = "m"; decennie = 2010; prenom = "Tom"; naissances = 31428 }
      { sexe = "m"; decennie = 2010; prenom = "Noah"; naissances = 30369 }
      { sexe = "m"; decennie = 2010; prenom = "Sacha"; naissances = 29373 }
      { sexe = "m"; decennie = 2010; prenom = "Mathis"; naissances = 29264 }
      { sexe = "m"; decennie = 2010; prenom = "Liam"; naissances = 27505 }
      { sexe = "m"; decennie = 2010; prenom = "Maël"; naissances = 26813 }
      { sexe = "m"; decennie = 2010; prenom = "Théo"; naissances = 26009 }
      { sexe = "m"; decennie = 2010; prenom = "Gabin"; naissances = 25808 }
      { sexe = "m"; decennie = 2010; prenom = "Timéo"; naissances = 25112 }
      { sexe = "m"; decennie = 2010; prenom = "Mohamed"; naissances = 25111 }
      { sexe = "m"; decennie = 2010; prenom = "Maxime"; naissances = 23823 }
      { sexe = "m"; decennie = 2010; prenom = "Axel"; naissances = 23708 }
      { sexe = "m"; decennie = 2010; prenom = "Antoine"; naissances = 22547 }
      { sexe = "m"; decennie = 2010; prenom = "Yanis"; naissances = 22253 }
      { sexe = "m"; decennie = 2010; prenom = "Clément"; naissances = 21597 }
      { sexe = "m"; decennie = 2010; prenom = "Baptiste"; naissances = 21212 }
      { sexe = "m"; decennie = 2010; prenom = "Aaron"; naissances = 20669 }
      { sexe = "m"; decennie = 2010; prenom = "Thomas"; naissances = 20485 }
      { sexe = "m"; decennie = 2010; prenom = "Victor"; naissances = 19762 }
      { sexe = "m"; decennie = 2010; prenom = "Evan"; naissances = 19702 }
      { sexe = "m"; decennie = 2010; prenom = "Rayan"; naissances = 19015 }
      { sexe = "m"; decennie = 2010; prenom = "Valentin"; naissances = 19005 }
      { sexe = "m"; decennie = 2010; prenom = "Maxence"; naissances = 18959 }
      { sexe = "m"; decennie = 2010; prenom = "Martin"; naissances = 18689 }
      { sexe = "m"; decennie = 2010; prenom = "Noé"; naissances = 18487 }
      { sexe = "m"; decennie = 2010; prenom = "Mathéo"; naissances = 18477 }
      { sexe = "m"; decennie = 2010; prenom = "Robin"; naissances = 18308 }
      { sexe = "m"; decennie = 2010; prenom = "Alexandre"; naissances = 18077 }
      { sexe = "m"; decennie = 2010; prenom = "Samuel"; naissances = 16245 }
      { sexe = "m"; decennie = 2010; prenom = "Alexis"; naissances = 15161 }
      { sexe = "m"; decennie = 2010; prenom = "Simon"; naissances = 15135 }
      { sexe = "m"; decennie = 2010; prenom = "Marius"; naissances = 14669 }
      { sexe = "m"; decennie = 2010; prenom = "Noa"; naissances = 14580 }
      { sexe = "m"; decennie = 2010; prenom = "Eden"; naissances = 14129 }
      { sexe = "m"; decennie = 2010; prenom = "Lenny"; naissances = 14074 }
      { sexe = "m"; decennie = 2010; prenom = "Tiago"; naissances = 13710 }
      { sexe = "m"; decennie = 2010; prenom = "Mathys"; naissances = 12898 }
      { sexe = "m"; decennie = 2010; prenom = "Eliott"; naissances = 12488 }
      { sexe = "m"; decennie = 2010; prenom = "Gaspard"; naissances = 12445 }
      { sexe = "m"; decennie = 2010; prenom = "Kylian"; naissances = 12130 }
      { sexe = "m"; decennie = 2010; prenom = "Augustin"; naissances = 12114 }
      { sexe = "m"; decennie = 2010; prenom = "Léon"; naissances = 11990 }
      { sexe = "m"; decennie = 2010; prenom = "Lorenzo"; naissances = 11931 }
      { sexe = "m"; decennie = 2010; prenom = "Esteban"; naissances = 11433 }
      { sexe = "m"; decennie = 2010; prenom = "Nino"; naissances = 10912 }
      { sexe = "m"; decennie = 2010; prenom = "Naël"; naissances = 10895 }
      { sexe = "m"; decennie = 2010; prenom = "Malo"; naissances = 10750 }
      { sexe = "m"; decennie = 2010; prenom = "Quentin"; naissances = 10618 }
      { sexe = "m"; decennie = 2010; prenom = "Adrien"; naissances = 10435 }
      { sexe = "m"; decennie = 2010; prenom = "Ilyes"; naissances = 10114 }
      { sexe = "m"; decennie = 2010; prenom = "Benjamin"; naissances = 10057 }
      { sexe = "m"; decennie = 2010; prenom = "Ibrahim"; naissances = 9943 }
      { sexe = "m"; decennie = 2010; prenom = "Camille"; naissances = 9865 }
      { sexe = "m"; decennie = 2010; prenom = "Isaac"; naissances = 9787 }
      { sexe = "m"; decennie = 2010; prenom = "Amine"; naissances = 9751 }
      { sexe = "m"; decennie = 2010; prenom = "Noam"; naissances = 9748 }
      { sexe = "m"; decennie = 2010; prenom = "Amir"; naissances = 9664 }
      { sexe = "m"; decennie = 2010; prenom = "Oscar"; naissances = 9494 }
      { sexe = "m"; decennie = 2010; prenom = "Kaïs"; naissances = 9197 }
      { sexe = "m"; decennie = 2010; prenom = "Kenzo"; naissances = 8696 }
      { sexe = "m"; decennie = 2010; prenom = "Pierre"; naissances = 8696 }
      { sexe = "m"; decennie = 2010; prenom = "Lyam"; naissances = 8580 }
      { sexe = "m"; decennie = 2010; prenom = "Antonin"; naissances = 8554 }
      { sexe = "m"; decennie = 2010; prenom = "Ayoub"; naissances = 8434 }
      { sexe = "m"; decennie = 2010; prenom = "Imran"; naissances = 8378 }
      { sexe = "m"; decennie = 2010; prenom = "Louka"; naissances = 8375 }
      { sexe = "m"; decennie = 2010; prenom = "William"; naissances = 8371 }
      { sexe = "m"; decennie = 2010; prenom = "Romain"; naissances = 8341 }
      { sexe = "m"; decennie = 2010; prenom = "Bastien"; naissances = 8329 }
      { sexe = "m"; decennie = 2010; prenom = "Soan"; naissances = 8244 }
      { sexe = "m"; decennie = 2010; prenom = "Jean"; naissances = 7999 }
      { sexe = "m"; decennie = 2010; prenom = "Diego"; naissances = 7998 }
      { sexe = "m"; decennie = 2010; prenom = "Leo"; naissances = 7974 }
      { sexe = "m"; decennie = 2010; prenom = "Mael"; naissances = 7896 }
      { sexe = "m"; decennie = 2010; prenom = "Ruben"; naissances = 7894 }
      { sexe = "m"; decennie = 2010; prenom = "Marceau"; naissances = 7877 }
      { sexe = "m"; decennie = 2010; prenom = "Raphael"; naissances = 7873 }
      { sexe = "m"; decennie = 2010; prenom = "Noham"; naissances = 7778 }
      { sexe = "m"; decennie = 2010; prenom = "Youssef"; naissances = 7778 }
      { sexe = "m"; decennie = 2010; prenom = "David"; naissances = 7470 }
      { sexe = "m"; decennie = 2010; prenom = "Wassim"; naissances = 7443 }
      { sexe = "m"; decennie = 2010; prenom = "Rafael"; naissances = 7396 }
      { sexe = "m"; decennie = 2010; prenom = "Mathias"; naissances = 7359 }
      { sexe = "m"; decennie = 2010; prenom = "Thibault"; naissances = 7315 }
      { sexe = "m"; decennie = 2010; prenom = "Milo"; naissances = 7243 }
      { sexe = "m"; decennie = 2010; prenom = "Younes"; naissances = 7215 }
      { sexe = "m"; decennie = 2010; prenom = "Ismaël"; naissances = 7127 }
      { sexe = "m"; decennie = 2010; prenom = "Owen"; naissances = 7123 }
      { sexe = "m"; decennie = 2010; prenom = "Sohan"; naissances = 7104 }
      { sexe = "m"; decennie = 2010; prenom = "Joseph"; naissances = 7027 }
      { sexe = "m"; decennie = 2010; prenom = "Charles"; naissances = 6960 }
      { sexe = "m"; decennie = 2010; prenom = "Titouan"; naissances = 6917 }
      { sexe = "m"; decennie = 2010; prenom = "Adem"; naissances = 6822 }
      { sexe = "m"; decennie = 2010; prenom = "Tristan"; naissances = 6799 }
      { sexe = "m"; decennie = 2010; prenom = "Timeo"; naissances = 6770 }
      { sexe = "m"; decennie = 2010; prenom = "Dylan"; naissances = 6670 }
      { sexe = "m"; decennie = 2010; prenom = "Mattéo"; naissances = 6646 }
      { sexe = "m"; decennie = 2010; prenom = "Ali"; naissances = 6502 }
      { sexe = "m"; decennie = 2010; prenom = "Milan"; naissances = 6445 }
      { sexe = "m"; decennie = 2010; prenom = "Mehdi"; naissances = 6416 }
      { sexe = "m"; decennie = 2010; prenom = "Nicolas"; naissances = 6373 }
      { sexe = "m"; decennie = 2010; prenom = "Corentin"; naissances = 6364 }
      { sexe = "m"; decennie = 2010; prenom = "Naïm"; naissances = 6361 }
      { sexe = "m"; decennie = 2010; prenom = "Loan"; naissances = 6348 }
      { sexe = "m"; decennie = 2010; prenom = "Léandre"; naissances = 6312 }
      { sexe = "m"; decennie = 2010; prenom = "Mathieu"; naissances = 6292 }
      { sexe = "m"; decennie = 2010; prenom = "Charly"; naissances = 6224 }
      { sexe = "m"; decennie = 2010; prenom = "Yassine"; naissances = 6201 }
      { sexe = "m"; decennie = 2010; prenom = "Sasha"; naissances = 6180 }
      { sexe = "m"; decennie = 2010; prenom = "Hamza"; naissances = 6147 }
      { sexe = "m"; decennie = 2010; prenom = "Erwan"; naissances = 6053 }
      { sexe = "m"; decennie = 2010; prenom = "Julian"; naissances = 6042 }
      { sexe = "m"; decennie = 2010; prenom = "Nassim"; naissances = 6015 }
      { sexe = "m"; decennie = 2010; prenom = "Timothée"; naissances = 5979 }
      { sexe = "m"; decennie = 2010; prenom = "Ayden"; naissances = 5953 }
      { sexe = "m"; decennie = 2010; prenom = "Ilan"; naissances = 5886 }
      { sexe = "m"; decennie = 2010; prenom = "Elias"; naissances = 5841 }
      { sexe = "m"; decennie = 2010; prenom = "Ewen"; naissances = 5736 }
      { sexe = "m"; decennie = 2010; prenom = "Côme"; naissances = 5718 }
      { sexe = "m"; decennie = 2010; prenom = "Julien"; naissances = 5697 }
      { sexe = "m"; decennie = 2010; prenom = "Pablo"; naissances = 5694 }
      { sexe = "m"; decennie = 2010; prenom = "Loris"; naissances = 5623 }
      { sexe = "m"; decennie = 2010; prenom = "Ryan"; naissances = 5566 }
      { sexe = "m"; decennie = 2010; prenom = "Charlie"; naissances = 5491 }
      { sexe = "m"; decennie = 2010; prenom = "Basile"; naissances = 5483 }
      { sexe = "m"; decennie = 2010; prenom = "Anas"; naissances = 5448 }
      { sexe = "m"; decennie = 2010; prenom = "Nael"; naissances = 5447 }
      { sexe = "m"; decennie = 2010; prenom = "Amaury"; naissances = 5433 }
      { sexe = "m"; decennie = 2010; prenom = "Mahé"; naissances = 5407 }
      { sexe = "m"; decennie = 2010; prenom = "Lucien"; naissances = 5331 }
      { sexe = "m"; decennie = 2010; prenom = "Luca"; naissances = 5322 }
      { sexe = "m"; decennie = 2010; prenom = "Zakaria"; naissances = 5279 }
      { sexe = "m"; decennie = 2010; prenom = "Lilian"; naissances = 5241 }
      { sexe = "m"; decennie = 2010; prenom = "Logan"; naissances = 5238 }
      { sexe = "m"; decennie = 2010; prenom = "Alban"; naissances = 5164 }
      { sexe = "m"; decennie = 2010; prenom = "Timothé"; naissances = 5147 }
      { sexe = "m"; decennie = 2010; prenom = "Theo"; naissances = 5100 }
      { sexe = "m"; decennie = 2010; prenom = "Issa"; naissances = 5095 }
      { sexe = "m"; decennie = 2010; prenom = "Luka"; naissances = 5072 }
      { sexe = "m"; decennie = 2010; prenom = "Rafaël"; naissances = 5072 }
      { sexe = "m"; decennie = 2010; prenom = "Aymen"; naissances = 5012 }
      { sexe = "m"; decennie = 2010; prenom = "Sandro"; naissances = 4900 }
      { sexe = "m"; decennie = 2010; prenom = "Ahmed"; naissances = 4850 }
      { sexe = "m"; decennie = 2010; prenom = "Nolhan"; naissances = 4785 }
      { sexe = "m"; decennie = 2010; prenom = "Joshua"; naissances = 4781 }
      { sexe = "m"; decennie = 2010; prenom = "Elio"; naissances = 4742 }
      { sexe = "m"; decennie = 2010; prenom = "Nahil"; naissances = 4723 }
      { sexe = "m"; decennie = 2010; prenom = "Eliot"; naissances = 4670 }
      { sexe = "m"; decennie = 2010; prenom = "Sofiane"; naissances = 4638 }
      { sexe = "m"; decennie = 2010; prenom = "Matteo"; naissances = 4636 }
      { sexe = "m"; decennie = 2010; prenom = "Abel"; naissances = 4608 }
      { sexe = "m"; decennie = 2010; prenom = "Nathanaël"; naissances = 4605 }
      { sexe = "m"; decennie = 2010; prenom = "Malone"; naissances = 4541 }
      { sexe = "m"; decennie = 2010; prenom = "Yann"; naissances = 4491 }
      { sexe = "m"; decennie = 2010; prenom = "Johan"; naissances = 4456 }
      { sexe = "m"; decennie = 2010; prenom = "Clement"; naissances = 4385 }
      { sexe = "m"; decennie = 2010; prenom = "Noe"; naissances = 4367 }
      { sexe = "m"; decennie = 2010; prenom = "Matéo"; naissances = 4364 }
      { sexe = "m"; decennie = 2010; prenom = "Samy"; naissances = 4336 }
      { sexe = "m"; decennie = 2010; prenom = "Matheo"; naissances = 4280 }
      { sexe = "m"; decennie = 2010; prenom = "Thiago"; naissances = 4257 }
      { sexe = "m"; decennie = 2010; prenom = "Morgan"; naissances = 4256 }
      { sexe = "m"; decennie = 2010; prenom = "Alex"; naissances = 4176 }
      { sexe = "m"; decennie = 2010; prenom = "Achille"; naissances = 4134 }
      { sexe = "m"; decennie = 2010; prenom = "Dorian"; naissances = 4133 }
      { sexe = "m"; decennie = 2010; prenom = "Daniel"; naissances = 4119 }
      { sexe = "m"; decennie = 2010; prenom = "Bilal"; naissances = 4050 }
      { sexe = "m"; decennie = 2010; prenom = "Rayane"; naissances = 4036 }
      { sexe = "m"; decennie = 2010; prenom = "Roméo"; naissances = 4016 }
      { sexe = "m"; decennie = 2010; prenom = "Armand"; naissances = 4007 }
      { sexe = "m"; decennie = 2010; prenom = "Alessio"; naissances = 3979 }
      { sexe = "m"; decennie = 2010; prenom = "Anthony"; naissances = 3955 }
      { sexe = "m"; decennie = 2010; prenom = "Florian"; naissances = 3935 }
      { sexe = "m"; decennie = 2010; prenom = "Auguste"; naissances = 3915 }
      { sexe = "m"; decennie = 2010; prenom = "Jordan"; naissances = 3902 }
      { sexe = "m"; decennie = 2010; prenom = "Ilyan"; naissances = 3898 }
      { sexe = "m"; decennie = 2010; prenom = "Yacine"; naissances = 3893 }
      { sexe = "m"; decennie = 2010; prenom = "Nolann"; naissances = 3873 }
      { sexe = "m"; decennie = 2010; prenom = "Marin"; naissances = 3846 }
      { sexe = "m"; decennie = 2010; prenom = "Anis"; naissances = 3841 }
      { sexe = "m"; decennie = 2010; prenom = "Imrane"; naissances = 3770 }
      { sexe = "m"; decennie = 2010; prenom = "Idriss"; naissances = 3691 }
      { sexe = "m"; decennie = 2010; prenom = "Matthieu"; naissances = 3675 }
      { sexe = "m"; decennie = 2010; prenom = "Djibril"; naissances = 3654 }
      { sexe = "m"; decennie = 2010; prenom = "Ezio"; naissances = 3616 }
      { sexe = "m"; decennie = 2010; prenom = "Mohammed"; naissances = 3600 }
      { sexe = "m"; decennie = 2020; prenom = "Gabriel"; naissances = 9394 }
      { sexe = "m"; decennie = 2020; prenom = "Léo"; naissances = 8857 }
      { sexe = "m"; decennie = 2020; prenom = "Raphaël"; naissances = 7937 }
      { sexe = "m"; decennie = 2020; prenom = "Louis"; naissances = 7509 }
      { sexe = "m"; decennie = 2020; prenom = "Arthur"; naissances = 7402 }
      { sexe = "m"; decennie = 2020; prenom = "Jules"; naissances = 7141 }
      { sexe = "m"; decennie = 2020; prenom = "Maël"; naissances = 6740 }
      { sexe = "m"; decennie = 2020; prenom = "Adam"; naissances = 6541 }
      { sexe = "m"; decennie = 2020; prenom = "Lucas"; naissances = 6304 }
      { sexe = "m"; decennie = 2020; prenom = "Noah"; naissances = 6195 }
      { sexe = "m"; decennie = 2020; prenom = "Hugo"; naissances = 6034 }
      { sexe = "m"; decennie = 2020; prenom = "Gabin"; naissances = 5445 }
      { sexe = "m"; decennie = 2020; prenom = "Liam"; naissances = 5419 }
      { sexe = "m"; decennie = 2020; prenom = "Sacha"; naissances = 5181 }
      { sexe = "m"; decennie = 2020; prenom = "Aaron"; naissances = 4809 }
      { sexe = "m"; decennie = 2020; prenom = "Paul"; naissances = 4766 }
      { sexe = "m"; decennie = 2020; prenom = "Nathan"; naissances = 4691 }
      { sexe = "m"; decennie = 2020; prenom = "Léon"; naissances = 4530 }
      { sexe = "m"; decennie = 2020; prenom = "Mohamed"; naissances = 4494 }
      { sexe = "m"; decennie = 2020; prenom = "Eden"; naissances = 4486 }
      { sexe = "m"; decennie = 2020; prenom = "Noé"; naissances = 4406 }
      { sexe = "m"; decennie = 2020; prenom = "Ethan"; naissances = 4387 }
      { sexe = "m"; decennie = 2020; prenom = "Isaac"; naissances = 4257 }
      { sexe = "m"; decennie = 2020; prenom = "Tom"; naissances = 4216 }
      { sexe = "m"; decennie = 2020; prenom = "Théo"; naissances = 3841 }
      { sexe = "m"; decennie = 2020; prenom = "Marius"; naissances = 3712 }
      { sexe = "m"; decennie = 2020; prenom = "Naël"; naissances = 3712 }
      { sexe = "m"; decennie = 2020; prenom = "Tiago"; naissances = 3630 }
      { sexe = "m"; decennie = 2020; prenom = "Victor"; naissances = 3579 }
      { sexe = "m"; decennie = 2020; prenom = "Ayden"; naissances = 3543 }
      { sexe = "m"; decennie = 2020; prenom = "Mathis"; naissances = 3535 }
      { sexe = "m"; decennie = 2020; prenom = "Martin"; naissances = 3486 }
      { sexe = "m"; decennie = 2020; prenom = "Malo"; naissances = 3436 }
      { sexe = "m"; decennie = 2020; prenom = "Marceau"; naissances = 3411 }
      { sexe = "m"; decennie = 2020; prenom = "Nino"; naissances = 3365 }
      { sexe = "m"; decennie = 2020; prenom = "Robin"; naissances = 3287 }
      { sexe = "m"; decennie = 2020; prenom = "Axel"; naissances = 3260 }
      { sexe = "m"; decennie = 2020; prenom = "Milo"; naissances = 3199 }
      { sexe = "m"; decennie = 2020; prenom = "Timéo"; naissances = 3168 }
      { sexe = "m"; decennie = 2020; prenom = "Enzo"; naissances = 3093 }
      { sexe = "m"; decennie = 2020; prenom = "Eliott"; naissances = 3065 }
      { sexe = "m"; decennie = 2020; prenom = "Nolan"; naissances = 2946 }
      { sexe = "m"; decennie = 2020; prenom = "Antoine"; naissances = 2903 }
      { sexe = "m"; decennie = 2020; prenom = "Lyam"; naissances = 2901 }
      { sexe = "m"; decennie = 2020; prenom = "Valentin"; naissances = 2889 }
      { sexe = "m"; decennie = 2020; prenom = "Augustin"; naissances = 2809 }
      { sexe = "m"; decennie = 2020; prenom = "Samuel"; naissances = 2719 }
      { sexe = "m"; decennie = 2020; prenom = "Gaspard"; naissances = 2676 }
      { sexe = "m"; decennie = 2020; prenom = "Amir"; naissances = 2674 }
      { sexe = "m"; decennie = 2020; prenom = "Rayan"; naissances = 2598 }
      { sexe = "m"; decennie = 2020; prenom = "Ibrahim"; naissances = 2576 }
      { sexe = "m"; decennie = 2020; prenom = "Yanis"; naissances = 2512 }
      { sexe = "m"; decennie = 2020; prenom = "Côme"; naissances = 2437 }
      { sexe = "m"; decennie = 2020; prenom = "Sohan"; naissances = 2421 }
      { sexe = "m"; decennie = 2020; prenom = "Imran"; naissances = 2411 }
      { sexe = "m"; decennie = 2020; prenom = "Simon"; naissances = 2365 }
      { sexe = "m"; decennie = 2020; prenom = "Mathéo"; naissances = 2347 }
      { sexe = "m"; decennie = 2020; prenom = "Maxence"; naissances = 2324 }
      { sexe = "m"; decennie = 2020; prenom = "Kaïs"; naissances = 2307 }
      { sexe = "m"; decennie = 2020; prenom = "Clément"; naissances = 2304 }
      { sexe = "m"; decennie = 2020; prenom = "Baptiste"; naissances = 2171 }
      { sexe = "m"; decennie = 2020; prenom = "Soan"; naissances = 2153 }
      { sexe = "m"; decennie = 2020; prenom = "Evan"; naissances = 2148 }
      { sexe = "m"; decennie = 2020; prenom = "Camille"; naissances = 2142 }
      { sexe = "m"; decennie = 2020; prenom = "Léandre"; naissances = 2063 }
      { sexe = "m"; decennie = 2020; prenom = "Elio"; naissances = 2037 }
      { sexe = "m"; decennie = 2020; prenom = "Alexandre"; naissances = 2036 }
      { sexe = "m"; decennie = 2020; prenom = "Owen"; naissances = 2035 }
      { sexe = "m"; decennie = 2020; prenom = "Ismaël"; naissances = 2016 }
      { sexe = "m"; decennie = 2020; prenom = "Maxime"; naissances = 2013 }
      { sexe = "m"; decennie = 2020; prenom = "Pablo"; naissances = 1990 }
      { sexe = "m"; decennie = 2020; prenom = "Naïm"; naissances = 1870 }
      { sexe = "m"; decennie = 2020; prenom = "Lenny"; naissances = 1847 }
      { sexe = "m"; decennie = 2020; prenom = "Oscar"; naissances = 1790 }
      { sexe = "m"; decennie = 2020; prenom = "Alessio"; naissances = 1769 }
      { sexe = "m"; decennie = 2020; prenom = "Noa"; naissances = 1756 }
      { sexe = "m"; decennie = 2020; prenom = "Ilyan"; naissances = 1755 }
      { sexe = "m"; decennie = 2020; prenom = "Joseph"; naissances = 1751 }
      { sexe = "m"; decennie = 2020; prenom = "Thomas"; naissances = 1720 }
      { sexe = "m"; decennie = 2020; prenom = "Charly"; naissances = 1698 }
      { sexe = "m"; decennie = 2020; prenom = "Basile"; naissances = 1693 }
      { sexe = "m"; decennie = 2020; prenom = "Livio"; naissances = 1693 }
      { sexe = "m"; decennie = 2020; prenom = "Diego"; naissances = 1644 }
      { sexe = "m"; decennie = 2020; prenom = "Noam"; naissances = 1592 }
      { sexe = "m"; decennie = 2020; prenom = "Mahé"; naissances = 1566 }
      { sexe = "m"; decennie = 2020; prenom = "Charlie"; naissances = 1565 }
      { sexe = "m"; decennie = 2020; prenom = "Ali"; naissances = 1548 }
      { sexe = "m"; decennie = 2020; prenom = "Ilyes"; naissances = 1533 }
      { sexe = "m"; decennie = 2020; prenom = "Malone"; naissances = 1528 }
      { sexe = "m"; decennie = 2020; prenom = "Milan"; naissances = 1498 }
      { sexe = "m"; decennie = 2020; prenom = "Anas"; naissances = 1482 }
      { sexe = "m"; decennie = 2020; prenom = "Logan"; naissances = 1460 }
      { sexe = "m"; decennie = 2020; prenom = "Marin"; naissances = 1457 }
      { sexe = "m"; decennie = 2020; prenom = "Auguste"; naissances = 1452 }
      { sexe = "m"; decennie = 2020; prenom = "Charles"; naissances = 1444 }
      { sexe = "m"; decennie = 2020; prenom = "Maé"; naissances = 1442 }
      { sexe = "m"; decennie = 2020; prenom = "Timothée"; naissances = 1428 }
      { sexe = "m"; decennie = 2020; prenom = "Youssef"; naissances = 1399 }
      { sexe = "m"; decennie = 2020; prenom = "William"; naissances = 1384 }
      { sexe = "m"; decennie = 2020; prenom = "Aylan"; naissances = 1382 }
      { sexe = "m"; decennie = 2020; prenom = "Mathys"; naissances = 1374 }
      { sexe = "m"; decennie = 2020; prenom = "Ezio"; naissances = 1373 }
      { sexe = "m"; decennie = 2020; prenom = "Kayden"; naissances = 1369 }
      { sexe = "m"; decennie = 2020; prenom = "Roméo"; naissances = 1349 }
      { sexe = "m"; decennie = 2020; prenom = "Ayoub"; naissances = 1342 }
      { sexe = "m"; decennie = 2020; prenom = "Issa"; naissances = 1339 }
      { sexe = "m"; decennie = 2020; prenom = "Adem"; naissances = 1319 }
      { sexe = "m"; decennie = 2020; prenom = "Milann"; naissances = 1318 }
      { sexe = "m"; decennie = 2020; prenom = "Abel"; naissances = 1297 }
      { sexe = "m"; decennie = 2020; prenom = "Lucien"; naissances = 1296 }
      { sexe = "m"; decennie = 2020; prenom = "Andrea"; naissances = 1280 }
      { sexe = "m"; decennie = 2020; prenom = "Jean"; naissances = 1265 }
      { sexe = "m"; decennie = 2020; prenom = "Achille"; naissances = 1254 }
      { sexe = "m"; decennie = 2020; prenom = "Mattéo"; naissances = 1236 }
      { sexe = "m"; decennie = 2020; prenom = "Wassim"; naissances = 1225 }
      { sexe = "m"; decennie = 2020; prenom = "Esteban"; naissances = 1221 }
      { sexe = "m"; decennie = 2020; prenom = "Adrien"; naissances = 1209 }
      { sexe = "m"; decennie = 2020; prenom = "Sandro"; naissances = 1203 }
      { sexe = "m"; decennie = 2020; prenom = "Thiago"; naissances = 1199 }
      { sexe = "m"; decennie = 2020; prenom = "David"; naissances = 1183 }
      { sexe = "m"; decennie = 2020; prenom = "Kylian"; naissances = 1165 }
      { sexe = "m"; decennie = 2020; prenom = "Léonard"; naissances = 1147 }
      { sexe = "m"; decennie = 2020; prenom = "Maylone"; naissances = 1138 }
      { sexe = "m"; decennie = 2020; prenom = "Alexis"; naissances = 1113 }
      { sexe = "m"; decennie = 2020; prenom = "Younes"; naissances = 1109 }
      { sexe = "m"; decennie = 2020; prenom = "Antonin"; naissances = 1106 }
      { sexe = "m"; decennie = 2020; prenom = "Noham"; naissances = 1089 }
      { sexe = "m"; decennie = 2020; prenom = "Sasha"; naissances = 1088 }
      { sexe = "m"; decennie = 2020; prenom = "Hamza"; naissances = 1083 }
      { sexe = "m"; decennie = 2020; prenom = "Lorenzo"; naissances = 1076 }
      { sexe = "m"; decennie = 2020; prenom = "Rafael"; naissances = 1067 }
      { sexe = "m"; decennie = 2020; prenom = "Kenzo"; naissances = 1050 }
      { sexe = "m"; decennie = 2020; prenom = "Zayn"; naissances = 1046 }
      { sexe = "m"; decennie = 2020; prenom = "Elias"; naissances = 1044 }
      { sexe = "m"; decennie = 2020; prenom = "Kassim"; naissances = 1041 }
      { sexe = "m"; decennie = 2020; prenom = "Benjamin"; naissances = 1037 }
      { sexe = "m"; decennie = 2020; prenom = "Amine"; naissances = 1027 }
      { sexe = "m"; decennie = 2020; prenom = "Rafaël"; naissances = 1023 }
      { sexe = "m"; decennie = 2020; prenom = "Gianni"; naissances = 1021 }
      { sexe = "m"; decennie = 2020; prenom = "Ulysse"; naissances = 997 }
      { sexe = "m"; decennie = 2020; prenom = "Zakaria"; naissances = 988 }
      { sexe = "m"; decennie = 2020; prenom = "Nathanaël"; naissances = 981 }
      { sexe = "m"; decennie = 2020; prenom = "Mayron"; naissances = 975 }
      { sexe = "m"; decennie = 2020; prenom = "Ahmed"; naissances = 974 }
      { sexe = "m"; decennie = 2020; prenom = "Julian"; naissances = 968 }
      { sexe = "m"; decennie = 2020; prenom = "Marcel"; naissances = 964 }
      { sexe = "m"; decennie = 2020; prenom = "Félix"; naissances = 960 }
      { sexe = "m"; decennie = 2020; prenom = "Nahil"; naissances = 953 }
      { sexe = "m"; decennie = 2020; prenom = "Hayden"; naissances = 950 }
      { sexe = "m"; decennie = 2020; prenom = "Ruben"; naissances = 935 }
      { sexe = "m"; decennie = 2020; prenom = "Louka"; naissances = 928 }
      { sexe = "m"; decennie = 2020; prenom = "Mathias"; naissances = 927 }
      { sexe = "m"; decennie = 2020; prenom = "Aymen"; naissances = 922 }
      { sexe = "m"; decennie = 2020; prenom = "Mylan"; naissances = 922 }
      { sexe = "m"; decennie = 2020; prenom = "Jayden"; naissances = 916 }
      { sexe = "m"; decennie = 2020; prenom = "Haroun"; naissances = 908 }
      { sexe = "m"; decennie = 2020; prenom = "Lino"; naissances = 906 }
      { sexe = "m"; decennie = 2020; prenom = "Yassine"; naissances = 904 }
      { sexe = "m"; decennie = 2020; prenom = "Alban"; naissances = 897 }
      { sexe = "m"; decennie = 2020; prenom = "Moussa"; naissances = 893 }
      { sexe = "m"; decennie = 2020; prenom = "Nassim"; naissances = 892 }
      { sexe = "m"; decennie = 2020; prenom = "Ewen"; naissances = 881 }
      { sexe = "m"; decennie = 2020; prenom = "Gustave"; naissances = 879 }
      { sexe = "m"; decennie = 2020; prenom = "Armand"; naissances = 855 }
      { sexe = "m"; decennie = 2020; prenom = "Swan"; naissances = 849 }
      { sexe = "m"; decennie = 2020; prenom = "Amaury"; naissances = 843 }
      { sexe = "m"; decennie = 2020; prenom = "Idriss"; naissances = 841 }
      { sexe = "m"; decennie = 2020; prenom = "Luca"; naissances = 836 }
      { sexe = "m"; decennie = 2020; prenom = "Timothé"; naissances = 836 }
      { sexe = "m"; decennie = 2020; prenom = "Daniel"; naissances = 829 }
      { sexe = "m"; decennie = 2020; prenom = "Pierre"; naissances = 829 }
      { sexe = "m"; decennie = 2020; prenom = "Arsène"; naissances = 827 }
      { sexe = "m"; decennie = 2020; prenom = "Matéo"; naissances = 820 }
      { sexe = "m"; decennie = 2020; prenom = "Ilyas"; naissances = 798 }
      { sexe = "m"; decennie = 2020; prenom = "Loan"; naissances = 777 }
      { sexe = "m"; decennie = 2020; prenom = "César"; naissances = 776 }
      { sexe = "m"; decennie = 2020; prenom = "James"; naissances = 774 }
      { sexe = "m"; decennie = 2020; prenom = "Octave"; naissances = 770 }
      { sexe = "m"; decennie = 2020; prenom = "Bastien"; naissances = 765 }
      { sexe = "m"; decennie = 2020; prenom = "Swann"; naissances = 765 }
      { sexe = "m"; decennie = 2020; prenom = "Djibril"; naissances = 757 }
      { sexe = "m"; decennie = 2020; prenom = "Léandro"; naissances = 721 }
      { sexe = "m"; decennie = 2020; prenom = "Eliot"; naissances = 713 }
      { sexe = "m"; decennie = 2020; prenom = "Emile"; naissances = 710 }
      { sexe = "m"; decennie = 2020; prenom = "Thibault"; naissances = 709 }
      { sexe = "m"; decennie = 2020; prenom = "Ilan"; naissances = 706 }
      { sexe = "m"; decennie = 2020; prenom = "Mohammed"; naissances = 703 }
      { sexe = "m"; decennie = 2020; prenom = "Ezra"; naissances = 695 }
      { sexe = "m"; decennie = 2020; prenom = "Joshua"; naissances = 694 }
      { sexe = "m"; decennie = 2020; prenom = "Elyo"; naissances = 693 }
      { sexe = "m"; decennie = 2020; prenom = "Mylann"; naissances = 686 }
      { sexe = "m"; decennie = 2020; prenom = "Hector"; naissances = 684 }
      { sexe = "m"; decennie = 2020; prenom = "Anatole"; naissances = 673 }
      { sexe = "m"; decennie = 2020; prenom = "Jad"; naissances = 673 }
      { sexe = "m"; decennie = 2020; prenom = "Kamil"; naissances = 670 }
      { sexe = "m"; decennie = 2020; prenom = "Élio"; naissances = 668 }
      { sexe = "m"; decennie = 2020; prenom = "Nicolas"; naissances = 663 }
      { sexe = "m"; decennie = 2020; prenom = "Matteo"; naissances = 659 }
      { sexe = "m"; decennie = 2020; prenom = "Nael"; naissances = 658 }
      { sexe = "m"; decennie = 2020; prenom = "Henri"; naissances = 652 }
    |]
