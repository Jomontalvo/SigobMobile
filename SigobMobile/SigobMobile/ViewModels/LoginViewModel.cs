namespace SigobMobile.ViewModels
{
    using System.Windows.Input;
    using GalaSoft.MvvmLight.Command;
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
            this.UserName = "isma";
            this.Password = "1234";
            this.Institution = "Select country and institution...";
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
            if (string.IsNullOrEmpty(UserName))
            {
                await Application.Current.MainPage.DisplayAlert(
                title: "Error",
                message: "Enter your username",
                cancel: "Cancel");
                return;
            }
            if (string.IsNullOrEmpty(Password))
            {
                await Application.Current.MainPage.DisplayAlert(
                title: "Error",
                message: "Enter your password",
                cancel: "Cancel");
                return;
            }
            if (string.IsNullOrEmpty(Institution))
            {
                await Application.Current.MainPage.DisplayAlert(
                title: "Error",
                message: "Select a Country and Institution",
                cancel: "Cancel");
                return;
            }
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
                    "Error",
                    connection.Message,
                    "Cancel");
                return;
            }

            // 2. Get the security parameters for connection.
            var response = await this.apiService.GetList<string[]>(
                App.UrlBaseApiSigob,
                App.PrefixApiSigob,
                "security/parameters"
            );
            if (!response.IsSuccess)
            {
                this.IsRunning = false;
                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    response.Message,
                    "Cancel");
                return;
            }
            var parameters = (string[])response.Result;
            await Application.Current.MainPage.DisplayAlert(
                "Ok",
                response.Message,
                "Cancel");
            //If sucessfuly then clean entries user / password / institution
            this.IsRunning = false;
            this.IsEnabled = true;
            this.UserName = this.Password = this.Institution = string.Empty;

            //Initialize Institutions to Connect Page (View)
            //await Application.Current.MainPage.p = new InstitutionsConnectPage();
        }
        #endregion
    }
}
