namespace ModelContextProtocol.ApiService.Services;

public interface ISearchService
{
    Task<string> Search(string query);
    Task<string> Ask(string query);
}