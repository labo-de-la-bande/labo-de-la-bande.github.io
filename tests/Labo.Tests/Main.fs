module Labo.Tests.Main

open Expecto

[<Tests>]
let tous =
    testList "labo" [ CsvTests.tests; DslTests.tests; DataTests.tests; OracleTests.tests; LentillesTests.tests; JeuTests.tests; TempsTests.tests; SceneTests.tests; SceneTests.testsSuite; SceneTests.testsAttitudes; SceneTests.testsCompletude; RevirementTests.tests; PrenomsTests.tests; JournalTests.tests; JournalTests.testsRegistre; JournalTests.testsHistoires; JournalTests.testsRessenti; JournalTests.testsSensibilite; JournalTests.testsConfianceRegard; JournalTests.testsVieAvance; JournalTests.testsRoles; JournalTests.testsParis; PrenomsTests.testsRegions ]

[<EntryPoint>]
let main argv = runTestsWithCLIArgs [] argv tous
