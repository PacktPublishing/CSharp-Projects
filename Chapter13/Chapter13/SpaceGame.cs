using Chapter13.Behaviors;
using Chapter13.Behaviors.Combat;
using Chapter13.Behaviors.Waypoints;
using Chapter13.Entities;
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
    private const int DesiredActiveShips = 4;
    private Bag<SpaceEntityBase> _entities = [];
    private Bag<SpaceEntityBase> _despawn = [];
    private SpriteBatch _sb;
    private AnimatedSprite _shipSprite;
    private AnimatedSprite _missileSprite;
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

    protected override void LoadContent()
    {
        // Background by leyren at https://opengameart.org/content/starsspace-background
        _background = Content.Load<Texture2D>("Starset");

        // Ship art created by Matt Eland for this book
        _shipSprite = LoadAnimatedSprite("FighterShip");

        // Missile art created by Matt Eland for this book
        _missileSprite = LoadAnimatedSprite("Missile", pingPong: true);

        _sb = new SpriteBatch(GraphicsDevice);
    }

    private AnimatedSprite LoadAnimatedSprite(string texture, bool pingPong = false)
    {
        Texture2D tex = Content.Load<Texture2D>(texture);
        Texture2DAtlas atlas = Texture2DAtlas.Create("Atlas", tex, 16, 16);
        SpriteSheet sheet = new("spriteSheet", atlas);
        sheet.DefineAnimation("Default", builder =>
        {
            builder.IsLooping(true);

            if (pingPong)
            {
                builder.IsPingPong(true);
            }

            for (int i = 0; i < atlas.RegionCount; i++)
            {
                builder.AddFrame(i, TimeSpan.FromSeconds(1.0 / 30));
            }
        });

        AnimatedSprite sprite = new(sheet, "Default");
        sprite.OriginNormalized = new Vector2(0.5f, 0.5f);

        return sprite;
    }

    protected override void Update(GameTime gameTime)
    {
        // Update our animations
        _shipSprite.Update(gameTime);
        _missileSprite.Update(gameTime);

        // Despawn any queued entities.
        // We do this here to avoid modifying Entities while iterating
        // or using a temporary collection each update.
        foreach (var entity in _despawn)
        {
            Despawn(entity);
        }
        _despawn.Clear();

        // Spawn new ships if needed
        if (_entities.OfType<ShipEntity>().Count() < DesiredActiveShips)
        {
            SpawnShip();
        }

        // Run ship AI and movement
        foreach (var entity in _entities)
        {
            entity.DetectedEntities = _entities.Where(s => s != entity && entity.DetectionBounds.Intersects(s.Bounds));
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
        _sb.Draw(_background, _worldBounds.ToRectangle(), Color.White);

        // Render each entity
        foreach (var entity in _entities)
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

    private void SpawnShip()
    {
        ShipEntity ship = new()
        {
            BehaviorTree = _shipBehaviors,
            Sprite = _shipSprite
        };

        int x = _rand.Next(0, _graphics.PreferredBackBufferWidth);
        int y = _rand.Next(0, _graphics.PreferredBackBufferHeight);
        float heading = MathHelper.ToRadians(Random.Shared.Next(360));

        ship.Initialize(x, y, heading);

        _entities.Add(ship);
        _collision.Insert(ship);
    }

    public void SpawnMissile(SpaceEntityBase attacker, SpaceEntityBase target)
    {
        MissileEntity missile = new(this, attacker)
        {
            BehaviorTree = _missileBehaviors,
            Sprite = _missileSprite,
            Target = target
        };

        Transform2 trans = attacker.Transform;
        Vector2 pos = trans.Position;
        missile.Initialize((int)pos.X, (int)pos.Y, trans.Rotation);
        _collision.Insert(missile);

        _entities.Add(missile);
    }

    public void QueueDespawn(SpaceEntityBase entity)
    {
        _despawn.Add(entity);
    }

    private void Despawn(SpaceEntityBase entity)
    {
        _collision.Remove(entity);
        _entities.Remove(entity);

        entity.Target = null;
        entity.DetectedEntities = [];

        // Clear any references to this entity from other entities
        foreach (var other in _entities)
        {
            if (other.Target == entity)
            {
                other.Target = null;
            }
        }
    }
}