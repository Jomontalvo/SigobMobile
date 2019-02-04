
namespace SigobMobile.ViewModels
{
    using System;
    using System.Collections.ObjectModel;
    using Helpers;

    /// <summary>
    /// Main view model.
    /// </summary>
    public class MainViewModel
    {
        #region Properties
        public ObservableCollection<MenuItemViewModel> MainMenu { get; set; }

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
            this.LoadMenu();
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

        #region Methods
        private void LoadMenu()
        {
            this.MainMenu = new ObservableCollection<MenuItemViewModel>
            {
                new MenuItemViewModel
                {
                    Icon = "ic_menu_home",
                    PageName = "ApplicationsPage",
                    Title = Languages.HomeMenu
                },
                new MenuItemViewModel
                {
                    Icon = "ic_menu_security",
                    PageName = "SecurityPage",
                    Title = Languages.SecurityMenu
                },
                new MenuItemViewModel
                {
                    Icon = "ic_menu_help",
                    PageName = "WebViewHelpPage",
                    Title = Languages.HelpMenu
                },
                new MenuItemViewModel
                {
                    Icon = "ic_menu_terms",
                    PageName = "WebViewTermsPage",
                    Title = Languages.TermsAndConditionsMenu
                },
                new MenuItemViewModel
                {
                    Icon = "ic_menu_contact",
                    PageName = "WebViewContactPage",
                    Title = Languages.ContactUs
                },
                new MenuItemViewModel
                {
                    Icon = "ic_menu_logout",
                    PageName = "LoginPage",
                    Title = Languages.LogoutMenu
                }
            };
        }
        #endregion
    }
}
