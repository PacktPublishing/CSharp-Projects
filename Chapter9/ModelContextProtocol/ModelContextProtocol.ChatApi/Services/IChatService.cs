using ModelContextProtocol.ChatApi.Requests;

namespace ModelContextProtocol.ChatApi.Services;

public interface IChatService
{
    IAsyncEnumerable<string> ChatAsync(ChatRequest request);
}