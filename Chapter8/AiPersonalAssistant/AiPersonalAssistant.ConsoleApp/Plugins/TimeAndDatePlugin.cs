using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AiPersonalAssistant.ConsoleApp.Plugins;

public class TimeAndDatePlugin
{
    [KernelFunction("TimeAndDate")]
    [Description("Gets a string representing the current time and date")]
    public string GetCurrentTimeAndDate()
    {
        DateTime time = DateTime.Now;
        return $"It is currently {time.ToShortTimeString()} on {time.ToShortDateString()}";
    }
}
