var builder = DistributedApplication.CreateBuilder(args);

var mcpServer = builder.AddProject<Projects.ModelContextProtocol_CustomServer>("mcpserver");

// TODO: It'd be nice to launch the server explicitly here and have the client somehow talk to it

var docs = builder.AddProject<Projects.ModelContextProtocol_DocumentsApi>("documentsapi");

var chat = builder.AddProject<Projects.ModelContextProtocol_ChatApi>("chatapi")
    .WithReference(docs)
    .WaitFor(mcpServer)
    .WaitFor(docs);

builder.AddProject<Projects.ModelContextProtocol_Web>("webfrontend")
    .WithExternalHttpEndpoints()
    .WithReference(chat)
    .WaitFor(docs)
    .WaitFor(chat);

builder.AddProject<Projects.ModelContextProtocol_TestClient>("mcpclient")
    .WithReference(docs)
    .WaitFor(docs);

builder.Build().Run();
