namespace SigobMobile.ViewModels
{
    using System;
    using System.Linq;
    using System.Windows.Input;
    using GalaSoft.MvvmLight.Command;
    using SigobMobile.Models;
    using Xamarin.Forms;

    /// <summary>
    /// Institution to connect item view model.
    /// </summary>
    public class InstitutionItemViewModel : InstitutionConnect
    {
        #region Commands
        public ICommand SelectInstitutionCommand
        {
            get
            {
                return new RelayCommand(SelectInstitution);
            }
        }

        /// <summary>
        /// Selects the institution an set global url API service.
        /// </summary>
        private async void SelectInstitution()
        {
            //Assign API URL global variable 
            App.UrlBaseApiSigob = 
                (this.UrlApiService.TrimEnd().Last() != '/') ? $"{this.UrlApiService.TrimEnd()}/" : this.UrlApiService;
            //Come back to Login
            var login = MainViewModel.GetInstance().Login;
            login.Institution = $"{this.Institution} ({this.ISOCountryCode})";
            await Application.Current.MainPage.Navigation.PopAsync() ;
        }
        #endregion
    }
}
