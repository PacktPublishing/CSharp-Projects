using System.IO;
using Microsoft.Xna.Framework.Graphics;
using SpriteFontPlus;

namespace Chapter13.Helpers;

public static class FontHelpers
{
    public static SpriteFont LoadAndBakeFont(this GraphicsDevice graphics, int size, string path)
    {
        byte[] bytes = File.ReadAllBytes(path);
        CharacterRange[] ranges = [
            CharacterRange.BasicLatin
        ];
        
        TtfFontBakerResult bakeResult = TtfFontBaker.Bake(bytes, size, 512, 512, ranges);
        return bakeResult.CreateSpriteFont(graphics);
    }
}