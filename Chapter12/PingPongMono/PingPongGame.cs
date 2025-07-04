using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace PingPongMono;

public class PingPongGame : Game
{
    private readonly GraphicsDeviceManager _graphics;
    private SpriteBatch? _spriteBatch;
    private int Width => _graphics.PreferredBackBufferWidth;
    private int Height => _graphics.PreferredBackBufferHeight;
    
    private Paddle? _paddle1;
    private Paddle? _paddle2;
    private Ball? _ball;
    private int _score1;
    private int _score2;

    private Texture2D? _whiteTexture;
    //private SpriteFont _font;

    private const int PaddleWidth = 10;
    private const int PaddleHeight = 60;

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
        
        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);
        //_font = Content.Load<SpriteFont>("ScoreFont");

        _whiteTexture = new Texture2D(GraphicsDevice, 1, 1);
        _whiteTexture.SetData([Color.White]);
    }

    protected override void Update(GameTime gameTime)
    {
        KeyboardState keyboard = Keyboard.GetState();
        float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

        _paddle1!.Update(keyboard, Height, deltaTime);
        _paddle2!.Update(keyboard, Height, deltaTime);
        _ball!.Update(Width, Height, deltaTime);

        // Ball collision with paddles
        if (_ball.Bounds.Intersects(_paddle1.Bounds) && _ball.IsFacingLeft)
        {
            _ball.FlipHorizontal();
        }

        if (_ball.Bounds.Intersects(_paddle2.Bounds) && _ball.IsFacingRight)
        {
            _ball.FlipHorizontal();
        }

        // Score
        if (_ball.Bounds.X < 0)
        {
            _score2++;
            _ball.Reset(Width, Height);
        }
        if (_ball.Bounds.X > Width)
        {
            _score1++;
            _ball.Reset(Width, Height);
        }

        if (keyboard.IsKeyDown(Keys.Escape))
        {
            Exit();
        }

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.Black);

        _spriteBatch!.Begin();

        _paddle1!.Draw(_spriteBatch, _whiteTexture!);
        _paddle2!.Draw(_spriteBatch, _whiteTexture!);
        _ball!.Draw(_spriteBatch, _whiteTexture!);

        // Draw score
        //_spriteBatch.DrawString(_font, $"{_score1}   {_score2}", new Vector2(370, 20), Color.White);

        _spriteBatch.End();

        base.Draw(gameTime);
    }
}