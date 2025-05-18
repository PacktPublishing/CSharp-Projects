using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelContextProtocol.CustomServer.Tools;

[McpServerToolType]
public class RandomTools
{
    private readonly Random _random = new();

    [McpServerTool(Name = "CoinFlip"), Description("Flips a coin and gets a heads or tails value")]
    public string CoinFlip() => 
        _random.Next(2) == 0
            ? "Heads"
            : "Tails";

    [McpServerTool(Name = "RockPaperScissors"), Description("Gets Rock, Paper, or Scissors")]
    public string RockPaperScissors() =>
        _random.Next(3) switch
        {
            0 => "rock",
            1 => "Paper",
            _ => "Scissors"
        };

    [McpServerTool(Name = "DiceRoll"), Description("Rolls a set of N-Sided dice and returns the total")]
    public string DiceRoll(int numSides, int numDice)
    {
        numDice = Math.Max(1, numDice);

        int[] rolls = new int[numDice];

        for (int i = 0; i < numDice; i++)
        {
            rolls[i] = _random.Next(numSides) + 1;
        }

        int total = rolls.Sum();
        string rollString = string.Join(',', rolls);
        return $"Rolled {numDice} {numSides}-sided dice resulting in a total of {total} on rolls of {rollString}";
    }
}
