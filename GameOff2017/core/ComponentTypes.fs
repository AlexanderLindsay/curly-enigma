module Core.Component.Types

type EntityId =
    | UninitalizedEntity
    | EntityId of int

type Entity =
    { Id: EntityId }

type WorldPositionComponent =
    {
    EntityId: EntityId;
    Position: float32*float32;
    Size: int*int;
    }

type RGBA =
    {
    R: int;
    G: int;
    B: int;
    A: int;
    }

type ColoredSquareComponent =
    {
    EntityId: EntityId;
    Color: RGBA;
    }

type VisualComponent =
    | ColoredSquare of ColoredSquareComponent

    member this.EntityId =
        match this with
        | ColoredSquare csc -> csc.EntityId

type ComponentType =
    | WorldPosition
    | Visual

type Component =
    | WorldPosition of WorldPositionComponent
    | Visual of VisualComponent

    member this.EntityId =
        match this with
        | WorldPosition wp -> wp.EntityId
        | Visual v -> v.EntityId
    
    member this.Type =
        match this with
        | WorldPosition _ -> ComponentType.WorldPosition
        | Visual _ -> ComponentType.Visual

type ComponentGroup =
    {
        WorldPosition: WorldPositionComponent option;
        Visual: VisualComponent option;
    }
let emptyGroup = { WorldPosition = None; Visual = None; }

type ComponentSystem =
    | ComponentSystem of Map<ComponentType, seq<Component>>