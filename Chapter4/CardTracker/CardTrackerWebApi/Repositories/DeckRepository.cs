namespace CardTrackerWebApi.Repositories;

public class DeckRepository(CardTrackerDbContext context)
{
    // TODO: Get a specific deck
    
    // TODO: Add a deck
    // TODO: Delete a deck
    // TODO: Update a deck

    public IEnumerable<Deck> GetAllDecks()
    {
        return context.Decks.ToList();
    }
}