using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PingPongMono.Components;

namespace PingPongMono.Entities;

public abstract class PingPongEntity(Rectangle bounds) : IPingPongEntity
{
    public Rectangle Bounds { get; set; } = bounds;

    private readonly List<IPingPongComponent> _components = [];

    public void Add(IPingPongComponent component) => _components.Add(component);

    public virtual void Draw(SpriteBatch spriteBatch, PingPongContext context)
    {
        foreach (var component in _components)
        {
            component.Draw(this, spriteBatch, context);
        }
    }

    public virtual void Update(PingPongContext context)
    {
        foreach (var component in _components)
        {
            component.Update(this, context);
        }
    }
}