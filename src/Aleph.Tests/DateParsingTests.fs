module DateParsingTests

open NUnit.Framework
open Parser

let parses date str =
    let result = Date.Parse str
    Assert.AreEqual(date, result, sprintf "expecting %A but was %A" date result)

[<Test>]
let ``can parse a simple year`` () =
    "1986" |> parses (Year 1986)
    "2015" |> parses (Year 2015)

[<Test>]
let ``can parse a BC year`` () =
    "300 BC" |> parses (Year -300)
    "5000 BC" |> parses (Year -5000)

[<Test>]
let ``can parse a date in long format`` () =
    "Thursday, August 24, 1899" |> parses (Day (24, August, 1899))
    "Saturday, June 14, 1986" |> parses (Day (14, June, 1986))