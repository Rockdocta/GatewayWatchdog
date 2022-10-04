using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace GatewayWatchdog
{
    public class StringEmptyConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return parameter;
            if (value.GetType() == typeof(string))
                return string.IsNullOrEmpty((string)value) ? parameter : value;
            if (value.GetType() == typeof(double))
                return (double)value;
            if (value.GetType() == typeof(int))
                return (int)value;

            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }


    public class VisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (bool)value == true ? Visibility.Visible : (parameter ?? Visibility.Hidden);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}


