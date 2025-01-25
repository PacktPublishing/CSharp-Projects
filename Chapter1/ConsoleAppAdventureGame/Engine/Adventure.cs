namespace ConsoleAppAdventureGame.Engine;

public class Adventure
{
    public const string StartNodeId = "Start";
    private readonly Dictionary<string, AdventureNode> _nodes = new(StringComparer.OrdinalIgnoreCase);
    
    public AdventureNode? CurrentNode { get; private set; }
    
    public void AddNode(AdventureNode node) 
        => _nodes[node.Id] = node;

    public AdventureNode? GetNode(string id) 
        => _nodes.GetValueOrDefault(id);

    public void Run(IAdventureRenderer renderer)
    {
        CurrentNode = GetNode(StartNodeId);
        if (CurrentNode == null)
        {
            throw new InvalidOperationException($"Node with ID '{StartNodeId}' not found. Ensure your adventure contains this node");
        }
        
        while (CurrentNode is not null)
        {
            ExecuteNodeAndGetChoice(renderer);
        }
    }

    private void ExecuteNodeAndGetChoice(IAdventureRenderer renderer)
    {
        if (CurrentNode == null)
        {
            throw new InvalidOperationException("Current node is null and cannot be rendered.");
        }
        
        renderer.Render(CurrentNode);
            
        var choice = renderer.GetChoice(CurrentNode);
            
        renderer.RenderChoiceAction(choice);
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