namespace SigobMobile.ViewModels
{
    using System.Threading.Tasks;
    using AsyncAwaitBestPractices.MVVM;
    using Models;
    using Views.Correspondence;

    public class DocumentsTrayItemViewModel : Document
    {
        #region Commands
        public IAsyncCommand OpenDocumentCommand => new AsyncCommand(this.OpenDocumentAsync);
        #endregion


        #region Methods
        private async Task OpenDocumentAsync()
        {
            switch (this.Source)
            {
                case Common.Models.Source.Internal:
                    break;
                case Common.Models.Source.External:
                    MainViewModel.GetInstance().ExternalDocument = new ExternalDocumentViewModel((Document)this);
                    await App.Navigator.PushAsync(new ExternalDocumentPage() { Title = this.RegistrationCode }, true);
                    break;
                default:
                    break;
            }
        }
        #endregion
    }
}
