namespace Weather.Services.Interfaces;

public interface IGeoLocationService
{
    Task<string> GetAreaFromLocationAsync();
}