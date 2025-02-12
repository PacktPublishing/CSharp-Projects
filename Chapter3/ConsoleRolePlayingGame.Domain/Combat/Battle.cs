namespace ConsoleRolePlayingGame.Domain.Combat;

public class Battle(Party party, EnemyGroup enemies)
{
    public EnemyGroup Enemies => enemies;
    public Party Party => party;
    
    public IEnumerable<GameCharacter> AllCharacters => [..party.Members, ..enemies.Members];
    
    public GameCharacter? ActiveMember => AllCharacters
            .Where(c => c.IsReady)
            .OrderBy(c => c.TimeUntilTurn)
            .FirstOrDefault();

    private const int TimeBetweenTurns = 100;
    
    public Battle Start()
    {
        foreach (var member in AllCharacters)
        {
            member.TimeUntilTurn = TimeBetweenTurns;
        }
        return this;
    }
    
    public void AdvanceTime()
    {
        foreach (var member in AllCharacters)
        {
            member.AdvanceTime();
        }
    }

    public void RunAiTurn(Enemy enemy)
    {
        // TODO: Implement AI
        
        enemy.TimeUntilTurn = TimeBetweenTurns;
    }

    public void RunTurn(GameCharacter character, Ability ability, GameCharacter? target)
    {
        // TODO: Actually handle this
        
        character.TimeUntilTurn = TimeBetweenTurns;
    }
}