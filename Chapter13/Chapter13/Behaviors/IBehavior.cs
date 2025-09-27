using Chapter13.Entities;
using Microsoft.Xna.Framework;

namespace Chapter13.Behaviors;

public interface IBehavior
{
    public bool CanExecute(SpaceEntityBase ship, GameTime time);
    public void Execute(SpaceEntityBase ship, GameTime time);
}
