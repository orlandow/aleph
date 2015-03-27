namespace DataProviders

module private IO =
    open System.IO

    let folder = @"d:\development\aleph data"
    let imgFolder = Path.Combine(folder, "imgs3")

    let files = 
        Directory.EnumerateFiles(folder, "*.json")
        |> Seq.map (fun x -> 
            (Path.GetFileNameWithoutExtension x,
             lazy File.ReadAllText x))

    let images =
        Directory.EnumerateFiles(imgFolder, "*.jpg")
        |> Seq.map (fun x -> 
            (Path.GetFileNameWithoutExtension x,
             lazy (File.Open(x, FileMode.Open) :> Stream)))

    let find str = 
        Seq.tryFind (fst >> (=) str) 
        >> Option.map (fun (_,raw) -> raw |> Lazy.get)

    let byName str = files |> find str
    let img str = images |> find str

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

    let getText str = Option.bind (Json.fromJ str) >> Option.map Text
    let getDate str data = maybe {
        let! value = data |> Option.bind (Json.fromJ str)
        let re = new Regex(@"\~*\s*(?<date>.*)(\s+\(.+\))+?")
        let date = re.Match(value).Groups.["date"].Value
        return date |> Date.parse } |> Option.map Date

    let getImg str =
        maybe {
            let! img = IO.img str
            return [img]
        }

    let suggest str =
        let data = maybe {
            let! raw = IO.byName str
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
        let img = getImg str

        async {
            return {
                id = id
                data = data
                images = img
                raw = None
            }
        }
