namespace ConsoleAppAdventureGame.Engine;

public record Choice(string Text)
{
    public string[] WhenChosen { get; init; } = [];
    public required string NextNodeId { get; init; }

    public void Execute(Adventure adventure, IAdventureRenderer renderer)
    {
        // Renders any custom text associated with the choice, helping bridge narrative gaps.
        renderer.RenderChoiceAction(this);
        if (string.IsNullOrWhiteSpace(NextNodeId))
        {
            adventure.CurrentNode = null;
        }
        else
        {
            adventure.CurrentNode = adventure.GetNode(NextNodeId);
        }
    }
}