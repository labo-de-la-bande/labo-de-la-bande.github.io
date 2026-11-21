// GÉNÉRÉ depuis data/pays/fr/jauges.csv par `labo generate`.
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
