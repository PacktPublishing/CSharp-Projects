using AutoMapper;

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
        app.MapPut("/decks/{id}", (int id, EditDeckRequest request, CardTrackerDbContext db, IMapper auto, HttpContext http) =>
            {
                string username = http.GetCurrentUsername();
                User user = db.Users.AsNoTracking().First(u => u.Username == username);
                
                Deck? deck = db.Decks
                    .Include(d => d.CardDecks)
                    .FirstOrDefault(d => d.Id == id);
                
                if (deck is null) return Results.NotFound();
                
                if (deck.UserId != user.Id && !user.IsAdmin)
                {
                    return Results.Forbid();
                }

                deck.Name = request.Name;
                deck.CardDecks.Clear();
                foreach (var group in request.CardIds.GroupBy(c => c))
                {
                    Card? card = db.Cards.FirstOrDefault(c => c.Id == group.Key);
                    if (card is null)
                    {
                        return Results.BadRequest($"Card {group.Key} not found");
                    }
            
                    deck.CardDecks.Add(new CardDeck
                    {
                        CardId = card.Id,
                        DeckId = deck.Id,
                        Count = group.Count()
                    });
                }

                db.Decks.Update(deck);
                db.SaveChanges();

                return Results.Ok(auto.Map<DeckResponse>(deck));
            })
            .WithName("ModifyDeck")
            .WithDescription("Modifies a deck in the system")
            .RequireAuthorization()
            .Produces<DeckResponse>()
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status403Forbidden)
            .Produces(StatusCodes.Status404NotFound);
    }

    private static void AddGetDeckEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("/decks/{id}", (int id, CardTrackerDbContext db, HttpContext http, IMapper auto) =>
            {
                Deck? deck = db.Decks
                    .Include(d => d.CardDecks)
                    .AsNoTracking()
                    .FirstOrDefault(d => d.Id == id);

                if (deck is null)
                {
                    return Results.NotFound();
                }

                string username = http.GetCurrentUsername();
                User user = db.Users.AsNoTracking().First(u => u.Username == username);

                if (deck.UserId != user.Id && !user.IsAdmin)
                {
                    return Results.Forbid();
                }

                return Results.Ok(auto.Map<DeckResponse>(deck));
            })
            .WithName("GetDeck")
            .WithDescription("Get a single deck by its ID")
            .RequireAuthorization()
            .Produces<DeckResponse>()
            .Produces(StatusCodes.Status403Forbidden);
    }

    private static void AddGetAllDecksEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("/decks", (CardTrackerDbContext db,  HttpContext http, IMapper auto) =>
            {
                string username = http.GetCurrentUsername();
                User user = db.Users.AsNoTracking().First(u => u.Username == username);

                IQueryable<Deck> decks = db.Decks.AsNoTracking()
                    .Where(d => d.UserId == user.Id);
                
                return Results.Ok(auto.Map<List<Deck>>(decks));
            })
            .WithName("GetDecks")
            .WithDescription("Get all registered decks for your current user")
            .RequireAuthorization()
            .Produces<List<DeckResponse>>();
    }

    private static void AddCreateDeckEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("/decks", (CreateDeckRequest request, CardTrackerDbContext db, HttpContext http, IMapper auto) =>
            {
                string username = http.GetCurrentUsername();
                User user = db.Users.AsNoTracking().First(u => u.Username == username);
                
                Deck deck = new()
                {
                    Name = request.Name,
                    UserId = user.Id,
                    // We don't set the cards here. We first create a deck, then the user can add cards to it
                };
                
                db.Decks.Add(deck);
                db.SaveChanges();
        
                return Results.Created($"/decks/{deck.Id}", auto.Map<DeckResponse>(deck));
            })
            .WithName("AddDeck")
            .WithDescription("Adds a new deck to the system")
            .RequireAuthorization()
            .Produces(StatusCodes.Status201Created);
    }
    
    private static void AddDeleteDeckEndpoint(IEndpointRouteBuilder app)
    {
        app.MapDelete("/decks/{id}", (int id, CardTrackerDbContext db, HttpContext http) =>
            {
                string username = http.GetCurrentUsername();
                User user = db.Users.AsNoTracking().First(u => u.Username == username);
                
                Deck? deck = db.Decks
                    .FirstOrDefault(d => d.Id == id);
                
                if (deck is null) return Results.NotFound();
                
                if (deck.UserId != user.Id && !user.IsAdmin)
                {
                    return Results.Forbid();
                }
                
                db.Decks.Remove(deck);
                db.SaveChanges();

                return Results.NoContent();
            })
            .WithName("DeleteDeck")
            .WithDescription("Deletes an existing deck from the system")
            .RequireAuthorization()
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status403Forbidden)
            .Produces(StatusCodes.Status404NotFound);
    }
}