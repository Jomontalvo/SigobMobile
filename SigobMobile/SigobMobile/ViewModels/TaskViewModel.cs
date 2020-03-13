namespace SigobMobile.ViewModels
{
    using System.Threading.Tasks;
    using AsyncAwaitBestPractices.MVVM;
    using Common.Services;
    using Helpers;
    using SigobMobile.Common.Models;
    using SigobMobile.Views.Common;
    using Xamarin.Forms;
    using TaskSigob = Models.TaskSigob;

    public class TaskViewModel : BaseViewModel
    {
        #region Services
        internal ApiService apiService;
        #endregion

        #region Attributes
        private readonly bool isCalledFromEvent;
        private bool isRunning;
        private bool isParentVisible;
        private string lastReportText;
        private string reportStatusText;
        private string auditStatusText;
        private string trimDescription;
        private string taskOrigin;
        private TaskSigob localTask;
        private ImageSource iconParent;
        #endregion

        #region Properties
        public bool IsRunning
        {
            get { return this.isRunning; }
            set { SetValue(ref this.isRunning, value); }
        }
        public bool IsParentVisible
        {
            get { return this.isParentVisible; }
            set
            {
                if (!this.isCalledFromEvent) SetValue(ref this.isParentVisible, value);
                else SetValue(ref this.isParentVisible, false); ;
            }
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

        public string ReportStatusText
        {
            get { return this.reportStatusText; }
            set { SetValue(ref this.reportStatusText, value); }
        }

        public string TrimDescription
        {
            get { return this.trimDescription; }
            set { SetValue(ref this.trimDescription, value); }
        }

        public string TaskOrigin
        {
            get { return this.taskOrigin; }
            set { SetValue(ref this.taskOrigin, value); }
        }

        public string AuditStatusText
        {
            get { return this.auditStatusText; }
            set { SetValue(ref this.auditStatusText, value); }
        }

        public ImageSource IconParent
        {
            get { return this.iconParent; }
            set { SetValue(ref this.iconParent, value); }
        }
        #endregion

        #region Constructor
        /// <summary>
        /// Task is called from Control Task View
        /// </summary>
        /// <param name="task"></param>
        public TaskViewModel (TaskSigob task, bool isCalledFromEvent)
        {
            this.apiService = new ApiService();
            this.LocalTask = task;
            this.isCalledFromEvent = isCalledFromEvent;
            this.LastReportText = string.IsNullOrEmpty(this.LocalTask.Report)
                ? Languages.NoReportAvailable
                : this.LocalTask.Report;
            this.SetExtendedTaskInfo();
        }
        #endregion

        #region Commands
        public IAsyncCommand GetDocumentsCommand => new AsyncCommand(GetDocumentsAsync);
        #endregion

        #region Methods


        private void SetExtendedTaskInfo()
        {
            this.TrimDescription = this.LocalTask.Description.TrimEnd();
            this.ReportStatusText = LocalTask.ReportStatus switch
            {
                TReportStatus.NotApply => Languages.ReportStatusNotApply,
                TReportStatus.OnTime => Languages.ReportStatusOnTime,
                TReportStatus.Overdue => Languages.ReportStatusOverdue,
                _ => Languages.ReportStatusNotApply
            };
            this.AuditStatusText = LocalTask.AuditStatus switch
            {
                TAuditStatus.NoRequested => Languages.AuditNoRequested,
                TAuditStatus.ToElaborate => Languages.AuditToElaborate,
                TAuditStatus.InElaboration => Languages.AuditInElaboration,
                TAuditStatus.Finished => Languages.AuditFinished,
                TAuditStatus.Reviewed => Languages.AuditReviewed,
                _ => Languages.AuditNoRequested
            };

            switch (LocalTask.Instrument)
            {
                case SigobInstrument.EventTask:
                    this.IconParent = ImageSource.FromFile("ic_task_source_calendar");
                    this.TaskOrigin = $"{Languages.TaskOrigin}: {Languages.Event}";
                    this.IsParentVisible = true;
                    break;
                case SigobInstrument.RequestTask:
                    this.IconParent = ImageSource.FromFile("ic_task_source_task");
                    this.TaskOrigin = $"{Languages.TaskOrigin}: {Languages.Request}";
                    this.IsParentVisible = true;
                    break;
                case SigobInstrument.InstructionTask:
                    this.IconParent = ImageSource.FromFile("ic_task_source_instruction");
                    this.TaskOrigin = $"{Languages.TaskOrigin}: {Languages.Instruction}";
                    this.IsParentVisible = true;
                    break;
                case SigobInstrument.Assignment:
                    this.IconParent = ImageSource.FromFile("ic_task_source_assignment");
                    this.TaskOrigin = $"{Languages.TaskOrigin}: {Languages.Assignment}";
                    this.IsParentVisible = false;
                    break;
                case SigobInstrument.CommunicativeActionTask:
                    this.IconParent = ImageSource.FromFile("ic_task_source_acom");
                    this.TaskOrigin = $"{Languages.TaskOrigin}: {Languages.CommunicationsActionsAppName}";
                    this.IsParentVisible = true;
                    break;
                case SigobInstrument.GapTask:
                    this.IconParent = ImageSource.FromFile("ic_task_source_goals");
                    this.TaskOrigin = $"{Languages.TaskOrigin}: {Languages.InstitutionalGoalsAppName}"; //TODO: Change by PAG Resource
                    this.IsParentVisible = true;
                    break;
                case SigobInstrument.Task:
                    this.IconParent = ImageSource.FromFile("ic_task_source_agenda");
                    this.TaskOrigin = $"{Languages.TaskOrigin}: {Languages.PersonalAgendas}";
                    this.IsParentVisible = false;
                    break;
                case SigobInstrument.Activity:
                    this.IconParent = ImageSource.FromFile("ic_task_source_workflow");
                    this.TaskOrigin = $"{Languages.TaskOrigin}: {Languages.Activity}";
                    this.IsParentVisible = true;
                    break;
                default:
                    this.IconParent = ImageSource.FromFile("ic_task_source_task");
                    this.TaskOrigin = $"{Languages.TaskOrigin}: {Languages.Task}";
                    this.IsParentVisible = false;
                    break;
            }
        }

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
        #endregion
    }
}
