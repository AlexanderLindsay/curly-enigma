module Managers.VisualManager

open Microsoft.Xna.Framework
open Microsoft.Xna.Framework.Graphics

open Core.Game
open Core.Component.Types
open Core.Component.Functions

let private colorFromRGBA rgba =
    Color(rgba.R, rgba.G, rgba.B, rgba.A)

let private vectorFromPosition position =
    let (x,y) = position.Position
    Vector2(x,y)

let private buildTexture (graphics: GraphicsDevice) getTexture visual =
    match visual with
    | ColoredSquare cs ->
        let width,height = cs.Size
        let texture = new Texture2D(graphics, width,height)
        let size = width * height
        let color = colorFromRGBA cs.Color
        let data = Array.init size (fun _ -> color)
        texture.SetData(data)
        texture, color
    | Textured t ->
        match getTexture t.TextureId with
        | Some tex -> tex, colorFromRGBA t.Color
        | None -> new Texture2D(graphics,1,1), Color.White

let drawComponents (graphics: GraphicsDevice) textures (spriteBatch: SpriteBatch) entityGroups =
    entityGroups
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
            
            let texture, color = buildTexture graphics getTexture visual
            let destination = vectorFromPosition position
            spriteBatch.Draw(texture, destination, color)
    )