using ConsoleRolePlayingGame.Domain.Overworld;
using Spectre.Console;

namespace ConsoleRolePlayingGame.ConsoleApp;

public static class CanvasMapRenderer
{
    public static void Render(WorldMap map)
    {
        Pos playerPos = map.PlayerPos;
        Pos upperLeftCorner = new Pos(playerPos.X - 10, playerPos.Y - 5);
        Pos lowerRightCorner = new Pos(playerPos.X + 10, playerPos.Y + 5);
        MapCell[,] mapWindow = map.GetMapWindow(upperLeftCorner, lowerRightCorner);
        
        Canvas canvas = new(mapWindow.GetLength(0), mapWindow.GetLength(1));
        foreach (var cell in mapWindow)
        {
            int x = cell.Position.X - upperLeftCorner.X;
            int y = cell.Position.Y - upperLeftCorner.Y;
            canvas.SetPixel(x, y, ToColor(cell.Terrain));
        }
    
        canvas.SetPixel(map.PlayerPos.X - upperLeftCorner.X, map.PlayerPos.Y - upperLeftCorner.Y, Color.Yellow1);
    
        AnsiConsole.Write(canvas);
    }

    private static Color ToColor(TerrainType terrainType) 
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