namespace AiPersonalAssistant.ConsoleApp.Plugins;

public static class TimeAndDatePlugin
{
    [Description("Gets a string representing the current time and date")]
    public static string GetCurrentTimeAndDate()
    {
        DateTime time = DateTime.Now;
        return $"It is currently {time.ToShortTimeString()} on {time.ToShortDateString()}";
    }
}
