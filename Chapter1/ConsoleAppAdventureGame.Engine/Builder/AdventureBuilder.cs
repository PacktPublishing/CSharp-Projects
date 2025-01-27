namespace ConsoleAppAdventureGame.Engine.Builder;

public class AdventureBuilder
{
    private readonly List<NodeBuilder> _nodes = new();
    
    public AdventureBuilder WithNode(string id, Action<NodeBuilder> configure)
    {
        NodeBuilder nodeBuilder = new(id);
        configure(nodeBuilder);
        
        _nodes.Add(nodeBuilder);
        
        return this;
    }
    
    public AdventureBuilder WithStartNode(Action<NodeBuilder> configure) 
        => WithNode(Adventure.StartNodeId, configure);

    public Adventure Build()
    {
        Validate();

        IEnumerable<StoryNode> nodes = _nodes.Select(n => n.Build());
        Adventure adventure = new Adventure(nodes);
        
        return adventure;
    }

    private void Validate()
    {
        foreach (var node in _nodes)
        {
            node.Validate();

            // For nodes with choices that go to other nodes, ensure those nodes exist
            foreach (var choice in node.Choices)
            {
                if (!string.IsNullOrWhiteSpace(choice.NextNodeId) && _nodes.All(n => n.Id != choice.NextNodeId))
                {
                    throw new InvalidOperationException($"Node '{node.Id}' references a non-existent node '{choice.NextNodeId}' in choice '{choice.Text}'");
                }
            }
        }
    }
}