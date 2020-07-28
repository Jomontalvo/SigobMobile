namespace SigobMobile.ViewModels
{
    using System;
    using System.Threading.Tasks;
    using System.Windows.Input;
    using AsyncAwaitBestPractices.MVVM;
    using Common.Models;
    using GalaSoft.MvvmLight.Command;

    public class TaskControlMenuItemViewModel : MenuOption
    {
        #region Commands
        public ICommand SelectMenuItemCommand => new RelayCommand(SelectMenuItemAsync);
        #endregion

        #region Methods
        private void SelectMenuItemAsync()
        {
            TaskDashboardViewModel vm = MainViewModel.GetInstance().TaskDashboard;
            vm.IsOpenControl = false;
        }
        #endregion
    }
}
