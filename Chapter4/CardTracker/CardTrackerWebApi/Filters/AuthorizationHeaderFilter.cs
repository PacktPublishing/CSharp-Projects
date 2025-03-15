namespace CardTrackerWebApi.Filters;

public class AuthorizationHeaderFilter(string? apiKey) : IEndpointFilter
{
    private readonly string _apiKey = apiKey ?? throw new ArgumentNullException(nameof(apiKey));

    public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {
        var httpContext = context.HttpContext;
        if (!httpContext.Request.Headers.TryGetValue("X-API-KEY", out var extractedApiKey) || extractedApiKey != _apiKey)
        {
            httpContext.Response.StatusCode = 401;
            await httpContext.Response.WriteAsync("Unauthorized");
            return null;
        }

        return await next(context);
    }
}