namespace SigobMobile.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Windows.Input;
    using AsyncAwaitBestPractices.MVVM;
    using GalaSoft.MvvmLight.Command;
    using Helpers;
    using Controls;
    using Interfaces;
    using Models;
    using Services;
    using Xamarin.Forms;

    public class CalendarsViewModel : BaseViewModel
    {
        #region Services
        internal ApiService apiService;
        internal string apiGetCalendarsController = "calendars";
        internal string apiPostVisibilityController = "calendars/linkedcal/{0}/visible/{1}";
        internal string apiPostChangeColorController = "calendars/color/{0}";
        internal const string AllCalendars = "TODOS";
        #endregion

        #region Attributes
        private ObservableCollection<CalendarItemViewModel> calendars;
        private List<Calendar> calendarList;
        private bool isRefreshing;
        private bool isRunning;
        private bool isEnabled;
        private bool isOpen;
        private Color myCalendarColor;
        private string selectDeselectAll;
        #endregion

        #region Agenda Color Attributes
        private string iconColorChecked;
        private CalendarViewModel calendarViewModel;
        private InstructionsViewModel instructionsViewModel;
        #endregion

        #region Properties
        public ObservableCollection<CalendarItemViewModel> CalendarSource
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
        public bool IsOpen
        {
            get { return this.isOpen; }
            set { SetValue(ref this.isOpen, value); }
        }
        public string SelectDeselectAll
        {
            get { return this.selectDeselectAll; }
            set { SetValue(ref this.selectDeselectAll, value); }
        }

        public string MyAgendaName => Settings.FullName;

        public Color MyCalendarColor
        {
            get { return this.myCalendarColor; }
            set { SetValue(ref this.myCalendarColor, value); }
        }

        public string IconColorChecked
        {
            get { return this.iconColorChecked; }
            set { SetValue(ref this.iconColorChecked, value); }
        }

        #endregion

        #region Constructors
        public CalendarsViewModel()
        {
            this.ModelInitialization();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:SigobMobile.ViewModels.CalendarsViewModel"/> class.
        /// </summary>
        /// <param name="parentViewModel">Parent view model.</param>
        public CalendarsViewModel(object parentViewModel)
        {
            switch(parentViewModel.GetType().Name)
            {
                case "CalendarViewModel":
                    this.calendarViewModel = (CalendarViewModel)parentViewModel;
                    break;
                case "InstructionsViewModel":
                    this.instructionsViewModel = (InstructionsViewModel)parentViewModel;
                    break;
            }
            this.ModelInitialization();
        }

        /// <summary>
        /// Initialization view.
        /// </summary>
        private void ModelInitialization ()
        {
            apiService = new ApiService();
            IErrorHandler errorHandler = null;
            SelectDeselectAll = Languages.ShowAll;
            this.LoadCalendars().FireAndForgetSafeAsync(errorHandler);
        }
        #endregion

        #region Commands
        public ICommand OpenChangeColorCommand => new RelayCommand(OpenChangeColor);
        public ICommand CloseChangeColorCommand => new RelayCommand(CloseChangeColor);
        #endregion

        #region Async Commands
        public ICommand SelectColorCommand => new AsyncCommand<IconView>(SelectColor);
        public IAsyncCommand OkAndCloseCommand => new AsyncCommand(OkAndClose);
        public IAsyncCommand CheckAllCalendarsCommand => new AsyncCommand(CheckAllCalendars);
        #endregion

        #region Methods

        /// <summary>
        /// Selects the color of the calendar and change button.
        /// </summary>
        private async Task SelectColor(IconView colorSelected)
        {
            colorSelected.Source = (colorSelected.Source == "ic_circle_color") ? "ic_circle_check_color" : "ic_circle_color";
            var connection = await this.apiService.CheckConnection();
            if (!connection.IsSuccess)
            {
                this.IsRunning = false;
                await Application.Current.MainPage.DisplayAlert(
                    Languages.Error,
                    connection.Message,
                    Languages.Cancel);
                IsOpen = false;
                return;
            }

            //Change Foreground color to Number, before call EndPoint
            int numColor = ((Convert.ToInt32(colorSelected.Foreground.R * 255) * 256 * 256) +
                            (Convert.ToInt32(colorSelected.Foreground.G * 255) * 256) +
                             Convert.ToInt32(colorSelected.Foreground.B * 255));

            var response = await this.apiService.Put<bool>(
                Settings.UrlBaseApiSigob,
                App.PrefixApiSigob,
                string.Format(apiPostChangeColorController, numColor),
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
                IsOpen = false;
                return;
            }
            bool resultSetColor = (bool)response.Result;
            if (!resultSetColor)
            {
                this.IsRunning = false;
                await Application.Current.MainPage.DisplayAlert(
                    Languages.Error,
                    Languages.GeneralError,
                    Languages.Cancel);
                return;
            }
            MyCalendarColor = colorSelected.Foreground;
            colorSelected.Source = "ic_circle_color";
            IsOpen = false;
        }

        /// <summary>
        /// Loads the calendars.
        /// </summary>
        private async Task LoadCalendars()
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
            this.CalendarSource = new ObservableCollection<CalendarItemViewModel>(ToCalendarItemViewModel());
            MyCalendarColor = calendarList.Where(c => c.OfficeId == Settings.OfficeCode).Select(c => c.CalendarColor).First();
            IsRefreshing = false;
            IsEnabled = true;
        }

        /// <summary>
        /// Convert list obtained with API Call to ViewModel ObservableCollection.
        /// </summary>
        /// <returns>The calendar item view model.</returns>
        private IEnumerable<CalendarItemViewModel> ToCalendarItemViewModel()
        {
            return calendarList.Select(c => new CalendarItemViewModel
            {
                AgendaName = c.AgendaName,
                BlueColor = c.BlueColor,
                GreenColor = c.GreenColor,
                IsOwner = c.IsOwner,
                IsVisible = c.IsVisible,
                ManagementCenterId = c.ManagementCenterId,
                NumberColor = c.NumberColor,
                OfficeId = c.OfficeId,
                Permission = c.Permission,
                RedColor = c.RedColor
            }) .Where(c => c.OfficeId != Settings.OfficeCode);
        }

        /// <summary>
        /// Sets the calendar visibility for all items.
        /// </summary>
        /// <returns>The calendar visibility.</returns>
        /// <param name="isVisible">Identifier.</param>
        public async Task SetVisibilityToAllCalendars(bool isVisible)
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
                string.Format(apiPostVisibilityController, AllCalendars, isVisible),
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
            if (!resultSetVisibility)
            {
                this.IsRunning = false;
                await Application.Current.MainPage.DisplayAlert(
                    Languages.Error,
                    Languages.GeneralError,
                    Languages.Cancel);
                return;
            }
        }

        /// <summary>
        /// Close view and return to Modal Async Parent
        /// </summary>
        private async Task OkAndClose()
        {
            IsRunning = true;
            //Refresh Observable collection from Parent
            if (this.calendarViewModel != null) this.calendarViewModel.LoadAppointments(calendarViewModel.SelectedDate.GetValueOrDefault());
            else if (this.instructionsViewModel != null) await this.instructionsViewModel.LoadItems();
            IsRunning = false;
            //Go to parent page
            await Application.Current.MainPage.Navigation.PopModalAsync();
        }

        /// <summary>
        /// Set all calendars with view enabled or disabled
        /// </summary>
        private async Task CheckAllCalendars()
        {
            IsEnabled = false;
            IsRunning = true;
            bool newState = (SelectDeselectAll == Languages.ShowAll);
            this.CalendarSource.Select(c => { c.IsVisible = newState; return c; }).ToList();
            await SetVisibilityToAllCalendars(newState);
            SelectDeselectAll = (SelectDeselectAll == Languages.ShowAll) ? Languages.HideAll : Languages.ShowAll;
            IsRunning = false;
            await this.LoadCalendars();
            IsEnabled = true;
        }

        /// <summary>
        /// Open Modal Page with available colors
        /// </summary>
        private void OpenChangeColor()
        {
            IsOpen = true;
        }

        /// <summary>
        /// Changes the color and close modal view
        /// </summary>
        private void CloseChangeColor()
        {
            IsOpen = false;
        }
        #endregion


    }
}