namespace SigobMobile.Views.ManagementCenter
{
    using ViewModels;
    using Telerik.XamarinForms.Input;
    using Xamarin.Forms;
    public partial class CalendaDayPage : ContentPage
    {
        #region Constructors
        public CalendaDayPage()
        {
            InitializeComponent();
        }
        #endregion

        #region Events
        void Calendar_SelectionChanged(object sender, Telerik.XamarinForms.Common.ValueChangedEventArgs<object> e)
        {
            if (e.PreviousValue != null)
            if (calendar.ViewMode == CalendarViewMode.Month )
            {
                //calendar.TrySetViewMode(CalendarViewMode.Day);
                //calendar.DisplayDate = calendar.SelectedDate.GetValueOrDefault();
                //calendar.SelectedDate = calendar.SelectedDate.GetValueOrDefault();
            }
        }
        #endregion

    }
}
