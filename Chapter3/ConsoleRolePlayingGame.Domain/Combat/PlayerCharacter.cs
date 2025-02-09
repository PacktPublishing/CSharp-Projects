namespace ConsoleRolePlayingGame.Domain.Combat;

public class PlayerCharacter: GameCharacter
{
    public override required string Name { get; init; }
    public override required string[] AsciiArt { get; init; }
}