/// TEMPORAIRE : les tables et les constantes de l'ancien moteur que le calcul consomme et qui ne
/// sont pas encore dans data/. Recopiées à l'identique pour reproduire l'oracle ; chacune est à
/// sortir en CSV (étape « retirer le hardcoding » de docs/refonte.md). Les constantes propres à
/// la France iront sous data/pays/fr/.
module Labo.Engine.Tables

/// Valeurs par défaut des curseurs (ancien `SDEF`).
let curseursParDefaut =
    Map
        [ "loss", 60.0
          "need", 0.31
          "yield", 0.0
          "supply", 776.0
          "mwh", 22.1
          "cell", 50.0
          "del", 30.0
          "pTirage", 50.0
          "pMandat", 12.0
          "pRevoc", 20.0
          "pMaj", 50.0
          "pQuorum", 30.0
          "pFreq", 2.0
          "pDom", 3.0
          "prDep", 8.0
          "prConf", 95.0
          "prAss", 8.0
          "prSocle", 99.0
          "prDur", 3.0 ]

/// L'état de départ du jeu (ancien `defaultState`).
let etatDeDepart: Etat =
    { c = Map.empty
      s = curseursParDefaut |> Map.add "yield" 25.0 |> Map.add "accD" 12.0
      mp = { ecart = 3.0; fonte = 2.0; plafond = 10.0; socle = 3.0 }
      p = "om"
      r = true }

// ---------- France ----------

/// Population : France entière (outre-mer compris) ou métropole.
let population0 perimetre = if perimetre = "om" then 69.7e6 else 66.85e6
/// Population projetée par l'Insee, en millions, aujourd'hui, dans 10 ans, dans 30 ans.
let populationHorizon = [| 69.1; 69.8; 67.55 |]
let anneesHorizon = [| 0; 10; 30 |]
/// Écart de revenus entre les 10 % les plus aisés et les 10 % les plus modestes (Insee).
let ecartD9D1 = 3.49
/// Énergie disponible aujourd'hui (TWh), service de référence.
let energieAujourdhui = 1496.0
/// Ammoniac par électrolyse : électricité retirée (TWh / an).
let nh3Twh = 28.0
/// Coordination par une IA lourde : électricité retirée (TWh / an).
let coordIaTwh = 5.0
/// Budget des prisons et de la police (Md€, 2025).
let budgetPolicePrisons = 29.5

// ---------- Stress test (ancien `STRESS`) : clé, verdict de départ ----------

let stress =
    [ Some "impulsif", "tient"
      Some "dissu", "fissure"
      Some "persist", "fissure"
      Some "foule", "tient"
      Some "cache", "fissure"
      Some "arme", "casse"
      None, "ouvert"
      Some "deleg", "tient"
      Some "rare", "tient" ]

// ---------- Test de fermeture (anciens `PER`, `COVER`, `RESROW`, `RES`) ----------

/// Rang d'une couverture : manquant / non vital, partiel, importé / couvert.
let rangCouverture =
    function
    | "manq"
    | "nv" -> 0
    | "part" -> 1
    | "imp"
    | "ok" -> 2
    | autre -> failwith $"couverture inconnue {autre}"

/// Ressource, couverture en métropole, couverture France entière.
let ressources =
    [ "Blé et céréales", "part", "part"
      "Légumes et fruits tempérés", "part", "part"
      "Protéines pour l’élevage", "part", "part"
      "Électricité", "part", "part"
      "Fruits tropicaux et riz", "manq", "part"
      "Engrais", "manq", "manq"
      "Pétrole et gaz", "manq", "manq"
      "Métaux critiques", "manq", "part"
      "Médicaments", "manq", "manq"
      "Eau, terres, forêt", "nv", "nv" ]

/// Ressources couvertes par l'import à chaque niveau d'ouverture.
let couvertParOuverture =
    Map
        [ 1, [ "Médicaments"; "Métaux critiques"; "Électricité" ]
          2, [ "Engrais"; "Pétrole et gaz"; "Fruits tropicaux et riz"; "Protéines pour l’élevage"; "Blé et céréales" ]
          3, [ "Légumes et fruits tempérés" ] ]

/// Ressource → table des ressources qui manquent qui la règle.
let ressourceReglee =
    Map [ "Engrais", "engrais"; "Pétrole et gaz", "energie"; "Médicaments", "medic" ]

/// Couverture obtenue par chaque option de la table des ressources qui manquent.
let couvertureOption =
    Map
        [ "engrais", [| "part"; "manq"; "part" |]
          "energie", [| "part"; "part"; "manq" |]
          "medic", [| "part"; "manq"; "manq" |] ]

// ---------- Échanges (anciens `IMPV`, `EXPO`) ----------

