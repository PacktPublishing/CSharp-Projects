using Spectre.Console;

namespace ConsoleRolePlayingGame.ConsoleApp;

public static class DisplayHelpers
{
    public static void DisplayTitle()
    {
        AnsiConsole.Write(new FigletText("Console RPG").Color(Color.Yellow));
        AnsiConsole.WriteLine("You are a group of adventurers in a fantasy world. Cleanse the world of evil!");
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
}