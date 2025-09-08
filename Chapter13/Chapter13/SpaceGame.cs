using Chapter13.Entities;
using Chapter13.Helpers;
using Chapter13.Managers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Collections;
using MonoGame.Extended.Graphics;

namespace Chapter13;
public class SpaceGame : Game
{
    private readonly GraphicsDeviceManager _graphics;
    private SpriteManager _sprites;
    private GameManager _gameManager;
    private SpriteBatch _spriteBatch;

    private const int MaxShips = 15;
    private const int InitialShips = 4;
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
        
        for (int i = 0; i < InitialShips; i++)
        {
            ShipEntity ship = ShipPool.Obtain();
            ship.Sprite = new Sprite(_sprites.SolidPixelTexture)
            {
                Color = Color.MediumPurple
            };
            Ships.Add(ship);
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

        _spriteBatch.Begin();
        foreach (var ship in Ships)
        {
            ship.Draw(_spriteBatch, gameTime);
        }
        _spriteBatch.End();
        
        base.Draw(gameTime);
    }
}