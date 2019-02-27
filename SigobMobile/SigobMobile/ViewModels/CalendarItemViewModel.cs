namespace SigobMobile.ViewModels
{
    using System.Windows.Input;
    using GalaSoft.MvvmLight.Command;
    using Models;
    using PropertyChanged;
    using SigobMobile.Helpers;
    using Telerik.XamarinForms.Primitives.CheckBox.Commands;
    using Xamarin.Forms;

    [AddINotifyPropertyChangedInterface]
    public class CalendarItemViewModel : Calendar
    {
        #region Properties
        public bool IsOpen {  get; set; }
        #endregion

        #region Commands
        public ICommand IsCheckedChangedCommand => new Command<CheckBoxIsCheckChangedCommandContext>(this.CheckBoxChange);
        public ICommand OpenChangeColorCommand
        {
            get
            {
                return new RelayCommand(OpenChangeColor);
            }
        }

        public ICommand ChangeColorAndCloseCommand
        {
            get { return new RelayCommand(ChangeColorAndClose); }
        }
        #endregion

        #region Methods
        /// <summary>
        /// Execute when CheckBox is change.
        /// </summary>
        /// <param name="context">Context.</param>
        private async void CheckBoxChange(CheckBoxIsCheckChangedCommandContext context)
        {
            await Application.Current.MainPage.DisplayAlert(
                    Languages.Success,
                    $"{Languages.GeneralError} en el checkbox {context.ToString()} valor {context.NewState}",
                    Languages.Accept);
        }
        /// <summary>
        /// Open Modal Page with available colors
        /// </summary>
        private void OpenChangeColor()
        {
            IsOpen = true;
            //MainViewModel.GetInstance().Calendars.IsOpen = true;
        }

        /// <summary>
        /// Changes the color and close modal view
        /// </summary>
        private void ChangeColorAndClose()
        {
            IsOpen = false;
            //MainViewModel.GetInstance().Calendars.IsOpen = false;
        }
        #endregion
    }
}
