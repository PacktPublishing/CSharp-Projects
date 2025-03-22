using AutoMapper;

namespace CardTrackerWebApi.Endpoints;

public static class CardEndpoints
{
    public static void AddCardEndpoints(this WebApplication app)
    {
        AddGetCardEndpoint(app);
        AddGetAllCardsEndpoint(app);
        AddGetAllCreatureCardsEndpoint(app);
        AddGetAllActionCardsEndpoint(app);
        AddCreateCreatureCardEndpoint(app);
        AddCreateActionCardEndpoint(app);
        AddDeleteCardEndpoint(app);
    }

    private static void AddGetCardEndpoint(WebApplication app)
    {
        app.MapGet("/cards/{id}", (int id, CardTrackerDbContext db) =>
            {
                Card? card = db.Cards.AsNoTracking()
                    .FirstOrDefault(c => c.Id == id);

                if (card is null)
                {
                    return Results.NotFound();
                }

                return Results.Ok(card);
            })
            .WithName("GetCardById")
            .WithDescription("Gets a specific card by its ID")
            .AllowAnonymous()
            .Produces<Card>()
            .Produces(StatusCodes.Status404NotFound);
    }

    private static void AddGetAllCardsEndpoint(WebApplication app)
    {
        app.MapGet("/cards", (CardTrackerDbContext db, IMapper auto) =>
            {
                List<Card> cards = db.Cards.AsNoTracking().ToList();
                return auto.Map<List<Card>, List<CardResponse>>(cards);
            })
            .WithName("GetAllCards")
            .WithDescription("Gets all cards")
            .AllowAnonymous()
            .Produces<List<Card>>();
    }
    
    private static void AddGetAllCreatureCardsEndpoint(WebApplication app)
    {
        app.MapGet("/cards/creatures", (CardTrackerDbContext db, IMapper auto) =>
            {
                IQueryable<CreatureCard> cards = db.CreatureCards.AsNoTracking();
                return auto.Map<IEnumerable<CreatureCardResponse>>(cards);
            })
            .WithName("GetAllCreatureCards")
            .WithDescription("Gets all creature cards")
            .AllowAnonymous()
            .Produces<List<CreatureCardResponse>>();
    }
    
    private static void AddGetAllActionCardsEndpoint(WebApplication app)
    {
        app.MapGet("/cards/actions", (CardTrackerDbContext db, IMapper auto) =>
            {
                IQueryable<ActionCard> cards = db.ActionCards.AsNoTracking();
                return auto.Map<IEnumerable<ActionCardResponse>>(cards);
            })
            .WithName("GetAllActionCards")
            .WithDescription("Gets all action cards")
            .AllowAnonymous()
            .Produces<List<ActionCard>>();
    }

    private static void AddCreateActionCardEndpoint(WebApplication app)
    {
        app.MapPost("/cards/actions", (CreateActionCardRequest request, IMapper auto, CardTrackerDbContext db) =>
            {
                ActionCard card = auto.Map<ActionCard>(request);
                
                db.ActionCards.Add(card);
                db.SaveChanges();

                return Results.Created($"/cards/{card.Id}", auto.Map<ActionCardResponse>(card));
            })
            .WithName("CreateActionCard")
            .WithDescription("Creates a new action card that can be included in future decks")
            .RequireAuthorization("AdminOnly")
            .Produces<ActionCardResponse>();
    }
    
    private static void AddCreateCreatureCardEndpoint(WebApplication app)
    {
        app.MapPost("/cards/creatures", (CreateCreatureCardRequest request, IMapper auto, CardTrackerDbContext db) =>
            {
                CreatureCard card = auto.Map<CreatureCard>(request);
                
                db.CreatureCards.Add(card);
                db.SaveChanges();

                return Results.Created($"/cards/{card.Id}", auto.Map<CreatureCardResponse>(card));
            })
            .WithName("CreateCreatureCard")
            .WithDescription("Creates a new creature card that can be included in future decks")
            .RequireAuthorization("AdminOnly")
            .Produces<CreatureCardResponse>();
    }

    private static void AddDeleteCardEndpoint(WebApplication app)
    {
        app.MapDelete("/cards/{id}", (int id, CardTrackerDbContext db) =>
            {
                Card? card = db.Cards.Find(id);

                if (card is null)
                {
                    return Results.NotFound();
                }

                db.Cards.Remove(card);
                db.SaveChanges();

                return Results.NoContent();
            })
            .WithName("DeleteCard")
            .WithDescription("Deletes a card by its ID")
            .RequireAuthorization("AdminOnly")
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status404NotFound);
    }
}