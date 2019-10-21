namespace SigobMobile.ViewModels
{
    using System.Threading.Tasks;
    using AsyncAwaitBestPractices.MVVM;
    using Helpers;
    using SigobMobile.Views.Common;
    using Views;
    using Xamarin.Forms.PlatformConfiguration.iOSSpecific;

    public class MenuItemViewModel : BaseViewModel
    {
        #region Properties
        public string Icon { get; set; }
        public string Title { get; set; }
        public string PageName { get; set; }
        public string Url { get; set; }
        #endregion

        #region Commands
        public IAsyncCommand NavigateCommand => new AsyncCommand(Navigate);
        #endregion

        #region Methods
        /// <summary>
        /// Navigate to Detail Page selected in MenuPage
        /// </summary>
        private async Task Navigate()
        {
            var mainViewModel = MainViewModel.GetInstance();
            await App.Navigator.PopToRootAsync(false);
            switch (this.PageName)
            {
                case "SecurityPage":
                    mainViewModel.Security = new SecurityViewModel();
                    await App.Navigator.PushAsync(new SecurityPage(),true);
                    break;
                case "WebViewHelpPage":
                case "WebViewTermsPage":
                case "WebViewContactPage":
                    mainViewModel.UrlViewer = new UrlViewerViewModel(this.Url);
                    await App.Navigator.PushAsync(new UrlViewerPage(){ Title = this.Title}, true);
                    break;
                case "LoginPage":
                    //Delete persist token values
                    Settings.Token = Settings.DbToken = Settings.InstitutionLogo = Settings.FullName = Settings.OfficeCode = string.Empty;
                    mainViewModel.Token = mainViewModel.DbToken = string.Empty;
                    //Navigate to Login 
                    var navLoginPage = new Xamarin.Forms.NavigationPage(new LoginPage());
                    navLoginPage.On<Xamarin.Forms.PlatformConfiguration.iOS>().SetPrefersLargeTitles(true);
                    navLoginPage.On<Xamarin.Forms.PlatformConfiguration.iOS>().SetLargeTitleDisplay(LargeTitleDisplayMode.Automatic);
                    Xamarin.Forms.Application.Current.MainPage  = navLoginPage;
                    break;
                default:
                    break;
            }
            App.Master.IsPresented = false;
        }
        #endregion
    }
} 
