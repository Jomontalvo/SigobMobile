namespace SigobMobile.ViewModels
{
    using System;
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
            //Assign global variable Url API
            App.UrlBaseApiSigob = this.UrlApiService;

            //Come back to Login
            var login = MainViewModel.GetInstance().Login;
            login.Institution = $"{this.Institution} ({this.ISOCountryCode})";
            await Application.Current.MainPage.Navigation.PopAsync() ;
        }
        #endregion
    }
}
