using PingPongMono.Entities;

namespace PingPongMono;

public interface IUpdateable
{
    void Update(PingPongContext context);
}