using System;
using Chapter13.Domain;
using Microsoft.Xna.Framework.Graphics;

namespace Chapter13.Managers;

public class SpriteManager
{
    public required Texture2D SolidPixelTexture { get; init; }
    public required SpriteFont SmallFont { get; init; }
    public required SpriteFont LargeFont { get; init; }

    public SpriteFont GetFont(GameFont font) => font switch
        {
            GameFont.Small => SmallFont,
            GameFont.Large => LargeFont,
            _ => throw new ArgumentOutOfRangeException(nameof(font), font, null)
        };
}