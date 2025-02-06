namespace ConsoleRolePlayingGame.Domain.Commands;

public class CommandRegistry
{
    private readonly Dictionary<ConsoleKey, Command> _commands = new()
    {
        { ConsoleKey.Q, new QuitCommand() },
        { ConsoleKey.W, new MovePlayerCommand(Direction.North) },
        { ConsoleKey.A, new MovePlayerCommand(Direction.West) },
        { ConsoleKey.S, new MovePlayerCommand(Direction.South) },
        { ConsoleKey.D, new MovePlayerCommand(Direction.East) },    
        { ConsoleKey.UpArrow, new MovePlayerCommand(Direction.North) },
        { ConsoleKey.LeftArrow, new MovePlayerCommand(Direction.West) },
        { ConsoleKey.DownArrow, new MovePlayerCommand(Direction.South) },
        { ConsoleKey.RightArrow, new MovePlayerCommand(Direction.East) },
    };
    
    public Command? GetCommandForKey(ConsoleKey key)
    {
        _commands.TryGetValue(key, out Command? command);
        
        return command;
    }
}