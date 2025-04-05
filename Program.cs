using GeoWeatherCurrencyApi.Configuration;
using GeoWeatherCurrencyApi.ExternalApis;
using GeoWeatherCurrencyApi.ExternalApis.Interfaces;
using GeoWeatherCurrencyApi.Infrastructure.Extensions;
using GeoWeatherCurrencyApi.Services;
using GeoWeatherCurrencyApi.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configurations
var config = builder.Configuration;

var weatherConfig = config.GetSection(nameof(WeatherApiConfiguration)).Get<WeatherApiConfiguration>();
builder.Services.Configure<WeatherApiConfiguration>(config.GetSection(nameof(WeatherApiConfiguration)));

var geoConfig = config.GetSection(nameof(GeoDbApiConfiguration)).Get<GeoDbApiConfiguration>();
builder.Services.Configure<GeoDbApiConfiguration>(config.GetSection(nameof(GeoDbApiConfiguration)));

var exchangeConfig = config.GetSection(nameof(ExchangeRateApiConfiguration)).Get<ExchangeRateApiConfiguration>();
builder.Services.Configure<ExchangeRateApiConfiguration>(config.GetSection(nameof(ExchangeRateApiConfiguration)));

// Register HttpClients based on configuration
builder.Services.AddExternalApiClient<IWeatherExternalApi, WeatherExternalApi, WeatherApiConfiguration>(weatherConfig!);
builder.Services.AddExternalApiClient<IGeoDbExternalApi, GeoDbExternalApi, GeoDbApiConfiguration>(geoConfig!);
builder.Services.AddExternalApiClient<IExchangeRateExternalApi, ExchangeRateExternalApi, ExchangeRateApiConfiguration>(exchangeConfig!);

// Services
builder.Services.AddScoped<ILocationService, LocationService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "GeoWeatherCurrencyApi v1");
        options.RoutePrefix = string.Empty;

    });
}

app.UseHttpsRedirection();

// <summary>
/// Gets combined information from multiple external APIs (GeoDB, OpenWeather, ExchangeRate).
/// </summary>
/// <param name="country">Two-letter ISO country code (e.g., PT, BR, US)</param>
/// <param name="city">City name</param>
/// <param name="targetCurrency">Target currency code (e.g., USD, EUR)</param>
app.MapGet("/api/location-info", async (
    string city,
    string targetCurrency,
    string country,
    ILocationService locationService) =>
{
    var result = await locationService.GetLocationInfoAsync(country, city, targetCurrency);
    return result is not null
        ? Results.Ok(result)
        : Results.BadRequest("Unable to retrieve data.");
})
.WithName("GetLocationInfo")
.WithOpenApi();

app.Run();

