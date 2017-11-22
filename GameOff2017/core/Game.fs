module Core.Game

open Component.Types
open Component.Functions

open Microsoft.Xna.Framework.Input

type GameState =
| Playing
| Done

type GameData = {
    GameState: GameState;
    Entities: seq<Entity>;
    Components: ComponentSystem;
    PreviousKeyboardState: KeyboardState option;
}

let buildGameData (components, entities) =
    {
        GameState = Playing;
        Entities = 
            entities 
            |> Seq.ofList;
        Components = 
            components
            |> Seq.ofList
            |> buildComponentSystem;
        PreviousKeyboardState = None;
    }