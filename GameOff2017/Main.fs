module Core.Main

open Microsoft.Xna.Framework
open Microsoft.Xna.Framework.Graphics

open Component.Types
open Component.Functions
open Managers
open EntityGenerator
open System

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
                texturedEntity ((100, 100), "dot", (0,0,255,255))
                movingEntity ((200,200), (0.5f,0.5f), "dot", (255,0,0,255))
            ]
            |> List.toSeq
            |> Seq.mapi initalizeEntities
            |> Seq.collect id
            |> buildComponentSystem
        )
    
    let mutable textureMap =
        Map.empty<TextureId,Texture2D>
    
    let mutable effectMap =
        Map.empty<EffectId,Effect>
    
    override gr.Initialize() =
        do spriteBatch <- new SpriteBatch(gr.GraphicsDevice)
        do base.Initialize()
        ()
    
    override gr.LoadContent() =
        textureMap <-
            componentSystem.Value
            |> TextureManager.loadTextures gr.Content
        effectMap <-
            componentSystem.Value
            |> EffectManager.loadEffects gr.Content
        do componentSystem.Force () |> ignore
        ()

    override gr.Update (gameTime) =
        let current = componentSystem.Value
        componentSystem <- lazy (
            current
            |> toEntityGroup
            |> Seq.map MovementManager.resolveVelocities
            |> fromEntityGroup
        )
        do componentSystem.Force () |> ignore
        ()
    
    override gr.Draw (gameTime) =
        do gr.GraphicsDevice.Clear Color.CornflowerBlue
        let draw = drawComponents' textureMap spriteBatch
        let effect = effectMap.TryFind (EffectId "setColor")

        match effect with
        | Some effect' ->
            do spriteBatch.Begin (SpriteSortMode.Deferred, null, null, null, null, effect')
        | None ->
            do spriteBatch.Begin()
        componentSystem.Value
        |> draw
        do spriteBatch.End ()
        ()