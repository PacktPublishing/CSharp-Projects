using PingPongMono.Entities;

namespace PingPongMono.Components;

public interface IPingPongComponent
{
    void Update(IPingPongEntity entity, PingPongContext context);
}