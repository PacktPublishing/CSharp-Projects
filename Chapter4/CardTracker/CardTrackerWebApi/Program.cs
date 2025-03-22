using CardTrackerWebApi.Helpers;
using CardTrackerWebApi.Settings;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services.AddDbContext<CardTrackerDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// Bind the Auth settings to the configuration
builder.Services.Configure<AuthSettings>(builder.Configuration.GetSection("Auth"));

// Configure dependency injection
builder.Services.AddScoped<IHashingService, HmacHashingService>();
builder.Services.AddScoped<ITokenGenerationService, JwtGenerationService>();

// Configure automapper
builder.Services.AddAutoMapper(map =>
{
    // Simple mappings
    map.CreateMap<User, UserResponse>();
    map.CreateMap<Deck, DeckResponse>();
    map.CreateMap<CreateActionCardRequest, ActionCard>();
    map.CreateMap<ActionCard, ActionCardResponse>();
    
    // We can customize property mappings if needed
    map.CreateMap<CreatureCard, CreatureCardResponse>()
        .ForMember(response => response.Cost, opt => opt.MapFrom(card => card.SummonCost));
    map.CreateMap<CreateCreatureCardRequest, CreatureCard>()
        .ForMember(card => card.SummonCost, opt => opt.MapFrom(request => request.Cost));
    
    // Set up polymorphic mapping
    map.CreateMap<Card, CardResponse>()
        .Include<ActionCard, ActionCardResponse>()
        .Include<CreatureCard, CreatureCardResponse>();
});

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
}

app.UseHttpsRedirection();

// Endpoints
app.AddLoginEndpoints();
app.AddDeckEndpoints();
app.AddUsersEndpoints();
app.AddCardEndpoints();

app.Run();