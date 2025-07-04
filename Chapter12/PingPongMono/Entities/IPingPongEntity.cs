using Microsoft.Xna.Framework;

namespace PingPongMono.Entities;

public interface IPingPongEntity : IDrawable, IUpdateable
{
    Rectangle Bounds { get; set; }
}