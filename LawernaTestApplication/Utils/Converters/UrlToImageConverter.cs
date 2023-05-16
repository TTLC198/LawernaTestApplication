﻿using System;
using System.Windows.Data;

namespace LawernaTestApplication.Utils.Converters;

public class UrlToImageConverter : IValueConverter
{
    public static UrlToImageConverter Instance { get; } = new();
    
    public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
    {
        return $"{App.CdnUrl}{value as string}@2x.png";
    }

    public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}