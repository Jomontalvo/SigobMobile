namespace SigobMobile.ViewModels
{
    using Common.Helpers;
    using Xamarin.Forms;
    using System;
    using Helpers;
    using Common.Models;
    using AgendaEvent = Models.AgendaEvent;
    using AsyncAwaitBestPractices.MVVM;
    using System.Threading.Tasks;
    using SigobMobile.Views.ManagementCenter;

    public class AppointmentViewModel : BaseViewModel
    {
        #region Attributes
        private bool isRunning;
        private bool isEnabled;
        private bool isEditable;
        private bool isSummaryVisible;
        private bool isCommentVisible;
        private bool isLocationVisible;
        private string iconStatus;
        private string iconPrivacy;
        private string participants;
        private Color calendarColor;
        private Color typeColor;
        #endregion

        #region Properties
        public AgendaEvent LocalAppointment { get; set; }

        public Color CalendarColor
        {
            get => this.calendarColor;
            set => SetValue(ref this.calendarColor, value);
        }
        public Color TypeColor
        {
            get => this.typeColor;
            set => SetValue(ref this.typeColor, value);
        }

        public bool IsRunning
        {
            get => this.isRunning;
            set => SetValue(ref this.isRunning, value);
        }

        public bool IsEnabled
        {
            get => this.isEnabled;
            set => SetValue(ref this.isEnabled, value);
        }

        public bool IsEditable
        {
            get => this.isEditable;
            set => SetValue(ref this.isEditable, value);
        }

        public string DateEvent { get; set; }

        public string Interval { get; set; }

        public string Participants
        {
            get => this.participants;
            set => SetValue(ref this.participants, value);
        }

        public string IconStatus
        {
            get => this.iconStatus;
            set => SetValue(ref this.iconStatus, value);
        }

        public string IconPrivacy
        {
            get => this.iconPrivacy;
            set => SetValue(ref this.iconPrivacy, value);
        }

        public bool IsSummaryVisible
        {
            get => this.isSummaryVisible;
            set => SetValue(ref this.isSummaryVisible, value);
        }
        
        public bool IsCommentVisible
        {
            get => this.isCommentVisible;
            set => SetValue(ref this.isCommentVisible, value);
        }

        public bool IsLocationVisible
        {
            get => this.isLocationVisible;
            set => SetValue(ref this.isLocationVisible, value);
        }
        #endregion

        #region Constructors
        public AppointmentViewModel(AgendaEvent appointment)
        {
            this.LocalAppointment = appointment;
            this.LoadAppointmentDetails();
        }
        #endregion

        #region Commands
        public IAsyncCommand LoadParticipantsCommand => new AsyncCommand(LoadParticipants);
        #endregion

        #region Methods
        // <summary>
        /// Load appointment participants view.
        /// </summary>
        private async Task LoadParticipants()
        {
            var participantsViewModel = MainViewModel.GetInstance();
            participantsViewModel.Participants = new ParticipantsViewModel(this);
            await App.Navigator.Navigation.PushModalAsync(new NavigationPage(new ParticipantsPage()));
        }

        /// <summary>
        /// Load the event details.
        /// </summary>
        private void LoadAppointmentDetails()
        {
            IsRunning = true;
            //Colors
            this.CalendarColor = (!Settings.IsEventColorByCalendar) ? LocalAppointment.CalendarColor : LocalAppointment.TypeColor;
            //Attributes
            IsEditable = (LocalAppointment.OwnerOfficeId == Settings.OfficeCode);
            //===============
            //Event Details
            //===============
            //Event date
            DateTime localStartDate = LocalAppointment.Start.ToLocalTime();
            DateTime localEndDate = LocalAppointment.End.ToLocalTime();
            this.DateEvent = localStartDate.ToString("D");
            //Event interval
            if (LocalAppointment.Start.Date == LocalAppointment.End.Date)
                this.Interval = $"{localStartDate.ToString("t")} - {localEndDate.ToString("t")}";
            else
                this.Interval = $"{localStartDate.ToString("g")} - {localEndDate.ToString("g")}";
            if (LocalAppointment.IsTentative) this.Interval = $"{this.Interval} ({Languages.TentativeLabel})";
            //Event Location
            IsLocationVisible = !string.IsNullOrEmpty(LocalAppointment.Location); 
            //Event Management Status
            IconStatus = LocalAppointment.Status switch
            {
                StatusAppointment.InManagement => "ic_ev_status_active",
                StatusAppointment.Finished => "ic_ev_status_finished",
                StatusAppointment.Suspended => "ic_ev_status_suspended",
                _ => "ic_ev_status",
            };
            //Even Security
            IconPrivacy = LocalAppointment.PrivacyLevel switch
            {
                SecurityLevelEvent.Public => "ic_doc_privacy_public",
                SecurityLevelEvent.Low => "ic_doc_privacy_ordinary",
                SecurityLevelEvent.Medium => "ic_ev_security",
                SecurityLevelEvent.High => "ic_doc_privacy_reserved",
                SecurityLevelEvent.Private => "ic_ev_privacy_private",
                _ => "ic_ev_security",
            };
            //Appointment Summary
            IsSummaryVisible = !string.IsNullOrEmpty(LocalAppointment.Summary);
            //Appointment Comment
            IsCommentVisible = !string.IsNullOrEmpty(LocalAppointment.Comment);
            //Participants
            Participants = $"{Languages.EventParticipantsText}";

            this.IsRunning = false;
        }

        #endregion
    }
}
