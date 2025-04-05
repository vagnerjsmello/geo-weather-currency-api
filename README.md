# 🌍 GeoWeatherCurrency API (.NET 9)

A simple API made with **.NET 9**.  
It shows how to call **external public APIs**, combine the results, and return one single response with useful data.

---

## 🧠 Goal of the project

This project shows how to:

- ✅ Use `HttpClientFactory` with **typed clients**
- ✅ Call **3 different public APIs**: GeoDB, OpenWeather, ExchangeRate
- ✅ Combine the data in one response (DTO)
- ✅ Run multiple API calls at the same time (`Task.WhenAll`)
- ✅ Add retry and timeout with `Polly`
- ✅ Use `IOptions<T>` to read settings
- ✅ Organize the code with `Minimal API`

---

## 📡 Available endpoint

### `GET /api/location-info`

**Returns city + weather + exchange rate in one request**

| Parameter         | Type   | Required | Description                                 |
|------------------|--------|----------|---------------------------------------------|
| `country`        | string | ✅        | ISO country code (example: `PT`, `BR`)      |
| `city`           | string | ✅        | City name (example: `Porto`)                |
| `targetCurrency` | string | ✅        | Currency code to convert to (ex: `USD`)     |

📌 **Example call:**

```
GET https://localhost:7034/api/location-info?country=PT&city=Porto&targetCurrency=USD
```

📥 **Example response:**

```json
{
  "city": "Porto",
  "country": "Portugal",
  "currencyCode": "EUR",
  "latitude": 41.15,
  "longitude": -8.61,
  "temperatureCelsius": 23.5,
  "exchangeRateToTarget": 1.08
}
```

---

## 🔐 External APIs used

| Service          | API Base URL                                  | Where to get the API key                                     |
|------------------|-----------------------------------------------|---------------------------------------------------------------|
| **GeoDB Cities** | `https://wft-geo-db.p.rapidapi.com/v1/geo/`   | [RapidAPI GeoDB](https://rapidapi.com/wirefreethought/api/geodb-cities/) |
| **OpenWeather**  | `https://api.openweathermap.org/data/2.5/`    | [OpenWeather API](https://openweathermap.org/api)            |
| **ExchangeRate** | `https://api.exchangerate.host/` (via Apilayer) | [Apilayer ExchangeRate](https://apilayer.com/marketplace/exchangerates_data-api) |

---

## ⚙️ Settings (in `appsettings.json`)

```json
"WeatherApiConfiguration": {
  "BaseUrl": "https://api.openweathermap.org/data/2.5/",
  "ApiKey": "YOUR_KEY",
  "MaxRetryAttempts": 3,
  "TimeoutSeconds": 10,
  "Endpoints": {
    "CurrentWeatherByCoords": "weather?lat={0}&lon={1}&appid={2}&units=metric"
  }
},
"GeoDbApiConfiguration": {
  "BaseUrl": "https://wft-geo-db.p.rapidapi.com/v1/geo/",
  "ApiKey": "YOUR_KEY",
  "MaxRetryAttempts": 3,
  "TimeoutSeconds": 10,
  "Endpoints": {
    "SearchCity": "cities?namePrefix={0}&countryIds={1}&limit=1"
  }
},
"ExchangeRateApiConfiguration": {
  "BaseUrl": "https://api.exchangerate.host/",
  "ApiKey": "YOUR_KEY",
  "Endpoints": {
    "Live": "live?access_key={0}&source={1}&currencies={2}&format=1"
  }
}
```

---

## 🧱 Project structure

```
GeoWeatherCurrencyApi/
│
├── Configuration/                          
│   ├── ExchangeRateApiConfiguration.cs
│   ├── GeoDbApiConfiguration.cs
│   └── WeatherApiConfiguration.cs
├── ExternalApis/                         
│   ├── ExchangeRate/
│   │   ├── ExchangeRateExternalApi.cs
│   │   ├── ExchangeRateExternalApiResponse.cs
│   │   └── IExchangeRateExternalApi.cs
│   ├── GeoDb/
│   │   ├── GeoDbExternalApi.cs
│   │   ├── GeoExternalApiResponse.cs
│   │   └── IGeoDbExternalApi.cs
│   ├── Weather/
│   │   ├── WeatherExternalApi.cs
│   │   ├── WeatherExternalApiResponse.cs
│   │   └── IWeatherExternalApi.cs
│   └── BaseExternalApi.cs                  │
├── Infrastructure/
│   └── Extensions/
│       └── HttpClientConfigurationExtensions.cs
├── Models/                                 
│   └── LocationInfoResponse.cs
├── Services/                               
│   ├── ILocationService.cs
│   └── LocationService.cs
├── appsettings.json
├── .gitignore
├── LICENSE.txt
├── Program.cs
└── README.md

```

---

## 🧪 Test with Swagger

After running the project, open your browser and go to:

```
https://localhost:7034/swagger
```

You will see the only endpoint `GET /api/location-info`, ready to test.

---

## 🔁 Internal flow

1. Call **GeoDB** with city + country to get coordinates and currency
2. Call **OpenWeatherMap** and **ExchangeRate** at the same time
3. Combine all results in one final response

---

## 🧰 Tech used

- .NET **9**
- ASP.NET Core **Minimal API**
- `HttpClientFactory` with `Polly`
- `System.Text.Json`
- `IOptions<T>`
- Swagger (OpenAPI)
