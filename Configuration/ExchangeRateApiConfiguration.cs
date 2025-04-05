namespace GeoWeatherCurrencyApi.Configuration;

/// <summary>
/// Configuration settings for the ExchangeRate.host API.
/// </summary>
public class ExchangeRateApiConfiguration
{
    public string BaseUrl { get; set; } = string.Empty;
    public string? ApiKey { get; set; }
    public int MaxRetryAttempts { get; set; } = 3;
    public int TimeoutSeconds { get; set; } = 10;

    public ExchangeRateEndpoints Endpoints { get; set; } = new();

    public class ExchangeRateEndpoints
    {
        public string Live { get; set; } = null!;
    }
}
