# AGENTS.md : le Labo de la Bande

Consignes pour toute personne ou tout assistant (Claude, autres modèles de langage) qui travaille
sur ce dépôt. Le projet ne dépend d'aucun assistant en particulier : tout ce qu'il faut savoir est
dans le dépôt (ce fichier, `docs/`, `CHANTIERS.md`), jamais dans la mémoire d'un outil.

Un jeu (français) qui explore une société sans prison, sans police et autant que possible sans
monnaie, et ce que chaque choix coûte. Le site public est servi par GitHub Pages depuis ce dépôt.

## Refonte en cours

Le nouveau jeu est en ligne, à côté de l'ancien : https://labo-de-la-bande.github.io/jeu/ (publié à
chaque poussée sur `refonte` par `.github/workflows/jeu.yml`, après `labo doctor`, les tests et la
vérification du moteur compilé contre l'oracle).

Décidée le 21 novembre 2026 : un seul dépôt, des CSV comme seule source, une CLI F# (`labo`),
le moteur et l'interface en F# (Fable). Plan, schéma et état d'avancement :
[docs/refonte.md](docs/refonte.md). Tant que la bascule n'est pas faite, **`index.html` reste le
jeu en ligne** et l'ancien moteur JavaScript fait foi.

## Où lire quoi

- [docs/bible.md](docs/bible.md) : ce que le jeu doit être (boucle, personnages, univers, images).
- [docs/refonte.md](docs/refonte.md) : l'architecture, le schéma des données, l'état d'avancement.
- [CHANTIERS.md](CHANTIERS.md) : les questions de conception ouvertes et les décisions datées.
- [MISES-A-JOUR.md](MISES-A-JOUR.md) : les chiffres à revoir, et quand.
- [docs/relais.md](docs/relais.md) : les idées des joueurs sans compte GitHub (relais, relecture, suivi).

## Commandes

```sh
dotnet tool restore && dotnet paket install      # une fois
dotnet build tools/Labo.Cli
dotnet run --project tools/Labo.Cli -- self install   # une fois : la commande `labo` partout dans le dépôt ; ensuite `labo self install` pour la mettre à jour
tools/Labo.Cli/bin/Debug/net10.0/labo doctor     # data/ valide, F# généré à jour (ne modifie rien)
tools/Labo.Cli/bin/Debug/net10.0/labo generate   # écrit src/Labo.Data/Generated/
tools/Labo.Cli/bin/Debug/net10.0/labo oracle capture   # fige l'ancien moteur dans oracle/ (Chrome ; LABO_CHROME sinon)
dotnet run --project tests/Labo.Tests            # tests Expecto, dont le moteur contre l'oracle
dotnet run --project tools/Labo.Jouer               # le jeu en mode texte, tout le parcours ; -- vivre pour une tranche, --graine N pour rejouer
tools/Labo.Cli/bin/Debug/net10.0/labo explorer   # où le modèle pousse : tout le parcours selon une façon de choisir, puis 10 et 30 ans face à la France
dotnet fable src/Labo.Interface -o jeu/js          # compile l'interface graphique dans jeu/js
tools/Labo.Cli/bin/Debug/net10.0/labo servir      # le jeu dans le navigateur : http://localhost:8080/ (#jouer pour entrer directement)
tools/Labo.Cli/bin/Debug/net10.0/labo verifier-js # le moteur compilé en JavaScript rejoue l'oracle
tools/Labo.Cli/bin/Debug/net10.0/labo assembler   # le jeu en une page autonome (jeu/autonome.html), à vérifier par : labo verifier-js assemble
tools/Labo.Cli/bin/Debug/net10.0/labo publier     # met le jeu assemblé dans jeu/ de main (ce que fait la mise en ligne) ; --essai pour voir
dotnet fable src/Labo.Relais -o src/Labo.Relais/js  # le relais des idées sans compte (Cloudflare Workers) : docs/relais.md
tools/Labo.Cli/bin/Debug/net10.0/labo relais deployer  # le relais sur Cloudflare par son API, sans wrangler (CLOUDFLARE_API_TOKEN) ; --essai pour voir
tools/Labo.Cli/bin/Debug/net10.0/labo idees      # relire les idées envoyées sans compte (collaborateurs du dépôt, avec gh)
tools/Labo.Cli/bin/Debug/net10.0/labo rumeurs telecharger | chercher <sujet>  # des fausses nouvelles déjà vérifiées (Data Commons, sans clé), dans une pile à trier (.tmp/)
```

