using ConsoleAppAdventureGame.Engine;
using ConsoleAppAdventureGame.Engine.Builder;

namespace ConsoleAppAdventureGame.Stories;

public static class SimplePrototype
{
    public static void Run()
    {
        Console.WriteLine("Your time machine is ready to go.");
        Console.WriteLine("Do you dare turn it on?");
        Console.WriteLine("1. Turn it on");
        Console.WriteLine("2. Destroy it!");
        Console.WriteLine("Choose an option: ");

        string choice = Console.ReadLine();

        if (choice == "1")
        {
            Console.WriteLine("You are now adrift in space without a spacesuit.");
            Console.WriteLine("It seems you failed to account for the Earth being at different points in its orbit over time.");
        }
        else if (choice == "2")
        {
            Console.WriteLine("You smash it to pieces!");
            Console.WriteLine("The device collapses, compressing all of time and space along with it.");
        }
        else
        {
            Console.WriteLine("Invalid choice.");
        }
    }
}