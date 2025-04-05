using System.Text.RegularExpressions;
using ConsoleAppAdventureGame.Engine;
using Spectre.Console;

namespace ConsoleAppAdventureGame.Renderers;

public partial class SpectreConsoleAdventureRenderer : IAdventureRenderer
{
    
    public Choice GetChoice(StoryNode node)
    {
        Choice choice = AnsiConsole.Prompt(new SelectionPrompt<Choice>()
            .Title("[Yellow]What do you want to do?[/]")
            .AddChoices(node.Choices)
            .UseConverter(c => c.Text));
        
        AnsiConsole.MarkupLineInterpolated($"[yellow]>[/] [bold blue]{choice.Text}[/]");
        
        return choice;
    }
    
    public void Render(StoryNode node)
    {
        foreach (var line in node.Text)
        {
            AnsiConsole.MarkupLine(ReplaceLine(line));
        }
    }

    public void RenderChoiceAction(Choice choice)
    {
        foreach (var line in choice.WhenChosen)
        {
            AnsiConsole.MarkupLine(ReplaceLine(line));
        }
    }

    [GeneratedRegex(@"\*(.*?)\*")]
    private static partial Regex ItalicsRegex();

    [GeneratedRegex(@"\*\*(.*?)\*\*")]
    private static partial Regex BoldRegex();

    private static string ReplaceLine(string line)
    {
        line = ItalicsRegex().Replace(line, "[italic cyan]$1[/]");
        line = BoldRegex().Replace(line, "[bold yellow]$1[/]");

        return line;
    }
}