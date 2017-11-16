module Managers.EffectManager

open Microsoft.Xna.Framework
open Microsoft.Xna.Framework.Graphics

open Core.Component.Types
open Core.Component.Functions
open Microsoft.Xna.Framework.Graphics

let loadEffects (content: Content.ContentManager) =
    ["setColor"]
    |> List.map (fun e -> 
        let effectId = EffectId e
        let path = e
        effectId, content.Load<Effect>(path))
    |> Map.ofList