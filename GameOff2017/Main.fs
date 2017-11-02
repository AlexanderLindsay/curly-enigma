module Core.Main

open Microsoft.Xna.Framework
open Microsoft.Xna.Framework.Graphics

open Core.Component.Types
open Core.Component.Functions
open Managers

type GameRoot () as gr =
    inherit Game()

    let graphics = new GraphicsDeviceManager(gr)
    let mutable spriteBatch = Unchecked.defaultof<SpriteBatch>
    let drawComponents' = VisualManager.drawComponents graphics

    let createSimpleEntity (id, position, size, color) =
        let entityId = EntityId id;

        let (x,y) = position
        let fx = float32 x
        let fy = float32 y
        let position =
            {
                EntityId = entityId;
                Position = (fx,fy);
                Size = size;
            }
        let square =
            {
                EntityId = entityId;
                Color = createColor color;
            }
        [
            WorldPosition position;
            square |> ColoredSquare |> Visual
        ]

    let mutable componentSystem =
        lazy (
            [
                (1, (50,50), (20, 20), (120, 200, 10, 255));
            ]
            |> List.toSeq
            |> Seq.map createSimpleEntity
            |> Seq.collect id
            |> buildComponentSystem
        )
    
    override gr.Initialize() =
        do spriteBatch <- new SpriteBatch(gr.GraphicsDevice)
        do base.Initialize()
        ()
    
    override gr.LoadContent() =
        do componentSystem.Force () |> ignore
        ()

    override gr.Update (gameTime) =
        do componentSystem.Force () |> ignore
        ()
    
    override gr.Draw (gameTime) =
        do gr.GraphicsDevice.Clear Color.CornflowerBlue
        let draw = drawComponents' spriteBatch

        do spriteBatch.Begin ()
        componentSystem.Value
        |> draw
        do spriteBatch.End ()
        ()