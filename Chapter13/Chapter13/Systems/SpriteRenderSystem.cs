using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.ECS;
using MonoGame.Extended.ECS.Systems;
using MonoGame.Extended.Graphics;

namespace Chapter13.Systems;

public class SpriteRenderSystem(GraphicsDevice graphics) 
    : EntityDrawSystem(Aspect.All(typeof(Sprite), typeof(Transform2)))
{
    private readonly SpriteBatch _spriteBatch = new(graphics);
    private ComponentMapper<Sprite> _spriteMapper;
    private ComponentMapper<Transform2> _transformMapper;

    public override void Initialize(IComponentMapperService mapperService)
    {
        _spriteMapper = mapperService.GetMapper<Sprite>();
        _transformMapper = mapperService.GetMapper<Transform2>();
    }

    public override void Draw(GameTime gameTime)
    {        
        _spriteBatch.Begin();
        
        foreach (int entityId in ActiveEntities)
        {
            Sprite sprite = _spriteMapper.Get(entityId);
            Transform2 transform = _transformMapper.Get(entityId);
            Vector2 scale = transform.Scale;
            Vector2 center = new(transform.Position.X - (scale.X / 2), transform.Position.Y - (scale.Y / 2));
            sprite.Draw(_spriteBatch, center, 0f, scale);
        }

        _spriteBatch.End();
    }
}