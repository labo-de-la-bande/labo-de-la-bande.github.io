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

## Les sources à réunir

À vérifier avant d'entrer dans `sources.csv` :

- Erica Chenoweth et Maria J. Stephan, *Why Civil Resistance Works* (2011) ; la source
  `chenoweth` est déjà dans le jeu ;
- Gene Sharp, *The Politics of Nonviolent Action* (1973), et ses méthodes ;
- des cas : la résistance passive de la Ruhr (1923), les enseignants norvégiens sous
  l'occupation, la Tchécoslovaquie (1968), les pays baltes (1991), les institutions parallèles
  du Kosovo (années 1990) ;
- l'occupation en France : travail forcé, Organisation Todt, otages fusillés, avec des
  témoignages et des archives.

Face à une armée décidée, les preuves d'efficacité de la défense civile sont minces : l'option
reste un **monopole**, elle tient en théorie et attend sa preuve, et le jeu le dit.
