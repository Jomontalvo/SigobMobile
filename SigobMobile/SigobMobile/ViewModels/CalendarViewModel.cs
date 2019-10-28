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
    using Interfaces;
    using Models;
    using Services;
    using Telerik.XamarinForms.Input;
    using Telerik.XamarinForms.Input.Calendar.Commands;
    using Views.ManagementCenter;
    using Views.Tasks;
    using Xamarin.Forms;

    public class CalendarViewModel : BaseViewModel
    {
        #region Services
        internal ApiService apiService;
        #endregion

        #region ApiControllers
        internal string apiEventsController = "events/start/{0}/end/{1}/tentatives/{2}/calendarcolor/{3}";
        internal string apiEventCgController = "cgcal/{0}/events/{1}";
        internal string apiPersonalAppointmentController = "events/{0}/owner/{1}";
        internal string apiManagementCenterAddOptions = "cg/addoptions";
        #endregion

        #region Attributes
        private ObservableCollection<Event> events;
        private bool isRunning;
        private bool isEnabled;
        private string startDate;
        private string endDate;
        private byte viewTentative;
        private DateTime? selectedDate;
        private DateTime displayDate;
        private List<AppointmentItem> eventList;
        private CalendarViewMode calendarView;
        private List<ManagementCenterNewItem> itemsToAddList;
        #endregion

        #region Properties

        public CalendarViewMode CalendarView
        {
            get { return this.calendarView; }
            set { SetValue(ref this.calendarView, value); }
        }
        public ObservableCollection<Event> Events
        {
            get { return this.events; }
            set { SetValue(ref this.events, value); }
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
        public string StartDate
        {
            get { return this.startDate; }
            set { SetValue(ref this.startDate, value); }
        }
        public string EndDate
        {
            get { return this.endDate; }
            set { SetValue(ref this.endDate, value); }
        }
        public byte ViewTentative
        {
            get { return this.viewTentative; }
            set { SetValue(ref this.viewTentative, value); }
        }
        public DateTime? SelectedDate
        {
            get => this.selectedDate;
            set
            {
                SetValue(ref this.selectedDate, value);
                //DateTime currentSelectedDate = this.selectedDate.GetValueOrDefault();
                //if (currentSelectedDate.Month != this.DisplayDate.Month || currentSelectedDate.Year != this.DisplayDate.Year)
                //    this.DisplayDate = this.selectedDate.GetValueOrDefault();
            }
        }
        public DateTime DisplayDate
        {
            get { return this.displayDate; }
            set { SetValue(ref this.displayDate, value); }
        }
        #endregion

        #region Constructors
        public CalendarViewModel()
        {
            this.apiService = new ApiService();
            this.LoadAppointments(DateTime.Today);
            this.CalendarView = (CalendarViewMode)Settings.CurrentCalendarViewMode;
            this.SelectedDate = Settings.SelectedDate;
            this.DisplayDate = Settings.SelectedDate;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Load the appointments.
        /// </summary>
        /// <param name="date">Date.</param>
        public void LoadAppointments(DateTime date)
        {
            IErrorHandler errorHandler = null;
            UpdateAppointments(date).FireAndForgetSafeAsync(errorHandler);
        }

        /// <summary>
        /// Update appointments Async Task.
        /// </summary>
        /// <returns>The appointments.</returns>
        /// <param name="date">Date.</param>
        public async Task UpdateAppointments(DateTime date)
        {
            DateTime pivot = (date == default) ? DateTime.Today : date;
            this.IsRunning = true;
            startDate = pivot.AddYears(-2).ToString("yyyyMMdd");
            endDate = pivot.AddYears(5).ToString("yyyyMMdd");
            var connection = await this.apiService.CheckConnection();
            if (!connection.IsSuccess)
            {
                IsRunning = false;
                await Application.Current.MainPage.DisplayAlert(
                    Languages.Error,
                    connection.Message,
                    Languages.Cancel);
                await Application.Current.MainPage.Navigation.PopAsync();
                return;
            }
            var response = await this.apiService.GetList<AppointmentItem>(
                Settings.UrlBaseApiSigob,
                App.PrefixApiSigob,
                string.Format(this.apiEventsController, startDate, endDate, viewTentative, Settings.IsEventColorByCalendar),
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
                return;
            }
            this.eventList = (List<AppointmentItem>)response.Result;
            this.Events = new ObservableCollection<Event>(ToCalendarEvents());
            this.IsRunning = false;
        }

        private IEnumerable<Event> ToCalendarEvents()
        {
            return this.eventList.Select(l => new Event
            {
                Id = l.Id,
                Title = (l.Status == StatusAppointment.Suspended) ? $"[{Languages.SuspendedStatus}] {ConvertNullToEmpty(l.Description)}" : l.IsTentative ? $"{ConvertNullToEmpty(l.Description)} ({Languages.TentativeLabel})" : ConvertNullToEmpty(l.Description),
                Color = l.IsTask ? Color.FromRgb(l.RedColorType, l.GreenColorType, l.BlueColorType) : (l.Status != StatusAppointment.Suspended) ? Color.FromRgb(l.RedColorItem, l.GreenColorItem, l.BlueColorItem) : (Color)Application.Current.Resources["grayBorder"],
                Detail = l.Place,
                StartDate = (l.Id == 0 || l.IsTask) ? l.End.DateTime.ToLocalTime().Date : l.Start.DateTime.ToLocalTime(), // IsHoliday or Task Control 
                EndDate = (l.Id == 0 || l.IsTask) ? l.End.DateTime.ToLocalTime().Date.AddMinutes(1) : l.End.DateTime.ToLocalTime(),
                IsAllDay = l.IsTask || l.Id == 0 || ((l.End - l.Start).Hours >= 24),
                IsLocked = l.IsVisible == 1,
                IsTentative = l.IsTentative,
                Owner = l.AgendaOwner,
                Programmer = l.ProgrammerAgenda,
                TypeColor = Color.FromRgb(l.RedColorType, l.GreenColorType, l.BlueColorType),
                ModuleType = l.ModuleType,
                IsHighlighted = l.IsHighlighted,
                IsTask = l.IsTask,
                IsVisible = (l.IsVisible == 1),
                SecurityLevel = l.SecurityLevel,
                TypeId = l.TypeId,
                Status = l.Status
            }).Where(l => (Settings.IsVisibleManagementStatus && (l.Status == StatusAppointment.InManagement))
                        || (Settings.IsVisibleCompletedStatus && (l.Status == StatusAppointment.Finished))
                        || (Settings.IsVisibleSuspendStatus && (l.Status == StatusAppointment.Suspended)));
        }

        private string ConvertNullToEmpty(string str)
        {
            return (string.IsNullOrWhiteSpace(str)) ? string.Empty : str;
        }

        /// <summary>
        /// Convert the date to int parameter for Api call.
        /// </summary>
        /// <returns>The date to int parameter.</returns>
        /// <param name="dateParameter">Date parameter.</param>
        private string GetDateToIntParameter(DateTime dateParameter)
        {
            string dy = dateParameter.Day.ToString("00");
            string mn = dateParameter.Month.ToString("00");
            string yy = dateParameter.Year.ToString();
            return $"{yy}{mn}{dy}";
        }

        /// <summary>
        /// Opens the calendars.
        /// </summary>
        private async Task OpenCalendars()
        {
            var calendarsMainViewModel = MainViewModel.GetInstance();
            calendarsMainViewModel.Calendars = new CalendarsViewModel(this);
            await Application.Current.MainPage.Navigation.PushModalAsync(new NavigationPage(new CalendarsPage()));
        }

        /// <summary>
        /// Opens the filters.
        /// </summary>
        private async Task OpenFilters()
        {
            var filtersMainViewModel = MainViewModel.GetInstance();
            filtersMainViewModel.CalendarFilters = new CalendarFiltersViewModel(this);
            await Application.Current.MainPage.Navigation.PushModalAsync(new CalendarFiltersPage());
        }

        /// <summary>
        /// Instructionses the list.
        /// </summary>
        private async Task InstructionsList()
        {
            var instructionMainViewModel = MainViewModel.GetInstance();
            instructionMainViewModel.Instructions = new InstructionsViewModel();
            await App.Navigator.PushAsync(new InstructionsPage(), true);
            return;
        }

        /// <summary>
        /// Firsts the button.
        /// </summary>
        private async Task SetCalendarViewMode()
        {
            var source = await Application.Current.MainPage.DisplayActionSheet(
                Languages.CalendarViewModeText,
                Languages.Cancel,
                null,
                Languages.DailyView,
                Languages.MultiDayView,
                Languages.MonthlyView,
                Languages.YearView);
            if (source == Languages.DailyView) CalendarView = CalendarViewMode.Day;
            if (source == Languages.MultiDayView) CalendarView = CalendarViewMode.MultiDay;
            if (source == Languages.MonthlyView) CalendarView = CalendarViewMode.Month;
            if (source == Languages.YearView) CalendarView = CalendarViewMode.Year;
            this.IsRunning = true;
            Settings.CurrentCalendarViewMode = (int)CalendarView;
            this.IsRunning = false;
            return;
        }

        /// <summary>
        /// Backs to main page.
        /// </summary>
        private async Task BackToMainPage()
        {
            await App.Navigator.PopAsync();
            return;
        }

        /// <summary>
        /// Tapped Appointments
        /// </summary>
        /// <param name="context">Context.</param>
        private async Task AppointmentTapped(AppointmentTapCommandContext context)
        {
            Event eventSelected = (Event)context.Appointment;
            if (eventSelected.IsVisible)
            {
                var appViewModel = MainViewModel.GetInstance();
                if (!eventSelected.IsTask)
                {
                    switch (eventSelected.ModuleType)
                    {
                        case '4':
                            appViewModel.Appointment = new AppointmentViewModel(eventSelected);
                            await App.Navigator.PushAsync(new AppointmentPage());
                            break;
                        case '7':
                            IsRunning = true;
                            ManagementCenterEvent eventCg = await GetEventAsync(eventSelected);
                            eventCg.Id = eventSelected.Id;
                            eventCg.CalendarColor = eventSelected.Color;
                            eventCg.TypeColor = eventSelected.TypeColor;
                            appViewModel.EventCg = new EventCgViewModel(eventCg);
                            IsRunning = false;
                            await App.Navigator.PushAsync(new EventCgPage() { Title = SelectedDate.Value.ToString("dd MMM yyyy")});
                            break;
                        default:
                            break;
                    }
                }
                else
                {
                    appViewModel.Task = new TaskViewModel(eventSelected);
                    await App.Navigator.PushAsync(new TaskPage());
                }
                this.SelectedDate = Settings.SelectedDate;
            }
        }

        /// <summary>
        /// Gets the event async.
        /// </summary>
        /// <returns>The event async.</returns>
        /// <param name="eventSelected">Event selected.</param>
        private async Task<ManagementCenterEvent> GetEventAsync(Event eventSelected)
        {
            var connection = await this.apiService.CheckConnection();
            if (!connection.IsSuccess)
            {
                IsRunning = false;
                await Application.Current.MainPage.DisplayAlert(
                    Languages.Error,
                    connection.Message,
                    Languages.Cancel);
                await Application.Current.MainPage.Navigation.PopAsync();
                return null;
            }
            var response = await this.apiService.Get<ManagementCenterEvent>(
                Settings.UrlBaseApiSigob,
                App.PrefixApiSigob,
                string.Format(this.apiEventCgController, eventSelected.Owner, eventSelected.Id),
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
                return null;
            }
            return (ManagementCenterEvent)response.Result;
        }


        /// <summary>
        /// Go Selected Date Today
        /// </summary>
        private void GoToday()
        {
            //Settings.SelectedDate = DateTime.Today;
            this.SelectedDate = DateTime.Today;
            this.DisplayDate = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day);
            Settings.SelectedDate = this.DisplayDate;
        }

        /// <summary>
        /// Happens when calendar cell is tapped.
        /// </summary>
        private async Task CellDateTapped(CalendarDayCell cell)
        {
            if (this.CalendarView == CalendarViewMode.Month || this.CalendarView == CalendarViewMode.Year)
            {
                CalendarView = CalendarViewMode.Day;
                this.SelectedDate = cell.Date;
                this.DisplayDate = cell.Date;
                await Task.Delay(10);
                Settings.CurrentCalendarViewMode = (int)CalendarView;
            //    //this.DisplayDate = cell.Date;
            }
            Settings.SelectedDate = cell.Date;
        }

        /// <summary>
        /// Add new item of Magagement Center (Appointmen, Instruction or Task)
        /// </summary>
        private async Task AddItem()
        {
            try
            {
                IsRunning = true;
                var connection = await this.apiService.CheckConnection();
                if (!connection.IsSuccess)
                {
                    IsRunning = false;
                    await Application.Current.MainPage.DisplayAlert(
                        Languages.Error,
                        connection.Message,
                        Languages.Cancel);
                    return;
                }
                var response = await this.apiService.GetList<ManagementCenterNewItem>(
                    Settings.UrlBaseApiSigob,
                    App.PrefixApiSigob,
                    this.apiManagementCenterAddOptions,
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
                    return;
                }
                this.itemsToAddList = (List<ManagementCenterNewItem>)response.Result;
                List<string> options = new List<string>();
                foreach (ManagementCenterNewItem item in this.itemsToAddList)
                {
                    options.Add (item.NewItemName);
                }
                var newItem = await Application.Current.MainPage.DisplayActionSheet(
                Languages.SelectItemToAddManagementCenter,
                Languages.Cancel,
                null,
                options.ToArray());
                await this.CreateItem(newItem);
            }
            finally { IsRunning = false; }
        }

        /// <summary>
        /// Creates the item.
        /// </summary>
        /// <returns>The item.</returns>
        /// <param name="newItem">New item.</param>
        private async Task CreateItem(string newItem)
        {
            var appViewModel = MainViewModel.GetInstance();
            if (newItem == Languages.Event || newItem == Languages.Activity)
            {
                appViewModel.EditEvent = new EditEventViewModel();
                await Application.Current.MainPage.Navigation.PushModalAsync(new EditEventPage() { Title = newItem });
            }
            else if (newItem == Languages.Appointment)
            {

            }
            else if (newItem == Languages.Instruction)
            {

            }
            else if (newItem == Languages.Assignment)
            {

            }
            else if (newItem == Languages.Task)
            {

            }
        }

        /// <summary>
        /// Can execute tap in Event.
        /// </summary>
        /// <returns><c>true</c>, if execute was caned, <c>false</c> otherwise.</returns>
        private bool CanExecute(object parameter)
        {
            return !IsRunning;
        }
        #endregion

        #region Async Commands
        public ICommand AppointmentTappedCommand => new AsyncCommand<AppointmentTapCommandContext>(AppointmentTapped, CanExecute);
        public ICommand CellDateTappedCommand => new AsyncCommand<CalendarDayCell>(CellDateTapped);
        public IAsyncCommand SetCalendarViewModeCommand => new AsyncCommand(SetCalendarViewMode);
        public IAsyncCommand BackToMainPageCommand => new AsyncCommand(BackToMainPage);
        public IAsyncCommand InstructionsListCommand => new AsyncCommand(InstructionsList);
        public IAsyncCommand OpenCalendarsCommand => new AsyncCommand(OpenCalendars);
        public IAsyncCommand OpenFiltersCommand => new AsyncCommand(OpenFilters);
        public IAsyncCommand AddItemCommand => new AsyncCommand(AddItem);
        #endregion

        #region Commands
        public ICommand GoTodayCommand => new RelayCommand(GoToday);
        #endregion

    }
}
