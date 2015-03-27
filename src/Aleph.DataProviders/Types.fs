namespace DataProviders

type Image = byte[]

type SuggestionId = { name: string; icon: Image option }

type Suggestions = {
    id: SuggestionId
    data: Map<string, obj>
    images: Image list option

    raw: string option
}
with 
    static member Nothing id = 
        { id = id
          data = Map.empty
          images = None
          raw = None }

type Provider = string -> Async<Suggestions>