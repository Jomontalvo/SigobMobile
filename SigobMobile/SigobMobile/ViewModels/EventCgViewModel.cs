namespace SigobMobile.ViewModels
{
    using Models;
    using Services;
    using Helpers;
    using Xamarin.Forms;
    using System;

    public class EventCgViewModel : BaseViewModel
    {
        #region Services
        internal ApiService apiService;
        #endregion

        #region ApiControllers
        internal string apiEventController = "cgcal/{0}/events/{1}";
        //internal string apiCalendarsController = "cgcal/{0}/events/{1}";
        //internal string apiEventTypeController = "cgcal/{0}/events/{1}";
        #endregion

        #region Attributes
        private bool isRunning;
        private ManagementCenterEvent eventDetails;
        private string interval;
        #endregion

        #region Properties
        /// <summary>
        /// For ActivityIndicator => Gets or sets a value indicating whether this
        /// <see cref="T:SigobMobile.ViewModels.EventManagementCenterViewModel"/> is running.
        /// </summary>
        /// <value><c>true</c> if is running; otherwise, <c>false</c>.</value>
        public bool IsRunning
        {
            get => this.isRunning;
            set => SetValue(ref this.isRunning, value);
        }

        /// <summary>
        /// Gets or sets the time interval when event is schedule.
        /// </summary>
        /// <value>The interval.</value>
        public string Interval
        {
            get => this.interval;
            set => SetValue(ref this.interval, value);
        }

        /// <summary>
        /// Property inherited from Previous Page Navigation
        /// </summary>
        /// <value>The local Appointment.</value>
        public Event LocalAppointment { get; set; }

        /// <summary>
        /// Gets or sets the event details.
        /// </summary>
        /// <value>The event details.</value>
        public ManagementCenterEvent EventCg { get; set; }
        #endregion

        #region Constructors
        public EventCgViewModel(Event appoitment)
        {
            this.LocalAppointment = appoitment;
            this.apiService = new ApiService();
            this.LoadEventDetails();
        }
        #endregion

        #region Methods

        private async void LoadEventDetails()
        {
            IsRunning = true;
            var connection = await apiService.CheckConnection();
            if (!connection.IsSuccess)
            {
                IsRunning = false;
                await Application.Current.MainPage.DisplayAlert(
                    Languages.Error,
                    connection.Message,
                    Languages.Cancel);
                await Application.Current.MainPage.Navigation.PopToRootAsync();
                return;
            }
            var response = await this.apiService.Get<ManagementCenterEvent>(
                Settings.UrlBaseApiSigob,
                App.PrefixApiSigob,
                string.Format(this.apiEventController, LocalAppointment.Owner, LocalAppointment.Id),
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
            this.eventDetails = (ManagementCenterEvent)response.Result;
            this.EventCg = eventDetails;
            this.Interval = DateTime.Now.ToString("t");
            this.IsRunning = false;
        }
        #endregion

    }
}
