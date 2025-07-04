using System.Collections.Generic;
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
    private readonly List<IUpdateable> _updatable = [];
    private readonly List<IDrawable> _drawable = [];

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
        
        // Entities + Components
        Ball ball = new(halfWidth, halfHeight, BallSize, BallSpeed);

        var paddle1 = new Paddle(30, halfHeight, PaddleWidth, PaddleHeight, Color.MediumPurple)
            //.AddComponent(new PaddleKeyboardControlComponent(Keys.W, Keys.S, PaddleSpeed))
            .AddComponent(new PaddleAiControlComponent(ball, PaddleSpeed));

        var paddle2 = new Paddle(Width - 30, halfHeight, PaddleWidth, PaddleHeight, Color.Yellow)
            .AddComponent(new PaddleKeyboardControlComponent(Keys.Up, Keys.Down, PaddleSpeed));
        
        // Systems
        ScoreManager score = new(ball);
        PaddleCollisionSystem collision = new(paddle1, paddle2, ball);
        ExitGameKeyHandlerSystem exit = new(this);
        
        Register(paddle1, paddle2, ball, score, collision, exit);
        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);
        
        Texture2D whiteTexture = new(GraphicsDevice, 1, 1);
        whiteTexture.SetData([Color.White]);
        
        _context = new PingPongContext
        {
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

        foreach (var entity in _updatable)
        {
            entity.Update(_context!);
        }

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.Black);
        _spriteBatch!.Begin();

        foreach (var entity in _drawable)
        {
            entity.Draw(_spriteBatch, _context!);
        }

        _spriteBatch.End();
        base.Draw(gameTime);
    }
    
    private void Register(params object[] objs)
    {
        foreach (var obj in objs)
        {
            if (obj is IUpdateable updateable)
            {
                _updatable.Add(updateable);
            }
            if (obj is IDrawable drawable)
            {
                _drawable.Add(drawable);
            }
        }
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