/// Importations à chaque niveau d'ouverture (Md€).
let importations = Map [ 1, [ 5.0; 3.0; 1.0 ]; 2, [ 4.0; 15.0; 4.0 ] ]
/// Ouverture complète : importations et exportations d'aujourd'hui (Md€, 2024).
let importationsAujourdhui, exportationsAujourdhui = 698.9, 617.9
/// Ce qu'on peut offrir en échange, dans l'ordre de l'ancien moteur.
let exportations = [ "elec"; "food"; "vin"; "savoir"; "accueil"; "bois"; "industrie" ]
let exportationsParDefaut = set [ "elec"; "food" ]

// ---------- Paris (ancien `BETS`) ----------

type Pari =
    { id: string
      /// maturité : 0 éprouvé, 1 petite échelle, 2 en développement, 3 expérimental
      maturite: int
      /// années avant d'être prêt
      pret: int
      confort: float
      energie: float
      rendement: float
      besoin: float
      captation: float
      couvre: (string * string) list
      /// ne joue qu'avec une monnaie (fondante ou plafonnée)
      monnaie: bool }

let pari id maturite pret =
    { id = id
      maturite = maturite
      pret = pret
      confort = 0.0
      energie = 0.0
      rendement = 0.0
      besoin = 0.0
      captation = 0.0
      couvre = []
      monnaie = false }

let paris =
    [ { pari "iso" 0 10 with confort = 0.12 }
      { pari "pac" 0 10 with confort = 0.08 }
      { pari "inertie" 0 30 with confort = 0.03 }
      { pari "lowtech" 0 0 with confort = 0.05 }
      { pari "autonome" 1 10 with confort = 0.02 }
      pari "volant" 0 0
      { pari "chaleur" 1 10 with confort = 0.03 }
      { pari "hydro" 1 10 with energie = 11.0 }
      { pari "geo" 0 10 with energie = 8.0 }
      { pari "houle" 3 30 with energie = 3.0 }
      { pari "biogaz" 0 10 with couvre = [ "Pétrole et gaz", "part" ] }
      { pari "efuel" 2 30 with energie = -50.0; couvre = [ "Pétrole et gaz", "part" ] }
      { pari "sodium" 1 10 with couvre = [ "Métaux critiques", "part" ] }
      { pari "rnr" 3 30 with couvre = [ "Électricité", "ok" ] }
      { pari "lithium" 2 10 with couvre = [ "Métaux critiques", "part" ] }
      { pari "recyclage" 2 10 with couvre = [ "Métaux critiques", "part" ] }
      { pari "urine" 1 10 with rendement = 8.0; couvre = [ "Engrais", "part" ] }
      { pari "struvite" 2 10 with rendement = 2.0; couvre = [ "Engrais", "part" ] }
      { pari "bacteries" 3 30 with rendement = 5.0 }
      { pari "riz" 0 0 with couvre = [ "Fruits tropicaux et riz", "part" ] }
      { pari "agrofor" 2 10 with besoin = 0.02; couvre = [ "Protéines pour l’élevage", "part" ] }
      pari "voile" 1 10
      { pari "pharma" 2 10 with couvre = [ "Médicaments", "part" ] }
      pari "atelier" 0 0
      { pari "quota" 0 0 with confort = 0.06; captation = -3.0 }
      { pari "jets" 0 0 with captation = -2.0; confort = 0.01 }
      { pari "prix" 0 0 with confort = 0.03; monnaie = true } ]

// ---------- Passage à l'échelle (ancien `SCALE`) ----------

type Automatique =
    | MonnaiePlafonnee
    /// monnaie fondante ou plafonnée, avec un écart de revenus d'au moins 2
    | MonnaieAvecEcart
    | ProtocoleAdopte

type Parade =
    { id: string
      adoptee: bool
      captation: float
      auto: Automatique option }

let parade id =
    { id = id
      adoptee = false
      captation = 0.0
      auto = None }

/// Les cinq faiblesses du passage à l'échelle et leurs parades.
let faiblesses =
    [ [ { parade "dunbar" with adoptee = true }; parade "communs"; parade "parrain" ]
      [ { parade "tirage" with adoptee = true }
        { parade "imperatif" with captation = -3.0 }
        { parade "controle" with captation = -3.0 }
        { parade "rotation" with captation = -2.0 } ]
      [ { parade "peu" with adoptee = true }
        parade "planif"
        { parade "monplaf" with auto = Some MonnaiePlafonnee } ]
      [ { parade "socle" with adoptee = true }
        { parade "formation" with adoptee = true }
        { parade "ecart" with auto = Some MonnaieAvecEcart } ]
      [ parade "assembler"; { parade "proto" with auto = Some ProtocoleAdopte } ] ]

// ---------- Prévention du gang (ancien `PREV`) : champ de l'état, facteur de risque ----------

let prevention = [ "prev_ete", 0.85; "prev_lieux", 0.92; "prev_legal", 0.70 ]
