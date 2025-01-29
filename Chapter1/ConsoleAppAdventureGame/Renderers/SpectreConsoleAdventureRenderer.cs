using ConsoleAppAdventureGame.Engine;
using Spectre.Console;

namespace ConsoleAppAdventureGame.Renderers;

public class SpectreConsoleAdventureRenderer : IAdventureRenderer
{
    
    public Choice GetChoice(StoryNode node)
    {
        Choice choice = AnsiConsole.Prompt(new SelectionPrompt<Choice>()
            .Title("[Yellow]What do you want to do?[/]")
            .AddChoices(node.Choices)
            .UseConverter(c => c.Text));
        
        AnsiConsole.MarkupLineInterpolated($"[yellow]>[/] [bold blue]{choice.Text}[/]");
        AnsiConsole.WriteLine();
        
        return choice;
    }
    
    public void Render(StoryNode node)
    {
        // Write a horizontal line to help separate the text 
        AnsiConsole.Write(new Rule($"[Yellow]{node.Id}[/]")
            .LeftJustified()
            .RuleStyle(new Style(foreground: Color.Blue)));
        
        foreach (var line in node.Text)
        {
            AnsiConsole.MarkupLine(ReplaceLine(line));
            AnsiConsole.WriteLine();
        }
    }

    public void RenderChoiceAction(Choice choice)
    {
        foreach (var line in choice.TextWhenChosen)
        {
            AnsiConsole.MarkupLine(ReplaceLine(line));
            AnsiConsole.WriteLine();
        }
    }

    private string ReplaceLine(string line) => line
            .Replace("“", "[bold cyan]“")
            .Replace("”", "”[/]");
}