namespace DataProviders

module private IO =
    open System.IO

    let folder = @"d:\development\aleph data"

    let files = 
        Directory.EnumerateFiles(folder, "*.json")
        |> Seq.map (fun x -> 
            (Path.GetFileNameWithoutExtension x,
             File.ReadAllText x))

    let byName str = files |> Seq.tryFind ((=) str)

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

    let getText str = Option.bind (Json.fromJ str) >> Option.map Text
    let getDate str data = maybe {
        let! value = data |> Option.bind (Json.fromJ str)
        let re = new Regex(@"\~*\s*(?<date>.*)(\s+\(.+\))+?")
        let date = re.Match(value).Groups.["date"].Value
        return date |> Date.parse } |> Option.map Date

    let suggest str =
        let data = maybe {
            let! (_,raw) = IO.byName str
            let data = Json.jsonToMap raw

            let input = data |> Map.tryFind "Input interpretation" 
            let basic = data |> Map.tryFind "Basic information" 

            let name = input |> getText "name"
            let profession = input |> getText "profession" 
            let fullname = basic |> getText "full name" 
            let birth = basic |> getDate "date of birth" 
            let death = basic |> getDate "date of death"

            let data = [ yield "name", name
                         yield "profession", profession
                         yield "fullname", fullname
                         yield "birth", birth
                         yield "death", death ] 
                       |> Map.ofList
                       |> Map.choose (
                            function
                            | name, Some data -> Some (name, data)
                            | _ -> None)

            return data } |> defaultArg <| Map.empty

        let id = { name = "local"; icon = None }

        async {
            return {
                id = id
                data = data
                images = None
                raw = None
            }
        }
