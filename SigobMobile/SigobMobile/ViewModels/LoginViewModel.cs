namespace SigobMobile.ViewModels
{
    using System.Windows.Input;
    using GalaSoft.MvvmLight.Command;
    using SigobMobile.Views;
    using Xamarin.Forms;

    public class LoginViewModel : BaseViewModel
    {

        #region Attributes
        private string username;
        private string password;
        private string institution;
        private bool isRunning;
        private bool isEnabled;
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
            this.isEnabled = true;
            //Test data
            this.UserName = "isma";
            this.Password = "1234";
            this.Institution = "Proyecto Regional";
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
            //Code to connect here!
            //If sucessfuly then clean entries user / password / institution

            this.IsRunning = false;
            this.IsEnabled = true;
            this.UserName = this.Password = this.Institution = string.Empty;

            //Initialize Institutions to Connect Page (View)
            //await Application.Current.MainPage.p = new InstitutionsConnectPage();

            MainViewModel.GetInstance().InstitutionsConnect = new InstitutionsConnectViewModel();
            Application.Current.MainPage = new InstitutionsConnectPage();
        }

        #endregion
    }
}
