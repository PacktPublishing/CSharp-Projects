using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.ECS;
using MonoGame.Extended.ECS.Systems;
using MonoGame.Extended.Graphics;

namespace Chapter13.Systems;

public class RenderSystem(GraphicsDevice graphics) 
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
        graphics.Clear(Color.Black);
        _spriteBatch.Begin();
        
        foreach (int entityId in ActiveEntities)
        {
            Sprite sprite = _spriteMapper.Get(entityId);
            Transform2 transform = _transformMapper.Get(entityId);
            sprite.Draw(_spriteBatch, transform.Position, 0f, transform.Scale);
        }

        _spriteBatch.End();
    }
}