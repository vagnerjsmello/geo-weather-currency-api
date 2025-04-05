namespace GeoWeatherCurrencyApi.Models;

public class GeoCityInfo
{
    private const string currencyCode = "EUR";

    public string City { get; set; } = string.Empty;
    public string Country { get; set; } = string.Empty;
    public string CurrencyCode { get; set; } = currencyCode;
    public double Latitude { get; set; }
    public double Longitude { get; set; }
}
