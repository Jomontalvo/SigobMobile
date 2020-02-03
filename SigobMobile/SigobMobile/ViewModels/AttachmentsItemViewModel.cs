namespace SigobMobile.ViewModels
{
    using System;
    using System.IO;
    using System.Threading.Tasks;
    using AsyncAwaitBestPractices.MVVM;
    using SigobMobile.Common.Helpers;
    using SigobMobile.Common.Models;
    using SigobMobile.Common.Services;
    using SigobMobile.Helpers;
    using SigobMobile.Interfaces;
    using SigobMobile.Views.Common;
    using Xamarin.Essentials;
    using Xamarin.Forms;

    /// <summary>
    /// Item View Model for Attachment
    /// </summary>
    public class AttachmentsItemViewModel : Attachment
    {
        #region Fields
        private readonly ApiService apiService = new ApiService();
        private readonly IFileViewerService fileViewerService;
        #endregion

        #region Properties
        public DocumentSource Source { get; set; }
        public string ApiController { get => $"documents/{this.Id}/source/{(int)this.Source}/type/{(int)this.FileType}"; }
        #endregion

        #region Constructors
        public AttachmentsItemViewModel()
        {
            this.fileViewerService = DependencyService.Get<IFileViewerService>();
        }
        #endregion

        #region Commands
        public IAsyncCommand SelectAttachmentCommand => new AsyncCommand(SelectAttachmentAsync);
        #endregion

        #region Methods
        private async Task SelectAttachmentAsync()
        {
            var attachmentsViewModel = MainViewModel.GetInstance().Attachments;
            attachmentsViewModel.IsRunning = true;
            if (this.FileType == DocumentType.PDF)
            {
                var mainViewModel = MainViewModel.GetInstance();
                mainViewModel.PdfViewer = new PdfViewerViewModel((Attachment)this, Source);
                await App.Navigator.PushAsync(new PdfViewerPage());
            }
            else
            {
                await OpenAttachmentDocumentAsync();
            }
            attachmentsViewModel.IsRunning = false;
        }

        /// <summary>
        /// Get Stream and Open Attachment
        /// </summary>
        /// <returns></returns>
        private async Task OpenAttachmentDocumentAsync()
        {
            try
            {
                var connection = await this.apiService.CheckConnection();
                if (!connection.IsSuccess)
                {
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
                    await App.Navigator.CurrentPage.DisplayAlert(
                        title: Languages.Error,
                        message: response.Message,
                        cancel: Languages.Cancel);
                    return;
                }

                //Get downliad file values
                var localDocument = (DownloadDocument)response.Result;
                bool canOpen = await Launcher.CanOpenAsync(localDocument.UrlDocument);
                if (canOpen)
                {
                    Uri uri = new Uri(localDocument.UrlDocument);
                    string filename = System.IO.Path.GetFileName(uri.LocalPath);
                    MemoryStream stream = await new GetStream().GetStreamFromUrl(localDocument.UrlDocument);
                    stream.Seek(0, SeekOrigin.Begin);
                    await this.fileViewerService.View(stream, filename);
                }
                else return;
            }
            catch (Exception ex)
            {
                await App.Navigator.CurrentPage.DisplayAlert(
                        title: Languages.Error,
                        message: ex.Message,
                        cancel: Languages.Cancel);
                return;
            }
        }
        #endregion
    }
}
