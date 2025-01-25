using Spectre.Console;

namespace ConsoleAppAdventureGame.Engine;

public class SpectreConsoleAdventureRenderer : IAdventureRenderer
{
    public void Render(AdventureNode currentNode)
    {
        foreach (var line in currentNode.Text)
        {
            AnsiConsole.MarkupLine(ReplaceLine(line));
            AnsiConsole.WriteLine();
        }
    }

    public AdventureChoice GetChoice(AdventureNode currentNode)
    {
        AdventureChoice choice = AnsiConsole.Prompt(new SelectionPrompt<AdventureChoice>()
            .Title("What do you want to do?")
            .AddChoices(currentNode.Choices)
            .UseConverter(c => c.Text));
        
        AnsiConsole.MarkupLineInterpolated($"[yellow]>[/] [bold blue]{choice.Text}[/]");
        AnsiConsole.WriteLine();
        
        return choice;
    }

    public void RenderChoiceAction(AdventureChoice choice)
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

    public void RenderError(Exception exception) 
        => AnsiConsole.WriteException(exception, ExceptionFormats.ShortenEverything);
}