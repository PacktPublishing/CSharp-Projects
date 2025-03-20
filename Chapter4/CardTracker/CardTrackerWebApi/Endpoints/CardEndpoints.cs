namespace CardTrackerWebApi.Endpoints;

public static class CardEndpoints
{
    public static void AddCardEndpoints(this WebApplication app)
    {
        AddGetCardEndpoint(app);
        AddGetAllCardsEndpoint(app);
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
        app.MapGet("/cards", (CardTrackerDbContext db) => db.Cards.AsNoTracking().ToList())
            .WithName("GetAllCards")
            .WithDescription("Gets all cards")
            .AllowAnonymous()
            .Produces<List<Card>>();
    }

    private static void AddCreateActionCardEndpoint(WebApplication app)
    {
        app.MapPost("/cards/action", (CreateActionCardRequest request, CardTrackerDbContext db) =>
            {
                ActionCard card = new() // TODO: Automapper would help here
                {
                    Name = request.Name,
                    Description = request.Description,
                    ImagePath = request.ImagePath,
                    Effect = request.Effect,
                    Cost = request.Cost
                };
                
                db.ActionCards.Add(card);
                db.SaveChanges();

                return Results.Created($"/cards/{card.Id}", card);
            })
            .WithName("CreateActionCard")
            .WithDescription("Creates a new action card that can be included in future decks")
            .RequireAuthorization("AdminOnly")
            .Produces<ActionCard>();
    }
    
    private static void AddCreateCreatureCardEndpoint(WebApplication app)
    {
        app.MapPost("/cards/creature", (CreateCreatureCardRequest request, CardTrackerDbContext db) =>
            {
                CreatureCard card = new() // TODO: Automapper would help here
                {
                    Name = request.Name,
                    Description = request.Description,
                    ImagePath = request.ImagePath,
                    SummonEffect = request.SummonEffect,
                    PerTurnEffect = request.PerTurnEffect,
                    SummonCost = request.Cost,
                    Power = request.Power,
                    CanFly = request.CanFly,
                    CanSwim = request.CanSwim,
                    CanClimb = request.CanClimb
                };
                
                db.CreatureCards.Add(card);
                db.SaveChanges();

                return Results.Created($"/cards/{card.Id}", card);
            })
            .WithName("CreateCreatureCard")
            .WithDescription("Creates a new creature card that can be included in future decks")
            .RequireAuthorization("AdminOnly")
            .Produces<CreatureCard>();
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