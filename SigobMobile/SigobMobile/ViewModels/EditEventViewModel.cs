namespace SigobMobile.ViewModels
{
    using System.Windows.Input;
    using GalaSoft.MvvmLight.Command;
    using Models;
    using Xamarin.Forms;

    public class EditEventViewModel : BaseViewModel
    {
        #region Constructors
        public EditEventViewModel(ManagementCenterEvent localEvent)
        {

        }
        #endregion

        #region Commands
        public ICommand CancelEditCommand => new RelayCommand(CancelEdit);

        #endregion

        #region Methods
        private async void CancelEdit()
        {
            await Application.Current.MainPage.Navigation.PopModalAsync();
        }
        #endregion
    }
}
