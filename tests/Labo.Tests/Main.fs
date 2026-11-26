module Labo.Tests.Main

open Expecto

[<Tests>]
let tous =
    testList "labo" [ CsvTests.tests; DslTests.tests; DataTests.tests; OracleTests.tests; LentillesTests.tests; JeuTests.tests; JeuTests.testsIssue; JeuTests.testsConseilsSuivis; TempsTests.tests; SceneTests.tests; SceneTests.testsSuite; SceneTests.testsAttitudes; SceneTests.testsCompletude; RevirementTests.tests; PrenomsTests.tests; JournalTests.tests; JournalTests.testsRegistre; JournalTests.testsHistoires; JournalTests.testsRessenti; JournalTests.testsSensibilite; JournalTests.testsConfianceRegard; JournalTests.testsVieAvance; JournalTests.testsRoles; JournalTests.testsParis; PrenomsTests.testsRegions; RelaisTests.tests; RelaisTests.testsDeploiement; RelaisTests.testsRumeurs; RelaisTests.testsFlux; RelaisTests.testsDeclarations; PrenomsTests.testsUsage; PrenomsTests.testsRenommer ]

[<EntryPoint>]
let main argv = runTestsWithCLIArgs [] argv tous
