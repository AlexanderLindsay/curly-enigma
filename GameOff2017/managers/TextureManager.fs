module Managers.TextureManager

open Microsoft.Xna.Framework
open Microsoft.Xna.Framework.Graphics

open Core.Component.Types
open Core.Component.Functions

let loadTextures (content: Content.ContentManager) (system: ComponentSystem) =
    getComponentsByType system ComponentType.Visual
    |> Seq.choose getVisual
    |> Seq.choose getTextured
    |> Seq.map (fun t -> 
        let (TextureId path) = t.TextureId
        t.TextureId, content.Load<Texture2D>(path))
    |> Map.ofSeq