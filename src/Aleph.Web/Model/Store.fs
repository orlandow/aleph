namespace Aleph.Web.Models

open Raven.Abstractions.FileSystem
open Raven.Client.Embedded

module Store =

    let store = new EmbeddableDocumentStore(RunInMemory = true)

    store.Conventions.CustomizeJsonSerializer <- FJsonConverters.converters
    store.Initialize() |> ignore
