namespace SigobMobile.ViewModels
{
    using Models;
    using Common.Services;
    using Helpers;
    using SigobMobile.Interfaces;
    using System;
    using System.Threading.Tasks;
    using SigobMobile.Common.Helpers;
    using AsyncAwaitBestPractices.MVVM;
    using SigobMobile.Views.Common;
    using SigobMobile.Common.Models;
    using TaskSigob = Models.TaskSigob;
    using EventTask = Models.EventTask;

    public class TaskViewModel : BaseViewModel
    {
        #region Services
        internal ApiService apiService;
        internal string ApiTaskController => $"tasks";
        #endregion

        #region Attributes
        private readonly int id;
        private bool isRunning;
        private string lastReportText;
        private string trimDescription;
        private TaskSigob localTask;
        private EventTask localEventTask;

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

        public string LastReportText
        {
            get { return this.lastReportText; }
            set { SetValue(ref this.lastReportText, value); }
        }

        public string TrimDescription
        {
            get { return this.trimDescription; }
            set { SetValue(ref this.trimDescription, value); }
        }

        #endregion

        #region Constructors
        /// <summary>
        /// Initialize with Task Id
        /// </summary>
        /// <param name="id"></param>
        public TaskViewModel(int id)
        {
            this.apiService = new ApiService();
            this.id = id;
            this.LoadTaskDetails();
            this.LastReportText = string.IsNullOrEmpty(this.LocalTask.Report)
                ? Languages.NoReportAvailable
                : this.LocalTask.Report;
            this.TrimDescription = this.LocalTask.Description.TrimEnd();
        }

        /// <summary>
        /// Initialize from EventTask parameter (called from Management Center)
        /// </summary>
        /// <param name="eventTask">Objet type EventTask</param>
        public TaskViewModel(EventTask eventTask)
        {
            this.localEventTask = eventTask;
            this.LocalTask = new TaskSigob
            {
                Id = localEventTask.Id,
                Title = localEventTask.Title,
                Description = localEventTask.Description.TrimEnd(),
                EndProgrammedDate = localEventTask.EndProgrammedDate,
                Status = localEventTask.Status,
                ResponsibleName = localEventTask.ResponsibleName,
                Report = localEventTask.Report,
                ReportStatus = localEventTask.ReportStatus,
                RevisedReport = localEventTask.RevisedReport,
                Type = localEventTask.Type,
                ProgrammerName = localEventTask.ProgrammerName,
                NextReportDate = localEventTask.NextReportDate,
                MonitorName = localEventTask.MonitorName,
                ReportFrequency = localEventTask.Periodicity,
                ModificationReportDate = localEventTask.LastReportUpdate,
                Source = localEventTask.Source,
                Instrument = SigobInstrument.EventTask
            };
            this.LastReportText = string.IsNullOrEmpty(this.LocalTask.Report)
                ? Languages.NoReportAvailable
                : this.LocalTask.Report;
            this.TrimDescription = this.LocalTask.Description;
        }

        /// <summary>
        /// Task is called from Control Task View
        /// </summary>
        /// <param name="task"></param>
        public TaskViewModel (TaskSigob task)
        {
            this.LocalTask = task;
            this.LastReportText = string.IsNullOrEmpty(this.LocalTask.Report)
                ? Languages.NoReportAvailable
                : this.LocalTask.Report;
            this.TrimDescription = this.LocalTask.Description.TrimEnd();
        }
        #endregion

        #region Commands
        public IAsyncCommand GetDocumentsCommand => new AsyncCommand(GetDocumentsAsync);
        #endregion

        #region Methods

        /// <summary>
        /// Open attachments
        /// </summary>
        /// <returns></returns>
        private async Task GetDocumentsAsync()
        {
            DocumentSource senderSource = LocalTask.Source switch
            {
                TOriginTask.ManagementCenterPrevious => DocumentSource.MgCenterEventTask,
                TOriginTask.ManagementCenterDuring => DocumentSource.MgCenterEventTask,
                TOriginTask.ManagementCenterPost => DocumentSource.MgCenterEventTask,
                TOriginTask.Assignment => DocumentSource.MgCenterDirectTask,
                TOriginTask.Instruction => DocumentSource.MgCenterInstructionTask,
                TOriginTask.ManagementCenterRequest => DocumentSource.MgCenterRequestTask,
                TOriginTask.PersonalAgenda => DocumentSource.AgendaTask,
                TOriginTask.CommunicativeAction => DocumentSource.CummunicationalTask,
                TOriginTask.GovernmentActionProgram => DocumentSource.GoalTask,
                TOriginTask.Task => DocumentSource.AgendaTask,
                _ => DocumentSource.None
            };
            var mainViewModel = MainViewModel.GetInstance();
            mainViewModel.Attachments = new AttachmentsViewModel(
                this.LocalTask.Id,
                this.LocalTask.Instrument,
                senderSource);
            await App.Navigator.PushAsync(new AttachmentsPage() { Title = Languages.Attachments});
        }

        /// <summary>
        /// Get Task details (using API)
        /// </summary>
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
