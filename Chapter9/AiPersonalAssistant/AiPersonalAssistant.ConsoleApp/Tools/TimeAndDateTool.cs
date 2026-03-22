namespace AiPersonalAssistant.ConsoleApp.Tools;

public static class TimeAndDateTool
{
    [Description("Gets a string representing the current time and date")]
    public static string GetCurrentTimeAndDate()
    {
        DateTime time = DateTime.Now;
        return $"It is currently {time:t} on {time:d}";
    }
}
