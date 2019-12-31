namespace SigobMobile.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using System.Windows.Input;
    using AsyncAwaitBestPractices.MVVM;
    using GalaSoft.MvvmLight.Command;
    using Common.Services;
    using SigobMobile.Helpers;
    using Xamarin.Forms;
    using Common.Helpers;
    using Common.Models;

    public class SecurityViewModel : BaseViewModel
    {
        #region Services
        internal ApiService apiService;
        internal string apiUserController = "user/security/changepwd";
        internal string apiSecurityController = "security/parameters";
        #endregion

        #region Attributes
        private string newPassword;
        private string verifyNewPassword;
        private bool isRunning;
        private bool isEnabled;
        private bool isNewPassword;
        private bool isVerifyNewPassword;
        private string iconViewNewPassword;
        private bool isVisibleIconViewNewPassword;
        private string iconViewVerifyPassword;
        private bool isVisibleIconViewVerifyPassword;

        #endregion

        #region Properties
        public string NewPassword
        {
            get { return this.newPassword; }
            set { SetValue(ref this.newPassword, value); }
        }
        public string VerifyNewPassword
        {
            get { return this.verifyNewPassword; }
            set { SetValue(ref this.verifyNewPassword, value); }
        }
        public bool IsRunning
        {
            get { return this.isRunning; }
            set { SetValue(ref this.isRunning, value); }
        }
        public bool IsEnabled
        {
            get { return this.isEnabled; }
            set { SetValue(ref this.isEnabled, value); }
        }
        public bool IsNewPassword
        {
            get { return this.isNewPassword; }
            set { SetValue(ref this.isNewPassword, value); }
        }
        public bool IsVerifyNewPassword
        {
            get { return this.isVerifyNewPassword; }
            set { SetValue(ref this.isVerifyNewPassword, value); }
        }
        public string IconViewNewPassword
        {
            get { return this.iconViewNewPassword; }
            set { SetValue(ref this.iconViewNewPassword, value); }
        }
        public bool IsVisibleIconViewNewPassword
        {
            get { return this.isVisibleIconViewNewPassword; }
            set { SetValue(ref this.isVisibleIconViewNewPassword, value); }
        }
        public string IconViewVerifyPassword
        {
            get { return this.iconViewVerifyPassword; }
            set { SetValue(ref this.iconViewVerifyPassword, value); }
        }
        public bool IsVisibleIconViewVerifyPassword
        {
            get { return this.isVisibleIconViewVerifyPassword; }
            set { SetValue(ref this.isVisibleIconViewVerifyPassword, value); }
        }
        #endregion

        #region Constructors
        public SecurityViewModel()
        {
            apiService = new ApiService();
            this.isEnabled = true;
            this.IsNewPassword = this.IsVerifyNewPassword = true;
            this.IconViewNewPassword = this.iconViewVerifyPassword = "ic_eye_closed";
        }
        #endregion

        #region Commands
        /// <summary>
        /// Gets the login command.
        /// </summary>
        /// <value>The login command.</value>
        public IAsyncCommand LoginCommand => new AsyncCommand(ChangePasswordLogin);

        /// <summary>
        /// Show or hide password command button.
        /// </summary>
        /// <value>The show hide password command.</value>
        public ICommand ShowHideNewPasswordCommand => new RelayCommand(ShowHideNewPassword);

        /// <summary>
        /// Show or hide password command button.
        /// </summary>
        /// <value>The show hide password command.</value>
        public ICommand ShowHideVerifyPasswordCommand => new RelayCommand(ShowHideVerifyPassword);
        #endregion

        #region Methods
        /// <summary>
        /// Shows or hide password characters in new password field
        /// </summary>
        private void ShowHideNewPassword()
        {
            IsNewPassword = !IsNewPassword;
            IconViewNewPassword = (!IsNewPassword) ? "ic_eye" : "ic_eye_closed";
        }

        /// <summary>
        /// Shows or hide password characters in new password field
        /// </summary>
        private void ShowHideVerifyPassword()
        {
            IsVerifyNewPassword = !IsVerifyNewPassword;
            IconViewVerifyPassword = (!IsVerifyNewPassword) ? "ic_eye" : "ic_eye_closed";
        }

        /// <summary>
        /// Login to Sigob System with user credentials (validate entries)
        /// </summary>
        private async Task ChangePasswordLogin()
        {
            #region Exists values in change password fields and match values
            if (string.IsNullOrEmpty(NewPassword))
            {
                await Application.Current.MainPage.DisplayAlert(
                title: Languages.Error,
                message: Languages.NewPasswordValidationMsg,
                cancel: Languages.Cancel);
                return;
            }
            if (string.IsNullOrEmpty(VerifyNewPassword))
            {
                await Application.Current.MainPage.DisplayAlert(
                title: Languages.Error,
                message: Languages.RepeatNewPasswordValidationMsg,
                cancel: Languages.Cancel);
                return;
            }
            if ( NewPassword != VerifyNewPassword)
            {
                await Application.Current.MainPage.DisplayAlert(
                title: Languages.Error,
                message: Languages.PasswordsMatchValidationMsg,
                cancel: Languages.Cancel);
                return;
            }
            #endregion

            #region API - Get Security Parameters for change password
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
                apiSecurityController
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
                #region API - Post for Change Password and assign token
                var parameters = (List<string>)response.Result;
                var newLoginCredentials =
                new NewPasswordModel()
                {
                    Password = this.NewPassword,
                    Ip = parameters[1],
                    UserAgent = parameters[0],
                };
                response = await this.apiService.Put<NewSessionSigob>(
                    Settings.UrlBaseApiSigob,
                    App.PrefixApiSigob,
                    apiUserController,
                    Settings.Token,
                    Settings.DbToken,
                    newLoginCredentials);
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
                var sucessLogin = (NewSessionSigob)response.Result;
                App.ActiveSession.AuthToken = sucessLogin.AuthToken;
                App.ActiveSession.DatabaseToken = sucessLogin.DatabaseToken;
                App.ActiveSession.LoggedUser.Username = sucessLogin.LoggedUser;
                App.ActiveSession.LoggedUser.Password = sucessLogin.Password;

                //Save persist token access values
                Settings.Token = sucessLogin.AuthToken;
                Settings.DbToken = sucessLogin.DatabaseToken;

                //Successful change alert
                await Application.Current.MainPage.DisplayAlert(
                        title: Languages.ChangePasswordTitle,
                        message: Languages.Success,
                        cancel: Languages.Ok);

                // Main View Model Access values
                var mainViewModel = MainViewModel.GetInstance();
                mainViewModel.Token = sucessLogin.AuthToken;
                mainViewModel.DbToken = sucessLogin.DatabaseToken;

                //Back to Main Page
                await App.Navigator.PopToRootAsync();
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
                this.NewPassword = this.VerifyNewPassword = string.Empty;
            }
        }
        #endregion

    }
}
