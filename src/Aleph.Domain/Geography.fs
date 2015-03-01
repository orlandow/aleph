[<AutoOpen>]
module Geography

type Continent =
    | Asia
    | Africa
    | America
    | Europe
    | Australia
    | Antarctica

type Country = {
    name: string
    continent: Continent
}
