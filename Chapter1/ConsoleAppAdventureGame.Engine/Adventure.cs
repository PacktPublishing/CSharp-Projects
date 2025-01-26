namespace ConsoleAppAdventureGame.Engine;

public class Adventure
{
    public const string StartNodeId = "Start";
    private readonly Dictionary<string, StoryNode> _nodes = new();

    public StoryNode? CurrentNode { get; internal set; }

    public void AddNode(StoryNode node) =>
        // This will error if we already have a node with this id, which is what we want
        _nodes.Add(node.Id, node);

    public StoryNode GetNode(string id) 
        => _nodes[id];

    public void Run(IAdventureRenderer renderer)
    {
        while (CurrentNode is not null)
        {
            Choice? choice = ExecuteNodeAndGetChoice(renderer);

            // Some nodes will not have choices at the end of a story.
            choice?.Execute(this, renderer);
        }
    }

    public void MoveToNode(string? nextNodeId)
    {
        if (string.IsNullOrWhiteSpace(nextNodeId))
        {
            CurrentNode = null;
        }
        else
        {
            CurrentNode = GetNode(nextNodeId);
        }
    }
    
    
    private Choice? ExecuteNodeAndGetChoice(IAdventureRenderer renderer)
    {
        if (CurrentNode == null)
        {
            throw new InvalidOperationException("Current node is null and cannot be rendered.");
        }

        renderer.Render(CurrentNode);

        // Some nodes may have zero choices. These represent the end of a branch in the adventure.
        if (CurrentNode.Choices.Length == 0)
        {
            CurrentNode = null;
            return null;
        }

        // Asks the user to make a choice
        return renderer.GetChoice(CurrentNode);
    }
}