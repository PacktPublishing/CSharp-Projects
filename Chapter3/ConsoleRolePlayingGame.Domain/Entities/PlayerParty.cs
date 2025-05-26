using ConsoleRolePlayingGame.Overworld.Entities;
using ConsoleRolePlayingGame.Overworld.Structure;

namespace ConsoleRolePlayingGame.Domain.Entities;

public class PlayerParty : IMapEntity
{
    public EntityType EntityType => EntityType.Player;
    public Pos MapPos { get; set; } = new(0, 0);
    public string Name { get; init; } = "The Party";
}