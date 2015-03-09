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
    open Parser
    open System.Text.RegularExpressions
    open People

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
        let getDate str data = maybe {
            let! value = data |> get str
            let re = new Regex(@"\~*\s*(.*)(\s\(.+\))*")
            let date = re.Match(value).Groups.[1].Value
            return date |> Date.parse }

        let name = input |> get "name"
        let profession = input |> get "profession"
        let fullname = basic |> get "full name"
        let birth = basic |> getDate "date of birth"
        let death = basic |> getDate "date of death"
        let lifespan = 
            match birth, death with 
            | Some b, Some d -> Some <| Dead (b, d)
            | Some b, None -> Some <| Alive b
            | _ -> None

        let id = { name = "local"; icon = None }
        async {
            return {
                id = id
                name = name
                fullname = fullname
                profession = profession
                lifespan = lifespan
                images = None
                raw = None
            }
        }
