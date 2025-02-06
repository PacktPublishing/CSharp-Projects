namespace ConsoleRolePlayingGame.Domain.Commands;

public class QuitCommand : Command
{
    public override void Execute(GameManager game)
    {
        game.Status = GameStatus.Terminated;
    }
}