using GeoWeatherCurrencyApi.Configuration;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace GeoWeatherCurrencyApi.ExternalApis.ExchangeRate;

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
        var result = await GetResponseAsync<ExchangeRateExternalApiResponse>(response);

        if (result == null || result.Quotes == null)
            return null;

        var key = fromCurrency + toCurrency;
        if (result.Quotes.TryGetValue(key, out var rate))
        {
            return rate;
        }

        return null;
    }

}
