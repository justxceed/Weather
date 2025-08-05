using System.Globalization;

namespace Weather.Converters;

public class CloudCoverageToIconConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value == null)
        {
            return null;
        }

        int cloudPercent;
        if (value is int intValue)
        {
            cloudPercent = intValue;
        }
        else if (!int.TryParse(value.ToString(), out cloudPercent))
        {
            return null;
        }

        var imageName = cloudPercent switch
        {
            <= 30 => "sunny_icon.png",
            <= 70 => "partially_cloudy_icon.png",
            _ => "fully_cloudy_icon.png"
        };

        return ImageSource.FromFile(imageName);
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}