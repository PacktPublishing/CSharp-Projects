var builder = DistributedApplication.CreateBuilder(args);

var docs = builder.AddProject<Projects.ModelContextProtocol_DocumentsApi>("documentsapi")
    .WithExternalHttpEndpoints();

var sse = builder.AddProject<Projects.ModelContextProtocol_SseServer>("mcpserver-sse")
    .WithExternalHttpEndpoints()
    .WithEnvironment("McpServer:DocumentsApiEndpoint", docs.GetEndpoint("http"))
    .WaitFor(docs);

builder.AddProject<Projects.ModelContextProtocol_TestClient>("mcpclient")
    .WithReference(sse)
    .WaitFor(sse);

var chat = builder.AddProject<Projects.ModelContextProtocol_ChatApi>("chatapi")
    .WithExternalHttpEndpoints()
    .WithEnvironment("Chat:mcpServerEndpoint", sse.GetEndpoint("http"))
    .WithEnvironment("OTEL_INSTRUMENTATION_GENAI_CAPTURE_MESSAGE_CONTENT", "true")
    .WaitFor(sse);

builder.AddProject<Projects.ModelContextProtocol_Web>("webfrontend")
    .WithExternalHttpEndpoints()
    .WithReference(chat)
    .WaitFor(chat);

builder.Build().Run();
