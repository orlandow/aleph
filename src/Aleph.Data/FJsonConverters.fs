module FJsonConverters 

open Raven.Imports.Newtonsoft.Json
open Aleph.Data.Converters
open System

let converters = 
    new Action<JsonSerializer>(fun x ->
        x.Converters.Add(new OptionConverter())
        x.Converters.Add(new ListConverter())
        x.Converters.Add(new SetConverter())
        x.Converters.Add(new MapConverter())
        x.Converters.Add(new UnionConverter()))