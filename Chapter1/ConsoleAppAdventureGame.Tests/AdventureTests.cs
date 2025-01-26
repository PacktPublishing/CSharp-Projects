namespace ConsoleAppAdventureGame.Tests;

public class AdventureTests
{
    [Fact]
    public void StartingAdventureShouldMakeStartNodeCurrent()
    {
        // Arrange / Act
        Adventure adventure = new AdventureBuilder().WithStartNode(node =>
        {
            node.HasText("Test");
        }).Build();
        
        // Assert
        adventure.CurrentNode.ShouldNotBeNull();
        adventure.CurrentNode.Text[0].ShouldBe("Test");
    }
}