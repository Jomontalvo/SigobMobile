namespace SigobMobile.ViewModels
{
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
        #endregion

        #region Commands
        public ICommand NavigateCommand
        { 
            get
            {
                return new RelayCommand(Navigate);
            }
        }

        private void Navigate()
        {
            switch (this.PageName)
            {
                case "ApplicationsPage":
                    break;
                case "SecurityPage":
                    break;
                case "WebViewHelpPage":
                    break;
                case "WebViewTermsPage":
                    break;
                case "WebViewContactPage":
                    break;
                case "LoginPage":
                    //Delete persist token values
                    Settings.Token = Settings.DbToken = string.Empty;
                    var mainViewModel = MainViewModel.GetInstance();
                    mainViewModel.Token = mainViewModel.DbToken = string.Empty;
                    Application.Current.MainPage = new NavigationPage(new LoginPage());
                    break;
            }
        }
        #endregion
    }
}
