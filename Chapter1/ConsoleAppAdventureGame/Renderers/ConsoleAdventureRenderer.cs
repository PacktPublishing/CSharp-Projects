using ConsoleAppAdventureGame.Engine;

namespace ConsoleAppAdventureGame.Renderers;

public class ConsoleAdventureRenderer : IAdventureRenderer
{
    public void Render(StoryNode node)
    {
        foreach (var line in node.Text)
        {
            Console.WriteLine(line);
            Console.WriteLine();
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
                Console.WriteLine($"{i + 1}. {node.Choices[i].Text}");
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
                Console.WriteLine("Invalid choice. Please try again.");
            }
        } while (choice is null);
        
        Console.WriteLine($"You chose: {choice.Text}");
        return choice;
    }

    public void RenderChoiceAction(Choice choice)
    {
        foreach (var line in choice.TextWhenChosen)
        {
            Console.WriteLine(line);
            Console.WriteLine();
        }
    }
}