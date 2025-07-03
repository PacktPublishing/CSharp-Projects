using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace PingPongMono;

public class PingPongGame : Game
{
    private readonly GraphicsDeviceManager _graphics;
    private SpriteBatch? _spriteBatch;

    private Paddle _paddle1;
    private Paddle _paddle2;
    private Rectangle _ball;
    private Vector2 _ballVelocity;
    private int _score1;
    private int _score2;

    private Texture2D? _whiteTexture;
    //private SpriteFont _font;

    private const int PaddleWidth = 10;
    private const int PaddleHeight = 60;
    private const int BallSize = 10;
    private const int BallSpeed = 4;

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
        _paddle1 = new Paddle(30, 210, PaddleWidth, PaddleHeight)
        {
            UpKey = Keys.W,
            DownKey = Keys.S,
            Color = Color.MediumPurple
        };
        _paddle2 = new Paddle(Width - 30, 210, PaddleWidth, PaddleHeight)
        {
            UpKey = Keys.Up,
            DownKey = Keys.Down,
            Color = Color.AliceBlue
        };
        _ball = new Rectangle(395, 235, BallSize, BallSize);
        _ballVelocity = new Vector2(BallSpeed, BallSpeed);
        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);
        //_font = Content.Load<SpriteFont>("ScoreFont");

        _whiteTexture = new Texture2D(GraphicsDevice, 1, 1);
        _whiteTexture.SetData([Color.White]);
    }

    private int Width => _graphics.PreferredBackBufferWidth;
    private int Height => _graphics.PreferredBackBufferHeight;

    protected override void Update(GameTime gameTime)
    {
        KeyboardState keyboard = Keyboard.GetState();

        _paddle1.Update(keyboard, Height);
        _paddle2.Update(keyboard, Height);

        // Move ball
        _ball.X += (int)_ballVelocity.X;
        _ball.Y += (int)_ballVelocity.Y;

        // Ball collision with top/bottom
        if (_ball.Y <= 0 || _ball.Y >= Height - BallSize)
        {
            _ballVelocity.Y *= -1;
        }

        // Ball collision with paddles
        if (_ball.Intersects(_paddle1.Bounds) && _ballVelocity.X < 0)
        {
            _ballVelocity.X *= -1;
        }

        if (_ball.Intersects(_paddle2.Bounds) && _ballVelocity.X > 0)
        {
            _ballVelocity.X *= -1;
        }

        // Score
        if (_ball.X < 0)
        {
            _score2++;
            ResetBall();
        }

        if (_ball.X > _graphics.PreferredBackBufferWidth)
        {
            _score1++;
            ResetBall();
        }

        if (keyboard.IsKeyDown(Keys.Escape))
        {
            Exit();
        }

        base.Update(gameTime);
    }

    private void ResetBall()
    {
        _ball.X = Width / 2 - BallSize / 2;
        _ball.Y = Height / 2 - BallSize / 2;

        bool isGoingLeft = Random.Shared.Next(2) == 0;
        bool isGoingUp = Random.Shared.Next(2) == 0;

        _ballVelocity = new Vector2(BallSpeed * (isGoingLeft ? -1 : 1),
            BallSpeed * (isGoingUp ? -1 : 1));
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.Black);

        _spriteBatch!.Begin();

        _paddle1.Draw(_spriteBatch, _whiteTexture!);
        _paddle2.Draw(_spriteBatch, _whiteTexture!);
        _spriteBatch.Draw(_whiteTexture, _ball, Color.Yellow);

        // Draw score
        //_spriteBatch.DrawString(_font, $"{_score1}   {_score2}", new Vector2(370, 20), Color.White);

        _spriteBatch.End();

        base.Draw(gameTime);
    }
}