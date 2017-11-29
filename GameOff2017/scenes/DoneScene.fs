module Scenes.DoneScene

open Core.Game
open Managers

open Microsoft.Xna.Framework
open Microsoft.Xna.Framework.Graphics
open Microsoft.Xna.Framework.Input

let update (gameTime: GameTime) (currentKeybordState: KeyboardState) gameData = 
    let keys = InputManager.getPressedKeys gameData.PreviousKeyboardState currentKeybordState
    let startGame =
        keys
            |> List.exists(fun k -> k = Keys.Enter)
    match startGame with
    | false -> Done
    | true -> 
        Playing <| PlayingScene.buildLevel -200.0f 1000.0f -200.0f 1000.0f

let draw (graphics: GraphicsDevice) =
    do graphics.Clear Color.Black