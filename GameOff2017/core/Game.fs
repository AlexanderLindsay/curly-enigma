module Core.Game

open Component.Types
open Component.Functions

type GameState =
| Playing
| Done

type GameData = {
    GameState: GameState;
    Entities: seq<Entity>;
    Components: ComponentSystem;
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
    }