namespace GeoWeatherCurrencyApi.ExternalApis.Interfaces;

public interface IWeatherExternalApi
{
    Task<double?> GetTemperatureCelsiusAsync(double latitude, double longitude);
}
