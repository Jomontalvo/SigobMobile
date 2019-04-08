namespace SigobMobile.Views.ManagementCenter
{
    using Xamarin.Forms;
    public partial class CalendarPage : ContentPage
    {
        #region Constructors
        public CalendarPage()
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
