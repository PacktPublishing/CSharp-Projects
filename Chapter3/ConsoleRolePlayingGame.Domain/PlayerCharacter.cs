using ConsoleRolePlayingGame.Domain.Overworld;

namespace ConsoleRolePlayingGame.Domain;

public class PlayerCharacter: GameCharacter
{
    public required override string Name { get; init; }
}