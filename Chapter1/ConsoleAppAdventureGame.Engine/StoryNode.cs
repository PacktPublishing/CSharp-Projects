namespace ConsoleAppAdventureGame.Engine;

public class StoryNode(string id)
{
    public string Id => id;
    public required string[] Text { get; init; }
    public Choice[] Choices { get; init; } = [];
}