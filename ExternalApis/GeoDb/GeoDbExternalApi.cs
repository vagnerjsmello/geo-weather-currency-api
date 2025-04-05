using GeoWeatherCurrencyApi.Configuration;
using GeoWeatherCurrencyApi.Models;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace GeoWeatherCurrencyApi.ExternalApis.GeoDb;

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
    public async Task<GeoCity?> GetCityDataAsync(string inputCountry, string inputCityName)
    {
        var endpoint = string.Format(_config.Endpoints.SearchCity, inputCityName, inputCountry);

        var response = await _httpClient.GetAsync(endpoint);
        var result = await GetResponseAsync<GeoExternalApiResponse>(response);        

        return result?.Data?.FirstOrDefault();
    }

}
