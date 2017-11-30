module Managers.TextureManager

open Microsoft.Xna.Framework
open Microsoft.Xna.Framework.Graphics

open Core.Component.Types

let loadTextures (content: Content.ContentManager) textures =
    textures
    |> List.map (fun t ->
        let id = TextureId t
        id, content.Load<Texture2D>(t)
    )
    |> Map.ofList