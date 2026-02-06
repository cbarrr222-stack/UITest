namespace WidgetDashboard.Models;

public class WeatherData
{
    public string Location { get; set; } = "Unknown";
    public double Temperature { get; set; }
    public string Condition { get; set; } = "Unknown";
    public int Humidity { get; set; }
    public double WindSpeed { get; set; }
    public string Icon { get; set; } = "?";
}
