using GeoWeatherCurrencyApi.Configuration;
using GeoWeatherCurrencyApi.ExternalApis.Interfaces;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace GeoWeatherCurrencyApi.ExternalApis;

/// <summary>
/// Service to retrieve exchange rates using the /live endpoint from ExchangeRate API.
/// </summary>
public class ExchangeRateExternalApi : BaseExternalApi, IExchangeRateExternalApi
{
    private readonly HttpClient _httpClient;
    private readonly ExchangeRateApiConfiguration _config;

    public ExchangeRateExternalApi(
        HttpClient httpClient,
        IOptions<ExchangeRateApiConfiguration> options,
        ILogger<BaseExternalApi> logger)
        : base(logger)
    {
        _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        _config = options.Value ?? throw new ArgumentNullException(nameof(options));
        _httpClient.BaseAddress = new Uri(_config.BaseUrl);
    }

    /// <inheritdoc/>
    public async Task<decimal?> GetExchangeRateAsync(string fromCurrency, string toCurrency)
    {
        var endpoint = string.Format(_config.Endpoints.Live, _config.ApiKey, fromCurrency, toCurrency);

        var response = await _httpClient.GetAsync(endpoint);
        var json = await GetResponseAsync<JsonDocument>(response);

        if (json == null) return null;

        if (json.RootElement.TryGetProperty("quotes", out var quotesProp)
            && quotesProp.ValueKind == JsonValueKind.Object)
        {
            var key = fromCurrency + toCurrency;
            if (quotesProp.TryGetProperty(key, out var rateProp) && rateProp.TryGetDecimal(out var rate))
            {
                return rate;
            }
        }


        return null;
    }
}
