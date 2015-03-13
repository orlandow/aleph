namespace DataProviders

open People

type Image = byte[]

type SuggestionId = { name: string; icon: Image option }

type Suggestions = {
    id: SuggestionId
    name: string option
    fullname: string option
    lifespan: Lifespan option
    profession: Profession option
    images: Image list option

    raw: string option
}
with 
    static member Nothing id = 
        { id = id
          name = None
          fullname = None
          lifespan = None
          profession = None
          images = None
          raw = None }

type Provider = string -> Async<Suggestions>