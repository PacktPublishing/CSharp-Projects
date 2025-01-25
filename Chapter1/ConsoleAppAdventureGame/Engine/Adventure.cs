namespace ConsoleAppAdventureGame.Engine;

public class Adventure
{
    protected const string StartNodeId = "Start";
    private readonly Dictionary<string, AdventureNode> _nodes = new(StringComparer.OrdinalIgnoreCase);
    
    public AdventureNode? CurrentNode { get; private set; }
    
    public void AddNode(AdventureNode node) 
        => _nodes[node.Id] = node;

    public AdventureNode GetNode(string id)
    {
        AdventureNode? node = _nodes.GetValueOrDefault(id);
        if (node == null)
        {
            throw new InvalidOperationException($"Could not find node with ID '{id}'");
        }
        return node;
    }

    public void Run(IAdventureRenderer renderer)
    {
        try
        {
            CurrentNode = GetNode(StartNodeId);

            while (CurrentNode is not null)
            {
                ExecuteNodeAndGetChoice(renderer);
            }
        }
        catch (InvalidOperationException ex)
        {
            renderer.RenderError(ex);
        }
    }

    private void ExecuteNodeAndGetChoice(IAdventureRenderer renderer)
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
            return;
        }
        
        // Get the choice and show any custom text for that choice
        // Custom text helps with transitions when there are multiple ways of getting to a story node
        AdventureChoice choice = renderer.GetChoice(CurrentNode);
        renderer.RenderChoiceAction(choice);
        
        // Move to the next node based on the player's choice
        if (string.IsNullOrWhiteSpace(choice.NextNodeId))
        {
            CurrentNode = null;
        }
        else
        {
            CurrentNode = GetNode(choice.NextNodeId);
        }
    }
}