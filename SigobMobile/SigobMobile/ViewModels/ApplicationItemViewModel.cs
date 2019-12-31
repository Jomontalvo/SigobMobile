namespace SigobMobile.ViewModels
{
    using System.Threading.Tasks;
    using AsyncAwaitBestPractices.MVVM;
    using Common.Models;
    using Views.Correspondence;
    using Views.ManagementCenter;
    using Views.Tasks;
    using ApplicationMenuItem = Models.ApplicationMenuItem;

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
                    appViewModel.TaskDashboard = new TaskDashboardViewModel();
                    await App.Navigator.PushAsync(new TaskDashboardPage(), true);
                    break;
                case TypeApplication.Correspondence:
                    appViewModel.MailBoxes = new MailBoxesViewModel();
                    await App.Navigator.PushAsync(new MailBoxesPage(), true);
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
