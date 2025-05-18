var builder = DistributedApplication.CreateBuilder(args);

var apiService = builder.AddProject<Projects.ModelContextProtocol_ApiService>("apiservice");

builder.AddProject<Projects.ModelContextProtocol_Web>("webfrontend")
    .WithExternalHttpEndpoints()
    .WithReference(apiService)
    .WaitFor(apiService);

builder.Build().Run();
