namespace ModelContextProtocol.CustomServer.Prompts;

[McpServerPromptType]
public class GenerateUnitTestsPrompt
{
    [McpServerPrompt(Name = "xunit_tests"), Description("Generates C# unit tests for xUnit")]
    public static string XUnitPrompt() => "Generate xUnit unit tests for this code using C#. Follow the Arrange / Act / Assert pattern and focus on readable unit tests.";

    [McpServerPrompt(Name = "nunit_tests"), Description("Generates C# unit tests for NUnit")]
    public static string NUnitPrompt() => "Generate NUnit unit tests for this code using C#. Follow the Arrange / Act / Assert pattern and focus on readable unit tests.";

    [McpServerPrompt(Name = "mstest_tests"), Description("Generates C# unit tests for MSTest")]
    public static string MSTestPrompt() => "Generate MSTest unit tests for this code using C#. Follow the Arrange / Act / Assert pattern and focus on readable unit tests.";
}
