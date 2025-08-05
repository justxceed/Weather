namespace Weather.Models;

public class OpenWeatherResponse
{
    public WeatherMain Main { get; set; }
    public OpenWeather[] Weather { get; set; }

    public WeatherClouds Clouds { get; set; }
    public string Name { get; set; }
}