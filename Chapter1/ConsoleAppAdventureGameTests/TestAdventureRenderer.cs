namespace ConsoleAppAdventureGameTests;

public class TestAdventureRenderer : IAdventureRenderer
{
    public void Render(AdventureNode currentNode)
    {
        // No action needed
    }

    public AdventureChoice GetChoice(AdventureNode currentNode)
    {
        return currentNode.Choices.First();
    }

    public void RenderChoiceAction(AdventureChoice choice)
    {
        // No action needed
    }
}