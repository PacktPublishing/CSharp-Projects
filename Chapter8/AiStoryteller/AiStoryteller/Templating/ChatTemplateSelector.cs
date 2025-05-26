using Microsoft.Extensions.AI;

namespace AiStoryteller.Templating;
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
