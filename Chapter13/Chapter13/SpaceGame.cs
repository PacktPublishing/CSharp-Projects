using Chapter13.Helpers;
using Chapter13.Managers;
using Chapter13.Systems;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.ECS;

namespace Chapter13;
public class SpaceGame : Game
{
    private readonly GraphicsDeviceManager _graphics;
    private SpriteManager _sprites;
    private GameManager _gameManager;
    private World _world;

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
        
        _world = new WorldBuilder()
            .AddSystem(new SpriteRenderSystem(GraphicsDevice))
            .AddSystem(new TextRenderSystem(GraphicsDevice, _sprites))
            .AddSystem(new MapLoaderSystem(_gameManager, _sprites))
            .AddSystem(new ShipTrackerSystem(_gameManager))
            .AddSystem(new ShipSpawnerSystem(_gameManager, _sprites))
            .AddSystem(new KeyboardInputSystem(_gameManager))
            .Build();
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
    }

    protected override void Update(GameTime gameTime)
    {
        _world.Update(gameTime);
        
        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.Black);
        _world.Draw(gameTime);
        
        base.Draw(gameTime);
    }
}