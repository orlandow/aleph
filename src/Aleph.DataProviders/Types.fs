namespace DataProviders

open People

type Image = byte[]

type Suggestions = {
    name: string option
    fullname: string option
    lifespan: Lifespan option
    profession: Profession option
    images: Image option

    raw: string option
}

type Provider = string -> Suggestions