module Managers.NpcManager

open Core.Game
open Core.Component.Types

let private wrap min max value =
    match value with
    | v when v < min -> max
    | v when v > max -> min
    | _ -> value

let updateNpc (world: World) componentGroup =
    match componentGroup.Entity.Type with
    | Npc ->
        let position = componentGroup.WorldPosition
        match position with
        | Some pos ->
            let (x,y) = pos.Position
            let x' = wrap world.Left world.Right x
            let y' = wrap world.Top world.Bottom y
            let positionComponent = 
                { pos with
                    Position = (x',y')
                }
            { componentGroup with
                WorldPosition = Some positionComponent
            }
        | None -> componentGroup
    | _ -> componentGroup