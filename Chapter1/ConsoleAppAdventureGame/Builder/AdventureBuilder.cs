using ConsoleAppAdventureGame.Engine;

namespace ConsoleAppAdventureGame.Builder;

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
        Adventure adventure = new Adventure();
        
        foreach (var nodeBuilder in _nodes)
        {
            AdventureNode node = nodeBuilder.Build();
            adventure.AddNode(node);
        }
        
        adventure.Start();
        
        return adventure;
    }


}