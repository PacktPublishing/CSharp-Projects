using ConsoleRolePlayingGame.Overworld.Structure;

namespace ConsoleRolePlayingGame.Overworld.Generators;

public class OpenPosSelector(WorldMap map, Random random)
{
    public Pos GetOpenPositionNear(Pos centralPosition, int minDistance, int maxDistance)
    {
        HashSet<Pos> occupied = [..map.Entities.Select(e => e.MapPos)];
        
        Pos pos;
        do
        {
            int offset = random.Next(minDistance, maxDistance + 1);
            int xOffset = (int)Math.Round(random.NextDouble() * offset);
            int yOffset = offset - xOffset;

            // Randomly flip the offsets to negative
            if (random.NextDouble() < 0.5)
            {
                xOffset *= -1;
            }
            if (random.NextDouble() < 0.5)
            {
                yOffset *= -1;
            }

            pos = new Pos(centralPosition.X + xOffset, centralPosition.Y + yOffset);
        } while (occupied.Contains(pos));

        return pos;
    }
}