namespace ConsoleAppAdventureGame;

public class AdventureNode(string id)
{
    public string Id { get; set; } = id;
    public required string[] Text { get; init; }
    public AdventureChoice[] Choices { get; init; } = [];
}