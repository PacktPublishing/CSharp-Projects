namespace ConsoleRolePlayingGame.Domain.Commands;

public class MovePlayerCommand(Direction direction) : Command
{
    public override void Execute(GameManager game)
    {
        game.Player.Move(direction);
    }
}