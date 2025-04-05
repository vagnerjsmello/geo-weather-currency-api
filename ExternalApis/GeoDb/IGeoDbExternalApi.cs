using GeoWeatherCurrencyApi.Models;

namespace GeoWeatherCurrencyApi.ExternalApis.GeoDb;

public interface IGeoDbExternalApi
{
    Task<GeoCity?> GetCityDataAsync(string country, string cityName);
}
