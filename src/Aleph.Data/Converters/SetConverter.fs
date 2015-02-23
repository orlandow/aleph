namespace Aleph.Data.Converters

open System
open Microsoft.FSharp.Reflection
open Raven.Imports.Newtonsoft.Json
open System.Collections.Generic
open Raven.Imports.Newtonsoft.Json.Linq
open System.Reflection

type SetConverter() =
    inherit JsonConverter()

    override x.CanConvert(t:Type) = 
        t.IsGenericType && t.GetGenericTypeDefinition() = typedefof<Set<_>>
 
    override x.WriteJson(writer, value, serializer) =
        let list = value :?> System.Collections.IEnumerable |> Seq.cast
        serializer.Serialize(writer, list)
 
    override x.ReadJson(reader, t, obj, serializer) = 
        let itemType = t.GetGenericArguments().[0]
        let collectionType = typedefof<List<_>>.MakeGenericType(itemType)
        let collection = serializer.Deserialize(reader, collectionType) 

        let setType = typedefof<Set<_>>.MakeGenericType(itemType)
        System.Activator.CreateInstance(setType, collection)