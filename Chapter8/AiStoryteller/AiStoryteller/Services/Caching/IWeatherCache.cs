namespace AiStoryteller.Services.Caching;
using WeatherForecast = AiStoryteller.Client.Models.WeatherForecast;
public interface IWeatherCache
{
    ValueTask<IImmutableList<WeatherForecast>> GetForecast(CancellationToken token);
}
