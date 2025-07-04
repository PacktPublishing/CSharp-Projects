using System.IO;
using Microsoft.Xna.Framework.Graphics;
using SpriteFontPlus;

namespace PingPongMono.Helpers;

public static class FontHelper
{
    public static SpriteFont LoadAndBakeFont(this GraphicsDevice graphics, string fontPath, int fontSize, int bitmapSize=512)
    {
        byte[] bytes = File.ReadAllBytes(fontPath);
        CharacterRange[] ranges = [
            CharacterRange.BasicLatin
        ];
        
        TtfFontBakerResult bakeResult = TtfFontBaker.Bake(bytes, fontSize, bitmapSize, bitmapSize, ranges);
        return bakeResult.CreateSpriteFont(graphics);
    }
}