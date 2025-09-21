using Chapter13.Entities;
using Microsoft.Xna.Framework;
using System.Linq;

namespace Chapter13.Behaviors.Combat;

public class SetTargetBehavior : IBehavior
{
    public bool CanExecute(SpaceEntityBase entity, GameTime time)
    {
        return entity.Target is null && entity.DetectedEntities.OfType<ShipEntity>().Any();
    }

    public void Execute(SpaceEntityBase entity, GameTime time)
    {
        entity.Target = entity.DetectedEntities.OfType<ShipEntity>().First();
    }
}
