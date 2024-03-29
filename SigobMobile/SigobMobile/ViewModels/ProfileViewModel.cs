﻿namespace SigobMobile.ViewModels
{
    using System;
    using System.Diagnostics;
    using System.IO;
    using System.Threading.Tasks;
    using System.Windows.Input;
    using AsyncAwaitBestPractices.MVVM;
    using Common.Helpers;
    using Common.Models;
    using Common.Services;
    using GalaSoft.MvvmLight.Command;
    using Helpers;
    using Interfaces;
    using Plugin.Media;
    using Plugin.Media.Abstractions;
    using Xamarin.Forms;

    public class ProfileViewModel : BaseViewModel
    {
        #region Services
        internal ApiService apiService;
        internal string apiProfileController = "user/profile";
        #endregion

        #region Attributes
        private ImageSource imageSource;
        private MediaFile file;
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
        private string isMaleSelected;
        private string isFemaleSelected;
        private byte? gender;
        #endregion

        #region Properties
        public ImageSource ImageSource
        {
            get { return this.imageSource; }
            set { SetValue(ref this.imageSource, value); }
        }
        public string IsMaleSelected
        {
            get { return isMaleSelected; }
            set { SetValue(ref this.isMaleSelected, value); }
        }
        public string IsFemaleSelected
        {
            get { return isFemaleSelected; }
            set { SetValue(ref this.isFemaleSelected, value); }
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
        public byte? Gender
        {
            get { return gender; }
            set { SetValue(ref this.gender, value); }
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
            IErrorHandler errorHandler = null;
            this.apiService = new ApiService();
            this.LoadUserProfile().FireAndForgetSafeAsync(errorHandler);
        }
        #endregion

        #region Commands
        public IAsyncCommand ChangeUserImageCommand => new AsyncCommand(ChangeUserImage);
        public ICommand SelectFemaleCommand => new RelayCommand(SelectFemale);
        public ICommand SelectMaleCommand => new RelayCommand(SelectMale);
        #endregion

        #region Methods
        /// <summary>
        /// Selects the female gender in radio button
        /// </summary>
        private void SelectFemale()
        {
            IsFemaleSelected = "ic_rbutton_selected";
            IsMaleSelected = "ic_rbutton_unselected";
            this.Gender = 2;
        }

        /// <summary>
        /// Selects the male gender in radio button
        /// </summary>
        private void SelectMale()
        {
            IsMaleSelected = "ic_rbutton_selected";
            IsFemaleSelected = "ic_rbutton_unselected";
            this.Gender = 1;
        }

        /// <summary>
        /// Changes the user image.
        /// </summary>
        private async Task ChangeUserImage()
        {
            await CrossMedia.Current.Initialize();

            if (CrossMedia.Current.IsCameraAvailable &&
                CrossMedia.Current.IsTakePhotoSupported)
            {
                var source = await Application.Current.MainPage.DisplayActionSheet(
                    Languages.SourceImageQuestion,
                    Languages.Cancel,
                    Languages.DeletePicture,
                    Languages.FromGallery,
                    Languages.FromCamera);

                if (source == Languages.Cancel)
                {
                    this.file = null;
                    return;
                }

                if (source == Languages.FromCamera)
                {
                    this.file = await CrossMedia.Current.TakePhotoAsync(
                        new StoreCameraMediaOptions
                        {
                            Directory = "Sample",
                            Name = "test.jpg",
                            PhotoSize = PhotoSize.Small,
                        }
                    );
                }
                else
                {
                    this.file = await CrossMedia.Current.PickPhotoAsync();
                }
            }
            else
            {
                this.file = await CrossMedia.Current.PickPhotoAsync();
            }

            if (this.file != null)
            {
                this.ImageSource = ImageSource.FromStream(() =>
                {
                    var stream = file.GetStream();
                    return stream;
                });
            }
        }

        /// <summary>
        /// Loads the user profile.
        /// </summary>
        private async Task LoadUserProfile()
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
                else ImageSource = (userProfile.Gender == 2) ? ImageSource.FromFile("ic_avatar_women") : ImageSource.FromFile("ic_avatar_men");
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
            if (userProfile.Gender == null) IsMaleSelected = IsFemaleSelected = "ic_rbutton_unselected";
            else
            {
                if (userProfile.Gender == 2)
                {
                    IsFemaleSelected = "ic_rbutton_selected";
                    IsMaleSelected = "ic_rbutton_unselected";
                }
                else
                {
                    IsMaleSelected = "ic_rbutton_selected";
                    IsFemaleSelected = "ic_rbutton_unselected";
                }
            }
        }
        #endregion
    }
}
