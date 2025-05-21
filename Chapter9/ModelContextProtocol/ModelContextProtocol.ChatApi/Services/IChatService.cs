using ModelContextProtocol.ChatApi.Requests;
using ModelContextProtocol.Domain.Requests;

namespace ModelContextProtocol.ChatApi.Services;

public interface IChatService
{
    IAsyncEnumerable<string> ChatAsync(ChatRequest request);
}