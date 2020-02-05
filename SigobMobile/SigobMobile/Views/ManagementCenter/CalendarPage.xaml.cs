using System;
using Telerik.XamarinForms.Input;
using Xamarin.Forms;

namespace SigobMobile.Views.ManagementCenter
{    
    public partial class CalendarPage : ContentPage
    {
        #region Constructors
        public CalendarPage()
        {
            InitializeComponent();
            if (!(Device.RuntimePlatform == Device.Android))
            {
                this.calendar.ScrollTimeIntoView(TimeSpan.FromHours(12));
            }
        }
        #endregion

        #region Events

        private void Calendar_SelectionChanged(object sender, Telerik.XamarinForms.Common.ValueChangedEventArgs<object> e)
        {
            //if (calendar.SelectedDate.HasValue)
            //{
            //    //calendar.SelectedDate = Settings.SelectedDate;
            //    calendar.DisplayDate = calendar.SelectedDate.Value;
            //}
        }

        private void Calendar_DisplayDateChanged(object sender, Telerik.XamarinForms.Common.ValueChangedEventArgs<object> e)
        {
            //if (calendar.SelectedDate.HasValue)
            //{
            //    //calendar.SelectedDate = Settings.SelectedDate;
            //    calendar.DisplayDate = calendar.SelectedDate.Value;
            //}
        }

        private void Calendar_ViewChanged(object sender, Telerik.XamarinForms.Common.ValueChangedEventArgs<CalendarViewMode> e)
        {
            //if (calendar.SelectedDate.HasValue)
            //{
            //    //calendar.SelectedDate = Settings.SelectedDate;
            //    calendar.DisplayDate = calendar.SelectedDate.Value;
            //}
            //this.calendar.DisplayDate = DateTime.Today;
            // Workaround for a limitation with ScrollToTime on Android
            //if (Device.RuntimePlatform == Device.Android)
            //{
            //    this.calendar.ScrollTimeIntoView(TimeSpan.FromHours(10));
            //    Device.BeginInvokeOnMainThread(() =>
            //    {
            //        this.calendar.ScrollTimeIntoView(TimeSpan.FromHours(12));
            //    });
            //}
            //else
            //{
            //    this.calendar.ScrollTimeIntoView(TimeSpan.FromHours(12));
            //}
        }


        private void Calendar_OnSelectionChanged(object sender, Telerik.XamarinForms.Common.ValueChangedEventArgs<object> e)
        {
            //if (calendar.SelectedDate.HasValue)
            //{
            //    //if (calendar.ViewMode == CalendarViewMode.Month)
            //    //{
            //    //    calendar.DisplayDate = calendar.SelectedDate.Value;
            //    //}
            //    calendar.DisplayDate = calendar.SelectedDate.Value;
            //}
            //if (calendar.ViewMode == CalendarViewMode.Year)
            //    Settings.CurrentCalendarViewMode = (int)CalendarViewMode.Month;
        }
        #endregion
    }
}
