using ConsoleRolePlayingGame.Domain;
using ConsoleRolePlayingGame.Domain.Overworld;
using Spectre.Console;

namespace ConsoleRolePlayingGame.ConsoleApp;

public class CanvasMapRenderer(int availableWidth, int availableHeight, int xOffset = 0, int yOffset = 0)
{
    public void Render(GameManager game)
    {
        Pos playerPos = game.Player.Position;
        int playerOffsetX = (int)Math.Ceiling(availableWidth / 2.0);
        int playerOffsetY = (int)Math.Ceiling(availableHeight / 2.0);
        Pos mapUpperLeft = new Pos(playerPos.X - playerOffsetX, playerPos.Y - playerOffsetY);
        Pos mapLowerRight = new Pos(mapUpperLeft.X + availableWidth, mapUpperLeft.Y + availableHeight);
        MapCell[,] mapWindow = game.Map.GetMapWindow(mapUpperLeft, mapLowerRight);
        
        Canvas canvas = new(mapWindow.GetLength(0), mapWindow.GetLength(1));
        foreach (var cell in mapWindow)
        {
            int x = cell.Position.X - mapUpperLeft.X;
            int y = cell.Position.Y - mapUpperLeft.Y;
            canvas.SetPixel(x, y, ToColor(cell.Terrain));
        }
    
        canvas.SetPixel(playerPos.X - mapUpperLeft.X, playerPos.Y - mapUpperLeft.Y, Color.Yellow1);
    
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