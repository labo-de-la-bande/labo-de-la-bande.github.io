# Le relais des idées sans compte

Un joueur sans compte GitHub peut proposer une idée depuis le jeu. La page `jeu/proposer.html`
(source : `src/Labo.Interface/Proposer.fs`) l'envoie au **relais** (`src/Labo.Relais`, F# compilé
par Fable pour Cloudflare Workers). Le relais filtre les robots, puis ouvre un ticket dans un dépôt
**privé** de relecture. Une personne de l'équipe relit, et c'est seulement alors qu'un ticket
public s'ouvre. Qui a un compte GitHub ouvre le ticket directement depuis la même page, sans
relecture.

```
joueur ── proposer.html ──► relais ──► dépôt privé « relecture » ──(étiquette publier)──► ticket public
             │  Turnstile, piège à robots,           │ fermé sans publier : écartée
             │  5 envois par heure et par IP          ▼
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
4. S'il a laissé une adresse, il reçoit un mail à chaque étape. Le bouton « Ne plus m'écrire »
   efface l'adresse.
5. Avec une **clé de confiance**, sa proposition est publiée tout de suite, sans relecture.

## Ce que fait l'équipe

- **Relire** : chaque idée arrive comme un ticket du dépôt privé `labo-de-la-bande/relecture`,
  avec l'étiquette « à relire ». Pour recevoir un mail à chaque idée, suivre ce dépôt (Watch → All
  activity).
  - **Publier** : ajouter l'étiquette `publier`. Le relais ouvre le ticket public (étiquettes
    « idée » et « via le jeu »), y met le lien, et ferme le ticket privé.
  - **Écarter** : écrire la raison en commentaire, puis fermer le ticket sans l'étiquette. Le
    dernier commentaire est envoyé au joueur.
- **Faire avancer** un ticket public venu du jeu : les étiquettes « à l'étude », « intégrée »,
  « écartée ». Le fermer vaut « intégrée », et le fermer comme « not planned » vaut « écartée » (la
  raison est le dernier commentaire).
- **Donner une clé de confiance** : `labo cle-confiance <pseudonyme>` affiche la clé à transmettre
  et la commande qui l'enregistre dans le relais. Le relais ne garde que l'empreinte de la clé.

## Données personnelles

- L'**adresse IP** n'est pas gardée : seule son empreinte (SHA-256) sert à compter les envois, et
  elle s'efface au bout d'une heure.
- L'**adresse mail** est facultative. Elle n'apparaît dans aucun ticket. Elle est effacée dès que la
  proposition est intégrée ou écartée, ou quand le joueur clique sur « Ne plus m'écrire ».
- Le **texte** de la proposition est gardé par le relais jusqu'à sa publication, puis seulement
  dans le ticket.
- Le **suivi** (statut, numéros de ticket) s'efface un an après la dernière nouvelle.
- Les mails partent par Brevo, un service établi en France.

## Mise en place (une fois)

Tous les comptes sont à créer **au nom du projet**. Aucun secret n'entre dans le dépôt.

1. **Le dépôt de relecture** : créer le dépôt privé `labo-de-la-bande/relecture`, avec les
   étiquettes `à relire` et `publier`. Dans le dépôt public, créer les étiquettes `via le jeu`,
   `à l'étude`, `intégrée` et `écartée`.
2. **Cloudflare** : créer un compte, puis un widget **Turnstile** pour les domaines
   `labo-de-la-bande.github.io` et `localhost`. La clé publique va dans `cleTurnstile`
   (`src/Labo.Interface/Relais.fs`). La clé secrète devient le secret `TURNSTILE_SECRET` (étape 6).
3. **Le stockage** : depuis `src/Labo.Relais`, lancer `npx wrangler login`, puis
   `npx wrangler kv namespace create IDEES`, et recopier l'identifiant obtenu dans `wrangler.toml`.
4. **L'application GitHub** (réglages de l'organisation → Developer settings → GitHub Apps → New) :
   - nom : « Labo de la Bande, relais » ; elle signera les tickets publiés ;
   - webhook actif, à l'adresse `https://labo-relais.<sous-domaine>.workers.dev/github` (connue
     après le premier déploiement, étape 7 : la mettre à jour alors), avec un secret au hasard
     (`openssl rand -hex 32`) : ce sera `WEBHOOK_SECRET` ;
   - permissions : *Issues* en lecture et écriture ;
   - événements : *Issues* ;
   - l'installer sur l'organisation, pour les deux dépôts seulement ;
   - noter l'**App ID** (page de l'application) et l'**Installation ID** (le nombre à la fin de
     l'adresse de la page d'installation) ;
   - générer une clé privée, puis la convertir : `openssl pkcs8 -topk8 -nocrypt -in cle.pem -out
     cle-pkcs8.pem`.
5. **Brevo** (facultatif, pour les mails) : créer un compte, vérifier l'adresse d'expédition du
   projet (elle va dans `MAIL_EXPEDITEUR`, `wrangler.toml`), et créer une clé API. Sans Brevo, le
   suivi marche quand même, mais sans mail.
6. **Les secrets**, depuis `src/Labo.Relais` :
   ```sh
   npx wrangler secret put TURNSTILE_SECRET
   npx wrangler secret put WEBHOOK_SECRET
   npx wrangler secret put GITHUB_APP_ID
   npx wrangler secret put GITHUB_INSTALLATION_ID
   npx wrangler secret put GITHUB_APP_CLE < cle-pkcs8.pem   # puis effacer cle.pem et cle-pkcs8.pem
   npx wrangler secret put BREVO_CLE                         # si Brevo
   ```
7. **Déployer** : `dotnet fable src/Labo.Relais -o src/Labo.Relais/js`, puis, depuis
   `src/Labo.Relais`, `npx wrangler deploy`. L'adresse affichée va dans `adresse`
   (`src/Labo.Interface/Relais.fs`) et dans le webhook de l'application GitHub. Pousser sur
   `refonte` : la page en ligne ouvre alors l'envoi sans compte.
8. **Déployer depuis GitHub** (facultatif) : créer un jeton Cloudflare (modèle « Edit Cloudflare
   Workers »), l'ajouter comme secret `CLOUDFLARE_API_TOKEN` du dépôt, puis ajouter la variable
   de dépôt `RELAIS_ACTIF` = `oui`. Le flux `relais.yml` redéploie alors le relais à chaque
   changement de `src/Labo.Relais`.

Tant que `adresse` et `cleTurnstile` sont vides, la page n'offre que GitHub, et elle dit que
l'envoi sans compte ouvre bientôt.
