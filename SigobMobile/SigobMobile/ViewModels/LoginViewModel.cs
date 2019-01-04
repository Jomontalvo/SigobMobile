namespace SigobMobile.ViewModels
{
    using System;
    using System.Windows.Input;
    using GalaSoft.MvvmLight.Command;
    using Xamarin.Forms;

    public class LoginViewModel
    {
        #region Properties
        public string UserName
        {
            get;
            set;
        }

        public string Password
        {
            get;
            set;
        }

        public string Institution
        {
            get;
            set;
        }

        public bool IsRunning
        {
            get;
            set;
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
        }
        #endregion

        #region Commands
        public ICommand LoginCommand
        {
            get
            {
                return new RelayCommand(Login);
            }
        }

        private async void Login()
        {
           if (string.IsNullOrEmpty(this.UserName))
            {
                await Application.Current.MainPage.DisplayAlert(
                title: "Error",
                message: "Enter your username",
                cancel: "Cancel");
                return;
            }
            if (string.IsNullOrEmpty(this.Password))
            {
                await Application.Current.MainPage.DisplayAlert(
                title: "Error",
                message: "Enter your password",
                cancel: "Cancel");
                return;
            }
            if (string.IsNullOrEmpty(this.Institution))
            {
                await Application.Current.MainPage.DisplayAlert(
                title: "Error",
                message: "Select a Country and Institution",
                cancel: "Cancel");
                return;
            }
        }

        #endregion
    }
}
