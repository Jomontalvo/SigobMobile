namespace SigobMobile.ViewModels
{
    using System.Threading.Tasks;
    using AsyncAwaitBestPractices.MVVM;
    using Common.Models;
    using SigobMobile.Helpers;
    using Views.Common;
    using Views.Correspondence;
    using Views.ManagementCenter;
    using Views.Tasks;
    using Views.WorkFlow;
    using Xamarin.Forms;
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
                    await App.Navigator.PushAsync(
                        new CalendarPage()
                        { Title = ( Device.RuntimePlatform != Device.Android)
                            ? string.Empty
                            : Languages.MasterPageTitle },
                        true);
                    break;
                case TypeApplication.Tasks:
                    appViewModel.TaskDashboard = new TaskDashboardViewModel();
                    await App.Navigator.PushAsync(new TaskDashboardPage(), true);
                    break;
                case TypeApplication.Correspondence:
                    if (this.IsVisible)
                    {
                        appViewModel.MailBoxes = new MailBoxesViewModel();
                        await App.Navigator.PushAsync(new MailBoxesPage(), true);
                    }
                    break;
                case TypeApplication.Goals:
                    appViewModel.UrlViewer = new UrlViewerViewModel("https://www.sigob.org/AR/CHACO/pres/pages/metas");
                    await App.Navigator.PushAsync(new UrlViewerPage() { Title = Languages.InstitutionalGoalsAppName }, true);
                    break;
                case TypeApplication.WorkFlows:
                    appViewModel.Workflows = new WorkflowsViewModel();
                    await App.Navigator.PushAsync(new WorkflowsPage(), true);
                    break;
                case TypeApplication. Communications:
                    appViewModel.UrlViewer = new UrlViewerViewModel("https://www.sigob.org/AR/CHACO/pres/pages/monitoreo");
                    await App.Navigator.PushAsync(new UrlViewerPage() { Title = Languages.CommunicationsActionsAppName }, true);
                    break;
            }
        }
        #endregion
    }
}
