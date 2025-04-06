using System.Collections.Immutable;

namespace ConsoleAppAdventureGame.Engine;

public class Adventure
{
    public const string StartNodeId = "Start";
    private readonly ImmutableDictionary<string, StoryNode> _nodes;

    public StoryNode? CurrentNode { get; internal set; }

    public Adventure(IEnumerable<StoryNode> nodes, string startNodeId = StartNodeId)
    {
        StringComparer comparison = StringComparer.OrdinalIgnoreCase;
        _nodes = nodes.ToImmutableDictionary(n => n.Id, comparison);
        CurrentNode = _nodes[startNodeId];
    }

    public StoryNode GetNode(string id) 
        => _nodes[id];

    public void Run(IAdventureRenderer renderer)
    {
        while (CurrentNode is not null)
        {
            renderer.Render(CurrentNode);

            // Some nodes may have zero choices. These represent the end of a branch in the adventure.
            if (CurrentNode.Choices.Length == 0)
            {
                CurrentNode = null;
            }
            else
            {
                Choice choice = renderer.GetChoice(CurrentNode);
                choice.Execute(this, renderer);
            }
        }
    }
}