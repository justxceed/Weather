using Weather.Models;

namespace Weather.Services.Interfaces;

public interface IWeatherRestApi
{
    Task<WeatherInfo> GetWeatherAsync(string area);
}