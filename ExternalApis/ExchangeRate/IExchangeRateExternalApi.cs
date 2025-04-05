namespace GeoWeatherCurrencyApi.ExternalApis.ExchangeRate;

public interface IExchangeRateExternalApi
{
    Task<decimal?> GetExchangeRateAsync(string fromCurrency, string toCurrency);
}
