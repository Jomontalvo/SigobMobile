namespace SigobMobile.ViewModels
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Windows.Input;
    using AsyncAwaitBestPractices.MVVM;
    using Common.Helpers;
    using Common.Models;
    using Common.Services;
    using GalaSoft.MvvmLight.Command;
    using Helpers;
    using Interfaces;
    using Views.Tasks;
    using EventTask = Models.EventTask;

    public class EventTasksViewModel : BaseViewModel
    {
        #region Services
        internal ApiService apiService;
        internal string ApiTaskController => $"tasks/cg/{mngCenterId}/event/{eventId}/authlevel/{(int)auth}/timeline/{(int)tick}";
        internal string ApiTaskAuthAddController => $"cg/{mngCenterId}/authorizedtimeline";
        #endregion

        #region Attributes
        private bool isRefreshing;
        private bool isVisibleSearch;
        private string filter;
        private int selectedIndex = -1;
        private readonly int mngCenterId;
        private readonly int eventId;
        private EventTasksAttribute auth;
        private EventTaskMoment tick;
        private List<EventTask> taskList;
        private ObservableCollection<string> segmentedControlItems;
        private ObservableCollection<EventTasksItemViewModel> taskItems;
        #endregion

        #region Properties

        internal bool IsLoadSegments { get; set; }

        public ObservableCollection<string> SegmentedControlItems
        {
            get => this.segmentedControlItems;
            set => SetValue(ref this.segmentedControlItems, value);
        }

        public bool IsRefreshing
        {
            get => this.isRefreshing;
            set => SetValue(ref this.isRefreshing, value);
        }

        public bool IsVisibleSearch
        {
            get => this.isVisibleSearch;
            set => SetValue(ref this.isVisibleSearch, value);
        }

        public List<Segment> TaskStatus { get; set; }

        public string Filter
        {
            get => this.filter;
            set
            {
                SetValue(ref this.filter, value);
                this.RefreshTaskList();
            }
        }

        public string EventTitle { get; set; }

        public string TotalTask { get; set; }

        public ObservableCollection<EventTasksItemViewModel> TaskItems
        {
            get => this.taskItems;
            set => SetValue(ref this.taskItems, value);
        }

        public List<EventTask> FilteredTaskList { get; private set; }

        public int SelectedIndex
        {
            get { return this.selectedIndex; }
            set
            {
                SetValue(ref this.selectedIndex, value);
                if (!IsRefreshing)
                { 
                    IErrorHandler errorHandler = null;
                    this.OnSelectionChanged().FireAndForgetSafeAsync(errorHandler);
                }
            }
        }
        #endregion

        #region Constructor
        public EventTasksViewModel(int mngCenterId, int eventId, EventTasksAttribute auth, EventTaskMoment tick)
        {
            this.apiService = new ApiService();
            this.IsVisibleSearch = false;
            this.mngCenterId = mngCenterId;
            this.eventId = eventId;
            this.auth = auth;
            this.tick = tick;
            this.LoadSegments();
            this.SelectedIndex = 0;
        }
        #endregion

        #region Commands
        public IAsyncCommand AddTaskCommand => new AsyncCommand(AddTaskAsync);
        public ICommand RefreshCommand => new RelayCommand(LoadEventTasks);
        public ICommand SearchCommand => new RelayCommand(RefreshTaskList);
        public ICommand SearchEnabledCommand => new RelayCommand(ActiveSearchBar);
        #endregion

        #region Methods
        /// <summary>
        /// Load Segments of Task Moment Types
        /// </summary>
        private void LoadSegments()
        {
            this.IsLoadSegments = true;
            int i = 0;
            TaskStatus = new List<Segment>
            {
                new Segment() { Id = i++, QueryId = 3, SegmentName = Languages.AllStatus }
            };
            var parentViewModel = MainViewModel.GetInstance().EventCg;
            if (parentViewModel.LocalEvent.PreviousTask != EventTasksAttribute.NotAuthorized)
                TaskStatus.Add(new Segment() {
                    Id = i++,
                    QueryId = 0,
                    SegmentName = string.IsNullOrEmpty(parentViewModel.LocalEvent.PreviousTaskTitle)
                    ? Languages.TaskPrevious
                    : parentViewModel.LocalEvent.PreviousTaskTitle
                });
            if (parentViewModel.LocalEvent.SupportTasks != EventTasksAttribute.NotAuthorized)
                TaskStatus.Add(new Segment() {
                    Id = i++,
                    QueryId = 1,
                    SegmentName = string.IsNullOrEmpty(parentViewModel.LocalEvent.SupportTaskTitle)
                    ? Languages.TaskSupport
                    : parentViewModel.LocalEvent.SupportTaskTitle});
            if (parentViewModel.LocalEvent.LaterTasks != EventTasksAttribute.NotAuthorized)
                TaskStatus.Add(new Segment() {
                    Id = i++,
                    QueryId = 2,
                    SegmentName = string.IsNullOrEmpty(parentViewModel.LocalEvent.CommitmentTaskTitle)
                    ? Languages.TaskCommitments
                    : parentViewModel.LocalEvent.CommitmentTaskTitle});
            //Built ObservableCollection<string> with Segments Names
            this.SegmentedControlItems = new ObservableCollection<string>(ToSegmentString());
            //Get information to StatusBar
            this.EventTitle = $"- {parentViewModel.LocalEvent.Title}";
            this.TotalTask = $"{Languages.TotalRows}: {parentViewModel.LocalEvent.TaskAccount} {Languages.Tasks}";
            this.IsLoadSegments = false;
        }

        /// <summary>
        /// Create Segments
        /// </summary>
        /// <returns></returns>
        private IEnumerable<string> ToSegmentString()
        {
            return this.TaskStatus.Select(s => s.SegmentName);
        }

        /// <summary>
        /// Main load of Event Task List
        /// </summary>
        private void LoadEventTasks()
        {
            this.IsRefreshing = true;
            IErrorHandler errorHandler = null;
            this.GetEventTasksAsync().FireAndForgetSafeAsync(errorHandler);
            this.IsRefreshing = false;
        }

        /// <summary>
        /// Active Search bar
        /// </summary>
        private void ActiveSearchBar()
        {
            this.IsVisibleSearch = !this.IsVisibleSearch;
        }


        /// <summary>
        /// Add New Event Task
        /// </summary>
        /// <returns></returns>
        private async Task AddTaskAsync()
        {
            var result = await App.Navigator.DisplayActionSheet(
                $"{Languages.Add} {Languages.Task}",
                Languages.Cancel,
                null,
                new string[] { "Tarea Previa", "Tarea Logística", "Compromiso" });
            if (result != Languages.Cancel)
            {
                var mainViewModel = MainViewModel.GetInstance();
                mainViewModel.EditTask = new EditTaskViewModel();
                await App.Navigator.PushAsync(new TaskPage());
            }
        }

        /// <summary>
        /// Get Task List of Management Center Event
        /// </summary>
        /// <returns></returns>
        private async Task GetEventTasksAsync()
        {
            var connection = await this.apiService.CheckConnection();
            if (!connection.IsSuccess)
            {
                this.IsRefreshing = false;
                await App.Navigator.DisplayAlert(
                    Languages.Error,
                    connection.Message,
                    Languages.Cancel);
                await App.Navigator.PopToRootAsync();
                return;
            }

            var response = await this.apiService.GetList<EventTask>(
                Settings.UrlBaseApiSigob,
                App.PrefixApiSigob,
                this.ApiTaskController,
                Settings.Token,
                Settings.DbToken
            );
            if (!response.IsSuccess)
            {
                this.IsRefreshing = false;
                await App.Navigator.DisplayAlert(
                    Languages.Error,
                    response.Message,
                    Languages.Cancel);
                await App.Navigator.PopAsync();
                return;
            }

            this.taskList = (List<EventTask>)response.Result;
            this.RefreshTaskList();            
        }

        /// <summary>
        /// Refresh Task List
        /// </summary>
        private void RefreshTaskList()
        {
            if (string.IsNullOrEmpty(this.Filter))
            {
                this.TaskItems = new ObservableCollection<EventTasksItemViewModel>(taskList.Select(t => new EventTasksItemViewModel
                {
                    Title = t.Title,
                    Description = t.Description,
                    IsExternalResponsible = t.IsExternalResponsible,
                    LastReportUpdate = t.LastReportUpdate,
                    Report = t.Report,
                    EndProgrammedDate = t.EndProgrammedDate,
                    HasReport = t.HasReport,
                    Id = t.Id,
                    IsOwner = t.IsOwner,
                    MonitorName = t.MonitorName,
                    MonitorOfficeId = t.MonitorOfficeId,
                    NextReportDate = t.NextReportDate,
                    PercentageOfCompletion = IsNumeric(t.PercentageOfCompletion)
                            ? $"{t.PercentageOfCompletion}%"
                            : t.PercentageOfCompletion,
                    Periodicity = t.Periodicity,
                    ProgrammerName = t.ProgrammerName,
                    Reiterations = t.Reiterations,
                    ReportStatus = t.ReportStatus,
                    ResponsibleName = t.ResponsibleName,
                    RevisedReport = t.RevisedReport,
                    Status = t.Status,
                    StatusDescription = t.StatusDescription,
                    TaskMomentId = t.TaskMomentId,
                    TimeManagementStatus = t.TimeManagementStatus,
                    Type = t.Type,
                    TypeBlue = t.TypeBlue,
                    TypeGreen = t.TypeGreen,
                    TypeRed = t.TypeRed
                })
            .OrderByDescending(t => t.TrafficLight)
            .ToList());
            }
            else
            {
                this.TaskItems = new ObservableCollection<EventTasksItemViewModel>(taskList.Select(t => new EventTasksItemViewModel
                {
                    Title = t.Title,
                    Description = t.Description,
                    IsExternalResponsible = t.IsExternalResponsible,
                    LastReportUpdate = t.LastReportUpdate,
                    Report = t.Report,
                    EndProgrammedDate = t.EndProgrammedDate,
                    HasReport = t.HasReport,
                    Id = t.Id,
                    IsOwner = t.IsOwner,
                    MonitorName = t.MonitorName,
                    MonitorOfficeId = t.MonitorOfficeId,
                    NextReportDate = t.NextReportDate,
                    PercentageOfCompletion = IsNumeric(t.PercentageOfCompletion)
                            ? $"{t.PercentageOfCompletion}%"
                            : t.PercentageOfCompletion,
                    Periodicity = t.Periodicity,
                    ProgrammerName = t.ProgrammerName,
                    Reiterations = t.Reiterations,
                    ReportStatus = t.ReportStatus,
                    ResponsibleName = t.ResponsibleName,
                    RevisedReport = t.RevisedReport,
                    Status = t.Status,
                    StatusDescription = t.StatusDescription,
                    TaskMomentId = t.TaskMomentId,
                    TimeManagementStatus = t.TimeManagementStatus,
                    Type = t.Type,
                    TypeBlue = t.TypeBlue,
                    TypeGreen = t.TypeGreen,
                    TypeRed = t.TypeRed
                })
            .Where (t => (!string.IsNullOrEmpty(t.Title) && t.Title.ToLower().Contains(this.Filter.ToLower())) ||
                         (!string.IsNullOrEmpty(t.Description) && t.Description.ToLower().Contains(this.Filter.ToLower())) ||
                         (!string.IsNullOrEmpty(t.ResponsibleName) && t.ResponsibleName.ToLower().Contains(this.Filter.ToLower())) ||
                         (!string.IsNullOrEmpty(t.Report) && t.Report.ToLower().Contains(this.Filter.ToLower())) ||
                         (!string.IsNullOrEmpty(t.MonitorName) && t.MonitorName.ToLower().Contains(this.Filter.ToLower())))
            .OrderByDescending(t => t.TrafficLight)
            .ToList());
            }
        }

        /// <summary>
        /// Event ocurrs when the selection is changed.
        /// </summary>
        private async Task OnSelectionChanged()
        {
            this.IsRefreshing = true;
            int queryId = this.TaskStatus.FirstOrDefault(s => s.Id == SelectedIndex).QueryId;
            var localEvent = MainViewModel.GetInstance().EventCg.LocalEvent;
            switch (queryId)
            {
                case 0:
                    this.tick = EventTaskMoment.Previous;
                    this.auth = localEvent.PreviousTask;
                    break;
                case 1:
                    this.tick = EventTaskMoment.Support;
                    this.auth = localEvent.SupportTasks;
                    break;
                case 2:
                    this.tick = EventTaskMoment.Later;
                    this.auth = localEvent.LaterTasks;
                    break;
                case 3:
                    this.tick = EventTaskMoment.All;
                    this.auth = EventTasksAttribute.ViewAll;
                    break;
                default:
                    break;
            }
            await this.GetEventTasksAsync();
            this.IsRefreshing = false;
        }

        /// <summary>
        /// Check the inpot is valid integer number
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsNumeric(string value)
        {
            return int.TryParse(value, out int retNum);
        }
        #endregion

    }
}
