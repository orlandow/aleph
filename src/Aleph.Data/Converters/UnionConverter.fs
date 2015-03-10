namespace Aleph.Data.Converters

open System
open Microsoft.FSharp.Reflection
open Raven.Imports.Newtonsoft.Json
open System.Collections
open Raven.Imports.Newtonsoft.Json.Linq

type UnionConverter() =
    inherit JsonConverter()

    let [<Literal>] CaseTagName = "Case"
    let [<Literal>] FieldsTagName = "Fields"

    override x.CanConvert(t) = 
        not(typeof<IEnumerable>.IsAssignableFrom(t)) && FSharpType.IsUnion(t)

    override x.WriteJson(writer, value, serializer) = 
        let t = value.GetType()
        let case, fields = FSharpValue.GetUnionFields(value, t)

        writer.WriteStartObject()
        writer.WritePropertyName(CaseTagName)
        writer.WriteValue(case.Name)

        if fields |> Array.length > 0 then
            writer.WritePropertyName(FieldsTagName)
            serializer.Serialize(writer, fields)

        writer.WriteEndObject()
            
    override x.ReadJson(reader, t, existingValue, serializer) = 
        let consume() = 
            if reader.Read() then () 
            else failwith "unexpected end when reading union"

        let (|Case|Fields|Other|) (name, value:JToken) =
            match name, value.Type with
            | CaseTagName, JTokenType.String ->
                FSharpType.GetUnionCases(t)
                |> Array.tryFind (fun c -> c.Name = value.ToString())
                |> Option.map (fun c -> Case c)
                |> defaultArg <| Other
            | FieldsTagName, JTokenType.Array -> Fields (value :?> JArray)
            | _ -> Other

        consume() // start object

        reader |> Seq.unfold (fun reader ->
            match reader.TokenType with
            | JsonToken.PropertyName ->
                let name = reader.Value.ToString()
                consume()
                let token = JToken.ReadFrom(reader)
                consume()
                Some ((name, token), reader)
            | JsonToken.EndObject -> None
            | _ -> failwith "expecting property name when reading union")
        |> Seq.toList
        |> function
            | [Case caseInfo; Fields fields]
            | [Fields fields; Case caseInfo] ->
                Array.init (fields.Count) (fun i -> fields.[i])
                |> Array.zip (caseInfo.GetFields())
                |> Array.map (fun (info, field) ->
                    field.ToObject(info.PropertyType, serializer))
                |> fun fields -> FSharpValue.MakeUnion(caseInfo, fields)
            | [Case caseInfo] when caseInfo.GetFields().Length = 0 ->
                FSharpValue.MakeUnion(caseInfo, [||])
            | _ -> failwith "unexpected property when reading union"
