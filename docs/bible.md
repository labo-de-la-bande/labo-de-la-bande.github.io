# Bible du jeu

Ce que le jeu doit être, avant d'écrire son moteur. Document de travail : chaque partie est une
proposition à discuter, et les questions ouvertes sont listées à la fin. Les chiffres et le
modèle restent dans `data/` ; ce document décrit l'expérience de jeu.

## 1. L'intention

Un **jeu de gestion** sur la durée : on règle une société sans prison, sans police et autant que
possible sans monnaie, et on vit ses conséquences. Le genre est celui des jeux de gestion et des
city builders ; les mécanismes, les personnages et l'univers sont les nôtres.

Ce qui le distingue :

- **chaque effet dit d'où il vient** : sourcé, hypothèse, jugement, et de quelle lentille il relève ;
- **le jeu montre ce que chaque choix fait subir aux gens**, jugé du point de vue de celles et ceux qui
  cumulent les difficultés ;
- **le modèle n'est jamais fini** : les données réelles changent chaque année, et les joueurs y
  apportent leurs sources et leur pensée.

## 2. Le temps : une révolution solaire par tour

Un tour est **une révolution de la Terre autour du Soleil**. Le compte est laïque et universel, et
le mot porte deux sens que le jeu assume : la révolution astronomique et la révolution politique.
Une partie se raconte en révolutions (« révolution 7 : la première cellule a voté son socle »).

La partie commence dans la France d'aujourd'hui, avec ses vraies données (la révolution 0 est
l'année en cours). Les tendances réelles continuent pour la France de comparaison ; les choix du
joueur font diverger sa société.

## 3. La boucle d'une révolution

1. **L'état du monde** : le territoire, les jauges, ce qui a changé depuis la dernière révolution.
2. **Quelques situations, jamais une liste** : une crise, un dilemme ou une occasion, deux ou trois
   par révolution au plus, choisis selon l'état de la société (le mini-langage des conditions sert
   à les déclencher). Les 35 leviers et les centaines de liens du modèle restent sous le capot ; le
   joueur les rencontre un à un, au moment où ils comptent.
