namespace GeoWeatherCurrencyApi.ExternalApis.Weather;

public class WeatherExternalApiResponse
{
    public MainInfo? Main { get; set; }

    public class MainInfo
    {
        public double Temp { get; set; }
    }
}

