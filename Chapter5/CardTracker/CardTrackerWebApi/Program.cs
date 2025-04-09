using CardTrackerWebApi.Converters;
using CardTrackerWebApi.Helpers;
using CardTrackerWebApi.Settings;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using JsonOptions = Microsoft.AspNetCore.Http.Json.JsonOptions;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services.AddDbContext<CardsDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// Configure dependency injection
builder.Services.AddScoped<IHashingService, HmacHashingService>();
builder.Services.AddScoped<ITokenGenerationService, JwtGenerationService>();

// Configure automapper
builder.Services.AddAutoMapper(map =>
{
    // Simple mappings
    map.CreateMap<User, UserResponse>();
    map.CreateMap<Deck, DeckResponse>();
    map.CreateMap<ActionCard, ActionCardResponse>();
    map.CreateMap<CreatureCard, CreatureCardResponse>();
    map.CreateMap<CreateActionCardRequest, ActionCard>();
    map.CreateMap<CreateCreatureCardRequest, CreatureCard>();
    
    // Polymorphic mappings
    map.CreateMap<Card, CardResponse>()
        .Include<ActionCard, ActionCardResponse>()
        .Include<CreatureCard, CreatureCardResponse>();
    map.CreateMap<CreateCardRequest, Card>()
        .Include<CreateActionCardRequest, ActionCard>()
        .Include<CreateCreatureCardRequest, CreatureCard>();
});

// Allow polymorphic mapping of create card requests
builder.Services.Configure<JsonOptions>(o =>
{
    o.SerializerOptions.Converters.Add(new CreateCardRequestConverter());
});

// Configure authentication
builder.Services.Configure<AuthSettings>(builder.Configuration.GetSection("Auth"));
AuthSettings jwtSettings = builder.Configuration.GetRequiredSection("Auth").Get<AuthSettings>()!;
builder.Services.AddJwtAuthentication(jwtSettings);
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy => policy.RequireRole("Admin"));
});

WebApplication app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();

// Configure the request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseDeveloperExceptionPage();
}

app.UseHttpsRedirection();

// Endpoints
app.AddLoginEndpoints();
app.AddDeckEndpoints();
app.AddUsersEndpoints();
app.AddCardEndpoints();

// See RFC 2324 for an explanation of the 418 status code added as an April Fool's joke
// https://www.rfc-editor.org/rfc/rfc2324#section-2.3.2
app.MapGet("/tea", () => Results.StatusCode(StatusCodes.Status418ImATeapot));

app.Run();