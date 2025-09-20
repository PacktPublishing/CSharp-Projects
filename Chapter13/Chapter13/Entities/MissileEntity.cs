using System;
using Chapter13.Behaviors;
using Microsoft.Xna.Framework;
using MonoGame.Extended;

namespace Chapter13.Entities;

public class MissileEntity : SpaceEntityBase
{
    private CircleF _bounds = new(Vector2.Zero, 1f);

    public float MaxSpeed { get; set; } = 25f;
    public override float MaxTurnRate => 0.75f;
    public BehaviorTree BehaviorTree { get; set; } = new();

    public override void Update(GameTime gameTime)
    {
        BehaviorTree.Execute(this, gameTime);

        // Move forward in current direction
        float moveAmount = MaxSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
        Vector2 forward = new((float)Math.Cos(Transform.Rotation), (float)Math.Sin(Transform.Rotation));

        // Update position and associated bounds
        Transform.Position += forward * moveAmount;
        MaintainBoundsAndDetection();
    }

    public void Reset()
    {
        Transform.Position = Vector2.Zero;
        Transform.Rotation = 0f;
        Transform.Scale = Vector2.One;
        BehaviorTree.Clear();
    }

    public override IShapeF Bounds => _bounds;

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
    }
}