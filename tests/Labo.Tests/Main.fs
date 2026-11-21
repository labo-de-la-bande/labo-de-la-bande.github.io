module Labo.Tests.Main

open Expecto

[<Tests>]
let tous =
    testList "labo" [ CsvTests.tests; DslTests.tests; DataTests.tests; OracleTests.tests ]

[<EntryPoint>]
let main argv = runTestsWithCLIArgs [] argv tous
