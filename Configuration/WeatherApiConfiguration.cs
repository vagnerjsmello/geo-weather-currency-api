namespace GeoWeatherCurrencyApi.Configuration;

/// <summary>
/// Configuration settings for the OpenWeatherMap API.
/// </summary>
public class WeatherApiConfiguration
{
    public string BaseUrl { get; set; } = string.Empty;
    public string ApiKey { get; set; } = string.Empty;
    public int MaxRetryAttempts { get; set; } = 3;
    public int TimeoutSeconds { get; set; } = 10;
    public WeatherEndpoints Endpoints { get; set; } = new();

    public class WeatherEndpoints
    {
        public string CurrentWeatherByCoords { get; set; } = null!;
    }
}
