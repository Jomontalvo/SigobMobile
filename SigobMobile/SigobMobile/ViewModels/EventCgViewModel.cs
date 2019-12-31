namespace SigobMobile.ViewModels
{
    using System;
    using System.Threading.Tasks;
    using AsyncAwaitBestPractices.MVVM;
    using Common.Services;
    using Helpers;
    using Common.Helpers;
    using Views.ManagementCenter;
    using Xamarin.Forms;
    using Common.Models;
    using ManagementCenterEvent = Models.ManagementCenterEvent;

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
        private bool isEditable;
        private string iconStatus;
        private bool hasAbstract;
        private bool hasRecord;
        private bool isAbstractVisible;
        private bool isRecordVisible;
        private bool isTasksVisible;
        private bool isDocumentsVisible;
        private bool isAnnotationsVisible;
        private string participants;
        private Color calendarColor;
        private Color typeColor;

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the local event.
        /// </summary>
        /// <value>The local event.</value>
        public ManagementCenterEvent LocalEvent { get; set; }
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

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="T:SigobMobile.ViewModels.EventCgViewModel"/> is editable.
        /// </summary>
        /// <value><c>true</c> if is editable; otherwise, <c>false</c>.</value>
        public bool IsEditable
        {
            get => this.isEditable;
            set => SetValue(ref this.isEditable, value);
        }

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
        public string DateEvent { get; set; }
        public string Interval { get; set; }
        public string Participants
        {
            get => this.participants;
            set => SetValue(ref this.participants, value);
        }
        public bool IsParticipantsVisible { get; set; }
        public string IconStatus
        {
            get => this.iconStatus;
            set => SetValue(ref this.iconStatus, value);
        }

        public bool IsAbstractVisible
        {
            get => this.isAbstractVisible;
            set => SetValue(ref this.isAbstractVisible, value);
        }

        public bool IsRecordVisible
        {
            get => this.isRecordVisible;
            set => SetValue(ref this.isRecordVisible, value);
        }

        public bool HasAbstract
        {
            get => this.hasAbstract;
            set => SetValue(ref this.hasAbstract, value);
        }

        public bool HasRecord
        {
            get => this.hasRecord;
            set => SetValue(ref this.hasRecord, value);
        }

        public bool IsTasksVisible
        {
            get => this.isTasksVisible;
            set => SetValue(ref this.isTasksVisible, value);
        }

        public bool IsDocumentsVisible
        {
            get => this.isDocumentsVisible;
            set => SetValue(ref this.isDocumentsVisible, value);
        }

        public bool IsAnnotationsVisible
        {
            get => this.isAnnotationsVisible;
            set => SetValue(ref this.isAnnotationsVisible, value);
        }

        #endregion

        #region Constructors
        public EventCgViewModel(ManagementCenterEvent eventCg)
        {
            this.IsEditable = true;
            this.LocalEvent = eventCg;
            this.apiService = new ApiService();
            this.LoadManagementCenterEventDetails();
        }
        #endregion

        #region Commands
        public IAsyncCommand EditButtonCommand => new AsyncCommand(EditButton);
        public IAsyncCommand LoadParticipantsCommand => new AsyncCommand(LoadParticipants);
        #endregion

        #region Methods

        /// <summary>
        /// Call Edit Event view.
        /// </summary>
        private async Task EditButton()
        {
            var editEventViewModel = MainViewModel.GetInstance();
            editEventViewModel.EditEvent = new EditEventViewModel(this.LocalEvent);
            await Application.Current.MainPage.Navigation.PushModalAsync(new EditEventPage());
        }

        /// <summary>
        /// Call Edit Event view.
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
        private void LoadManagementCenterEventDetails()
        {
            IsRunning = true;
            //Colors
            this.CalendarColor = (Settings.IsEventColorByCalendar) ? LocalEvent.CalendarColor : LocalEvent.TypeColor;
            this.TypeColor = (!Settings.IsEventColorByCalendar) ? LocalEvent.CalendarColor : LocalEvent.TypeColor;
            //Attributes
            IsEditable = LocalEvent.AttributeOnEvents == EventAttribute.Create ||
                         (LocalEvent.AttributeOnEvents == EventAttribute.CreateOwner && LocalEvent.ProgrammerOfficeId == Settings.OfficeCode);
            //===============
            //Event Details
            //===============
            //Event date
            DateTime localStartDate = LocalEvent.Start.DateTime.ToLocalTime();
            DateTime localEndDate = LocalEvent.End.DateTime.ToLocalTime();
            this.DateEvent = localStartDate.ToString("D");
            //Event interval
            if (LocalEvent.Start.Date == LocalEvent.End.Date)
                this.Interval = $"{localStartDate.ToString("t")} - {localEndDate.ToString("t")}";
            else
                this.Interval = $"{localStartDate.ToString("g")} - {localEndDate.ToString("g")}";
            if (LocalEvent.Tentative) this.Interval = $"{this.Interval} ({Languages.TentativeLabel})";
            //Event Management Status
            IconStatus = LocalEvent.Status switch
            {
                StatusAppointment.InManagement => "ic_ev_status_active",
                StatusAppointment.Finished => "ic_ev_status_finished",
                StatusAppointment.Suspended => "ic_ev_status_suspended",
                _ => "ic_ev_status",
            };
            //Participants
            if (LocalEvent.ParticipantsAccount <= 1)
                Participants = $"1 {Languages.EventParticipantText}";
            else
            {
                Participants = (LocalEvent.IsParticipant) ? 
                    $"{Languages.EventParticipantsText} ({Languages.Me} {Languages.And} {LocalEvent.ParticipantsAccount - 1}+)" : 
                    $"{LocalEvent.ParticipantsAccount} {Languages.EventParticipantsText}";
            }
            this.IsParticipantsVisible = !(LocalEvent.ModifyParticipants == EventParticipantsAttribute.NotAuthorized);

            //Abstract - Executive File -Record 
            this.IsAbstractVisible = !((LocalEvent.AbstractAttribute == EventAbstractOrRecordAttribute.NotAuthorized) || (LocalEvent.AbstractDocSize == 0));
            this.IsRecordVisible = !((LocalEvent.RecordAttribute == EventAbstractOrRecordAttribute.NotAuthorized) || (LocalEvent.RecordDocSize == 0));

            //Tasks
            this.IsTasksVisible = !((LocalEvent.TaskAccount == 0) ||
                (LocalEvent.PreviousTask == EventTasksAttribute.NotAuthorized && 
                 LocalEvent.SupportTasks == EventTasksAttribute.NotAuthorized && 
                 LocalEvent.LaterTasks == EventTasksAttribute.NotAuthorized)
                );

            //Documents
            this.IsDocumentsVisible = !((LocalEvent.DocumentsAttribute == EventDocumentsAttribute.NotAuthorized) || (LocalEvent.DocumentsAccount == 0));
            this.IsRunning = false;

            //Annotations
            this.IsAnnotationsVisible = !string.IsNullOrEmpty(LocalEvent.Annotations);
        }
        #endregion

    }
}
