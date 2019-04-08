namespace SigobMobile.ViewModels
{
    using System.Threading.Tasks;
    using System.Windows.Input;
    using Models;
    using Helpers;
    using Views.ManagementCenter;

    public class ApplicationItemViewModel : ApplicationMenuItem
    {
        #region Commands
        public ICommand SelectApplicationCommand
        {
            get
            {
                return new AsyncCommand(SelectApplication);
            }
        }

        /// <summary>
        /// Select the app to navigate
        /// </summary>
        private async Task SelectApplication()
        {
            // 1. GetInstance of View Model
            var appViewModel = MainViewModel.GetInstance();
            switch (this.TypeApplication)
            {
                case TypeApplication.ManagementCenter:
                    appViewModel.Calendar = new CalendarViewModel();
                    await App.Navigator.PushAsync(new CalendarPage());
                    break;
                case TypeApplication.Tasks:
                    appViewModel.CalendarMonth = new CalendarMonthViewModel();
                    await App.Navigator.PushAsync(new CalendarMonthPage(), true);
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
            return;
        }
        #endregion
    }
}
