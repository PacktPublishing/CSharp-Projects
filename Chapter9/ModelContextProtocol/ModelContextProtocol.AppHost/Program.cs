var builder = DistributedApplication.CreateBuilder(args);

// TODO: It'd be nice to launch the server explicitly here and have the client somehow talk to it

var docs = builder.AddProject<Projects.ModelContextProtocol_DocumentsApi>("documentsapi");

var mcpServer = builder.AddProject<Projects.ModelContextProtocol_StdioServer>("mcpserver-stdio")
    .WithReference(docs)
    .WaitFor(docs);

var chat = builder.AddProject<Projects.ModelContextProtocol_ChatApi>("chatapi")
    .WithReference(docs)
    .WaitFor(mcpServer)
    .WaitFor(docs);

builder.AddProject<Projects.ModelContextProtocol_Web>("webfrontend")
    .WithExternalHttpEndpoints()
    .WithReference(chat)
    .WaitFor(docs)
    .WaitFor(chat);

var sse = builder.AddProject<Projects.ModelContextProtocol_SseServer>("mcpserver-sse")
    .WithExternalHttpEndpoints()
    .WithReference(docs)
    .WaitFor(docs);

builder.AddProject<Projects.ModelContextProtocol_TestClient>("mcpclient")
    .WithReference(sse)
    .WaitFor(sse);

builder.Build().Run();
