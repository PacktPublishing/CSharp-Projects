using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.AI;

namespace Chatbot.Templating;
public class ChatTemplateSelector : DataTemplateSelector
{
    public DataTemplate UserTemplate { get; set; }
    public DataTemplate AssistantTemplate { get; set; }

    protected override DataTemplate SelectTemplateCore(object item)
    {
        if (item is not ChatMessage message)
        {
            return base.SelectTemplateCore(item);
        }

        return message.Role == ChatRole.User ? UserTemplate : AssistantTemplate;
    }
}
