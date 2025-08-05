using System.Globalization;

namespace Weather.Converters;

public class TemperatureToColorConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is double temperature)
        {
            return temperature switch
            {
                < 0 => Colors.Blue,
                < 10 => Colors.DodgerBlue,
                < 20 => Colors.ForestGreen,
                < 30 => Colors.DarkOrange,
                < 40 => Colors.Red,
                _ => Colors.Black
            };
        }
        return Colors.Black;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}