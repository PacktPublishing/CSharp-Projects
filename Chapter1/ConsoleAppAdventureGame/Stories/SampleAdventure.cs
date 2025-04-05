using ConsoleAppAdventureGame.Engine;
using ConsoleAppAdventureGame.Engine.Builder;

namespace ConsoleAppAdventureGame.Stories;

public static class SampleAdventure
{
    public static Adventure BuildMinimalAdventure()
    {
        StoryNode start = new("Start")
        {
            Text = [
                "Your time machine is ready to go.",
                "Do you dare turn it on?"
            ],
            Choices = [
                new Choice("Turn it on")
                {
                    WhenChosen = ["You are now adrift in space without a spacesuit."],
                    NextNodeId = "Stranded"
                },
                new Choice("Destroy it!")
                {
                    WhenChosen = ["You smash it to pieces!"],
                    NextNodeId = "Destroy"
                }
            ]
        };
        StoryNode stranded = new("Stranded")
        {
            Text = ["It seems you failed to account for the Earth being at different points in its orbit over time."]
        };
        StoryNode destroy = new("Destroy")
        {
            Text = ["The device collapses, compressing all of time and space along with it."]
        };

        Adventure adventure = new([start, stranded, destroy]);

        return adventure;
    }

    public static Adventure BuildMinimalAdventureUsingBuilder()
    {
        Adventure adventure = new AdventureBuilder()
            .WithStartNode(n =>
            {
                n.HasText("Your time machine is ready to go.",
                    "Do you dare turn it on?");
                n.HasChoice("Turn it on")
                    .WithText("You are now **adrift in space** without a spacesuit.")
                    .ThatLeadsTo("Stranded");
                n.HasChoice("Destroy it!")
                    .WithText("You **smash** it to pieces!")
                    .ThatLeadsTo("Destroy");
            })
            .WithNode("Stranded", n => n.HasText("It seems you failed to account for the *Earth being at different points in its orbit* over time."))
            .WithNode("Destroy", n => n.HasText("The device collapses, *compressing all of time and space* along with it."))
            .Build();

        return adventure;
    }
}