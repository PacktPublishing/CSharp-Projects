namespace ConsoleAppAdventureGame.Engine;

public record StoryNode(string Id)
{
    public required string[] Text { get; init; }
    public Choice[] Choices { get; init; } = [];
}