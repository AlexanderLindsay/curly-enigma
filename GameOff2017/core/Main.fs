module Core.Main

open Microsoft.Xna.Framework
open Microsoft.Xna.Framework.Graphics

type GameRoot () as gr =
    inherit Game()

    let graphics = new GraphicsDeviceManager(gr)
    let mutable spriteBatch = Unchecked.defaultof<SpriteBatch>
    
    override gr.Initialize() =
        do spriteBatch <- new SpriteBatch(gr.GraphicsDevice)
        do base.Initialize()
        ()
    
    override gr.LoadContent() =
        ()

    override gr.Update (gameTime) =
        ()
    
    override gr.Draw (gameTime) =
        do gr.GraphicsDevice.Clear Color.CornflowerBlue
        ()