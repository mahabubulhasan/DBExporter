using System;
using System.Globalization;
using Avalonia.Data.Converters;

namespace DBExporter.Converters;

public class BooleanInverterConverter : IValueConverter
{
    public static readonly BooleanInverterConverter Instance = new();

    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        => value is bool b ? !b : value;

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        => value is bool b ? !b : value;
}