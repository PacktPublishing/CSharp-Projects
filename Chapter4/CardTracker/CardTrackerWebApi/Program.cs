using CardTrackerWebApi.Filters;
using Microsoft.EntityFrameworkCore;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();

builder.Services.AddDbContext<CardTrackerDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// Configure dependency injection
builder.Services.AddScoped<DeckRepository>();
builder.Services.AddScoped<CardRepository>();
builder.Services.AddScoped<UserRepository>();

// TODO: Will want some basic authentication

WebApplication app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.MapGet("/decks", (DeckRepository decks) => decks.GetAllDecks())
   .WithName("GetDecks")
   .WithDescription("Get all registered decks");

app.MapGet("/users", async (UserRepository users) => await users.GetAllUsers())
    .WithName("GetUsers")
    .WithDescription("Get all registered users")
    .WithMetadata(new AuthorizationHeaderFilter(builder.Configuration.GetValue<string>("ApiKey")));

app.MapPost("/users", (User user, UserRepository users) => users.AddUser(user))
    .WithName("AddUser")
    .WithDescription("Adds a new user to the system")
    .WithMetadata(new AuthorizationHeaderFilter(builder.Configuration.GetValue<string>("ApiKey")));

app.Run();