namespace ConsoleAppAdventureGame.Tests;

public class TestAdventureRenderer : IAdventureRenderer
{
    public void Render(StoryNode currentNode)
    {
        // No action needed
    }

    public Choice GetChoice(StoryNode currentNode)
    {
        return currentNode.Choices.First();
    }

    public void RenderChoiceAction(Choice choice)
    {
        // No action needed
    }
}