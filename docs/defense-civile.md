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
- Ouvert : l'occupant. **[assistant]** Une puissance fictive, sans nom de pays réel, pour ne viser
  personne ; ses méthodes, elles, sont historiques et sourcées.

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
