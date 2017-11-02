module Managers.VisualManager

open Microsoft.Xna.Framework
open Microsoft.Xna.Framework.Graphics

open Core.Component.Types
open Core.Component.Functions

let private neededComponents = [
    ComponentType.WorldPosition;
    ComponentType.Visual;
]

let private colorFromRGBA rgba =
    Color(rgba.R, rgba.G, rgba.B, rgba.A)

let private vectorFromPosition position =
    let (x,y) = position.Position
    Vector2(x,y)

let private fillTexture (texture: Texture2D) visual =
    match visual with
    | ColoredSquare cs ->
        let size = texture.Width * texture.Height
        let data = Array.init size (fun _ -> colorFromRGBA cs.Color)
        texture.SetData(data)
        ()

let drawComponents (graphics: GraphicsDeviceManager) (spriteBatch: SpriteBatch) system =
    system
    |> getEntityGroupings neededComponents 
    |> Seq.iter (fun group ->

        let pos = group.WorldPosition
        let vis = group.Visual

        match pos,vis with
        | None,_ -> ()
        | _,None -> ()
        | Some position, Some visual ->
            let width,height = position.Size
            let texture = new Texture2D(graphics.GraphicsDevice, width,height)
            fillTexture texture visual
            let destination = vectorFromPosition position
            spriteBatch.Draw(texture, destination, Color.White)
    )