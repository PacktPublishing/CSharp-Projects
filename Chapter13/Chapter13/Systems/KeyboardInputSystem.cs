using Chapter13.Managers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.ECS.Systems;

namespace Chapter13.Systems;

public class KeyboardInputSystem(GameManager game) : UpdateSystem
{
    public override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
        {
            game.Exit();
        }
    }
}