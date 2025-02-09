namespace ConsoleRolePlayingGame.Domain.Combat;

public class Enemy : GameCharacter
{
    public override string Name { get; init; }
    
    public override required string[] AsciiArt { get; init; }
}