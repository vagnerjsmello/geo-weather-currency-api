using GeoWeatherCurrencyApi.ExternalApis.ExchangeRate;
using GeoWeatherCurrencyApi.ExternalApis.GeoDb;
using GeoWeatherCurrencyApi.ExternalApis.Weather;
using GeoWeatherCurrencyApi.Models;

namespace GeoWeatherCurrencyApi.Services;

public class LocationService : ILocationService
{
    private readonly IGeoDbExternalApi _geoExternalApi;
    private readonly IWeatherExternalApi _weatherExternalApi;
    private readonly IExchangeRateExternalApi _exchangeRateExternalApi;

    public LocationService(
        IGeoDbExternalApi geoExternalApi,
        IWeatherExternalApi weatherExternalApi,
        IExchangeRateExternalApi exchangeRateExternalApi
    )
    {
        _geoExternalApi = geoExternalApi ?? throw new ArgumentNullException(nameof(geoExternalApi));
        _weatherExternalApi = weatherExternalApi ?? throw new ArgumentNullException(nameof(weatherExternalApi));
        _exchangeRateExternalApi = exchangeRateExternalApi ?? throw new ArgumentNullException(nameof(exchangeRateExternalApi));
    }

    public async Task<LocationInfoResponse?> GetLocationInfoAsync(string country, string city, string targetCurrency)
    {
        var geoCity = await _geoExternalApi.GetCityDataAsync(country, city);
        if (geoCity == null) return null;


        var tempTask = _weatherExternalApi.GetTemperatureCelsiusAsync(geoCity.Latitude, geoCity.Longitude);
        var rateTask = _exchangeRateExternalApi.GetExchangeRateAsync(geoCity.CurrencyCode, targetCurrency);

        await Task.WhenAll(tempTask, rateTask);

        var temp = await tempTask;
        var exchangeRate = await rateTask;

        if (temp == null || exchangeRate == null) return null;

        return new LocationInfoResponse
        {
            City = geoCity.City,
            Country = geoCity.Country,
            CurrencyCode = geoCity.CurrencyCode,
            Latitude = geoCity.Latitude,
            Longitude = geoCity.Longitude,
            TemperatureCelsius = temp.Value,
            ExchangeRateToTarget = exchangeRate.Value
        };
    }

}
