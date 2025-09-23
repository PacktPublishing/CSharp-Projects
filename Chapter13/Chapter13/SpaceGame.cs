using Chapter13.Behaviors;
using Chapter13.Behaviors.Combat;
using Chapter13.Behaviors.Waypoints;
using Chapter13.Entities;
using Chapter13.Helpers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.Collections;
using MonoGame.Extended.Collisions;
using MonoGame.Extended.Graphics;
using System;
using System.Linq;

namespace Chapter13;

public class SpaceGame : Game
{
    private readonly GraphicsDeviceManager _graphics;
    private readonly Random _rand = Random.Shared;
    private RectangleF _worldBounds;
    private CollisionComponent _collision;
    private BehaviorTree _shipBehaviors;
    private BehaviorTree _missileBehaviors;
    private Texture2D _background;
    private Texture2D _shipArt;
    private const int DesiredActiveShips = 4;
    public Bag<SpaceEntityBase> Entities { get; } = [];
    private Bag<SpaceEntityBase> _despawn = [];
    private Sprite _missileSprite;
    private SpriteBatch _sb;
    private Sprite _shipSprite;
    private const bool ShowDebugVisuals = false;

    public SpaceGame()
    {
        _graphics = new GraphicsDeviceManager(this);

        // Set the window size
        _graphics.PreferredBackBufferWidth = 1280;
        _graphics.PreferredBackBufferHeight = 800;

        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    protected override void Initialize()
    {
        base.Initialize();

        _worldBounds = new RectangleF(0, 0, _graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight);
        _collision = new CollisionComponent(_worldBounds);
        _shipBehaviors = new BehaviorTree(
            new ClearReachedWaypointBehavior(),
            new SetTargetBehavior(),
            new AttackTargetBehavior(this),
            new SteerAwayFromMissileBehavior(),
            new SteerTowardsTargetBehavior(),
            new SteerTowardsWaypointBehavior(),
            new SetRandomWaypointBehavior(_worldBounds)
        );
        _missileBehaviors = new BehaviorTree(
            new SteerTowardsTargetBehavior(),
            new SetTargetBehavior()
        );
    }

    private void SpawnShip()
    {
        ShipEntity ship = new()
        {
            BehaviorTree = _shipBehaviors,
            Sprite = _shipSprite
        };

        int x = _rand.Next(0, _graphics.PreferredBackBufferWidth);
        int y = _rand.Next(0, _graphics.PreferredBackBufferHeight);
        float heading = MovementHelpers.GetRandomHeadingInRadians();

        ship.Initialize(x, y, heading);

        Entities.Add(ship);
        _collision.Insert(ship);
    }

    protected override void LoadContent()
    {
        // Background by leyren at https://opengameart.org/content/starsspace-background
        _background = Content.Load<Texture2D>("Starset");

        // Ship art by Master484 at https://opengameart.org/content/1616-ship-collection
        _shipArt = Content.Load<Texture2D>("Ships");
        Texture2DRegion shipTextureRegion = new(_shipArt, new Rectangle(528, 42, 16, 16));
        _shipSprite = new Sprite(shipTextureRegion)
        {
            OriginNormalized = new Vector2(0.5f, 0.5f),
        };

        Texture2DRegion missileTextureRegion = new(_shipArt, new Rectangle(248, 62, 16, 16));
        _missileSprite = new Sprite(missileTextureRegion)
        {
            OriginNormalized = new Vector2(0.5f, 0.5f)
        };

        _sb = new SpriteBatch(GraphicsDevice);

    }

    protected override void Update(GameTime gameTime)
    {
        // Despawn any queued entities.
        // We do this here to avoid modifying Entities while iterating
        // or using a temporary collection each update.
        foreach (var entity in _despawn)
        {
            Despawn(entity);
        }
        _despawn.Clear();

        // Spawn new ships if needed
        if (Entities.OfType<ShipEntity>().Count() < DesiredActiveShips)
        {
            SpawnShip();
        }

        // Run ship AI and movement
        foreach (var entity in Entities)
        {
            entity.DetectedEntities = Entities.Where(s => s != entity && entity.DetectionBounds.Intersects(s.Bounds));
            entity.Update(gameTime);
        }

        _collision.Update(gameTime);

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.Black);
        _sb.Begin();

        // Draw the background
        _sb.Draw(_background, new Rectangle(0, 0, _graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight), Color.White);

        // Render each entity
        foreach (var entity in Entities)
        {
            if (ShowDebugVisuals && entity is ShipEntity ship)
            {
                DrawShipDebugVisuals(entity, ship);
            }

            _sb.Draw(entity.Sprite, entity.Transform);
        }

        _sb.End();
        base.Draw(gameTime);
    }

    private void DrawShipDebugVisuals(SpaceEntityBase entity, ShipEntity ship)
    {
        _sb.DrawCircle(entity.Bounds.Position, entity.DetectionRadius, sides: 32, Color.Yellow);

        if (entity.Target is not null)
        {
            _sb.DrawLine(entity.Bounds.Position, entity.Target.Transform.Position, Color.OrangeRed, 2);
        }
        if (ship.Waypoint is not null)
        {
            _sb.DrawLine(entity.Bounds.Position, ship.Waypoint.Value, Color.CornflowerBlue, 2);
        }
    }

    public void SpawnMissile(SpaceEntityBase attacker, ShipEntity target)
    {
        MissileEntity missile = new(this, attacker)
        {
            BehaviorTree = _missileBehaviors,
            Sprite = _missileSprite
        };

        Transform2 trans = attacker.Transform;
        Vector2 pos = trans.Position;

        // Set the missile's rotation to face the target
        float angleToTarget = (float)Math.Atan2(
            target.Transform.Position.Y - pos.Y,
            target.Transform.Position.X - pos.X);

        missile.Initialize((int)pos.X, (int)pos.Y, angleToTarget);
        missile.Target = target;
        _collision.Insert(missile);

        Entities.Add(missile);
    }

    public void QueueDespawn(SpaceEntityBase entity)
    {
        _despawn.Add(entity);
    }

    private void Despawn(SpaceEntityBase entity)
    {
        _collision.Remove(entity);
        Entities.Remove(entity);

        // Clear any references to this entity from other entities
        foreach (var other in Entities)
        {
            if (other.Target == entity)
            {
                other.Target = null;
            }
        }
    }
}