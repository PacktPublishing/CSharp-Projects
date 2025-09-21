using Chapter13.Behaviors;
using Microsoft.Xna.Framework;
using MonoGame.Extended;
using MonoGame.Extended.Collisions;
using MonoGame.Extended.Graphics;
using System;
using System.Collections.Generic;

namespace Chapter13.Entities;

public abstract class SpaceEntityBase : IUpdateable, ICollisionActor
{
    public Transform2 Transform { get; } = new();

    public ShipEntity? Target { get; set; }
    public Sprite Sprite { get; internal set; }
    public abstract float MaxTurnRate { get; }
    public abstract float MaxSpeed { get; }
    public abstract float DetectionRadius { get; }

    public BehaviorTree BehaviorTree { get; set; } = new();

    public IShapeF Bounds => _bounds;
    public IShapeF DetectionBounds => _detection;

    public IEnumerable<SpaceEntityBase> DetectedEntities { get; set; } = [];


    public virtual void OnCollision(CollisionEventArgs collisionInfo)
    {
        // No operation needed
    }

    public virtual void Update(GameTime gameTime)
    {
        BehaviorTree.Execute(this, gameTime);

        // Move forward in current direction
        float moveAmount = MaxSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
        Vector2 forward = new((float)Math.Cos(Transform.Rotation), (float)Math.Sin(Transform.Rotation));

        // Update position and associated bounds
        Transform.Position += forward * moveAmount;
        MaintainBoundsAndDetection();
    }

    public void Initialize(int x, int y, float rotation)
    {
        Transform.Position = new Vector2(x, y);
        Transform.Rotation = rotation;
        Transform.Scale = new Vector2(16, 16);
        MaintainBoundsAndDetection();
    }

    private void MaintainBoundsAndDetection()
    {
        _bounds = _bounds with { Center = Transform.Position, Radius = Transform.Scale.X / 2f };
        _detection = _detection with { Center = Transform.Position, Radius = DetectionRadius };
    }

    private CircleF _bounds = new(Vector2.Zero, 1f);
    private CircleF _detection = new(Vector2.Zero, 1f);

    public bool Enabled { get; }
    public int UpdateOrder { get; }
    public event EventHandler<EventArgs> EnabledChanged;
    public event EventHandler<EventArgs> UpdateOrderChanged;
}