3. **Le conseil** : les conseillers (les penseurs, le robot, éventuellement d'autres joueurs)
   commentent la situation, chacun selon sa lentille. Ils se contredisent : c'est voulu.
4. **La décision** : le joueur choisit. Avant de choisir, il voit l'effet probable et ce qu'en
   diraient les habitants (le jeu actuel le fait déjà).
5. **Les conséquences** : elles se voient sur le territoire, dans les jauges, la réputation et la
   voix des habitants ; certaines arrivent plus tard (un pari mûrit, une norme change lentement).
6. **Le bilan de la révolution** : viable ? fidèle au but ? mieux que la France ?

## 4. Les personnages

### Labo, le robot

Un robot de notre invention, nommé **Labo** (nom provisoire), qui se présente ainsi : « Je propose, tu vérifies. » C'est la racine du projet
personnifiée, le même nom que l'outil qui vérifie et assemble le modèle (`labo`). Ce n'est aucun
assistant existant ; il ne porte le nom ni l'image d'aucun produit. C'est l'assistant du joueur. Il calcule, propose, met en forme, et **signe toujours
ce qui vient de lui** : un classement, une lecture ou une aide qu'il apporte est marqué comme sa
proposition, jamais comme un fait. Il peut se tromper, et le joueur peut le contredire. Il incarne
une règle du projet : l'orientation d'un outil d'apprentissage automatique doit rester visible.

### Les penseurs

Les lentilles incarnées en caricatures bienveillantes : Marx, Proudhon, Ostrom, Hayek, Federici,
Geddes et Howard, Davis, Gilmore, Graeber, Chenoweth, Neurath, Georgescu-Roegen… (liste dans
`data/lentilles.csv` et `CHANTIERS.md`, à équilibrer avec des adversaires du projet).

Deux règles :

- **aucune phrase inventée** : une réplique est une citation sourcée ou une paraphrase marquée
  comme telle, avec sa référence ;
- **les auteurs vivants** sont dessinés avec un style franchement stylisé, jamais photoréaliste.

Chaque penseur porte un **indicateur de cohérence** : à quel point la société du joueur suit sa
lentille. Plaire à un penseur n'est pas gagner.

### Les habitants

Les voix déjà présentes dans le jeu, à développer en personnages avec un lieu, un métier et une
situation :

| Habitant | Ce qu'on entend déjà |
|---|---|
| Jeanne | l'âge, le village, « une liste par cellule, comme à l'époque » |
| Malik | le concret des importations, le pain, les médicaments |
| Sam | le doute, la curiosité, « du vent et du soleil, enfin ! » |
| Inès | le soin, l'hôpital, les limites techniques |
| Bruno | l'agriculteur : engrais, gazole, récoltes |
| Aïcha | l'accueil, la formation, la cellule qui agit |

À compléter pour que les groupes qui cumulent les difficultés soient présents en personnes, pas
en statistiques (outre-mer, handicap, personnes sans papiers, jeunes des quartiers relégués…).

### Les joueurs

Des avatars personnalisés. Un joueur qui apporte une idée au modèle peut voir son avatar devenir
un conseiller, aux côtés des penseurs (voir § 6).

## 5. La réputation

Un compte de points selon les décisions, tenu **auprès des groupes d'habitants** : la confiance de
chaque groupe monte ou baisse selon ce que les décisions lui font. On peut avoir la faveur d'un
penseur et perdre celle des plus fragiles : c'est l'arbitrage que le jeu rend sensible. La
réputation pèse sur la suite (une cellule qui ne fait plus confiance coopère moins).

## 6. Les monopoles et la pensée des joueurs

Le **monopole magnétique** de Dirac (1931) est cohérent en théorie et n'a jamais été observé. Le
jeu appelle « monopoles » les affirmations du modèle qui tiennent sur le papier et attendent leur
preuve : beaucoup d'hypothèses et de jugements, et la société elle-même (aucune société de cette
taille n'a fonctionné ainsi).

- Un monopole se résout quand on trouve un précédent réel, une étude ou une contre-preuve : il
  devient un fait sourcé, ou il tombe. C'est une quête du jeu, et une façon réelle d'améliorer le
  modèle.
- **Un joueur apporte aussi sa pensée** : une solution, un raisonnement, une critique, sa propre
  lentille. Son idée entre comme un jugement signé de son avatar, c'est-à-dire comme un nouveau
  monopole, que d'autres peuvent étayer ou contredire. Le robot peut l'aider à la mettre en forme,
  en signant son aide.
- Rien ne pèse sur les scores sans relecture humaine.

## 7. L'univers et les décors

**L'univers physique comme socle.** Ce qu'on connaît de notre univers sert de décor et de contrainte :
les révolutions solaires, l'énergie, l'entropie. Les lois physiques bornent ce qu'une société peut
produire ; elles ne décident pas de son organisation (c'est la place des lentilles).

**La coupe de vallée de Geddes comme structure des décors** : de la montagne à la mer, chaque étage a
son milieu et ses métiers (la mine et la carrière, la forêt, le pâturage, les champs, le village,
la ville, le port). Le territoire du joueur se lit comme une coupe, et ses cellules s'y installent.

**Les contextes** : la métropole et l'outre-mer (qui change les ressources, le climat, l'histoire
coloniale), la ville et la campagne, la France fermée ou ouverte sur le monde.

## 8. La direction artistique et la chaîne de production des images

Le dessin viendra de collaborateurs ; on commence par de la génération d'images, qu'ils
reprendront. La chaîne est pensée comme les données : chaque image a un statut, et le statut
progresse.

1. **Une charte d'abord** : palette (celle du jeu), trait, proportions des caricatures, règles pour
   les personnes réelles. Elle vaut pour les images générées comme pour les dessins.
2. **Des fiches personnages en données** : un fichier par type d'image (`data/art/personnages.csv`,
   puis décors…) avec, pour chacun, son rôle, ses traits visuels, ses références iconographiques
   libres de droits (portraits du domaine public pour les penseurs disparus), le texte de
   génération, et le **statut de l'image** : à faire, générée, retouchée, dessinée, validée, avec
   l'auteur et la licence. `labo doctor` vérifie qu'aucun personnage ne manque d'image.
3. **Des formes simples d'abord** : silhouettes et couleurs générées par le code, pour que le moteur
   et la tranche jouable avancent sans attendre les images.
4. **La génération** : les textes de génération sont versionnés ; on garde l'outil utilisé, la
   graine et les conditions d'utilisation. Les images générées sont signalées comme telles dans les
   crédits.
5. **La reprise par les collaborateurs** : une image générée passe à « retouchée » ou « dessinée »
   quand un collaborateur la reprend, avec son nom dans les crédits.

**Licences** : les textes et données sont en CC BY-SA 4.0, le code en GPL-3.0. Il faut choisir une
licence pour les images, et vérifier pour chaque outil de génération ce que ses conditions
permettent.

## 9. La narration

Des littéraires écriront la narration ; en attendant, l'auteur et un assistant l'écrivent
ensemble. Pour que la relève soit simple :

- **tous les textes du jeu sont des données** (situations, répliques des habitants et des
  conseillers, récits de révolution), dans des CSV que l'on peut reprendre sans toucher au code ;
- **chaque texte porte sa provenance et son état** : brouillon d'un assistant, écrit par l'auteur,
  repris par tel ou telle, relu ; comme les images (§ 8) et les chiffres ;
