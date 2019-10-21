namespace SigobMobile.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using System.Windows.Input;
    using AsyncAwaitBestPractices.MVVM;
    using GalaSoft.MvvmLight.Command;
    using Helpers;
    using Models;
    using Services;
    using Views;
    using Xamarin.Forms;

    public class LoginViewModel : BaseViewModel
    {
        #region Services
        internal ApiService apiService;
        #endregion

        #region Attributes
        private string username;
        private string password;
        private string institution;
        private bool isRunning;
        private bool isEnabled;
        private bool isPassword;
        private string iconViewPassword;
        private bool isVisibleIconViewPassword;
        private Color isSelectedColor;
        #endregion

        #region Properties
        public string UserName
        {
            get { return this.username; }
            set { SetValue(ref this.username, value); }
        }

        public string Password
        {
            get { return this.password; }
            set { SetValue(ref this.password, value); }
        }

        public string Institution
        {
            get { return this.institution; }
            set { SetValue(ref this.institution, value); }
        }

        public bool IsRunning
        {
            get { return isRunning; }
            set { SetValue(ref this.isRunning, value); }
        }

        public bool IsEnabled
        {
            get { return this.isEnabled; }
            set { SetValue(ref this.isEnabled, value); }
        }

        public bool IsPassword
        {
            get { return this.isPassword; }
            set { SetValue(ref this.isPassword, value); }
        }

        public string IconViewPassword
        {
            get { return this.iconViewPassword; }
            set { SetValue(ref this.iconViewPassword, value); }
        }

        public bool IsVisibleIconViewPassword
        {
            get { return this.isVisibleIconViewPassword; }
            set { SetValue(ref this.isVisibleIconViewPassword, value); }
        }

        public object KeyLowerCases
        {
            get;
            set;
        }
        public Color IsSelectedColor
        {
            get { return this.isSelectedColor; }
            set { SetValue(ref this.isSelectedColor, value); }
        }
        #endregion

        #region Constructors
        public LoginViewModel()
        {
            //Initialize default values
            this.KeyLowerCases = Keyboard.Create(KeyboardFlags.None);
            //Enabled Login button
            this.IsEnabled = true;
            this.IsPassword = true;
            this.IconViewPassword = "ic_eye_closed";
            //Institution data
            this.isSelectedColor = Color.LightGray;
            this.Institution = Languages.SelectApiInstitution;
        }
        #endregion

        #region Commands
        /// <summary>
        /// Gets the login command.
        /// </summary>
        /// <value>The login command.</value>
        public IAsyncCommand LoginCommand => new AsyncCommand(Login);

        /// <summary>
        /// Gets the list to select Institution - API command.
        /// </summary>
        /// <value>The select API command.</value>
        public ICommand SelectApiCommand => new AsyncCommand(SelectApi);

        /// <summary>
        /// Show or hide password command button.
        /// </summary>
        /// <value>The show hide password command.</value>
        public ICommand ShowHidePasswordCommand => new RelayCommand(ShowHidePassword);
        #endregion

        #region Methods

        /// <summary>
        /// Shows or hide password characters in login
        /// </summary>
        private void ShowHidePassword()
        {
            IsPassword = !IsPassword;
            IconViewPassword = (!IsPassword) ? "ic_eye" : "ic_eye_closed";
        }

        /// <summary>
        /// Selects the API related with Institution
        /// </summary>
        private async Task SelectApi()
        {
            ////Call without Navigate
            //MainViewModel.GetInstance().InstitutionsConnect = new InstitutionsConnectViewModel();
            //Application.Current.MainPage = new InstitutionsConnectPage();

            //Instance new ViewModel before Navigate.
            var mainViewModel = MainViewModel.GetInstance();
            mainViewModel.InstitutionsConnect = new InstitutionsConnectViewModel();
            await Application.Current.MainPage.Navigation.PushAsync(new InstitutionsConnectPage());
        }


        /// <summary>
        /// Login to Sigob System with user credentials (validate entries)
        /// </summary>
        private async Task Login()
        {
            #region Exists values in login fields
            if (string.IsNullOrEmpty(UserName))
            {
                await Application.Current.MainPage.DisplayAlert(
                title: Languages.Error,
                message: Languages.UserValidationMsg,
                cancel: Languages.Cancel);
                return;
            }
            if (string.IsNullOrEmpty(Password))
            {
                await Application.Current.MainPage.DisplayAlert(
                title: Languages.Error,
                message: Languages.PasswordValidationMsg,
                cancel: Languages.Cancel);
                return;
            }
            if (string.IsNullOrEmpty(Institution) || Institution==Languages.SelectApiInstitution)
            {
                await Application.Current.MainPage.DisplayAlert(
                title: Languages.Error,
                message: Languages.SelectApiInstitution,
                cancel: Languages.Cancel);
                return;
            }
            #endregion

            #region API - Get Security Parameters for login
            this.IsRunning = true;
            this.IsEnabled = false;

            //Connect using API
            this.apiService = new ApiService();

            // 1. Verify connection
            var connection = await this.apiService.CheckConnection();
            if (!connection.IsSuccess)
            {
                this.IsRunning = false;
                await Application.Current.MainPage.DisplayAlert(
                    title: Languages.Error,
                    message: connection.Message,
                    cancel: Languages.Cancel);
                return;
            }

            // 2. Get the security parameters for connection.
            var response = await this.apiService.GetList<string>(
                Settings.UrlBaseApiSigob,
                App.PrefixApiSigob,
                "security/parameters"
            );
            if (!response.IsSuccess)
            {
                this.IsRunning = false;
                this.IsEnabled = true;
                await Application.Current.MainPage.DisplayAlert(
                    title: Languages.Error,
                    message: response.Message,
                    cancel: Languages.Cancel);
                return;
            }
            #endregion

            // 3. Call API Login
            try
            {
                #region API - Post for Login and assign token
                var parameters = (List<string>)response.Result;
                var loginCredentials =
                new LoginCredentials()
                {
                    Attemps = 1,
                    Ip = parameters[1],
                    Password = this.Password,
                    UserAgent = parameters[0],
                    UserName = this.UserName,
                };
                response = await this.apiService.PostLogin<LoginCredentials>(
                    Settings.UrlBaseApiSigob,
                    App.PrefixApiSigob,
                    "user/login",
                    loginCredentials);
                if (!response.IsSuccess)
                {
                    this.IsRunning = false;
                    this.IsEnabled = true;
                    await Application.Current.MainPage.DisplayAlert(
                        title: Languages.Error,
                        message: Languages.InvalidCredentials,
                        cancel: Languages.Cancel);
                    return;
                }
                //Get Session Model (Token and DbToken for logged user).
                var sucessLogin = (SessionSigob)response.Result;
                App.ActiveSession = sucessLogin;
                #endregion

                #region Global Parameters Central API
                response = null;
                int id = -1; //not use identifier
                response = await this.apiService.Get<GlobalParameters>(
                    App.UrlBaseCentral,
                    App.PrefixParametersCentral,
                    App.ControllerCentral,
                    id
                );
                if (!response.IsSuccess)
                {
                    IsRunning = false;
                    IsEnabled = true;
                    await Application.Current.MainPage.DisplayAlert(
                        Languages.Error,
                        response.Message,
                        Languages.Cancel);
                    return;
                }
                var globalParameters = (GlobalParameters)response.Result;
                Settings.UrlWebCentral = globalParameters.UrlWebContent.AbsoluteUri;
                Settings.UrlWebHelpContent = globalParameters.UrlWebHelp.AbsoluteUri;
                Settings.UrlWebTermsContent = globalParameters.UrlWebTerms.AbsoluteUri;
                Settings.UrlWebContactUsContent = globalParameters.UrlWebContactUs.AbsoluteUri;
                #endregion

                #region Navigate to SIGOB Main Page

                var mainViewModel = MainViewModel.GetInstance();
                mainViewModel.Token = sucessLogin.AuthToken;
                mainViewModel.DbToken = sucessLogin.DatabaseToken;

                //Save persist token access values
                Settings.Token = sucessLogin.AuthToken;
                Settings.DbToken = sucessLogin.DatabaseToken;
                Settings.InstitutionLogo = sucessLogin.InstitutionLogo;
                Settings.FullName = sucessLogin.LoggedUser.Name;
                Settings.OfficeCode = sucessLogin.LoggedUser.OfficeCode;

                //Load Master Detail with ApplicationsPage
                mainViewModel.Applications = new ApplicationsViewModel();
                mainViewModel.Menu = new MenuViewModel();
                Application.Current.MainPage = new MasterDetailSigobPage();

                //await Application.Current.MainPage.Navigation.PushAsync(new ApplicationsPage());
                #endregion
            }
            catch (Exception ex)
            {
                await Xamarin.Forms.Application.Current.MainPage.DisplayAlert(
                      title: Languages.Error,
                      message: ex.Message,
                      cancel: Languages.Cancel);
            }
            finally
            {
                //If sucessfuly then clean entries user / password
                this.IsRunning = false;
                this.IsEnabled = true;
                this.UserName = this.Password = string.Empty;
            }
            //Initialize Institutions to Connect Page (View)
            //await Application.Current.MainPage.p = new InstitutionsConnectPage();
        }
        #endregion
    }
}
