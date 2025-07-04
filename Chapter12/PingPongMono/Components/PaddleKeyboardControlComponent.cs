using Microsoft.Xna.Framework.Input;
using PingPongMono.Entities;

namespace PingPongMono.Components;

public class PaddleKeyboardControlComponent(Keys upKey, Keys downKey, int speed) : IPingPongComponent
{
    public void Update(IPingPongEntity entity, PingPongContext context)
    {
        int delta = (int)(speed * context.DeltaTime * 60);
        int y = entity.Bounds.Y;
        
        if (context.Keys.IsKeyDown(upKey) && y > 0)
        {
            y -= delta;
        }
        else if (context.Keys.IsKeyDown(downKey) && y < context.Height - entity.Bounds.Height)
        {
            y += delta;
        }
        
        entity.Bounds = entity.Bounds with { Y = y };
    }
}