module Parser

open System

module private Regex = 
    open System.Text.RegularExpressions

    let [<Literal>] yearBC = @"(?<year>\d+)\s*(bc|b.c|b.c.|BC)"
    let [<Literal>] year = @"^\d+$"

    let matches re f =
        fun str ->
            let re = new Regex(re)
            let m = re.Match(str)
            if m.Success
                then Some (f <| m.Groups)
                else None

module private DateTime =
    let private dfi = new Globalization.DateTimeFormatInfo()
    let parse str =
        match DateTime.TryParse(str) with
        | (true, d) -> Some (d.Day, d.Month, d.Year)
        | _ -> None

    let toMonth i = dfi.GetMonthName i |> Reflection.toDU<Month>

let (|YearBC|_|) = Regex.matches Regex.yearBC (fun m -> Int32.Parse(m.["year"].Value))
let (|Year|_|) = Regex.matches Regex.year (fun m -> Int32.Parse(m.[0].Value))
let (|DateTime|_|) str = DateTime.parse str

type Date with 
    static member parse str = 
        match str with
        | Year year -> Year year
        | YearBC year -> Year (-year)
        | DateTime (d,m,y) -> Day (d, m |> DateTime.toMonth, y)
        | _ -> failwith "bad input"
