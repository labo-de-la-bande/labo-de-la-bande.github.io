# Le Labo de la Bande

Un jeu pour explorer un modèle de société **sans prison, sans police et, autant que possible, sans monnaie**, pensé pour la France, outre-mer compris. Il montre aussi ce que chaque choix coûte.

**Jouer :** https://labo-de-la-bande.github.io/

## Ce que contient le jeu

Le modèle part de quelques principes : société anticarcérale, non patriarcale, petite échelle d'abord, pas de monnaie, et un système « fermé » à la France, pris comme hypothèse de calcul. On l'explore table par table :

- un **quiz** sur les chiffres de la première version du modèle, dont certains ont été corrigés ;
- un **stress test** de la société sans police, avec dix réparations ;
- l'**outre-mer**, l'**ouverture du pays** avec l'**équilibre des échanges** (ce qu'on importe et ce qu'on offre en échange), et les **ressources qui manquent** (engrais, énergie, médicaments) ;
- des **paris** sur des solutions en développement : low-tech, isolation, hydroliennes, silos à sable, engrais à base d'urine, cargos à voile, règles pour les gros pollueurs, etc. ;
- la **monnaie** (cinq formes, avec leurs réglages), l'**héritage**, la **défense** ;
- les **calculs** de nourriture et d'énergie, l'**énergie par habitant** traduite en niveau de vie, et une **comparaison avec aujourd'hui** ;
- la **pyramide des cellules**, le **chantier politique** et l'**épreuve des paliers** pour le passage à l'échelle.

## Les jauges

Une bannière fixe en haut de la page affiche douze jauges, du rouge (fragile) au vert (solide), sur deux lignes. Elles réagissent à chaque décision. Un clic sur une jauge mène à la table correspondante ; le détail s'affiche au survol.

| Jauge | Ce qu'elle mesure |
|---|---|
| Sécurité | Part des attaques du stress test sans police qui tiennent |
| Criminalité | Une fourchette : de l'hypothèse pessimiste (sans police ni prison, homicides +16 % comme lors des grèves de police au Brésil) à l'hypothèse optimiste (prévention, socle et réparations qui marchent). 50 % correspond à la France d'aujourd'hui (976 homicides en 2024) |
| Liberté | Personnes privées de liberté : aucune prison dans le modèle, contre 90 020 détenus en France |
| Moyens libérés | Part du budget des prisons et de la police (29,5 Md€ en 2025) réaffectée à la prévention, aux soins et au socle, une fois payés les gardes, la médiation et les soins |
| Ressources | Ressources couvertes dans le test de fermeture, par la production ou par import |
| Niveau de vie | Service rendu par l'énergie disponible par personne (comparé à aujourd'hui), multiplié par la part des personnes dont les besoins de base sont couverts. Pour la France d'aujourd'hui : 85 %, car 15,4 % de la population vit sous le seuil de pauvreté (Insee, 2023), avec un écart de 3,5 entre les 10 % les plus aisés et les 10 % les plus modestes. Dans le modèle, la part couverte dépend aussi de la monnaie et de l'écart de revenus |
| Nourriture | Personnes que les terres peuvent nourrir (rapportées à la population, selon l'ouverture du pays), multiplié par la part des personnes qui mangent à leur faim (selon la monnaie et l'écart de revenus : en nature, seules manquent les personnes que le socle n'atteint pas ; avec une monnaie, la part qui manque suit l'écart de revenus). Pour la France d'aujourd'hui : 88 %, car 12 % de la population dit ne pas avoir assez à manger (Crédoc, 2024) |
| Contre-pouvoirs | Inverse du risque de prise du pouvoir (table Politique) |
| Passage à l'échelle | Statut de l'épreuve des paliers : réduit, sur le papier, ou vert à confirmer |
| Échanges | Ce qu'on offre en échange, rapporté à ce qu'on importe |
| Solidité des paris | Inverse du risque pris en misant sur des solutions peu mûres |
| Fidélité aux principes | Baisse avec la place de la monnaie, une armée ou des gardes renforcées, la centralisation, l'héritage libre et la grande industrie. L'ouverture du pays n'y compte pas : la fermeture n'est qu'une base de calcul |

À gauche de la bannière :

- un **sélecteur de temps** (aujourd'hui, 10 ans, 30 ans) fait avancer le modèle : population projetée par l'Insee, paris qui arrivent à maturité ;
- **« Comparer à »** place sur chaque jauge un repère de la France : **aujourd'hui** (trait plein), selon la **tendance** actuelle (trait creux) ou selon les **objectifs officiels** (trait bleu). Les projections n'apparaissent que là où il existe des données : niveau de vie (−0,9 % d'énergie par an ; objectif de loi −50 % en 2050), nourriture (terres perdues, population projetée, et demande d'aide alimentaire qui continue de croître comme depuis 20 ans : les Restos du Cœur sont passés de 81,5 à 161 millions de repas entre 2006-2007 et 2024-2025) et, à 10 ans, liberté (hausse de 5 % des détenus par an). Certains de ces repères sont sourcés (énergie, échanges), d'autres sont des jugements (sécurité, contre-pouvoirs, fidélité aux principes) ; le détail s'affiche au survol.

Les formules des jauges sont des choix du jeu : elles montrent dans quel sens va chaque décision, pas une mesure.

## Partager son modèle

Dans la table « Ton modèle », le bouton **Créer le lien de partage** fabrique un lien qui contient tous les choix et réglages. Ce lien ouvre une page de synthèse : les jauges, les choix et leur prix, la comparaison avec aujourd'hui et le bilan. La personne qui le reçoit peut ensuite explorer le détail ou repartir de ce modèle.

Rien n'est envoyé à un serveur : les choix restent dans le navigateur de chacun, et seules les personnes à qui on donne le lien voient le modèle.

## Comment lire les chiffres

Chaque chiffre porte un statut :

- **sourcé** : une source vérifiable est citée (liste en bas de la page) ;
- **hypothèse** : un chiffre ou un effet choisi pour le jeu, pour explorer ce qu'il implique ;
- **jugement** : un avis argumenté, jamais une mesure.

Les verdicts (*tient*, *se fissure*, *casse*, *ouvert*) et les jauges sont des jugements et des formules du jeu, pas des mesures. Les faits marqués « de mémoire » restent à vérifier.

## Contribuer

Corrections de chiffres, sources plus solides, nouvelles solutions à ajouter aux paris : ouvrez une *issue* ou proposez une modification. Tout le jeu tient dans un seul fichier, `index.html`, sans dépendance à installer.

Pour l'essayer en local, il suffit d'ouvrir `index.html` dans un navigateur.

## Licences

- Le code est sous licence **GPL-3.0** (fichier `LICENSE`).
- Les textes et les contenus du jeu sont sous licence **CC BY-SA 4.0** (fichier `LICENCE-TEXTES.md`).

Chacun peut réutiliser et modifier le jeu, à condition de le repartager sous les mêmes licences.
