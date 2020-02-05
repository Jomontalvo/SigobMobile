namespace SigobMobile.Helpers
{
    using System;
    using System.Collections;
    using System.Globalization;
    using Xamarin.Forms;

    public class DataSourceConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null) return ((IList)value).Count == 0;
            else return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }
}