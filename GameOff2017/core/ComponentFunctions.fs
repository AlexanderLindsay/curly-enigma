module Core.Component.Functions

open Core.Component.Types
open SharpDX.Direct3D9
open SharpDX.Direct3D9

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
    | Movement m -> None

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

let getAllComponents (system: ComponentSystem) =
    match system with
    | ComponentSystem cs ->
        cs
        |> Map.toSeq
        |> Seq.collect (fun (k,v) -> v)

let toEntityGroup (system: ComponentSystem) =
    system
    |> getAllComponents
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
            | Movement m ->
                { g with
                    Movement = Some m;
                }
        ) emptyGroup
    )

let toComponents group =
    let position =
        match group.WorldPosition with
        | Some wp -> WorldPosition wp |> Some
        | None -> None
    let visual =
        match group.Visual with
        | Some v -> Visual v |> Some
        | None -> None
    let movement =
        match group.Movement with
        | Some m -> Movement m |> Some
        | None -> None
    [
        position;
        visual;
        movement;
    ] |> List.toSeq

let fromEntityGroup groups =
    groups
    |> Seq.map toComponents
    |> Seq.collect id
    |> Seq.choose id
    |> buildComponentSystem