namespace ConsoleAppAdventureGame.Engine;

public interface IAdventureRenderer
{
    void Render(AdventureNode currentNode);
    AdventureChoice GetChoice(AdventureNode currentNode);
    void RenderChoiceAction(AdventureChoice choice);
    void RenderError(Exception exception);
}