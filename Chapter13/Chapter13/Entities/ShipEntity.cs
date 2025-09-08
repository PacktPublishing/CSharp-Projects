using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Graphics;

namespace Chapter13.Entities;

public class ShipEntity
{
    public void Update(GameTime gameTime)
    {
    }

    public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
    {
        if (Sprite != null)
        {
            spriteBatch.Draw(Sprite, Vector2.Zero, rotation: 0f, scale: new Vector2(16,16));
        }
    }

    public Sprite Sprite { get; set; }

    public void Reset()
    {
        Sprite = null;
    }
}