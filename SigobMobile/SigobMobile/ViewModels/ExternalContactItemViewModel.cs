namespace SigobMobile.ViewModels
{
    using System.Threading.Tasks;
    using AsyncAwaitBestPractices.MVVM;
    using Models;
    using Xamarin.Forms;

    public class ExternalContactItemViewModel : Participant
    {
        #region Commands
        public IAsyncCommand SelectMobileContactCommand => new AsyncCommand(SelectMobileContact);

        /// <summary>
        /// Selects the institution an set global url API service.
        /// </summary>
        private async Task SelectMobileContact()
        {
            await Application.Current.MainPage.DisplayAlert("Selected contact", "Sure add a new external contact?", "Ok");
            ////Assign API URL global variable and Settings
            //App.UrlBaseApiSigob =
            //    (this.UrlApiService.TrimEnd().Last() != '/') ? $"{this.UrlApiService.TrimEnd()}/" : this.UrlApiService;
            //Settings.UrlBaseApiSigob = App.UrlBaseApiSigob;
            ////Come back to Login
            //var login = MainViewModel.GetInstance().Login;
            //login.Institution = $"{this.Institution} ({this.ISOCountryCode})";
            //login.IsSelectedColor = Color.Black;
            //await Application.Current.MainPage.Navigation.PopAsync();
        }
        #endregion
    }
}