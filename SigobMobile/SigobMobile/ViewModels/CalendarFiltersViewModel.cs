namespace SigobMobile.ViewModels
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Threading.Tasks;
    using AsyncAwaitBestPractices.MVVM;
    using Common.Helpers;
    using Common.Models;
    using Helpers;
    using Models;
    using Xamarin.Forms;

    public class CalendarFiltersViewModel : BaseViewModel
    {
        #region Attributes
        private ObservableCollection<Event> eventItems = new ObservableCollection<Event>();
        private List<string> filterItems = new List<string>
        {
            Languages.OnlyConfirmed,
            Languages.OnlyTentative,
            Languages.ConfirmedAndTentative
        };
        private string selectedFilter;
        private int filterSelectedIndex;
        private byte viewTentative;
        private CalendarViewModel calendarViewModel;

        private bool isVisibleInManagementStatus;
        private bool isVisibleCompletedStatus;
        private bool isVisibleSuspendedStatus;
        private bool isEventColorByCalendar;

        #endregion

        #region Properties
        public bool IsVisibleInManagementStatus
        {
            get => this.isVisibleInManagementStatus;
            set => SetValue(ref this.isVisibleInManagementStatus, value);
        }
        public bool IsVisibleCompletedStatus
        {
            get => this.isVisibleCompletedStatus;
            set => SetValue(ref this.isVisibleCompletedStatus, value);
        }
        public bool IsVisibleSuspendedStatus
        {
            get => this.isVisibleSuspendedStatus;
            set => SetValue(ref this.isVisibleSuspendedStatus, value);
        }
        public bool IsEventColorByCalendar
        {
            get => this.isEventColorByCalendar;
            set => SetValue(ref this.isEventColorByCalendar, value);
        }
        public List<string> ConfirmedFilterItems
        {
            get => this.filterItems;
            set => SetValue(ref this.filterItems, value);
        }

        public string SelectedFilter
        { 
            get => this.selectedFilter;
            set => SetValue(ref this.selectedFilter,value); 
        }

        public int FilterSelectedIndex
        {
            get => this.filterSelectedIndex;
            set
            {
                if (filterSelectedIndex != value)
                {
                    SetValue(ref this.filterSelectedIndex, value);
                    // Trigger some action to take such as updating other labels or fields
                    SelectedFilter = ConfirmedFilterItems[filterSelectedIndex];
                }
            }
        }
        #endregion

        #region Constructors
        public CalendarFiltersViewModel(CalendarViewModel sourceCalendarViewModel)
        {
            this.IsVisibleInManagementStatus = Settings.IsVisibleManagementStatus;
            this.IsVisibleCompletedStatus = Settings.IsVisibleCompletedStatus;
            this.IsVisibleSuspendedStatus = Settings.IsVisibleSuspendStatus;
            this.IsEventColorByCalendar = Settings.IsEventColorByCalendar;
            this.calendarViewModel = sourceCalendarViewModel;
            this.viewTentative = this.calendarViewModel.ViewTentative;
            this.FilterSelectedIndex = this.viewTentative;
        }
        #endregion

        #region Commands
        public IAsyncCommand CancelFiltersCommand => new AsyncCommand(CancelFilters);
        public IAsyncCommand SaveFiltersCommand => new AsyncCommand(SaveFilters);
        #endregion

        #region Methods
        /// <summary>
        /// Close filter modal view
        /// </summary>
        private async Task CancelFilters()
        {
            await Application.Current.MainPage.Navigation.PopModalAsync(); 
        }

        /// <summary>
        /// Save the filter options.
        /// </summary>
        private async Task SaveFilters()
        {
            calendarViewModel.ViewTentative = (byte)FilterSelectedIndex;
            if ((this.viewTentative != this.FilterSelectedIndex) || 
                (Settings.IsVisibleManagementStatus != IsVisibleInManagementStatus) || 
                (Settings.IsVisibleCompletedStatus != IsVisibleCompletedStatus) ||
                (Settings.IsVisibleSuspendStatus != IsVisibleSuspendedStatus) ||
                (Settings.IsEventColorByCalendar != IsEventColorByCalendar))
            {
                calendarViewModel.LoadAppointments(calendarViewModel.SelectedDate.GetValueOrDefault());
            }
            Settings.IsVisibleManagementStatus = this.IsVisibleInManagementStatus;
            Settings.IsVisibleCompletedStatus = this.IsVisibleCompletedStatus;
            Settings.IsVisibleSuspendStatus = this.IsVisibleSuspendedStatus;
            Settings.IsEventColorByCalendar = this.IsEventColorByCalendar;

            //Rebuild Observable Collection
            eventItems = new ObservableCollection<Event>(calendarViewModel.Events.
                    Where((appointment) => (IsVisibleInManagementStatus && (appointment.Status == StatusAppointment.InManagement))
                                        || (IsVisibleCompletedStatus && (appointment.Status == StatusAppointment.Finished)) 
                                        || (IsVisibleSuspendedStatus && appointment.Status == StatusAppointment.Suspended)));
            this.calendarViewModel.Events = eventItems;
            await Application.Current.MainPage.Navigation.PopModalAsync();
        }
        #endregion
    }
}
