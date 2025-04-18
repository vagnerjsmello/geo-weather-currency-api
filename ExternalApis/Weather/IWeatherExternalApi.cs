﻿namespace GeoWeatherCurrencyApi.ExternalApis.Weather;

public interface IWeatherExternalApi
{
    Task<double?> GetTemperatureCelsiusAsync(double latitude, double longitude);
}
