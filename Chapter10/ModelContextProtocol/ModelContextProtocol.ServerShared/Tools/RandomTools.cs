using System.ComponentModel;
using System.Diagnostics;
using ModelContextProtocol.Server;

namespace ModelContextProtocol.ServerShared.Tools;

[McpServerToolType]
public class RandomTools
{
    private readonly ActivitySource _activitySource = new(typeof(RandomTools).Assembly.FullName!);
    private readonly Random _random = new();

    [McpServerTool(Name = "CoinFlip"), Description("Flips a coin and gets a heads or tails value")]
    public string CoinFlip()
    {
        using Activity? activity = _activitySource.StartActivity(ActivityKind.Server);

        string result = _random.Next(2) == 0
            ? "Heads"
            : "Tails";
        
        activity?.AddEvent(new ActivityEvent(result));
        return result;
    }

    [McpServerTool(Name = "RockPaperScissors"), Description("Gets Rock, Paper, or Scissors")]
    public string RockPaperScissors()
    {
        using Activity? activity = _activitySource.StartActivity(ActivityKind.Server);
        string choice = _random.Next(3) switch
        {
            0 => "rock",
            1 => "Paper",
            _ => "Scissors"
        };
        
        activity?.AddEvent(new ActivityEvent($"Chose {choice}"));
        return choice;
    }

    [McpServerTool(Name = "DiceRoll"), Description("Rolls a set of N-Sided dice and returns the total")]
    public string DiceRoll(int numSides, int numDice)
    {
        using Activity? activity = _activitySource.StartActivity(ActivityKind.Server);
        
        numDice = Math.Max(1, numDice);

        int[] rolls = new int[numDice];

        for (int i = 0; i < numDice; i++)
        {
            rolls[i] = _random.Next(numSides) + 1;
            activity?.AddEvent(new ActivityEvent($"Rolled a {rolls[i]}"));
        }

        int total = rolls.Sum();
        string rollString = string.Join(',', rolls);
        return $"Rolled {numDice} {numSides}-sided dice resulting in a total of {total} on rolls of {rollString}";
    }
}
