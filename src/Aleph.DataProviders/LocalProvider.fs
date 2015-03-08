namespace DataProviders

module private IO =
    open System.IO

    let folder = @"d:\development\aleph data"

    let files = 
        Directory.EnumerateFiles(folder, "*.json")
        |> Seq.map (fun x -> 
            (Path.GetFileNameWithoutExtension x,
             File.ReadAllText x))

module private Json =
    open Newtonsoft.Json
    open Newtonsoft.Json.Linq

    let jsonToMap = fun x -> JsonConvert.DeserializeObject<Map<string,obj>>(x)
    let fromJ str (ob:obj) = 
        match (ob :?> JObject).TryGetValue(str) with
        | (true, value) -> Some <| value.ToString()
        | _ -> None


module LocalProvider =

    let data = 
        IO.files
        |> Seq.map (fun (name,value) -> (name, Json.jsonToMap value))

    let byName str =
        data 
        |> Seq.find (fst >> ((=) str))
        |> snd

    let suggest str =
        let data = byName str

        let input = data |> Map.tryFind "Input interpretation" 
        let basic = data |> Map.tryFind "Basic information" 

        let get str = Option.bind (Json.fromJ str)

        let name = input |> get "name"
        let profession = input |> get "profession"
        let fullname = basic |> get "full name"

        let id = { name = "local"; icon = None }
        async {
            return {
                id = id
                name = name
                fullname = fullname
                profession = profession
                lifespan = None
                images = None
                raw = None
            }
        }
