using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using CardTrackerClient;
using CardTrackerClient.Services;

WebAssemblyHostBuilder builder = WebAssemblyHostBuilder.CreateDefault(args);

string apiBaseUrl = builder.Configuration["ApiBaseUrl"]!;

// Tell the app how to interact with the web page
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// Dependency injection for services
builder.Services.AddScoped<ICardApiService, CardApiService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<HttpClient>(_ => new HttpClient { BaseAddress = new Uri(apiBaseUrl) });

await builder.Build().RunAsync();