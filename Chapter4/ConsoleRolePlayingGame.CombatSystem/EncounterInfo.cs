namespace ConsoleRolePlayingGame.CombatSystem;

public class EncounterInfo
{
    public required string Name { get; init; }
    public required List<EncounterEnemyInfo> Enemies { get; init; } = [];
}