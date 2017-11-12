module Managers.VisualManager

open Microsoft.Xna.Framework
open Microsoft.Xna.Framework.Graphics

open Core.Component.Types
open Core.Component.Functions

let private colorFromRGBA rgba =
    Color(rgba.R, rgba.G, rgba.B, rgba.A)

let private vectorFromPosition position =
    let (x,y) = position.Position
    Vector2(x,y)

let private buildTexture (graphics: GraphicsDeviceManager) getTexture visual =
    match visual with
    | ColoredSquare cs ->
        let width,height = cs.Size
        let texture = new Texture2D(graphics.GraphicsDevice, width,height)
        let size = width * height
        let data = Array.init size (fun _ -> colorFromRGBA cs.Color)
        texture.SetData(data)
        texture
    | Textured t ->
        match getTexture t.TextureId with
        | Some tex -> tex
        | None -> new Texture2D(graphics.GraphicsDevice,1,1)


let drawComponents (graphics: GraphicsDeviceManager) textures (spriteBatch: SpriteBatch) system =
    system
    |> toEntityGroup 
    |> Seq.iter (fun group ->

        let pos = group.WorldPosition
        let vis = group.Visual

        match pos,vis with
        | None,_ -> ()
        | _,None -> ()
        | Some position, Some visual ->
            let getTexture id = 
                textures
                |> Map.tryFind id

            let texture = buildTexture graphics getTexture visual
            let destination = vectorFromPosition position
            spriteBatch.Draw(texture, destination, Color.White)
    )