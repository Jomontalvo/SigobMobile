namespace SigobMobile.Helpers
{
    using System;
    using System.Globalization;
    using Xamarin.Forms;

    /// <summary>
    /// Color to source icon converter.
    /// </summary>
    public class ColorToSourceIconConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string returningSource = "ic_circle_color";
            if (value is Color?)
            {
                if (parameter != null && parameter is Color)
                    if ((Color)parameter == (Color)value)
                        returningSource = "ic_circle_check_color";
            }
            return returningSource;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new Exception("Conversion not allowed");
        }
    }
}
