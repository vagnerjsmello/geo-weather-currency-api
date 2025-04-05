namespace GeoWeatherCurrencyApi.ExternalApis.GeoDb
{
    public class GeoExternalApiResponse
    {
        public List<GeoCity>? Data { get; set; }

    }

    public class GeoCity
    {
        private const string currencyCode = "EUR";

        public string City { get; set; } = string.Empty;
        public string Country { get; set; } = string.Empty;
        public string CurrencyCode { get; set; } = currencyCode;
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string? Region { get; set; }
        public string? CountryCode { get; set; }
        public List<string>? CurrencyCodes { get; set; }
    }
}
