namespace SigobMobile.ViewModels
{
    using System;
    using System.Collections.ObjectModel;
    using System.IO;
    using System.Threading.Tasks;
    using AsyncAwaitBestPractices.MVVM;
    using Helpers;
    using Interfaces;
    using Models;
    using Services;
    using Views;
    using Xamarin.Forms;
    //using Xamarin.Forms.PlatformConfiguration.iOSSpecific;

    public class MenuViewModel : BaseViewModel
    {
        #region Services
        internal ApiService apiService;
        internal string apiProfileController = "user/profile";
        #endregion

        #region Attributes
        private ImageSource imageSource;
        private ImageSource institutionLogo;
        private string userFullName;
        private bool isRunning;
        private UserBasicProfile profile;

        #endregion

        #region Properties

        public ObservableCollection<MenuItemViewModel> MainMenu { get; set; }

        public ImageSource ImageSource
        {
            get { return this.imageSource; }
            set { SetValue(ref this.imageSource, value); } 
        }

        public ImageSource InstitutionLogo
        {
            get { return this.institutionLogo; }
            set { SetValue(ref this.institutionLogo, value); }
        }

        public string UserFullName
        {
            get { return this.userFullName; }
            set { SetValue(ref this.userFullName, value); }
        }
        public bool IsRunning
        {
            get { return isRunning; }
            set { SetValue(ref this.isRunning, value); }
        }
        #endregion

        #region Constructors
        public MenuViewModel()
        {
            //Get Profile image, full name and menu options.
            IErrorHandler errorHandler = null;
            this.apiService = new ApiService();
            this.LoadMenu();
            this.LoadBasicProfileData().FireAndForgetSafeAsync(errorHandler);

        }
        #endregion

        #region Commands
        public IAsyncCommand GetProfileCommand => new AsyncCommand(GetProfile);
        #endregion

        #region Methods
        /// <summary>
        /// Gets the current user profile (Tap image or full name).
        /// </summary>
        private async Task GetProfile()
        {
            await App.Navigator.PopToRootAsync(false);
            App.Master.IsPresented = false;
            var profileViewModel = MainViewModel.GetInstance();
            profileViewModel.Profile = new ProfileViewModel();
            await App.Navigator.PushAsync(new ProfilePage(), false);
        }

        /// <summary>
        /// Backs to login page.
        /// </summary>
        private void BackToLoginPage()
        {
            Settings.Token = Settings.DbToken = Settings.InstitutionLogo = string.Empty;
            var mainViewModel = MainViewModel.GetInstance();
            mainViewModel.Token = mainViewModel.DbToken = string.Empty;
            //Return to ViewModel support iPhoneX LargeTitle
            App.Navigator.PopToRootAsync();
            var navLoginPage = new NavigationPage(new LoginPage());
            //navLoginPage.On<Xamarin.Forms.PlatformConfiguration.iOS>().SetPrefersLargeTitles(true);
            //navLoginPage.On<Xamarin.Forms.PlatformConfiguration.iOS>().SetLargeTitleDisplay(LargeTitleDisplayMode.Automatic);
            Application.Current.MainPage = navLoginPage;
        }

        /// <summary>
        /// Loads the name of the user image.
        /// </summary>
        private async Task LoadBasicProfileData()
        {
            this.IsRunning = true;
            var connection = await this.apiService.CheckConnection();
            if (!connection.IsSuccess)
            {
                this.IsRunning = false;
                if (Application.Current.MainPage != null)
                await Application.Current.MainPage.DisplayAlert(
                    Languages.Error,
                    connection.Message,
                    Languages.Cancel);
                this.BackToLoginPage();
                return;
            }

            //EndPoint API Call
            var response = await this.apiService.Get<UserBasicProfile>(
                urlBase: Settings.UrlBaseApiSigob,
                servicePrefix: App.PrefixApiSigob,
                controller: apiProfileController,
                authToken: Settings.Token,
                authDbToken: Settings.DbToken
            );

            if (!response.IsSuccess)
            {
                this.IsRunning = false;
                var apiErrorResult = response.Result;
                await Xamarin.Forms.Application.Current.MainPage.DisplayAlert(
                    Languages.Error,
                    response.Message,
                    Languages.Cancel);
                this.BackToLoginPage();
                return;
            }
            this.profile = (UserBasicProfile)response.Result;

            //Assign Profile values Full Name and User Image
            this.UserFullName  = $"{profile.FirstName}  {profile.LastName}";
            if (!string.IsNullOrEmpty(profile.UserImage))
            {
                byte[] userImageStream = Convert.FromBase64String(this.profile.UserImage);
                ImageSource = ImageSource.FromStream(() => new MemoryStream(userImageStream));
            }
            else ImageSource = ImageSource.FromFile("ic_avatar_men");

            //Institution Logo
            if (!string.IsNullOrEmpty(Settings.InstitutionLogo))
            {
                byte[] logoImageStream = Convert.FromBase64String(Settings.InstitutionLogo);
                InstitutionLogo = ImageSource.FromStream(() => new MemoryStream(logoImageStream));
            }
            else InstitutionLogo = ImageSource.FromFile("ic_sigob_logo");
            this.IsRunning = false;
        }

        /// <summary>
        /// Loads the menu left panel
        /// </summary>
        private void LoadMenu()
        {
            this.MainMenu = new ObservableCollection<MenuItemViewModel>
            {
                new MenuItemViewModel
                {
                    Icon = "ic_menu_home",
                    PageName = "ApplicationsPage",
                    Title = Languages.HomeMenu,
                    Url = string.Empty
                },
                new MenuItemViewModel
                {
                    Icon = "ic_menu_security",
                    PageName = "SecurityPage",
                    Title = Languages.SecurityMenu,
                    Url = string.Empty
                },
                new MenuItemViewModel
                {
                    Icon = "ic_menu_help",
                    PageName = "WebViewHelpPage",
                    Title = Languages.HelpMenu,
                    Url = Settings.UrlWebHelpContent
                },
                new MenuItemViewModel
                {
                    Icon = "ic_menu_terms",
                    PageName = "WebViewTermsPage",
                    Title = Languages.TermsAndConditionsMenu,
                    Url = Settings.UrlWebTermsContent
                },
                new MenuItemViewModel
                {
                    Icon = "ic_menu_contact",
                    PageName = "WebViewContactPage",
                    Title = Languages.ContactUs,
                    Url = Settings.UrlWebContactUsContent
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
