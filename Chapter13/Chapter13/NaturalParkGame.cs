using Chapter13.Systems;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.ECS;
using MonoGame.Extended.Graphics;

namespace Chapter13;
public class NaturalParkGame : Game
{
    private readonly GraphicsDeviceManager _graphics;
    private Texture2D _solidPixelTexture;

    public NaturalParkGame()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    protected override void Initialize()
    {
        base.Initialize();

        World world = new WorldBuilder()
            .AddSystem(new MapGeneratorSystem(_solidPixelTexture))
            .AddSystem(new RenderSystem(GraphicsDevice))
            .AddSystem(new ExitWhenKeypressedSystem(this))
            .Build();

        Components.Add(world);
    }

    protected override void LoadContent()
    {
        _solidPixelTexture = new Texture2D(GraphicsDevice, 1, 1);
        _solidPixelTexture.SetData([Color.White]);
    }
}