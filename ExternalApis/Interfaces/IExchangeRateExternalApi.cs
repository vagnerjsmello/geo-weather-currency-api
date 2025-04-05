namespace GeoWeatherCurrencyApi.ExternalApis.Interfaces;

public interface IExchangeRateExternalApi
{
    Task<decimal?> GetExchangeRateAsync(string fromCurrency, string toCurrency);
}
