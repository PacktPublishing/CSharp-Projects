using ConsoleAppAdventureGame.Engine;
using ConsoleAppAdventureGame.Engine.Builder;

namespace ConsoleAppAdventureGame.Stories;

public static class SampleAdventure
{
    public static Adventure BuildAdventure()
    {
        StoryNode stranded = new("Stranded")
        {
            Text = ["It seems you failed to account for the [yellow italic]Earth being at different points in its orbit[/] over time."]
        };
        StoryNode destroy = new("Destroy")
        {
            Text = ["The device collapses, [cyan underline]compressing all of time and space[/] along with it."]
        };

        StoryNode start = new("Start")
        {
            Text = [
                "Your time machine is ready to go.",
        "Do you dare turn it on?"
            ],
            Choices = [
                new Choice("Turn it on")
        {
            WhenChosen = ["You are now [red bold]adrift in space without a spacesuit[/]."],
            NextNodeId = stranded.Id
        },
        new Choice("Destroy it!")
        {
            WhenChosen = ["You [bold yellow]smash it[/] to pieces!"],
            NextNodeId = destroy.Id
        }
            ]
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
                    .WithText("You are now [red]adrift in space[/] without a spacesuit.")
                    .ThatLeadsTo("Stranded");
                n.HasChoice("Destroy it!")
                    .WithText("You [bold yellow]smash[/] it to pieces!")
                    .ThatLeadsTo("Destroy");
            })
            .WithNode("Stranded", n => n.HasText("It seems you failed to account for the [bold cyan]Earth being at different points in its orbit[/] over time."))
            .WithNode("Destroy", n => n.HasText("The device collapses, [italic red]compressing all of time and space[/] along with it."))
            .Build();

        return adventure;
    }
}