using System.Text.Json;
using ModelContextProtocol.Domain.Requests;

namespace ModelContextProtocol.Web;

public class ChatApiClient(HttpClient httpClient)
{
    public async Task<IEnumerable<ApiChatMessage>> ChatAsync(IEnumerable<ApiChatMessage> messages, CancellationToken cancellationToken = default)
    {
        ChatRequest request = new()
        {
            Messages = messages
        };
        
        JsonContent requestBody = JsonContent.Create(request);
        HttpResponseMessage response = await httpClient.PostAsync("/chat", requestBody, cancellationToken);
        response.EnsureSuccessStatusCode();

        // Deserialize the response content to a list of ApiChatMessage
        string responseContent = await response.Content.ReadAsStringAsync(cancellationToken);
        return JsonSerializer.Deserialize<IEnumerable<ApiChatMessage>>(responseContent)!;
    }
}
