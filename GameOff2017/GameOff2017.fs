module GameOff2017

open Core.Main

[<EntryPoint>]
let main argv =
    use gr = new GameRoot()
    gr.Run()
    0 // return an integer exit code
