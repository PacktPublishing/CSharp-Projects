using ConsoleRolePlayingGame.Domain.Overworld;
using Spectre.Console;

namespace ConsoleRolePlayingGame.ConsoleApp;

public static class MapRenderingHelpers
{
    public static Color ToColor(this TerrainType terrainType) 
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