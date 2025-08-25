using Chapter13.Components;
using Chapter13.Managers;
using Microsoft.Xna.Framework;
using MonoGame.Extended.ECS;
using MonoGame.Extended.ECS.Systems;

namespace Chapter13.Systems;

public class ShipTrackerSystem(GameManager game) : EntityUpdateSystem(Aspect.All(typeof(TrackedShipComponent)))
{
    public override void Initialize(IComponentMapperService mapperService)
    {
        // No initialization needed
    }

    public override void Update(GameTime gameTime)
    {
        game.TrackedShipsCount = ActiveEntities.Count;
    }
}