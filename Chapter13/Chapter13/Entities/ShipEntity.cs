using System;
using System.Collections;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using MonoGame.Extended;
using MonoGame.Extended.Collections;
using MonoGame.Extended.Collisions;

namespace Chapter13.Entities;

public class ShipEntity : Updateable, ICollisionActor
{
    private CircleF _bounds = new(Vector2.Zero, 1f);
    private CircleF _detection = new(Vector2.Zero, 1f);

    private readonly Bag<IUpdateable> _updateables = [];
    public Bag<object> Components { get; } = [];

    public float MaxSpeed { get; set; } = 10f;
    
    public float DetectionRadius { get; set; } = 100f;

    public IEnumerable<ShipEntity> DetectedShips { get; set; } = [];

    public override void Update(GameTime gameTime)
    {
        float moveAmount = MaxSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
        Vector2 forward = new((float)Math.Cos(Transform.Rotation), (float)Math.Sin(Transform.Rotation));
        Transform.Position += forward * moveAmount;
        _bounds = _bounds with { Center = Transform.Position, Radius = Transform.Scale.X / 2f};
        _detection = _detection with { Center = Transform.Position, Radius = DetectionRadius};
        
        foreach (var updateable in _updateables)
        {
            updateable.Update(gameTime);
        }
    }

    public void Attach(object component)
    {
        Components.Add(component);
        if (component is IUpdateable u)
        {
            _updateables.Add(u);
        } 
    }

    public void Reset()
    {
        Components.Clear();
        Transform.Position = Vector2.Zero;
        Transform.Rotation = 0f;
        Transform.Scale = Vector2.One;
    }

    public Transform2 Transform { get; } = new();

    public void OnCollision(CollisionEventArgs collisionInfo)
    {
        // No operation needed
    }

    public IShapeF Bounds => _bounds;
    public IShapeF DetectionBounds => _detection;

    public void Initialize(int x, int y, float rotation)
    {
        Transform.Position = new Vector2(x, y);
        Transform.Rotation = rotation;
        Transform.Scale = new Vector2(16, 16);
    }
}