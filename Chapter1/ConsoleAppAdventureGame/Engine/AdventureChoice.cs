namespace ConsoleAppAdventureGame.Engine;

public class AdventureChoice(string text)
{
    public string Text { get; } = text;
    public string[] TextWhenChosen { get; init; } = [];
    public string? NextNodeId { get; init; }
}