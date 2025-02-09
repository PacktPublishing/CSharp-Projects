using ConsoleRolePlayingGame.Domain.Combat;
using Spectre.Console;
using Spectre.Console.Rendering;

namespace ConsoleRolePlayingGame.ConsoleApp.Visuals;

public class CombatGroupRenderer(ICombatGroup group, bool includeStats = false) : IVisualGenerator
{
    private IRenderable GenerateCharacterVisual(GameCharacter c)
    {
        List<IRenderable> visuals = [
            ..c.AsciiArt.Select(l => new Markup(l).Justify(Justify.Center)), 
            new Text(c.Name).Justify(Justify.Center)
        ];
        
        if (includeStats)
        {
            string statsText = $"[red]HP: {c.Health}/{c.MaxHealth}[/] " +
                               $"[blue]MP: {c.Mana}/{c.MaxMana}[/]";
            
            visuals.Add(new Markup(statsText).Justify(Justify.Center));
        }

        return new Padder(
            new Rows(visuals), 
            new Padding(1, 0, 1, 1)
        );
    }
    
    public IRenderable GenerateVisual()
    {
        IRenderable content = new Columns(group.Members.Select(GenerateCharacterVisual));
        
        return new Panel(content)
            .Header($"[Yellow] {group.Name} [/]")
            .RoundedBorder()
            .Expand();
    }
}