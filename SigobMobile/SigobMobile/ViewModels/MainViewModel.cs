
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

        public CalendarDayViewModel CalendarDay { get; set;  }

        public CalendarWeekViewModel CalendarWeek { get; set; }

        public CalendarMonthViewModel CalendarMonth { get; set; }
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
                return new MainViewModel();
            }
            return instance;
        }
        #endregion
    }
}
