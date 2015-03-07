namespace DataProviders

module Providers =
    let all : Provider list =
        [ LocalProvider.suggest 
          BingImageProvider.suggest ]