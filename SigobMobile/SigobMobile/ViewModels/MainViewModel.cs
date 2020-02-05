namespace SigobMobile.ViewModels
{
    /// <summary>
    /// Main view model.
    /// </summary>
    public class MainViewModel
    {
        #region Properties
        public string Token { get; set; }
        public string DbToken { get; set; }
        #endregion

        #region ViewModels
        #region Login Views
        public LoginViewModel Login { get; set; }
        public InstitutionsConnectViewModel InstitutionsConnect { get; set; }
        #endregion

        #region MainView
        public MasterDetailSigobViewModel MasterDetailSigob { get; set; }
        public ApplicationsViewModel Applications { get; set; }
        public MenuViewModel Menu { get; set; }
        #endregion

        #region Options Menu Views
        public SecurityViewModel Security { get; set; }
        public ProfileViewModel Profile { get; set; }
        public UrlViewerViewModel UrlViewer { get; set; }
        #endregion

        #region Management Center Views
        public CalendarsViewModel Calendars { get; set; }
        public CalendarFiltersViewModel CalendarFilters { get; set; }
        public CalendarViewModel Calendar { get; set; }
        public EventCgViewModel EventCg { get; set; }
        public EditEventViewModel EditEvent { get; set; }
        public EditAppointmentViewModel EditAppointment { get; set; }
        public AppointmentViewModel Appointment { get; set; }
        public CalendarWeekViewModel CalendarWeek { get; set; }
        public CalendarMonthViewModel CalendarMonth { get; set; }
        public InstructionsViewModel Instructions { get; set; }
        public InstructionViewModel Instruction { get; set; }
        #endregion

        #region Task Views
        public TaskViewModel Task { get; set; }
        public TaskDashboardViewModel TaskDashboard { get; set; }
        public EditTaskViewModel EditTask { get; set; }
        public EventTasksViewModel EventTasks { get; set; }
        public TaskGeneralControlViewModel TaskGeneralControl { get; set; }
        #endregion

        #region Contacts Views
        public ParticipantsViewModel Participants { get; set; }
        public ExternalContactsViewModel ExternalContacts { get; set; }
        public ContactsViewModel Contacts { get; set; }
        #endregion

        #region AttachmentsViews
        public AttachmentsViewModel Attachments { get; set; }
        #endregion

        #region Correspondence
        public MailBoxesViewModel MailBoxes { get; set; }
        public ExternalDocumentViewModel ExternalDocument { get; set; }
        public InternalDocumentViewModel InternalDocument { get; set; }
        public DocumentsTrayViewModel DocumentsTray { get; set; }
        public SearchDocumentViewModel SearchDocument { get; set; }
        public ScannedTiffViewModel ScannedTiff { get; set; }
        public ScannedPdfViewModel ScannedPdf { get; set;  }
        public ScannedDocumentViewModel ScannedDocument { get; set; }
        public DocumentTraceRouteViewModel DocumentTraceRoute { get; set; }
        #endregion

        #region Documents
        public DocumentViewerViewModel DocumentViewer { get; set; }
        public PdfViewerViewModel PdfViewer { get; set; }
        #endregion

        #region Workflows Views
        public WorkflowsViewModel Workflows { get; set; }
        //public ActivitiesViewModel Activities { get; set; }
        #endregion
        #endregion

        #region Constructors
        public MainViewModel()
        {
            instance = this;
            this.Login = new LoginViewModel();
        }
        #endregion

        #region Singleton
        private static MainViewModel instance;

        public static MainViewModel GetInstance()
        {
            if (instance == null)
            {
                instance = new MainViewModel();
            }
            return instance;
        }
        #endregion
    }
}
