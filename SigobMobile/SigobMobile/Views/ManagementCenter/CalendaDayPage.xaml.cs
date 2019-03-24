namespace SigobMobile.Views.ManagementCenter
{
    using ViewModels;
    using Telerik.XamarinForms.Input;
    using Xamarin.Forms;
    using Helpers;
    public partial class CalendaDayPage : ContentPage
    {
        #region Constructors
        public CalendaDayPage()
        {
            InitializeComponent();
        }
        #endregion

        #region Events
        void Calendar_OnSelectionChanged(object sender, Telerik.XamarinForms.Common.ValueChangedEventArgs<object> e)
        {
            //RadCalendar cal = (RadCalendar)sender;
            //if (cal.ViewMode == CalendarViewMode.Year) Settings.CurrentCalendarViewMode = (int)CalendarViewMode.Month;
        }
        #endregion

    }
}
