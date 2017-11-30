module Core.Game

open Component.Types
open Component.Functions

open Microsoft.Xna.Framework.Input

type World = {
    Left: float32;
    Right: float32;
    Top: float32;
    Bottom: float32;
}

type PlayData = {
    World: World;
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

let buildGameData gameState =
    {
        GameState = gameState;
        PreviousKeyboardState = None;
    }