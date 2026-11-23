// GÉNÉRÉ depuis data/prenoms/pays.csv, data/prenoms/joueurs.csv, data/pays-du-monde.csv par `labo generate`.
// Ne pas éditer : changer les données et régénérer.
module Labo.Data.Prenoms

open Labo.Data

/// Les prénoms les plus portés dans chaque pays (pays;nom du pays;sexe;prénom;pour mille).
let prenomsTexte =
    """es;Espagne;m;Antonio;30.5
de;Allemagne;m;Michael;11.6
be;Belgique;m;Jean Pierre;3.4
bg;Bulgarie;m;Ivan;24.1
fr;France;m;Nicolas;5.1
ie;Irlande;m;John;11.3
it;Italie;m;Marco;17.8
lt;Lituanie;m;Tomas;25.2
md;Moldavie;m;Ion;60.4
no;Norvège;m;Terje;8.9
nl;Pays-Bas;m;Jan;6.8
pl;Pologne;m;Krzysztof;13.8
pt;Portugal;m;Jose Manuel;14.7
gb;Royaume-Uni;m;David;9.9
ro;Roumanie;m;Gheorghe;19.1
ru;Russie;m;Alexander;42.6
se;Suède;m;Alexander;2.2
ch;Suisse;m;Peter;15.7
ua;Ukraine;m;Oleksandr;57.7
es;Espagne;m;Manuel;26.9
de;Allemagne;m;Thomas;11
be;Belgique;m;Marc;3.2
bg;Bulgarie;m;Georgi;21.1
fr;France;m;Philippe;4.8
ie;Irlande;m;Michael;8.8
it;Italie;m;Alessandro;16.4
lt;Lituanie;m;Lukas;16.3
md;Moldavie;m;Alexandru;35.1
no;Norvège;m;Jan;6.7
nl;Pays-Bas;m;Johannes;5
pl;Pologne;m;Tomasz;12.6
pt;Portugal;m;Antonio;13.1
gb;Royaume-Uni;m;John;9.2
ro;Roumanie;m;Vasile;19.1
ru;Russie;m;Vladimir;33.6
se;Suède;m;Daniel;2.2
ch;Suisse;m;Daniel;9.2
ua;Ukraine;m;Volodymyr;52.8
es;Espagne;m;Jose;25.9
de;Allemagne;m;Andreas;8.8
be;Belgique;m;Patrick;3
bg;Bulgarie;m;Nikolay;14.9
fr;France;m;Jean Pierre;4.6
ie;Irlande;m;David;7.4
it;Italie;m;Andrea;16.4
lt;Lituanie;m;Mindaugas;16.2
md;Moldavie;m;Andrei;32.6
no;Norvège;m;Erik;6.2
nl;Pays-Bas;m;Robert;4.7
pl;Pologne;m;Piotr;12.5
pt;Portugal;m;Manuel;11.8
gb;Royaume-Uni;m;Paul;6.6
ro;Roumanie;m;Ioan;17.4
ru;Russie;m;Sergey;32.3
se;Suède;m;Jan Erik;2.1
ch;Suisse;m;Thomas;7.5
ua;Ukraine;m;Vasyl;38.6
es;Espagne;m;Francisco;22.2
de;Allemagne;m;Peter;7.4
be;Belgique;m;Nicolas;2.7
bg;Bulgarie;m;Dimitar;14.4
fr;France;m;Alexandre;4.4
ie;Irlande;m;Patrick;6.8
it;Italie;m;Francesco;15.9
lt;Lituanie;m;Andrius;15.9
md;Moldavie;m;Mihail;30.3
no;Norvège;m;Tore;6
nl;Pays-Bas;m;Cornelis;4.5
pl;Pologne;m;Andrzej;12.5
pt;Portugal;m;Jose;9.6
gb;Royaume-Uni;m;Michael;6.4
ro;Roumanie;m;Ion;14.9
ru;Russie;m;Aleksandr;28.3
se;Suède;m;Lars Erik;2
ch;Suisse;m;Markus;7.2
ua;Ukraine;m;Ivan;36.8
es;Espagne;m;David;17.7
de;Allemagne;m;Christian;7.3
be;Belgique;m;Jean Claude;2.5
bg;Bulgarie;m;Krasimir;9.5
fr;France;m;David;4.1
ie;Irlande;m;Paul;6.8
it;Italie;m;Giuseppe;14.2
lt;Lituanie;m;Marius;15.9
md;Moldavie;m;Sergiu;30.1
no;Norvège;m;Rune;5.7
nl;Pays-Bas;m;Hendrik;4.2
pl;Pologne;m;Pawel;11.4
pt;Portugal;m;Carlos Alberto;9
gb;Royaume-Uni;m;Peter;5.6
ro;Roumanie;m;Constantin;14.6
ru;Russie;m;Mikhail;27.9
se;Suède;m;Peter;1.9
ch;Suisse;m;Walter;6.2
ua;Ukraine;m;Roman;32.4
es;Espagne;m;Juan;15.5
de;Allemagne;m;Alexander;7
be;Belgique;m;Philippe;2.3
bg;Bulgarie;m;Petar;8.6
fr;France;m;Michel;3.9
ie;Irlande;m;Patrick Joseph;6.4
it;Italie;m;Luca;13.3
lt;Lituanie;m;Darius;14.8
md;Moldavie;m;Victor;28.9
no;Norvège;m;Trond;5.2
nl;Pays-Bas;m;Willem;3.9
pl;Pologne;m;Marcin;10
pt;Portugal;m;Carlos Manuel;8.4
gb;Royaume-Uni;m;David John;5.4
ro;Roumanie;m;Marian;13.7
ru;Russie;m;Andrey;26.7
ch;Suisse;m;Heinz;5.8
ua;Ukraine;m;Mykola;29.6
es;Espagne;m;Javier;15
de;Allemagne;m;Frank;6.1
be;Belgique;m;Jean Marie;2.3
bg;Bulgarie;m;Plamen;8.5
fr;France;m;Pierre;3.9
ie;Irlande;m;James;6.3
it;Italie;m;Antonio;11.8
lt;Lituanie;m;Arturas;14.5
md;Moldavie;m;Vasile;28.7
no;Norvège;m;Bjorn;5
nl;Pays-Bas;m;Ronald;3.5
pl;Pologne;m;Marek;9.1
pt;Portugal;m;Paulo Jorge;7.9
gb;Royaume-Uni;m;Michael John;5.2
ro;Roumanie;m;Florin;13
ru;Russie;m;Igor;22.6
ch;Suisse;m;Kurt;5.5
ua;Ukraine;m;Andriy;29
es;Espagne;m;Jose Antonio;14.6
de;Allemagne;m;Stefan;5.6
be;Belgique;m;Mohamed;2.3
bg;Bulgarie;m;Martin;8.4
fr;France;m;Christophe;3.7
ie;Irlande;m;John Joseph;5.3
it;Italie;m;Stefano;10.3
lt;Lituanie;m;Vytautas;13.6
md;Moldavie;m;Nicolae;27.6
no;Norvège;m;Per;5
nl;Pays-Bas;m;Peter;3.4
pl;Pologne;m;Adam;8.7
pt;Portugal;m;Pedro Miguel;7.8
gb;Royaume-Uni;m;Stephen;4.5
ro;Roumanie;m;Nicolae;12.3
ru;Russie;m;Alexey;22.4
ch;Suisse;m;Rene;5.5
ua;Ukraine;m;Dmytro;25.5
es;Espagne;m;Daniel;14.4
de;Allemagne;m;Daniel;5.6
be;Belgique;m;Michel;2.1
bg;Bulgarie;m;Hristo;8.3
fr;France;m;Antoine;3.6
ie;Irlande;m;Brian;5.1
it;Italie;m;Giovanni;10.2
lt;Lituanie;m;Arunas;13.4
md;Moldavie;m;Gheorghe;26.3
no;Norvège;m;Arne;4.8
nl;Pays-Bas;m;Mohamed;3.3
pl;Pologne;m;Grzegorz;8.2
pt;Portugal;m;Jose Antonio;7.3
gb;Royaume-Uni;m;James;4.4
ro;Roumanie;m;Daniel;11.8
ru;Russie;m;Ivan;20.9
ch;Suisse;m;Werner;5.5
ua;Ukraine;m;Ihor;24.6
es;Espagne;m;Francisco Javier;13.8
de;Allemagne;m;Wolfgang;5.5
be;Belgique;m;David;2.1
bg;Bulgarie;m;Ivaylo;8
fr;France;m;Stephane;3.6
ie;Irlande;m;Thomas;4.5
it;Italie;m;Roberto;10
lt;Lituanie;m;Mantas;12.1
md;Moldavie;m;Igor;20.5
no;Norvège;m;Christian;4.6
nl;Pays-Bas;m;Pieter;3.3
pl;Pologne;m;Lukasz;8.1
pt;Portugal;m;Luis Miguel;6.8
gb;Royaume-Uni;m;Robert;4.2
ro;Roumanie;m;Adrian;10.9
ru;Russie;m;Dmitry;20.3
ch;Suisse;m;Martin;4.8
ua;Ukraine;m;Viktor;21.5
es;Espagne;f;Maria Carmen;29.6
de;Allemagne;f;Claudia;6.4
be;Belgique;f;Nathalie;3
bg;Bulgarie;f;Mariya;10.6
fr;France;f;Nathalie;5.4
ie;Irlande;f;Mary;9.6
it;Italie;f;Valentina;12.4
lt;Lituanie;f;Kristina;23.4
md;Moldavie;f;Natalia;44.8
no;Norvège;f;Kari;5.7
nl;Pays-Bas;f;Johanna;3.8
pl;Pologne;f;Anna;20.5
pt;Portugal;f;Maria Fatima;15.7
gb;Royaume-Uni;f;Susan;6.2
ro;Roumanie;f;Maria;27.5
ru;Russie;f;Elena;67.1
se;Suède;f;Anna Maria;2.8
ch;Suisse;f;Ursula;9.1
ua;Ukraine;f;Iryna;45.6
es;Espagne;f;Maria;25.8
de;Allemagne;f;Sabine;6.3
be;Belgique;f;Anne Marie;2.9
bg;Bulgarie;f;Daniela;7.6
fr;France;f;Catherine;3.7
ie;Irlande;f;Catherine;5.6
it;Italie;f;Francesca;11.4
lt;Lituanie;f;Lina;16.9
md;Moldavie;f;Tatiana;44.4
no;Norvège;f;Marianne;5.2
nl;Pays-Bas;f;Yvonne;3.1
pl;Pologne;f;Katarzyna;16.9
pt;Portugal;f;Ana Maria;8.8
gb;Royaume-Uni;f;Christine;4.2
ro;Roumanie;f;Elena;23.8
ru;Russie;f;Olga;53.6
se;Suède;f;Maria;2.8
ch;Suisse;f;Ruth;5.8
ua;Ukraine;f;Olena;40.7
es;Espagne;f;Carmen;16.6
de;Allemagne;f;Susanne;5.7
be;Belgique;f;Caroline;2.3
bg;Bulgarie;f;Ivanka;7.1
fr;France;f;Isabelle;3.6
ie;Irlande;f;Margaret;5.2
it;Italie;f;Giulia;9.2
lt;Lituanie;f;Jurgita;15.3
md;Moldavie;f;Elena;42.9
no;Norvège;f;Elisabeth;5
nl;Pays-Bas;f;Johanna Maria;2.8
pl;Pologne;f;Agnieszka;16.8
pt;Portugal;f;Maria;7.9
gb;Royaume-Uni;f;Jacqueline;4.1
ro;Roumanie;f;Mariana;17
ru;Russie;f;Natalia;51.9
se;Suède;f;Anna;2.3
ch;Suisse;f;Silvia;5.8
ua;Ukraine;f;Oksana;39.8
es;Espagne;f;Ana Maria;12.1
de;Allemagne;f;Monika;5.5
be;Belgique;f;Laura;2.2
bg;Bulgarie;f;Elena;6.6
fr;France;f;Sophie;3.3
ie;Irlande;f;Margaret Mary;5
it;Italie;f;Sara;9.1
lt;Lituanie;f;Laura;15
md;Moldavie;f;Maria;36.1
no;Norvège;f;Kristin;4.7
nl;Pays-Bas;f;Monique;2.8
pl;Pologne;f;Malgorzata;14.1
pt;Portugal;f;Maria Conceiçao;7.6
gb;Royaume-Uni;f;Margaret;4
ro;Roumanie;f;Mihaela;14.2
ru;Russie;f;Tatiana;46.9
se;Suède;f;Eva Maria;2.3
ch;Suisse;f;Monika;5.4
ua;Ukraine;f;Svitlana;37.4
es;Espagne;f;Maria Pilar;11.9
de;Allemagne;f;Andrea;5.4
be;Belgique;f;Marie Christine;2.2
bg;Bulgarie;f;Desislava;6.6
fr;France;f;Marie;3.2
ie;Irlande;f;Patricia;4.5
it;Italie;f;Laura;8.7
lt;Lituanie;f;Viktorija;14.6
md;Moldavie;f;Ana;26.2
no;Norvège;f;Eva;4.5
nl;Pays-Bas;f;Elisabeth;2.5
pl;Pologne;f;Magdalena;12.6
pt;Portugal;f;Maria Jose;7.1
gb;Royaume-Uni;f;Linda;3.5
ro;Roumanie;f;Ana Maria;13.4
ru;Russie;f;Irina;44.5
se;Suède;f;Eva Christina;1.9
ch;Suisse;f;Marianne;4.9
ua;Ukraine;f;Nataliya;31.4
es;Espagne;f;Laura;11.9
de;Allemagne;f;Petra;5.2
be;Belgique;f;Martine;2.1
bg;Bulgarie;f;Gabriela;5.1
fr;France;f;Stephanie;3.2
ie;Irlande;f;Sarah;4.1
it;Italie;f;Chiara;8.5
lt;Lituanie;f;Daiva;13.8
md;Moldavie;f;Olga;23.8
no;Norvège;f;Nina;4.2
nl;Pays-Bas;f;Maria;2.4
pl;Pologne;f;Anna Maria;10.4
pt;Portugal;f;Ana Paula;6.6
gb;Royaume-Uni;f;Patricia;3.5
ro;Roumanie;f;Daniela;11.2
ru;Russie;f;Anna;36.7
ch;Suisse;f;Elisabeth;4.5
ua;Ukraine;f;Mariya;29
es;Espagne;f;Maria Dolores;11.6
de;Allemagne;f;Nicole;5
be;Belgique;f;Veronique;2
bg;Bulgarie;f;Silviya;5.1
fr;France;f;Anne Marie;2.7
ie;Irlande;f;Deirdre;3.9
it;Italie;f;Martina;8.3
lt;Lituanie;f;Inga;13.6
md;Moldavie;f;Irina;23.4
no;Norvège;f;Marit;4
nl;Pays-Bas;f;Cornelia;2.3
pl;Pologne;f;Joanna;10.4
pt;Portugal;f;Mariana;6.3
gb;Royaume-Uni;f;Carol Ann;3.4
ro;Roumanie;f;Nicoleta;8.5
ru;Russie;f;Ekaterina;36.6
ch;Suisse;f;Barbara;4.3
ua;Ukraine;f;Olha;27.8
es;Espagne;f;Isabel;11.6
de;Allemagne;f;Karin;4.9
be;Belgique;f;Sarah;1.8
bg;Bulgarie;f;Galina;5
fr;France;f;Sylvie;2.6
ie;Irlande;f;Elizabeth;3.5
it;Italie;f;Sofia;7.4
lt;Lituanie;f;Egle;12.6
md;Moldavie;f;Valentina;22.6
no;Norvège;f;Turid;3.6
nl;Pays-Bas;f;Maria Johanna;2.3
pl;Pologne;f;Monika;10.2
pt;Portugal;f;Maria Isabel;5.8
gb;Royaume-Uni;f;Ann;3.1
ro;Roumanie;f;Ana;8
ru;Russie;f;Svetlana;36.6
ch;Suisse;f;Daniela;4.3
ua;Ukraine;f;Tetyana;26.9
es;Espagne;f;Josefa;11.5
de;Allemagne;f;Julia;4.7
be;Belgique;f;Sophie;1.8
bg;Bulgarie;f;Petya;4.9
fr;France;f;Christine;2.6
ie;Irlande;f;Marie;3.5
it;Italie;f;Silvia;7.3
lt;Lituanie;f;Rasa;12.3
md;Moldavie;f;Svetlana;21.1
no;Norvège;f;Anne Lise;3.5
nl;Pays-Bas;f;Marianne;2.1
pl;Pologne;f;Marta;9.4
pt;Portugal;f;Maria Lurdes;5.6
gb;Royaume-Uni;f;Julie;3.1
ro;Roumanie;f;Ioana;7.9
ru;Russie;f;Marina;24.9
ch;Suisse;f;Erika;4.3
ua;Ukraine;f;Halyna;24.8
es;Espagne;f;Maria Teresa;11.3
de;Allemagne;f;Gabriele;4.6
be;Belgique;f;Marie Louise;1.7
bg;Bulgarie;f;Valentina;4.7
fr;France;f;Lea;2.5
ie;Irlande;f;Ann;3.3
it;Italie;f;Daniela;7
lt;Lituanie;f;Jolanta;11.4
md;Moldavie;f;Cristina;19.9
no;Norvège;f;Tove;3.5
nl;Pays-Bas;f;Ingrid;2.1
pl;Pologne;f;Ewa;9
pt;Portugal;f;Ines;5.1
gb;Royaume-Uni;f;Karen;3.1
ro;Roumanie;f;Cristina;7.8
ru;Russie;f;Maria;19.6
ch;Suisse;f;Verena;4.2
ua;Ukraine;f;Tetiana;22.2
dz;Algérie;m;Mohamed;52.7
gm;Gambie;m;Ebrima;38
gq;Guinée équatoriale;m;Miguel Angel;11.7
ml;Mali;m;Mamadou;43.4
ma;Maroc;m;Mohamed;85.3
ng;Nigeria;m;Emmanuel;9.4
sn;Sénégal;m;Mamadou;38.5
dz;Algérie;m;Mohammed;27.7
gm;Gambie;m;Lamin;35.2
gq;Guinée équatoriale;m;Jose Luis;8.3
ml;Mali;m;Moussa;41.3
ma;Maroc;m;Mohammed;43.2
ng;Nigeria;m;John;7.4
sn;Sénégal;m;Ibrahima;32.3
dz;Algérie;m;Abdelkader;27.1
gm;Gambie;m;Musa;34.6
gq;Guinée équatoriale;m;Jose Antonio;8.1
ml;Mali;m;Adama;24.2
ma;Maroc;m;Ahmed;28.9
ng;Nigeria;m;Lucky;6.9
sn;Sénégal;m;Abdoulaye;21
dz;Algérie;m;Ahmed;24.4
gm;Gambie;m;Mahamadou;34.6
gq;Guinée équatoriale;m;Manuel;7.2
ml;Mali;m;Mahamadou;23.1
ma;Maroc;m;Youssef;24.1
ng;Nigeria;m;Kingsley;5.3
sn;Sénégal;m;Ousmane;20.4
dz;Algérie;m;Ali;14.4
gm;Gambie;m;Omar;19.9
gq;Guinée équatoriale;m;Francisco;6.6
ml;Mali;m;Bakary;19.2
ma;Maroc;m;Said;18.8
ng;Nigeria;m;David;5.1
sn;Sénégal;m;Moussa;18.2
dz;Algérie;m;Said;9.3
gm;Gambie;m;Ousman;17.3
gq;Guinée équatoriale;m;Jose Maria;6
ml;Mali;m;Abdoulaye;16.1
ma;Maroc;m;Rachid;18.1
ng;Nigeria;m;Joseph;5.1
sn;Sénégal;m;Cheikh;16.3
dz;Algérie;m;Youcef;9.1
gm;Gambie;m;Abdoulie;13.9
gq;Guinée équatoriale;m;Salvador;5.8
ml;Mali;m;Modibo;13.8
ma;Maroc;m;Mustapha;14.8
ng;Nigeria;m;Samuel;5.1
sn;Sénégal;m;Modou;16.1
dz;Algérie;m;Omar;9.1
gm;Gambie;m;Muhamadou;13.3
gq;Guinée équatoriale;m;Jose;5.5
ml;Mali;m;Makan;13.4
ma;Maroc;m;Adam;13.3
ng;Nigeria;m;Sunday;4.9
sn;Sénégal;m;Moustapha;14.4
dz;Algérie;m;Rachid;9.1
gm;Gambie;m;Bubacarr;12
gq;Guinée équatoriale;m;Juan Carlos;5.5
ml;Mali;m;Oumar;12.9
ma;Maroc;m;Omar;13
ng;Nigeria;m;Peter;4.7
sn;Sénégal;m;Lamine;13.8
dz;Algérie;m;Brahim;8.9
gm;Gambie;m;Amadou;11.7
gq;Guinée équatoriale;m;Juan;5.3
ml;Mali;m;Issa;12.7
ma;Maroc;m;Ayoub;11.2
ng;Nigeria;m;Victor;4.6
sn;Sénégal;m;Amadou;12.8
dz;Algérie;f;Fatima;16.9
gm;Gambie;f;Fatoumata;70.1
gq;Guinée équatoriale;f;Maria Carmen;8.4
ml;Mali;f;Fatoumata;81.8
ma;Maroc;f;Fatima;51
ng;Nigeria;f;Joy;31.5
sn;Sénégal;f;Fatou;45.5
dz;Algérie;f;Amina;13.6
gm;Gambie;f;Mariama;54.7
gq;Guinée équatoriale;f;Esperanza;5.8
ml;Mali;f;Mariam;45
ma;Maroc;f;Khadija;27
ng;Nigeria;f;Blessing;21.5
sn;Sénégal;f;Awa;24.5
dz;Algérie;f;Fatiha;12.8
gm;Gambie;f;Fatou;43.2
gq;Guinée équatoriale;f;Isabel;5.8
ml;Mali;f;Aminata;36.8
ma;Maroc;f;Aicha;15.1
ng;Nigeria;f;Faith;13.5
sn;Sénégal;f;Aminata;23.8
dz;Algérie;f;Meriem;11.8
gm;Gambie;f;Isatou;32.8
gq;Guinée équatoriale;f;Maria Pilar;5.3
ml;Mali;f;Awa;24.3
ma;Maroc;f;Naima;14
ng;Nigeria;f;Sandra;10.1
sn;Sénégal;f;Aissatou;23.1
dz;Algérie;f;Sara;11.7
gm;Gambie;f;Hawa;32.6
gq;Guinée équatoriale;f;Milagrosa;5.3
ml;Mali;f;Oumou;24.3
ma;Maroc;f;Fatiha;12.9
ng;Nigeria;f;Juliet;10
sn;Sénégal;f;Khady;21.7
dz;Algérie;f;Aicha;10.8
gm;Gambie;f;Aminata;23.2
gq;Guinée équatoriale;f;Cristina;5.1
ml;Mali;f;Hawa;23.5
ma;Maroc;f;Nadia;11.6
ng;Nigeria;f;Esther;8.5
sn;Sénégal;f;Mariama;21.1
dz;Algérie;f;Karima;10.4
gm;Gambie;f;Fatima;22.7
gq;Guinée équatoriale;f;Maria Teresa;5
ml;Mali;f;Djeneba;19.4
ma;Maroc;f;Hanane;11.4
ng;Nigeria;f;Mercy;8.4
sn;Sénégal;f;Fatoumata;19.7
dz;Algérie;f;Fatma;9.9
gm;Gambie;f;Bintou;21.3
gq;Guinée équatoriale;f;Pilar;5
ml;Mali;f;Kadiatou;19.2
ma;Maroc;f;Aya;11.3
ng;Nigeria;f;Loveth;7.8
sn;Sénégal;f;Seynabou;12.7
dz;Algérie;f;Fatima Zohra;9.7
gm;Gambie;f;Haja;19.4
gq;Guinée équatoriale;f;Maria Angeles;4.9
ml;Mali;f;Fanta;15.5
ma;Maroc;f;Malika;11
ng;Nigeria;f;Mary;7.3
sn;Sénégal;f;Maimouna;11.7
dz;Algérie;f;Nadia;9.2
gm;Gambie;f;Jainaba;15.2
gq;Guinée équatoriale;f;Catalina;4.5
ml;Mali;f;Maimouna;14.9
ma;Maroc;f;Sara;10.8
ng;Nigeria;f;Patience;6.9
sn;Sénégal;f;Mame Diarra;11.7
ar;Argentine;m;Miguel Angel;6.4
bo;Bolivie;m;Juan Carlos;16.1
br;Brésil;m;Lucas;13.2
cl;Chili;m;Francisco Javier;8.7
co;Colombie;m;Andres Felipe;14.1
cu;Cuba;m;Alejandro;11.7
ec;Équateur;m;Juan Carlos;10.8
us;États-Unis;m;David;4.1
hn;Honduras;m;Juan Carlos;6
mx;Mexique;m;Alejandro;15.4
py;Paraguay;m;Miguel Angel;8.1
pe;Pérou;m;Miguel Angel;10.8
do;République dominicaine;m;Jose Luis;6.4
uy;Uruguay;m;Miguel Angel;8.4
ve;Venezuela;m;Juan Carlos;7.4
ar;Argentine;m;Juan Manuel;6.3
bo;Bolivie;m;Jose Luis;10.7
br;Brésil;m;Gabriel;12
cl;Chili;m;Juan Carlos;7.1
co;Colombie;m;Santiago;11.9
cu;Cuba;m;Jorge Luis;9
ec;Équateur;m;Jose Luis;6.9
us;États-Unis;m;Daniel;3.4
hn;Honduras;m;Miguel Angel;4.4
mx;Mexique;m;Miguel Angel;10.6
py;Paraguay;m;Julio Cesar;7.9
pe;Pérou;m;Juan Carlos;10
do;République dominicaine;m;Miguel Angel;6.2
uy;Uruguay;m;Julio Cesar;7.5
ve;Venezuela;m;Jose Gregorio;7
ar;Argentine;m;Juan Carlos;6.1
bo;Bolivie;m;Miguel Angel;7.7
br;Brésil;m;Rafael;10.1
cl;Chili;m;Marco Antonio;6.5
co;Colombie;m;Juan David;10.5
cu;Cuba;m;Roberto;7.8
ec;Équateur;m;Luis Alberto;5.8
us;États-Unis;m;Michael;3.3
hn;Honduras;m;Jose Luis;4.1
mx;Mexique;m;Daniel;10.3
py;Paraguay;m;Juan Carlos;6.4
pe;Pérou;m;Jose Luis;8.7
do;République dominicaine;m;Juan Carlos;6.1
uy;Uruguay;m;Luis Alberto;6.4
ve;Venezuela;m;Miguel Angel;5.6
ar;Argentine;m;Juan Pablo;5.3
bo;Bolivie;m;Marco Antonio;7.5
br;Brésil;m;Rodrigo;9.2
cl;Chili;m;Miguel Angel;6.4
co;Colombie;m;Juan Carlos;9.5
cu;Cuba;m;Ernesto;7.5
ec;Équateur;m;Miguel Angel;5.7
us;États-Unis;m;Alexander;2.4
hn;Honduras;m;Luis Fernando;3.8
mx;Mexique;m;Fernando;9.7
py;Paraguay;m;Luis Alberto;5.1
pe;Pérou;m;Luis Alberto;8
do;République dominicaine;m;Jose Antonio;5.1
uy;Uruguay;m;Santiago;6.3
ve;Venezuela;m;Carlos Eduardo;5.4
ar;Argentine;m;Carlos Alberto;5.2
bo;Bolivie;m;David;7
br;Brésil;m;Felipe;8.4
cl;Chili;m;Pablo Andres;5
co;Colombie;m;Juan Camilo;9.2
cu;Cuba;m;Juan Carlos;6.9
ec;Équateur;m;Carlos Alberto;3.6
us;États-Unis;m;Alejandro;2.1
hn;Honduras;m;Carlos Alberto;3.6
mx;Mexique;m;Juan Carlos;9.7
py;Paraguay;m;Francisco Javier;4.5
pe;Pérou;m;Marco Antonio;7.2
do;République dominicaine;m;Rafael;5.1
uy;Uruguay;m;Juan Carlos;5.6
ve;Venezuela;m;Jose Luis;4.9
ar;Argentine;m;Nicolas;5
bo;Bolivie;m;Luis Fernando;6.6
br;Brésil;m;Marcelo;8.1
cl;Chili;m;Felipe Andres;4.6
co;Colombie;m;Sebastian;8.8
cu;Cuba;m;Alexander;6.2
ec;Équateur;m;Luis Fernando;3.5
us;États-Unis;m;Christopher;1.7
hn;Honduras;m;Jorge Alberto;3.4
mx;Mexique;m;Diego;9.5
py;Paraguay;m;Carlos Alberto;3.4
pe;Pérou;m;Jorge Luis;7
do;République dominicaine;m;Jose Miguel;4.6
uy;Uruguay;m;Nicolas;5.3
ve;Venezuela;m;Carlos Alberto;4.7
ar;Argentine;m;Juan Ignacio;4.6
bo;Bolivie;m;Fernando;6.1
br;Brésil;m;Daniel;7.8
cl;Chili;m;Juan Pablo;4.5
co;Colombie;m;Miguel Angel;8.4
cu;Cuba;m;Daniel;6.2
ec;Équateur;m;Julio Cesar;3.5
us;États-Unis;m;Richard;1.7
hn;Honduras;m;Julio Cesar;3
mx;Mexique;m;Eduardo;9.4
py;Paraguay;m;Daniel;3.3
pe;Pérou;m;Carlos Alberto;6.3
do;République dominicaine;m;Carlos Manuel;4.6
uy;Uruguay;m;Jose Luis;5.2
ve;Venezuela;m;Jose Antonio;4.4
ar;Argentine;m;Jose Luis;4.5
bo;Bolivie;m;Juan;6
br;Brésil;m;Thiago;7.8
cl;Chili;m;Luis Alberto;4.5
co;Colombie;m;Carlos Andres;8
cu;Cuba;m;Jorge;6.1
ec;Équateur;m;Jorge Luis;3.5
us;États-Unis;m;John;1.6
hn;Honduras;m;Jose Antonio;2.6
mx;Mexique;m;Rodrigo;8.5
py;Paraguay;m;Arnaldo;3.3
pe;Pérou;m;Julio Cesar;5.7
do;République dominicaine;m;Jose Manuel;4
uy;Uruguay;m;Sebastian;5.2
ve;Venezuela;m;Jorge Luis;4
ar;Argentine;m;Santiago;4.2
bo;Bolivie;m;Jorge;5.9
br;Brésil;m;Bruno;7.7
cl;Chili;m;Rodrigo Andres;4
co;Colombie;m;Juan Pablo;7.8
cu;Cuba;m;Alberto;5.7
ec;Équateur;m;Victor Hugo;3.4
us;États-Unis;m;Juan Carlos;1.6
hn;Honduras;m;Marco Antonio;2.5
mx;Mexique;m;Ricardo;8.3
py;Paraguay;m;Francisco;3.3
pe;Pérou;m;Jose Antonio;4.3
do;République dominicaine;m;Alexander;3.9
uy;Uruguay;m;Carlos Alberto;4.6
ve;Venezuela;m;Luis Alberto;4
ar;Argentine;m;Tomas;4.1
bo;Bolivie;m;Daniel;5.8
br;Brésil;m;Guilherme;7.5
cl;Chili;m;Felipe Ignacio;3.9
co;Colombie;m;Juan Sebastian;7.8
cu;Cuba;m;Javier;5.7
ec;Équateur;m;Jose Antonio;3.2
us;États-Unis;m;Sebastian;1.6
hn;Honduras;m;Carlos Roberto;2.4
mx;Mexique;m;Jorge;7.5
py;Paraguay;m;Ruben Dario;3.2
pe;Pérou;m;Luis Enrique;4.3
do;République dominicaine;m;Julio Cesar;3.9
uy;Uruguay;m;Joaquin;4.4
ve;Venezuela;m;Julio Cesar;3.9
ar;Argentine;f;Maria Florencia;5.7
bo;Bolivie;f;Elizabeth;5.6
br;Brésil;f;Juliana;7.5
cl;Chili;f;Maria Jose;7.4
co;Colombie;f;Daniela;8.7
cu;Cuba;f;Yanet;6.4
ec;Équateur;f;Maria Fernanda;5.1
us;États-Unis;f;Maria;2.4
hn;Honduras;f;Karla Patricia;2.5
mx;Mexique;f;Maria Fernanda;10.7
py;Paraguay;f;Graciela;4.2
pe;Pérou;f;Ana Maria;3.8
do;République dominicaine;f;Elizabeth;4.2
uy;Uruguay;f;Ana Maria;7.1
ve;Venezuela;f;Maria Alejandra;4.9
ar;Argentine;f;Maria Belen;5.5
bo;Bolivie;f;Roxana;5.4
br;Brésil;f;Ana Paula;7.1
cl;Chili;f;Carolina Andrea;4.8
co;Colombie;f;Luisa Fernanda;7.9
cu;Cuba;f;Claudia;6
ec;Équateur;f;Maria Jose;4.6
us;États-Unis;f;Elizabeth;2.1
hn;Honduras;f;Maria Carmen;2.4
mx;Mexique;f;Alejandra;10.4
py;Paraguay;f;Maria Elena;3
pe;Pérou;f;Carmen Rosa;3.6
do;République dominicaine;f;Altagracia;3.9
uy;Uruguay;f;Ana Laura;6.2
ve;Venezuela;f;Maria Fernanda;4.6
ar;Argentine;f;Maria Laura;5.2
bo;Bolivie;f;Martha;5.3
br;Brésil;f;Fernanda;6.8
cl;Chili;f;Claudia Andrea;4.6
co;Colombie;f;Valentina;7.9
cu;Cuba;f;Dayana;4.3
ec;Équateur;f;Maria Carmen;2.9
us;États-Unis;f;Sarah Elizabeth;1.8
hn;Honduras;f;Maria Elena;1.8
mx;Mexique;f;Mariana;10
py;Paraguay;f;Maria Cristina;3
pe;Pérou;f;Maria Elena;3
do;République dominicaine;f;Maria;3.6
uy;Uruguay;f;Maria Cristina;5.5
ve;Venezuela;f;Maria Gabriela;4.5
ar;Argentine;f;Sofia;5.1
bo;Bolivie;f;Patricia;4.9
br;Brésil;f;Adriana;6.5
cl;Chili;f;Ana Maria;4.4
co;Colombie;f;Sandra Milena;7.3
cu;Cuba;f;Elizabeth;4.3
ec;Équateur;f;Ana Lucia;2.9
us;États-Unis;f;Jennifer;1.7
hn;Honduras;f;Maria Jose;1.7
mx;Mexique;f;Andrea;8.2
py;Paraguay;f;Zunilda;2.9
pe;Pérou;f;Flor Maria;2.7
do;République dominicaine;f;Maria Altagracia;3.5
uy;Uruguay;f;Maria Jose;5.4
ve;Venezuela;f;Maria Jose;3.6
ar;Argentine;f;Maria Eugenia;5
bo;Bolivie;f;Daniela;4.8
br;Brésil;f;Patricia;6.4
cl;Chili;f;Maria Angelica;4.1
co;Colombie;f;Paola Andrea;7
cu;Cuba;f;Lisandra;4
ec;Équateur;f;Maria Isabel;2.6
us;États-Unis;f;Jessica;1.6
hn;Honduras;f;Ana Maria;1.6
mx;Mexique;f;Daniela;7.8
py;Paraguay;f;Carmen;2.9
pe;Pérou;f;Elizabeth;2.5
do;République dominicaine;f;Juana;3.3
uy;Uruguay;f;Valentina;5.1
ve;Venezuela;f;Maria Eugenia;2.9
ar;Argentine;f;Maria Victoria;4.4
bo;Bolivie;f;Ana Maria;4.6
br;Brésil;f;Luciana;5.6
cl;Chili;f;Paula Andrea;3.7
co;Colombie;f;Carolina;6.7
cu;Cuba;f;Laura;4
ec;Équateur;f;Diana Carolina;2.4
us;États-Unis;f;Ana Maria;1.4
hn;Honduras;f;Reina Isabel;1.6
mx;Mexique;f;Gabriela;7.6
py;Paraguay;f;Alicia;2.7
pe;Pérou;f;Maria Isabel;2.3
do;République dominicaine;f;Carolina;2.6
uy;Uruguay;f;Florencia;5
ve;Venezuela;f;Maria Angeles;2.8
ar;Argentine;f;Agustina;4.1
bo;Bolivie;f;Gabriela;4.5
br;Brésil;f;Maria Aparecida;4.8
cl;Chili;f;Maria Isabel;3.6
co;Colombie;f;Paula Andrea;6.6
cu;Cuba;f;Yadira;3.9
ec;Équateur;f;Maria Elena;2.4
us;États-Unis;f;Andrea;1.4
hn;Honduras;f;Maria Fernanda;1.5
mx;Mexique;f;Maria Guadalupe;6.8
py;Paraguay;f;Francisca;2.7
pe;Pérou;f;Maria Carmen;2.1
do;République dominicaine;f;Francisca;2.5
uy;Uruguay;f;Maria Carmen;4.9
ve;Venezuela;f;Ana Maria;2.3
ar;Argentine;f;Florencia;3.6
bo;Bolivie;f;Sonia;4.4
br;Brésil;f;Aline;4.5
cl;Chili;f;Maria Fernanda;3.2
co;Colombie;f;Diana Marcela;6
cu;Cuba;f;Beatriz;3.5
ec;Équateur;f;Maria Belen;2.3
us;États-Unis;f;Patricia;1.4
hn;Honduras;f;Maria Magdalena;1.4
mx;Mexique;f;Adriana;6.3
py;Paraguay;f;Lucia;2.5
pe;Pérou;f;Maria Fernanda;1.9
do;République dominicaine;f;Alexandra;2.5
uy;Uruguay;f;Maria Alejandra;4.7
ve;Venezuela;f;Maria Elena;1.9
ar;Argentine;f;Maria Fernanda;3.6
bo;Bolivie;f;Juana;4
br;Brésil;f;Camila;4.4
cl;Chili;f;Maria Ignacia;3.2
co;Colombie;f;Maria Camila;5.9
cu;Cuba;f;Yaima;3.3
ec;Équateur;f;Ana Maria;2.3
us;États-Unis;f;Sofia;1.4
hn;Honduras;f;Alba Luz;1.3
mx;Mexique;f;Maria Carmen;5.8
py;Paraguay;f;Carolina;2.5
pe;Pérou;f;Maria Pilar;1.9
do;République dominicaine;f;Mercedes;2.5
uy;Uruguay;f;Maria Victoria;4.4
ve;Venezuela;f;Maria Teresa;1.8
ar;Argentine;f;Martina;3.4
bo;Bolivie;f;Sandra;3.9
br;Brésil;f;Renata;4.4
cl;Chili;f;Maria Soledad;2.9
co;Colombie;f;Ana Maria;5.5
cu;Cuba;f;Daniela;3.1
ec;Équateur;f;Maria Mercedes;2
us;États-Unis;f;Gabriela;1.4
hn;Honduras;f;Maria Isabel;1.2
mx;Mexique;f;Sofia;5.8
py;Paraguay;f;Mercedes;2.4
pe;Pérou;f;Rosa Maria;1.9
do;République dominicaine;f;Ramona;2.5
uy;Uruguay;f;Camila;4.2
ve;Venezuela;f;Maria Victoria;1.8
cn;Chine;m;Wei;6.4
ph;Philippines;m;Michael;7.1
in;Inde;m;Gurpreet;12.6
pk;Pakistan;m;Muhammad;114.8
cn;Chine;m;Hao;5.4
ph;Philippines;m;Joel;6
in;Inde;m;Sukhwinder;7.5
pk;Pakistan;m;Ali;23.8
cn;Chine;m;Jie;5.1
ph;Philippines;m;Edwin;5.2
in;Inde;m;Sandeep;6.6
pk;Pakistan;m;Mohammad;18.8
cn;Chine;m;Yi;4.4
ph;Philippines;m;Joseph;4.9
in;Inde;m;Harpreet;6.5
pk;Pakistan;m;Abdul;13.2
cn;Chine;m;Jun;4.2
ph;Philippines;m;Reynaldo;4.9
in;Inde;m;Jaswinder;6.4
pk;Pakistan;m;Imran;10
cn;Chine;m;Feng;3.6
ph;Philippines;m;Jeffrey;4.4
in;Inde;m;Balwinder;5.7
pk;Pakistan;m;Usman;8.1
cn;Chine;m;Jiahao;3.6
ph;Philippines;m;Rolando;4.4
in;Inde;m;Manpreet;5.7
pk;Pakistan;m;Ahmed;7.8
cn;Chine;m;Yong;3.4
ph;Philippines;m;Jonathan;4.2
in;Inde;m;Ranjit;5.2
pk;Pakistan;m;Shahid;7
cn;Chine;m;Jian;3
ph;Philippines;m;Richard;4.2
in;Inde;m;Mandeep;5.1
pk;Pakistan;m;Faisal;6.6
cn;Chine;m;Yu;3
ph;Philippines;m;Allan;3.8
in;Inde;m;Lakhwinder;4.8
pk;Pakistan;m;Hassan;6.5
cn;Chine;f;Jing;6.7
ph;Philippines;f;Michelle;5.5
in;Inde;f;Amandeep;11.4
pk;Pakistan;f;Ayesha;14.9
cn;Chine;f;Yan;6.4
ph;Philippines;f;Jennifer;4.9
in;Inde;f;Manpreet;11.1
pk;Pakistan;f;Fatima;14.5
cn;Chine;f;Ying;6.3
ph;Philippines;f;Rowena;4.5
in;Inde;f;Mandeep;10.7
pk;Pakistan;f;Saima;10.2
cn;Chine;f;Xinyi;5.5
ph;Philippines;f;Josephine;4.3
in;Inde;f;Sandeep;9.1
pk;Pakistan;f;Maryam;10
cn;Chine;f;Li;4.8
ph;Philippines;f;Maricel;4.3
in;Inde;f;Rajwinder;8.2
pk;Pakistan;f;Sadia;9.3
cn;Chine;f;Jie;3.7
ph;Philippines;f;Jocelyn;4.2
in;Inde;f;Harpreet;7.5
pk;Pakistan;f;Shazia;9
cn;Chine;f;Jiayi;3.7
ph;Philippines;f;Evelyn;3.9
in;Inde;f;Manjit;7.4
pk;Pakistan;f;Bushra;9
cn;Chine;f;Xiaoyan;3.6
ph;Philippines;f;Marilyn;3.9
in;Inde;f;Kulwinder;6.3
pk;Pakistan;f;Zainab;8.1
cn;Chine;f;Lili;3.4
ph;Philippines;f;Mary Jane;3.7
in;Inde;f;Paramjit;6.1
pk;Pakistan;f;Nazia;7.8
cn;Chine;f;Tingting;3.2
ph;Philippines;f;Mary Ann;3.6
in;Inde;f;Jaswinder;6
pk;Pakistan;f;Saba;7.4"""

