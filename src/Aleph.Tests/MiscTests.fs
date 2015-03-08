module MiscTests

open NUnit.Framework

type Enum = One | Two | Three

[<Test>]
let ``can convert str to du`` () =
    let one = "One" |> Reflection.toDU<Enum>
    Assert.AreEqual(One, one)