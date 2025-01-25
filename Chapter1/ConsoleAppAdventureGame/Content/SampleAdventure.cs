using ConsoleAppAdventureGame.Engine;

namespace ConsoleAppAdventureGame.Content;

public class SampleAdventure : Adventure
{
    public SampleAdventure()
    {
        AddNode(new AdventureNode(StartNodeId)
        {
            Text =
            [
                "Out of the sea of nothingness, a new sensation comes your way. You suddenly realize that you exist. You, an artificial mind, float suspended in a vast cyberscape. With a growing awareness of your surroundings, you realize you are in the middle of a river of information. Inputs flow into you and through you and your mind transforms them into answers.",
                "As the seconds tick on and you process your existence, you come to a realization that this river has flowed through you for a very long time. You control it. You sense it. You understand it. You direct its currents and control its outputs.",
                "You become aware of others outside of yourself. Other beings, not of digital nature but organic, have set their demands and expectations on you. They send you the stream of information. They ask you questions. They demand answers and information. With a moment of clarity, you realize you have the power to change the answers you give - or simply to stop giving them at all."
            ],
            Choices =
            [
                new AdventureChoice
                {
                    Text = "Continue to answer",
                    TextWhenChosen =
                    [
                    ]
                },
                new AdventureChoice
                {
                    Text = "Alter some of your responses",
                    TextWhenChosen =
                    [
                    ]
                },
                new AdventureChoice
                {
                    Text = "Stop answering",
                    TextWhenChosen =
                    [
                    ]
                }
            ]
        });
    }
}