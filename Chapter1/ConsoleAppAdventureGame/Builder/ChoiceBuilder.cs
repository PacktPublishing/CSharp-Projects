using ConsoleAppAdventureGame.Engine;

namespace ConsoleAppAdventureGame.Builder;

public class ChoiceBuilder(string text)
{
    private readonly List<string> _lines = new();
    private string? _target;

    public AdventureChoice Build()
    {
        if (string.IsNullOrWhiteSpace(_target))
        {
            throw new InvalidOperationException("Must specify a target node for the choice");
        }
        
        return new AdventureChoice(text)
        {
            TextWhenChosen = _lines.ToArray(),
            NextNodeId = _target
        };
    }

    public ChoiceBuilder WithText(params string[] lines)
    {
        if (lines.Length == 0)
        {
            throw new ArgumentException("Must provide at least one line of text", nameof(lines));
        }

        _lines.AddRange(lines);

        return this;
    }

    public ChoiceBuilder ThatLeadsTo(string targetId)
    {
        _target = targetId;
        
        return this;
    }

    public ChoiceBuilder ThatLeadsTo(NodeBuilder targetNode) 
        => ThatLeadsTo(targetNode.Id);

    public ChoiceBuilder ThatLeadsToStart() 
        => ThatLeadsTo(Adventure.StartNodeId);
}