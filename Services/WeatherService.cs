using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using WidgetDashboard.Models;

namespace WidgetDashboard.Services;

public class WeatherService
{
    private readonly HttpClient _httpClient = new();

    // Uses the free Open-Meteo API (no API key required)
    public async Task<WeatherData> GetWeatherAsync(double latitude, double longitude, string locationName)
    {
        try
        {
            var url = $"https://api.open-meteo.com/v1/forecast?latitude={latitude}&longitude={longitude}"
                    + "&current=temperature_2m,relative_humidity_2m,wind_speed_10m,weather_code";

            var response = await _httpClient.GetStringAsync(url);
            using var doc = JsonDocument.Parse(response);
            var current = doc.RootElement.GetProperty("current");

            var temp = current.GetProperty("temperature_2m").GetDouble();
            var humidity = current.GetProperty("relative_humidity_2m").GetInt32();
            var windSpeed = current.GetProperty("wind_speed_10m").GetDouble();
            var weatherCode = current.GetProperty("weather_code").GetInt32();

            var (condition, icon) = MapWeatherCode(weatherCode);

            return new WeatherData
            {
                Location = locationName,
                Temperature = temp,
                Condition = condition,
                Humidity = humidity,
                WindSpeed = windSpeed,
                Icon = icon,
            };
        }
        catch (Exception)
        {
            return GetFallbackWeather(locationName);
        }
    }

    public WeatherData GetFallbackWeather(string location = "New York")
    {
        return new WeatherData
        {
            Location = location,
            Temperature = 18.5,
            Condition = "Partly Cloudy",
            Humidity = 62,
            WindSpeed = 12.3,
            Icon = "\u26c5",
        };
    }

    private static (string condition, string icon) MapWeatherCode(int code)
    {
        return code switch
        {
            0 => ("Clear Sky", "\u2600\ufe0f"),
            1 => ("Mainly Clear", "\ud83c\udf24\ufe0f"),
            2 => ("Partly Cloudy", "\u26c5"),
            3 => ("Overcast", "\u2601\ufe0f"),
            45 or 48 => ("Foggy", "\ud83c\udf2b\ufe0f"),
            51 or 53 or 55 => ("Drizzle", "\ud83c\udf26\ufe0f"),
            61 or 63 or 65 => ("Rain", "\ud83c\udf27\ufe0f"),
            66 or 67 => ("Freezing Rain", "\ud83c\udf28\ufe0f"),
            71 or 73 or 75 => ("Snow", "\u2744\ufe0f"),
            77 => ("Snow Grains", "\u2744\ufe0f"),
            80 or 81 or 82 => ("Rain Showers", "\ud83c\udf26\ufe0f"),
            85 or 86 => ("Snow Showers", "\ud83c\udf28\ufe0f"),
            95 => ("Thunderstorm", "\u26c8\ufe0f"),
            96 or 99 => ("Thunderstorm w/ Hail", "\u26c8\ufe0f"),
            _ => ("Unknown", "?"),
        };
    }
}
