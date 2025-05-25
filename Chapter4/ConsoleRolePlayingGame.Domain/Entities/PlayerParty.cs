using ConsoleRolePlayingGame.CombatSystem;
using ConsoleRolePlayingGame.Overworld;

namespace ConsoleRolePlayingGame.Domain.Entities;

public class PlayerParty : IMapEntity, ICombatGroup
{
    public EntityType EntityType => EntityType.Player;
    public Pos MapPos { get; set; } = new(0, 0);
    public string Name { get; init; } = "The Party";
    public required List<Combatant> Members { get; init; }
}