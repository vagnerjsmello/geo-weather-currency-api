using GeoWeatherCurrencyApi.Configuration;
using GeoWeatherCurrencyApi.ExternalApis.Interfaces;
using GeoWeatherCurrencyApi.Models;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace GeoWeatherCurrencyApi.ExternalApis;

/// <summary>
/// Service to retrieve city and country information from GeoDB API.
/// </summary>
public class GeoDbExternalApi : BaseExternalApi, IGeoDbExternalApi
{
    private readonly HttpClient _httpClient;
    private readonly GeoDbApiConfiguration _config;

    /// <summary>
    /// Initializes a new instance of the <see cref="GeoDbExternalApi"/> class.
    /// </summary>
    public GeoDbExternalApi(
        HttpClient httpClient,
        IOptions<GeoDbApiConfiguration> options,
        ILogger<BaseExternalApi> logger)
        : base(logger)
    {
        _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        _config = options.Value ?? throw new ArgumentNullException(nameof(options));


        _httpClient.BaseAddress = new Uri(_config.BaseUrl);
        _httpClient.DefaultRequestHeaders.Add("X-RapidAPI-Key", _config.ApiKey);
        _httpClient.DefaultRequestHeaders.Add("X-RapidAPI-Host", "wft-geo-db.p.rapidapi.com");
    }

    /// <inheritdoc/>
    public async Task<GeoCityInfo?> GetCityInfoAsync(string inputCountry, string inputCityName)
    {
        var endpoint = string.Format(_config.Endpoints.SearchCity, inputCityName, inputCountry);

        var response = await _httpClient.GetAsync(endpoint);

        var result = await GetResponseAsync<JsonDocument>(response);

        var city = result?.RootElement
                         .GetProperty("data")
                         .EnumerateArray()
                         .FirstOrDefault();

        if (!city.HasValue || city.Value.ValueKind == JsonValueKind.Undefined)
            return null;

        string currencyCode = "EUR";

        if (city.Value.TryGetProperty("currencyCodes", out var currencies)
            && currencies.ValueKind == JsonValueKind.Array
            && currencies.GetArrayLength() > 0)
        {
            var code = currencies[0].GetString();
            if (!string.IsNullOrWhiteSpace(code))
            {
                currencyCode = code;
            }
        }

        return new GeoCityInfo
        {
            City = city.Value.TryGetProperty("city", out var cityProp) ? cityProp.GetString() ?? string.Empty : string.Empty,
            Country = city.Value.TryGetProperty("country", out var countryProp) ? countryProp.GetString() ?? string.Empty : string.Empty,
            Latitude = city.Value.TryGetProperty("latitude", out var latitudeProp) ? latitudeProp.GetDouble() : 0.0,
            Longitude = city.Value.TryGetProperty("longitude", out var longitudeProp) ? longitudeProp.GetDouble() : 0.0,
            CurrencyCode = currencyCode,
        };
    }

}
