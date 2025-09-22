using System;
using System.Linq;
using Chapter13.Behaviors.Combat;
using Chapter13.Behaviors.Waypoints;
using Chapter13.Entities;
using Chapter13.Helpers;
using Chapter13.Managers;
using Chapter13.Systems;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.Collections;
using MonoGame.Extended.Collisions;
using MonoGame.Extended.Graphics;

namespace Chapter13;

public class SpaceGame : Game
{
    private readonly GraphicsDeviceManager _graphics;
    private SpriteManager _sprites;
    private readonly Random _rand = Random.Shared;
    private CollisionComponent _collision;
    private Texture2D _background;
    private Texture2D _shipArt;
    private Texture2DRegion _shipTextureRegion;
    private Texture2DRegion _missileTextureRegion;
    private const int InitialShips = 3;
    public Bag<SpaceEntityBase> Entities { get; } = [];
    private Bag<SpaceEntityBase> _despawn = [];
    private SpriteBatch _sb;

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

        RectangleF worldBounds = new(0, 0, _graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight);
        _collision = new CollisionComponent(worldBounds);
        this.Components.Add(_collision);

        this.Components.Add(new SpriteRendererSystem(this));
        //this.Components.Add(new SensorRendererSystem(this));
        //this.Components.Add(new WaypointRenderingSystem(this));
        //this.Components.Add(new TargetRenderingSystem(this));

        for (int i = 0; i < InitialShips; i++)
        {
            ShipEntity ship = new();
            ship.Sprite = new Sprite(_shipTextureRegion)
            {
                OriginNormalized = new Vector2(0.5f, 0.5f),
            };
            ship.BehaviorTree.Add(
                new ClearReachedWaypointBehavior(),
                new SetTargetBehavior(),
                new AttackTargetBehavior(this),
                new SteerTowardsTargetBehavior(),
                new SteerTowardsWaypointBehavior(),
                new SetRandomWaypointBehavior(worldBounds)
            );

            ship.Initialize(
                x: _rand.Next(32, _graphics.PreferredBackBufferWidth - 32),
                y: _rand.Next(32, _graphics.PreferredBackBufferHeight - 32),
                rotation: MovementHelpers.GetRandomHeadingInRadians());

            Entities.Add(ship);
            _collision.Insert(ship);
        }
    }

    protected override void LoadContent()
    {
        Texture2D solidPixelTexture = new(GraphicsDevice, 1, 1);
        solidPixelTexture.SetData([Color.White]);

        _sprites = new SpriteManager
        {
            SolidPixelTexture = solidPixelTexture,
            SmallFont = GraphicsDevice.LoadAndBakeFont(size: 10, "Content/DroidSans.ttf"),
            LargeFont = GraphicsDevice.LoadAndBakeFont(size: 18, "Content/DroidSans.ttf"),
        };

        // Background by leyren at https://opengameart.org/content/starsspace-background
        _background = Content.Load<Texture2D>("Starset");

        // Ship art by Master484 at https://opengameart.org/content/1616-ship-collection
        _shipArt = Content.Load<Texture2D>("Ships");
        _shipTextureRegion = new Texture2DRegion(_shipArt, new Rectangle(528, 42, 16, 16));
        _missileTextureRegion = new Texture2DRegion(_shipArt, new Rectangle(248, 62, 16, 16));

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

        foreach (var entity in Entities)
        {
            entity.DetectedEntities = Entities.Where(s => s != entity && entity.DetectionBounds.Intersects(s.Bounds));
            entity.Update(gameTime);
        }

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.Black);

        // Draw the background
        _sb.Begin();
        _sb.Draw(_background, new Rectangle(0, 0, _graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight), Color.White);
        _sb.End();

        base.Draw(gameTime);
    }

    public void SpawnMissile(SpaceEntityBase attacker, ShipEntity target)
    {
        MissileEntity missile = new(this, attacker)
        {
            Sprite = new Sprite(_missileTextureRegion)
            {
                OriginNormalized = new Vector2(0.5f, 0.5f)
            }
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

        missile.BehaviorTree.Add(
            new SteerTowardsTargetBehavior(),
            new SetTargetBehavior()
        );

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