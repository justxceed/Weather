using Weather.Services.Interfaces;

namespace Weather.Services;

public class GeoLocationService : IGeoLocationService
{
    public async Task<string> GetAreaFromLocationAsync()
    {
        try
        {
            if (!await RequestPermission())
            {
                return null;
            }

            var location = await Geolocation.GetLastKnownLocationAsync()
                           ?? await Geolocation.GetLocationAsync();
            if (location != null)
            {
                var placemarks = await Geocoding.GetPlacemarksAsync(location.Latitude, location.Longitude);
                var placemark = placemarks?.FirstOrDefault();

                if (placemark != null)
                {
                    // Try 'Locality' first (city); fallback to other properties if needed
                    return placemark.Locality
                           ?? placemark.SubAdminArea
                           ?? placemark.AdminArea
                           ?? placemark.CountryName
                           ?? $"{location.Latitude},{location.Longitude}";
                }
            }
        }
        catch (Exception ex)
        {
            // Log error
        }

        return null;
    }

    private async Task<bool> RequestPermission()
    {
        if (await Permissions.CheckStatusAsync<Permissions.LocationWhenInUse>() == PermissionStatus.Granted)
        {
            return true;
        }

        var status = await Permissions.RequestAsync<Permissions.LocationWhenInUse>();
        if (status != PermissionStatus.Granted)
        {
            // Handle the case when user denied permission (show message, etc)
            return false;
        }

        return true;
    }
}