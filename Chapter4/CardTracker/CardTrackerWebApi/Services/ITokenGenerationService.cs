namespace CardTrackerWebApi.Services;

public interface ITokenGenerationService
{
    string GenerateToken(string username, string role);
}