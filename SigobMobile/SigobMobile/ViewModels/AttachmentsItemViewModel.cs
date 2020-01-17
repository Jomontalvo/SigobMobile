using System;
using System.Threading.Tasks;
using System.Windows.Input;
using AsyncAwaitBestPractices.MVVM;
using GalaSoft.MvvmLight.Command;
using SigobMobile.Common.Models;
using SigobMobile.Views.Common;

namespace SigobMobile.ViewModels
{
    public class AttachmentsItemViewModel : Attachment
    {
        #region Properties
        public DocumentSource Source { get; set; }
        #endregion

        #region Commands
        public IAsyncCommand SelectAttachmentCommand => new AsyncCommand(SelectAttachmentAsync); 
        #endregion

        #region Methods
        private async Task SelectAttachmentAsync()
        {
            var mainViewModel = MainViewModel.GetInstance();
            mainViewModel.DocumentViewer = new DocumentViewerViewModel((Attachment)this, Source);
            await App.Navigator.PushAsync(new DocumentViewerPage());
        }
        #endregion
    }
}
