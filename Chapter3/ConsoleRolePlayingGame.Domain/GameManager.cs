using ConsoleRolePlayingGame.Domain.Combat;
using ConsoleRolePlayingGame.Domain.Commands;
using ConsoleRolePlayingGame.Domain.Overworld;

namespace ConsoleRolePlayingGame.Domain;

public class GameManager
{
    public GameStatus Status { get; internal set; } = GameStatus.Overworld;
    public WorldMap Map { get; }
    public CommandRegistry Commands { get; } = new();
    public PlayerCharacter Player { get; } = new(new Pos(0, 0));

    public GameManager()
    {
        MapGenerator generator = new();
        Map = new WorldMap(generator);
    }
}