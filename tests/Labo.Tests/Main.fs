module Labo.Tests.Main

open Expecto

[<Tests>]
let tous =
    testList "labo" [ CsvTests.tests; DslTests.tests; DataTests.tests ]

[<EntryPoint>]
let main argv = runTestsWithCLIArgs [] argv tous
