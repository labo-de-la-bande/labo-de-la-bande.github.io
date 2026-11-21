# Refonte : un dépôt, des métadonnées, du F#

Décidé le 21 novembre 2026 : **un seul dépôt**, une **CLI F# propre au Labo** (`labo`), tout le
JavaScript et le Python remplacés par du F# (Fable pour le navigateur). Les données du jeu
deviennent des **CSV, seule source à éditer** ; la CLI les valide et génère le code qui les lit.
But à terme : pouvoir ajouter d'autres pays que la France.

## Où on en est

| Étape | État |
|---|---|
| 1. Inventaire et schéma | fait (ce document) |
| 2. Oracle, CLI `generate` / `doctor`, premiers CSV | **fait le 21 novembre 2026** : sources, jauges, valeurs France des jauges, leviers, options, liens |
| 3. Moteur F# contre l'oracle | **fait le 21 novembre 2026** : `src/Labo.Engine`, identique à l'ancien moteur sur les 2 300 états des trois lots |
| 3 bis. Retirer le hardcoding | à faire : les tables de `src/Labo.Engine/Tables.fs` (stress test, test de fermeture, échanges, paris, passage à l'échelle, prévention, constantes France) passent en CSV, puis les formules propres à la France sous `data/pays/fr/` ; le reste des tables (quiz, monnaies, héritage, défense, textes du jeu) suit |
| 4. Interface Fable | à faire |
| 5. Bascule du site | à faire |

Ce que l'étape 2 a établi :

- **L'oracle** (`oracle/golden.json`, 300 états ; `oracle/large.json`, 1 000 états) garde
  pour chaque état **l'entrée et les sorties**. Il est déterministe (deux captures identiques à
  l'octet près). Les liens y sont désignés par leur rang dans l'ancien tableau `LINKS`, qui est
  aussi l'ordre des lignes de `data/liens.csv`.
- **Le mini-langage des liens est vérifié contre l'oracle** : les 175 liens sans courbe,
  traduits en `quand` / `poids`, s'activent avec le même poids que dans l'ancien moteur sur les
  1 300 états (227 500 comparaisons, aucun écart). Les 16 liens à courbe seront vérifiés avec le
  moteur F#.
- **Identifiants normalisés en kebab-case** : sources (`powellThyne` → `powell-thyne`) et
  leviers (`pTirage` → `part-tirage`). Les **champs de l'état** (`c.def0`, `s.pTirage`…) ne
  changent pas, pour que les anciens liens de partage `#m=` restent lisibles.
- **Pas de `sorties.toml`** : un seul consommateur, la liste des fichiers générés est dans le code
  (`tools/Labo.Cli/Generate.fs`).
- Ce que l'étape 3 a établi :

- **Un troisième lot d'oracle, `etendu`** (1 000 états), tire ce qu'aucun levier du jeu ne règle
  (horizon de temps, paris, réglages de la monnaie, périmètre, exportations) : sans lui, ces
  chemins du moteur n'auraient jamais été vérifiés. L'oracle garde aussi les scores de la France.
- **Le moteur F# reproduit l'ancien** sur les 2 300 états : jauges, liens actifs, score,
  légitimité, fidélité, viabilité, et les mêmes mesures pour la France. Le test échoue bien
  quand on fausse le moteur (vérifié en changeant une constante et une courbe).
- Le moteur compile avec Fable. Reste à vérifier que **sa version JavaScript compilée** donne
  les mêmes nombres (à faire avec l'interface, étape 4).
- Pas encore portés, faute d'être utilisés par le calcul : les textes des jauges (`t`, `h`), les
  objectifs officiels (`goal`) et la tendance de la Liberté, qui ne servent qu'à l'affichage.

L'extraction depuis l'ancien `index.html` était ponctuelle et n'est pas dans le dépôt : à
  partir de maintenant, **les CSV font foi**.

Reste dans `~/repo/labo-outils` (hors dépôt), à porter en commandes `labo` une fois le moteur F#
écrit : `verify_net.py` (liens jamais activés, valeurs hors bornes), `sens.py` (sensibilité),
`prevention.py`, `test_ecrans.py`, `stylediff.py`. `golden.py` est remplacé par
`labo oracle capture`.

---

L'étape 1 suit : l'inventaire de ce que contient `index.html` et le schéma.

## 1. Inventaire (mesuré, pas déduit)

Le moteur a été exécuté dans Node et chaque table inspectée. `index.html` contient deux scripts :
le moteur (environ 145 000 caractères, dans une IIFE, exporté par `window.LABO`) et le jeu
(environ 59 000 caractères).

### Tables du moteur

| Table | Taille | Forme | Contenu |
|---|---|---|---|
| `SRC` | 139 | `{t, u, f?:{p, ty, m, lim}}` | Sources. 70 ont une fiche : producteur, type (primaire, secondaire…), méthode, limites |
| `LINKS` | 191 | `{f, g, d, st, n, w, r?, c?}` | Le réseau d'effets : condition `f`, jauge `g`, effet `d`, statut, nature, raison `w`, source `r`, contre-preuve `c` |
| `QUIZ` | 10 | `{c, ok, exp, src, ref, st}` | Affirmations vrai/faux |
| `STRESS` | 9 | `{a, k, v, why}` | Attaques du stress test sans police |
| `PER` | 10 | `{r, m, o, why}` | Ressources du test de fermeture (métropole / outre-mer) |
| `MON` | 5 | `{label, short, desc, v, n, cost, dist}` | Formes de monnaie, avec un verdict et une note par épreuve |
| `HER` | 5 | `{label, desc, crit, c, eg}` | Régimes d'héritage |
| `DEF` | 5 | `{t, o:[2]}` | Scénarios de défense |
| `DIL` | 2 | `{q, o}` | Dilemmes |
| `RES` | 3 | `{t, facts, src, ref, o:[{l, cost, imp, per, fx, y?, need?, s?}]}` | Ressources qui manquent (engrais, énergie, médicaments) |
| `OUV` | 4 | `{l, d, dist}` | Niveaux d'ouverture du pays |
| `BETS` | 27 | `{id, cat, t, mat, ready, src, ref, d, eff, fail, comfort}` | Paris sur des solutions en développement |
| `SCALE` | 5 | `{id, t, p, fact, src, ref, par:[{id, l, adopt?, cap?, auto?}]}` | Faiblesses du passage à l'échelle et parades (hors jeu depuis le 31 octobre) |
| `ACC`, `SOIN`, `LOGT`, `VCO`, `COORD`, `RELAIS`, `SURV`, `ALGO`, `LAIC` | 2 à 4 chacune | `{l, n}` | Options des leviers simples |
| `SDEF` | 19 | nombres | Valeurs par défaut des curseurs (surface, besoin, énergie, paramètres politiques…) |
| `FRD` | 13 | `[date, quand revoir]` | Date de chaque valeur France et rythme de révision |
| `PREV` | 3 | facteurs | Prévention du gang (× 0,85, × 0,92, × 0,70) |
| `COVER`, `IMPV`, `EXPO`, `RESROW`, `MAT`, `HZ`, `RANK`, `PIDS` | petites | divers | Tables de calcul (couverture, échanges, maturité, horizons) |

### Tables du jeu

| Table | Contenu |
|---|---|
| `TR` | 5 tranches ; 40 étapes : 11 constats (chiffres sourcés), 22 dilemmes (levier, question, voix d'habitants), 5 bilans, 1 prévention |
| `lev` | 35 leviers, dont 6 curseurs (`range`) ; chacun : titre, ancre, options, lecture de l'état |
| `ATL` | Pour chaque jauge, les leviers de l'atelier qui la touchent |
| `PARS` | Parades actives par défaut |

### Les liens, en détail

Le point délicat est la condition `f` des liens, qui est une fonction. Mesuré :

- **147 sur 191** sont déclaratives : `LKON(levier, option)` (138) ou `LKG(option)` (9, gang
  pondéré par la prévention) ;
- **15** sont presque déclaratives : `LKMON(monnaie)`, `LKOUV(niveau)`, `LKPV(moyen)`, ou une
  combinaison (`coord>=1 et relais=0`), parfois avec un **poids** (la coordination compte à
  moitié quand une monnaie existe déjà) ;
- **environ 15** sont des **courbes** sur un curseur (`pTirage`, `pRevoc`, `pMandat`, `prSocle`,
  `accD`, rendement des terres).

Statuts : 8 sourcés, 66 hypothèses, 117 jugements.

### Les valeurs France

Elles sont aujourd'hui **dispersées dans le code** du calcul des jauges (`gaugesCalc`) : 975
homicides, 90 020 détenus, 29,5 Md€, 15,4 % sous le seuil de pauvreté, 12 % qui ne mangent pas à
leur faim, 776 TWh, la valeur « France aujourd'hui » de chaque jauge (`now`)… avec leurs dates
dans `FRD` et leur suivi dans `MISES-A-JOUR.md`. Les extraire est la condition pour ajouter un
pays.

## 2. Arborescence proposée

```
data/                         seule source à éditer (CSV, UTF-8, RFC 4180, en-tête exact)
├── sources.csv               id, titre, url, producteur, type, methode, limites
├── sources-documents.csv     source, rang, titre, url (une source faite de plusieurs documents)
├── jauges.csv                id, libelle_fr, ancre, seuil_vital, seuil_france, hors_score, aide_fr
├── leviers.csv               id, titre_fr, ancre, genre (choix | oui-non | curseur), min, max, pas, defaut
├── options.csv               levier, rang, id, libelle_fr, note_fr
├── liens.csv                 id, jauge, effet, statut, nature, raison_fr, source, contre_source,
│                             quand, poids, courbe
├── indicateurs.csv           id, libelle_fr, unite, description_fr   (communs à tous les pays)
├── pays/
│   └── fr/
│       ├── pays.csv          nom, population, perimetre…
│       └── valeurs.csv       indicateur, valeur, date, revoir, source, statut
├── quiz.csv, stress.csv, paris.csv, ressources.csv, ressource-options.csv,
│   monnaies.csv, monnaie-verdicts.csv, heritage.csv, defense.csv, ouverture.csv, perimetre.csv
└── jeu/
    ├── tranches.csv          id, titre_fr, sous_titre_fr, couleur, emoji, jauges
    ├── etapes.csv            tranche, rang, genre (constat | dilemme | bilan | prevention), levier, titre_fr, texte_fr
    ├── chiffres.csv          etape, valeur_fr, legende_fr, sources
    └── voix.csv              etape, rang, texte_fr
src/
├── Labo.Data/                F# généré depuis data/ (ne pas éditer)
├── Labo.Engine/              le moteur, pur : un état donne les jauges (partagé .NET / Fable)
└── Labo.Game/                l'interface, compilée par Fable en modules ES, sans bundler
tools/Labo.Cli/               labo doctor | generate | verify | sens | oracle
tests/Labo.Tests/             tests du moteur, dont la comparaison avec l'oracle
oracle/                       résultats du moteur JavaScript actuel, figés (temporaire)
docs/
```

### Choix de conception

- **Les conditions des liens (`quand`)** s'écrivent dans un mini-langage que la CLI analyse et
  vérifie contre `leviers.csv` et `options.csv` : `coord=plateforme`, `coord>=1 & relais=0`,
  `monnaie=sans`, `ouverture=3`. Les options sont désignées par leur **id**, plus par leur rang :
  insérer une option ne casse plus les liens.
- **`poids`** : vide (1), un nombre, ou `monnaie!=sans:0.5` (ce poids s'applique quand la
  condition est vraie).
- **`courbe`** : pour la quinzaine de liens qui dépendent d'un curseur, le nom d'une fonction F#
  du moteur (`tirage-hausse`, `socle-pente`…). La CLI vérifie que le nom existe. On ne met pas de
  formules dans les CSV : elles restent du code, typé et testé.
- **Les textes portent le suffixe `_fr`** dès maintenant : ajouter une langue revient à ajouter
  une colonne.
- **Les données sont générées en F# typé** (`Labo.Data`) : une jauge ou un levier qui n'existe
  pas devient une erreur de compilation. Les fichiers générés sont versionnés, pour que le dépôt
  se compile sans lancer la CLI. Plus tard, les données de chaque pays pourront être chargées à la
  demande (JSON) au lieu d'être toutes livrées dans la page.
- **`labo doctor`** vérifie les CSV (erreur avec le fichier et la ligne), que les fichiers
  générés sont à jour, que chaque source citée existe, que chaque lien a un statut et une raison,
  et **liste les valeurs dont la date de révision est dépassée**. `MISES-A-JOUR.md` devient un
  rapport généré.
- **Pas de npm** : Fable produit des modules ES que GitHub Pages sert tels quels. Le déploiement
  passe par une GitHub Action (`dotnet fable`, puis publication).

## 3. Méthode : l'oracle d'abord

Le jeu JavaScript actuel fait foi tant que le portage n'est pas vert.

1. **Capture de l'oracle.** Le moteur actuel tourne dans Node sans navigateur (vérifié) : on fige
   ses résultats sur les 300 états de `golden.py` et les 3 000 de `verify_net.py` (score,
   fidélité, légitimité, viabilité, chaque jauge, chaque lien actif). C'est la seule exception
   temporaire au « zéro JavaScript » : le script de capture disparaît avec l'ancien moteur, seuls
   les JSON restent.
2. **CSV et `labo generate` / `labo doctor`**, remplis par une extraction automatique depuis les
   tables actuelles, puis relus.
3. **Moteur F#**, testé contre l'oracle à 10⁻⁶ près.
4. **Interface Fable**, écran par écran, comparée au jeu actuel.
5. **Bascule du site.**
6. Ensuite seulement : nouveaux mécanismes (coordination impersonnelle, voir `CHANTIERS.md`),
   répartitions au lieu du tout ou rien, puis le travail « pays ».

Aucun changement de comportement du jeu pendant les étapes 1 à 5.

## 4. Questions encore ouvertes

- **Les valeurs « France aujourd'hui » (`now`) des jauges** : une ligne de `pays/fr/valeurs.csv`
  par jauge, ou une colonne de `jauges.csv` ? Je propose `valeurs.csv`, car ce sont des valeurs
  d'un pays, pas des propriétés d'une jauge.
- **Les tables de calcul du test de fermeture** (`PER`, `COVER`, `IMPV`, `EXPO`) dépendent de la
  France (métropole et outre-mer). Elles iront sous `pays/fr/`, mais leur forme générale ne sera
  claire qu'avec un deuxième pays.
