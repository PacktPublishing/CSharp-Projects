using Microsoft.UI.Dispatching;
using Uno.UI.Extensions;

namespace AiStoryteller.Presentation;

public sealed partial class MainPage : Page
{
    public MainPage()
    {
        this.InitializeComponent();
    }

    public ListView? MessageList { get; set; }
    public ScrollViewer? MessageScrollViewer { get; set; }

    private void ListViewLoaded(object sender, RoutedEventArgs e)
    {
        MessageList = sender as ListView;

        if (MessageList is null)
            return;

        MessageScrollViewer = MessageList.FindFirstDescendant<ScrollViewer>();

        ScrollToBottomHandler = () => MessageScrollViewer?.DispatcherQueue.TryEnqueue(DispatcherQueuePriority.Low,
            () => MessageScrollViewer?.ScrollToVerticalOffset(MessageScrollViewer.ScrollableHeight));
    }

    public static Action? ScrollToBottomHandler { get; private set; }
}
