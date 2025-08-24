using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.ECS;
using MonoGame.Extended.ECS.Systems;

namespace Chapter13.Systems;

public class ExitWhenKeypressedSystem(Game game) : IUpdateSystem
{
    public void Dispose()
    {
    }

    public void Initialize(World world)
    {
    }

    public void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
        {
            game.Exit();
        }    
    }
}