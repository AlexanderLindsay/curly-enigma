module Scenes.PlayingScene

open Core.Game
open Core.Component.Functions
open Managers

open Microsoft.Xna.Framework
open Microsoft.Xna.Framework.Graphics
open Microsoft.Xna.Framework.Input

let update (gameTime: GameTime) (currentKeyboardState: KeyboardState) gameData playState =
    let keys = InputManager.getPressedKeys gameData.PreviousKeyboardState currentKeyboardState
    let handleInput' = InputManager.handleInput keys
    let updatePlayer' = PlayerManager.updatePlayer gameTime.ElapsedGameTime

    let hasEscape = 
        keys
        |> List.exists(fun k -> k = Keys.Escape)

    if hasEscape
    then Done
    else
        let update = 
            handleInput'
            >> MovementManager.resolveVelocities 
            >> updatePlayer'

        let (entities', components') = 
            (playState.Entities, playState.Components)
            |> toEntityGroup
            |> CollisionManager.resolveCollisions
            |> Seq.map update
            |> fromEntityGroup
        
        let playState' = 
            { playState with
                Entities = entities';
                Components = components';
            }

        Playing playState'

let draw (graphics: GraphicsDevice) textureMap spriteBatch playState =
    do graphics.Clear Color.CornflowerBlue
    let draw = VisualManager.drawComponents graphics textureMap spriteBatch
    do spriteBatch.Begin()
    (playState.Entities, playState.Components)
    |> toEntityGroup
    |> draw
    do spriteBatch.End ()