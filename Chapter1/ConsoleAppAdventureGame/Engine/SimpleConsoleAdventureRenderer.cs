namespace ConsoleAppAdventureGame.Engine;

public class SimpleConsoleAdventureRenderer : IAdventureRenderer
{
    public void Render(AdventureNode currentNode)
    {
        foreach (var line in currentNode.Text)
        {
            Console.WriteLine(line);
            Console.WriteLine();
        }
    }

    public AdventureChoice GetChoice(AdventureNode currentNode)
    {
        Console.WriteLine("What do you want to do?");
        Console.WriteLine();

        AdventureChoice? choice = null;
        while (choice is null)
        {
            for (var i = 0; i < currentNode.Choices.Length; i++)
            {
                Console.WriteLine($"{i + 1}. {currentNode.Choices[i].Text}");
            }

            Console.WriteLine();
            Console.Write("Enter your choice: ");
            
            string? input = Console.ReadLine();
            
            if (int.TryParse(input, out var index) && index > 0 && index <= currentNode.Choices.Length)
            {
                choice = currentNode.Choices[index - 1];
            }
            else
            {
                Console.WriteLine("Invalid choice. Please try again.");
            }
        }

        
        Console.WriteLine($"You chose: {choice.Text}");
        
        return choice;
    }

    public void RenderChoiceAction(AdventureChoice choice)
    {
        foreach (var line in choice.TextWhenChosen)
        {
            Console.WriteLine(line);
            Console.WriteLine();
        }
    }
}