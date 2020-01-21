namespace SigobMobile.ViewModels
{
    using System;
    using System.Threading.Tasks;
    using AsyncAwaitBestPractices.MVVM;
    using SigobMobile.Common.Helpers;
    using SigobMobile.Common.Models;
    using SigobMobile.Common.Services;
    using SigobMobile.Helpers;
    using SigobMobile.Interfaces;
    using Xamarin.Essentials;

    public class DocumentViewerViewModel : BaseViewModel
    {
        #region Services
        private readonly ApiService apiService;
        public string ApiController { get => $"documents/{this.Attachment.Id}/source/{(int)this.Source}/type/{(int)this.Attachment.FileType}"; }
        #endregion

        #region Attributes
        private bool isRunning;
        private bool isEnabled;
        private DownloadDocument doc;
        #endregion

        #region Properties
        public Attachment Attachment { get; set; }
        public DocumentSource Source { get; }
        public DownloadDocument Doc
        {
            get => this.doc;
            set => SetValue(ref this.doc, value);
        }
        public bool IsRunning
        {
            get => this.isRunning;
            set => SetValue(ref this.isRunning, value);
        }
        public bool IsEnabled
        {
            get => this.isEnabled;
            set => SetValue(ref this.isEnabled, value);
        }
        #endregion

        #region Constructors
        public DocumentViewerViewModel(Attachment attach, DocumentSource source)
        {
            this.apiService = new ApiService();
            this.Attachment = attach;
            this.Source = source;
            IErrorHandler errorHandler = null;
            this.GetDownloadDocument().FireAndForgetSafeAsync(errorHandler);
        }
        #endregion

        #region Commands
        public IAsyncCommand ShareDocumentCommand => new AsyncCommand(this.ShareDocument);
        #endregion

        #region Methods
        /// <summary>
        /// Share command
        /// </summary>
        /// <returns></returns>
        private async Task ShareDocument()
        {
            await Share.RequestAsync(new ShareFileRequest
            {
                Title = Languages.Attachment,
                File = new ShareFile(Doc.UrlDocument)
            });
        }

        /// <summary>
        /// Get download document
        /// </summary>
        /// <returns></returns>
        private async Task GetDownloadDocument()
        {
            this.IsEnabled = true;
            this.IsRunning = true;
            try
            {
                // 1. Verify connection
                var connection = await this.apiService.CheckConnection();
                if (!connection.IsSuccess)
                {
                    this.IsRunning = false;
                    await App.Navigator.CurrentPage.DisplayAlert(
                        Languages.Error,
                        connection.Message,
                        Languages.Cancel);
                    return;
                }

                // 2. Get the menu list from API
                var response = await this.apiService.Get<DownloadDocument>(
                        Settings.UrlBaseApiSigob,
                        App.PrefixApiSigob,
                        this.ApiController,
                        Settings.Token,
                        Settings.DbToken
                    );
                if (!response.IsSuccess)
                {
                    this.IsRunning = false;
                    await App.Navigator.CurrentPage.DisplayAlert(
                        title: Languages.Error,
                        message: response.Message,
                        cancel: Languages.Cancel);
                    return;
                }

                //Get downliad file values
                this.Doc = (DownloadDocument)response.Result;
            }
            catch (Exception ex)
            {
                await App.Navigator.CurrentPage.DisplayAlert(
                    Languages.Error,
                    $"{Languages.GeneralError}: {ex.Message}",
                    Languages.Cancel);
            }
            finally
            {
                IsRunning = false;
                IsEnabled = true;
            }
        }
        #endregion
    }
}
