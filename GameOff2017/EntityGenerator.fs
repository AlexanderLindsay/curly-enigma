module EntityGenerator

open Core.Component.Types
open Core.Component.Functions

type private RectangleSize =
    | RectangleSize of (int*int)

type private RectangleColor =
    | RectangleColor of (int*int*int*int)

type private EntityType =
    | Rectangle of RectangleSize*RectangleColor
    | Texture of string

let private createPositionComponent id position =
    let (x,y) = position
    let fx = float32 x
    let fy = float32 y
    {
        EntityId = id;
        Position = (fx,fy);
    }

let private createRectangle id size color =
    let (RectangleSize rs) = size
    let (RectangleColor rc) = color
    {
        EntityId = id;
        Color = createColor rc;
        Size = rs;
    }
    |> ColoredSquare

let private createTexture id name =
    {
        EntityId = id;
        TextureId = TextureId name;
    }
    |> Textured

let private createEntity position entityType =
    let entityId = UninitalizedEntity
    let positionComp = createPositionComponent entityId position

    let visualComp =
        match entityType with
        | Rectangle (size, color) ->
            createRectangle entityId size color
        | Texture name ->
            createTexture entityId name
    [
        WorldPosition positionComp;
        Visual visualComp
    ]

let simpleEntity (position, size, color) =
    let entityType = Rectangle ((RectangleSize size), (RectangleColor color))
    createEntity position entityType

let movingEntity (position, velocity, texture) =
    let entityType = Texture texture
    let movement =
        {
        EntityId = UninitalizedEntity
        Velocity = velocity
        } |> Movement
    movement :: createEntity position entityType

let texturedEntity (position, texture) =
    let entityType = Texture texture
    createEntity position entityType

let initalizeEntities index (components: Component list) =
    let entityId = EntityId index
    components
    |> List.map (fun comp ->
        comp.UpdateId entityId
    )