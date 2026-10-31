# Chantiers

Les grandes questions de conception encore ouvertes. Pour les chiffres à mettre à jour, voir [MISES-A-JOUR.md](MISES-A-JOUR.md).

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
