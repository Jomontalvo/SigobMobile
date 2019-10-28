namespace SigobMobile.ViewModels
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Threading.Tasks;
    using Helpers;
    using Interfaces;
    using Models;
    using Services;
    using Xamarin.Forms;

    public class TaskBoardViewModel : BaseViewModel
    {
        #region Services
        internal ApiService apiService;
        #endregion

        #region ApiControllers
        internal string apiGetTaskProfile = "tasks/profile";
        internal string apiGetTaskStatistics = "tasks/agenda/{0}/option/{1}";
        #endregion

        #region Attributes
        private ObservableCollection<TaskSigob> taskList;
        private List<Tasks> taskObject;
        private bool isRunning;
        #endregion

        #region Properties
        public ObservableCollection<TaskSigob> TaskList
        {
            get { return this.taskList; }
            set { SetValue(ref this.taskList, value); }
        }
        public bool IsRunning
        {
            get { return this.isRunning; }
            set { SetValue(ref this.isRunning, value); }
        }

        #endregion

        #region Constructors
        public TaskBoardViewModel()
        {
            this.apiService = new ApiService();
            this.LoadTaskBoard();
        }
        #endregion

        #region Methods
        public void LoadTaskBoard()
        {
            IErrorHandler errorHandler = null;
            GetStatisticsAndTaskList().FireAndForgetSafeAsync(errorHandler);
        }

        /// <summary>
        /// Get Graph and Task List according user profile
        /// </summary>
        /// <returns></returns>
        private async Task GetStatisticsAndTaskList()
        {
            this.IsRunning = true;
            var connection = await this.apiService.CheckConnection();
            if (!connection.IsSuccess)
            {
                IsRunning = false;
                await Application.Current.MainPage.DisplayAlert(
                    Languages.Error,
                    connection.Message,
                    Languages.Cancel);
                await Application.Current.MainPage.Navigation.PopAsync();
                return;
            }
            var response = await this.apiService.GetList<Tasks>(
                Settings.UrlBaseApiSigob,
                App.PrefixApiSigob,
                string.Format(this.apiGetTaskStatistics, Settings.OfficeCode,10),
                Settings.Token,
                Settings.DbToken
            );
            if (!response.IsSuccess)
            {
                this.IsRunning = false;
                await Application.Current.MainPage.DisplayAlert(
                    Languages.Error,
                    response.Message,
                    Languages.Cancel);
                return;
            }
            this.taskObject = (List<Tasks>)response.Result;
            this.TaskList = new ObservableCollection<TaskSigob>(taskObject[0].TaskList);
            this.IsRunning = false;
        }
        #endregion

        #region Commands

        #endregion


    }
}
