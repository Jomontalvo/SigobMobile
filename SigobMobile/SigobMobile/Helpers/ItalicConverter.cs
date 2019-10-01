namespace SigobMobile.Helpers
{
    using System;
    using System.Globalization;
    using Xamarin.Forms;
    public class ItalicConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var isItalic = (bool)value;
            if (isItalic)
            {
                return FontAttributes.Italic;
            }
            else
            {
                return FontAttributes.None;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var isItalic = (bool)value;
            if (isItalic)
            {
                return FontAttributes.None;
            }
            else
            {
                return FontAttributes.Italic;
            }
        }
    }
}
