namespace CardTrackerWebApi.Endpoints;

public static class LoginEndpoints
{
    public static void AddLoginEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapPost("/login", HandleLogin)
            .WithName("Login")
            .WithDescription("Login a user")
            .AllowAnonymous()
            .Produces<LoginResponse>()
            .Produces(StatusCodes.Status400BadRequest);
    }

    private static IResult HandleLogin(LoginRequest request, CardsDbContext db, IHashingService hasher, ITokenGenerationService tokenGenerator)
    {
        request.Username = request.Username.ToLowerInvariant();
        User? user = db.Users.AsNoTracking().FirstOrDefault(u => u.Username == request.Username);

        if (user == null)
        {
            return Results.BadRequest("Invalid username or password.");
        }

        byte[] hash = hasher.ComputeHash(request.Password, user.Salt);
        if (!user.PasswordHash.SequenceEqual(hash))
        {
            return Results.BadRequest("Invalid username or password.");
        }

        // Generate and return a signed JWT
        string role = user.IsAdmin ? "Admin" : "User";
        string jwt = tokenGenerator.GenerateToken(request.Username, role);

        return Results.Ok(new LoginResponse(jwt));
    }
}