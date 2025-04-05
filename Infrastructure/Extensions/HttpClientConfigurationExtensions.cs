using Polly.Extensions.Http;
using Polly;

namespace GeoWeatherCurrencyApi.Infrastructure.Extensions;

public static class HttpClientConfigurationExtensions
{
    public static IHttpClientBuilder AddExternalApiClient<TInterface, TImplementation, TConfig>(
        this IServiceCollection services,
        TConfig config)
        where TInterface : class
        where TImplementation : class, TInterface
        where TConfig : class
    {
        return services.AddHttpClient<TInterface, TImplementation>(client =>
        {
            if (!string.IsNullOrWhiteSpace(config.GetType().GetProperty("BaseUrl")?.GetValue(config)?.ToString()))
            {
                client.BaseAddress = new Uri(config.GetType().GetProperty("BaseUrl")?.GetValue(config)?.ToString()!);
            }
        })
        .AddPolicyHandler(GetRetryPolicy((int)config.GetType().GetProperty("MaxRetryAttempts")?.GetValue(config)!))
        .AddPolicyHandler(GetTimeoutPolicy((int)config.GetType().GetProperty("TimeoutSeconds")?.GetValue(config)!));
    }

    private static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy(int maxRetries) =>
        HttpPolicyExtensions
            .HandleTransientHttpError()
            .WaitAndRetryAsync(maxRetries, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));

    private static IAsyncPolicy<HttpResponseMessage> GetTimeoutPolicy(int timeoutSeconds) =>
        Policy.TimeoutAsync<HttpResponseMessage>(TimeSpan.FromSeconds(timeoutSeconds));
}
