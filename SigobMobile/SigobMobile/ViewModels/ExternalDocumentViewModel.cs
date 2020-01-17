namespace SigobMobile.ViewModels
{
    using System;
    using System.Threading.Tasks;
    using AsyncAwaitBestPractices.MVVM;
    using Common.Helpers;
    using Common.Services;
    using Helpers;
    using Interfaces;
    using SigobMobile.Common.Models;
    using SigobMobile.Views.Common;
    using SigobMobile.Views.Correspondence;
    using Xamarin.Forms;
    using Document = Models.Document;
    using ExternalDocument = Models.ExternalDocument;

    public class ExternalDocumentViewModel : BaseViewModel
    {
        #region API Constants
        private readonly string apiController = "correspondences/";
        private readonly string apiControllerSufixSteps = "/steps/";
        private readonly string apiControllerSufixSource = "/corex";
        #endregion

        #region Attributes
        private ApiService apiService;
        private string backPageTitle;
        private bool isBusy;
        private bool isEnabled;
        private ExternalDocument externalDocument;
        #endregion

        #region Properties
        public ExternalDocument ExternalDocument
        {
            get => this.externalDocument;
            set => SetValue(ref this.externalDocument, value);
        }
        public bool IsBusy
        {
            get => this.isBusy;
            set => SetValue(ref this.isBusy, value);
        }
        public bool IsEnabled
        {
            get => this.isEnabled;
            set => SetValue(ref this.isEnabled, value);
        }

        /// <summary>
        /// Sender Object
        /// </summary>
        public Document Document { get; set; }

        public string BackPageTitle
        {
            get => this.backPageTitle;
            set => SetValue(ref this.backPageTitle, value);
        }
        #endregion

        #region Constructor
        /// <summary>
        /// Instance a new view of External Correspondence
        /// </summary>
        /// <param name="document">Object of Document List</param>
        public ExternalDocumentViewModel(Document document)
        {
            this.Document = document;
            this.apiService = new ApiService();
            this.GetDefaultValues();
            IErrorHandler errorHandler = null;
            this.LoadDocumentDetailsAsync().FireAndForgetSafeAsync(errorHandler);
        }
        #endregion

        #region Commands
        public IAsyncCommand GetScannedCommand => new AsyncCommand(GetScannedAsync);
        public IAsyncCommand GetAttachmentsCommand => new AsyncCommand(GetAttachmentsAsync);
        #endregion

        #region Methods
        private void GetDefaultValues()
        {
            var parent = App.Navigator.CurrentPage.GetType().Name;
            this.BackPageTitle = parent == "DocumentsTrayPage" ? Languages.TagTray : Languages.Ok;
        }

        /// <summary>
        /// Open scanned documents
        /// </summary>
        /// <returns></returns>
        private async Task GetScannedAsync()
        {
            if (ExternalDocument.ScannedCount > 0)
            { 
                var mainViewModel = MainViewModel.GetInstance();
                mainViewModel.ScannedDocument = new ScannedDocumentViewModel(Document.RegistrationCode);
                await App.Navigator.PushAsync(new ScannedDocumentPage());
            }
            return;
        }

        /// <summary>
        /// Get attachment list
        /// </summary>
        /// <returns></returns>
        private async Task GetAttachmentsAsync()
        {
            if (ExternalDocument.AttachmentCount > 0)
            {
                var mainViewModel = MainViewModel.GetInstance();
                mainViewModel.Attachments = new AttachmentsViewModel(this.ExternalDocument.Attachments, DocumentSource.Correspondence);
                await App.Navigator.PushAsync(new AttachmentsPage());
            }
            return;
        }


        /// <summary>
        /// Load document details, difference between Request from Tray or Archive
        /// </summary>
        /// <returns>ExternalDocument Object</returns>
        private async Task LoadDocumentDetailsAsync()
        {
            this.IsBusy = true;
            this.IsEnabled = false;
            try
            {
                // 1. Verify connection
                var connection = await this.apiService.CheckConnection();
                if (!connection.IsSuccess)
                {
                    this.IsBusy = false;
                    await Application.Current.MainPage.DisplayAlert(
                        Languages.Error,
                        connection.Message,
                        Languages.Cancel);
                    return;
                }

                // 2. Get the menu list from API
                var response = await this.apiService.Get<ExternalDocument>(
                        Settings.UrlBaseApiSigob,
                        App.PrefixApiSigob,
                        $"{this.apiController}{Document.Id}{apiControllerSufixSteps}{Document.TrackingId}{apiControllerSufixSource}",
                        Settings.Token,
                        Settings.DbToken
                    );
                if (!response.IsSuccess)
                {
                    this.IsBusy = false;
                    await Application.Current.MainPage.DisplayAlert(
                        title: Languages.Error,
                        message: response.Message,
                        cancel: Languages.Cancel);
                    return;
                }

                //Get List values
                this.ExternalDocument = (ExternalDocument)response.Result;
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert(
                    Languages.Error,
                    $"{Languages.GeneralError}: {ex.Message}",
                    Languages.Cancel);
            }
            finally
            {
                IsBusy = false;
                IsEnabled = true;
            }
        }
        #endregion
    }
}
