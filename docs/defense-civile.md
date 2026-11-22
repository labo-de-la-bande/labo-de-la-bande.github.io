# La défense civile : document de conception

Partie annexe de la tranche Sécurité. Rien n'est codé. Chaque point dit d'où il vient :
**[auteur]** une décision de l'auteur, **[assistant]** une proposition à contester.

## Les décisions prises

- **[auteur]** Un jeu par sujet, plus tard : on *joue* le choix au lieu de le cocher. On commence
  par la sécurité.
- **[auteur]** La défense civile non violente, parce que c'est le plus cohérent avec le modèle.
- **[auteur]** On l'ouvre depuis la situation « Une armée étrangère menace le pays » : choisir la
  défense civile lance la partie, dont l'issue revient dans le modèle.
- **[auteur]** L'occupant **impose** la collaboration : travail forcé sous la menace d'être
  fusillé, otages, représailles. Exemple : la base de sous-marins de Keroman, à Lorient
  (témoignage de l'auteur, source `temoignage-auteur-lorient`).
- **[auteur]** L'échelle : **la région** choisie à l'accueil.
- **[auteur]** Le rythme : **une partie courte**.
- **[auteur]** L'occupant est fictif ; ses méthodes sont historiques et sourcées. Il n'est pas
  forcément un pays (c'est l'assistant qui avait parlé d'un pays) : ce peut être un simple
  groupe armé.

## La boucle [assistant]

- **But** : rendre la région ingouvernable jusqu'à ce que l'occupation coûte plus qu'elle ne
  rapporte. On gagne quand l'occupant renonce ; on peut aussi tenir sans gagner, ou céder.
- **Durée** : une douzaine de tours d'un mois, soit une première année d'occupation (de 10 à
  20 minutes de jeu).
- **Le terrain** : la coupe de vallée déjà dessinée (montagne, forêt, champs, village, ville, mer),
  avec les cellules de la région. Plus tard, un profil par région (côte et ports, montagne,
  île d'outre-mer).
- **Ce dont l'occupant a besoin** : administration, énergie, transports, information,
  main-d'œuvre. Son contrôle de chaque zone dépend de la collaboration qu'il obtient (la thèse de
  Gene Sharp : le pouvoir repose sur l'obéissance).
- **À chaque tour, le joueur organise** (il ne décide jamais de la mort de quelqu'un) :
  - *protester* : manifestations, symboles, refus visibles ;
  - *ne pas coopérer* : grève, refus administratif, boycott, travail ralenti ;
  - *intervenir* : blocages, institutions parallèles (écoles, soins, registres hors d'atteinte) ;
  - *protéger* : cacher les réfractaires, soutenir les familles des personnes contraintes,
    sabotage discret qu'on ne peut attribuer à personne, documenter les crimes pour la justice
    d'après.
- **L'occupant répond** par des mesures historiques : réquisitions, travail forcé sous la menace,
  couvre-feu, otages et représailles collectives, fichiers, déportations. Les représailles peuvent
  briser la résistance ou la renforcer en retournant l'opinion (le « jiu-jitsu politique » de
  Sharp) : le jeu montre les deux, et les morts, sans conditionnel.
- **Céder est une option honnête** : obéir pour survivre n'est pas une faute, et le jeu ne le punit
  pas moralement. Les personnages ont des avis, et s'en souviennent.
- **Les ressources du joueur** sont humaines : la participation, la cohésion, les savoir-faire,
  les relais à l'étranger, la confiance des habitants.

## Le lien avec la partie principale [assistant]

| Décision de la partie principale | Effet dans la défense civile |
|---|---|
| Défense civile choisie à l'invasion | les cellules sont formées : la participation part plus haut |
| Coordination par IA ou salle Cybersyn | un centre à saisir : l'occupant prend tout d'un coup |
| Registre des manques, plateforme décentralisée | rien à saisir d'un coup, les réseaux tiennent |
| Surveillance généralisée | les fichiers tombent aux mains de l'occupant (`seltzer`) |
| Énergie décentralisée | plus difficile à contrôler qu'une centrale |
| Confiance des habitants | les personnages suivent, ou non |

En retour, l'issue agit sur les jauges (sécurité, liberté, égalité, moyens libérés) et sur la
confiance des habitants.

## Les données [assistant]

Comme le reste du jeu, en CSV, avec statut et provenance ; aucun nombre dans le code.

- `data/defense/methodes.csv` : les actions du joueur (famille, participation requise, risque,
  effet sur le contrôle, sources) ;
- `data/defense/mesures.csv` : les mesures de l'occupant (déclencheur, effet, sources ou
  témoignages) ;
- `data/defense/scenes.csv` : les répliques des habitants, comme dans les scènes ;
- les coefficients dans `data/coefficients.csv`, en hypothèses tant qu'ils ne sont pas sourcés.

## Les sources (vérifiées le 21 novembre 2026)

Dans `sources.csv`, chacune avec ce qui en a été lu et ses limites :

- `stephan2008`, `chenoweth` : 323 campagnes de 1900 à 2006, 53 % de réussite pour les non
  violentes contre 26 % pour les violentes ; pas de taux séparé pour les occupations dans ce qui a
  été lu ;
- `sharp1973` : le pouvoir repose sur l'obéissance ; 198 méthodes en trois familles ;
- des cas : `norvege1942` (les enseignants face au régime de Quisling, qui renonce),
  `fischer2003` (la Ruhr, 1923, résistance passive puis abandon), `tchecoslovaquie1968`
  (résistance spontanée, occupation maintenue), `lituanie1991` (janvier 1991, puis une défense
  fondée sur la défense civile), `kosovo1992` (institutions parallèles) ;
- la coercition : `temoignage-auteur-lorient` et `lorient-keroman` (l'Organisation Todt, et
  jusqu'à 22 285 travailleurs fournis par le service du travail obligatoire à Lorient) ;
- à lire encore : Howard Clark, *Civil Resistance in Kosovo* (2000) ; des historiens du travail
  forcé et des otages en France.

Face à une armée décidée, les preuves d'efficacité de la défense civile sont minces : l'option
reste un **monopole**, elle tient en théorie et attend sa preuve, et le jeu le dit.

## L'occupant et ses raisons (en discussion)

- **[auteur]** On doit pouvoir identifier l'occupant et ses raisons. Dans une société sans
  monnaie, sans patriarcat, sans police ni prison, il n'y a pas de raison d'user de la violence ;
  l'invasion dit donc quelque chose de la manière dont l'*autre* endroit est géré. Piste : un
  joueur qui a décidé d'envahir d'autres pays pour obtenir des ressources.
- **[assistant]** L'occupant serait une autre société du même modèle, dont les choix la poussent
  à envahir : le jeu calcule ses raisons avec les mêmes jauges. Trois raisons, chacune avec un cas
  historique :
  - **les ressources** : une société qui manque d'énergie ou de matières (la jauge des ressources,
    la contrainte thermodynamique) les prend chez le voisin ; la Ruhr, en 1923, est occupée pour
    son charbon (`fischer2003`) ;
  - **la position** : des côtes, des ports, un passage ; Keroman est construite parce que Lorient
    ouvre sur l'Atlantique (`lorient-keroman`) ;
  - **écraser un modèle qui fait envie** : en 1968, l'invasion de la Tchécoslovaquie arrête une
    réforme que le pouvoir voisin jugeait contagieuse (`tchecoslovaquie1968`).
- **[assistant]** Ce qui rend une société plus encline à envahir se lit dans ses règles, pas dans
  ses gens : un pouvoir concentré qui gagne à la guerre (la loi d'airain), une armée permanente,
  la dépendance aux importations ; et, en hypothèse exploratoire, l'inégalité entre femmes et
  hommes (`hudson2012`, causalité non établie). Le lien avec « chaque joueur sa région » : à
  plusieurs, l'occupant peut être un autre joueur.
- **[assistant]** Une nuance à discuter : sans monnaie ni police, une société a moins de raisons
  *internes* de violence ; mais elle n'empêche pas un voisin d'en avoir, et une société sans
  armée est une cible facile (la lentille réaliste). C'est ce que la défense civile doit rendre
  coûteux.
- **[auteur]** Décision : **la raison de l'occupant dépend de la région et de la situation
  politique actuelle**, donc elle se met à jour, comme les chiffres de la France.
- **[assistant]** Proposition de forme, à discuter avant d'écrire la moindre ligne :
  - `data/pays/fr/convoitises.csv` : ce qu'une région a de convoité (`region`, `nature` parmi
    ressource, position, modèle, `texte_fr`, `sources`, `statut`, `date`, `revoir`, `provenance`).
    Par exemple, pour la Bretagne, des ports sur l'Atlantique ; pour une région d'outre-mer, sa
    position et sa zone maritime. Chaque ligne datée et sourcée ;
  - la situation politique actuelle en faits datés et sourcés (dépendances, routes, tensions), pas
    en opinions ; l'occupant reste fictif, sans nom de pays réel : ce sont les enjeux qui sont
    réels ;
  - un suivi dans `MISES-A-JOUR.md`, avec un rythme de révision (au moins une fois par an, et à
    chaque grand événement), et `labo doctor` qui avertit quand une date de révision est passée.

## Qui peut occuper : au-delà d'une armée étrangère

- **[auteur]** Il peut y avoir des **conflits entre régions** ; différents joueurs peuvent jouer
  différentes régions. **L'État** aussi peut entrer en jeu.
- **[assistant]** Trois sortes d'occupants, chacune avec ses raisons et ses moyens :
  - **un groupe armé** (milice, bande organisée) : il veut une ressource, un trafic, un
    territoire ; il a peu d'administration, et la coercition directe pèse d'autant plus ;
  - **une autre région**, jouée par un autre joueur ou par le jeu : ses raisons se calculent avec
    les mêmes jauges (elle manque d'énergie, de nourriture, de ports) ;
  - **l'État** : une région qui s'organise sans police ni prison peut le voir revenir pour
    reprendre la main. La Bretagne en a l'expérience, comme la France : à Plogoff (1978-1981,
    `plogoff`) et au Larzac (1971-1981, `larzac`), des résistances civiles ont fait reculer
    l'État. C'est sans doute le cas le plus probable, et le plus clivant : il demande la même
    rigueur sur les faits.

## La Bretagne, première région (données proposées)

`data/pays/fr/convoitises.csv`, neuf lignes proposées par l'assistant, chacune sourcée et datée,
**validées par l'auteur (pseudonyme Trashooters) le 22 novembre 2026** (colonne `valide_par`) ; `labo doctor` les compte et avertit quand
une date de révision est passée :

- une position : les ports atlantiques, Brest et Lorient (`lorient-keroman`,
  `temoignage-auteur-lorient`) ;
- une position : la base de l'Île Longue et les sous-marins de la dissuasion (`ile-longue`) ;
- une ressource : l'élevage, 55 % des porcs et 23 % du lait du pays (`draaf-bzh-2025`) ;
- une ressource : le parc éolien en mer de Saint-Brieuc, environ 9 % de l'électricité de la
  région (`saint-brieuc-eolien`) ;
- une faiblesse : la région ne produit que 31 % de l'électricité qu'elle consomme
  (`insee-bzh-energie`) ;
- une force : une résistance civile déjà gagnée, Plogoff (`plogoff`).
- une ressource : la pêche, 40 % des navires de pêche de métropole sur la façade Bretagne et
  Pays de la Loire (`dirm-namo-peche`) ;
- une position : onze îles habitées, 11 200 habitants, reliées par bateau (`insee-iles-bzh`) ;
- une faiblesse : Sein, Molène et Ouessant, hors du réseau électrique, au fioul
  (`iles-energie-2022`).

## Le premier scénario : l'État et la dépendance électrique

- **[auteur]** L'occupant de la première version : **l'État**. Le moyen de pression : la
  **dépendance électrique** de la Bretagne. Mais « ça dépend, et ça peut aussi être aléatoire ».
- **[assistant]** Proposition pour concilier les deux :
  - **un tirage rejouable** (avec la graine de la partie) parmi les couples occupant × convoitise
    possibles pour la région ; les poids dépendent des données de la région (`convoitises.csv`)
    et de l'état de la partie. Exemple : une région qui a choisi une énergie décentralisée est
    moins dépendante, donc la pression électrique pèse moins et devient moins probable ;
  - **Labo dit pourquoi cet occupant et cette pression**, avec les sources : rien n'est tiré en
    cachette ;
  - pour la première version, un seul couple est écrit en entier : l'État et la dépendance
    électrique ; les autres viendront avec leurs données.
- **[assistant]** Ce que ce scénario change par rapport à une armée étrangère : la pression n'est
  pas d'abord la violence, c'est la coupure (la Bretagne produit 31 % de l'électricité qu'elle
  consomme, `insee-bzh-energie`), puis, si la région tient, les moyens de l'État (forces de
  l'ordre, procédures, arrestations), à documenter sur des cas réels (Plogoff, le Larzac) avec la
  même rigueur. Les habitants le vivent tout de suite : Jeanne et le froid, Inès et l'hôpital.
- **[assistant]** Ce que la région peut opposer : la sobriété organisée, sa propre production
  (l'éolien en mer de Saint-Brieuc, `saint-brieuc-eolien`), la priorité aux soins et aux plus
  fragiles, la solidarité d'autres régions (d'autres joueurs, plus tard), la négociation, et
  l'opinion du reste du pays.

## Les actions et les mesures du premier scénario (validées)

`data/defense/actions.csv` (dix actions de la région) et `data/defense/mesures.csv` (six mesures
de l'État), scénario `etat-electricite` : proposées par l'assistant, **validées par l'auteur
(Trashooters) le 22 novembre 2026**. Chaque ligne a un précédent réel et au moins une source ;
`labo doctor` refuse une ligne sans source et une validation par un assistant. Le délestage ciblé
sur une région reste une hypothèse (le plan de 2022 était national et n'a jamais été appliqué).

## Deux principes (auteur)

- **On ne compare pas l'État d'aujourd'hui à l'occupation de 1940.** Le scénario de l'État passe
  par le droit et la force légale (réquisitions, évacuations, poursuites) ; la menace de mort
  appartient aux scénarios de groupe armé ou de guerre.
- **Un scénario de guerre, avec des victimes, est possible tant que les personnages restent
  fictifs.** Les faits historiques qui l'appuient sont réels et sourcés ; les personnes du jeu
  sont inventées.

## La boucle de tour minimale [assistant, à valider]

Tout ce qui suit est une proposition. Les nombres sont des **hypothèses** de départ, faits pour
être contestés ; validés, ils iront dans `data/coefficients.csv` (statut « hypothèse », lentille,
raison), jamais dans le code.

### Trois jauges, de 0 à 100

| Jauge | Ce qu'elle mesure | Au départ |
|---|---|---|
| **Participation** | la part des habitants de la région qui agissent | 10, plus un bonus si la défense civile a été choisie et préparée dans la partie principale, plus un peu par point de confiance des habitants |
| **Électricité** | la part des besoins couverts | 100 avant la première coupure ; sous délestage, elle tombe vers la part que la région produit elle-même (31 %, `insee-bzh-energie`), que ses actions peuvent relever |
| **Opinion** | le soutien du reste du pays | 50 |

L'État a aussi un **niveau d'escalade** (de 0 à 5) qui ne s'affiche pas en jauge mais dans ce
qu'il fait : il commence par la coupure, puis monte si la région tient.

### Douze tours d'un mois, de novembre à octobre

L'hiver pèse : de novembre à mars, une électricité basse use la participation bien plus vite.

### Un tour

1. **La situation** : Labo montre les trois jauges, le mois et la dernière mesure de l'État ; les
   habitants réagissent (Jeanne et le froid, Inès et l'hôpital).
2. **La région agit** : deux actions au plus par tour (comme les deux décisions par révolution),
   parmi celles que la participation permet (une action « forte » demande plus de monde).
3. **Les actions agissent** sur les jauges (tableau ci-dessous).
4. **L'État répond** : il choisit une mesure parmi celles qui répondent à ce que la région vient
   de faire (une occupation appelle l'évacuation ; remettre le courant appelle la réquisition),
   par un tirage rejouable dont les poids dépendent du niveau d'escalade et de l'opinion. Plus
   l'opinion soutient la région, plus « négocier ou renoncer » devient probable.
5. **Le retournement** : une mesure de force contre une région restée non violente fait monter
   l'opinion en faveur de la région (le « jiu-jitsu politique » de Sharp, `sharp1973`) ; un
   rassemblement qui tourne à l'affrontement la fait baisser.
6. **Le froid** : si l'électricité est sous le seuil vital pendant l'hiver, la participation
   baisse, sauf si « les fragiles d'abord » ou la sobriété sont en place.
7. **La fin** : l'État renonce si l'opinion et la participation restent hautes trois mois de
   suite ; la région cède si la participation tombe sous un plancher ; sinon, au douzième mois,
   une issue négociée, plus ou moins favorable selon les jauges.

### Ce que font les actions (signes ; une hypothèse d'échelle : + = 5 points, ++ = 10)

| Action | Participation | Électricité | Opinion | Remarque |
|---|---|---|---|---|
| Grand rassemblement | ++ | | + | risque d'affrontement : alors, opinion −− |
| Radio libre | + | | + | l'émetteur peut être saisi |
| Occuper un lieu | + | + | | appelle l'évacuation |
| Refus d'appliquer | + | + | + | appelle la réquisition |
| Remettre le courant | | ++ | ± | appelle les poursuites ; l'opinion se divise |
| Sobriété organisée | | + | + | protège du froid |
| Les fragiles d'abord | + | | + | protège du froid |
| Produire sur place | | + (lent) | | effet qui s'accumule de mois en mois |
| Appel aux régions | | | ++ | dépend de l'image du mouvement |
| Saisir la justice | | | + | effet lent ; peut annuler une dissolution |

### Ce que font les mesures de l'État

| Mesure | Effet | Le retournement possible |
|---|---|---|
| Délestage ciblé | électricité vers la production locale | opinion + si le pays le juge injuste |
| Réquisition | annule « refus d'appliquer » et « remettre le courant » ce mois-ci | opinion + si les agents sont visés |
| Forces sur place | participation − | opinion + si la région reste non violente |
| Évacuation | met fin à une occupation ; participation − | opinion + |
| Dissolution | la radio et le collectif doivent se refaire ; participation − | la justice peut l'annuler |
| Négocier ou renoncer | fin de partie : issue négociée, ou abandon de la pression | |

### Ce qui revient dans la partie principale

L'issue agit sur les jauges du modèle (liberté, pouvoirs, sécurité, moyens libérés) et sur la
confiance des habitants, selon ce que la région a fait et ce qu'elle a subi ; les personnages s'en
souviennent dans leur histoire.

### Les coefficients à créer après validation

`defense-participation-depart`, `defense-bonus-preparation`, `defense-echelle-plus` (5),
`defense-actions-par-tour` (2), `defense-seuil-vital-electricite`, `defense-usure-hiver`,
`defense-seuil-renoncement` et sa durée (3 mois), `defense-plancher-participation`,
`defense-retournement` ; chacun en hypothèse, avec sa raison.
