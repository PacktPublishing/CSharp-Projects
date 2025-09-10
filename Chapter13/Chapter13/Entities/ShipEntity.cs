using System;
using System.Linq;
using Microsoft.Xna.Framework;
using MonoGame.Extended;
using MonoGame.Extended.Collections;

namespace Chapter13.Entities;

public class ShipEntity
{
    public Bag<object> Components { get; } = [];

    public float MaxSpeed { get; set; } = 10f;
    
    public void Update(GameTime gameTime)
    {
        float moveAmount = MaxSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
        Vector2 forward = new((float)Math.Cos(Transform.Rotation), (float)Math.Sin(Transform.Rotation));
        Transform.Position += forward * moveAmount;
        Bounds = Bounds with { Center = Transform.Position, Radius = Transform.Scale.X / 2f};
    }

    public void Attach(object component)
    {
        Components.Add(component);
    }

    public void Reset()
    {
        Components.Clear();
        Transform.Position = Vector2.Zero;
        Transform.Rotation = 0f;
        Transform.Scale = Vector2.One;
    }

    public Transform2 Transform { get; } = new();

    public CircleF Bounds { get; private set; } = new(Vector2.Zero, 1f);

    public void Initialize(int x, int y, float rotation)
    {
        Transform.Position = new Vector2(x, y);
        Transform.Rotation = rotation;
        Transform.Scale = new Vector2(16, 16);
    }
}