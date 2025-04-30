using Avalonia.Data.Converters;
using System;
using System.Globalization;

namespace DBExporter.Converters
{
    public class EnumToBooleanConverter : IValueConverter
    {
        public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value == null || parameter == null)
                return false;

            // Check if the enum value equals the parameter
            return value.Equals(parameter);
        }

        public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            // Only return the parameter if the radio button is checked (true)
            if (value is bool boolValue && boolValue && parameter != null)
                return parameter;

            // Don't create a default instance here - this was causing the issue
            // Just return null and the binding system will ignore it
            return null!;
        }
    }
}