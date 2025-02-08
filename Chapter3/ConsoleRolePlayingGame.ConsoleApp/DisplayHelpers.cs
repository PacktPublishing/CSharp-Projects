using ConsoleRolePlayingGame.Domain;
using Spectre.Console;

namespace ConsoleRolePlayingGame.ConsoleApp;

public static class DisplayHelpers
{
    public static void DisplayTitle()
    {
        AnsiConsole.Write(new FigletText("Console RPG").Color(Color.Yellow));
        AnsiConsole.MarkupLine("You are a group of [yellow1]adventurers[/] in a fantasy world. Cleanse the world of [red1]evil[/]!");
        AnsiConsole.WriteLine();
    }

    public static void TrySetEncoding()
    {
        try
        {
            Console.InputEncoding = System.Text.Encoding.UTF8;
            Console.OutputEncoding = System.Text.Encoding.UTF8;
        }
        catch
        {
            AnsiConsole.MarkupLine("[red]Error setting console encoding; the application may not render properly[/]");
        }
    }

    public static void WriteHeader(GameManager game)
    {
        AnsiConsole.MarkupLineInterpolated($"Location: [bold white]{game.Player.Position.X}[/],[bold white]{game.Player.Position.Y}[/]. Press [yellow bold]q[/] to quit");
        AnsiConsole.WriteLine();
    }
}