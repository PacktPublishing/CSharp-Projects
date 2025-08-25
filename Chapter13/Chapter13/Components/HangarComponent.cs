using Chapter13.Domain;
using Microsoft.Xna.Framework;

namespace Chapter13.Components;

/// <summary>
/// The Hangar Component defines ships that can be spawned from a location 
/// </summary>
public class HangarComponent
{
    private double _timeUntilNextSpawn;
    public double MinSpawnIntervalInSeconds { get; set; } = 30.0;
    
    public void Update(GameTime gameTime)
    {
        if (_timeUntilNextSpawn > 0)
        {
            _timeUntilNextSpawn -= gameTime.ElapsedGameTime.TotalSeconds;
        }
    }
    
    public bool IsReadyToSpawn => _timeUntilNextSpawn <= 0;
    public Faction Faction { get; set; }
    public float ShipSize { get; set; } = 4f;

    public void ResetSpawnTimer()
    {
        _timeUntilNextSpawn = MinSpawnIntervalInSeconds;
    }
}