# La dette : document de conception

Une société qui sort de la monnaie hérite de dettes. Neurath l'écrivait en 1919 : les dettes en
cours, surtout celles de l'État, rendent une transition sans monnaie bien plus difficile
(`neurath1919`). Graeber en a fait l'histoire (`graeber2011`). Rien n'est codé.
**[auteur]** une décision ou une position de l'auteur, **[assistant]** une proposition à
contester.

## Les faits de départ (sources vérifiées le 22 novembre 2026)

- La dette publique de la France : **3 460 milliards d'euros fin 2025, 115,7 % du PIB**
  (`insee-dette-2025`).
- Qui la détient : **environ 55 à 56 %** de la dette négociable de l'État est détenue par des
  non-résidents, que l'État ne peut pas identifier un à un (`aft-detenteurs`).
- Qui détient l'épargne : les 10 % de ménages les mieux dotés détiennent **48 % du patrimoine** ;
  l'assurance-vie, qui finance une part de la dette, pèse davantage chez eux, mais 42 % des ménages
  en ont une (`insee-patrimoine-2024`).
- Reste à trouver : la charge des intérêts payée chaque année, et la part de la dette publique
  qui revient, par l'épargne, à chaque tranche de patrimoine.

## Les positions

- **[auteur]** L'État fait beaucoup d'erreurs ; les intérêts rapportent surtout à des
  investisseurs qui ont plus d'argent que nécessaire. *Appuis possibles* : la concentration du
  patrimoine (`insee-patrimoine-2024`), la part des non-résidents, surtout institutionnels
  (`aft-detenteurs`). *Lentille* : matérialisme historique (proposition de classement).
- **[assistant]** La position opposée, pour que le jeu la montre aussi : une partie de la dette
  est financée par l'épargne de ménages modestes (assurance-vie, retraites), et une remise de
  dettes rend le crédit plus cher ensuite, parce que les prêteurs craignent la suivante.
  *Lentilles* : ordre spontané, réalisme.
- Le jeu ne tranche pas entre les deux : il montre sur quoi chacune s'appuie, et ce que chaque
  option fait aux gens.

## La question proposée : « Que faire des dettes ? » [assistant, à valider]

Dans « Nourrir et chauffer », après « Avec ou sans argent ? ».

| Option | Le précédent | Source |
|---|---|---|
| Honorer : rembourser par les exportations | la règle ordinaire | à documenter |
| Remettre les dettes des ménages, comme un jubilé | une trentaine de remises générales en Mésopotamie (2400-1400 avant notre ère) | `hudson2018`, `graeber2011` |
| Auditer, puis ne payer que ce qui est jugé légitime | l'Équateur : audit (2008), défaut, rachat vers 35 % de la valeur (2009) | `equateur-2008` |
| Refuser par référendum | l'Islande et Icesave : deux refus, puis la Cour de l'AELE donne raison à l'Islande (2013) | `icesave-2013` |
| Faire défaut | l'Argentine en 2001 | à sourcer |

Effets possibles dans le modèle, en hypothèses :

- honorer pèse sur les ressources (il faut exporter pour payer) ;
- remettre les dettes des ménages libère les plus endettés (égalité) ;
- auditer coûte du temps, et peut rendre la décision plus légitime (principes) ;
- refuser ou faire défaut ferme une partie des échanges (les créanciers ne prêtent plus et ne
  commercent plus), et renchérit le retour vers les marchés.

## À trancher

- les options gardées, et leurs effets ;
- s'il faut distinguer la dette de l'État et celle des ménages (deux questions, ou une) ;
- la Grèce de 2015 (un « non » au référendum, puis un accord) : à sourcer, comme l'Argentine.

## Décisions de l'auteur (22 novembre 2026)

- **Deux questions** : la dette de l'État et les dettes des ménages.
- **Toutes les options gardées**, avec la Grèce et l'Argentine (sources `grece-2015`,
  `argentine-2001`), et l'Équateur.

Ce qui est entré dans le jeu, en fin de tranche « Nourrir et chauffer » (avant le pari) :

- `dette-etat` : honorer, auditer (Équateur), référendum (Islande, Grèce), défaut (Argentine) ;
- `dette-menages` : les dettes restent dues, remise des dettes personnelles (comme en
  Mésopotamie), remise de toutes les dettes ;
- seize liens, tous signés « assistant », en hypothèses ou jugements, à contester ; des voix pour
  cinq habitants ; pas encore de scène de conversation.
- Deux liens réexprimés pour garder l'oracle (voir `CHANTIERS.md`, bornes souples) : « honorer »
  ne gagne plus d'échanges (ce sont les autres options qui en perdent) ; la transparence de
  l'audit porte sur la jauge « pouvoirs ». Depuis le 27 novembre 2026 (bornes souples pour toutes les
  jauges), le moteur permet leur forme d'origine : à reproposer.

## La dette qui arrange les créanciers (27 novembre 2026)

- **Idée de l'auteur** : il y a des cas où il arrange les investisseurs qu'une dette existe. Un
  créancier gagne sur les intérêts, donc tant que la dette court ; et tant qu'on lui doit, il
  pèse sur ce qu'on décide (en Grèce, les plans d'aide de 2010, 2012 et 2015, accordés contre des
  privatisations et des baisses de retraites). Graeber, et Maurizio Lazzarato (*La fabrique de
  l'homme endetté*, 2011), en font un moyen de gouverner.
- **Fait** (propositions de l'assistant, à contester) :
  - une option **« Faire rouler »** : payer les intérêts, réemprunter pour le capital ; la dette ne
    s'éteint jamais. Liens : pouvoirs −2 (hypothèse, `grece-2015`), égalité −2 (jugement, la rente,
    `insee-patrimoine-2024`), ressources −1 (hypothèse : exporter pour les intérêts seulement) ;
  - **« Honorer » coûte aussi en pouvoirs** (−1, hypothèse, `grece-2015`) ; sa description le dit ;
  - une voix (l'infirmière, contre : « payer un loyer à ceux qui ont prêté ») et deux répliques
    après le choix.
- **À sourcer** : « c'est ce que font la plupart des États » (`[réf:?]` : le programme d'émission
  de l'Agence France Trésor, qui réemprunte chaque année ce qui arrive à échéance) ; les plans
  grecs de 2010 et 2012 (la source `grece-2015` ne couvre que le troisième).
- **Contre-liens (27 novembre 2026, après la séparation du modèle et du jeu)**, propositions de
  l'assistant pour ne pas laisser une seule lecture :
  - « honorer » : pouvoirs +1 (hypothèse, réalisme, `grece-2015`) : un pays qui paie garde
    l'accès aux marchés sans conditions ; les conditions grecques sont venues quand le pays ne
    pouvait plus payer. Avec le lien d'en face (−1), l'effet net d'« honorer » sur les pouvoirs
    est nul : les deux lectures se valent faute de mieux ;
  - « faire rouler » : égalité +1 (hypothèse, à classer, `blanchard2019`) : tant que le taux
    d'intérêt reste sous la croissance, la dette se porte sans austérité. Effet net sur l'égalité :
    −1 au lieu de −2 ;
  - pas de contre-lien sourcé trouvé pour « faire rouler → pouvoirs −2 » ; il reste seul.
