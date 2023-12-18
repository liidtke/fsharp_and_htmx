module Models

open Domain


module Device =
    let convert (d: Device) =
        match d with
        | CPU -> "CPU"
        | Memory -> "Memory"
        | GPU -> "GPU"
        | _ -> failwith "todo"

    let parse (str: string) =
        match str with
        | "CPU" -> Device.CPU
        | "Memory" -> Device.Memory
        | "GPU" -> Device.GPU
        | _ -> failwith "todo"
