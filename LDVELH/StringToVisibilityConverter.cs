using System;
using System.Windows;
using System.Windows.Data;

namespace LDVELH
{
    [ValueConversion(sourceType: typeof(string), targetType: typeof(Visibility))]
    public class StringToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null || value == DependencyProperty.UnsetValue || (value is string valueAsString && string.IsNullOrEmpty(valueAsString)))
                return Visibility.Collapsed;
            else
                return Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
