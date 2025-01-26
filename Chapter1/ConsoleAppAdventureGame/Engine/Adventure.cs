namespace ConsoleAppAdventureGame.Engine;

public class Adventure
{
    public const string StartNodeId = "Start";
    private readonly Dictionary<string, AdventureNode> _nodes = new();

    public AdventureNode? CurrentNode { get; private set; }

    public void AddNode(AdventureNode node)
    {
        // This will error if we already have a node with this id, which is what we want
        _nodes.Add(node.Id, node);
    }

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
        Start();

        while (CurrentNode is not null)
        {
            AdventureChoice? choice = ExecuteNodeAndGetChoice(renderer);

            // Some nodes will not have choices at the end of a story.
            if (choice != null)
            {
                ExecuteChoice(choice, renderer);
            }
        }
    }

    internal void Start()
    {
        ValidateNodes();
        CurrentNode = GetNode(StartNodeId);
    }

    private void ValidateNodes()
    {
        foreach (var node in _nodes.Values)
        {
            // Ensure all nodes have text
            if (node.Text.Length == 0)
            {
                throw new InvalidOperationException($"Node '{node.Id}' has no text.");
            }

            // For nodes with choices that go to other nodes, ensure those nodes exist
            foreach (var choice in node.Choices)
            {
                if (!string.IsNullOrWhiteSpace(choice.NextNodeId) && !_nodes.ContainsKey(choice.NextNodeId))
                {
                    throw new InvalidOperationException(
                        $"Node '{node.Id}' references a non-existent node '{choice.NextNodeId}' in choice '{choice.Text}'");
                }
            }
        }
    }

    private AdventureChoice? ExecuteNodeAndGetChoice(IAdventureRenderer renderer)
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
    
    internal void ExecuteChoice(AdventureChoice choice, IAdventureRenderer renderer)
    {
        // Renders any custom text associated with the choice, helping bridge narrative gaps.
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