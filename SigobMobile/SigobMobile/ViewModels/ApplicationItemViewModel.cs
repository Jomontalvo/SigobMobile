namespace SigobMobile.ViewModels
{
    using System.Threading.Tasks;
    using AsyncAwaitBestPractices.MVVM;
    using Models;
    using Views.ManagementCenter;
    using Views.Tasks;

    public class ApplicationItemViewModel : ApplicationMenuItem
    {
        #region Async Commands
        public IAsyncCommand SelectApplicationCommand => new AsyncCommand(SelectApplicationAsync);

        /// <summary>
        /// Select the app to navigate
        /// </summary>
        private async Task SelectApplicationAsync()
        {
            // 1. GetInstance of View Model
            var appViewModel = MainViewModel.GetInstance();
            switch (this.TypeApplication)
            {
                case TypeApplication.ManagementCenter:
                    appViewModel.Calendar = new CalendarViewModel();
                    await App.Navigator.PushAsync(new CalendarPage(),true);
                    break;
                case TypeApplication.Tasks:
                    appViewModel.TaskBoard = new TaskBoardViewModel();
                    await App.Navigator.PushAsync(new TaskBoardPage(), true);
                    break;
                case TypeApplication.Correspondence:
                    break;
                case TypeApplication.WorkFlows:
                    break;
                case TypeApplication.Communications:
                    break;
            }
        }
        #endregion
    }
}
