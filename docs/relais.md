# Le relais des idées et des prénoms

Un joueur sans compte GitHub peut proposer une idée depuis le jeu. La page `jeu/proposer.html`
(source : `src/Labo.Interface/Proposer.fs`) l'envoie au **relais** (`src/Labo.Relais`, F# compilé
par Fable pour Cloudflare Workers). Le relais filtre les robots et garde l'idée en attente. **Un
collaborateur du dépôt GitHub la relit** avec `labo idees`, et la publie (un ticket public, ouvert
avec son compte) ou l'écarte. Qui a un compte GitHub ouvre le ticket directement depuis la même
page.

```
joueur ── proposer.html ──► relais (en attente) ◄── labo idees (collaborateur, jeton gh)
             │  Turnstile, piège à robots,                │ publier : ticket public (gh)
             │  5 envois par heure et par IP              │ écarter : la raison
             └──── suivi (code) ◄──────────── statut, mail facultatif
```

## Ce que vit le joueur

1. Il écrit son idée dans le jeu, puis clique sur « L'envoyer au projet ». La page s'ouvre
   préremplie.
2. Sans compte : il choisit d'où vient le texte, peut laisser une adresse mail, passe la
   vérification anti-spam (une case, sans puzzle d'images), puis envoie. Il reçoit un **lien de
   suivi**.
3. Le lien de suivi, et la liste « Mes propositions » de la page, disent où en est l'idée : reçue,
   relue et publiée (avec le lien du ticket public, lisible sans compte), à l'étude, intégrée, ou
   écartée (avec la raison).
4. S'il a laissé une adresse, et si l'envoi de mails est branché (Brevo, facultatif), il reçoit un
   mail à chaque étape. Le bouton « Ne plus m'écrire » efface l'adresse.

## Ce que font les collaborateurs

Il suffit d'avoir cloné le dépôt et d'être connecté à `gh` (`gh auth login`) : le relais demande à
GitHub si ce compte peut écrire dans le dépôt. Aucune clé à se transmettre ; un collaborateur
ajouté sur GitHub peut relire tout de suite, un collaborateur retiré perd l'accès aussitôt.

- `labo idees` : les idées à relire (situation, idée, appuis, provenance, signature), et celles
  publiées dont le sort reste à suivre.
- `labo idees publier <code>` : ouvre le ticket public avec ton compte (étiquettes « idée » et
  « via le jeu »), et le joueur voit le lien sur son suivi. Les huit premiers caractères du code
  suffisent.
- `labo idees ecarter <code> "raison"` : le joueur voit la raison sur son suivi.
- `labo idees suivre` : reporte le sort des tickets publiés sur le suivi des joueurs : les
  étiquettes « à l'étude », « intégrée », « écartée », ou la fermeture du ticket (« not planned » :
  écartée, sinon intégrée ; la raison est le dernier commentaire).
- `labo prenoms importer` : les prénoms proposés (voir plus bas).

Une appli de relecture pourra venir plus tard : elle parlera au relais par les mêmes routes
(`/moderation/…`, voir `src/Labo.Relais/Worker.fs`).

## Les prénoms proposés par les joueurs

Dans le jeu, la fiche d'un habitant permet de **lui donner un autre prénom** (pour la partie : il
est gardé dans le code de partie). Le joueur peut ensuite **le proposer aux banques** : la page
`jeu/prenom.html` (source : `src/Labo.Interface/Prenom.fs`) arrive préremplie avec la décennie de
naissance, le groupe de pays et le sexe du personnage, et demande dans quel pays du groupe, et à
qui ce prénom se donne là-bas (aux filles, aux garçons, aux deux, je ne sais pas). C'est un
témoignage sur des prénoms, pas une donnée sur le joueur. Le relais ajoute 1 au compte « ce
prénom, ce pays, cette décennie, cet usage ».

- **Verser les comptes** : `labo prenoms importer` (en collaborateur, avec `gh`), ou `labo prenoms
  importer <export.json>`. Seuls les comptes d'au moins 3 déclarations entrent dans
  `data/prenoms/joueurs.csv` (coefficient `prenoms-joueurs-seuil`). Les prénoms nouveaux sont
  listés, à valider.
- **Valider** : écrire `auteur` dans la colonne `valide_par`, puis `labo generate`. Le tirage ne
  se sert que des prénoms validés.
- **Dans le tirage** : pour un personnage né à l'étranger, les prénoms déclarés par des joueurs nés
  dans un pays de son groupe et dans sa décennie passent en premier. Ils respectent l'âge, ce que
  la banque de l'INE ne fait pas. À partir de 5 prénoms différents (coefficient
  `prenoms-joueurs-minimum`), ils servent seuls ; en dessous, ils s'ajoutent à ceux de l'INE.

