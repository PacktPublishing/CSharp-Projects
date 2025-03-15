using Microsoft.EntityFrameworkCore;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();

builder.Services.AddDbContext<CardTrackerDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// Configure dependency injection
builder.Services.AddScoped<DeckRepository>();
builder.Services.AddScoped<CardRepository>();

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

app.Run();