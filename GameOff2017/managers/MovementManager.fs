module Managers.MovementManager

open Microsoft.Xna.Framework
open Microsoft.Xna.Framework.Graphics

open Core.Component.Types
open Core.Component.Functions

let private updatePosition position movement =
    let x,y = position.Position
    let dx,dy = movement.Velocity

    let x' = x + dx
    let y' = y + dy

    { position with
        Position = x',y'
    }

let resolveVelocities entityGroup =
    let pos = entityGroup.WorldPosition
    let mov = entityGroup.Movement

    match pos,mov with
    | None,_ -> entityGroup
    | _,None -> entityGroup
    | Some position, Some movement ->
        { entityGroup with
            WorldPosition = updatePosition position movement |> Some;
            Movement = Some movement
        }

let applyMovement adjustment components =
    match components.Movement with
    | None -> components
    | Some movement ->
        let movement' =
            { movement with
                Velocity = adjustment movement.Velocity
            }
        { components with
            Movement = Some movement'
        }