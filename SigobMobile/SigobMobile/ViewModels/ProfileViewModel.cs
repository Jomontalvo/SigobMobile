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
    using System.Diagnostics;

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
        private UserBasicProfile userProfile;
        //private MediaFile fileAvatar;
        private string officeId;
        private string firstName;
        private string lastName;
        private string institution;
        private string position;
        private string section;
        private string phone;
        private string cellPhone;
        private string email;
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

        #region Personal Data Profile
        /// <summary>
        /// Gets or sets the office identifier (Despacho Code)
        /// </summary>
        /// <value>The office identifier.</value>
        public string OfficeId
        {
            get { return officeId; }
            set { SetValue(ref this.officeId, value); }
        }
        public string FirstName
        {
            get { return firstName; }
            set { SetValue(ref this.firstName, value); }
        }
        public string LastName
        {
            get { return lastName; }
            set { SetValue(ref this.lastName, value); }
        }
        public string Institution
        {
            get { return institution; }
            set { SetValue(ref this.institution, value); }
        }
        public string Position
        {
            get { return position; }
            set { SetValue(ref this.position, value); }
        }
        public string Section
        {
            get { return section; }
            set { SetValue(ref this.section, value); }
        }
        public string Phone
        {
            get { return phone; }
            set { SetValue(ref this.phone, value); }
        }
        public string CellPhone
        {
            get { return cellPhone; }
            set { SetValue(ref this.cellPhone, value); }
        }
        public string Email
        {
            get { return email; }
            set { SetValue(ref this.email, value); }
        }
        #endregion

        #endregion

        #region Constructors
        public ProfileViewModel()
        {
            this.apiService = new ApiService();
            this.LoadUserProfile();
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
            try 
            {
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
                this.userProfile = (UserBasicProfile)response.Result;
                BindingUserObject();
                //Assign Profile values to Image and TableView components
                if (!string.IsNullOrEmpty(userProfile.UserImage))
                {
                    byte[] userImageStream = Convert.FromBase64String(this.userProfile.UserImage);
                    ImageSource = ImageSource.FromStream(() => new MemoryStream(userImageStream));
                }
                else ImageSource = ImageSource.FromFile("ic_avatar_men");
            }
            catch (Exception ex) { Debug.WriteLine(ex.Message); }
            finally { this.IsRunning = false; }
        }

        /// <summary>
        /// Assign properties and Bindings to user object.
        /// </summary>
        private void BindingUserObject()
        {
            OfficeId = userProfile.OfficeId;
            if (!String.IsNullOrEmpty(userProfile.FirstName)) FirstName = userProfile.FirstName;
            if (!String.IsNullOrEmpty(userProfile.LastName)) LastName = userProfile.LastName;
            if (!String.IsNullOrEmpty(userProfile.Institution)) Institution = userProfile.Institution;
            if (!String.IsNullOrEmpty(userProfile.Area)) Section = userProfile.Area;
            if (!String.IsNullOrEmpty(userProfile.Position)) Position = userProfile.Position;
            if (!String.IsNullOrEmpty(userProfile.Phone)) Phone = userProfile.Phone;
            if (!String.IsNullOrEmpty(userProfile.CellPhone)) CellPhone = userProfile.CellPhone;
            if (!String.IsNullOrEmpty(userProfile.Email)) Email = userProfile.Email;
        }
        #endregion
    }
}
