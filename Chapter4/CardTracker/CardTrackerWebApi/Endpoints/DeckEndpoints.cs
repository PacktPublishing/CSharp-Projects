namespace CardTrackerWebApi.Endpoints;

public static class DeckEndpoints
{
    public static void AddDeckEndpoints(this IEndpointRouteBuilder app)
    {
        AddGetAllDecksEndpoint(app);
        AddGetDeckEndpoint(app);
        
        // TODO: Add a deck
        // TODO: Delete a deck
        // TODO: Add a card to a deck
        // TODO: Remove a card from a deck
        // TODO: Validate decks
    }

    private static void AddGetDeckEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("/decks/{id}", (int id, CardTrackerDbContext db, HttpContext http) =>
            {
                Deck? deck = db.Decks
                    .Include(d => d.Cards)
                    .Include(d => d.User)
                    .AsNoTracking()
                    .FirstOrDefault(d => d.Id == id);
        
                if (deck is null)
                {
                    return Results.NotFound();
                }
        
                string username = http.User.Identity!.Name!;
                User user = db.Users.AsNoTracking().First(u => u.Username == username);
        
                if (deck.User.Id != user.Id && !user.IsAdmin)
                {
                    return Results.Forbid();
                }
        
                return Results.Ok(deck); // TODO: Map to a return object
            })
            .WithName("GetDeck")
            .WithDescription("Get a single deck by its ID")
            .RequireAuthorization(); // Any role
    }

    private static void AddGetAllDecksEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("/decks", (CardTrackerDbContext context) => context.Decks.AsNoTracking().ToList())
            .WithName("GetDecks")
            .WithDescription("Get all registered decks")
            .AllowAnonymous();
    }
}