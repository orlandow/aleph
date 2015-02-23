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
    id: Id
    name: string
    continent: Continent
}

type City = {
    id: Id
    name: string
    country: Country
}

type Region =
    | CityRegion of City
    | CountryRegion of Country
    | ContinentRegion of Continent
    | UnknownRegion