- **une charte d'écriture** à poser avec eux : le ton de chaque habitant, la manière de citer les
  penseurs (citation sourcée ou paraphrase marquée), le français clair du jeu ;
- la règle de fond reste : les textes montrent ce que les choix font aux gens, jamais ce qu'un
  groupe ferait ou serait supposé faire.

## 10. La technique

- Le **moteur actuel** (`src/Labo.Engine`) reste le cœur : un état donne les jauges.
- Un **moteur de jeu** en F# (`Labo.Jeu`) posé dessus : les révolutions, le choix des situations, les
  conseillers, la réputation, les monopoles. Pur et testable comme le moteur ; les situations et
  les répliques en CSV.
- L'**affichage** en F# compilé par Fable, d'abord en 2D (canvas), avec les formes simples du § 8.
- La page actuelle (`index.html`) reste le jeu en ligne tant que le nouveau n'est pas prêt.

## 11. Questions ouvertes

- Le nom définitif de Labo, le robot (provisoire pour l'instant).
- La durée d'une partie (combien de révolutions ?) et ce qui la termine.
- Joue-t-on seul, à plusieurs sur la même société, ou chacun la sienne avec des échanges ?
- Comment représenter les lentilles qu'on éteint ou qu'on superpose.
- La licence des images et le choix des outils de génération.
- Ce que voit un joueur la première fois : la première révolution doit apprendre à jouer sans
  écran d'explications.

## 12. Ce que la première partie a montré (21 novembre 2026)

Le moteur de jeu (`src/Labo.Jeu`) et sa version texte (`tools/Labo.Jouer`) déroulent la tranche
« Nourrir et chauffer » de bout en bout : constats, dilemmes, conseil, voix, réputation, bilan.

- **Le conseil fonctionne sans phrase inventée** : chaque lentille parle avec les liens qu'elle porte
  (leur raison, leur statut), et signale quand son classement vient de l'assistant. Pour l'instant
  ce sont des lentilles, pas encore des personnages : les penseurs prendront la parole quand leurs
  répliques (citations sourcées ou paraphrases marquées) seront écrites.
- **Le score bouge trop peu** (de 63,7 à 64,1 en trois révolutions, contre 80,4 pour la France) :
  le joueur ne sent pas que ses choix comptent. Les jauges, elles, bougent fort (Ressources
  +55, Échanges −79 en ouvrant les échanges). À travailler : montrer les jauges plutôt qu'une
  moyenne, et des conséquences qui arrivent plus tard.
- **Rien ne se passe entre deux décisions** : pas d'événement, pas de temps qui agit. Les situations
  déclenchées par l'état (la colonne `quand` existe déjà) et les paris qui mûrissent sont la suite
  logique.
- **La réputation se lit bien** (« Bruno : confiance +1 »), mais les positions des habitants ont été
  lues dans leurs répliques par l'assistant : à vérifier par l'auteur, puis par les littéraires.
- Choisir une option fait aussi disparaître les effets de l'option de départ : le conseil le dit
  (« ne joue plus »), sinon la lecture s'inverse.

## 13. Le mouvement dans le temps (21 novembre 2026)

- **Une partie enchaîne les tranches** dans l'ordre du parcours (Nourrir et chauffer, Foyers,
  Égaux, Qui décide ?, Sécurité) : une douzaine de révolutions, de quoi laisser agir le temps.
  `labo-jouer` sans argument joue tout le parcours ; avec une tranche, cette tranche seule.
- **Les décisions s'installent** : une part de leur effet tout de suite, le reste à la révolution
  suivante (`jeu-inertie-revolutions`). L'aperçu donne le score « aussitôt » et « à terme ».
- **Les événements** (`data/jeu/evenements.csv`) surviennent quand l'état de la société les
  appelle (conditions sur les leviers et sur les jauges) ou par hasard, au plus un par
  révolution. Leurs effets durent quelques révolutions et touchent la confiance des habitants.
  Les dix premiers sont des brouillons de l'assistant, signés comme tels.
- **Les paris** se choisissent dans une situation dédiée et mûrissent le nombre d'années qu'ils
  demandent après avoir été choisis ; le journal annonce leur arrivée.
- **Le hasard est rejouable** : une graine, affichée au début, rejoue la même partie
  (`--graine N`). Sans hasard, seuls surviennent les événements certains (utile aux tests).

Ce que la partie complète montre : le temps agit (un cargo, un hiver rigoureux, une fête des
cellules), le score finit autour de 75 contre 79 pour la France, qui a elle aussi évolué. Un pari
choisi à la révolution 3 et prêt en 10 ans mûrit au moment du bilan, sans être vu : la partie est
trop courte pour les paris lents, ou il faut proposer les paris plus tôt.

