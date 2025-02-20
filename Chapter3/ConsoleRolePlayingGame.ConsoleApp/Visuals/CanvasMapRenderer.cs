using ConsoleRolePlayingGame.Domain;
using ConsoleRolePlayingGame.Domain.Overworld;
using Spectre.Console;
using Spectre.Console.Rendering;

namespace ConsoleRolePlayingGame.ConsoleApp.Visuals;

public class CanvasMapRenderer(GameManager game, int availableWidth, int availableHeight) : IVisualGenerator
{
    public int Width => availableWidth;
    public int Height => availableHeight;
    
    public IRenderable GenerateVisual()
    {
        Pos center = game.Party.Position;
        int offsetX = (int)Math.Ceiling(availableWidth / 2.0);
        int offsetY = (int)Math.Ceiling(availableHeight / 2.0);
        Pos mapUpperLeft = new Pos(center.X - offsetX, center.Y - offsetY);
        Pos mapLowerRight = new Pos(mapUpperLeft.X + availableWidth, mapUpperLeft.Y + availableHeight);
        MapCell[,] mapWindow = game.Map.GetMapWindow(mapUpperLeft, mapLowerRight);
        
        Canvas canvas = new(mapWindow.GetLength(0), mapWindow.GetLength(1));
        
        // Render terrain
        for (int y = 0; y < mapWindow.GetLength(1); y++)
        {
            for (int x = 0; x < mapWindow.GetLength(0); x++)
            {
                MapCell cell = mapWindow[x,y];
                IMapEntity? entity = game.Map.Entities.FirstOrDefault(e => cell.Position == e.Position);
                
                Color color = entity switch
                {
                    Party => Color.Yellow1,
                    EnemyGroup => Color.Red,
                    _ => ToColor(cell.Terrain)
                };
                canvas.SetPixel(x, y, color);
            }
        }

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