# CLAUDE.md : le Labo de la Bande

Un jeu (français) qui explore une société sans prison, sans police et autant que possible sans
monnaie, et ce que chaque choix coûte. Le site public est servi par GitHub Pages depuis ce dépôt.

## Refonte en cours

Décidée le 21 novembre 2026 : un seul dépôt, des CSV comme seule source, une CLI F# (`labo`),
le moteur et l'interface en F# (Fable). Plan, schéma et état d'avancement :
[docs/refonte.md](docs/refonte.md). Tant que la bascule n'est pas faite, **`index.html` reste le
jeu en ligne** et l'ancien moteur JavaScript fait foi.

## Commandes

```sh
dotnet tool restore && dotnet paket install      # une fois
dotnet build tools/Labo.Cli
tools/Labo.Cli/bin/Debug/net10.0/labo doctor     # data/ valide, F# généré à jour (ne modifie rien)
tools/Labo.Cli/bin/Debug/net10.0/labo generate   # écrit src/Labo.Data/Generated/
tools/Labo.Cli/bin/Debug/net10.0/labo oracle capture   # fige l'ancien moteur dans oracle/ (Chrome ; LABO_CHROME sinon)
dotnet run --project tests/Labo.Tests            # tests Expecto, dont le moteur contre l'oracle
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
  Chaque lien a un statut (sourcé, hypothèse, jugement) et une raison. Règle de fond : un lien
  mesure ce que les règles font subir aux gens, jamais ce qu'un groupe ferait ou serait supposé
  faire.
- **Ne pas changer le comportement du jeu pendant la refonte** : le moteur F# doit reproduire
  l'oracle (`oracle/*.json`) à 10⁻⁶ près. Un nouveau mécanisme attend la bascule (voir
  `CHANTIERS.md`).
- **F#** : paket (jamais `dotnet add package`), Expecto pour les tests, CliWrap pour lancer des
  processus, Fable pour le navigateur. Pas de Python ni de JavaScript écrit à la main. Seule
  exception temporaire : le script que `labo oracle capture` injecte dans l'ancienne page.
- **Aucun chemin propre à une machine** dans le dépôt : la CLI trouve le dépôt par `Labo.slnx`.
- Les textes du jeu suivent la langue du jeu : français clair, chiffres sourcés, et le statut
  de chaque chiffre affiché.
