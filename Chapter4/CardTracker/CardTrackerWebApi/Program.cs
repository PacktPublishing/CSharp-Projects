using System.Text;
using CardTrackerWebApi.Configuration;
using CardTrackerWebApi.Helpers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;

IdentityModelEventSource.ShowPII = true;
WebApplicationBuilder builder = WebApplication.CreateBuilder(args);


builder.Services.AddOpenApi();

builder.Services.AddDbContext<CardTrackerDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// Bind the Auth settings to the configuration
builder.Services.Configure<AuthSettings>(builder.Configuration.GetSection("Auth"));

// Configure dependency injection
builder.Services.AddScoped<DeckRepository>();
builder.Services.AddScoped<CardRepository>();
builder.Services.AddScoped<UserRepository>();
builder.Services.AddScoped<IHashingService, HmacHashingService>();
builder.Services.AddScoped<ITokenGenerationService, JwtGenerationService>();

// Configure authentication
AuthSettings jwtSettings = builder.Configuration.GetRequiredSection("Auth").Get<AuthSettings>()!;
builder.Services.AddJwtAuthentication(jwtSettings);
builder.Services.AddAuthorization(o =>
{
    o.AddPolicy("AdminOnly", p => p.RequireRole("Admin"));
});

WebApplication app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();

// Configure the request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseDeveloperExceptionPage();
    IdentityModelEventSource.ShowPII = true;
}

app.UseHttpsRedirection();

// Endpoints

app.MapPost("/login", async (LoginRequest loginRequest, UserRepository users, IHashingService hasher, ITokenGenerationService tokenGenerator) =>
    {
        User? user = await users.GetUserAsync(loginRequest.Username);
        
        if (user == null)
        {
            return Results.BadRequest("Invalid username or password.");
        }

        byte[] hash = hasher.ComputeHash(loginRequest.Password, user.Salt);
        if (!user.PasswordHash.SequenceEqual(hash))
        {
            return Results.BadRequest("Invalid username or password.");
        }
        
        // Generate and return a signed JWT
        string role = user.IsAdmin ? "Admin" : "User";
        return Results.Ok(new LoginResponse
        {
            Token = tokenGenerator.GenerateToken(loginRequest.Username, role)
        });
    })
    .WithName("Login")
    .WithDescription("Login a user")
    .AllowAnonymous();

app.MapGet("/decks", (DeckRepository decks) => decks.GetAllDecks())
   .WithName("GetDecks")
   .WithDescription("Get all registered decks")
   .AllowAnonymous();

// TODO: Get a single deck
// TODO: Add a deck
// TODO: Delete a deck

// TODO: Get a specific card
// TODO: Get all cards (paged)
// TODO: Search for cards (paged)
// TODO: Add a card (admin only)
// TODO: Delete a card (admin only)
// TODO: Update a card (admin only)

// TODO: Add a card to a deck
// TODO: Remove a card from a deck
// TODO: Validate decks

app.MapGet("/users", async (UserRepository users) => await users.GetAllUsers())
    .WithName("GetUsers")
    .WithDescription("Get all registered users")
    .RequireAuthorization("AdminOnly");

app.MapGet("/users/{username}", async (string username, UserRepository users) => await users.GetUserAsync(username))
    .WithName("GetUserByUsername")
    .WithDescription("Gets a specific user by their username")
    .RequireAuthorization("AdminOnly");

app.MapPost("/users", (CreateUserRequest userRequest, UserRepository users, IHashingService hasher) =>
    {
        byte[] salt = hasher.GenerateSalt();
        byte[] hash = hasher.ComputeHash(userRequest.Password, salt);

        try {
            string route = $"/users/{userRequest.Username}";
            return Results.Created(route, users.AddUser(new()
            {
                Username = userRequest.Username.ToLowerInvariant(),
                Salt = salt,
                PasswordHash = hash,
                IsAdmin = userRequest.IsAdmin
            }));
        }
        catch (InvalidOperationException e)
        {
            return Results.BadRequest(e.Message);
        }
    })
    .WithName("AddUser")
    .WithDescription("Adds a new user to the system")
    .RequireAuthorization("AdminOnly");

app.Run();