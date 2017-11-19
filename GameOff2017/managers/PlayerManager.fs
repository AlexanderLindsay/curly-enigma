module Managers.PlayerManager

open System
open Core.Component.Types

let private setPlayerActivity state group activity =
        let state' =
            { state with 
                Activity = activity
            }
        let entity' = { group.Entity with Type = Player state' }
        { group with Entity = entity' }

let private updatePlayerMovementDuration elapsedTime duration playerState componentGroup =
    let (Duration totalTime) = duration
    let timeLeft = totalTime - elapsedTime
    let updateActivity = setPlayerActivity playerState componentGroup
    match timeLeft with
    | x when x <= 0.0f ->
        updateActivity Standing
        |> MovementManager.applyMovement (fun (_,_) -> 0.0f,0.0f)
    | _ ->
        updateActivity (Moving <| Duration timeLeft)

let updatePlayer (elapsedTime: TimeSpan) componentGroup =
    match componentGroup.Entity.Type with
    | Player state ->
        match state.Activity with
        | Standing -> componentGroup
        | Moving duration ->
            let elapsedMilliseconds = float32 elapsedTime.TotalMilliseconds
            updatePlayerMovementDuration elapsedMilliseconds duration state componentGroup
    | _ -> componentGroup