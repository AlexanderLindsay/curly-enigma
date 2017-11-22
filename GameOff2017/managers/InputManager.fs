module Managers.InputManager

open Core.Component.Types

open Microsoft.Xna.Framework.Input

let changePlayerState duration components =
    match components.Entity.Type with
    | Player state ->
        let state' = 
            { state with 
                Activity = Moving duration
            }
        let entity' =
            { components.Entity with
                Type = Player state'
            }
        { components with 
            Entity = entity'
        }
    | _ -> components

let applyMovement = MovementManager.applyMovement

let private moveLeft speed duration components =
    components
    |> applyMovement (fun (_,y) -> -1.0f * speed,y)
    |> changePlayerState duration

let private moveRight speed duration components =
    components
    |> applyMovement (fun (_,y) -> speed,y)
    |> changePlayerState duration

let private moveUp speed duration components =
    components
    |> applyMovement (fun (x,_) -> x,-1.0f * speed)
    |> changePlayerState duration

let private moveDown speed duration components =
    components
    |> applyMovement (fun (x,_) -> x,speed)
    |> changePlayerState duration

let private handleKey key state components =
    let (Speed speed) = state.Speed
    let duration = state.MovementDuration
    match key with
    | Keys.Left ->
        moveLeft speed duration components
    | Keys.Right ->
        moveRight speed duration components
    | Keys.Up ->
        moveUp speed duration components
    | Keys.Down ->
        moveDown speed duration components
    | _ -> components

let private getPressedKeys (previousState: KeyboardState option) (currentState: KeyboardState) =
    let previouslyPressed =
        match previousState with
        | None -> []
        | Some state ->
            state.GetPressedKeys()
            |> Array.toList

    currentState.GetPressedKeys() 
        |> Array.toList
        |> List.except previouslyPressed


let handleInput (previousState: KeyboardState option) (state: KeyboardState) components =
    let keys = getPressedKeys previousState state

    match components.Entity.Type with
    | Player state ->
        match state.Activity with
        | Standing ->
            keys
            |> List.fold (fun comps key -> handleKey key state comps ) components
        | _ -> components
    | _ -> components