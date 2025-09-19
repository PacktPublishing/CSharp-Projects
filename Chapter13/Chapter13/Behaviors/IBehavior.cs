using Chapter13.Entities;
using Microsoft.Xna.Framework;

namespace Chapter13.Behaviors;

public interface IBehavior
{
    public bool CanExecute(ShipEntity ship);
    public void Execute(ShipEntity ship, GameTime time);
}
