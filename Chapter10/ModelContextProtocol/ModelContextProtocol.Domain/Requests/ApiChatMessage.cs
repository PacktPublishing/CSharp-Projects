using ModelContextProtocol.Protocol;

namespace ModelContextProtocol.Domain.Requests;

public record ApiChatMessage(Role Role, string Message);