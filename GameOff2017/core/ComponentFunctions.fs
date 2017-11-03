module Core.Component.Functions

open Core.Component.Types

let createColor (r,g,b,a) =
    {
        R = r;
        G = g;
        B = b;
        A = a;
    }

// type Getters/Checkers
let getVisual comp =
    match comp with
    | WorldPosition _ -> None
    | Visual v -> Some v

let getTextured visual =
    match visual with
    | ColoredSquare _ -> None
    | Textured t -> Some t

//Component System Functions
let buildComponentSystem (components: seq<Component>) =
    components
    |> Seq.groupBy (fun comp -> comp.Type)
    |> Map.ofSeq
    |> ComponentSystem

let getComponentsByType (system: ComponentSystem) componentType =
    match system with
    | ComponentSystem cs ->
        let value =
            cs
            |> Map.tryFind componentType
        match value with
        | Some components -> components
        | None -> Seq.empty<Component>

let getEntityGroupings componentTypes (system: ComponentSystem) =
    componentTypes
    |> List.map (getComponentsByType system)
    |> Seq.collect id
    |> Seq.groupBy (fun c -> c.EntityId)
    |> Seq.map (fun (_, comps) ->
        comps
        |> Seq.fold (fun g c ->
            match c with
            | WorldPosition wp ->
                { g with
                    WorldPosition = Some wp;
                }
            | Visual v ->
                { g with
                    Visual = Some v;
                }
        ) emptyGroup
    )