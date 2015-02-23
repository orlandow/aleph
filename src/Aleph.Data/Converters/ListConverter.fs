namespace Aleph.Data.Converters

open System
open Microsoft.FSharp.Reflection
open Raven.Imports.Newtonsoft.Json
open System.Collections.Generic
open Raven.Imports.Newtonsoft.Json.Linq

type ListConverter() =
    inherit JsonConverter()
    
    override x.CanConvert(t:Type) = 
        t.IsGenericType && t.GetGenericTypeDefinition() = typedefof<list<_>>
 
    override x.WriteJson(writer, value, serializer) =
        let list = value :?> System.Collections.IEnumerable |> Seq.cast
        serializer.Serialize(writer, list)
 
    override x.ReadJson(reader, t, _, serializer) = 
        let itemType = t.GetGenericArguments().[0]
        let collectionType = typedefof<List<_>>.MakeGenericType(itemType)
        let collection = serializer.Deserialize(reader, collectionType) 
                            :?> System.Collections.IEnumerable
                            |> Seq.cast
                            |> Seq.toList

        let listType = typedefof<list<_>>.MakeGenericType(itemType)
        let cases = FSharpType.GetUnionCases(listType)
        let rec make = function
            | [] -> FSharpValue.MakeUnion(cases.[0], [||])
            | head::tail -> FSharpValue.MakeUnion(cases.[1], [| head; (make tail); |])                    
        make (collection)
