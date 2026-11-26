# L'atelier des données

Des collaborateurs invités modifient des tables du jeu **sans compte GitHub**, depuis la page
`jeu/atelier.html`. Le relais (Cloudflare) écrit chaque modification comme un commit dans un dépôt
à part, `labo-de-la-bande/atelier`, qui n'est qu'une salle d'attente. L'auteur relit et verse les
modifications dans le dépôt du jeu avec `labo atelier fusionner`.

```
invité ── atelier.html (code d'invitation) ──► relais ──► commit dans labo-de-la-bande/atelier
                                                              │ (application GitHub, ce dépôt seul)
auteur ◄── labo atelier fusionner (gh) ◄──────────────────────┘
   └── relire, commiter, labo atelier envoyer (l'atelier repart de data/)
```

Les tables ouvertes, et les colonnes qu'on peut y écrire, sont listées dans
`src/Labo.Relais/Atelier.fs` (aujourd'hui : `data/jeu/abolitions.csv` et
`data/jeu/types-partie.csv`). `labo doctor` vérifie que leurs en-têtes restent ceux de
`tools/Labo.Cli/Schema.fs`.

## Ce que voit l'invité

1. Il ouvre https://labo-de-la-bande.github.io/jeu/atelier.html et entre son code (`atl_…`).
2. Il voit les tables, ligne par ligne. « Modifier » ouvre les colonnes permises ; « Ajouter une
   ligne », une ligne nouvelle. Chaque changement demande une phrase : pourquoi.
3. Le changement est enregistré aussitôt dans l'atelier ; l'historique public dit qui a changé
   quoi, et pourquoi : https://github.com/labo-de-la-bande/atelier/commits/main.

## Ce que fait l'auteur

- `labo atelier inviter <id> <nom…>` : une invitation. Le code ne s'affiche **qu'une fois** ; le
  transmettre à la personne seule, par un canal privé. `<id>` devient la provenance de ses lignes
  dans les CSV (minuscules, chiffres, tirets).
- `labo atelier invitations`, `labo atelier retirer <id>` : retirer coupe l'accès en une minute au plus (le temps que le stockage de Cloudflare propage la suppression).
- `labo atelier fusionner` : verse dans `data/` ce que l'atelier a changé depuis le dernier
  envoi, ligne par ligne et champ par champ (fusion à trois avec `.base/`), puis régénère. Un
  champ changé des deux côtés garde la version locale et s'affiche comme conflit. Puis : relire
  (`git diff data/`), `labo doctor`, commiter.
- `labo atelier envoyer` : remet dans l'atelier les tables de `data/` (et `.base/`). Refuse tant
  que l'atelier a des changements pas encore fusionnés.

## Sécurité

- **Isolement** : l'application GitHub de l'atelier n'est installée que sur le dépôt `atelier`,
  avec le seul droit « Contents » (ni « Workflows », ni « Administration »). Même volée, sa clé
  ne peut rien écrire dans le dépôt du jeu. Chaque requête du relais demande un jeton d'une heure,
  réduit à ce dépôt et à ce droit.
- **Règles du dépôt `atelier`** : sa branche `main` refuse les réécritures d'historique et la
  suppression, pour tout le monde : l'historique (qui a changé quoi) ne peut pas être effacé.
- **Rien d'autre que des tables n'entre dans le jeu** : `labo atelier fusionner` ne lit que les
  tables listées dans `Atelier.fs`, et seulement leurs lignes ; aucun code ne vient de l'atelier.
- **Le relais n'écrit que ce que `Atelier.fs` permet** : une table listée, une colonne permise,
  une valeur d'une ligne (1000 caractères au plus, sans caractère de contrôle), un identifiant
  bien formé, l'en-tête exact. Il réécrit lui-même le CSV (RFC 4180) : l'invité n'envoie jamais
  de fichier. Il refuse si la ligne a changé depuis que l'invité l'a lue.
- **Codes d'invitation** : 256 bits aléatoires ; le relais n'en garde que l'empreinte (SHA-256).
  20 essais ratés par heure et par IP (empreinte seulement), 60 modifications par heure et par
  invité ; seules les pages de `labo-de-la-bande.github.io` peuvent appeler le relais.
- **La page** : le code reste dans l'onglet (`sessionStorage`), jamais dans l'adresse ; une
  politique de sécurité (CSP) n'autorise que son propre script et le relais.
- **Les invitations** se créent et se retirent seulement par un collaborateur du dépôt du jeu
  (jeton `gh` vérifié auprès de GitHub, comme pour `labo idees`).
- **Clé privée** : jamais dans le dépôt ni dans une conversation. `labo atelier cle` la convertit
  et la pose comme secret du relais sans l'afficher ; effacer ensuite le fichier `.pem`. Une clé
  compromise se révoque dans les réglages de l'application (GitHub → Settings → Developer
  settings → GitHub Apps → labo-atelier → Private keys).

## Mise en place

1. **Le dépôt** `labo-de-la-bande/atelier` (public), avec la règle « main » : pas de réécriture
   d'historique, pas de suppression.
2. **L'application GitHub**, au nom de l'organisation (Settings de l'organisation → Developer
   settings → GitHub Apps → New GitHub App) : nom `labo-atelier` ; pas d'adresse de rappel ;
   webhook désactivé ; permissions du dépôt : **Contents : Read and write**, rien d'autre ;
   « Only on this account ». Puis « Install App » → **Only select repositories** → `atelier`.
3. **Les identifiants** : l'App ID (page de l'application) et l'identifiant de l'installation (le
   nombre à la fin de l'adresse de la page d'installation) vont dans `ATELIER_APP_ID` et
   `ATELIER_INSTALLATION` (`src/Labo.Relais/wrangler.toml`).
4. **La clé** : « Generate a private key », puis `labo atelier cle <fichier.pem>`, puis effacer
   le fichier.
5. **Déployer le relais** : `dotnet fable src/Labo.Relais -o src/Labo.Relais/js`, puis
   `labo relais deployer`.
6. **Remplir l'atelier** : `labo atelier envoyer`. Puis inviter.
