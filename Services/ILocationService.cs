using GeoWeatherCurrencyApi.Models;

namespace GeoWeatherCurrencyApi.Services;

public interface ILocationService
{
    Task<LocationInfoResponse?> GetLocationInfoAsync(string country, string city, string targetCurrency);
}

