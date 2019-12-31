namespace SigobMobile.ViewModels
{
    using System.Threading.Tasks;
    using AsyncAwaitBestPractices.MVVM;
    using SigobMobile.Common.Helpers;
    using SigobMobile.Common.Models;
    using SigobMobile.Views.Correspondence;

    public class MailBoxMenuItemViewModel : MailBoxMenu
    {
        #region Extended properties
        public bool IsSelected { get; set; }
        #endregion

        #region Commands
        public IAsyncCommand SelectMailBoxCommand => new AsyncCommand(SelectMailBoxAsync);

        #endregion

        #region Command Methods
        private async Task SelectMailBoxAsync()
        {
            Settings.DocumentTraySelected = Id;
            this.IsSelected = true;
            // 1.GetInstance of View Model
            var appViewModel = MainViewModel.GetInstance();
            switch (this.Type)
            {
                case TypeMenuBox.AppType:
                    appViewModel.DocumentsTray = new DocumentsTrayViewModel(Id);
                    await App.Navigator.PushAsync(new DocumentsTrayPage() { Title = this.Name }, true);
                    break;
                case TypeMenuBox.TagType:
                    appViewModel.DocumentsTray = new DocumentsTrayViewModel();
                    await App.Navigator.PushAsync(new DocumentsTrayPage() { Title = this.Name}, true);
                    break;
            }
            await appViewModel.MailBoxes.LoadTraysAsync();
            return;
        }
        #endregion
    }
}