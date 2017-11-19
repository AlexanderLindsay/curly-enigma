module Core.Component.Types

type EntityId =
    | UninitalizedEntity
    | EntityId of int

type Speed =
    | Speed of float32

type Duration =
    | Duration of float32

type PlayerActivity =
    | Standing
    | Moving of Duration

type PlayerState =
    {
    Speed: Speed;
    MovementDuration: Duration;
    Activity: PlayerActivity;
    }

type EntityType =
    | Player of PlayerState
    | Npc

type Entity =
    { 
        Id: EntityId;
        Type: EntityType;
    }

type Collider =
    | NoCollision
    | Square of float32*float32

type WorldPositionComponent =
    {
    EntityId: EntityId;
    Position: float32*float32;
    Collider: Collider;
    }

type MovementComponent =
    {
    EntityId: EntityId;
    Velocity: float32*float32;
    }

type RGBA =
    {
    R: int;
    G: int;
    B: int;
    A: int;
    }

type EffectId =
| EffectId of string

type TextureId =
| TextureId of string

type ColoredSquareComponent =
    {
    EntityId: EntityId;
    Color: RGBA;
    Size: int*int;
    }

type TexturedComponent =
    {
    EntityId: EntityId;
    TextureId: TextureId;
    Color: RGBA;
    }

type VisualComponent =
    | ColoredSquare of ColoredSquareComponent
    | Textured of TexturedComponent

    member this.EntityId =
        match this with
        | ColoredSquare csc -> csc.EntityId
        | Textured t -> t.EntityId
    
    member this.UpdateId id =
        match this with
        | ColoredSquare csc ->
            { csc with 
                EntityId = id
            } |> ColoredSquare
        | Textured t ->
            { t with 
                EntityId = id
            } |> Textured

type ComponentType =
    | WorldPosition
    | Visual
    | Movement

type Component =
    | WorldPosition of WorldPositionComponent
    | Visual of VisualComponent
    | Movement of MovementComponent

    member this.EntityId =
        match this with
        | WorldPosition wp -> wp.EntityId
        | Visual v -> v.EntityId
        | Movement m -> m.EntityId
    
    member this.UpdateId id =
        match this with
        | WorldPosition wp ->
            { wp with
                EntityId = id
            } |> WorldPosition
        | Visual v -> 
            v.UpdateId id
            |> Visual
        | Movement m ->
            { m with
                EntityId = id
            } |> Movement
    
    member this.Type =
        match this with
        | WorldPosition _ -> ComponentType.WorldPosition
        | Visual _ -> ComponentType.Visual
        | Movement _ -> ComponentType.Movement

type ComponentGroup =
    {
        Entity: Entity;
        WorldPosition: WorldPositionComponent option;
        Visual: VisualComponent option;
        Movement: MovementComponent option;
    }
let emptyGroup entity =
    { 
    Entity = entity;
    WorldPosition = None;
    Visual = None;
    Movement = None;
    }

type ComponentSystem =
    | ComponentSystem of Map<ComponentType, seq<Component>>