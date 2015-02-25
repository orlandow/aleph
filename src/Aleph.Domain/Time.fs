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

type Year = 
    | BC of int
    | Year of int

type Century = string

type Date = 
    | CenturyDate of Century
    | YearDate of Year
    | MonthDate of Month * Year
    | DayDate of Day * Month * Year
 
type Period =
    | Unknown
    | Single of Date
    | Exact of start:Date * finish:Date