namespace SigobMobile.Helpers
{
    using System;
    using System.Globalization;
    using Xamarin.Forms;

    /// <summary>
    /// Get KB value
    /// </summary>
    public class IntToKBytesConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (int)value/1024;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (int)value*1024;
        }
    }
}
