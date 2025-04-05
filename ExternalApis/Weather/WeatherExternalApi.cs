using GeoWeatherCurrencyApi.Configuration;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace GeoWeatherCurrencyApi.ExternalApis.Weather;

/// <summary>
/// Service to retrieve current temperature data from OpenWeatherMap API.
/// </summary>
public class WeatherExternalApi : BaseExternalApi, IWeatherExternalApi
{
    private readonly HttpClient _httpClient;
    private readonly WeatherApiConfiguration _config;

    /// <summary>
    /// Initializes a new instance of the <see cref="WeatherExternalApi"/> class.
    /// </summary>
    public WeatherExternalApi(
        HttpClient httpClient,
        IOptions<WeatherApiConfiguration> options,
        ILogger<BaseExternalApi> logger)
        : base(logger)
    {
        _httpClient = httpClient;
        _config = options.Value;

        _httpClient.BaseAddress = new Uri(_config.BaseUrl);
    }

    /// <inheritdoc />
    public async Task<double?> GetTemperatureCelsiusAsync(double latitude, double longitude)
    {
        var endpoint = string.Format(_config.Endpoints.CurrentWeatherByCoords,latitude,longitude,_config.ApiKey);

        var response = await _httpClient.GetAsync(endpoint);
        var result = await GetResponseAsync<WeatherExternalApiResponse>(response);

        return result?.Main?.Temp;
    }

}

