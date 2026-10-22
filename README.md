# Le Labo de la Bande

Un jeu pour explorer un modèle de société **sans prison, sans police et, autant que possible, sans monnaie**, pensé pour la France, outre-mer compris. Il montre aussi ce que chaque choix coûte.

**Jouer :** https://labo-de-la-bande.github.io/

## Ce que contient le jeu

Le modèle part de quelques principes : société anticarcérale, non patriarcale, sans racisme (chaque règle jugée du point de vue de celles et ceux qui cumulent les discriminations : c’est la lecture intersectionnelle), petite échelle d'abord, pas de monnaie, et un système « fermé » à la France, pris comme hypothèse de calcul. On l'explore table par table :

- un **quiz** sur les chiffres de la première version du modèle, dont certains ont été corrigés ;
- un **stress test** de la société sans police, avec dix réparations ;
- l'**outre-mer**, l'**ouverture du pays** avec l'**équilibre des échanges** (ce qu'on importe et ce qu'on offre en échange), et les **ressources qui manquent** (engrais, énergie, médicaments) ;
- des **paris** sur des solutions en développement : low-tech, isolation, hydroliennes, silos à sable, engrais à base d'urine, cargos à voile, règles pour les gros pollueurs, etc. ;
- la **monnaie** (cinq formes, avec leurs réglages), l'**héritage**, la **défense** ;
- les **calculs** de nourriture et d'énergie, l'**énergie par habitant** traduite en niveau de vie, et une **comparaison avec aujourd'hui** ;
- la **pyramide des cellules**, le **chantier politique** et l'**épreuve des paliers** pour le passage à l'échelle.

## Le jeu (nouvelle version)

