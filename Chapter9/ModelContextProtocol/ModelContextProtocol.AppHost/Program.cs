var builder = DistributedApplication.CreateBuilder(args);

var apiService = builder.AddProject<Projects.ModelContextProtocol_ApiService>("apiservice");

builder.AddProject<Projects.ModelContextProtocol_Web>("webfrontend")
    .WithExternalHttpEndpoints()
    .WithReference(apiService)
    .WaitFor(apiService);

//var mcpServer = builder.AddProject<Projects.ModelContextProtocol_CustomServer>("mcpserver");

builder.AddProject<Projects.ModelContextProtocol_TestClient>("mcpclient");
//    .WithReference(mcpServer)
    //.WaitFor(mcpServer);

builder.Build().Run();
