type Env = {
    host:string
    apiKey:string
}

type Arguments =
    | Host of string
    | ApiKey of string

[<EntryPoint>]
let main args =
    printfn "Arguments passed to function : %A" args
    // Return 0. This indicates success.
    0
