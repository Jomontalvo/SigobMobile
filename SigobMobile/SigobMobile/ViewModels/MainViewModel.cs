
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
        public LoginViewModel Login { get; set; }

        public InstitutionsConnectViewModel InstitutionsConnect { get; set; }

        public MasterDetailSigobViewModel MasterDetailSigob { get; set; }

        public ApplicationsViewModel Applications { get; set; }

        public MenuViewModel Menu { get; set; }

        public SecurityViewModel Security { get; set; }

        public ProfileViewModel Profile { get; set; }

        public CalendarsViewModel Calendars { get; set; }

        public CalendarFiltersViewModel CalendarFilters { get; set; }

        public CalendarDayViewModel CalendarDay { get; set;  }

        public EventCgViewModel EventCg { get; set; }

        public EditEventViewModel EditEvent { get; set; }

        public EditAppointmentViewModel EditAppointment { get; set; }

        public AppointmentViewModel Appointment { get; set; }

        public CalendarWeekViewModel CalendarWeek { get; set; }

        public CalendarMonthViewModel CalendarMonth { get; set; }

        public InstructionsViewModel Instructions { get; set;  }

        public InstructionViewModel Instruction { get; set; }

        public TaskViewModel Task { get; set; }
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
