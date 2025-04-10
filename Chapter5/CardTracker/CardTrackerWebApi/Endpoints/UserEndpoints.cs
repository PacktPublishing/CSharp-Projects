using AutoMapper;
using CardTracker.Contracts.Requests;
using CardTracker.Contracts.Responses;

namespace CardTrackerWebApi.Endpoints;

public static class UserEndpoints
{
    public static void AddUsersEndpoints(this WebApplication app)
    {
        app.MapGet("/users/{username}", HandleGetUserByUsername)
            .WithName("GetUserByUsername")
            .WithDescription("Gets a specific user by their username")
            .RequireAuthorization("AdminOnly")
            .Produces<UserResponse>()
            .Produces(StatusCodes.Status404NotFound);
        
        app.MapGet("/users", HandleGetAllUsers)
            .WithName("GetUsers")
            .WithDescription("Get all registered users")
            .RequireAuthorization("AdminOnly")
            .Produces<List<UserResponse>>();
        
        app.MapPost("/users", HandleAddUser)
            .WithName("AddUser")
            .WithDescription("Adds a new user to the system")
            .AllowAnonymous()
            .Produces<UserResponse>(StatusCodes.Status204NoContent);
    }

    private static IResult HandleGetAllUsers(CardsDbContext context, IMapper auto)
    {
        IQueryable<User> users = context.Users.AsNoTracking();

        List<UserResponse> usersResponse = auto.Map<List<UserResponse>>(users);
        return Results.Ok(usersResponse);
    }

    private static IResult HandleAddUser(CreateUserRequest userRequest, CardsDbContext context, IMapper auto, IHashingService hasher)
    {
        userRequest.Username = userRequest.Username.ToLowerInvariant();
        if (context.Users.AsNoTracking().Any(u => u.Username == userRequest.Username))
        {
            return Results.BadRequest($"A user already exists with a username of {userRequest.Username}");
        }

        byte[] salt = hasher.GenerateSalt();
        byte[] hash = hasher.ComputeHash(userRequest.Password, salt);

        User user = new() { Username = userRequest.Username, Salt = salt, PasswordHash = hash };

        context.Users.Add(user);
        context.SaveChanges();

        UserResponse userResponse = auto.Map<UserResponse>(user);
        return Results.Created($"/users/{userRequest.Username}", userResponse);
    }

    private static IResult HandleGetUserByUsername(string username, IMapper auto, CardsDbContext context)
    {
        User? user = context.Users.AsNoTracking()
            .FirstOrDefault(u => u.Username == username.ToLowerInvariant());

        if (user is null)
        {
            return Results.NotFound();
        }

        UserResponse result = auto.Map<UserResponse>(user);
        return Results.Ok(result);
    }
}