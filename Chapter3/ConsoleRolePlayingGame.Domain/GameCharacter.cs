using ConsoleRolePlayingGame.Domain.Overworld;

namespace ConsoleRolePlayingGame.Domain;

public abstract class GameCharacter(Pos pos)
{
    public Pos Position { get; set; } = pos;
    public int Health { get; set; }
    public int Mana { get; set; }
    public int Strength { get; set; }
    public int Dexterity { get; set; }
    public int Speed { get; set; }
}