using GeoWeatherCurrencyApi.Models;

namespace GeoWeatherCurrencyApi.Services.Interfaces;

public interface ILocationService
{
    Task<LocationInfoResult?> GetLocationInfoAsync(string country, string city, string targetCurrency);
}

