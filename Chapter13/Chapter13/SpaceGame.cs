using System;
using System.Collections.ObjectModel;
using Chapter13.Components;
using Chapter13.Entities;
using Chapter13.Helpers;
using Chapter13.Managers;
using Chapter13.Systems;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.Collections;
using MonoGame.Extended.Collisions;
using MonoGame.Extended.Collisions.Layers;
using MonoGame.Extended.Collisions.QuadTree;
using MonoGame.Extended.Graphics;
using MonoGame.Extended.Input.InputListeners;

namespace Chapter13;
public class SpaceGame : Game
{
    private readonly GraphicsDeviceManager _graphics;
    private SpriteManager _sprites;
    private GameManager _gameManager;
    private SpriteBatch _spriteBatch;
    private readonly Random _rand = Random.Shared;
    private CollisionComponent _collision;

    private const int MaxShips = 15;
    private const int InitialShips = 3;
    public Bag<ShipEntity> Ships { get; } = [];
    public Pool<ShipEntity> ShipPool { get; } = new(
        createItem: () => new ShipEntity(), 
        resetItem: ship => ship.Reset(), 
        maximum: MaxShips);
    
    public bool CanSpawnMoreShips => Ships.Count < MaxShips;

    public SpaceGame()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    protected override void Initialize()
    {
        base.Initialize();

        _gameManager = new GameManager(this);
        
        RectangleF worldBounds = new(0, 0, _graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight);
        Layer defaultLayer = new(new QuadTreeSpace(worldBounds));
        Layer sensorLayer = new(new QuadTreeSpace(worldBounds));
        _collision = new CollisionComponent(defaultLayer);
        _collision.Add("Sensors", sensorLayer);
        _collision.AddCollisionBetweenLayer(defaultLayer, sensorLayer);
        this.Components.Add(_collision);
        
        this.Components.Add(new SpriteRendererSystem(this));
        this.Components.Add(new SensorRendererSystem(this));
        
        for (int i = 0; i < InitialShips; i++)
        {
            ShipEntity ship = ShipPool.Obtain();
            ship.Attach(new Sprite(_sprites.SolidPixelTexture)
            {
                Color = Color.MediumPurple,
                OriginNormalized = new Vector2(0.5f, 0.5f)
            });
            SensorsComponent sensors = new(ship)
            {
                DetectionRadius = 200f
            };
            ship.Attach(sensors);
            sensorLayer.Space.Insert(sensors);
            
            ship.Initialize(
                x: _rand.Next(32, _graphics.PreferredBackBufferWidth - 32),
                y: _rand.Next(32, _graphics.PreferredBackBufferHeight - 32),
                rotation: MovementHelpers.GetRandomHeadingInRadians());
            
            Ships.Add(ship);
            _collision.Insert(ship);
        }
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);

        Texture2D solidPixelTexture = new(GraphicsDevice, 1, 1);
        solidPixelTexture.SetData([Color.White]);
        
        _sprites = new SpriteManager
        {
            SolidPixelTexture = solidPixelTexture,
            SmallFont = GraphicsDevice.LoadAndBakeFont(size: 10, "Content/DroidSans.ttf"),
            LargeFont = GraphicsDevice.LoadAndBakeFont(size: 18, "Content/DroidSans.ttf"),
        };
    }

    protected override void Update(GameTime gameTime)
    {
        foreach (var ship in Ships)
        {
            ship.Update(gameTime);
        }
        
        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.Black);

        base.Draw(gameTime);
    }
}