namespace GeoWeatherCurrencyApi.Configuration;

/// <summary>
/// Configuration settings for the GeoDB Cities API.
/// </summary>
public class GeoDbApiConfiguration
{
    public string BaseUrl { get; set; } = string.Empty;
    public string ApiKey { get; set; } = string.Empty;
    public int MaxRetryAttempts { get; set; } = 3;
    public int TimeoutSeconds { get; set; } = 10;
    public GeoDbEndpoints Endpoints { get; set; } = new();

    public class GeoDbEndpoints
    {
        public string SearchCity { get; set; } = null!;
    }
}
