module Core.Game

open Component.Types
open Component.Functions

open Microsoft.Xna.Framework.Input

type PlayData = {
    Entities: seq<Entity>;
    Components: ComponentSystem;
}

type GameState =
| Playing of PlayData
| Done

type GameData = {
    GameState: GameState;
    PreviousKeyboardState: KeyboardState option;
}

let buildGameData (components, entities) =
    let playState = {
        Entities = 
            entities 
            |> Seq.ofList;
        Components =
            components
            |> Seq.ofList
            |> buildComponentSystem;
    }
    {
        GameState = Playing playState;
        PreviousKeyboardState = None;
    }