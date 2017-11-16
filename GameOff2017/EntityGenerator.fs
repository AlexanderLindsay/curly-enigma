module EntityGenerator

open Core.Component.Types
open Core.Component.Functions

type private RectangleSize =
    | RectangleSize of (int*int)

type private EntityColor =
    | EntityColor of (int*int*int*int)

type private EntityType =
    | Rectangle of RectangleSize*EntityColor
    | Texture of string*EntityColor

let private createPositionComponent id position size =
    let (x,y) = position
    let fx = float32 x
    let fy = float32 y
    {
        EntityId = id;
        Position = (fx,fy);
        Collider = Square size
    }

let private createRectangle id size color =
    let (RectangleSize rs) = size
    let (EntityColor ec) = color
    {
        EntityId = id;
        Color = createColor ec;
        Size = rs;
    }
    |> ColoredSquare

let private createTexture id name color =
    let (EntityColor ec) = color
    {
        EntityId = id;
        TextureId = TextureId name;
        Color = createColor ec 
    }
    |> Textured

let private createEntity position size entityType =
    let entityId = UninitalizedEntity
    let positionComp = createPositionComponent entityId position size

    let visualComp =
        match entityType with
        | Rectangle (size, color) ->
            createRectangle entityId size color
        | Texture (name, color) ->
            createTexture entityId name color
    [
        WorldPosition positionComp;
        Visual visualComp
    ]

let simpleEntity (position, collision, size, color) =
    let entityType = Rectangle ((RectangleSize size), (EntityColor color))
    createEntity position collision entityType

let movingEntity (position, size, velocity, texture, color) =
    let entityType = Texture (texture, color |> EntityColor)
    let movement =
        {
        EntityId = UninitalizedEntity
        Velocity = velocity
        } |> Movement
    movement :: createEntity position size entityType

let texturedEntity (position, size, texture, color) =
    let entityType = Texture (texture, color |> EntityColor)
    createEntity position size entityType

let initalizeEntities index (components: Component list) =
    let entityId = EntityId index
    components
    |> List.map (fun comp ->
        comp.UpdateId entityId
    )