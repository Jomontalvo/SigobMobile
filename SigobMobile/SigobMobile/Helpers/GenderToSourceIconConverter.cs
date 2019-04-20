namespace SigobMobile.Helpers
{
    using System;
    using System.Globalization;
    using Models;
    using ViewModels;
    using Xamarin.Forms;

    /// <summary>
    /// Gender to source icon converter.
    /// </summary>
    public class GenderToSourceIconConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string returningSource = "ic_contact_external";
            if (!(parameter is BindableObject bindableObject))
                return returningSource;
            var item = bindableObject.BindingContext;
            ParticipantItemViewModel itemViewModel = item as ParticipantItemViewModel;
            if (!string.IsNullOrEmpty(itemViewModel.OfficeId))
                returningSource = ((Gender)value == Gender.Female) ? "ic_contact_female" : "ic_contact_male";
            return returningSource;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new Exception("Conversion not allowed");
        }
    }
}