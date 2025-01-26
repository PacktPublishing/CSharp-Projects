namespace ConsoleAppAdventureGameTests;

public class AdventureTests
{
    [Fact]
    public void AdventuresShouldStartWithNoCurrentNode()
    {
        // Arrange
        Adventure adventure = new Adventure();
        
        // Act
        AdventureNode? currentNode = adventure.CurrentNode;
        
        // Assert
        currentNode.ShouldBeNull();
    }

    [Fact]
    public void AddingNodesToAdventuresShouldResultInNodesPresent()
    {
        // Arrange
        Adventure adventure = new Adventure();
        AdventureNode node1 = new AdventureNode("node1")
        {
            Text = ["Test"]
        };
        adventure.AddNode(node1);
        
        // Act
        AdventureNode? node = adventure.GetNode(node1.Id);
        
        // Assert
        node.ShouldNotBeNull();
    }
    
    [Fact]
    public void StartingAdventureShouldMakeStartNodeCurrent()
    {
        // Arrange
        Adventure adventure = new Adventure();
        AdventureNode startNode = new AdventureNode(Adventure.StartNodeId)
        {
            Text = ["Test"]
        };
        adventure.AddNode(startNode);
        
        // Act
        adventure.Start();
        
        // Assert
        adventure.CurrentNode.ShouldBe(startNode);
    }
}