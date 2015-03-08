module Parser

open System
open System.Text.RegularExpressions

let [<Literal>] yearBC = @"(?<year>\d+)\s*(bc|b.c|b.c.|BC)"

let Re re f =
    fun str ->
        let re = new Regex(re)
        let m = re.Match(str)
        if m.Success
            then Some (f <| m.Groups)
            else None

let (|YearBC|_|) = Re yearBC (fun m -> Int32.Parse(m.["year"].Value))
let (|Year|_|) = Re "^\d+$" (fun m -> Int32.Parse(m.[0].Value))

type Date with 
    static member Parse str = 
        match str with
        | Year year -> Year year
        | YearBC year -> Year (-year)
        | _ -> failwith "bad input"
