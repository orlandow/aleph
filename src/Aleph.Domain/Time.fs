[<AutoOpen>]
module Time

type Day = int

type Month = 
    | January
    | February
    | March
    | April
    | May
    | June
    | July
    | August
    | September
    | October
    | November
    | December

type Year = int

type Century = string

type Date = 
    | Century of string
    | Year of Year
    | Day of Day * Month * Year
 
type Period =
    | Unknown
    | Single of Date
    | Exact of start:Date * finish:Date
