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
    using SigobMobile.Interfaces;
    using System.IO;
    using System.Windows.Input;
    using GalaSoft.MvvmLight.Command;
    using SigobMobile.Views.Common;
    using SigobMobile.Views.Tasks;

    public class EventCgViewModel : BaseViewModel
    {
        #region Services
        internal ApiService apiService;
        private readonly IFileViewerService fileViewerService;
        #endregion

        #region ApiControllers
        internal string apiEventController = "cgcal/{0}/events/{1}";
        internal string apiAbstractController = "cg/events/{0}/abstract";
        internal string apiRecordController = "cg/events/{0}/record";
        internal string apiDeleteController = "cg/{0}/events";
        internal string apiStatusController = "events/{0}/status/{1}";
        //internal string apiEventTypeController = "cgcal/{0}/events/{1}";
#endregion

#region Attributes
private bool isRunning;
        private bool isEnabled;
        private bool isEditable;
        private string iconStatus;
        private string iconPrivacy;
        private bool hasAbstract;
        private bool hasRecord;
        private bool isSummaryVisible;
        private bool isLocationVisible;
        private bool isAbstractVisible;
        private bool isRecordVisible;
        private bool isTasksVisible;
        private bool isDocumentsVisible;
        private bool isAnnotationsVisible;
        private string participants;
        private Color calendarColor;
        private Color typeColor;
        private DownloadDocument docDownloaded; 
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
        public bool IsLocationVisible
        {
            get => this.isLocationVisible;
            set => SetValue(ref this.isLocationVisible, value);
        }
        public bool IsSummaryVisible
        {
            get => this.isSummaryVisible;
            set => SetValue(ref this.isSummaryVisible, value);
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

        public bool IsEnabled
        {
            get => this.isEnabled;
            set => SetValue(ref this.isEnabled, value);
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

        public string IconPrivacy
        {
            get => this.iconPrivacy;
            set => SetValue(ref this.iconPrivacy, value);
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
            this.fileViewerService = DependencyService.Get<IFileViewerService>();
            this.IsEditable = true;
            this.LocalEvent = eventCg;
            this.apiService = new ApiService();
            this.LoadManagementCenterEventDetails();
            this.IsEnabled = true;
        }
        #endregion

        #region Commands
        public IAsyncCommand EditButtonCommand => new AsyncCommand(EditButton);
        public IAsyncCommand LoadParticipantsCommand => new AsyncCommand(LoadParticipants);
        public IAsyncCommand LoadExecutiveAbstractCommand => new AsyncCommand(LoadExecutiveAbstract);
        public IAsyncCommand LoadRecordCommand => new AsyncCommand(LoadRecord);
        public IAsyncCommand GetTasksCommand => new AsyncCommand(GetTasks);
        public IAsyncCommand GetDocumentsCommand => new AsyncCommand(GetDocuments);
        public IAsyncCommand DeleteEventCommand => new AsyncCommand(DeleteEvent);
        public IAsyncCommand<int> ChangeStatusCommand => new AsyncCommand<int>(ChangeStatusEvent);
        #endregion

        #region Methods

        /// <summary>
        /// Get Event Tasks related 
        /// </summary>
        /// <returns></returns>
        private async Task GetTasks()
        {
            var mainViewModel = MainViewModel.GetInstance();
            mainViewModel.EventTasks = new EventTasksViewModel(
                LocalEvent.ManagementCenterId,
                LocalEvent.Id, EventTasksAttribute.ViewOnlyAssigned, EventTaskMoment.All);
            await App.Navigator.PushAsync(new EventTasksPage());
        }

        /// <summary>
        /// Get Event Attachments
        /// </summary>
        /// <returns></returns>
        private async Task GetDocuments()
        {
            var mainViewModel = MainViewModel.GetInstance();
            mainViewModel.Attachments = new AttachmentsViewModel(
                LocalEvent.Id,
                SigobInstrument.ManagementCenterEvent,
                DocumentSource.MgCenterEvent);
            await App.Navigator.PushAsync(new  AttachmentsPage());
        }


        /// <summary>
        /// Chnage status management of event
        /// </summary>
        /// <returns></returns>
        private async Task ChangeStatusEvent(int status)
        {
            var title = (StatusAppointment)status switch
            {
                StatusAppointment.InManagement => Languages.EventActivateCommand,
                StatusAppointment.Suspended => Languages.EventSuspendCommand,
                StatusAppointment.Finished => Languages.EventFinishCommand,
                _ => Languages.EventResumeMessage
            };
            var message = (StatusAppointment)status switch
            {
                StatusAppointment.InManagement => Languages.EventResumeMessage,
                StatusAppointment.Suspended => Languages.EventSuspendMessage,
                StatusAppointment.Finished => Languages.EventFinishMessage,
                _ => Languages.EventResumeMessage
            };
            var confirm = await App.Navigator.DisplayAlert(
                title,
                message,
                Languages.Yes,
                Languages.No);
            if (!confirm) return;

            this.IsRunning = true;
            this.IsEnabled = false;

            // 1. Verify connection
            var connection = await this.apiService.CheckConnection();
            if (!connection.IsSuccess)
            {
                this.IsRunning = false;
                await App.Navigator.CurrentPage.DisplayAlert(
                    Languages.Error,
                    connection.Message,
                    Languages.Cancel);
                return;
            }

            // 2. Try change status event
            var response = await this.apiService.Put<TransactionResponse>(
                    Settings.UrlBaseApiSigob,
                    App.PrefixApiSigob,
                    string.Format(apiStatusController, LocalEvent.Id, status),
                    Settings.Token,
                    Settings.DbToken,
                    LocalEvent.Id);
            this.IsRunning = false;
            this.IsEnabled = true;

            if (!response.IsSuccess)
            {
                await App.Navigator.DisplayAlert(
                    "Error",
                    response.Message,
                    "Accept");
                return;
            }
            MainViewModel.GetInstance().Calendar.LoadAppointments(LocalEvent.Start);
            await App.Navigator.PopAsync();
        }

        /// <summary>
        /// Download File of Event
        /// </summary>
        /// <param name="apiController"></param>
        /// <returns></returns>
        private async Task DowloadEventFileAsync(string apiController)
        {
            this.IsRunning = true;
            try
            {
                // 1. Verify connection
                var connection = await this.apiService.CheckConnection();
                if (!connection.IsSuccess)
                {
                    this.IsRunning = false;
                    await App.Navigator.CurrentPage.DisplayAlert(
                        Languages.Error,
                        connection.Message,
                        Languages.Cancel);
                    return;
                }

                // 2. Get the menu list from API
                var response = await this.apiService.Get<DownloadDocument>(
                        Settings.UrlBaseApiSigob,
                        App.PrefixApiSigob,
                        apiController,
                        Settings.Token,
                        Settings.DbToken
                    );
                if (!response.IsSuccess)
                {
                    this.IsRunning = false;
                    await App.Navigator.CurrentPage.DisplayAlert(
                        title: Languages.Error,
                        message: response.Message,
                        cancel: Languages.Cancel);
                    return;
                }

                //Get download file values
                this.docDownloaded = (DownloadDocument)response.Result;
            }
            catch (Exception ex)
            {
                await App.Navigator.CurrentPage.DisplayAlert(
                    Languages.Error,
                    $"{Languages.GeneralError}: {ex.Message}",
                    Languages.Cancel);
            }
            finally
            {
                IsRunning = false;
            }
        }

        /// <summary>
        /// Get Executive Abstract of Management Center Event
        /// </summary>
        /// <returns></returns>
        private async Task LoadExecutiveAbstract()
        {
            string apiController = string.Format(this.apiAbstractController, LocalEvent.Id);
            await DowloadEventFileAsync(apiController);
            var bytes = Convert.FromBase64String(this.docDownloaded.Document);
            var stream = new MemoryStream(bytes);
            stream.Seek(0, SeekOrigin.Begin);
            await this.fileViewerService.View(stream,
                $"{Languages.EventAbstractText.Replace(" ",string.Empty)}" +
                $"-{LocalEvent.Id}.{docDownloaded.Extension}");
        }

        /// <summary>
        /// Get Record of Finished Management Center Event
        /// </summary>
        /// <returns></returns>
        private async Task LoadRecord()
        {
            string apiController = string.Format(this.apiRecordController, LocalEvent.Id);
            await DowloadEventFileAsync(apiController);
            var bytes = Convert.FromBase64String(this.docDownloaded.Document);
            var stream = new MemoryStream(bytes);
            stream.Seek(0, SeekOrigin.Begin);
            await this.fileViewerService.View(stream,
                $"{LocalEvent.Id}.{docDownloaded.Extension}");
        }

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
        /// Delete Event
        /// </summary>
        /// <returns></returns>
        private async Task DeleteEvent()
        {
            var confirm = await App.Navigator.DisplayAlert(
                Languages.Confirm,
                Languages.ConfirmEventDeletion,
                Languages.Delete,
                Languages.No);
            if (!confirm) return;

            this.IsRunning = true;
            this.IsEnabled = false;

            // 1. Verify connection
            var connection = await this.apiService.CheckConnection();
            if (!connection.IsSuccess)
            {
                this.IsRunning = false;
                await App.Navigator.CurrentPage.DisplayAlert(
                    Languages.Error,
                    connection.Message,
                    Languages.Cancel);
                return;
            }

            // 2. Try delete event
            var response = await this.apiService.DeleteAsync(
                    Settings.UrlBaseApiSigob,
                    App.PrefixApiSigob,
                    string.Format(apiDeleteController,LocalEvent.ManagementCenterId),
                    Settings.Token,
                    Settings.DbToken,
                    LocalEvent.Id);
            this.IsRunning = false;
            this.IsEnabled = true;

            if (!response.IsSuccess)
            {
                await App.Navigator.DisplayAlert(
                    "Error",
                    response.Message,
                    "Accept");
                return;
            }
            MainViewModel.GetInstance().Calendar.DeleteEventInList(this.LocalEvent.Id);
            await App.Navigator.PopAsync();
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
            DateTime localStartDate = LocalEvent.Start.ToLocalTime();
            DateTime localEndDate = LocalEvent.End.ToLocalTime();
            this.DateEvent = localStartDate.ToString("D");
            //Event interval
            if (LocalEvent.Start.Date == LocalEvent.End.Date)
                this.Interval = $"{localStartDate.ToString("t")} - {localEndDate.ToString("t")}";
            else
                this.Interval = $"{localStartDate.ToString("g")} - {localEndDate.ToString("g")}";
            if (LocalEvent.Tentative) this.Interval = $"{this.Interval} ({Languages.TentativeLabel})";
            //Event Location
            IsLocationVisible = !string.IsNullOrEmpty(LocalEvent.Location);
            //Event Management Status
            IconStatus = LocalEvent.Status switch
            {
                StatusAppointment.InManagement => "ic_ev_status_active",
                StatusAppointment.Finished => "ic_ev_status_finished",
                StatusAppointment.Suspended => "ic_ev_status_suspended",
                _ => "ic_ev_status",
            };
            //Even Security
            IconPrivacy = LocalEvent.PrivacyLevel switch
            {
                SecurityLevelEvent.Public => "ic_doc_privacy_public",
                SecurityLevelEvent.Low => "ic_doc_privacy_ordinary",
                SecurityLevelEvent.Medium => "ic_ev_security",
                SecurityLevelEvent.High => "ic_doc_privacy_reserved",
                SecurityLevelEvent.Private => "ic_ev_privacy_private",
                _ => "ic_ev_security",
            };
            //Event Summary
            IsSummaryVisible = !string.IsNullOrEmpty(LocalEvent.Summary);
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
