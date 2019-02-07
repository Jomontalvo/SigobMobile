namespace SigobMobile.ViewModels
{
    using System.Linq;
    using System.Windows.Input;
    using GalaSoft.MvvmLight.Command;
    using Helpers;
    using Views;
    using Xamarin.Forms;

    public class MenuItemViewModel
    {
        #region Properties
        public string Icon { get; set; }
        public string Title { get; set; }
        public string PageName { get; set; }
        public string CurrentPageName
        {
            get { return App.Navigator.CurrentPage.ToString().Split('.').Last(); }
        }
        #endregion

        #region Commands
        public ICommand NavigateCommand
        {
            get
            {
                return new RelayCommand(Navigate);
            }
        }

        /// <summary>
        /// Navigate to Detail Page selected in MenuPage
        /// </summary>
        private void Navigate()
        {
            App.Master.IsPresented = false;
            var mainViewModel = MainViewModel.GetInstance();
            switch (this.PageName)
            {
                case "ApplicationsPage":
                    mainViewModel.Applications = new ApplicationsViewModel();
                    App.Master.Detail = new NavigationPage(new ApplicationsPage());
                    //App.Navigator.PushAsync(appPage);
                    break;
                case "SecurityPage":
                    mainViewModel.Security = new SecurityViewModel();
                    App.Master.Detail = new NavigationPage(new SecurityPage());
                    //App.Navigator.PushAsync(secPage);
                    break;
                case "WebViewHelpPage":
                    break;
                case "WebViewTermsPage":
                    break;
                case "WebViewContactPage":
                    break;
                case "LoginPage":
                    //Delete persist token values
                    Settings.Token = Settings.DbToken = Settings.InstitutionLogo = string.Empty;
                    mainViewModel.Token = mainViewModel.DbToken = string.Empty;
                    Application.Current.MainPage = new NavigationPage(new LoginPage());
                    break;
            }
        }
        #endregion
    }
}
