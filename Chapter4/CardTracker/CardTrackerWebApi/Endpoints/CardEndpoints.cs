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
        
        app.MapGet("/cards/creatures", HandleGetAllCreatureCards)
            .WithName("GetAllCreatureCards")
            .WithDescription("Gets all creature cards")
            .AllowAnonymous()
            .Produces<List<CreatureCardResponse>>();
        
        app.MapGet("/cards/actions", HandleGetAllActionCards)
            .WithName("GetAllActionCards")
            .WithDescription("Gets all action cards")
            .AllowAnonymous()
            .Produces<List<ActionCard>>();
        
        app.MapPost("/cards/creatures", HandleCreateCreature)
            .WithName("CreateCreatureCard")
            .WithDescription("Creates a new creature card that can be included in future decks")
            .RequireAuthorization("AdminOnly")
            .Produces<CreatureCardResponse>();
        
        app.MapPost("/cards/actions", HandleCreateAction)
            .WithName("CreateActionCard")
            .WithDescription("Creates a new action card that can be included in future decks")
            .RequireAuthorization("AdminOnly")
            .Produces<ActionCardResponse>();
        
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

    private static IResult HandleCreateAction(CreateActionCardRequest request, IMapper auto, CardsDbContext db)
    {
        ActionCard card = auto.Map<ActionCard>(request);

        db.ActionCards.Add(card);
        db.SaveChanges();

        return Results.Created($"/cards/{card.Id}", auto.Map<ActionCardResponse>(card));
    }

    private static IResult HandleCreateCreature(CreateCreatureCardRequest request, IMapper auto, CardsDbContext db)
    {
        CreatureCard card = auto.Map<CreatureCard>(request);

        db.CreatureCards.Add(card);
        db.SaveChanges();

        return Results.Created($"/cards/{card.Id}", auto.Map<CreatureCardResponse>(card));
    }

    private static IResult HandleGetAllActionCards(CardsDbContext db, IMapper auto)
    {
        IQueryable<ActionCard> cards = db.ActionCards.AsNoTracking();
        return Results.Ok(auto.Map<IEnumerable<ActionCardResponse>>(cards));
    }

    private static IResult HandleGetAllCreatureCards(CardsDbContext db, IMapper auto)
    {
        IQueryable<CreatureCard> cards = db.CreatureCards.AsNoTracking();
        return Results.Ok(auto.Map<IEnumerable<CreatureCardResponse>>(cards));
    }

    private static IResult HandleGetAllCards(CardsDbContext db, IMapper auto)
    {
        List<Card> cards = db.Cards.AsNoTracking().ToList();
        return Results.Ok(auto.Map<List<Card>, List<CardResponse>>(cards));
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