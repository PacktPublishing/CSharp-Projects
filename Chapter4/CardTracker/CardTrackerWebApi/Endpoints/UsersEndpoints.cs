namespace CardTrackerWebApi.Endpoints;

public static class UsersEndpoints
{
    public static void AddUsersEndpoints(this WebApplication app)
    {
        AddGetUserEndpoint(app);
        AddGetUsersEndpoint(app);

        app.MapPost("/users", (CreateUserRequest userRequest, CardTrackerDbContext context, IHashingService hasher) =>
            {
                userRequest.Username = userRequest.Username.ToLowerInvariant();
                if (context.Users.AsNoTracking().Any(u => u.Username == userRequest.Username)) 
                {
                    return Results.BadRequest($"A user already exists with a username of {userRequest.Username}");
                }
        
                byte[] salt = hasher.GenerateSalt();
                byte[] hash = hasher.ComputeHash(userRequest.Password, salt);
        
                User user = new()
                {
                    Username = userRequest.Username,
                    Salt = salt,
                    PasswordHash = hash
                };
    
                context.Users.Add(user);
                context.SaveChanges();
        
                return Results.Created($"/users/{userRequest.Username}", user.Id);
            })
            .WithName("AddUser")
            .WithDescription("Adds a new user to the system")
            .AllowAnonymous();
    }

    private static void AddGetUserEndpoint(WebApplication app)
    {
        app.MapGet("/users/{username}", (string username, CardTrackerDbContext context) =>
            {
                User? user = context.Users.AsNoTracking()
                    .FirstOrDefault(u => u.Username == username.ToLowerInvariant());

                if (user is null)
                {
                    return Results.NotFound();
                }

                return Results.Ok(user);
            })
            .WithName("GetUserByUsername")
            .WithDescription("Gets a specific user by their username")
            .RequireAuthorization("AdminOnly")
            .Produces<User>()
            .Produces(StatusCodes.Status404NotFound);
    }

    private static void AddGetUsersEndpoint(this WebApplication app)
    {
        app.MapGet("/users", async (CardTrackerDbContext context) => await context.Users.AsNoTracking().ToListAsync())
            .WithName("GetUsers")
            .WithDescription("Get all registered users")
            .RequireAuthorization("AdminOnly")
            .Produces<List<User>>();
    }
}