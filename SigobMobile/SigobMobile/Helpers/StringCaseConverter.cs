namespace SigobMobile.Helpers
{
    using System;
    using System.Globalization;
    using Xamarin.Forms;

    public class StringCaseConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (parameter == null) parameter = "U";
            return ((parameter as string).ToUpper()[0]) switch
            {
                'U' => ((string)value).ToUpper(),
                'L' => ((string)value).ToLower(),
                _ => ((string)value),
            };
            ;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