## 14. La première interface (21 novembre 2026)

`src/Labo.Interface`, compilée par Fable dans `jeu/` (la page actuelle `index.html` reste le jeu
en ligne). Sans bibliothèque d'interface : un petit constructeur de page (`Html.fs`) et un canvas.
La compilation (`jeu/js/`) n'est pas versionnée : `dotnet fable src/Labo.Interface -o jeu/js`, puis
`labo servir`. La mise en ligne passera par une compilation automatique (GitHub Action) au moment
de la bascule.

- **L'écran** : l'en-tête de la révolution (une orbite, le numéro, la tranche, le score face à la
  France) ; la scène (le territoire, les constats, la situation) ; les jauges à droite, avec le
  repère de la France et la zone de l'effet envisagé.
- **Le territoire** en formes simples, sur la coupe de vallée de Geddes : la forêt dit les
  ressources, les champs la nourriture, les fenêtres allumées le niveau de vie, les bateaux les
  échanges, les cellules l'égalité ; le soleil avance avec les révolutions.
- **Choisir une option** montre aussitôt le score (aussitôt, à terme), les habitants et le conseil ;
  Labo, le robot, signe ce que l'assistant a rangé.
- **Les nouvelles** de chaque révolution (événements, paris mûris) s'ouvrent en premier plan.
- Le moteur compilé en JavaScript donne les mêmes nombres que l'oracle sur les 2 300 états
  (`labo verifier-js`), et la vérification échoue bien quand on fausse un coefficient.

À reprendre : les constats prennent beaucoup de place avant la décision ; le territoire reste
schématique ; aucun son, aucune animation de conséquence.

## 15. Les scènes (21 novembre 2026)

Avant une décision, une conversation : les habitants parlent, les lentilles paraphrasent, Labo
rappelle le modèle, et le joueur choisit ses répliques ou écrit sa propre idée
(`data/jeu/repliques.csv`, un petit graphe : qui parle, ce qu'il dit, d'où ça vient, ce qui suit).
`labo doctor` fait respecter les règles de chaque locuteur : un habitant « dit », une lentille ne
parle qu'en paraphrase rattachée à une source ou à un lien du modèle, Labo ne fait que des rappels
du modèle. La première scène porte sur la monnaie ; ses répliques sont des brouillons de
l'assistant. « J'ai une autre idée » ouvre un champ libre : l'idée entre au journal de la partie
comme une proposition (elle ne change pas le modèle) et peut être envoyée au projet par le
formulaire GitHub `idee.yml`.

## 16. Les prénoms (noté le 21 novembre 2026)

Pour nommer les habitants qu'on ne voit qu'en passant (les membres d'une cellule, les gens d'une
foule), une **banque de prénoms par pays**, avec les proportions réelles et le hasard. Pour la
France, le « fichier des prénoms » de l'Insee (naissances par prénom et par année depuis 1900,
licence ouverte) : tirer d'abord une année de naissance selon la pyramide des âges, puis un
prénom selon sa fréquence cette année-là, pour que les prénoms suivent l'âge des personnages.
L'Insee regroupe déjà les prénoms très rares, ce qui évite de désigner une personne réelle. Les
habitants principaux (Jeanne, Malik…) restent des personnages écrits.

Une chaîne de Markov sur les lettres inventerait des prénoms qui ressemblent aux vrais : utile pour
un pays imaginaire, ou pour des prénoms qu'aucune liste ne contient, mais moins fidèle qu'un
tirage dans les prénoms réels, et capable de produire des mots malheureux. À garder en réserve.

Ajouts du même jour, après le premier essai de l'auteur :

- **Du hasard dans les répliques** : plusieurs répliques possibles au même moment (même rang à
  l'entrée, ou plusieurs suites qui ne sont pas du joueur) ; le jeu en tire une avec la graine de la
  partie, donc rejouable.
- **La réaction après la décision** (`moment` = `apres`, `option`) : chaque option de la monnaie a sa
  scène.
- **Une relation qui se construit** : une réplique peut dépendre de la confiance d'un habitant
  (`si_confiance`, et elle l'emporte quand sa condition est remplie : Jeanne remercie le joueur qui
  l'a écoutée), et une réplique du joueur peut la changer (`confiance`).
- **Le mode apprentissage** : Labo indique l'option qui remonte le plus le maillon faible, ou le
  score quand la société est viable, en disant que ce n'est qu'un calcul.
- **Les attitudes** : après chaque décision, ceux qui ne parlent pas ont un geste, selon leur humeur
  tirée de ce qu'ils avaient dit (approuve, désapprouve, déception, soulagement, doute, neutre ;
  `data/jeu/attitudes.csv`, un geste au moins par habitant et par humeur, avec des variantes
  tirées au sort). Brouillons de l'assistant.
