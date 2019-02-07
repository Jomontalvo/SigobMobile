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
            if (this.PageName != CurrentPageName)
            {
                switch (this.PageName)
                {
                    case "ApplicationsPage":

                        MainViewModel.GetInstance().Applications = new ApplicationsViewModel();
                        var appPage = new ApplicationsPage();
                        App.Master.Detail = new NavigationPage(appPage);
                        App.Navigator.PushAsync(appPage);
                        break;
                    case "SecurityPage":
                        MainViewModel.GetInstance().Security = new SecurityViewModel();
                        var secPage = new SecurityPage();
                        App.Master.Detail = new NavigationPage(secPage);
                        App.Navigator.PushAsync(secPage);
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
                        var mainViewModel = MainViewModel.GetInstance();
                        mainViewModel.Token = mainViewModel.DbToken = string.Empty;
                        Application.Current.MainPage = new NavigationPage(new LoginPage());
                        break;
                }
            }
        }
        #endregion
    }
}
