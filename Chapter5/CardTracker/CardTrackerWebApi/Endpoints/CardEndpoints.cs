using AutoMapper;

namespace CardTrackerWebApi.Endpoints;

public static class CardEndpoints
{
    public static void AddCardEndpoints(this WebApplication app)
    {
        app.MapGet("/cards/{id}", HandleGetCardById)
            .WithName("GetCardById")
            .WithDescription("Gets a specific card by its ID")
            .AllowAnonymous()
            .Produces<Card>()
            .Produces(StatusCodes.Status404NotFound);
        
        app.MapGet("/cards", HandleGetAllCards)
            .WithName("GetAllCards")
            .WithDescription("Gets all cards")
            .AllowAnonymous()
            .Produces<List<Card>>();
        
        app.MapPost("/cards", HandleCreateCard)
            .WithName("CreateCard")
            .WithDescription("Creates a new card that can be included in future decks")
            .RequireAuthorization("AdminOnly")
            .Produces<CardResponse>();
        
        app.MapDelete("/cards/{id}", HandleDeleteCard)
            .WithName("DeleteCard")
            .WithDescription("Deletes a card by its ID")
            .RequireAuthorization("AdminOnly")
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status404NotFound);
    }

    private static IResult HandleDeleteCard(int id, CardsDbContext db)
    {
        Card? card = db.Cards.Find(id);

        if (card is null)
        {
            return Results.NotFound();
        }

        db.Cards.Remove(card);
        db.SaveChanges();

        return Results.NoContent();
    }

    private static IResult HandleCreateCard(CreateCardRequest request, IMapper auto, CardsDbContext db)
    {
        // Use polymorphic mapping to create the correct card type
        Card card = auto.Map<Card>(request);

        // Add it to the database
        db.Cards.Add(card);
        db.SaveChanges();
        
        return Results.Created($"/cards/{card.Id}", auto.Map<CardResponse>(card));
    }

    private static IResult HandleGetAllCards(CardsDbContext db, IMapper auto)
    {
        List<Card> cards = db.Cards.AsNoTracking().ToList();
        List<CardResponse> responses = auto.Map<List<Card>, List<CardResponse>>(cards);
        return Results.Ok(responses);
    }

    private static IResult HandleGetCardById(int id, CardsDbContext db, IMapper auto)
    {
        Card? card = db.Cards.AsNoTracking()
            .FirstOrDefault(c => c.Id == id);

        if (card is null)
        {
            return Results.NotFound();
        }

        return Results.Ok(auto.Map<CardResponse>(card));
    }
}