namespace SigobMobile.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Windows.Input;
    using GalaSoft.MvvmLight.Command;
    using Services;
    using Models;
    using Views;
    using Xamarin.Forms;
    using Helpers;

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


        #endregion

        #region Constructors
        public LoginViewModel()
        {
            //Initialize default values
            this.KeyLowerCases = Keyboard.Create(KeyboardFlags.None);
            //Enabled Login button
            this.IsEnabled = true;
            this.IsPassword = true;
            this.IconViewPassword = "ic_eye";
            //Test data
            //this.UserName = "isma";
            //this.Password = "1234";
            this.Institution = Languages.SelectApiInstitution;
        }
        #endregion

        #region Commands
        /// <summary>
        /// Gets the login command.
        /// </summary>
        /// <value>The login command.</value>
        public ICommand LoginCommand
        {
            get
            {
                return new RelayCommand(Login);
            }
        }

        /// <summary>
        /// Gets the list to select Institution - API command.
        /// </summary>
        /// <value>The select API command.</value>
        public ICommand SelectApiCommand
        {
            get
            {
                return new RelayCommand(SelectApi);
            }
        }

        public ICommand ShowHidePasswordCommand
        { 
            get
            {
                return new RelayCommand(ShowHidePassword);
            }
        }
        #endregion

        #region Methods

        /// <summary>
        /// Shows or hide password characters in login
        /// </summary>
        private void ShowHidePassword()
        {
            IsPassword = !IsPassword;
            IconViewPassword = (IsPassword) ? "ic_eye" : "ic_eye_closed";
        }

        /// <summary>
        /// Selects the API related with Institution
        /// </summary>
        private async void SelectApi()
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
        private async void Login()
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
                App.UrlBaseApiSigob,
                App.PrefixApiSigob,
                "security/parameters"
            );
            if (!response.IsSuccess)
            {
                this.IsRunning = false;
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
                    App.UrlBaseApiSigob,
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
                //Get Session Model, Token and DbToken for logged user.
                var sucessLogin = (SessionSigob)response.Result;
                App.ActiveSession = sucessLogin;
                // Headers values to all API secured calls
                App.AuthToken = sucessLogin.AuthToken;
                App.DataBaseToken = sucessLogin.DatabaseToken;
                #endregion

                #region Navigate to SIGOB Main Page
                await Application.Current.MainPage.DisplayAlert(
                   title: Languages.Success,
                   message: response.Message,
                   cancel: Languages.Ok);
                #endregion
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert(
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
