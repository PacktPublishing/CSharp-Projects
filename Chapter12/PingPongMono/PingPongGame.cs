using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using PingPongMono.Components;
using PingPongMono.Entities;
using PingPongMono.Helpers;
using PingPongMono.Systems;

namespace PingPongMono;

public class PingPongGame : Game
{
    private readonly GraphicsDeviceManager _graphics;
    private SpriteBatch? _spriteBatch;
    private const string FontPath = "Content/DroidSans.ttf";
    private int Width => _graphics.PreferredBackBufferWidth;
    private int Height => _graphics.PreferredBackBufferHeight;

    private PingPongContext? _context;
    private readonly World _world = new();

    private const int BallSize = 10;
    private const int PaddleWidth = 10;
    private const int PaddleHeight = 60;
    private const int PaddleSpeed = 7;
    private const int BallSpeed = 4;
    private const int SmallFontSize = 16;
    private const int LargeFontSize = 32;

    public PingPongGame()
    {
        _graphics = new GraphicsDeviceManager(this)
        {
            PreferredBackBufferWidth = 800,
            PreferredBackBufferHeight = 480
        };
        Content.RootDirectory = "Content";
        IsMouseVisible = false;
    }

    protected override void Initialize()
    {
        int halfWidth = Width / 2;
        int halfHeight = Height / 2;
        
        // Entities with components
        Ball ball = new(halfWidth, halfHeight, BallSize, BallSpeed);
        ball.Add(new RectangleRendererComponent(Color.White));

        Paddle paddle1 = new(30, halfHeight, PaddleWidth, PaddleHeight);
        paddle1.Add(new RectangleRendererComponent(Color.MediumPurple));
        paddle1.Add(new PaddleKeyboardControlComponent(Keys.Up, Keys.Down, PaddleSpeed));

        Paddle paddle2 = new(Width - 30, halfHeight, PaddleWidth, PaddleHeight);
        paddle2.Add(new RectangleRendererComponent(Color.Yellow));
        paddle2.Add(new PaddleAiControlComponent(PaddleSpeed));
        
        _world.Add(ball, paddle1, paddle2);
        
        // Systems
        ScoreManager score = new();
        PaddleCollisionSystem collision = new(paddle1, paddle2, ball);
        ExitGameKeyHandlerSystem exit = new(this);
        _world.Add(score, collision, exit);
        
        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);
        
        Texture2D whiteTexture = new(GraphicsDevice, 1, 1);
        whiteTexture.SetData([Color.White]);
        
        _context = new PingPongContext
        {
            World = _world,
            Width = Width,
            Height = Height,
            WhitePixel = whiteTexture,
            Keys = Keyboard.GetState(),
            DeltaTime = 0f,
            SmallFont = GraphicsDevice.LoadAndBakeFont(FontPath, SmallFontSize),
            LargeFont = GraphicsDevice.LoadAndBakeFont(FontPath, LargeFontSize),
        };
    }

    protected override void Update(GameTime gameTime)
    {
        // Update context with the current state
        _context!.Keys = Keyboard.GetState();
        _context.DeltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
        _context.Width = Width;
        _context.Height = Height;

        foreach (var entity in _world.Updateable)
        {
            entity.Update(_context!);
        }

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.Black);
        _spriteBatch!.Begin();

        foreach (var entity in _world.Drawable)
        {
            entity.Draw(_spriteBatch, _context!);
        }

        _spriteBatch.End();
        base.Draw(gameTime);
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            _spriteBatch?.Dispose();
            _context?.WhitePixel.Dispose();
        }
        base.Dispose(disposing);
    }
}
