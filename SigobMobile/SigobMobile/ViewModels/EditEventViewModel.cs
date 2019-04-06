namespace SigobMobile.ViewModels
{
    using System.Windows.Input;
    using GalaSoft.MvvmLight.Command;
    using Models;
    using Xamarin.Forms;

    public class EditEventViewModel : BaseViewModel
    {
        #region Attributes
        private bool isNewEvent;
        #endregion


        #region Properties
        public bool IsNewEvent
        {
            get => this.isNewEvent;
            set => SetValue(ref this.isNewEvent, value);
        }
        #endregion

        #region Constructors
        public EditEventViewModel(ManagementCenterEvent localEvent)
        {
            IsNewEvent = false;
        }
        public EditEventViewModel()
        {
            IsNewEvent = true;
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
