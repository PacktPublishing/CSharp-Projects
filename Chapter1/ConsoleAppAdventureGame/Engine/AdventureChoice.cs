namespace ConsoleAppAdventureGame;

public class AdventureChoice
{
    public required string Text { get; init; }
    public string[] TextWhenChosen { get; init; } = [];
    public string? NextNodeId { get; init; }
}