Le jeu s'ouvre d'emblée : quatre tranches (Sécurité, Nourrir et chauffer, Qui décide ?, Égaux) qui suivent le même rythme, un **constat** (des chiffres réels et sourcés), des **dilemmes** (l'effet de chaque option et la réaction d'un habitant s'affichent avant de choisir) et un **bilan** face à la France. L'atelier libre réunit toutes les jauges et tous les leviers. Les choix du jeu sont gardés dans le navigateur ; le Labo détaillé reste accessible (lien « voir le Labo détaillé », ou l'adresse `#labo`) pour les explications et les sources.

## Le score

En haut à gauche, un score résume le modèle : la moyenne des dix jauges de résultat. Le même calcul, fait avec les chiffres réels, donne le score de la France (76 aujourd'hui). Le modèle de départ fait moins bien (62) ; le lien « Repartir du modèle de départ », sous le score, efface tous les choix et réglages pour y revenir. Premier but : égaler la France sans police, sans prison, sans patriarcat et sans racisme, y compris pour celles et ceux qui cumulent les discriminations ; ensuite, aller le plus haut possible. Liberté, Moyens libérés et Principes ne comptent pas, car le modèle y gagne par définition ; un score n'est classé que si la fidélité aux principes reste d'au moins 75 %. Le système fermé n'est qu'une hypothèse de calcul, pas un principe : ouvrir le pays ne coûte aucun point de principe.

À côté du score, la **légitimité** dit à quel point on peut croire ces points : un point appuyé sur un chiffre sourcé ou un précédent réel compte en entier, sur une hypothèse à moitié, sur un jugement à 30 %. La France, qui existe, a 66 de légitimité ; le modèle de départ 37, et environ 55 une fois ouvert et réglé. Chaque source ajoutée au jeu peut la faire monter.

## Les jauges

Toutes les jauges sont reliées en réseau : chaque choix touche toutes les branches auxquelles il se rapporte (160 liens, chacun avec sa raison et son statut : sourcé, hypothèse ou jugement). Une règle pour tous les liens : ils mesurent ce que les règles font subir aux gens, jamais ce qu'un groupe ferait ou serait supposé faire. Dans l'atelier, chaque jauge affiche les liens qui la font bouger ; la [matrice des effets](https://labo-de-la-bande.github.io/matrice/) les montre tous.

Une bannière fixe en haut de la page affiche treize jauges, du rouge (fragile) au vert (solide), sur deux lignes. Elles réagissent à chaque décision. Un clic sur une jauge mène à la table correspondante ; le détail s'affiche au survol.

| Jauge | Ce qu'elle mesure |
|---|---|
| Sécurité | Part des attaques du stress test sans police qui tiennent, plus les effets du réseau trop petits pour faire basculer un verdict (la médiation, par exemple) |
| Criminalité | Une fourchette : de l'hypothèse pessimiste (sans police ni prison, homicides +16 % comme lors des grèves de police au Brésil) à l'hypothèse optimiste (prévention, socle et réparations qui marchent). 50 % correspond à la France d'aujourd'hui (976 homicides en 2024) |
| Liberté | Personnes privées de liberté : aucune prison dans le modèle, contre 90 020 détenus en France. Le modèle part de 90 : les contraintes (gardes, surveillance, sobriété…) la font baisser, les capacités (partir, décider, avoir un toit) la font monter |
| Moyens libérés | Part du budget des prisons et de la police (29,5 Md€ en 2025) réaffectée à la prévention, aux soins et au socle, une fois payés les gardes, la médiation et les soins |
| Ressources | Ressources couvertes dans le test de fermeture, par la production ou par import |
| Niveau de vie | Service rendu par l'énergie disponible par personne (comparé à aujourd'hui), multiplié par la part des personnes dont les besoins de base sont couverts. Pour la France d'aujourd'hui : 85 %, car 15,4 % de la population vit sous le seuil de pauvreté (Insee, 2023), avec un écart de 3,5 entre les 10 % les plus aisés et les 10 % les plus modestes. Dans le modèle, la part couverte dépend aussi de la monnaie et de l'écart de revenus |
| Nourriture | Personnes que les terres peuvent nourrir (rapportées à la population, selon l'ouverture du pays), multiplié par la part des personnes qui mangent à leur faim (selon la monnaie et l'écart de revenus : en nature, seules manquent les personnes que le socle n'atteint pas ; avec une monnaie, la part qui manque suit l'écart de revenus). Pour la France d'aujourd'hui : 88 %, car 12 % de la population dit ne pas avoir assez à manger (Crédoc, 2024) |
| Contre-pouvoirs | Inverse du risque de prise du pouvoir (table Politique). France : 45, un jugement daté d'octobre 2026 : élections et presse libre, mais une justice à deux vitesses constatée depuis les gilets jaunes (2018-2019), puis en 2023 et en octobre 2026. Valeur à revoir régulièrement |
| Passage à l'échelle | Statut de l'épreuve des paliers : réduit, sur le papier, ou vert à confirmer |
| Échanges | Ce qu'on offre en échange, rapporté à ce qu'on importe |
| Solidité des paris | Inverse du risque pris en misant sur des solutions peu mûres |
| Égalité | Intersectionnelle : écarts selon l'origine, le genre, la classe, les convictions religieuses, le handicap et le territoire, outre-mer compris, notés sur le groupe le plus mal loti et non sur la moyenne. France : 35 (jugement : 77 % des habitants de Mayotte sous le seuil de pauvreté national, Insee ; contrôles au faciès 20 fois plus fréquents pour les jeunes hommes perçus comme noirs ou arabes, Défenseur des droits). Modèle : 45 au départ |
| Fidélité aux principes | Baisse avec la place de la monnaie, une armée ou des gardes renforcées, la centralisation, l'héritage libre et la grande industrie. L'ouverture du pays n'y compte pas : la fermeture n'est qu'une base de calcul |

À gauche de la bannière, sous le score, un bouton ouvre les options (par défaut, les jauges sont comparées à la France d'aujourd'hui) :

- un **sélecteur de temps** (aujourd'hui, 10 ans, 30 ans) fait avancer le modèle : population projetée par l'Insee, paris qui arrivent à maturité ;
- **« Comparer à »** place sur chaque jauge un repère de la France : **aujourd'hui** (trait plein), selon la **tendance** actuelle (trait creux) ou selon les **objectifs officiels** (trait bleu). Les projections n'apparaissent que là où il existe des données : niveau de vie (−0,7 % d'énergie par an depuis 2012, corrigé du climat ; objectif de loi −50 % en 2050), nourriture (terres perdues, population projetée, et demande d'aide alimentaire qui continue de croître comme depuis 20 ans : les Restos du Cœur sont passés de 81,5 à 161 millions de repas entre 2006-2007 et 2024-2025) et, à 10 ans, liberté (hausse de 5 % des détenus par an). Certains de ces repères sont sourcés (énergie, échanges), d'autres sont des jugements (sécurité, contre-pouvoirs, fidélité aux principes) ; le détail s'affiche au survol.

Chaque valeur France porte sa date (au survol dans la bannière, entre parenthèses dans l'atelier) : la liste de ce qu'il faut revoir, et quand, est dans [MISES-A-JOUR.md](MISES-A-JOUR.md).

Les formules des jauges sont des choix du jeu : elles montrent dans quel sens va chaque décision, pas une mesure.

## Partager son modèle

Dans la table « Ton modèle », le bouton **Créer le lien de partage** fabrique un lien qui contient tous les choix et réglages. Ce lien ouvre une page de synthèse : les jauges, les choix et leur prix, la comparaison avec aujourd'hui et le bilan. La personne qui le reçoit peut ensuite explorer le détail ou repartir de ce modèle.

Rien n'est envoyé à un serveur : les choix restent dans le navigateur de chacun, et seules les personnes à qui on donne le lien voient le modèle.

## Comment lire les chiffres

Comme sur Wikipédia, les numéros entre crochets renvoient à la liste des sources en bas de la page ; la flèche ↑ ramène au texte. La mention « réf. nécessaire » signale un chiffre encore sans source vérifiée.

Chaque chiffre porte aussi un statut :

- **sourcé** : une source vérifiable est citée (liste en bas de la page) ;
- **hypothèse** : un chiffre ou un effet choisi pour le jeu, pour explorer ce qu'il implique ;
- **jugement** : un avis argumenté, jamais une mesure.

Les verdicts (*tient*, *se fissure*, *casse*, *ouvert*) et les jauges sont des jugements et des formules du jeu, pas des mesures. Les faits marqués « de mémoire » restent à vérifier.

## Contribuer : un jeu qui évolue

Le jeu progresse avec ses participants. Chaque chiffre encore sans source porte la mention **[réf. nécessaire]** : un clic ouvre le formulaire [« Proposer une source »](https://github.com/labo-de-la-bande/labo-de-la-bande.github.io/issues/new?template=source.yml), déjà rempli avec le passage concerné. Dans la liste des sources, le lien **signaler** ouvre le formulaire [« Signaler une erreur »](https://github.com/labo-de-la-bande/labo-de-la-bande.github.io/issues/new?template=correction.yml).

Une bonne source est, par ordre de préférence : un organisme officiel ou statistique (Insee, ministère, SDES…), une étude publiée, le rapport d'une association reconnue, ou un article de presse qui cite l'une d'elles. Le chiffre doit figurer dans le document.

Chaque proposition est vérifiée avant d'être ajoutée. Si une source contredit le jeu, on corrige le chiffre, même quand le score du modèle baisse.

Il faut un compte GitHub ; un pseudonyme suffit. Tout le jeu tient dans un seul fichier, `index.html`, sans dépendance à installer : pour l'essayer en local, il suffit de l'ouvrir dans un navigateur.

## Licences

- Le code est sous licence **GPL-3.0** (fichier `LICENSE`).
- Les textes et les contenus du jeu sont sous licence **CC BY-SA 4.0** (fichier `LICENCE-TEXTES.md`).

Chacun peut réutiliser et modifier le jeu, à condition de le repartager sous les mêmes licences.
