using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SpriteFontPlus;

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
    private SpriteFont? _scoreFont;
    private SpriteFont? _smallFont;

    private const int PaddleWidth = 10;
    private const int PaddleHeight = 60;
    private const int PaddleSpeed = 5;
    private const int BallSize = 10;
    private const int BallSpeed = 7;
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
        int halfX = Width / 2;
        int halfY = Height / 2;
        
        _paddle1 = new Paddle(30, halfY, PaddleWidth, PaddleHeight, PaddleSpeed)
        {
            UpKey = Keys.W,
            DownKey = Keys.S,
            Color = Color.MediumPurple
        };
        int rightX = Width - 30;
        _paddle2 = new Paddle(rightX, halfY, PaddleWidth, PaddleHeight, PaddleSpeed)
        {
            UpKey = Keys.Up,
            DownKey = Keys.Down,
            Color = Color.Yellow
        };
        _ball = new Ball(Width, Height, BallSize, BallSpeed);
        
        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);
        _whiteTexture = new Texture2D(GraphicsDevice, 1, 1);
        _whiteTexture.SetData([Color.White]);
        
        // Load the font with SpriteFontPlus
        _smallFont = LoadAndBakeFont(SmallFontSize);
        _scoreFont = LoadAndBakeFont(LargeFontSize);
    }

    private SpriteFont LoadAndBakeFont(int size)
    {
        byte[] bytes = File.ReadAllBytes("Content/DroidSans.ttf");
        CharacterRange[] ranges = [
            CharacterRange.BasicLatin
        ];
        
        TtfFontBakerResult bakeResult = TtfFontBaker.Bake(bytes, size, 512, 512, ranges);
        return bakeResult.CreateSpriteFont(GraphicsDevice);
    }

    protected override void Update(GameTime gameTime)
    {
        KeyboardState keyboard = Keyboard.GetState();
        float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

        _paddle1!.Update(keyboard, Height, deltaTime);
        _paddle2!.Update(keyboard, Height, deltaTime);
        _ball!.Update(Height, deltaTime);

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
        if (_ball.Bounds.Left < 0)
        {
            _score2++;
            _ball.Reset(Width, Height);
        }
        if (_ball.Bounds.Right > Width)
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
        string scoreText = $"{_score1}   {_score2}";
        Vector2 scoreSize = _scoreFont!.MeasureString(scoreText);
        _spriteBatch.DrawString(_scoreFont, scoreText, new Vector2((Width / 2) - scoreSize.X / 2, 20), Color.White);
        _spriteBatch.DrawString(_smallFont, "Welcome to Ping Pong Mono", new Vector2(10, 10), Color.White);
        _spriteBatch.DrawString(_smallFont, "Press ESC to exit", new Vector2(10, Height - 10 - SmallFontSize), Color.White);
        _spriteBatch.End();

        base.Draw(gameTime);
    }
    
    protected override void UnloadContent()
    {
        _spriteBatch?.Dispose();
        _whiteTexture?.Dispose();
        
        base.UnloadContent();
    }
}