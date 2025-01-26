using ConsoleAppAdventureGame.Engine;

namespace ConsoleAppAdventureGame.Builder;

public class NodeBuilder(string id)
{
    private readonly List<string> _text = new();
    private readonly List<ChoiceBuilder> _choices = new();
    
    public string Id => id;
    
    public AdventureNode Build()
    {
        return new AdventureNode(id)
        {
            Text = _text.ToArray(),
            Choices = _choices.Select(c => c.Build()).ToArray()
        };
    }

    public NodeBuilder HasText(params string[] lines)
    {
        if (lines.Length == 0)
        {
            throw new ArgumentException("Must provide at least one line of text", nameof(lines));
        }
        
        _text.AddRange(lines);

        return this;
    }

    public ChoiceBuilder HasChoice(string text)
    {
        ChoiceBuilder choiceBuilder = new(text);
        _choices.Add(choiceBuilder);
        
        return choiceBuilder;
    }
}