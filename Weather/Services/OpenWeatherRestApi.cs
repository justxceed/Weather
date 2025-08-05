using System.Net.Http.Json;
using Weather.Models;
using Weather.Services.Interfaces;

namespace Weather.Services;

public class OpenWeatherRestApi : IWeatherRestApi
{
    private readonly string apiKey = "f2ac96ecf1309026512928e93a6883d4";

    public async Task<WeatherInfo> GetWeatherAsync(string area)
    {
        using (var httpClient = new HttpClient())
        {
            var url = $"https://api.openweathermap.org/data/2.5/weather?q={area}&units=metric&appid={apiKey}";
            var response = await httpClient.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadFromJsonAsync<OpenWeatherResponse>();
                if (data != null && data.Weather != null && data.Weather.Length > 0)
                {
                    return new WeatherInfo
                    {
                        Description = data.Weather[0].Description,
                        Temperature = data.Main.Temp,
                        Location = data.Name,
                        CloudPercentage = data.Clouds.All
                    };
                }

                throw new Exception("Malformed weather data");
            }

            throw new Exception("Weather API call failed");
        }
    }
}