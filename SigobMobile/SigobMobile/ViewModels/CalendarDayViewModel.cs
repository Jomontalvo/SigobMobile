﻿namespace SigobMobile.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Windows.Input;
    using GalaSoft.MvvmLight.Command;
    using Models;
    using Services;
    using SigobMobile.Helpers;
    using Telerik.XamarinForms.Input;
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
            get { return this.selectedDate; }
            set { SetValue(ref this.selectedDate, value); }
        }
        #endregion

        #region Constructors
        public CalendarDayViewModel()
        {
            selectedDate = DateTime.Today;
            apiService = new ApiService();
            this.LoadAppointments(selectedDate.GetValueOrDefault());
        }
        #endregion

        #region Methods
        private async void LoadAppointments(DateTime date)
        {
            DateTime pivot = (date == default(DateTime)) ? DateTime.Today : date;
            this.isRunning = true;
            startDate = pivot.AddDays(-30).ToString("yyyyMMdd");
            endDate = pivot.AddDays(30).ToString("yyyyMMdd");
            var connection = await this.apiService.CheckConnection();
            if (!connection.IsSuccess)
            {
                isRunning = false;
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
                this.isRunning = false;
                await Application.Current.MainPage.DisplayAlert(
                    Languages.Error,
                    response.Message,
                    Languages.Cancel);                    
                return;
            }
            this.eventList = (List<AppointmentItem>)response.Result;
            this.Events = new ObservableCollection<Event>(ToCalendarEvents());
            this.isRunning = false;
        }

        private IEnumerable<Event> ToCalendarEvents()
        {
            return this.eventList.Select(l => new Event
            {
                Id = l.Id,
                Color = (l.IsTask) ? Color.FromRgb(l.RedColorType,l.GreenColorType,l.BlueColorType) : Color.FromRgb(l.RedColorItem, l.GreenColorItem, l.BlueColorItem),
                Detail = l.Place,
                StartDate = (l.Id == 0 || l.IsTask) ? l.End.DateTime.ToLocalTime().Date : l.Start.DateTime.ToLocalTime(), // IsHoliday or Task Control 
                EndDate = (l.Id == 0 || l.IsTask) ? l.End.DateTime.ToLocalTime().Date.AddMinutes(1) : l.End.DateTime.ToLocalTime(),
                IsAllDay = (l.IsTask || l.Id == 0),
                IsLocked = (l.IsVisible == 1),
                IsTentative = l.IsTentative,
                Owner = l.AgendaOwner,
                Programmer = l.ProgrammerAgenda,
                Title = l.Description,
            });
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

        private async void OpenCalendars()
        {
            var calendarsMainViewModel = MainViewModel.GetInstance();
            calendarsMainViewModel.Calendars = new CalendarsViewModel();
            await Application.Current.MainPage.Navigation.PushModalAsync(new NavigationPage(new CalendarsPage()));
        }

        private void OpenFilters()
        {
            var filtersMainViewModel = MainViewModel.GetInstance();
            filtersMainViewModel.CalendarFilters = new CalendarFiltersViewModel();
            Application.Current.MainPage.Navigation.PushModalAsync(new CalendarFiltersPage());
        }

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
            return;
        }
        #endregion

        #region Commands

        public ICommand SetCalendarViewModeCommand
        {
            get
            {
                return new RelayCommand(SetCalendarViewMode);
            }
        }



        public ICommand InstructionsListCommand
        {
            get
            {
                return new RelayCommand(InstructionsList);
            }
        }
        public ICommand OpenCalendarsCommand
        {
            get
            {
                return new RelayCommand(OpenCalendars);
            }
        }

        public ICommand OpenFiltersCommand
        {
            get
            {
                return new RelayCommand(OpenFilters);
            }
        }
        #endregion

    }
}
