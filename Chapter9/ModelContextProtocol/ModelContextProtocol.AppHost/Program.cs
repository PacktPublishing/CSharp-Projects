var builder = DistributedApplication.CreateBuilder(args);

var docs = builder.AddProject<Projects.ModelContextProtocol_DocumentsApi>("documentsapi")
    .WithExternalHttpEndpoints();

var sse = builder.AddProject<Projects.ModelContextProtocol_SseServer>("mcpserver-sse")
    .WithExternalHttpEndpoints()
    .WithEnvironment("McpServer:KernelMemoryEndpoint", docs.GetEndpoint("https"))
    .WaitFor(docs);

builder.AddProject<Projects.ModelContextProtocol_TestClient>("mcpclient")
    .WithReference(sse)
    .WaitFor(sse);

var chat = builder.AddProject<Projects.ModelContextProtocol_ChatApi>("chatapi")
    .WithExternalHttpEndpoints()
    .WithEnvironment("Chat:mcpServerEndpoint", sse.GetEndpoint("https"))
    .WaitFor(sse);

builder.AddProject<Projects.ModelContextProtocol_Web>("webfrontend")
    .WithExternalHttpEndpoints()
    .WithReference(chat)
    .WaitFor(docs)
    .WaitFor(chat);

builder.Build().Run();
