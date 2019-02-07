namespace SigobMobile.ViewModels
{
    using System;
    using System.Windows.Input;
    using GalaSoft.MvvmLight.Command;
    using Services;
    using Helpers;
    using Xamarin.Forms;
    using Models;
    using System.IO;

    public class ProfileViewModel : BaseViewModel
    {
        #region Services
        internal ApiService apiService;
        internal string apiProfileController = "user/profile";
        #endregion

        #region Attributes
        private ImageSource imageSource;
        private bool isRunning;
        private bool isEnabled;
        private UserBasicProfile profile;
        //private MediaFile fileAvatar;
        #endregion

        #region Properties
        public ImageSource ImageSource
        {
            get { return this.imageSource; }
            set { SetValue(ref this.imageSource, value); }
        }
        public bool IsRunning
        {
            get { return isRunning; }
            set { SetValue(ref this.isRunning, value); }
        }
        public bool IsEnabled
        {
            get { return isEnabled; }
            set { SetValue(ref this.isEnabled, value); }
        }
        public UserBasicProfile UserLocal
        {
            get;
            set;
        }
        #endregion

        #region Constructors
        public ProfileViewModel()
        {
            this.apiService = new ApiService();
            this.LoadUserProfile();
            this.UserLocal = this.profile;
        }
        #endregion

        #region Commands
        public ICommand ChangeUserImageCommand
        {
            get { return new RelayCommand(ChangeUserImage); }

        }
        #endregion

        #region Methods
        /// <summary>
        /// Changes the user image.
        /// </summary>
        private async void ChangeUserImage()
        {
            await Application.Current.MainPage.DisplayAlert(
                    Languages.Error,
                    Languages.Success,
                    Languages.Accept);
        }

        /// <summary>
        /// Loads the user profile.
        /// </summary>
        private async void LoadUserProfile()
        {
            this.IsRunning = true;
            var connection = await this.apiService.CheckConnection();
            if (!connection.IsSuccess)
            {
                this.IsRunning = false;
                await Application.Current.MainPage.DisplayAlert(
                    Languages.Error,
                    connection.Message,
                    Languages.Cancel);
                //this.BackToLoginPage();
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
                await Application.Current.MainPage.DisplayAlert(
                    Languages.Error,
                    response.Message,
                    Languages.Cancel);
                //this.BackToLoginPage();
                return;
            }
            this.profile = (UserBasicProfile)response.Result;

            //Assign Profile values to Image and TableView components
            if (!string.IsNullOrEmpty(profile.UserImage))
            {
                byte[] userImageStream = Convert.FromBase64String(this.profile.UserImage);
                ImageSource = ImageSource.FromStream(() => new MemoryStream(userImageStream));
            }
            else ImageSource = ImageSource.FromFile("ic_avatar_men");
            this.IsRunning = false;
        }
        #endregion
    }
}
