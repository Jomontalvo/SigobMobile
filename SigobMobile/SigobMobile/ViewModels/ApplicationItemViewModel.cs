namespace SigobMobile.ViewModels
{
    using System.Windows.Input;
    using GalaSoft.MvvmLight.Command;
    using Models;
    using Views.ManagementCenter;
    using Views;
    using Xamarin.Forms;

    public class ApplicationItemViewModel : ApplicationMenuItem
    {
        #region Commands
        public ICommand SelectApplicationCommand
        {
            get
            {
                return new RelayCommand(SelectApplication);
            }
        }

        /// <summary>
        /// Select the app to navigate
        /// </summary>
        private async void SelectApplication()
        {
            // 1. GetInstance of View Model
            var appViewModel = MainViewModel.GetInstance();
            switch (this.TypeApplication)
            {
                case TypeApplication.ManagementCenter:
                    appViewModel.Profile = new ProfileViewModel();
                    await App.Navigator.PushAsync(new ProfilePage());
                    break;
                case TypeApplication.Tasks:
                    break;
                case TypeApplication.Correspondence:
                    break;
                case TypeApplication.WorkFlows:
                    break;
                case TypeApplication.Communications:
                    break;
                default:
                    break;
            }
        }
        #endregion
    }
}
