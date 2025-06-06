using AutoMapper;
using CardTracker.Contracts.Requests;
using CardTracker.Contracts.Responses;
using CardDeck = CardTrackerWebApi.Models.CardDeck;

namespace CardTrackerWebApi.Endpoints;

public static class DeckEndpoints
{
    public static void AddDeckEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGet("/decks", HandleGetDecks)
            .WithName("GetDecks")
            .WithDescription("Get all registered decks for your current user")
            .RequireAuthorization()
            .Produces<List<DeckResponse>>();
        
        app.MapGet("/decks/{id}", HandleGetDeckById)
            .WithName("GetDeck")
            .WithDescription("Get a single deck by its ID")
            .RequireAuthorization()
            .Produces<DeckResponse>()
            .Produces(StatusCodes.Status403Forbidden);
        
        app.MapPost("/decks", HandleCreateDeck)
            .WithName("AddDeck")
            .WithDescription("Adds a new deck to the system")
            .RequireAuthorization()
            .Produces(StatusCodes.Status201Created);
        
        app.MapDelete("/decks/{id}", HandleDeleteDeck)
            .WithName("DeleteDeck")
            .WithDescription("Deletes an existing deck from the system")
            .RequireAuthorization()
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status403Forbidden)
            .Produces(StatusCodes.Status404NotFound);
        
        app.MapPut("/decks/{id}", HandleEditDeck)
            .WithName("ModifyDeck")
            .WithDescription("Modifies a deck in the system")
            .RequireAuthorization()
            .Produces<DeckResponse>()
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status403Forbidden)
            .Produces(StatusCodes.Status404NotFound);
    }

    private static IResult HandleEditDeck(int id, EditDeckRequest request, CardsDbContext db, IMapper auto, HttpContext http)
    {
        string username = http.GetCurrentUsername();
        User user = db.Users.AsNoTracking().First(u => u.Username == username);

        Deck? deck = db.Decks.Include(d => d.CardDecks)
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

            deck.CardDecks.Add(new CardDeck { CardId = card.Id, DeckId = deck.Id, Count = group.Count() });
        }

        db.Decks.Update(deck);
        db.SaveChanges();

        return Results.Ok(auto.Map<DeckResponse>(deck));
    }

    private static IResult HandleDeleteDeck(int id, CardsDbContext db, HttpContext http)
    {
        string username = http.GetCurrentUsername();
        User user = db.Users.AsNoTracking().First(u => u.Username == username);

        Deck? deck = db.Decks.FirstOrDefault(d => d.Id == id);

        if (deck is null) return Results.NotFound();

        if (deck.UserId != user.Id && !user.IsAdmin)
        {
            return Results.Forbid();
        }

        db.Decks.Remove(deck);
        db.SaveChanges();

        return Results.NoContent();
    }

    private static IResult HandleCreateDeck(CreateDeckRequest request, CardsDbContext db, HttpContext http, IMapper auto)
    {
        string username = http.GetCurrentUsername();
        User user = db.Users.AsNoTracking().First(u => u.Username == username);

        Deck deck = new()
        {
            Name = request.Name, UserId = user.Id,
            // We don't set the cards here. We first create a deck, then the user can add cards to it
        };

        db.Decks.Add(deck);
        db.SaveChanges();

        return Results.Created($"/decks/{deck.Id}", auto.Map<DeckResponse>(deck));
    }

    private static IResult HandleGetDeckById(int id, CardsDbContext db, HttpContext http, IMapper auto)
    {
        Deck? deck = db.Decks.Include(d => d.CardDecks)
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
    }

    private static IResult HandleGetDecks(CardsDbContext db, HttpContext http, IMapper auto)
    {
        string username = http.GetCurrentUsername();
        User user = db.Users.AsNoTracking().First(u => u.Username == username);

        IQueryable<Deck> decks = db.Decks.AsNoTracking()
            .Where(d => d.UserId == user.Id)
            .Include(d => d.CardDecks);

        return Results.Ok(auto.Map<List<Deck>>(decks));
    }
}