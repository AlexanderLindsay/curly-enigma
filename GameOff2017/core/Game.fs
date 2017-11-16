module Core.Game

open Component.Types

type GameState =
| IsAlive
| IsDead

type GameData = {
    GameState: GameState;
    Components: Lazy<ComponentSystem>;
}