## Règles

- **Éditer `data/*.csv`, jamais `src/Labo.Data/Generated/`.** Puis `labo generate`, et
  `labo doctor` avant de commiter. Les fichiers générés sont versionnés.
- **Les CSV** : UTF-8, RFC 4180, en-tête exact (voir `tools/Labo.Cli/Schema.fs`). Identifiants
  en kebab-case. Une source citée dans un texte s'écrit `[réf:id]` ; `[réf:?]` marque un chiffre
  qui attend sa source.
- **Les liens** (`data/liens.csv`) : `quand` dans le mini-langage (`coord=plateforme|ia &
  relais!=relais`, `accueil=*`), `poids` (`monnaie!=sans:0.5`, `@risque-gang`), `courbe` pour
  ce qui dépend d'un curseur (une fonction nommée du moteur, jamais une formule dans le CSV).
  Chaque lien a un statut (sourcé, hypothèse, jugement, témoignage, de mémoire) et une raison. Un
  **témoignage** est un savoir de première main (quelqu'un qui l'a vécu ou connu sur place) : il
  vaut par lui-même et cite une source de type « témoignage » qui dit qui témoigne et à quel titre ;
  un texte écrit ne sert qu'à le croiser, jamais à le valider. Règle de fond : un lien
  mesure ce que les règles font subir aux gens, jamais ce qu'un groupe ferait ou serait supposé
  faire.
- **Aucun nombre du modèle dans le code** : un coefficient va dans `data/coefficients.csv`, un fait
  d'un pays dans `data/pays/<pays>/constantes.csv`, avec statut, lentille et raison. Le code désigne
  les options par leur identifiant (`est "invasion" "armee"`), jamais par leur rang.
- **Ne pas changer le comportement du jeu pendant la refonte** : le moteur F# doit reproduire
  l'oracle (`oracle/*.json`) à 10⁻⁶ près. Un nouveau mécanisme attend la bascule (voir
  `CHANTIERS.md`).
- **F#** : paket (jamais `dotnet add package`), Expecto pour les tests, CliWrap pour lancer des
  processus, Fable pour le navigateur. Pas de Python ni de JavaScript écrit à la main. Seule
  exception temporaire : le script que `labo oracle capture` injecte dans l'ancienne page.
- **Ni Python ni Node, même pour un script jetable** gardé hors du dépôt (convertir une table,
  vérifier des données) : une commande `labo`, un script F# (`dotnet fsi`), ou le shell pour une
  retouche simple.
- **Aucun chemin propre à une machine** dans le dépôt : la CLI trouve le dépôt par `Labo.slnx`.
- **Ne jamais relativiser un crime historique établi** (travail forcé, otages fusillés, déportations,
  génocides) : pas de conditionnel, pas de « selon certains », pas de doute affiché sur un témoin
  au nom de récits lus ailleurs. Le doute de méthode porte sur les chiffres et les hypothèses du
  modèle, jamais sur l'existence des crimes ; mettre en doute une victime ou un témoin nourrit le
  négationnisme, même par prudence.
- **Personnages fictifs, faits réels.** Les personnes du jeu sont inventées (y compris dans un
  scénario de guerre avec des victimes) ; les faits qui les entourent sont réels et sourcés. On ne
  compare pas l'État d'aujourd'hui à l'occupation de 1940.
- **Provenance des propositions** : ce qu'un assistant propose sur le fond (un classement dans
  une lentille, une lecture théorique, un texte) est marqué comme sa proposition, jamais présenté
  comme un fait ni validé à la place de l'auteur. L'auteur se méfie à raison de l'orientation que
  donnent les méthodes d'apprentissage : ne pas lui demander de relire des centaines de lignes, mais
  rendre chaque proposition visible et contestable là où elle sert.
- **Labo parle peu de lui-même** : plutôt « Les arguments sont rangés par lentille : une
  proposition, à vérifier » que « C'est moi qui ai rangé… ».
- **Les textes du jeu sont des données** (CSV), pour que des personnes qui n'écrivent pas de code
  (narration, illustration) puissent les reprendre. Chaque texte porte qui l'a écrit et où il en
  est.
- Les textes du jeu suivent la langue du jeu : français clair, chiffres sourcés, et le statut
  de chaque chiffre affiché.
