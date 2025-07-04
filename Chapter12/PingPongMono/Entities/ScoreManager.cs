using System.Diagnostics.CodeAnalysis;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PingPongMono.Entities;

public class ScoreManager(Ball ball) : IPingPongEntity
{
    public int Player1Score { get; private set; }
    public int Player2Score { get; private set; }
    public SpriteFont? Font { get; set; }

    public void Update(PingPongContext context)
    {
        if (ball.Bounds.X < 0)
        {
            Player2Score++;
            ball.Reset(context);
        } 
        else if (ball.Bounds.X > context.Width)
        {
            Player1Score++;
            ball.Reset(context);
        }    
    }
    
    [SuppressMessage("ReSharper", "PossibleLossOfFraction", Justification = "Fractional loss is fine since we're converting to pixels")]
    public void Draw(SpriteBatch spriteBatch, PingPongContext context)
    {
        string score = $"{Player1Score}   {Player2Score}";
        int scoreWidth = (int) Font!.MeasureString(score).X;
        Vector2 position = new (context.Width / 2 - scoreWidth / 2, 10);
        
        spriteBatch.DrawString(Font, score, position, Color.White);
    }
}