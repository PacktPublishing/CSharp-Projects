namespace ConsoleAppAdventureGame.Engine;

public interface IAdventureRenderer
{
    void Render(StoryNode node);
    Choice GetChoice(StoryNode node);
    void RenderChoiceAction(Choice choice);
}