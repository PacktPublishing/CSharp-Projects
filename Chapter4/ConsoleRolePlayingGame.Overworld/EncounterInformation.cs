namespace ConsoleRolePlayingGame.Overworld;

public class EncounterInformation
{
    public required string Name { get; init; }
    public required List<EncounterEnemyInformation> Enemies { get; init; } = [];
}