using ConsoleAppAdventureGame.Engine;

namespace ConsoleAppAdventureGame.Renderers;

public class ConsoleAdventureRenderer : IAdventureRenderer
{
    public void Render(StoryNode node)
    {
        foreach (var line in node.Text)
        {
            Console.WriteLine(line);
        }
    }

    public Choice GetChoice(StoryNode node)
    {
        Console.WriteLine("What do you want to do?");
        Console.WriteLine();

        Choice? choice = null;
        do
        {
            for (var i = 0; i < node.Choices.Length; i++)
            {
                string text = node.Choices[i].Text;
                Console.WriteLine($"{i+1}. {text}");
            }

            Console.WriteLine();
            Console.Write("Enter your choice: ");

            string? input = Console.ReadLine();

            if (int.TryParse(input, out int index) && index > 0 && index <= node.Choices.Length)
            {
                choice = node.Choices[index - 1];
            }
            else
            {
                Console.WriteLine("Invalid choice.");
            }
        } while (choice is null);
        
        Console.WriteLine($"You chose: {choice.Text}");
        return choice;
    }

    public void RenderChoiceAction(Choice choice)
    {
        foreach (var line in choice.Text)
        {
            Console.WriteLine(line);
        }
    }
}