module Labo.Tests.Main

open Expecto

[<Tests>]
let tous =
    testList "labo" [ CsvTests.tests; DslTests.tests; DataTests.tests; OracleTests.tests; LentillesTests.tests; JeuTests.tests; TempsTests.tests; SceneTests.tests ]

[<EntryPoint>]
let main argv = runTestsWithCLIArgs [] argv tous
