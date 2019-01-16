namespace SigobMobile.ViewModels
{
    using System.Windows.Input;
    using GalaSoft.MvvmLight.Command;
    using Models;
    using Xamarin.Forms;

    public class ApplicationItemViewModel : ApplicationMenuItem
    {
        #region Commands
        public ICommand SelectedApplicationCommand
        {
            get
            {
                return new RelayCommand(SelectedApplication);
            }
        }

        /// <summary>
        /// Selects the institution an set global url API service.
        /// </summary>
        private async void SelectedApplication()
        {
            //Navigate to especific Application Sigob Page
            //var login = MainViewModel.GetInstance().Login;
            //login.Institution = $"{this.Institution} ({this.ISOCountryCode})";
            await Application.Current.MainPage.Navigation.PopAsync();
        }
        #endregion
    }
}
