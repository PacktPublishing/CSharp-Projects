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
            AnsiConsole.MarkupLine(line);
        }
    }

    public void RenderChoiceAction(Choice choice)
    {
        foreach (var line in choice.WhenChosen)
        {
            AnsiConsole.MarkupLine(line);
        }
    }
}