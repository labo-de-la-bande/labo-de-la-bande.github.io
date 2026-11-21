/// Éteindre une lentille retire ses liens, et seulement eux.
module Labo.Tests.LentillesTests

open Expecto
open Labo.Engine

let tests =
    testList
        "lentilles"
        [ test "éteindre l'ordre spontané retire le lien de Hayek sur la coordination à la main, et lui seul" {
              let avec = { Tables.etatDeDepart with c = Map [ "coord", Num 0.0 ] }
              let sans = { avec with c = avec.c |> Map.add "lentilles" (Obj(Map [ "ordre-spontane", Bool false ])) }
              let ids e = (Moteur.evaluer e).liens |> List.map _.lien.id |> Set.ofList
              let retires = Set.difference (ids avec) (ids sans)
              Expect.equal retires (set [ "coord.main.niveau-de-vie" ]) "liens retirés"
              Expect.isEmpty (Set.difference (ids sans) (ids avec)) "aucun lien ajouté"
          }
          test "par défaut, toutes les lentilles sont actives" {
              let e = { Tables.etatDeDepart with c = Map [ "coord", Num 0.0 ] }
              let tout = { e with c = e.c |> Map.add "lentilles" (Obj(Map [ "ordre-spontane", Bool true ])) }
              Expect.equal (Moteur.evaluer e).score (Moteur.evaluer tout).score "même score"
          } ]
