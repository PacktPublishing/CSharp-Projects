var builder = DistributedApplication.CreateBuilder(args);

// TODO: It'd be nice to launch the server explicitly here and have the client somehow talk to it

var docs = builder.AddProject<Projects.ModelContextProtocol_DocumentsApi>("documentsapi")
    .WithExternalHttpEndpoints();

/*
var mcpServer = builder.AddProject<Projects.ModelContextProtocol_StdioServer>("mcpserver-stdio")
    .WithReference(docs)
    .WaitFor(docs);
*/

var sse = builder.AddProject<Projects.ModelContextProtocol_SseServer>("mcpserver-sse")
    .WithExternalHttpEndpoints()
    .WithReference(docs)
    .WaitFor(docs);

builder.AddProject<Projects.ModelContextProtocol_TestClient>("mcpclient")
    .WithReference(sse)
    .WaitFor(sse);

var chat = builder.AddProject<Projects.ModelContextProtocol_ChatApi>("chatapi")
    .WithExternalHttpEndpoints()
    .WithReference(docs)
    .WaitFor(sse)
    .WaitFor(docs);

builder.AddProject<Projects.ModelContextProtocol_Web>("webfrontend")
    .WithExternalHttpEndpoints()
    .WithReference(chat)
    .WaitFor(docs)
    .WaitFor(chat);

builder.Build().Run();
