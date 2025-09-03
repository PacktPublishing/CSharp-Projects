using Chapter13.Domain;
using Microsoft.Xna.Framework;

namespace Chapter13.Components;

public class LauncherComponent(float timeBetweenSpawns = 5f)
{
    public float TimeBetweenSpawns { get; set; } = timeBetweenSpawns;
    public float DetectionDistance { get; set; } = 10f;
    public float DetectionRadius { get; set; } = 5f;
    public bool IsReadyToSpawn => _timeUntilNextSpawn <= 0;
    public ShipType ShipType { get; init; } = ShipType.Missile;

    private float _timeUntilNextSpawn = timeBetweenSpawns;
    
    public void Update(GameTime gameTime)
    {
        if (_timeUntilNextSpawn > 0)
        {
            _timeUntilNextSpawn -= (float)gameTime.ElapsedGameTime.TotalSeconds;
        }
    }

    public void ResetSpawnTimer()
    {
        _timeUntilNextSpawn = TimeBetweenSpawns;
    }
}