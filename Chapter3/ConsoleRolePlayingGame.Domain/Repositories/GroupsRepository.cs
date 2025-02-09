using ConsoleRolePlayingGame.Domain.Combat;

namespace ConsoleRolePlayingGame.Domain.Repositories;

public class GroupsRepository(CharacterRepository characterRepository)
{
    public Party LoadParty()
    {
        List<GameCharacter> members = new()
        {
            characterRepository.LoadCharacter("Sam the Terramancer"),
            characterRepository.LoadCharacter("Stephanie the Paladin"),
            characterRepository.LoadCharacter("James the Goat Herder"),
            characterRepository.LoadCharacter("Sara of the Knives")
        };
        
        return new Party { Members = members };
    }
}