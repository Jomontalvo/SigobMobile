namespace SigobMobile.Helpers
{
    using System;
    using System.Globalization;
    using Xamarin.Forms;
    public class BoldConverter : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var isBold = (bool)value;
            if (isBold)
            {
                return FontAttributes.Bold;                
            }
            else
            {
                return FontAttributes.None;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var isBold = (bool)value;
            if (isBold)
            {
                return FontAttributes.None;
            }
            else
            {
                return FontAttributes.Bold;
            }
        }
    }
}
