namespace CardTrackerWebApi.Endpoints;

public static class DeckEndpoints
{
    public static void AddDeckEndpoints(this IEndpointRouteBuilder app)
    {
        AddGetAllDecksEndpoint(app);
        AddGetDeckEndpoint(app);
        AddCreateDeckEndpoint(app);
        AddDeleteDeckEndpoint(app);
        AddModifyDeckEndpoint(app);
    }

    private static void AddModifyDeckEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPut("/decks/{id}", (int id, EditDeckRequest request, CardTrackerDbContext db, HttpContext http) =>
            {
                string username = http.GetCurrentUsername();
                User user = db.Users.AsNoTracking().First(u => u.Username == username);
                
                Deck? deck = db.Decks
                    .Include(d => d.User)
                    .Include(d => d.Cards)
                    .FirstOrDefault(d => d.Id == id);
                
                if (deck is null) return Results.NotFound();
                
                if (deck.User.Username != username && !user.IsAdmin)
                {
                    return Results.Forbid();
                }

                deck.Name = request.Name;
                SetDeckCards(deck, request.CardIds, db);

                db.Decks.Update(deck);
                db.SaveChanges();

                return Results.Ok(deck); // TODO: Map to a return object?
            })
            .WithName("ModifyDeck")
            .WithDescription("Modifies a deck in the system")
            .RequireAuthorization()
            .Produces<Deck>()
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status403Forbidden)
            .Produces(StatusCodes.Status404NotFound);
    }

    private static void SetDeckCards(Deck deck, IEnumerable<int> cardIds, CardTrackerDbContext db)
    {
        deck.Cards.Clear();
        foreach (var group in cardIds.GroupBy(c => c))
        {
            Card? card = db.Cards.FirstOrDefault(c => c.Id == group.Key);
            if (card is not null)
            {
                deck.Cards.AddRange(Enumerable.Repeat(card, group.Count()));
            }
        }
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

                string username = http.GetCurrentUsername();
                User user = db.Users.AsNoTracking().First(u => u.Username == username);

                if (deck.User.Id != user.Id && !user.IsAdmin)
                {
                    return Results.Forbid();
                }

                return Results.Ok(deck); // TODO: Map to a return object
            })
            .WithName("GetDeck")
            .WithDescription("Get a single deck by its ID")
            .RequireAuthorization()
            .Produces<Deck>()
            .Produces(StatusCodes.Status403Forbidden);
    }

    private static void AddGetAllDecksEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("/decks", (CardTrackerDbContext db,  HttpContext http) =>
            {
                string username = http.GetCurrentUsername();
                
                return db.Decks.AsNoTracking()
                    .Include(d => d.User)
                    .Where(d => d.User.Username == username)
                    .ToList();
            })
            .WithName("GetDecks")
            .WithDescription("Get all registered decks for your current user")
            .RequireAuthorization()
            .Produces<List<Deck>>();
    }

    private static void AddCreateDeckEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("/decks", (CreateDeckRequest request, CardTrackerDbContext db, HttpContext http) =>
            {
                string username = http.GetCurrentUsername();
                User user = db.Users.AsNoTracking().First(u => u.Username == username);
                
                Deck deck = new()
                {
                    Name = request.Name,
                    User = user
                };
                SetDeckCards(deck, request.CardIds, db);
                
                db.Decks.Add(deck);
                db.SaveChanges();
        
                return Results.Created($"/decks/{deck.Id}", deck.Id);
            })
            .WithName("AddDeck")
            .WithDescription("Adds a new deck to the system")
            .RequireAuthorization()
            .Produces(StatusCodes.Status201Created); // Any authenticated user
    }
    
    private static void AddDeleteDeckEndpoint(IEndpointRouteBuilder app)
    {
        app.MapDelete("/decks/{id}", (int id, CardTrackerDbContext db, HttpContext http) =>
            {
                string username = http.GetCurrentUsername();
                User user = db.Users.AsNoTracking().First(u => u.Username == username);
                
                Deck? deck = db.Decks
                    .Include(d => d.User)
                    .FirstOrDefault(d => d.Id == id);
                
                if (deck is null) return Results.NotFound();
                
                if (deck.User.Username != username && !user.IsAdmin)
                {
                    return Results.Forbid();
                }
                
                db.Decks.Remove(deck);
                db.SaveChanges();

                return Results.NoContent();
            })
            .WithName("AddDeck")
            .WithDescription("Adds a new deck to the system")
            .RequireAuthorization()
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status403Forbidden)
            .Produces(StatusCodes.Status404NotFound);
    }
}