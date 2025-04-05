using System.Text;
using System.Text.Json;

namespace GeoWeatherCurrencyApi.ExternalApis;

/// <summary>
/// Abstract base class for external API service implementations.
/// Provides common functionality such as logging, serialization and response parsing.
/// </summary>
public abstract class BaseExternalApi
{
    private readonly ILogger<BaseExternalApi> _logger;

    /// <summary>
    /// Default JSON serialization options using camelCase and ignoring nulls.
    /// </summary>
    protected static readonly JsonSerializerOptions _jsonOptions = new JsonSerializerOptions
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        WriteIndented = false,
        DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
    };

    /// <summary>
    /// Initializes a new instance of the <see cref="BaseExternalApi"/> class.
    /// </summary>
    /// <param name="logger">Logger instance for structured logging.</param>
    protected BaseExternalApi(ILogger<BaseExternalApi> logger)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>
    /// Serializes an object into a JSON StringContent for HTTP request bodies.
    /// </summary>
    /// <typeparam name="T">Type of the object to serialize.</typeparam>
    /// <param name="request">The request object.</param>
    /// <returns>StringContent with JSON payload.</returns>
    protected StringContent GetStringContent<T>(T request)
    {
        var contentString = JsonSerializer.Serialize(request, _jsonOptions);

        _logger.LogInformation("Sending JSON payload from {Method}: {Payload}",
            nameof(GetStringContent), contentString);

        return new StringContent(contentString, Encoding.UTF8, "application/json");
    }

    /// <summary>
    /// Deserializes a successful HTTP response body into the specified type.
    /// Logs both success and error scenarios.
    /// </summary>
    /// <typeparam name="T">Type to deserialize into.</typeparam>
    /// <param name="httpResponseMessage">HTTP response received from API.</param>
    /// <returns>Deserialized object or throws exception if response is not successful.</returns>
    /// <exception cref="HttpRequestException">Thrown when the response is not successful.</exception>
    protected async Task<T?> GetResponseAsync<T>(HttpResponseMessage httpResponseMessage)
    {
        var responseBody = await httpResponseMessage.Content.ReadAsStringAsync();

        _logger.LogInformation("Received response at {Method}: {Content}",
            nameof(GetResponseAsync), responseBody);

        if (!httpResponseMessage.IsSuccessStatusCode)
        {
            _logger.LogError("Request failed with status code {StatusCode}. Response: {Response}",
                httpResponseMessage.StatusCode, responseBody);

            throw new HttpRequestException($"External API call failed: {responseBody}");
        }

        return JsonSerializer.Deserialize<T>(responseBody, _jsonOptions);
    }

    /// <summary>
    /// Retrieves the value of a specific response header, if present.
    /// </summary>
    /// <param name="response">The HTTP response message.</param>
    /// <param name="headerName">The name of the header to retrieve.</param>
    /// <returns>The header value or null if not found.</returns>
    protected string? GetHeaderValue(HttpResponseMessage response, string headerName)
    {
        if (response.Headers.TryGetValues(headerName, out var values))
        {
            return values.FirstOrDefault();
        }

        return null;
    }
}
