namespace ConsoleAppAdventureGame.Tests;

public class TestAdventureRenderer : IAdventureRenderer
{
    public void Render(StoryNode node)
    {
        // No action needed
    }

    public Choice GetChoice(StoryNode node)
    {
        return node.Choices.First();
    }

    public void RenderChoiceAction(Choice choice)
    {
        // No action needed
    }
}