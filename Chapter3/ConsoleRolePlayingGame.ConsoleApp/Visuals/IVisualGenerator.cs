using Spectre.Console.Rendering;

namespace ConsoleRolePlayingGame.ConsoleApp.Visuals;

public interface IVisualGenerator
{
    IRenderable GenerateVisual();
}