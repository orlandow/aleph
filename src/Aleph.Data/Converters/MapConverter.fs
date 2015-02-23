namespace Aleph.Data.Converters

open System
open Microsoft.FSharp.Reflection
open Raven.Imports.Newtonsoft.Json
open System.Collections.Generic
open Raven.Imports.Newtonsoft.Json.Linq
open System.Reflection

type MapConverter() =
    inherit JsonConverter()

    let genericTypes (t:Type) =
        let arr = t.GetGenericArguments()
        (arr.[0], arr.[1])

    static member BuildMap<'k,'v when 'k:comparison>(d:Dictionary<'k,'v>) =
        d :> IEnumerable<KeyValuePair<'k,'v>>
        |> Seq.map (fun t -> (t.Key, t.Value))
        |> Map.ofSeq

    override x.CanConvert(t:Type) = 
        t.IsGenericType && t.GetGenericTypeDefinition() = typedefof<Map<_,_>>
 
    override x.WriteJson(writer, value, serializer) =
        let k,v = genericTypes <| value.GetType()
        let dictType = typedefof<Dictionary<_,_>>.MakeGenericType(k, v)
        let d = Activator.CreateInstance(dictType, value)

        serializer.Serialize(writer, d)
 
    override x.ReadJson(reader, t, obj, serializer) = 
        let k,v = genericTypes t
        let collectionType = typedefof<Dictionary<_,_>>.MakeGenericType(k, v)
        let collection = serializer.Deserialize(reader, collectionType) 

        let builder = typedefof<MapConverter>.GetMethod("BuildMap")
        let builder = builder.MakeGenericMethod([|k;v|])
        builder.Invoke(null, [|collection|])
