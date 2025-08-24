using Chapter13.Components;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.ECS;
using MonoGame.Extended.ECS.Systems;
using MonoGame.Extended.Graphics;

namespace Chapter13.Systems;

public class MapGeneratorSystem(Texture2D grassTexture) : UpdateSystem
{
    private World _world;
    private bool _generated;
    private readonly Vector2 LevelSize = new(24, 24);
    private readonly Vector2 TileSizes = new(16, 16); // TODO: Should be able to look at the bounds of grassTexture

    public override void Initialize(World world)
    {
        _world = world;
        base.Initialize(world);
    }

    public override void Update(GameTime gameTime)
    {
        if (_generated) return;

        for (int y = 0; y < LevelSize.Y; y++)
        {
            for (int x = 0; x < LevelSize.X; x++)
            {
                Entity tile = _world.CreateEntity();
                tile.Attach(new Transform2(x * TileSizes.X, y * TileSizes.Y)
                {
                    Scale = TileSizes // TODO: Shouldn't be necessary if the texture is already the right size
                });
                tile.Attach(new Sprite(grassTexture)
                {
                    Color = Color.DarkGreen
                });
                tile.Attach(new GrassStatusComponent());
            }
        }

        _generated = true;
    }
}