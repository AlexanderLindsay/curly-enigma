module Scenes.DoneScene

open Core.Game
open Core.Component.Types
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
        PlayingScene.buildLevel -200.0f 1000.0f -200.0f 1000.0f
        |> Playing

let draw (graphics: GraphicsDevice) fontsMap (spriteBatch: SpriteBatch) =
    do graphics.Clear Color.Black

    let maybeHeader = 
        fontsMap
        |> Map.tryFind (FontId "header")

    let maybeSubheader = 
        fontsMap
        |> Map.tryFind (FontId "subheader")

    match maybeHeader,maybeSubheader with
    | Some header, Some subheader ->
        do spriteBatch.Begin()
        spriteBatch.DrawString(header, "Jay Walker", Vector2(100.0f, 100.0f), Color.LimeGreen)
        spriteBatch.DrawString(subheader, "Press enter to start", Vector2(100.0f, 200.0f), Color.LimeGreen)
        do spriteBatch.End ()
        ()
    | _ -> ()
