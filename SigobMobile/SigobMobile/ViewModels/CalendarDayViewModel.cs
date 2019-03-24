﻿namespace SigobMobile.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Windows.Input;
    using GalaSoft.MvvmLight.Command;
    using Models;
    using Services;
    using SigobMobile.Helpers;
    using SigobMobile.Views.Tasks;
    using Telerik.XamarinForms.Input;
    using Telerik.XamarinForms.Input.Calendar.Commands;
    using Views.ManagementCenter;
    using Xamarin.Forms;

    public class CalendarDayViewModel : BaseViewModel
    {
        #region Services
        internal ApiService apiService;
        #endregion

        #region ApiControllers
        internal string apiEventsController = "events/start/{0}/end/{1}/viewattempts/{2}";
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
                this.DisplayDate = this.selectedDate.GetValueOrDefault();
            }
        }
        public DateTime DisplayDate
        {
            get { return this.displayDate; }
            set { SetValue(ref this.displayDate, value); }
        }
    #endregion

    #region Constructors
    public CalendarDayViewModel()
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
        public async void LoadAppointments(DateTime date)
        {
            await UpdateAppointments(date);
        }

        /// <summary>
        /// Update appointments Async Task.
        /// </summary>
        /// <returns>The appointments.</returns>
        /// <param name="date">Date.</param>
        public async Task UpdateAppointments(DateTime date)
        {
            DateTime pivot = (date == default(DateTime)) ? DateTime.Today : date;
            this.IsRunning = true;
            startDate = pivot.AddYears(-3).ToString("yyyyMMdd");
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
                string.Format(this.apiEventsController,startDate, endDate, viewTentative),
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
                Color = l.IsTask ? Color.FromRgb(l.RedColorType, l.GreenColorType, l.BlueColorType) : (l.Status != StatusAppointment.Suspended) ? Color.FromRgb(l.RedColorItem, l.GreenColorItem, l.BlueColorItem) : (Color)Application.Current.Resources["grayBorder"],
                Detail = l.Place,
                StartDate = (l.Id == 0 || l.IsTask) ? l.End.DateTime.ToLocalTime().Date : l.Start.DateTime.ToLocalTime(), // IsHoliday or Task Control 
                EndDate = (l.Id == 0 || l.IsTask) ? l.End.DateTime.ToLocalTime().Date.AddMinutes(1) : l.End.DateTime.ToLocalTime(),
                IsAllDay = l.IsTask || l.Id == 0,
                IsLocked = l.IsVisible == 1,
                IsTentative = l.IsTentative,
                Owner = l.AgendaOwner,
                Programmer = l.ProgrammerAgenda,
                Title = (l.Status == StatusAppointment.Suspended) ? $"[{Languages.SuspendedStatus}] {l.Description}" : l.IsTentative ? $"{l.Description} ({Languages.TentativeLabel})" : l.Description,
                TypeColor = Color.FromRgb(l.RedColorType, l.GreenColorType, l.BlueColorType),
                ModuleType = l.ModuleType,
                IsHighlighted = l.IsHighlighted,
                IsTask = l.IsTask,
                IsVisible = (l.IsVisible == 1),
                SecurityLevel = l.SecurityLevel,
                TypeId = l.TypeId,
                Status = l.Status
            }).Where(l =>  (Settings.IsVisibleManagementStatus && (l.Status == StatusAppointment.InManagement))
                        || (Settings.IsVisibleCompletedStatus && (l.Status == StatusAppointment.Finished))
                        || (Settings.IsVisibleSuspendStatus && (l.Status == StatusAppointment.Suspended)));
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
        private async void OpenCalendars()
        {
            var calendarsMainViewModel = MainViewModel.GetInstance();
            calendarsMainViewModel.Calendars = new CalendarsViewModel(this);
            await Application.Current.MainPage.Navigation.PushModalAsync(new NavigationPage(new CalendarsPage()));
        }

        /// <summary>
        /// Opens the filters.
        /// </summary>
        private async void OpenFilters()
        {
            var filtersMainViewModel = MainViewModel.GetInstance();
            filtersMainViewModel.CalendarFilters = new CalendarFiltersViewModel(this);
            await Application.Current.MainPage.Navigation.PushModalAsync(new CalendarFiltersPage());
        }

        /// <summary>
        /// Instructionses the list.
        /// </summary>
        private async void InstructionsList()
        {
            var instructionMainViewModel = MainViewModel.GetInstance();
            instructionMainViewModel.Instructions = new InstructionsViewModel();
            await App.Navigator.PushAsync(new InstructionsPage(),true);
            return;
        }

        /// <summary>
        /// Firsts the button.
        /// </summary>
        private async void SetCalendarViewMode()
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
        private async void BackToMainPage()
        {
            await App.Navigator.PopAsync();
            return;
        }

        /// <summary>
        /// Tapped Appointments
        /// </summary>
        /// <param name="context">Context.</param>
        private async void AppointmentTapped(AppointmentTapCommandContext context)
        {
            Event eventSelected = (Event)context.Appointment;
            var appViewModel = MainViewModel.GetInstance();
            if (!eventSelected.IsTask)
            {
                switch (eventSelected.ModuleType)
                {
                    case 4:
                        appViewModel.EventAgenda = new EventAgendaViewModel(eventSelected);
                        await App.Navigator.PushAsync(new EventAgendaPage());
                        break;
                    case 7:
                        appViewModel.EventCg = new EventCgViewModel(eventSelected);
                        await App.Navigator.PushAsync(new EventCgPage());
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
        }

        /// <summary>
        /// Go Selected Date Today
        /// </summary>
        private void GoToday()
        {
            this.SelectedDate = DateTime.Today;
            this.DisplayDate = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day);
            Settings.SelectedDate = this.DisplayDate;
        }

        /// <summary>
        /// Happens when calendar cell is tapped.
        /// </summary>
        private async void CellDateTapped(CalendarDayCell cell)
        {
            Settings.SelectedDate = cell.Date;
            this.SelectedDate = cell.Date;
            if (this.CalendarView == CalendarViewMode.Month || this.CalendarView == CalendarViewMode.Year)
            {
                CalendarView = CalendarViewMode.Day;
                await Task.Delay(50);
                Settings.CurrentCalendarViewMode = (int)CalendarView;
                this.DisplayDate = cell.Date;
            }
        }

        #endregion

        #region Commands

        public ICommand AppointmentTappedCommand => new RelayCommand<AppointmentTapCommandContext>(AppointmentTapped);

        public ICommand CellDateTappedCommand => new RelayCommand<CalendarDayCell>(CellDateTapped);

        public ICommand GoTodayCommand => new RelayCommand(GoToday);

        public ICommand SetCalendarViewModeCommand => new RelayCommand(SetCalendarViewMode);

        public ICommand BackToMainPageCommand => new RelayCommand(BackToMainPage);

        public ICommand InstructionsListCommand => new RelayCommand(InstructionsList);

        public ICommand OpenCalendarsCommand => new RelayCommand(OpenCalendars);

        public ICommand OpenFiltersCommand => new RelayCommand(OpenFilters);

        #endregion

    }
}
