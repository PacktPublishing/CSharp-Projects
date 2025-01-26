namespace ConsoleAppAdventureGame.Engine;

public interface IAdventureRenderer
{
    void Render(StoryNode currentNode);
    Choice GetChoice(StoryNode currentNode);
    void RenderChoiceAction(Choice choice);
}