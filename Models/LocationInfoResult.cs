namespace GeoWeatherCurrencyApi.Models;

public class LocationInfoResult
{
    public string City { get; set; } = string.Empty;
    public string Country { get; set; } = string.Empty;
    public string CurrencyCode { get; set; } = string.Empty;
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public double TemperatureCelsius { get; set; }
    public decimal ExchangeRateToTarget { get; set; }
}