## Données personnelles

- L'**adresse IP** n'est pas gardée : seule son empreinte (SHA-256) sert à compter les envois, et
  elle s'efface au bout d'une heure.
- L'**adresse mail** est facultative. Elle n'apparaît dans aucun ticket. Elle est effacée dès que la
  proposition est intégrée ou écartée, ou quand le joueur clique sur « Ne plus m'écrire ».
- Le **texte** de la proposition est gardé par le relais jusqu'à sa publication, puis seulement
  dans le ticket.
- Le **suivi** (statut, numéros de ticket) s'efface un an après la dernière nouvelle.
- Les **prénoms proposés** ne sont gardés qu'en comptes (prénom, pays, décennie, usage : un
  nombre), sans rien qui relie un compte à une personne. Un prénom n'entre dans une banque qu'à
  partir de 3 propositions, comme l'Insee qui ne publie pas les prénoms donnés moins de trois
  fois.
- Les mails partent par Brevo, un service établi en France.

## Sécurité

- **Anti-spam** : le jeton Turnstile est vérifié par le relais, qui exige aussi qu'il ait été émis
  sur `labo-de-la-bande.github.io` (variable `HOTES`) et pour la bonne page (action `idee` ou
  `prenom`). Champ piège, cinq envois par heure et par IP, corps limité à 20 Ko.
- **Origines** : seul `https://labo-de-la-bande.github.io` peut écrire au relais (variable
  `ORIGINES`) ; `localhost` n'y est pas. Pas d'essai local du relais pour l'instant : l'outil de
  Cloudflare pour ça (wrangler) tourne sous Node, que le projet n'utilise pas.
- **Pages** : `proposer.html` et `prenom.html` déclarent une politique de sécurité (CSP) : seul leur
  propre script (par son empreinte, calculée par `labo assembler`), le widget Cloudflare, les
  polices et le relais sont autorisés.
- **Secrets** : jamais dans le dépôt ni dans une conversation ; une clé qui y est passée se
  renouvelle (Turnstile : Settings → Rotate Secret Key, puis `labo relais
  secret TURNSTILE_SECRET`). Double authentification sur les comptes Cloudflare et
  GitHub.
- **Aucune action GitHub de tiers** (`.github/workflows/`) : les flux ne lancent que git, le script
  d'installation officiel de .NET et la CLI `labo` ; rien ne change sans passer par le dépôt.
- **Confiance** : seuls les collaborateurs du dépôt GitHub décident (publier, écarter, valider) ; le
  relais le vérifie auprès de GitHub à chaque fois, avec le jeton `gh` du collaborateur, qu.il ne
  garde pas. Tout le monde peut proposer : idées sans compte, prénoms, tickets GitHub.

## Mise en place (faite le 23 novembre 2026)

Tous les comptes sont au nom du projet. Aucun secret n'entre dans le dépôt.

1. **Cloudflare** : un compte, puis un widget **Turnstile** pour le domaine
   `labo-de-la-bande.github.io`. La clé publique est dans `cleTurnstile`
   (`src/Labo.Interface/Relais.fs`) ; la clé secrète est le secret `TURNSTILE_SECRET`.
2. **Le stockage** : un espace KV `IDEES`, créé une fois (son identifiant est dans
   `wrangler.toml`).
3. **Les secrets** : `labo relais secret TURNSTILE_SECRET` (la valeur est demandée, jamais en
   argument), et, si l'on branche les mails, `labo relais secret BREVO_CLE` (et l'adresse d'expédition dans
   `MAIL_EXPEDITEUR`, `wrangler.toml`).
4. **Déployer** : `dotnet fable src/Labo.Relais -o src/Labo.Relais/js`, puis
   `labo relais deployer` (par l'API de Cloudflare, sans wrangler ni Node ; `--essai` montre ce qui
   serait envoyé). Il faut un jeton Cloudflare dans `CLOUDFLARE_API_TOKEN`. L'adresse du relais est dans `adresse`
   (`src/Labo.Interface/Relais.fs`) et dans `tools/Labo.Cli/Moderation.fs`.
5. **Déployer depuis GitHub** (facultatif) : un jeton Cloudflare (modèle « Edit Cloudflare
   Workers ») en secret `CLOUDFLARE_API_TOKEN` du dépôt, et la variable `RELAIS_ACTIF` = `oui` : le
   flux `relais.yml` redéploie alors le relais à chaque changement de `src/Labo.Relais`.
