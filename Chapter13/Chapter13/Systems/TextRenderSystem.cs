using Chapter13.Components;
using Chapter13.Managers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.ECS;
using MonoGame.Extended.ECS.Systems;

namespace Chapter13.Systems;

public class TextRenderSystem(GraphicsDevice graphics, SpriteManager sprites) 
    : EntityDrawSystem(Aspect.All(typeof(TextLabelComponent), typeof(Transform2)))
{
    private readonly SpriteBatch _spriteBatch = new(graphics);
    private ComponentMapper<TextLabelComponent> _textMapper;
    private ComponentMapper<Transform2> _transformMapper;

    public override void Initialize(IComponentMapperService mapperService)
    {
        _textMapper = mapperService.GetMapper<TextLabelComponent>();
        _transformMapper = mapperService.GetMapper<Transform2>();
    }

    public override void Draw(GameTime gameTime)
    {        
        _spriteBatch.Begin();
        
        foreach (int entityId in ActiveEntities)
        {
            TextLabelComponent label = _textMapper.Get(entityId);
            Transform2 transform = _transformMapper.Get(entityId);
            
            Vector2 pos = transform.Position;
            SpriteFont font = sprites.GetFont(label.GameFont);
            Vector2 textSize = font.MeasureString(label.Text);
            
            float yOffset = (transform.Scale.Y / 2) + 1;
            Vector2 renderPos = label.Centered
                ? new Vector2(pos.X - textSize.X / 2, pos.Y + yOffset)
                : new Vector2(pos.X, pos.Y + yOffset);
            
            _spriteBatch.DrawString(font, label.Text, renderPos, label.Color);
        }

        _spriteBatch.End();
    }
}