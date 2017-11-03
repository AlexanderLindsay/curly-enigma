module Core.Main

open Microsoft.Xna.Framework
open Microsoft.Xna.Framework.Graphics

open Component.Types
open Component.Functions
open Managers
open EntityGenerator

type GameRoot () as gr =
    inherit Game()

    do gr.Content.RootDirectory <- "content"

    let graphics = new GraphicsDeviceManager(gr)
    let mutable spriteBatch = Unchecked.defaultof<SpriteBatch>
    let drawComponents' = VisualManager.drawComponents graphics

    let mutable componentSystem =
        lazy (
            [
                simpleEntity ((50,50), (20, 20), (120, 200, 10, 255));
                texturedEntity ((100, 100), "dot")
            ]
            |> List.toSeq
            |> Seq.mapi initalizeEntities
            |> Seq.collect id
            |> buildComponentSystem
        )
    
    let mutable textureMap =
        Map.empty<TextureId,Texture2D>
    
    override gr.Initialize() =
        do spriteBatch <- new SpriteBatch(gr.GraphicsDevice)
        do base.Initialize()
        ()
    
    override gr.LoadContent() =
        textureMap <-
            componentSystem.Value
            |> TextureManager.loadTextures gr.Content
        ()

    override gr.Update (gameTime) =
        do componentSystem.Force () |> ignore
        ()
    
    override gr.Draw (gameTime) =
        do gr.GraphicsDevice.Clear Color.CornflowerBlue
        let draw = drawComponents' textureMap spriteBatch

        do spriteBatch.Begin ()
        componentSystem.Value
        |> draw
        do spriteBatch.End ()
        ()