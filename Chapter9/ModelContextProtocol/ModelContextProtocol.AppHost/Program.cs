var builder = DistributedApplication.CreateBuilder(args);

var apiService = builder.AddProject<Projects.ModelContextProtocol_ApiService>("apiservice");

builder.AddProject<Projects.ModelContextProtocol_Web>("webfrontend")
    .WithExternalHttpEndpoints()
    .WithReference(apiService)
    .WaitFor(apiService);

//var mcpServer = builder.AddProject<Projects.ModelContextProtocol_CustomServer>("mcpserver");

// TODO: It'd be nice to launch the server explicitly here and have the client somehow talk to it

builder.AddProject<Projects.ModelContextProtocol_TestClient>("mcpclient")
       .WithReference(apiService)
       .WaitFor(apiService);

builder.Build().Run();
