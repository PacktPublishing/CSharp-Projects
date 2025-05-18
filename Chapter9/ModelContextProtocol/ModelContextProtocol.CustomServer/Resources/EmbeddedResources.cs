namespace ModelContextProtocol.CustomServer.Resources;

[McpServerResourceType]
public class EmbeddedResources
{
    [McpServerResource(UriTemplate = "test://about/author", Name = "MCP Server Author", MimeType = "text/plain")]
    [Description("The author of this MCP Server")]
    public static string AuthorTextResource() => "This MCP Server project was created by Matt Eland";

    [McpServerResource(UriTemplate = "test://about/context", Name = "MCP Server Context", MimeType = "text/plain")]
    [Description("More information about this MCP Server")]
    public static string ContextTextResource() => "This MCP Server was created as part of a book on C# projects for publication with Packt Publishing";
}
