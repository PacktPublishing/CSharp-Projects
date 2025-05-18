using ModelContextProtocol.Protocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelContextProtocol.CustomServer.Resources;

[McpServerResourceType]
public class FileBasedResources
{
    [McpServerResource(UriTemplate = "test://files", Name = "File Resources")]
    [Description("Files containing blog posts")]
    public static IEnumerable<ResourceContents> AllFiles(RequestContext<ReadResourceRequestParams> requestContext)
    {
        DirectoryInfo directory = new(Path.Combine(Environment.CurrentDirectory, "Data"));
        if (!directory.Exists)
            yield break;

        foreach (var file in directory.GetFiles("*.md"))
        {
            yield return new TextResourceContents()
            {
                MimeType = "text/markdown",
                Text = File.ReadAllText(file.FullName),
                Uri = $"test://files/{file.Name}"
            };
        }
    }

    [McpServerResource(UriTemplate = "test://files/{name}", Name = "Individual Files")]
    [Description("Individual blog posts")]
    public static ResourceContents FindFile(RequestContext<ReadResourceRequestParams> requestContext, string name)
    {
        FileInfo file = new(Path.Combine(Environment.CurrentDirectory, "Data", name));
        if (!file.Exists || file.Extension != ".md")
            throw new InvalidOperationException($"Could not find file {file.FullName}");

        return new TextResourceContents
        {
            MimeType = "text/markdown",
            Text = File.ReadAllText(file.FullName),
            Uri = $"test://files/{file.Name}"
        };
    }
}
