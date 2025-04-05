using GeoWeatherCurrencyApi.ExternalApis.Interfaces;
using GeoWeatherCurrencyApi.Models;
using GeoWeatherCurrencyApi.Services.Interfaces;

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

    public async Task<LocationInfoResult?> GetLocationInfoAsync(string country, string city, string targetCurrency)
    {
        var geo = await _geoExternalApi.GetCityInfoAsync(country, city);
        if (geo == null) return null;


        var tempTask = _weatherExternalApi.GetTemperatureCelsiusAsync(geo.Latitude, geo.Longitude);
        var rateTask = _exchangeRateExternalApi.GetExchangeRateAsync(geo.CurrencyCode, targetCurrency);

        await Task.WhenAll(tempTask, rateTask);

        var temp = await tempTask;
        var exchangeRate = await rateTask;

        if (temp == null || exchangeRate == null) return null;

        return new LocationInfoResult
        {
            City = geo.City,
            Country = geo.Country,
            CurrencyCode = geo.CurrencyCode,
            Latitude = geo.Latitude,
            Longitude = geo.Longitude,
            TemperatureCelsius = temp.Value,
            ExchangeRateToTarget = exchangeRate.Value
        };
    }

}
