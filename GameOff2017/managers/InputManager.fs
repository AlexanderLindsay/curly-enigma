module Managers.InputManager

open Core.Component.Types

open Microsoft.Xna.Framework.Input

let applyMovement components adjustment =
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

let private moveLeft delta components =
    applyMovement components (fun (x,y) -> x - delta,y)

let private moveRight delta components =
    applyMovement components (fun (x,y) -> x + delta,y)

let private moveUp delta components =
    applyMovement components (fun (x,y) -> x,y - delta)

let private moveDown delta components =
    applyMovement components (fun (x,y) -> x,y + delta)

let private handleKey key components =
    let delta = 1.0f
    match key with
    | Keys.Left ->
        moveLeft delta components
    | Keys.Right ->
        moveRight delta components
    | Keys.Up ->
        moveUp delta components
    | Keys.Down ->
        moveDown delta components
    | _ -> components

let handleInput (state: KeyboardState) components =
    let keys = state.GetPressedKeys() |> Array.toList
    match components.Entity.Type with
    | Player ->
        keys
        |> List.fold (fun state key -> handleKey key state ) components
    | _ -> components

