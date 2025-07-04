using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using PingPongMono.Entities;
using PingPongMono.Helpers;

namespace PingPongMono;

public class PingPongGame : Game
{
    private readonly GraphicsDeviceManager _graphics;
    private SpriteBatch? _spriteBatch;
    private static readonly string FontPath = "Content/DroidSans.ttf";
    private int Width => _graphics.PreferredBackBufferWidth;
    private int Height => _graphics.PreferredBackBufferHeight;
    
    private Paddle? _paddle1;
    private Paddle? _paddle2;
    private Ball? _ball;
    private ScoreManager? _score;

    private Texture2D? _whiteTexture;
    private SpriteFont? _smallFont;
    private PingPongContext? _context;

    private const int PaddleWidth = 10;
    private const int PaddleHeight = 60;
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
        
        _paddle1 = new Paddle(30, halfHeight, PaddleWidth, PaddleHeight)
        {
            UpKey = Keys.W,
            DownKey = Keys.S,
            Color = Color.MediumPurple
        };
        _paddle2 = new Paddle(Width - 30, halfHeight, PaddleWidth, PaddleHeight)
        {
            UpKey = Keys.Up,
            DownKey = Keys.Down,
            Color = Color.Yellow
        };
        _ball = new Ball(halfWidth, halfHeight);
        _score = new ScoreManager(_ball);
        
        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);
        _whiteTexture = new Texture2D(GraphicsDevice, 1, 1);
        _whiteTexture.SetData([Color.White]);
        
        _smallFont = GraphicsDevice.LoadAndBakeFont(FontPath, SmallFontSize);
        _score!.Font = GraphicsDevice.LoadAndBakeFont(FontPath, LargeFontSize);
        _context = new PingPongContext
        {
            Width = Width,
            Height = Height,
            WhitePixel = _whiteTexture,
            Keys = Keyboard.GetState(),
            DeltaTime = 0f
        };
    }

    protected override void Update(GameTime gameTime)
    {
        // Update context with the current state
        _context!.Keys = Keyboard.GetState();
        _context.DeltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
        _context.Width = Width;
        _context.Height = Height;

        _paddle1!.Update(_context);
        _paddle2!.Update(_context);
        _ball!.Update(_context);
        _score!.Update(_context);

        // Ball collision with paddles
        if (_ball.Bounds.Intersects(_paddle1.Bounds) && _ball.IsFacingLeft)
        {
            _ball.FlipHorizontal();
        }
        if (_ball.Bounds.Intersects(_paddle2.Bounds) && _ball.IsFacingRight)
        {
            _ball.FlipHorizontal();
        }

        if (_context!.Keys.IsKeyDown(Keys.Escape))
        {
            Exit();
        }

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.Black);

        _spriteBatch!.Begin();

        _paddle1!.Draw(_spriteBatch, _context!);
        _paddle2!.Draw(_spriteBatch, _context!);
        _ball!.Draw(_spriteBatch, _context!);
        _score!.Draw(_spriteBatch, _context!);

        _spriteBatch.DrawString(_smallFont, "Welcome to Ping Pong Mono", new Vector2(10, 10), Color.White);
        _spriteBatch.DrawString(_smallFont, "Press ESC to exit", new Vector2(10, Height - 10 - SmallFontSize), Color.White);
        _spriteBatch.End();

        base.Draw(gameTime);
    }
}