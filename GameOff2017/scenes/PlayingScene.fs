module Scenes.PlayingScene

open Core.Game
open Core.Component.Types
open Core.Component.Functions
open Managers
open EntityGenerator

open Microsoft.Xna.Framework
open Microsoft.Xna.Framework.Graphics
open Microsoft.Xna.Framework.Input

let buildLevel () = 
    let components,entities =
        [
            player <| Speed 1.0f <| Duration 300.0f <| movingEntity ((400, 400), (50.0f,50.0f), (0.0f,0.0f), "dot", (0,0,255,255));
            npc <| movingEntity ((200,200), (50.0f,50.0f), (2.0f,0.0f), "dot", (0,10,130,255));
            npc <| movingEntity ((0,200), (50.0f,50.0f), (2.0f,0.0f), "dot", (0,10,130,255));
            npc <| movingEntity ((-200,200), (50.0f,50.0f), (2.0f,0.0f), "dot", (0,10,130,255));
            npc <| movingEntity ((800,100), (50.0f,50.0f), (-2.0f,0.0f), "dot", (0,10,130,255));
            npc <| movingEntity ((1000,100), (50.0f,50.0f), (-2.0f,0.0f), "dot", (0,10,130,255));
            npc <| movingEntity ((200,100), (50.0f,50.0f), (-2.0f,0.0f), "dot", (0,10,130,255));
        ]
        |> List.mapi initalizeEntities
        |> List.collect id
        |> List.unzip
    {
        Components = components |> buildComponentSystem;
        Entities = entities;
    }

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