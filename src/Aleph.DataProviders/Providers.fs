namespace DataProviders

module Providers =
    let all : Provider list =
        [ LocalProvider.suggest 
          BingImageProvider.suggest ]

    let suggest str =
        all
        |> Seq.map (fun p -> 
            async {
                try
                    let! result = p str
                    return Success result 
                with
                | ex -> return Failure ex })
        |> Async.Parallel