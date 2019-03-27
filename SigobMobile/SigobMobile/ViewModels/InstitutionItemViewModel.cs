namespace SigobMobile.ViewModels
{
    using System.Linq;
    using System.Windows.Input;
    using GalaSoft.MvvmLight.Command;
    using Models;
    using Helpers;
    using Xamarin.Forms;

    /// <summary>
    /// Institution to connect item view model.
    /// </summary>
    public class InstitutionItemViewModel : InstitutionConnect
    {
        #region Commands
        public ICommand SelectInstitutionCommand => new RelayCommand(SelectInstitution);

        /// <summary>
        /// Selects the institution an set global url API service.
        /// </summary>
        private async void SelectInstitution()
        {
            //Assign API URL global variable and Settings
            App.UrlBaseApiSigob = 
                (this.UrlApiService.TrimEnd().Last() != '/') ? $"{this.UrlApiService.TrimEnd()}/" : this.UrlApiService;
            Settings.UrlBaseApiSigob = App.UrlBaseApiSigob;
            //Come back to Login
            var login = MainViewModel.GetInstance().Login;
            login.Institution = $"{this.Institution} ({this.ISOCountryCode})";
            login.IsSelectedColor = Color.Black;
            await Application.Current.MainPage.Navigation.PopAsync() ;
        }
        #endregion
    }
}
