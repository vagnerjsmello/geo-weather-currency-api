using GeoWeatherCurrencyApi.Models;

namespace GeoWeatherCurrencyApi.ExternalApis.Interfaces;

public interface IGeoDbExternalApi
{
    Task<GeoCityInfo?> GetCityInfoAsync(string country, string cityName);
}
