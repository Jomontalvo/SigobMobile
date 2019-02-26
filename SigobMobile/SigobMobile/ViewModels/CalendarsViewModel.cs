using System;
namespace SigobMobile.ViewModels
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Windows.Input;
    using GalaSoft.MvvmLight.Command;
    using Helpers;
    using Models;
    using Services;
    using Xamarin.Forms;

    public class CalendarsViewModel : BaseViewModel
    {
        #region Services
        internal ApiService apiService;
        internal string apiGetCalendarsController = "calendars";
        internal string apiPostVisibilityController = "calendars/linkedcal/{0}/visible/{1}";
        #endregion

        #region Attributes
        private ObservableCollection<Calendar> calendars;
        private List<Calendar> calendarList;
        private bool isRefreshing;
        private bool isRunning;
        private bool isEnabled;
        private string selectDeselectAll;
        #endregion

        #region Properties
        public ObservableCollection<Calendar> CalendarList
        {
            get { return this.calendars; }
            set { SetValue(ref this.calendars, value); }
        }
        public bool IsRefreshing
        {
            get { return this.isRefreshing; }
            set { SetValue(ref this.isRefreshing, value); }
        }
        public bool IsRunning
        {
            get { return this.isRunning; }
            set { SetValue(ref this.isRunning, value); }
        }
        public bool IsEnabled
        {
            get { return this.isEnabled; }
            set { SetValue(ref this.isEnabled, value); }
        }
        public string SelectDeselectAll
        {
            get { return this.selectDeselectAll; }
            set { SetValue(ref this.selectDeselectAll, value); }
        }
        #endregion

        #region Constructors
        public CalendarsViewModel()
        {
            this.apiService = new ApiService();
            this.SelectDeselectAll = Languages.ShowAll;
            this.LoadCalendars();
            this.IsEnabled = true;
        }
        #endregion

        #region Methods
        private async void LoadCalendars()
        {
            this.IsRefreshing = true;
            var connection = await this.apiService.CheckConnection();
            if (!connection.IsSuccess)
            {
                this.IsRefreshing = false;
                await Application.Current.MainPage.DisplayAlert(
                    Languages.Error,
                    connection.Message,
                    Languages.Cancel);
                await Application.Current.MainPage.Navigation.PopModalAsync();
                return;
            }

            var response = await this.apiService.GetList<Calendar>(
                Settings.UrlBaseApiSigob,
                App.PrefixApiSigob,
                this.apiGetCalendarsController,
                Settings.Token,
                Settings.DbToken
            );
            if (!response.IsSuccess)
            {
                this.IsRefreshing = false;
                await Application.Current.MainPage.DisplayAlert(
                    Languages.Error,
                    response.Message,
                    Languages.Cancel);
                await Application.Current.MainPage.Navigation.PopModalAsync();
                return;
            }
            calendarList = (List<Calendar>)response.Result;
            CalendarList = new ObservableCollection<Calendar>(calendarList);
            IsRefreshing = false;
        }

        /// <summary>
        /// Sets the calendar visibility.
        /// </summary>
        /// <returns>The calendar visibility.</returns>
        /// <param name="itemCalendar">Identifier.</param>
        public async Task SetCalendarVisibility(Calendar itemCalendar)
        {
            var connection = await this.apiService.CheckConnection();
            if (!connection.IsSuccess)
            {
                this.IsRunning = false;
                await Application.Current.MainPage.DisplayAlert(
                    Languages.Error,
                    connection.Message,
                    Languages.Cancel);
                await Application.Current.MainPage.Navigation.PopModalAsync();
                return;
            }

            var response = await this.apiService.Post<bool>(
                Settings.UrlBaseApiSigob,
                App.PrefixApiSigob,
                string.Format(apiPostVisibilityController,itemCalendar.OfficeId, itemCalendar.IsVisible),
                Settings.Token,
                Settings.DbToken
            );
            if (!response.IsSuccess)
            {
                this.IsRunning = false;
                await Application.Current.MainPage.DisplayAlert(
                    Languages.Error,
                    response.Message,
                    Languages.Cancel);
                await Application.Current.MainPage.Navigation.PopModalAsync();
                return;
            }
            var resultSetVisibility = (bool)response.Result;
            // Upate Calendar item in ObservableCollection
        }

        /// <summary>
        /// Close view and return to Modal Async Parent
        /// </summary>
        private async void OkAndClose()
        {
            IsRunning = true;
            //Update all checkboxes
            foreach (var item in CalendarList)
            {
                await SetCalendarVisibility(item);
            }
            IsRefreshing = false;
            //Go to parent page
            await Application.Current.MainPage.Navigation.PopModalAsync();
        }
        /// <summary>
        /// Set all calendars with view enabled or disabled
        /// </summary>
        private void CheckAllCalendars()
        {
            IsEnabled = false;
            bool newState = (SelectDeselectAll == Languages.ShowAll);
            this.CalendarList.Select(c => { c.IsVisible = newState; return c; }).ToList();
            SelectDeselectAll = (SelectDeselectAll == Languages.ShowAll) ? Languages.HideAll : Languages.ShowAll;
            IsEnabled = true;
        }
        #endregion

        #region Commands
        public ICommand OkAndCloseCommand
        {
            get { return new RelayCommand(OkAndClose); }
        }

        public ICommand RefreshCommand
        {
            get { return new RelayCommand(LoadCalendars); }
        }
        public ICommand CheckAllCalendarsCommand
        {
            get { return new RelayCommand(CheckAllCalendars); }
        }
        #endregion
    }
}
