module Managers.CollisionManager

open Core.Component.Types
open Core.Component.Functions

type private Box = {
    Left: float32;
    Top: float32;
    Right: float32;
    Bottom: float32;
}

let private buildBox width height position =
    let x,y = position
    {
        Left = x;
        Top = y;
        Right = x + width;
        Bottom = y - height;
    }

let private getBoundingBox position =
    match position.Collider with
    | NoCollision -> None
    | Square (width, height) ->
        Some <| buildBox width height position.Position

let private intersect (oneStart,oneEnd) (twoStart,twoEnd) =
    (oneStart > twoStart && oneStart < twoEnd) ||
    (oneEnd > twoStart && oneEnd < twoEnd) ||
    (oneStart < twoStart && oneEnd > twoEnd) ||
    ( oneStart = twoStart && oneEnd = twoEnd)

let private intersectBox box1 box2 =
    let x = intersect (box1.Left,box1.Right) (box2.Left,box2.Right)
    let y = intersect (box1.Top,box1.Bottom) (box2.Top,box2.Bottom)
    x && y

let private checkForCollisions entities entity position =
    let bounds = getBoundingBox position
    match bounds with
    | None -> false
    | Some box ->
        entities
        |> Seq.filter (fun e ->
            e.Entity.Id <> entity.Entity.Id
        )
        |> Seq.exists (fun e ->
            match e.WorldPosition with
            | None -> false
            | Some wp ->
                let otherBox = getBoundingBox wp
                match otherBox with
                | None -> false
                | Some box2 -> intersectBox box box2
        )


let private collide entity =
    match entity.Visual with
    | None -> entity
    | Some v ->
        let visual =
            match v with
            | ColoredSquare cs ->
                { cs with
                    Color = makeColor 255 0 0 255
                }
                |> ColoredSquare
            | Textured t ->
                { t with
                    Color = makeColor 255 0 0 255
                }
                |> Textured
        { entity with
            Visual = Some visual
        }
    
let resolveCollisions groups =
    groups
    |> Seq.map (fun entity ->
        match entity.WorldPosition with
        | None -> entity
        | Some position -> 
            let collision = checkForCollisions groups entity position
            match collision with
            | true -> collide entity
            | false -> entity
    )