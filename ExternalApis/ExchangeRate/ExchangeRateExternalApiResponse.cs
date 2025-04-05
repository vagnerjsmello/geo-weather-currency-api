namespace GeoWeatherCurrencyApi.ExternalApis.ExchangeRate;

public class ExchangeRateExternalApiResponse
{
    public bool Success { get; set; }
    public string? Source { get; set; }
    public Dictionary<string, decimal>? Quotes { get; set; }
}
