using System.Diagnostics.CodeAnalysis;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PingPongMono.Entities;

namespace PingPongMono.Systems;

public class ScoreManager(Ball ball) : IPingPongSystem, IDrawable
{
    private int Player1Score { get; set; }
    private int Player2Score { get; set; }

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
        SpriteFont font = context.LargeFont;
        int scoreWidth = (int) font.MeasureString(score).X;
        Vector2 position = new (context.Width / 2 - scoreWidth / 2, 10);
        
        spriteBatch.DrawString(font, score, position, Color.White);
    }
}