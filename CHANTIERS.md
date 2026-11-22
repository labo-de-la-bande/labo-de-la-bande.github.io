# Chantiers

Les grandes questions de conception encore ouvertes. Pour les chiffres à mettre à jour, voir [MISES-A-JOUR.md](MISES-A-JOUR.md).

## Refonte en F# et en métadonnées (décidée le 21 novembre 2026)

Un seul dépôt, une CLI F# (`labo`), les données en CSV, le moteur et l'interface en F# (Fable).
Inventaire, schéma et méthode : [docs/refonte.md](docs/refonte.md). Le comportement du jeu ne
change pas tant que le moteur F# ne reproduit pas l'actuel.

## Ce que le jeu assume d'être (noté le 21 novembre 2026)

**Un jeu de gestion**, dans la lignée de Tropico et des city builders : on règle une société, on
voit les conséquences, on arbitre entre des jauges. On ne le cache pas. Le genre seulement : les
mécanismes, les textes et l'apparence restent les nôtres, et la différence tient dans ce que le
jeu montre : des chiffres sourcés, le statut de chaque effet, et ce que chaque choix fait subir
aux gens.

## Vers un jeu graphique (noté le 21 novembre 2026)

Pour l'instant, la page HTML actuelle reste le jeu : elle sert à construire la base (données en
CSV, moteur F# vérifié contre l'oracle). Cette base ne dépend d'aucun affichage, et c'est voulu :
un jeu graphique (carte, cellules qui se forment, territoire qui change avec les choix, à la
manière des jeux de gestion) pourra se brancher sur le même moteur et les mêmes données. À
concevoir : ce qu'on voit (le territoire, les cellules, les flux d'énergie et de nourriture), la
technique (canvas ou WebGL, toujours en F# avec Fable) et le lien avec les textes, les sources et
les statuts, qui doivent rester visibles.

## Des penseurs pour choisir des directions (noté le 21 novembre 2026)

Inclure **un maximum de penseurs qui ont écrit sur les modèles de société**, pour que le joueur
puisse choisir une direction et voir ce qu'elle implique, au lieu d'un seul modèle imposé. Chaque
penseur ou courant apporte des options, des liens, des critiques des autres, et ses sources.
Liste de départ, à compléter et à équilibrer (y compris des auteurs hostiles au projet, sans
quoi la critique n'en est pas une) : Marx et Engels, Proudhon, Bakounine, Kropotkine, Owen,
Fourier, Gesell, Hayek, Polanyi, Ostrom, Bookchin, Graeber et Wengrow, Federici, Davis, Gilmore,
Fanon, Lordon, Friot, Neurath, Cockshott et Cottrell, Albert et Hahnel, Wright (utopies réelles),
Patrick Geddes et le mouvement des cités-jardins (Ebenezer Howard ; Letchworth, Welwyn) : la
petite échelle, l'union de la ville et de la campagne, le sol détenu en commun, l'enquête sur un
lieu avant de le planifier, et la région comme échelle (déjà une lentille dans
`data/lentilles.csv`).

**Le matérialisme historique, lentille de départ assumée.** L'auteur y est attaché, et il
convient bien au genre : le moteur part déjà de la base matérielle (terres, énergie, engrais,
échanges) avant les institutions. Proposition à valider : le déclarer comme **lentille par
défaut**, visible, et non comme la vérité du modèle. Chaque lien et chaque hypothèse dit de quelle
lentille il relève, et le joueur peut en changer ou en superposer d'autres. Les critiques qui lui
sont adressées entrent dans le jeu comme les autres : déterminisme et téléologie des stades,
réfutabilité (Popper), poids des idées et des institutions (Weber, Ostrom), travail domestique et
reproduction sociale (Federici), lecture eurocentrée de l'histoire (Fanon, Graeber et Wengrow),
critique libertaire de l'État de transition (Bakounine, Proudhon).

## Plus de théories, moins d'enfermement ? (noté le 21 novembre 2026)

Vrai à quatre conditions, sinon c'est l'inverse (un modèle plus gros, plus difficile à
contester, qui mélange tout dans les mêmes jauges) :

1. **Les théories doivent pouvoir contester les mesures elles-mêmes**, pas seulement ajouter des
   effets : le choix des jauges, leurs poids, les seuils vitaux, la France comme point de
   comparaison sont déjà une théorie. Une lentille doit pouvoir dire « cette jauge mesure mal ».
2. **Les désaccords doivent rester visibles**, pas s'additionner : quand deux lentilles donnent
   des valeurs différentes au même paramètre, le jeu montre l'écart au lieu d'en faire la moyenne.
   Piste : un coefficient peut avoir une valeur par lentille (une ligne par couple coefficient,
   lentille), et le joueur voit ce qui change selon la lentille.
3. **Inclure des adversaires** du projet (Hayek y est), sinon on ajoute des voix qui pensent pareil.
4. **Confronter aux faits** : ce qui ouvre le plus un modèle, ce sont les précédents réels et les
   contre-preuves (Kerala, Mondragon, Letchworth, Wörgl), pas le nombre de théories. Chaque
   théorie ajoutée apporte surtout des jugements : la légitimité baisse si elle n'apporte pas
   aussi ses preuves.

## La coordination impersonnelle, et la critique de toutes les méthodes (noté le 21 novembre 2026)

À prendre en compte dans le jeu : un mécanisme de coordination **impersonnel**, qui fasse
coopérer des personnes qui ne se connaissent pas. Point de départ retenu : les prix chez Hayek.
Mais c'est trop léger seul : **chaque méthode doit être critiquée**, les prix comme la
planification, les assemblées comme les plateformes, et le modèle doit porter des **mécanismes de
critique pour ne pas s'enfermer** dans ses propres hypothèses. Pistes nommées :

- **Proudhon** : le mutuellisme et le fédéralisme par contrat, une critique à la fois de l'État
  et de la propriété ;
- **Ostrom** : pas de panacée, ni le marché ni l'État ; des communs gouvernés par leurs usagers,
  une gouvernance polycentrique, des principes tirés de cas réels (la source `ostrom` est déjà
  dans le jeu).

Forme envisagée, à concevoir : une table de critiques (qui critique quoi, l'argument, la source,
ce que la critique changerait dans le modèle), qui vise aussi bien une option qu'un lien ou une
hypothèse du modèle, sur le modèle des contre-preuves déjà attachées aux sources.

Le jeu touche déjà la question par deux bouts sans la poser en ces termes :

- le levier « Comment savoir qui a besoin de quoi ? » (`COORD` : à la main, plateforme ouverte,
  Cybersyn, IA), qui traite de l'**information** sur les besoins ;
- la faiblesse « L'anonymat » du passage à l'échelle (`SCALE`, hors jeu depuis le 31 octobre) : à
  69 millions, la moquerie et la mise à l'écart, qui règlent les conflits dans les petits groupes,
  ne marchent plus.

À préciser avant de concevoir : le sens retenu, les options, les jauges touchées et les sources.
Mécanisme nouveau, donc après la refonte (étape 6 de `docs/refonte.md`).

## Passer des choix tout ou rien aux répartitions

Aujourd'hui, chaque levier du jeu impose une seule option : toutes les personnes âgées en établissement, ou toutes à domicile, ou toutes dans la cellule. La réalité est un mélange, et choisir une seule option est arbitraire. Le problème vaut pour presque tous les domaines : énergie, engrais, monnaie, accueil, soin, logement…

Piste : chaque levier devient une répartition (par exemple 20 % / 30 % / 50 %), et chaque lien du réseau s'applique à la part concernée.

Questions à trancher :

1. **L'offre et l'usage.** La société décide de ce qu'elle offre ; les pourcentages décrivent ce que les gens en feraient, ce sont des hypothèses de comportement. Faut-il deux réglages ?
2. **Les effets non proportionnels.** Certains effets ont un seuil (le socle à 100 %) ou s'installent dès qu'on commence (un outil de surveillance).
3. **Les combinaisons.** Les synergies entre leviers deviennent graduelles.
4. **L'affichage.** Des répartitions simples à jouer, y compris sur téléphone.
5. **La matrice et les vérifications**, à refaire pour des répartitions.

En attente de ce chantier : le levier « Vieillir : où et comment ? » (établissements ouverts, domicile avec aide, vieillir dans la cellule), dont le constat et les valeurs sont prêts.

## Prévention avant les scénarios (pilote : le gang, 31 octobre 2026)

Chaque scénario a une probabilité, réduite par des moyens de prévention ; les effets de la réponse comptent au prorata, et le risque évité rapporte en Sécurité (+5 × risque évité, validé). Pilote sur le gang : travail d'été (× 0,85, Heller 2014 ; contre-preuve : les atteintes aux biens augmentent ensuite), remise en état des lieux (× 0,92, Moyer 2019 ; le −29 % de Branas 2018 ne vaut que pour les quartiers pauvres), légaliser (× 0,70, Uruguay 2026 et Gavrilova 2019 ; un marché gris proche de 30 % demeure). Le contrôle des armes n'est pas un levier : la France le pratique déjà. Les verdicts des tests de résistance restent tout ou rien.

À étendre : meurtre de sang-froid, violences dans la famille, attentat, invasion, sécession.

## L'ancienne page est retirée (31 octobre 2026)

Le jeu est la seule vue. Le moteur ne lit plus la page : ses calculs sont identiques au centième sur 300 états tirés au hasard (`golden.py`, hors dépôt). Rapatriés dans le jeu : les sources (par sujet, avec les contre-preuves et le compte des faits à deux origines), le quiz, les tests de résistance, les paris (avec l'horizon) et le partage par lien (`#m=`, les anciens liens marchent).

Pas rapatriés, récupérables dans l'historique git (commit 5d32675 et avant) :

- les cinq faiblesses du passage à l'échelle et leurs parades (`SCALE`, `choix.par`) ;
- les réglages fins de la monnaie (écart de revenus, fonte, plafond, socle) ;
- les curseurs de calcul (perte de terres, besoin par personne, énergie disponible…) ;
- le comparatif, la pyramide des cellules, le niveau de vie détaillé, le bilan écrit, le périmètre France entière / métropole.

## Ce que la loi interdit : la liste

Le scénario du gang armé parle de « produits que la loi interdit » sans dire lesquels, et renvoie ici. Il faut dresser la liste de ce que la législation française interdit de produire, de vendre ou de détenir : stupéfiants, armes, espèces protégées, contrefaçons, médicaments sans ordonnance, etc. Chaque entrée doit citer le texte de loi lui-même (code de la santé publique, code de la sécurité intérieure, code de l'environnement…), pas un résumé de presse.

Questions à trancher :

1. **Le périmètre.** Tout ce qui est interdit, ou seulement ce qui alimente des trafics ?
2. **Ce que la société du jeu en fait.** Pour chaque interdit, la garder, l'assouplir ou la lever devient peut-être un levier ; le choix pèse sur les trafics, et donc sur les bandes.
3. **L'affichage.** Une page ou une section de la page principale, vers laquelle pointent les scénarios.

## Les biais des sources : des garde-fous

Une source peut être biaisée : par qui la produit (le ministère de l'Intérieur pour la police, des associations engagées pour la rétention), par sa méthode, par la petite taille de l'échantillon (beaucoup de leviers s'appuient sur des sociétés de quelques centaines de personnes), ou par notre propre envie de confirmer le but.

Garde-fous déjà en place : les statuts (sourcé, hypothèse, jugement), la règle de remonter à la source d'origine, les dates, le lien « signaler », la règle « les liens mesurent ce que les règles font aux gens, jamais ce que des groupes feraient ».

### A. Test de sensibilité (fait le 23 octobre 2026)

On fait varier chaque élément de ±20 % (et on supprime chaque lien) sur 1 002 états : le modèle de départ, le modèle ouvert et 1 000 états tirés au hasard. On compte les verdicts qui basculent (viable ? mieux que la France ?).

- **Le réseau :** seuls 9 liens sur 180 sont sourcés ; 49 sont des hypothèses, 122 des jugements. Mais à ±20 %, seuls 12 liens font basculer un verdict, et rarement (3 états sur 1 002 au plus). Supprimer un lien ne change aucun verdict pour 156 liens sur 180.
- **Les éléments fragiles sont tous des jugements sur les Pouvoirs :** la force fédérale armée, l'IA qu'on ne comprend pas, la concentration des fichiers. Ce sont eux qu'il faut appuyer sur des sources en premier.
- **Les seuils vitaux pèsent plus que le réseau :** bouger le seuil des Pouvoirs de 20 % fait basculer 36 à 44 états. Ce sont des jugements.
- **Les faits qui pèsent le plus :** la surface agricole par personne (+20 % : 55 états basculent), l'énergie disponible (−20 % : 22 états), les terres perdues (+20 % : 24 états).
- **Les deux modèles de référence tiennent :** aucun verdict ne bascule pour le modèle de départ (non viable : Ressources à 30 % pour un seuil de 50 %) ni pour le modèle ouvert (viable ; son maillon le plus faible est la Nourriture, à 98,5 % pour un seuil de 85 %). Mais dans le modèle de départ, la Nourriture est sur le fil : 86,8 % pour un seuil de 85 %.
- **Les valeurs France :** à ±20 %, chacune fait basculer « mieux que la France » pour 9 états au plus.
- **Le curseur « consommation par personne » n'a aucun effet** sur les jauges : à vérifier.

Limites du test : il ne touche pas les constantes écrites dans les formules (1 496 TWh, 15,4 % de pauvreté, les verdicts du stress test, eux-mêmes des jugements) ; et il ne peut pas voir un lien qui manque. Un biais d'omission, le plus probable quand on cherche à confirmer un but, échappe à tout calcul.

### À faire

- **B. Une fiche par source :** qui la produit et son intérêt, primaire ou secondaire, méthode et taille de l'échantillon, limites connues. Règle : un lien « sourcé » avec une seule source secondaire redescend en hypothèse.
  - **Fait le 29 octobre 2026 :** les 21 sources du réseau ont leur fiche, affichée sous chaque source du Labo et au survol. Quatre jugements fragiles sur les Pouvoirs passent en hypothèses appuyées sur des cas, chacun avec une nuance (« à nuancer ») : la force armée (base mondiale des coups d'État, putsch d'Alger), l'IA qu'on ne comprend pas (allocations aux Pays-Bas), les fichiers (registres sous l'Occupation), le code (Robodebt en Australie). Le mécanisme est documenté ; l'ampleur (−3 à −10) reste supposée. Cybersyn s'appuie sur l'historienne Eden Medina, plus sur Wikipédia.
  - **Fait le 29 octobre 2026 :** les sources fragiles du réseau remplacées par leur source d'origine (Dyble 2019, Hewlett 1991, IAB 2025, étude du ministère de l'Intérieur 2024, rapport du comité d'évaluation de la vidéosurveillance, Whitehead 1977 ; le kibboutz par une revue universitaire, encore secondaire). **Trois fois sur huit, la presse disait plus que la source :**
    - les féminicides : la presse écrivait « 47 % des femmes tuées avaient alerté la police » ; l'étude dit que 47 % avaient déjà subi des violences, et que 74 % de celles-ci les avaient signalées, soit environ 35 % des femmes tuées ;
    - la vidéosurveillance : pas de sans-abri signalés ; le logiciel prenait des personnes assises près de leurs bagages pour des objets, « ce qui pourrait conduire à cibler davantage les sans domicile fixe » (le lien passe de sourcé à hypothèse) ;
    - les pères aka : ils ne « s'occupent » pas des bébés la moitié du temps, ils les tiennent ou restent à portée de bras 47 % du temps.
  - **Leçon :** une source de presse ne suffit jamais pour un chiffre ; remonter au document d'origine avant de l'écrire.
  - **Fait le 29 octobre 2026 : les valeurs France vérifiées dans leur document d'origine**, avec leur fiche (prisons, rétention, homicides, budgets, pauvreté, faim, commerce, Mayotte, IPP). Confirmés : 90 020 détenus, 15,4 % de pauvreté et un rapport de 3,49, 12 % qui n'ont pas assez à manger, 5,24 + 24,3 Md€, 81 Md€ de déficit, 77 % de pauvreté à Mayotte. Corrigés :
    - **la rétention :** « environ 3 700 personnes à un instant donné » venait d'un calcul faux (33 jours de durée moyenne appliqués aux 40 592 personnes, dont 24 364 en outre-mer, surtout à Mayotte, où la rétention est bien plus courte) ; il y a 1 959 places. La Liberté de la France passe de 135,4 à 132,9 personnes enfermées pour 100 000 habitants ;
    - **l'IPP :** la note a été corrigée par l'institut en mai 2025 : 83 % des affaires de violences sexuelles ne sont pas poursuivies (et non 86 %), autant que pour les autres atteintes à la personne (84 %) ; pour les viols, 92 % en 2019 (le chiffre de 94 % n'y figure pas) ;
    - **les homicides :** mis à jour avec le bilan 2025 (975 victimes, publié en juillet 2026). Un député cite 1 186 homicides pour 2024, sans source : écarté.
  - **À noter :** la pauvreté de l'Insee (15,4 %) ne couvre que la métropole ; l'outre-mer, plus pauvre, n'y est pas.
  - **Reste :** 80 sources sur 110 sans fiche, dont les sources de presse des valeurs France et des constats (par exemple Euronews pour les féminicides de 2025) ; « la salle de pilotage concentre tout » et l'alliance européenne pour l'arme nucléaire restent des jugements.
- **C. Une contre-preuve par levier :** la source la plus solide qui le contredit, affichée à côté. Sources discutées déjà repérées : la règle des 3,5 % (Chenoweth, descriptive, nuancée par l'autrice), les Mosuo (souvent idéalisés), Dyble 2015 (deux groupes).
  - **Fait le 29 octobre 2026 :** 19 leviers ont une contre-preuve, dirigée contre l'option que le modèle favorise, affichée en rouge sous le titre du levier, dans l'atelier et dans les tranches, avec sa source et sa fiche. Les plus gênantes pour le modèle :
    - **sans monnaie / droit de partir :** au kibboutz, le partage égal a fait partir les plus qualifiés, et beaucoup de kibboutz l'ont abandonné (Abramitzky, 2018) ;
    - **pas de surveillance :** les caméras font baisser la délinquance de 16 % en moyenne, de 51 % dans les parkings (Welsh et Farrington, 2009) ;
    - **engrais organiques :** au Sri Lanka, l'interdiction des engrais chimiques en 2021 a fait chuter les rendements de riz d'environ 25 à 30 % ;
    - **démanteler l'arme nucléaire :** l'Ukraine a rendu la sienne en 1994 et a été envahie en 2014 et 2022 ;
    - **soin partagé :** au kibboutz, les bébés en dortoir collectif la nuit avaient moins souvent un attachement sûr (48 % contre 80 %) ;
    - **tirage au sort :** en Colombie-Britannique, les autres citoyens n'ont pas suivi la réforme de l'assemblée tirée au sort (57,7 %, sous le seuil de 60 %) ;
    - **mandats courts et révocation facile :** l'administration gagne du pouvoir (États-Unis) ; la révocation sert à l'opposition et rend les communes instables (Pérou).
  - **Intégrées au réseau le 30 octobre 2026 (validé avec l'utilisateur) :** habitat partagé → Égalité +3 → +1 ; laïcité de 1905 → Égalité +3 → +1 ; SASA! → Sécurité +3 → +2 ; exil → Sécurité +2 → +1 ; nouveaux liens : mandats courts → Pouvoirs −3 (à 3 mois, nul à 6 mois), révocation facile → Pouvoirs −2 à 10 %, −4 à 5 %, paliers → Échelle −3, plateforme décentralisée → Pouvoirs −2, sans monnaie et pays ouvert → Ressources −3. Déjà comptés, sources ajoutées : engrais (25 % de perte), arme nucléaire (−5), départ des plus qualifiés (−3) ; nuances ajoutées sur la médiation, la défense civile et la prévention. Non intégrées : les caméras (elles agissent sur les vols, que le modèle ne mesure pas), l'algorithme (la simulation porte sur la détention), le dortoir du kibboutz (autre chose que le soin partagé).
  - **Effet mesuré :** 185 liens, tous actifs. Le modèle ouvert passe de 87,62 à 86,92 points et reste viable ; le modèle de départ ne change pas. Sur 300 états tirés au hasard, 34 viables au lieu de 36. Les mandats de 3 mois restent favorables aux Pouvoirs (68 contre 57,5) : la contre-preuve tempère sans renverser.
  - **À refaire :** la matrice (`matrice/`) ne montre pas encore ces cinq nouveaux liens ; son générateur a été perdu avec le dossier temporaire, il faut le réécrire.
  - **Ajoutées le 30 octobre 2026 :** laïcité (à CV égal, un candidat au prénom musulman a 2,5 fois moins de chances d’être rappelé, malgré un État neutre), sécession (le Soudan du Sud, indépendant par référendum négocié en 2011, en guerre civile dès 2013), exil (à Seattle, l’interdiction de territoire déplaçait les problèmes, et la plupart revenaient), coordination décentralisée (Mastodon se recentralise), relais (les conseillers numériques financés par l’État disparaissent avec le budget), médicaments (l’État ne visait qu’une cinquantaine des 450 médicaments essentiels).
  - **Reste sans contre-preuve :** l’accueil (aucune source solide trouvée contre l’appartenance immédiate) ; les curseurs du socle et des domaines nationaux.
  - **Oubli révélé :** le modèle ne mesure pas les vols et les atteintes aux biens. Les caméras agissent surtout là-dessus, pas sur les violences.
- **D. Une relecture extérieure** par des spécialistes, une tranche chacun.
- Étendre le test de sensibilité aux constantes des formules et au stress test.

## Le score mélange trois questions (fait le 23 octobre 2026)

Fait : les trois mesures sont séparées dans le jeu (viable, fidèle au but, mieux que la France). Reste ouvert : les seuils vitaux, qui sont des jugements, et la quatrième question, le chemin.

Question de l'utilisateur : le score dit-il si le modèle est viable, ou s'il se rapproche du but ? Ni l'un ni l'autre exactement. Le score est la moyenne de dix jauges de résultat (Sécurité, Criminalité, Niveau de vie, Nourriture, Ressources, Échanges, Pouvoirs, Échelle, Paris, Égalité), comparée à la France calculée de la même façon. Le but (sans prison, sans police, sans patriarcat, sans racisme) n'est mesuré que par la jauge Principes, qui ne compte pas dans le score : elle sert seulement de seuil (75 %) pour être classé.

Le biais : une moyenne laisse un excédent compenser un effondrement. Exemple vérifié dans le moteur (23 octobre 2026) : avec une Nourriture à 65 % (un tiers des besoins non couverts, une famine), un modèle obtient 69,90 points, plus que le modèle de départ (61,69), grâce à l'Égalité, aux Pouvoirs et aux Ressources.

Piste : séparer trois mesures.

1. **Viable ?** Le maillon le plus faible des jauges vitales (Nourriture, Niveau de vie, Sécurité, Ressources, Pouvoirs) : une famine ne se compense pas. Un seuil plutôt qu'une moyenne.
2. **Fidèle au but ?** Sans prison, sans police, sans patriarcat (le patriarcat restant), sans racisme (l'Égalité), sans enfermement.
3. **Mieux que la France ?** La comparaison, sur les modèles viables et fidèles seulement.

Et une quatrième question, celle du départ : **comment y arriver ?** (la transition, les paliers, le temps), aujourd'hui seulement esquissée.

## À préparer : les violences et les conflits dans la famille

Le modèle couvre les violences dans le couple, mais presque rien d'autre dans la famille. À préparer (constat sourcé, anthropologie, puis leviers) :

- les violences faites aux enfants, dont l'inceste : environ 160 000 enfants victimes de violences sexuelles chaque année, 81 % des violences racontées à la Ciivise ont lieu dans la famille, 1 % des agresseurs condamnés en cas d'inceste (Ciivise, 2023 et bilan 2026) ; 57 311 mineurs victimes de violences physiques intrafamiliales enregistrées en 2024, contre 22 556 en 2017 (Inspection générale de la justice, 2026) ; les mineurs sont 75 % des victimes de violences physiques et 96 % des victimes de violences sexuelles intrafamiliales hors couple (SSMSI, via la presse) ;
- la protection de l'enfance sans police ni juge des enfants : 384 900 jeunes suivis par l'aide sociale à l'enfance fin 2023, dont 221 000 accueillis hors de chez eux (DREES) ;
- la maltraitance des personnes âgées : environ 7 000 dossiers ouverts par le 3977 en 2021, 75 % des cas à domicile, la famille souvent en cause (fédération 3977, via la presse) ; une personne âgée sur dix maltraitée chaque mois selon l'OMS, à vérifier ;
- les conflits sans violence : garde des enfants après une séparation, disputes d'héritage (la justice civile, sans levier pour l'instant).

L'épreuve « violences cachées dans le cercle proche » du stress test couvre toute la famille en principe, mais ses chiffres ne parlent que du couple.

Anthropologie : selon Jill Korbin (Child Abuse and Neglect: Cross-Cultural Perspectives, 1981), le facteur le plus protecteur est que l'éducation des enfants soit partagée par un réseau de proches et de voisins au-delà des parents : plus de regards, plus d'aide, plus de contrôle. C'est le soin partagé de la cellule.

## Prêts, en attente des répartitions

### Vieillir : où et comment ?

Constat : 22 % de la population a 65 ans ou plus (Insee, bilan démographique 2025) ; 750 000 personnes de 60 ans ou plus en « mort sociale », sans aucun contact (Petits Frères des Pauvres, 2025) ; 590 800 résidents en Ehpad fin 2023 (DREES). Anthropologie : l'hypothèse de la grand-mère (Hawkes, chez les Hadza : les anciennes nourrissent et élèvent les petits ; hypothèse débattue). Sans monnaie, pas de question des retraites : le socle couvre tout le monde.

| Jauge | Établissements spécialisés, ouverts | Chez soi, avec une aide à domicile | Vieillir dans la cellule (logement adapté, soin partagé, rôle reconnu) |
|---|---|---|---|
| Égalité | −2 j | −1 j | +3 j |
| Liberté | −2 j | +2 j | +2 j |
| Niveau de vie | 0 | 0 | +1 h |
| Moyens libérés | −3 h | −3 h | −1 h |

Aucune option n'enferme (pas d'unité fermée).

### Apprendre : quelle école ?

Constat :
- Le lien entre milieu social et résultats reste fort : en mathématiques, une unité de l'indice socio-économique vaut 50 points de score PISA en 2022 (57 en 2006) (DEPP).
- La ségrégation scolaire : parmi les 10 % de collèges les plus défavorisés, 96,7 % sont publics ; parmi les 5 % les plus favorisés, 71 % sont privés (analyse SNES-FSU des indices de position sociale publiés par le ministère).
- 7,9 % des 18-24 ans ont quitté l'école sans diplôme du second cycle en 2024, contre 10,8 % en 2012 (Insee).
- À Mayotte, de 3 000 à plus de 15 000 enfants ne sont pas scolarisés selon les sources (Défenseur des droits, université Paris-Nanterre, chambre régionale des comptes) ; en Guyane, environ 6 200 (Insee, 2020).

Anthropologie et expériences :
- Chez les chasseurs-cueilleurs, les enfants apprennent surtout en jouant librement, en groupes d'âges mélangés, et en observant les plus grands ; il y a très peu d'enseignement formel (Peter Gray, American Journal of Play, 2009, d'après les ethnographies).
- La pédagogie Freinet (française : coopération, conseil de classe, travail par projets) : à milieu social égal, des résultats semblables à ceux des écoles traditionnelles et un climat jugé meilleur par les élèves (étude de Liège, 2018, plus de 1 500 élèves).

| Jauge | L'école d'aujourd'hui (classes par âge, notes, tri précoce, public et privé séparés) | Une école commune dans chaque cellule (une seule école pour tous, âges mélangés, coopération type Freinet, pas de tri précoce) | L'apprentissage libre (jeu, observation, âges mélangés, comme chez les chasseurs-cueilleurs) |
|---|---|---|---|
| Égalité | −2 s : la ségrégation scolaire | +3 j | −1 h : les enfants moins soutenus chez eux risquent d'apprendre moins |
| Liberté | 0 | +1 j | +3 j |
| Pouvoirs | 0 | +1 j : apprendre à décider en conseil | +1 j |
| Ressources | +1 h : former aux métiers rares | 0 | −1 h : former aux métiers rares prend plus de temps |
| Moyens libérés | 0 | −1 h | +1 h |

Liens à prévoir : avec la formation de masse aux métiers rares (droit de partir), avec les relais humains et le numérique, avec le soin partagé (garde des enfants).

Sources : [DEPP, PISA 2022](https://www.education.gouv.fr/depp/pisa-2022-la-france-ne-fait-pas-exception-la-baisse-generalisee-des-performances-en-culture-380205) · [SNES-FSU, mixité sociale au collège](https://www.rennes.snes.edu/IMG/pdf/mixitecollege.pdf) · [Insee, sorties précoces](https://www.insee.fr/fr/statistiques/2658583) · [Banque des Territoires, la non-scolarisation](https://www.banquedesterritoires.fr/non-scolarisation-des-enfants-un-phenomene-bien-reel-toujours-aussi-meconnu) · [Human Rights Watch, Mayotte](https://www.hrw.org/fr/news/2025/06/19/france-de-nombreux-enfants-a-mayotte-prives-deducation) · [Peter Gray, résumé](https://naturalstart.org/sites/default/files/staff/peter_gray_handout.pdf) · [Revue des sciences de l'éducation, Freinet (Liège)](https://releve.erudit.org/en/journals/rse/2013-v39-n2-rse01422/1025245ar.pdf)

## Des données qui vivent : mises à jour, propositions, contributions (noté le 21 novembre 2026)

Le modèle n'est jamais fini, sur trois plans qui avancent au même rythme :

1. **Les données réelles changent chaque année** (homicides, détenus, pauvreté, énergie,
   échanges…). Chaque valeur d'un pays doit porter une **série datée** plutôt qu'un seul chiffre :
   une ligne par année, avec sa source, et le moteur prend la plus récente. `labo doctor` signale
   une valeur dont la prochaine publication est passée (le calendrier de chaque source : Insee à
   l'automne, SSMSI en début d'année, douanes en février…). `MISES-A-JOUR.md` devient un rapport
   généré par `labo doctor`.
2. **Les propositions s'enrichissent** : chaque étude ou modèle découvert (un penseur, un cas réel,
   une contre-preuve) entre comme une ligne : une source, une lentille, et ce qu'elle change dans
   le modèle (un lien, un coefficient, une option, une critique). L'historique git garde chaque
   état du modèle, et le jeu peut dire « ce chiffre a changé le … parce que … ».
3. **Les joueurs apportent des sources.** Le jeu le fait déjà : chaque « réf. nécessaire » ouvre un
   formulaire GitHub (`.github/ISSUE_TEMPLATE/source.yml`), et il existe un formulaire pour
   signaler une erreur. À étendre : un formulaire par type de contribution (mettre à jour un
   chiffre, proposer une étude ou un modèle, contester un lien ou un coefficient), dont les champs
   suivent les colonnes des CSV. Chaîne visée : formulaire → une action GitHub prépare une
   proposition de modification des CSV → `labo doctor` la vérifie → une personne relit et décide
   → fusion, et le site se reconstruit. Rien n'entre dans le modèle sans relecture humaine.

   **Un joueur apporte aussi sa pensée, pas seulement des sources.** Il peut proposer une idée de
   solution (une option, un levier), un raisonnement (un lien : « tel choix fait telle chose à telles
   personnes, parce que… »), une critique, et jusqu'à sa propre lentille. Sa contribution entre avec
   un statut honnête (un jugement, comme beaucoup de liens du modèle), signée de son avatar, qui
   peut devenir un conseiller parmi les penseurs. Une idée sans preuve est un « monopole » : elle
   tient en théorie et attend son précédent ; d'autres joueurs peuvent la discuter, l'étayer ou la
   contredire. Le robot peut aider à mettre une idée en forme (condition, effet, raison), en
   signant son aide comme la sienne.

## Une base robuste pour un système qui va grossir (noté le 21 novembre 2026)

Le système va grossir beaucoup (des dizaines de tables, des pays, des contributions). Ce qui le
tient déjà : CSV relus dans git, validation avec fichier et ligne, F# généré et typé, oracle et
tests. Ce qu'il faudra avant de grossir :

- **Un schéma déclaré une seule fois** (types de colonnes, références, valeurs permises) d'où la
  CLI tire la validation et la génération, au lieu d'un code écrit à la main par table dans
  `Schema.fs` et `Validate.fs`.
- **Des identifiants stables et jamais réutilisés**, et des renommages tracés.
- **Des invariants testés** en plus de l'oracle, qui disparaîtra avec l'ancien moteur : jauges
  entre 0 et 1, aucun lien jamais actif, sensibilité (les anciens `verify_net.py` et `sens.py`).
- **Des séries datées** pour les données réelles (ci-dessus).
- Garder les CSV comme source tant que git et la relecture humaine suffisent ; une base de données
  ne s'impose que si les contributions deviennent trop nombreuses pour être relues ainsi.

## La thermodynamique et la physique (noté le 21 novembre 2026)

Cohérent, et déjà en partie dans le modèle (énergie disponible, rendements, besoin en terres).
Deux niveaux à distinguer :

- **Les lois physiques sont des contraintes, pas une lentille** : conservation de l'énergie,
  entropie, rendement énergétique (énergie obtenue pour l'énergie investie, l'EROI de Charles
  Hall). Aucune lentille ne doit pouvoir les éteindre ; elles bornent ce que chaque modèle de
  société peut produire.
- **L'économie écologique est une lentille** (Georgescu-Roegen, les Odum, Hall) : elle lit
  l'économie comme une transformation d'énergie et de matière. Elle rejoint le matérialisme
  historique (Engels, Podolinsky), avec un garde-fou connu depuis le débat entre Weber et Ostwald
  (1909) : la physique borne ce qui est possible, elle ne décide pas de l'organisation sociale.

## Les idées des joueurs : le ticket n° 1 (21 novembre 2026)

- **Registre des manques** (ticket GitHub n° 1, de l'auteur) : chaque cellule ne signale que ce
  qui lui manque, celles qui peuvent répondre le font, sans centre. Entré dans le jeu comme
  option `registre` du levier `coord`, avec une réplique du joueur et une scène de réaction. Ses
  cinq liens sont des **propositions de l'assistant** (hypothèses et jugements, lentilles
  communs, ordre spontané, intersectionnalité), à contester.
- Les précédents cités dans le ticket ont été vérifiés (21 novembre 2026) et entrés dans
  `sources.csv` : `neurath1919`, `technocracy1934`, `fresco2002`, `cockshott1993`, `ohno1988`,
  `valueflows`, `sardex2017`, `stodder2009`, `willer2012`, `sel2015` (Cybersyn : `medina`). Ils
  prouvent que ces propositions et ces dispositifs existent, **pas qu'un registre des manques
  marche à l'échelle d'un pays** : les liens restent des hypothèses. Le kanban (`ohno1988`)
  appuie le mécanisme, mais dans une entreprise entourée de prix. Non retenus : les « boîtes à
  besoins » des tiers-lieux et les listes des Fab Labs (pratiques réelles, sans source générale).
- La boucle des contributions : formulaire `idee.yml` (avec « d'où vient ce texte ? » et un
  pseudonyme pour la citation), étiquette `idée`, réponse dans le ticket qui dit ce que l'idée
  devient. Le formulaire doit être sur la branche par défaut (`main`) pour que GitHub le propose.

## Le jeu de stratégie annexe : la défense civile (21 novembre 2026)

- **Décision de l'auteur** : la partie annexe sur la sécurité sera un jeu de **défense civile non
  violente** (rendre une occupation impossible plutôt que la combattre), parce que c'est le plus
  cohérent avec le modèle. Écartés pour l'instant : la défense mixte (milice) et la sécurité au
  quotidien comme jeu séparé.
- La conception : [docs/defense-civile.md](docs/defense-civile.md) (décisions de l'auteur et
  propositions de l'assistant, marquées). Rien n'est codé.
- **Décision de l'auteur : un jeu par sujet, plus tard.** Chaque grand sujet pourrait avoir son
  jeu, qui fait partie des choix : on *joue* le choix au lieu de le cocher. On commence par la
  sécurité ; dans la tranche Sécurité, choisir la défense civile face à l'invasion lance la partie
  de défense civile, dont l'issue revient dans le modèle.
- **Ajout de l'auteur : la coercition par la menace de mort.** L'occupant ne se contente pas de
  demander la collaboration : il l'impose (travail forcé sous la menace, otages, représailles
  collectives, déportation). Exemple donné par l'auteur, qui a travaillé à Lorient : la base de sous-marins de
  Keroman, construite sous l'occupation allemande par des habitants contraints sous la menace
  d'être fusillés. Connaissance de l'auteur (statut « de mémoire ») ; une source écrite sera
  jointe pour que les joueurs puissent la vérifier eux aussi. C'est le
  dilemme central de la non-coopération : son coût retombe sur des personnes.
- **En maturation (22 novembre 2026)** : la boucle de tour minimale de la défense civile est
  écrite comme proposition dans [docs/defense-civile.md](docs/defense-civile.md) ; l'auteur la
  laisse mûrir avant de décider (durée, règle de victoire, force du retournement). Rien n'est codé,
  aucun coefficient n'est créé.

## Les joueurs créent des scénarios (22 novembre 2026)

- **Idée de l'auteur** : répertorier toutes sortes de conflits et en faire des jeux est un travail
  monumental ; les joueurs doivent pouvoir **ajouter des scénarios**, comme ils proposent des
  idées pour les choix. Comme Minecraft ou Roblox, donner les outils pour créer des jeux, à la
  différence qu'on s'efforce de **conserver une légitimité**, et **sans sortir du contexte de
  base** (une société sans prison, sans police, autant que possible sans monnaie).
- **Proposition de l'assistant** : un scénario est déjà fait de données (`data/defense/` :
  convoitises, actions, mesures). Un modèle de scénario documenté, un formulaire de ticket
  « Proposer un scénario », puis plus tard un éditeur dans le jeu. La légitimité tient aux mêmes
  règles que le reste : chaque action et chaque mesure a un précédent réel et une source (ou un
  témoignage), la provenance est signée, une personne valide (jamais un assistant), et
  `labo doctor` refuse ce qui sort du cadre (une action sans précédent, une mesure qui suppose
  une police ou une prison dans la société du joueur).

## L'évolution des habitants : les sources de psychologie (22 novembre 2026)

Les quatre règles d'évolution (ce qu'on vit pèse plus, la confiance colore le regard, la vie
avance, le joueur convainc) prendront leur direction dans des sources vérifiées sur Crossref, leur
ampleur restant un réglage de jeu (« jugement ») :

- ce qu'on vit pèse plus : `kahneman1979`, discuté par `gal2018`, à fixer d'après la méta-analyse
  `brown2024` ; l'apaisement et ses limites : `lucas2007` ; ce qui marque durablement :
  `malmendier2011`, `bietenbeck2023` ;
- la confiance colore le regard : `tyler2006` ;
- la vie avance : `krosnick1989`, comme tendance et jamais comme stéréotype ;
- le joueur convainc : `luskin2002`, dans des conditions de délibération ; `nyhan2010` n'est pas
  répliqué (`wood2019`).

**Le garde-fou a servi** : `giuliano2014` (« Growing up in a Recession »), cité de mémoire par
l'assistant, est **rétracté** depuis janvier 2023 (résultats non reproductibles, probablement une
erreur de code). `labo sources verifier` le signale tout seul, et `labo doctor` refuse désormais
qu'une source rétractée serve d'appui. Une réplication partielle (`bietenbeck2023`) prend le
relais.

## La dette (22 novembre 2026)

Sujet proposé par l'auteur, à partir de Graeber : conception dans [docs/dette.md](docs/dette.md)
(faits de départ sourcés, position de l'auteur et position opposée, une question « Que faire des
dettes ? » à valider). C'est un nouveau levier, donc un changement du modèle pendant la refonte,
comme le registre des manques : décision assumée.
- **Défaut hérité de l'ancien moteur, à corriger après la bascule** : le ralentissement d'une
  jauge près de 0 ou de 100 (« bornes souples ») ne s'applique qu'aux jauges qu'au moins un lien
  peut faire monter (ou baisser). Ajouter le premier lien qui fait monter une jauge change donc
  tous les scores, même quand ce lien est inactif. Constaté le 22 novembre 2026 avec la dette
  (« honorer → échanges », « auditer → principes ») : liens réexprimés pour garder l'oracle.
  Après la bascule, les bornes souples devraient valoir pour toutes les jauges.

## Types de partie, et des personnages qui changent de rôle (22 novembre 2026)

- **Idée de l'auteur** : on commence une partie avec des réglages prédéterminés, et ce sont eux qui
  définissent la partie ; d'autres réglages feraient d'autres types de partie. Il aime que des
  personnages porteurs d'un contexte interviennent dans un autre : la garde dans la conversation
  sur la monnaie, comme dès la première version des scènes (ce n'est pas une idée nouvelle).
- **Ajouts de l'assistant, le 22 novembre 2026** (à garder ou retirer selon l'auteur) : un rôle qui
  change en cours de partie (approuvé par l'auteur) ; le passé des personnes
  (`antecedents.csv`) et les répliques « souvenir », nés d'un malentendu sur cette idée.
- **Proposition de l'assistant** : séparer la **personne** (prénom tiré, âge, ce qui compte pour
  elle, son histoire, sa confiance) de son **rôle**, qui dépend du type de partie et du contexte.
  Aïcha est garde tirée au sort dans la société sans police ; dans la défense civile, elle
  organise la résistance ; dans une partie qui partirait de la France d'aujourd'hui, elle pourrait
  être gendarme. Bruno reste agriculteur, mais réquisitionné sous l'occupation. Les scènes
  désigneraient un rôle (`{garde}`), que chaque type de partie attribue à une personne.
- Un **type de partie** serait un jeu de réglages de départ en données : ce qui est déjà décidé
  (sans prison, gardes tirés au sort…), les rôles, la région, les tranches.

## Les paris et le budget de la cellule (22 novembre 2026)

- Plusieurs paris peuvent être lancés à la fois (trois au plus, `jeu-paris-par-occasion`,
  jugement) : le moteur les faisait déjà mûrir chacun.
- **Idée de l'auteur** : ce sur quoi on mise dépend du budget de la cellule. **Proposition de
  l'assistant** : dans une société sans monnaie, le « budget » est fait de moyens réels (travail,
  énergie, matériaux) ; la jauge « Moyens libérés » (ce que prisons et police ne consomment plus)
  pourrait être ce budget, et chaque pari coûterait des moyens, ce qui remplacerait la limite de
  trois. Il faut pour cela un coût sourcé par pari (`paris.csv` n'en a pas encore).

## La cellule ou le pays ? (22 novembre 2026, à reprendre en session fraîche)

- **Remarque de l'auteur** : pendant la session et en jouant, il a du mal à dissocier la cellule du
  pays : il prend des décisions à l'échelle du pays, mais il est dans une cellule ; quelque chose
  n'est pas clair.
- **Diagnostic de l'assistant** : le jeu mêle deux échelles sans dire qui est le joueur. Le modèle
  et les questions sont nationaux (la monnaie, la dette de l'État, l'énergie du pays), tandis que
  les scènes, les personnages et la région sont ceux d'une cellule d'une cinquantaine de personnes.
- **Pistes** (rien de décidé) :
  1. le joueur est le **délégué** de sa cellule aux conseils : les questions nationales se
     tranchent aux paliers, la cellule l'a mandaté, la confiance des habitants est son mandat (et
     ils peuvent le révoquer) ; cohérent avec le modèle (délégués, mandats, révocation) ;
  2. **deux niveaux de décision** : ce que la cellule décide elle-même, et ce que le pays décide,
     où le joueur ne pèse que pour sa part ;
  3. le joueur **incarne la société entière** (vue d'en haut, comme dans un jeu de gestion), et la
     cellule n'est qu'un groupe de témoins ; il faut alors le dire.

## Le temps du jeu, et la constitution comme point de départ (22 novembre 2026)

- **Remarque de l'auteur** : la révolution solaire devrait plutôt passer une fois qu'on a tout
  décidé, pour l'instant, quitte à mettre plus tard un décompte du temps pour les décisions
  suivantes. Le rythme doit pouvoir s'appliquer dans le réel : comment ça se passerait si cette
  société existait ? S'inspirer des sociétés actuelles et des jeux de gestion politique.
- **Décision de l'auteur** : on ne réécrit pas une constitution tous les ans. La **constitution**
  est le point de départ : elle présente la société dans laquelle le joueur arrive, ce sont les
  réglages de base. Elle rejoint les types de partie (ci-dessus) : un type de partie serait une
  constitution.
- **Exemples apportés par l'assistant** (à vérifier avant d'en faire un texte du jeu) : dans le
  réel, un moment fondateur puis un rythme ordinaire (France 1958, Islande 2011, Chili 2021-2022,
  contrat social du Rojava en 2014) ; à l'inverse, des décisions imposées une à une par l'urgence
  (Commune de Paris, Espagne 1936) ; et le délai des consultations à la base (zapatistes, paliers
  du confédéralisme). Dans les jeux : le délai entre deux lois de Frostpunk, le capital politique
  par trimestre de Democracy, les chapitres de Suzerain.
- **Conséquences proposées par l'assistant** (rien de décidé) :
  1. les décisions actuelles des tranches (monnaie, coordination, révocation, paliers…) ont une
     valeur de départ inscrite dans la constitution, présentée au début de la partie ; le joueur ne
     les choisit plus d'un bloc, il les **remet en question** quand la vie l'y amène ;
  2. ce qui fait venir une décision : un événement, un pari qui mûrit, une habitante qui conteste
     une règle, une décision qu'on rouvre ; ce qui la limite : un coût (le budget de la cellule,
     voir « Les paris et le budget ») ou un délai, plutôt qu'un plafond arbitraire de deux par
     révolution ;
  3. le décompte du temps peut être le **délai des paliers** : une question portée au conseil
     revient une ou deux révolutions plus tard ; cela répond aussi à « la cellule ou le pays ? »
     (piste du délégué) ;
  4. en attendant, `jeu-situations-par-revolution` assez haut pour que tout se décide dans la même
     révolution. **Fait le 22 novembre 2026** (100) : toutes les décisions d'une tranche se prennent
     avant que l'année passe, le bilan clôt la révolution, donc une révolution par tranche (cinq
     dans une partie). Le bilan vient désormais dès qu'aucune décision n'attend, et un événement
     lié à une tranche peut survenir à la fin de la révolution où elle s'est jouée. Les parties
     sauvegardées avant ce changement ne se rejouent plus. La bible (§ 3, « deux ou trois par
     révolution au plus ») n'est pas modifiée : c'est à l'auteur de dire ce qu'elle devient.
- **Idée de l'auteur** : la règle de révision de la constitution serait elle-même un **réglage**,
  un choix du joueur parmi plusieurs propositions qui ont toutes leur légitimité.
- **Propositions de l'assistant pour ce choix** (exemples de mémoire, à sourcer avant d'en faire un
  texte du jeu) :
  1. **la constitution stable** : on ne la révise que par une convention, rarement ; elle protège
     les droits de la majorité du moment, mais les vivants sont gouvernés par des règles qu'ils
     n'ont pas votées (les constitutions actuelles ; New York demande tous les 20 ans s'il faut
     une convention) ;
  2. **la revue annuelle** : chaque année on vote ce qu'on rouvre, tout le reste est maintenu ;
     seuls les articles contestés reviennent en décision (Athènes au IVe siècle av. J.-C., les
     nomothètes tirés au sort) ;
  3. **l'expiration** : tout expire après N révolutions et doit être reconduit, ou meurt
     (Jefferson à Madison, 1789 : « la terre appartient aux vivants », 19 ans ; le vote annuel de
     l'armée anglaise après 1689) ;
  4. **l'initiative permanente** : on peut rouvrir un article à tout moment, à partir d'un seuil
     de signatures (la Suisse) ;
  5. **les règles par entreprise** : chaque projet commun signe ses propres articles avant de
     commencer (les articles des équipages pirates au XVIIIe siècle, étudiés par Peter Leeson).
- **Ce que le choix ferait au jeu** (proposition) : il fixe le rythme des décisions (le décompte
  du temps) et ce qu'il coûte, entre stabilité, protection des minorités, fatigue de délibérer et
  règle des vivants. Les effets sur les jauges seraient des liens sourcés comme les autres ; c'est
  un nouveau mécanisme, il attend la bascule.
- **Ajout de l'auteur** : parmi ces propositions, le joueur peut aussi **ajouter un ticket**, une
  règle de révision à lui, par la même boucle que les idées des joueurs (voir « Les idées des
  joueurs : le ticket n° 1 »).
