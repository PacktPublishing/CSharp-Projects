using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PingPongMono.Components;

namespace PingPongMono.Entities;

public abstract class PingPongEntity(Rectangle bounds) : IPingPongEntity
{
    public Rectangle Bounds { get; set; } = bounds;

    private readonly List<IPingPongComponent> _components = [];

    public PingPongEntity AddComponent(IPingPongComponent component)
    {
        _components.Add(component);
        return this;
    }
    public abstract void Draw(SpriteBatch spriteBatch, PingPongContext context);

    public virtual void Update(PingPongContext context)
    {
        foreach (var component in _components)
        {
            component.Update(this, context);
        }
    }
}