var builder = DistributedApplication.CreateBuilder(args);

var apiService = builder.AddProject<Projects.ModelContextProtocol_ApiService>("apiservice");

//var mcpServer = builder.AddProject<Projects.ModelContextProtocol_CustomServer>("mcpserver");

// TODO: It'd be nice to launch the server explicitly here and have the client somehow talk to it

var docs = builder.AddProject<Projects.ModelContextProtocol_DocumentsApi>("documentsapi");
var chat = builder.AddProject<Projects.ModelContextProtocol_ChatApi>("chatapi")
    .WithReference(docs)
    .WaitFor(docs);

builder.AddProject<Projects.ModelContextProtocol_Web>("webfrontend")
    .WithExternalHttpEndpoints()
    .WithReference(apiService)
    .WithReference(chat)
    .WaitFor(apiService)
    .WaitFor(docs)
    .WaitFor(chat);

builder.AddProject<Projects.ModelContextProtocol_TestClient>("mcpclient")
    .WithReference(docs)
    .WithReference(apiService)
    .WaitFor(docs)
    .WaitFor(apiService);

builder.Build().Run();
