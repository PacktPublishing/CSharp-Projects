namespace ConsoleAppAdventureGameTests;

public class ChoiceTests
{
    [Fact]
    public void ExecutingAChoiceShouldAdvanceTheStoryToTargetNode()
    {
        // Arrange
        Adventure adventure = new Adventure();
        AdventureNode destinationNode = new AdventureNode("node2")
        {
            Text = ["The story's second node"]
        };
        AdventureChoice choice = new AdventureChoice("Go to the next node")
        {
            TextWhenChosen = ["You went to the next node"],
            NextNodeId = destinationNode.Id
        };
        AdventureNode startNode = new AdventureNode(Adventure.StartNodeId)
        {
            Text = ["The story's start"],
            Choices = [choice]
        };
        adventure.AddNode(startNode);
        adventure.AddNode(destinationNode);
        adventure.Start();
        IAdventureRenderer renderer = new TestAdventureRenderer();
        
        // Act
        adventure.ExecuteChoice(startNode.Choices.First(), renderer);
        
        // Assert
        adventure.CurrentNode.ShouldBe(destinationNode);
    }
}