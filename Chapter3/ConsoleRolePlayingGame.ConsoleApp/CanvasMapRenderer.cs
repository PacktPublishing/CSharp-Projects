using ConsoleRolePlayingGame.Domain;
using ConsoleRolePlayingGame.Domain.Overworld;
using Spectre.Console;
using Spectre.Console.Rendering;

namespace ConsoleRolePlayingGame.ConsoleApp;

public class CanvasMapRenderer(int availableWidth, int availableHeight)
{
    public int Width => availableWidth;
    public int Height => availableHeight;
    
    public IRenderable Render(GameManager game)
    {
        Pos playerPos = game.Party.Position;
        int playerOffsetX = (int)Math.Ceiling(availableWidth / 2.0);
        int playerOffsetY = (int)Math.Ceiling(availableHeight / 2.0);
        Pos mapUpperLeft = new Pos(playerPos.X - playerOffsetX, playerPos.Y - playerOffsetY);
        Pos mapLowerRight = new Pos(mapUpperLeft.X + availableWidth, mapUpperLeft.Y + availableHeight);
        MapCell[,] mapWindow = game.Map.GetMapWindow(mapUpperLeft, mapLowerRight);
        
        Canvas canvas = new(mapWindow.GetLength(0), mapWindow.GetLength(1));
        for (int y = 0; y < mapWindow.GetLength(1); y++)
        {
            for (int x = 0; x < mapWindow.GetLength(0); x++)
            {
                canvas.SetPixel(x, y, ToColor(mapWindow[x,y].Terrain));
            }
        }
    
        canvas.SetPixel(playerPos.X - mapUpperLeft.X, playerPos.Y - mapUpperLeft.Y, Color.Yellow1);

        return canvas;
    }

    private static Color ToColor(TerrainType terrainType) 
        => terrainType switch
        {
            TerrainType.Grass => Color.Green,
            TerrainType.Water => Color.Blue,
            TerrainType.DeepWater => Color.Blue3_1,
            TerrainType.Mountain => new Color(128, 128, 128),
            TerrainType.Forest => Color.DarkGreen,
            TerrainType.Desert => Color.MistyRose1,
            _ => Color.HotPink // Highlight the problem
        };
}