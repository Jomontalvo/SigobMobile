namespace SigobMobile.ViewModels
{
    using Models;
    using Common.Services;
    using Helpers;
    using SigobMobile.Interfaces;
    using System;
    using System.Threading.Tasks;
    using SigobMobile.Common.Helpers;

    public class TaskViewModel : BaseViewModel
    {
        #region Services
        internal ApiService apiService;
        internal string ApiTaskController => $"tasks";
        #endregion

        #region Attributes
        private readonly int id;
        private bool isRunning;
        private TaskSigob localTask;
        #endregion

        #region Properties
        public bool IsRunning
        {
            get { return this.isRunning; }
            set { SetValue(ref this.isRunning, value); }
        }

        public TaskSigob LocalTask
        {
            get { return this.localTask; }
            set { SetValue(ref this.localTask, value); }
        }

        #endregion

        #region Constructors
        public TaskViewModel(int id)
        {
            this.apiService = new ApiService();
            this.id = id;
            this.LoadTaskDetails();
        }
        #endregion

        #region Methods
        private void LoadTaskDetails()
        {
            IsRunning = true;
            IErrorHandler errorHandler = null;
            this.GetTaskDetailAsync().FireAndForgetSafeAsync(errorHandler);
            IsRunning = false;
        }

        /// <summary>
        /// Get detail task.
        /// </summary>
        /// <returns></returns>
        private async Task GetTaskDetailAsync()
        {
            var connection = await this.apiService.CheckConnection();
            if (!connection.IsSuccess)
            {
                this.IsRunning = false;
                await App.Navigator.DisplayAlert(
                    Languages.Error,
                    connection.Message,
                    Languages.Cancel);
                await App.Navigator.PopToRootAsync();
                return;
            }

            var response = await this.apiService.Get<TaskSigob>(
                Settings.UrlBaseApiSigob,
                App.PrefixApiSigob,
                this.ApiTaskController,
                Settings.Token,
                Settings.DbToken,
                id
            );
            if (!response.IsSuccess)
            {
                this.IsRunning = false;
                await App.Navigator.DisplayAlert(
                    Languages.Error,
                    response.Message,
                    Languages.Cancel);
                await App.Navigator.PopAsync();
                return;
            }
            this.LocalTask = (TaskSigob)response.Result;
        }
        #endregion
    }
}
