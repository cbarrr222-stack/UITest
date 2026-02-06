using System;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using WidgetDashboard.Services;

namespace WidgetDashboard.ViewModels;

public partial class WeatherViewModel : ObservableObject
{
    private readonly WeatherService _weatherService = new();

    [ObservableProperty] private string _location = "Loading...";
    [ObservableProperty] private string _temperature = "--";
    [ObservableProperty] private string _condition = "--";
    [ObservableProperty] private string _humidity = "--";
    [ObservableProperty] private string _windSpeed = "--";
    [ObservableProperty] private string _icon = "...";
    [ObservableProperty] private bool _isLoading = true;
    [ObservableProperty] private string _searchCity = "";

    // Preset cities for quick selection
    public string[] PresetCities { get; } = { "New York", "London", "Tokyo", "Paris", "Sydney", "Berlin" };

    public WeatherViewModel()
    {
        _ = LoadWeatherAsync("New York", 40.7128, -74.0060);
    }

    [RelayCommand]
    private async Task SearchWeather()
    {
        if (string.IsNullOrWhiteSpace(SearchCity)) return;

        var (lat, lon) = GetCityCoordinates(SearchCity.Trim());
        await LoadWeatherAsync(SearchCity.Trim(), lat, lon);
    }

    [RelayCommand]
    private async Task SelectCity(string city)
    {
        SearchCity = city;
        var (lat, lon) = GetCityCoordinates(city);
        await LoadWeatherAsync(city, lat, lon);
    }

    [RelayCommand]
    private async Task Refresh()
    {
        var (lat, lon) = GetCityCoordinates(Location);
        await LoadWeatherAsync(Location, lat, lon);
    }

    private async Task LoadWeatherAsync(string locationName, double lat, double lon)
    {
        IsLoading = true;
        try
        {
            var weather = await _weatherService.GetWeatherAsync(lat, lon, locationName);
            Location = weather.Location;
            Temperature = $"{weather.Temperature:F1}\u00b0C";
            Condition = weather.Condition;
            Humidity = $"{weather.Humidity}%";
            WindSpeed = $"{weather.WindSpeed:F1} km/h";
            Icon = weather.Icon;
        }
        catch
        {
            var fallback = _weatherService.GetFallbackWeather(locationName);
            Location = fallback.Location;
            Temperature = $"{fallback.Temperature:F1}\u00b0C";
            Condition = fallback.Condition;
            Humidity = $"{fallback.Humidity}%";
            WindSpeed = $"{fallback.WindSpeed:F1} km/h";
            Icon = fallback.Icon;
        }
        finally
        {
            IsLoading = false;
        }
    }

    private static (double lat, double lon) GetCityCoordinates(string city)
    {
        return city.ToLowerInvariant() switch
        {
            "new york" => (40.7128, -74.0060),
            "london" => (51.5074, -0.1278),
            "tokyo" => (35.6762, 139.6503),
            "paris" => (48.8566, 2.3522),
            "sydney" => (-33.8688, 151.2093),
            "berlin" => (52.5200, 13.4050),
            "los angeles" => (34.0522, -118.2437),
            "chicago" => (41.8781, -87.6298),
            "toronto" => (43.6532, -79.3832),
            "mumbai" => (19.0760, 72.8777),
            "dubai" => (25.2048, 55.2708),
            "singapore" => (1.3521, 103.8198),
            "hong kong" => (22.3193, 114.1694),
            "seoul" => (37.5665, 126.9780),
            "moscow" => (55.7558, 37.6173),
            "rome" => (41.9028, 12.4964),
            "madrid" => (40.4168, -3.7038),
            "beijing" => (39.9042, 116.4074),
            "san francisco" => (37.7749, -122.4194),
            "miami" => (25.7617, -80.1918),
            _ => (40.7128, -74.0060), // Default to NYC
        };
    }
}
