namespace SigobMobile.Helpers
{
    using System;
    using System.Globalization;
    using Xamarin.Forms;
    public class IntToFontAttribute : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            byte attribute = (byte)value;
            var attributes = FontAttributes.None;
            switch (attribute)
            {
                case 1:
                    attributes = FontAttributes.Bold;
                    break;
                case 2:
                    attributes = FontAttributes.Italic;
                    break;
                case 3:
                    attributes = FontAttributes.Bold | FontAttributes.Italic;
                    break;
            }
            return attributes;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
