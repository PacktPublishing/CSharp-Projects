using ConsoleAppAdventureGame.Engine;
using ConsoleAppAdventureGame.Engine.Builder;
using ConsoleAppAdventureGame.Stories;

namespace ConsoleAppAdventureGame.Tests;

public class ChoiceTests
{
    [Fact]
    public void MakingAChoiceShouldAdvanceTheStory()
    {
        // Arrange
        Adventure story = SampleAdventure.BuildAdventure();
        StoryNode startNode = story.CurrentNode!;
        Choice firstChoice = startNode.Choices.First();
        TestAdventureRenderer renderer = new();

        // Act
        firstChoice.Execute(story, renderer);

        // Assert
        story.CurrentNode.ShouldNotBe(startNode);
    }

    [Fact]
    public void ExecutingAChoiceShouldAdvanceTheStoryToTargetNode()
    {
        // Arrange
        const string destinationNodeId = "node2";
        Adventure adventure = new AdventureBuilder()
            .WithStartNode(node => { 
                node.HasText("Test");
                node.HasChoice("Go to next").ThatLeadsTo(destinationNodeId);
            })
            .WithNode(destinationNodeId, node =>
                {
                    node.HasText("The story's second node");
                })
            .Build();
        IAdventureRenderer renderer = new TestAdventureRenderer();
        StoryNode startNode = adventure.CurrentNode!;
        
        // Act
        startNode.Choices.First().Execute(adventure, renderer);

        // Assert
        adventure.CurrentNode.ShouldNotBeNull();
        adventure.CurrentNode.Id.ShouldBe(destinationNodeId);
    }
}