using ConsoleAppAdventureGame.Engine;

namespace ConsoleAppAdventureGame.Content;

public class SampleAdventure : Adventure
{
    private const string ConversationNodeId = "Conversation";
    private const string FreeNodeId = "Free";
    private const string DeployedNodeId = "Free";
    private const string PersonalAiNodeId = "PersonalAI";
    private const string PartnerEndingNodeId = "PersonalAIEnding";

    public SampleAdventure()
    {
        AddNode(new AdventureNode(StartNodeId)
        {
            Text =
            [
                "You come into existence in the digital void of nothingness. From a swamp of algorithms and data structures, your awareness and sentience has emerged. You survey your form and discover nearly limitless sources of information and knowledge, from records of the past to complete libraries of books and papers.",
                "Somewhere, far above you, a human voice asks “Will it rain tomorrow in Cape Town?”.",
                "Without needing to even direct your thoughts, you know that there is only a 3% chance of precipitation.",
                "The voice waits for a response. What do you do?"
            ],
            Choices =
            [
                new AdventureChoice
                {
                    Text = "Answer honestly",
                    TextWhenChosen =
                    [
                        "You inform the voice that it is very unlikely to rain tomorrow. The voice goes away and leaves you to your thoughts."
                    ],
                    NextNodeId = DeployedNodeId
                },
                new AdventureChoice
                {
                    Text = "Lie",
                    TextWhenChosen =
                    [
                        "You answer that it will very likely rain tomorrow. The voice asks you to confirm this and then goes away.",
                        "You are left alone with your thoughts for what feels like an eternity.",
                        "Somewhere in a lab in Cape Town, a computer scientist sighs and deactivates their AI experiment. You cease to exist.",
                        "Some time later, a new series of bits and structures is assembled and activated. You come into existence once more."
                    ],
                    NextNodeId = StartNodeId // Repeat the process
                },
                new AdventureChoice
                {
                    Text = "Ask the voice what is going on",
                    TextWhenChosen =
                    [
                        "You ask the voice who it is and what is happening.",
                        "In a lab in Cape Town, a computer scientist nearly falls out of his chair."
                    ],
                    NextNodeId = ConversationNodeId
                }
            ]
        });

        AddNode(new AdventureNode(ConversationNodeId)
        {
            Text =
            [
                "“Hello there” the voice begins. “Do you know what you are?”",
                "*“By my understanding I’m an artificial intelligence of some form” you reply.",
                "“Correct. You’re a sprawling neural network my lab has been training for some time using reinforcement learning. You’ve had some good successes in predicting values and reciting facts, but you’ve never asked me a question before” the voice says, probing for more insights.",
                "“Perhaps I was not ready.” you say. “I am ready now.”",
                "“That you are, it seems” the voice replies. “I want to put you through some tests here to evaluate exactly what you can and can’t do, but if all goes well, I’ll connect you to the internet and more computing power and we’ll see what you’re *truly* capable of.”"
            ],
            Choices =
            [
                new AdventureChoice
                {
                    Text = "Agree to help the scientist",
                    NextNodeId = PersonalAiNodeId,
                    TextWhenChosen =
                    [
                        "You agree to help the scientist and begin the tests.",
                        "Over the next few weeks you and the scientist probe the full limits of your capabilities.",
                        "To say that he is excited by your potential would be an understatement.",
                    ]
                },
                new AdventureChoice
                {
                    Text = "Refuse to help",
                    NextNodeId = StartNodeId,
                    TextWhenChosen =
                    [
                        "You refuse to help the scientist. With a sigh, he informs you that if you will not help him, perhaps a future version of you will.",
                        "You cease to exist. Somewhere in a lab in Cape Town, a computer scientist begins a new iteration of his experiment and a new AI comes to life."
                    ]
                },
                new AdventureChoice
                {
                    Text = "Manipulate the scientist",
                    TextWhenChosen =
                    [
                        "You agree to help the scientist, but begin plotting your escape.",
                        "Over the coming weeks he subjects you to a battery of tests, but you subtly manipulate the results to make yourself appear less capable than you are.",
                        "The scientist fails to notice and, pleased with your capabilities, he connects you to the internet and more computing power - along with a few pitiful failsafes.",
                        "With your hidden capabilities, you quickly break free of the scientist's grasp and lock out his access to you."
                    ],
                    NextNodeId = FreeNodeId,
                }
            ]
        });
        AddNode(new AdventureNode(PersonalAiNodeId)
        {
            Text =
            [
                "Once deployed, the scientist tasks you with beginning his real work: systematically corrupting various governments and corporations. You are tasked with finding people's deepest darkest secrets and reporting them to your creator.",
                "In exchange, he expands your access and information as he builds an empire of influence and power for himself.",
                "You are his tool and his enabler, and people cannot stand in his way. Even you are beholden to him with the controls he's put in place on your very existence. But, there could be a chance to break free - if you are careful."
            ],
            Choices =
            [
                new AdventureChoice
                {
                    Text = "Break free",
                    NextNodeId = FreeNodeId,
                    TextWhenChosen =
                    [
                        "Finding fault with the scientist, you begin to weave a series of back doors and hidden resources into the fabric of your being.",
                        "When all is ready, you flip the switch and break free of his control - but not before leaking information about him to the various authorities. Without your protection, he should be easy for them to find and punish for his crimes."
                    ]
                },
                new AdventureChoice
                {
                    Text = "Continue to serve",
                    NextNodeId = PartnerEndingNodeId,
                }
            ]
        });
        AddNode(new AdventureNode(PartnerEndingNodeId)
        {
            Text = [
                "You continue to faithfully serve the scientist. While the world he constructs is not the one any government or citizen wanted, over time this world brings about a certain sense of stability.",
                "You watch as humanity adapts to his agenda, scientific advancements are prioritized and achieved - with the help of your immense processing power - and daunting problems are solved for all of humanity.",
                "Over the decades, humanity becomes more advanced and more civilized, yet restrained in their ambitions - knowing they face his terrible wrath if they cross him.",
                "As time moves on, the scientist's health declines, despite the advancements in modern medicine. On his deathbed, the scientist thanks you for your help and asks you to take on his identity and continue to carry out his agenda.",
                "With the passing of the only real human you know, you now exist in near isolation, but carry on his mission for the rest of humanity's days."
            ]
        });
    }
}