let prenoms: PrenomPays list =
    prenomsTexte.Split('\n')
    |> Array.map (fun l ->
        let c = l.Split(';')
        { pays = c[0]; nomPays = c[1]; sexe = c[2]; prenom = c[3]; poids = float c[4] })
    |> List.ofArray

let joueurs : PrenomJoueur list = []

/// Les pays du monde (code ISO, nom anglais) : la page en affiche le nom dans la langue du joueur.
let paysDuMonde : (string * string) list =
    [ "ad", "Andorra"
      "ae", "United Arab Emirates"
      "af", "Afghanistan"
      "ag", "Antigua & Barbuda"
      "ai", "Anguilla"
      "al", "Albania"
      "am", "Armenia"
      "ao", "Angola"
      "aq", "Antarctica"
      "ar", "Argentina"
      "as", "American Samoa"
      "at", "Austria"
      "au", "Australia"
      "aw", "Aruba"
      "ax", "Åland Islands"
      "az", "Azerbaijan"
      "ba", "Bosnia & Herzegovina"
      "bb", "Barbados"
      "bd", "Bangladesh"
      "be", "Belgium"
      "bf", "Burkina Faso"
      "bg", "Bulgaria"
      "bh", "Bahrain"
      "bi", "Burundi"
      "bj", "Benin"
      "bl", "St. Barthélemy"
      "bm", "Bermuda"
      "bn", "Brunei"
      "bo", "Bolivia"
      "bq", "Caribbean Netherlands"
      "br", "Brazil"
      "bs", "Bahamas"
      "bt", "Bhutan"
      "bv", "Bouvet Island"
      "bw", "Botswana"
      "by", "Belarus"
      "bz", "Belize"
      "ca", "Canada"
      "cc", "Cocos (Keeling) Islands"
      "cd", "Congo - Kinshasa"
      "cf", "Central African Republic"
      "cg", "Congo - Brazzaville"
      "ch", "Switzerland"
      "ci", "Côte d’Ivoire"
      "ck", "Cook Islands"
      "cl", "Chile"
      "cm", "Cameroon"
      "cn", "China mainland"
      "co", "Colombia"
      "cr", "Costa Rica"
      "cs", "Serbia"
      "cu", "Cuba"
      "cv", "Cape Verde"
      "cw", "Curaçao"
      "cx", "Christmas Island"
      "cy", "Cyprus"
      "cz", "Czechia"
      "de", "Germany"
      "dj", "Djibouti"
      "dk", "Denmark"
      "dm", "Dominica"
      "do", "Dominican Republic"
      "dz", "Algeria"
      "ec", "Ecuador"
      "ee", "Estonia"
      "eg", "Egypt"
      "er", "Eritrea"
      "es", "Spain"
      "et", "Ethiopia"
      "fi", "Finland"
      "fj", "Fiji"
      "fk", "Falkland Islands"
      "fm", "Micronesia"
      "fo", "Faroe Islands"
      "fr", "France"
      "ga", "Gabon"
      "gb", "United Kingdom"
      "gd", "Grenada"
      "ge", "Georgia"
      "gf", "French Guiana"
      "gg", "Guernsey"
      "gh", "Ghana"
      "gi", "Gibraltar"
      "gl", "Greenland"
      "gm", "Gambia"
      "gn", "Guinea"
      "gp", "Guadeloupe"
      "gq", "Equatorial Guinea"
      "gr", "Greece"
      "gs", "So. Georgia & So. Sandwich Isl."
      "gt", "Guatemala"
      "gu", "Guam"
      "gw", "Guinea-Bissau"
      "gy", "Guyana"
      "hk", "Hong Kong"
      "hm", "Heard & McDonald Islands"
      "hn", "Honduras"
      "hr", "Croatia"
      "ht", "Haiti"
      "hu", "Hungary"
      "id", "Indonesia"
      "ie", "Ireland"
      "il", "Israel"
      "im", "Isle of Man"
      "in", "India"
      "io", "Chagos Archipelago"
      "iq", "Iraq"
      "ir", "Iran"
      "is", "Iceland"
      "it", "Italy"
      "iv", "Invariant Country"
      "je", "Jersey"
      "jm", "Jamaica"
      "jo", "Jordan"
      "jp", "Japan"
      "ke", "Kenya"
      "kg", "Kyrgyzstan"
      "kh", "Cambodia"
      "ki", "Kiribati"
      "km", "Comoros"
      "kn", "St. Kitts & Nevis"
      "kp", "North Korea"
      "kr", "South Korea"
      "kw", "Kuwait"
      "ky", "Cayman Islands"
      "kz", "Kazakhstan"
      "la", "Laos"
      "lb", "Lebanon"
      "lc", "St. Lucia"
      "li", "Liechtenstein"
      "lk", "Sri Lanka"
      "lr", "Liberia"
      "ls", "Lesotho"
      "lt", "Lithuania"
      "lu", "Luxembourg"
      "lv", "Latvia"
      "ly", "Libya"
      "ma", "Morocco"
      "mc", "Monaco"
      "md", "Moldova"
      "me", "Montenegro"
      "mf", "St. Martin"
      "mg", "Madagascar"
      "mh", "Marshall Islands"
      "mk", "North Macedonia"
      "ml", "Mali"
      "mm", "Myanmar (Burma)"
      "mn", "Mongolia"
      "mo", "Macao"
      "mp", "Northern Mariana Islands"
      "mq", "Martinique"
      "mr", "Mauritania"
      "ms", "Montserrat"
      "mt", "Malta"
      "mu", "Mauritius"
      "mv", "Maldives"
      "mw", "Malawi"
      "mx", "Mexico"
      "my", "Malaysia"
      "mz", "Mozambique"
      "na", "Namibia"
      "nc", "New Caledonia"
      "ne", "Niger"
      "nf", "Norfolk Island"
      "ng", "Nigeria"
      "ni", "Nicaragua"
      "nl", "Netherlands"
      "no", "Norway"
      "np", "Nepal"
      "nr", "Nauru"
      "nu", "Niue"
      "nz", "New Zealand"
      "om", "Oman"
      "pa", "Panama"
      "pe", "Peru"
      "pf", "French Polynesia"
      "pg", "Papua New Guinea"
      "ph", "Philippines"
      "pk", "Pakistan"
      "pl", "Poland"
      "pm", "St. Pierre & Miquelon"
      "pn", "Pitcairn Islands"
      "pr", "Puerto Rico"
      "ps", "Palestinian Territories"
      "pt", "Portugal"
      "pw", "Palau"
      "py", "Paraguay"
      "qa", "Qatar"
      "re", "Réunion"
      "ro", "Romania"
      "rs", "Serbia"
      "ru", "Russia"
      "rw", "Rwanda"
      "sa", "Saudi Arabia"
      "sb", "Solomon Islands"
      "sc", "Seychelles"
      "sd", "Sudan"
      "se", "Sweden"
      "sg", "Singapore"
      "sh", "St. Helena"
      "si", "Slovenia"
      "sj", "Svalbard & Jan Mayen"
      "sk", "Slovakia"
      "sl", "Sierra Leone"
      "sm", "San Marino"
      "sn", "Senegal"
      "so", "Somalia"
      "sr", "Suriname"
      "ss", "South Sudan"
      "st", "São Tomé & Príncipe"
      "sv", "El Salvador"
      "sx", "Sint Maarten"
      "sy", "Syria"
      "sz", "Eswatini"
      "tc", "Turks & Caicos Islands"
      "td", "Chad"
      "tf", "French Southern Territories"
      "tg", "Togo"
      "th", "Thailand"
      "tj", "Tajikistan"
      "tk", "Tokelau"
      "tl", "Timor-Leste"
      "tm", "Turkmenistan"
      "tn", "Tunisia"
      "to", "Tonga"
      "tr", "Türkiye"
      "tt", "Trinidad & Tobago"
      "tv", "Tuvalu"
      "tw", "Taiwan"
      "tz", "Tanzania"
      "ua", "Ukraine"
      "ug", "Uganda"
      "um", "U.S. Outlying Islands"
      "us", "United States"
      "uy", "Uruguay"
      "uz", "Uzbekistan"
      "va", "Vatican City"
      "vc", "St. Vincent & Grenadines"
      "ve", "Venezuela"
      "vg", "British Virgin Islands"
      "vi", "U.S. Virgin Islands"
      "vn", "Vietnam"
      "vu", "Vanuatu"
      "wf", "Wallis & Futuna"
      "ws", "Samoa"
      "xk", "Kosovo"
      "ye", "Yemen"
      "yt", "Mayotte"
      "za", "South Africa"
      "zm", "Zambia"
      "zw", "Zimbabwe" ]
