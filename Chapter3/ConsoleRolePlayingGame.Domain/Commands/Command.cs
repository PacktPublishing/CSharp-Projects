namespace ConsoleRolePlayingGame.Domain.Commands;

public abstract class Command
{
    public abstract void Execute(GameManager game);
}