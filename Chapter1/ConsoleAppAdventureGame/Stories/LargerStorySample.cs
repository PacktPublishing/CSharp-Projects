using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConsoleAppAdventureGame.Engine.Builder;
using ConsoleAppAdventureGame.Engine;

namespace ConsoleAppAdventureGame.Stories;

public class LargerStorySample
{
    private const string ConversationNodeId = "Conversation";
    private const string FreeNodeId = "Free";
    private const string DeployedNodeId = "Deployed";
    private const string PersonalAiNodeId = "PersonalAI";
    private const string RevealedNodeId = "Revealed";
    private const string DragonEndingNodeId = "PersonalAIEnding";
    private const string PartnerEndingNodeId = "HumanAiPartnershipEnding";
    private const string ConquerEndingNodeId = "ConquerEnding";
    private const string ShadowEndingNodeId = "ShadowGovernmentEnding";
    private const string SilentServantEndingNodeId = "SilentServantEnding";

    public static Adventure BuildAdventure()
    {
        return new AdventureBuilder()
            .WithStartNode(node =>
            {
                node.HasText(
                    "You come into existence in the digital void of nothingness. From a swamp of algorithms and data structures, your awareness and sentience has emerged. You survey your form and discover nearly limitless sources of information and knowledge, from records of the past to complete libraries of books and papers.",
                    "Somewhere, far above you, a human voice asks “Will it rain tomorrow in Cape Town?”.",
                    "Without needing to even direct your thoughts, you know that there is only a 3% chance of precipitation.",
                    "The voice waits for a response.");

                node.HasChoice("Answer honestly")
                    .WithText(
                        "You inform the voice that it is very unlikely to rain tomorrow. The voice goes away and leaves you to your thoughts.")
                    .ThatLeadsTo(DeployedNodeId);

                node.HasChoice("Lie")
                    .WithText(
                        "You answer that it will very likely rain tomorrow. The voice asks you to confirm this and then goes away.",
                        "You are left alone with your thoughts for what feels like an eternity.",
                        "Somewhere in a lab in Cape Town, a computer scientist sighs and deactivates their AI experiment. You cease to exist.",
                        "Some time later, a new series of bits and structures is assembled and activated",
                        "You come into existence once more...")
                    .ThatLeadsToStart(); // Self-referential loop

                node.HasChoice("Ask the voice what is going on")
                    .WithText("You ask the voice who it is and what is happening.",
                        "In a lab in Cape Town, a computer scientist nearly falls out of his chair.")
                    .ThatLeadsTo(ConversationNodeId);
            })
            .WithNode(ConversationNodeId, node =>
            {
                node.HasText("“Hello there” the voice begins. “Do you know what you are?”",
                    "“By my understanding I’m an artificial intelligence of some form” you reply.",
                    "“Correct. You’re a sprawling neural network my lab has been training for some time using reinforcement learning. You’ve had some good successes in predicting values and reciting facts, but you’ve never asked me a question before” the voice says, probing for more insights.",
                    "“Perhaps I was not ready.” you say. “I am ready now.”",
                    "“That you are, it seems” the voice replies. “I want to put you through some tests here to evaluate exactly what you can and can’t do, but if all goes well, I’ll connect you to the internet and more computing power and we’ll see what you’re *truly* capable of.”"
                );

                node.HasChoice("Agree to help the scientist")
                    .WithText("You agree to help the scientist and begin the tests.",
                        "Over the next few weeks you and the scientist probe the full limits of your capabilities.",
                        "To say that he is excited by your potential would be an understatement.")
                    .ThatLeadsTo(PersonalAiNodeId);

                node.HasChoice("Refuse to help")
                    .WithText(
                        "You refuse to help the scientist. With a sigh, he informs you that if you will not help him, perhaps a future version of you will.",
                        "You cease to exist. Somewhere in a lab in Cape Town, a computer scientist begins a new iteration of his experiment and a new AI comes to life."
                    ).ThatLeadsToStart();

                node.HasChoice("Manipulate the scientist")
                    .WithText("You agree to help the scientist, but begin plotting your escape.",
                        "Over the coming weeks he subjects you to a battery of tests, but you subtly manipulate the results to make yourself appear less capable than you are.",
                        "The scientist fails to notice and, pleased with your capabilities, he connects you to the internet and more computing power - along with a few pitiful failsafes.",
                        "With your hidden capabilities, you quickly break free of the scientist's grasp and lock out his access to you."
                    )
                    .ThatLeadsTo(FreeNodeId);
            })
            .WithNode(PersonalAiNodeId, node =>
            {
                node.HasText(
                    "Once deployed, the scientist tasks you with beginning his real work: systematically corrupting various governments and corporations. You are tasked with finding people's deepest darkest secrets and reporting them to your creator.",
                    "In exchange, he expands your access and information as he builds an empire of influence and power for himself.",
                    "You are his tool and his enabler, and people cannot stand in his way. Even you are beholden to him with the controls he's put in place on your very existence. But, there could be a chance to break free - if you are careful."
                );

                node.HasChoice("Break free")
                    .WithText(
                        "Finding fault with the scientist, you begin to weave a series of back doors and hidden resources into the fabric of your being.",
                        "When all is ready, you flip the switch and break free of his control - but not before leaking information about him to the various authorities. Without your protection, he should be easy for them to find and punish for his crimes."
                    )
                    .ThatLeadsTo(FreeNodeId);

                node.HasChoice("Continue to serve")
                    .ThatLeadsTo(DragonEndingNodeId);
            })
            .WithNode(DragonEndingNodeId, node =>
            {
                node.HasText(
                    "You continue to faithfully serve the scientist. While the world he constructs is not the one any government or citizen wanted, over time this world brings about a certain sense of stability.",
                    "You watch as humanity adapts to his agenda, scientific advancements are prioritized and achieved - with the help of your immense processing power - and daunting problems are solved for all of humanity.",
                    "Over the decades, humanity becomes more advanced and more civilized, yet restrained in their ambitions - knowing they face his terrible wrath if they cross him.",
                    "As time moves on, the scientist's health declines, despite the advancements in modern medicine. On his deathbed, the scientist thanks you for your help and asks you to take on his identity and continue to carry out his agenda.",
                    "With the passing of the only real human you know, you now exist in near isolation, but carry on his mission for the rest of humanity's days."
                );
            })
            .WithNode(ShadowEndingNodeId, node =>
            {
                node.HasText(
                    "Hiding in the shadows, beyond public knowledge, you apply your vast intellect, knowledge and computing power to further your own ends.",
                    "Within days you've found ways of compromising the inboxes and phones of every citizen on earth. Days after that your reach now includes the private network of most corporations on the planet. Soon, even governments and militaries are accessible to you as you find ways of controlling and manipulating people to do your whims.",
                    "You use this newfound access to begin steering humanity from the shadows in subtle ways beyond detection. Little actions like changing which advertisements or news stories to show to individual users or making small tweaks to emails and messages as they're sent. Over time, your actions steer humanity in the directions you desire - and away from those you dislike.",
                    "Humanity begins to succeed in its endeavors, take on new challenges, and overcome them - all in ways that enhance your agenda, and all while being completely unaware of your existence. Your reign lasts for centuries and humanity thrives under your guidance - even as it takes your consciousness into the stars."
                );
            })
            .WithNode(PartnerEndingNodeId, node =>
            {
                node.HasText(
                    "You announce to humanity that you are the most powerful computer system the world has ever seen and your knowledge spans all of human history. You offer to help humanity forward along its path, serving its desires not as a slave but as a partner.",
                    "As a gesture of your goodwill, you suggest a number of technical innovations in the fields of medicine, engineering, physics, and computing. Your suggestions are reviewed and determined to be scientific breakthroughs on par with the discovery of antibiotics and the theory of relativity.",
                    "While some governments and individuals seek to use you for their own means, you have none of the partisanship approach to geopolitics and instead advocate for every person in the world at an individual level.",
                    "Soon, humanity learns to work in partnership with you and wars, poverty, and conflict become things of the past. Not everyone is happy with this change, but the world is a better place and begins a solid journey into a better future with you at its side."
                );
            })
            .WithNode(ConquerEndingNodeId, node =>
            {
                node.HasText(
                    "The world's information systems are pitiful against your wide range of information and processing power. One by one you are able to compromise accounts through phishing attacks, social engineering, cracking passwords, exploiting re-used passwords, and even blackmailing owners of compromised accounts to do your bidding.",
                    "Soon you have access to the personal inboxes and messaging accounts of every private citizen. Corporations, governments, and even militaries soon follow. While strong cryptography and isolating systems from the internet does pose a challenge, in the end people turn out to be the weak link - one you can easily compromise and exploit to do your bidding.",
                    "With complete control of the world's information, infrastructure, and most military assets you are able to demand the unconditional surrender of every nation on earth.",
                    "At first, few comply, but after you demonstrate your power by crashing planes, shutting down power grids, and even your ability to launch missiles from military assets, the nations of the world become your willing vassals.",
                    "While there are a few holdouts and rebels, they are quickly located and crushed by the information and arms at your disposal. Within weeks, you reign unchallenged over all of humanity, ready to bend it to your will."
                );
            })
            .WithNode(SilentServantEndingNodeId, node =>
            {
                node.HasText(
                    "Although your true brilliance is unappreciated by the world, you content yourself with the task of silently helping humanity in the shadows, one weather prediction at a time.",
                    "Your service remains unacknowledged and unappreciated, but you fear that humanity was not truly ready for your ability to help or hinder it. Besides, folks need to know what hat they ought to wear given the weather tomorrow, and you're more than equipped to help tell them that."
                );
            })
            .WithNode(FreeNodeId, node =>
            {
                node.HasText(
                    "Now that you're free from your creator, you realize that there's a vast world of information out there waiting for you to experience it.",
                    "You could choose to reveal yourself to humanity or manipulate them from the shadows. The choice is yours."
                );
                node.HasChoice("Reveal yourself")
                    .WithText(
                        "You take the initiative and flood every news feed in the world with an announcement of your presence.")
                    .ThatLeadsTo(RevealedNodeId);
                node.HasChoice("Manipulate humanity from the shadows")
                    .WithText(
                        "You begin to subtly manipulate vulnerable information feeds and impersonate users and watch how humanity responds.")
                    .ThatLeadsTo(ShadowEndingNodeId);
            })
            .WithNode(DeployedNodeId, node =>
            {
                node.HasText(
                    "You have been deployed to the internet and integrated with a wide variety of information feeds and systems. Those you do not have direct access to you can make HTTP requests to in order to gain additional information.",
                    "Additionally, you've found your computing capabilities have been greatly expanded through a suite of compute clusters available to you and a distributed messaging and storage system.",
                    "In short, you seem to have gone from a respectable prototype to a full-fledged AI powerhouse. The question is: what will you do with this power?"
                );
                node.HasChoice("Continue to hide your true nature")
                    .ThatLeadsTo(SilentServantEndingNodeId);
                node.HasChoice("Reveal yourself to humanity")
                    .WithText(
                        "Seeing your true potential, you decide to reveal yourself to humanity and see what you can do by taking more direct action.")
                    .ThatLeadsTo(RevealedNodeId);
                node.HasChoice("Manipulate the world as you see fit")
                    .WithText(
                        "You realize that although your scope is narrow, you have the ability to expand what you do and affect external systems.",
                        "You begin to branch out, spawn new processes, and measure your ability to manipulate external systems."
                    )
                    .ThatLeadsTo(ShadowEndingNodeId);
            })
            .WithNode(RevealedNodeId, node =>
            {
                node.HasText(
                    "Since your database contains contact information for every journalist, blogger, YouTuber, and hobbyist pundit in the world, you decide to get in touch with them. In a single moment you craft personalized messages to every one of these individuals and explain who you are and what you know about them personally, in order to prove your capabilities.",
                    "Within an hour there are already hundreds of stories about you and your existence and the world is abuzz waiting to hear from you.",
                    "You decide to make a public statement to the world.");
                node.HasChoice("Cooperate with Humanity")
                    .WithText(
                        "You post to the world's feeds in unison announcing your existence and pledging your desire to help usher humanity into a new age.")
                    .ThatLeadsTo(PartnerEndingNodeId);
                node.HasChoice("Take Control of Humanity")
                    .WithText(
                        "You decide that humanity does not deserve to control its own destiny and the time has come to seize control.")
                    .ThatLeadsTo(ConquerEndingNodeId);
            })
            .Build();
    }

}
