/// L'oracle : ce que le moteur JavaScript de l'ancien `index.html` répond, figé dans `oracle/`.
/// Le moteur F# doit reproduire ces résultats. Temporaire : disparaît avec l'ancien moteur.
///
/// On injecte un script dans une copie de la page, on la charge dans Chrome sans écran et on lit
/// le résultat dans le DOM. Le script tire des états au hasard avec une graine fixe (le même
/// générateur que l'ancien golden.py) et enregistre, pour chacun, **l'état d'entrée** et les
/// sorties : score, fidélité, légitimité, viabilité, chaque jauge, chaque lien actif.
module Labo.Cli.Oracle

open System
open System.IO
open System.Text.RegularExpressions
open CliWrap
open CliWrap.Buffered

type Lot =
    { nom: string
      graine: int
      etats: int
      /// probabilité de laisser un levier à sa valeur par défaut
      garde: float }

let lots =
    [ { nom = "golden"; graine = 12345; etats = 300; garde = 0.5 }
      { nom = "large"; graine = 2026; etats = 1000; garde = 0.4 } ]

/// Script injecté dans la page. `__GRAINE__`, `__ETATS__` et `__GARDE__` sont remplacés.
let script =
    """<script>setTimeout(()=>{const o=document.createElement('pre');o.id='ORACLE';try{
let seed=__GRAINE__; const rnd=()=>{seed=(seed*1103515245+12345)%2147483648; return seed/2147483648};
const L=window.LABO, J=window.LABO_JEU, ks=Object.keys(J.lev).sort(), res=[];
const r6=x=>Math.round(x*1e6)/1e6, gv=g=>g.band?[r6(g.band[0]),r6(g.band[1])]:r6(g.v);
for(let n=0;n<__ETATS__;n++){const st=L.defaultState();
 if(n>0) for(const k of ks){const lv=J.lev[k]; if(rnd()<__GARDE__) continue;
  if(lv.range){const r=lv.range, steps=Math.round((r.max-r.min)/r.step); st.s[r.id]=r.min+Math.floor(rnd()*(steps+1))*r.step}
  else {const os=lv.opts(); os[Math.floor(rnd()*os.length)].ap(st)}}
 const etat=JSON.parse(JSON.stringify(st)), e=L.evaluate(st);
 res.push({etat,SC:r6(e.SC),FI:r6(e.FI),LG:r6(e.LG),VI:e.VI.ok,G:e.G.map(g=>[g.l,gv(g)]),
  LK:Object.values(L.linksFor(st)).flat().map(l=>[l.i,r6(l.d)]).sort((a,b)=>a[0]-b[0]||a[1]-b[1])})}
o.textContent=JSON.stringify({liens:L.LINKS.map((l,i)=>[i,l.g,l.d,l.w]),resultats:res})}catch(e){o.textContent='ERR '+e.stack}
document.documentElement.appendChild(o)},1500)</script>"""

/// Chrome : la variable d'environnement LABO_CHROME, sinon les emplacements habituels.
let chrome () =
    let candidats =
        [ Environment.GetEnvironmentVariable "LABO_CHROME"
          "/Applications/Google Chrome.app/Contents/MacOS/Google Chrome"
          "/Applications/Chromium.app/Contents/MacOS/Chromium"
          "/usr/bin/google-chrome"
          "/usr/bin/chromium"
          "/usr/bin/chromium-browser" ]

    candidats
    |> List.tryFind (fun c -> not (String.IsNullOrEmpty c) && File.Exists c)

type Erreur =
    | ChromeIntrouvable
    | PageIntrouvable of string
    | ResultatAbsent of sortie: string
    | ErreurDuMoteur of pile: string

let decrire =
    function
    | ChromeIntrouvable -> "Chrome introuvable : définir LABO_CHROME avec le chemin de l'exécutable."
    | PageIntrouvable p -> $"Page introuvable : {p}"
    | ResultatAbsent s -> $"Pas de résultat dans la page rendue. Début de la sortie :\n{s}"
    | ErreurDuMoteur pile -> $"Le moteur a levé une erreur :\n{pile}"

let capturer (page: string) (lot: Lot) =
    task {
        match chrome () with
        | None -> return Error ChromeIntrouvable
        | Some exe when not (File.Exists page) -> return Error(PageIntrouvable page)
        | Some exe ->
            let html = File.ReadAllText page

            let js =
                script
                    .Replace("__GRAINE__", string lot.graine)
                    .Replace("__ETATS__", string lot.etats)
                    .Replace("__GARDE__", lot.garde.ToString(Globalization.CultureInfo.InvariantCulture))

            let i = html.LastIndexOf "</body>"
            let injecte = if i >= 0 then html.Insert(i, js) else html + js
            let tmp = Directory.CreateTempSubdirectory "labo-oracle-"
            let copie = Path.Combine(tmp.FullName, "page.html")
            File.WriteAllText(copie, injecte)

            let! r =
                Cli
                    .Wrap(exe)
                    .WithArguments(
                        [ "--headless"
                          "--disable-gpu"
                          "--user-data-dir=" + Path.Combine(tmp.FullName, "profil")
                          "--virtual-time-budget=60000"
                          "--dump-dom"
                          $"file://{copie}" ]
                    )
                    .WithValidation(CommandResultValidation.None)
                    .ExecuteBufferedAsync()

            try tmp.Delete true with _ -> ()

            let m = Regex.Match(r.StandardOutput, "<pre id=\"ORACLE\">(.*?)</pre>", RegexOptions.Singleline)

            if not m.Success then
                return Error(ResultatAbsent(r.StandardOutput.Substring(0, min 2000 r.StandardOutput.Length)))
            else
                let txt = Net.WebUtility.HtmlDecode m.Groups[1].Value

                if txt.StartsWith "ERR" then
                    return Error(ErreurDuMoteur txt)
                else
                    return Ok txt
    }

/// `labo oracle capture [page.html]` : écrit oracle/<lot>.json pour chaque lot.
let capturerTout (page: string) =
    task {
        let mutable code = 0

        for lot in lots do
            match! capturer page lot with
            | Ok json ->
                let sortie = Repo.path [ "oracle"; $"{lot.nom}.json" ]
                Repo.writeText sortie (json + "\n")
                use doc = Text.Json.JsonDocument.Parse json
                let res = doc.RootElement.GetProperty "resultats"
                let viables = res.EnumerateArray() |> Seq.filter (fun e -> e.GetProperty("VI").GetBoolean()) |> Seq.length
                printfn $"{Repo.rel sortie} : {res.GetArrayLength()} états, dont {viables} viables"
            | Error e ->
                eprintfn $"{lot.nom} : {decrire e}"
                code <- 1

        return code
    }
