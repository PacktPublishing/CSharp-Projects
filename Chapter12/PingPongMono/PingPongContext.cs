using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace PingPongMono;

public record PingPongContext
{
    public float DeltaTime { get; set; }
    public KeyboardState Keys { get; set; }
    public int Width { get; set; }
    public int Height { get; set; }
    public required Texture2D WhitePixel { get; init; }
    public required SpriteFont SmallFont { get; init; }
    public required SpriteFont LargeFont { get; init; }
    public required World World { get; init; }
}