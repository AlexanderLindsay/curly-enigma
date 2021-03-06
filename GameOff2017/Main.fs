module Core.Main

open System

open Microsoft.Xna.Framework
open Microsoft.Xna.Framework.Graphics
open Microsoft.Xna.Framework.Input

open Component.Types
open Component.Functions
open Managers
open EntityGenerator
open Game
open Scenes

type GameRoot () as gr =
    inherit Game()

    do gr.Content.RootDirectory <- "content"

    let graphics = new GraphicsDeviceManager(gr)
    let mutable spriteBatch = Unchecked.defaultof<SpriteBatch>

    let mutable gameData =
        // PlayingScene.buildLevel -200.0f 1000.0f -200.0f 1000.0f
        // |> Playing
        // |> buildGameData
        Done
        |> buildGameData
    
    let mutable textureMap =
        Map.empty<TextureId,Texture2D>
    
    let mutable effectMap =
        Map.empty<EffectId,Effect>
    
    let mutable fontsMap =
        Map.empty<FontId, SpriteFont>
    
    override gr.Initialize() =
        do spriteBatch <- new SpriteBatch(gr.GraphicsDevice)
        do base.Initialize()
        ()
    
    override gr.LoadContent() =
        let components =
            match gameData.GameState with
            | Playing data -> data.Components
            | Done -> [] |> buildComponentSystem
        textureMap <-
            [
                "dot"
            ]
            |> TextureManager.loadTextures gr.Content
        effectMap <-
            EffectManager.loadEffects gr.Content
        fontsMap <-
            [
            "header";
            "subheader";
            ]
            |> List.map (fun n -> FontId n,gr.Content.Load<SpriteFont>(n))
            |> Map.ofList
        ()

    override gr.Update (gameTime) =
        let currentKeyboardState = Keyboard.GetState()

        let state' = 
            match gameData.GameState with
            | Playing data ->
                PlayingScene.update gameTime currentKeyboardState gameData data
            | Done ->
                DoneScene.update gameTime currentKeyboardState gameData

        gameData <- 
            { gameData with
                GameState = state';
                PreviousKeyboardState = Some currentKeyboardState;
            }
        ()
    
    override gr.Draw (gameTime) =
        match gameData.GameState with
        | Playing data ->
            PlayingScene.draw graphics.GraphicsDevice textureMap spriteBatch data
            ()
        | Done ->
            DoneScene.draw graphics.GraphicsDevice fontsMap spriteBatch
            ()