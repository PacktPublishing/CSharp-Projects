using Microsoft.Xna.Framework;

namespace Chapter13.Components;

public class HullComponent 
{
    public float TimeUntilCollisionEnabled { get; private set; } = 2f;

    public void Update(GameTime gameTime)
    {
        if (TimeUntilCollisionEnabled > 0)
        {
            TimeUntilCollisionEnabled -= (float)gameTime.ElapsedGameTime.TotalSeconds;
        }
    }
    
    public bool IsCollisionEnabled => TimeUntilCollisionEnabled <= 0;
}