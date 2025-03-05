using ConsoleRolePlayingGame.Domain;
using ConsoleRolePlayingGame.Domain.Combat;
using ConsoleRolePlayingGame.Domain.Overworld;
using Spectre.Console;
using Spectre.Console.Rendering;

namespace ConsoleRolePlayingGame.ConsoleApp.Visuals;

public class MapRenderer(GameManager game, int width, int height)
{
    
    public IRenderable GenerateVisual()
    {
        Pos center = game.Party.MapPos;
        int offsetX = (int)Math.Ceiling(width / 2.0);
        int offsetY = (int)Math.Ceiling(height / 2.0);
        Pos mapTopLeft = new Pos(center.X - offsetX, center.Y - offsetY);
        MapCell[,] mapWindow = game.Map.GetMapWindow(mapTopLeft, width, height);
        
        Canvas canvas = new(mapWindow.GetLength(0), mapWindow.GetLength(1));
        
        // Render terrain
        for (int y = 0; y < mapWindow.GetLength(1); y++)
        {
            for (int x = 0; x < mapWindow.GetLength(0); x++)
            {
                MapCell cell = mapWindow[x,y];
                CombatGroup? entity = game.Map.Entities.FirstOrDefault(e => cell.Position == e.MapPos);
                canvas.SetPixel(x, y, GetCellColor(entity, cell.Terrain));
            }
        }

        return canvas;
    }

    private Color GetCellColor(CombatGroup? entity, TerrainType terrain)
    {
        if (entity is not null) return entity == game.Party 
            ? Color.Yellow1 
            : Color.Red;
        
        return terrain switch
        {
            TerrainType.Grass => Color.Green,
            TerrainType.Water => Color.Blue,
            TerrainType.DeepWater => Color.Blue3_1,
            TerrainType.Mountain => new Color(128, 128, 128),
            TerrainType.Forest => Color.DarkGreen,
            TerrainType.Desert => Color.MistyRose1,
            _ => throw new NotSupportedException($"Unsupported terrain: {terrain}")
        };
    }
}