using ConsoleRolePlayingGame.Domain.Overworld;
using Spectre.Console;

namespace ConsoleRolePlayingGame.ConsoleApp;

public static class CanvasMapRenderer
{
    public static void Render(WorldMap map)
    {
        Canvas canvas = new(map.Width, map.Height);
        foreach (var cell in map.Map)
        {
            canvas.SetPixel(cell.Position.X, cell.Position.Y, ToColor(cell.Terrain));
        }
    
        canvas.SetPixel(map.PlayerPos.X, map.PlayerPos.Y, Color.Yellow1);
    
        AnsiConsole.Write(canvas);
    }
    
    public static Color ToColor(TerrainType terrainType) 
        => terrainType switch
        {
            TerrainType.Grass => Color.Green,
            TerrainType.Water => Color.Blue,
            TerrainType.Mountain => Color.DarkSlateGray1,
            TerrainType.Forest => Color.DarkGreen,
            TerrainType.Desert => Color.MistyRose1,
            TerrainType.Road => Color.SandyBrown,
            _ => Color.HotPink // Highlight the problem
        };
}