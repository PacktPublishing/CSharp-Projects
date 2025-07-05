using System.Collections.Generic;
using System.Linq;

namespace PingPongMono;

public class World
{
    private readonly List<object> _tracked = [];

    public void Add(params object[] objects) => _tracked.AddRange(objects);

    public IEnumerable<IUpdateable> Updateable 
        => _tracked.OfType<IUpdateable>();
    public IEnumerable<IDrawable> Drawable 
        => _tracked.OfType<IDrawable>();
    public IEnumerable<T> FindEntities<T>() 
        => _tracked.OfType<T>();
}