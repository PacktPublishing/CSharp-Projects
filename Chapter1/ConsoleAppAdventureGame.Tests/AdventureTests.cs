namespace ConsoleAppAdventureGame.Tests;

public class AdventureTests
{
    [Fact]
    public void StartNodeShouldBeCurrentNodeOnStart()
    {
        // Arrange
        AdventureBuilder builder = new AdventureBuilder()
            .WithStartNode(node => node.HasText("Test"));

        // Act
        Adventure adventure = builder.Build();
        
        // Assert
        adventure.CurrentNode.ShouldNotBeNull();
        adventure.CurrentNode.Text.Count().ShouldBe(1);
        adventure.CurrentNode.Text[0].ShouldBe("Test");
    }
}