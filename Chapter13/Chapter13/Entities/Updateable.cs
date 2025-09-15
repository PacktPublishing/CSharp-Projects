using System;
using Microsoft.Xna.Framework;

namespace Chapter13.Entities;

public class Updateable : IUpdateable
{
    public virtual void Update(GameTime gameTime)
    {
    }

    public bool Enabled { get; }
    public int UpdateOrder { get; }
    public event EventHandler<EventArgs> EnabledChanged;
    public event EventHandler<EventArgs> UpdateOrderChanged;
}