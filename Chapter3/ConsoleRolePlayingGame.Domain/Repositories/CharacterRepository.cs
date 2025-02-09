using System.Text.Json;
using ConsoleRolePlayingGame.Domain.Combat;

namespace ConsoleRolePlayingGame.Domain.Repositories;

public class CharacterRepository
{
    public GameCharacter LoadCharacter(string name)
    {
        string filePath = Path.Combine(Environment.CurrentDirectory, "Characters.json");
        
        if (!File.Exists(filePath))
        {
            throw new FileNotFoundException($"Could not find {filePath}");
        }
        
        using var stream = File.OpenRead(filePath);
        List<PlayerCharacter>? characters = JsonSerializer.Deserialize<List<PlayerCharacter>>(stream);
        
        if (characters is null || characters.Count <= 0)
        {
            throw new JsonException($"{filePath} did not contain valid character information");
        }
        
        // TODO: Store this as a Dictionary

        PlayerCharacter match = characters.First(x => x.Name == name);
        
        match.Health = match.MaxHealth;
        match.Mana = match.MaxMana;
        
        return match;
    }
}