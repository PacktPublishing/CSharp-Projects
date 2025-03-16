using System.Text;
using CardTrackerWebApi.Configuration;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();

builder.Services.AddDbContext<CardTrackerDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// Bind the Auth settings to the configuration
builder.Services.Configure<AuthSettings>(builder.Configuration.GetSection("Auth"));

// Grab our JWT Auth key from the configuration
string authKey = builder.Configuration["Auth:Key"] ?? throw new InvalidOperationException("Auth:Key is missing and required");

// Configure dependency injection
builder.Services.AddScoped<DeckRepository>();
builder.Services.AddScoped<CardRepository>();
builder.Services.AddScoped<UserRepository>();
builder.Services.AddScoped<IHashingService, HmacHashingService>();
builder.Services.AddScoped<ITokenGenerationService, JwtGenerationService>();

// Configure authentication
builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options => options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Auth:Issuer"],
        ValidAudience = builder.Configuration["Auth:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(authKey))
    });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy => policy.RequireRole("Admin"));
});

WebApplication app = builder.Build();

// Configure the request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseDeveloperExceptionPage();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

// Endpoints

// Login endpoint
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
    .RequireAuthorization();

app.MapPost("/users", (CreateUserRequest userRequest, UserRepository users, IHashingService hasher) =>
    {
        byte[] salt = hasher.GenerateSalt();
        byte[] hash = hasher.ComputeHash(userRequest.Password, salt);

        return users.AddUser(new()
        {
            Username = userRequest.Username,
            Salt = salt,
            PasswordHash = hash,
        });
    })
    .WithName("AddUser")
    .WithDescription("Adds a new user to the system")
    .AllowAnonymous();

app.Run();