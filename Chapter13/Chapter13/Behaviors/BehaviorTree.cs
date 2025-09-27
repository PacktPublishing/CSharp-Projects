using Chapter13.Entities;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Linq;

namespace Chapter13.Behaviors;

public class BehaviorTree(params IBehavior[] behaviors) : IBehavior
{
    public IEnumerable<IBehavior> Behaviors { get; } = [..behaviors];

    public bool CanExecute(SpaceEntityBase ship, GameTime time) 
        => Behaviors.Any(b => b.CanExecute(ship, time));

    public void Execute(SpaceEntityBase ship, GameTime time)
    {
        foreach (var behavior in Behaviors)
        {
            if (behavior.CanExecute(ship, time))
            {
                behavior.Execute(ship, time);
                return;
            }
        }
    }
}
