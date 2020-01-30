namespace SigobMobile.ViewModels
{
    using System;
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
        private int selectedIndex;
        private readonly int mngCenterId;
        private readonly int eventId;
        private readonly EventTasksAttribute auth;
        private readonly EventTaskMoment tick;
        private List<EventTask> taskList;
        private ObservableCollection<string> segmentedControlItems;
        private ObservableCollection<EventTasksItemViewModel> taskItems;
        #endregion

        #region Properties

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

        public ObservableCollection<Segment> TaskStatus { get; set; }

        public string Filter
        {
            get => this.filter;
            set
            {
                SetValue(ref this.filter, value);
                if (value == null)
                {
                    IErrorHandler errorHandler = null;
                    this.SearchTaskAsync().FireAndForgetSafeAsync(errorHandler);
                }
            }
        }

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
                IErrorHandler errorHandler = null;
                this.OnSelectionChanged().FireAndForgetSafeAsync(errorHandler);
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
            this.LoadEventTasks();
        }
        #endregion

        #region Commands
        public IAsyncCommand AddTaskCommand => new AsyncCommand(AddTaskAsync);
        public IAsyncCommand SearchCommand => new AsyncCommand(SearchTaskAsync);
        public ICommand SearchEnabledCommand => new RelayCommand(ActiveSearchBar);
        #endregion

        #region Methods
        /// <summary>
        /// Load Segments of Task Moment Types
        /// </summary>
        private void LoadSegments()
        {
            int i = 0;
            TaskStatus = new ObservableCollection<Segment>
            {
                new Segment() { Id = i, QueryId = 3, SegmentName = Languages.AllStatus }
            };
            var parentViewModel = MainViewModel.GetInstance().EventCg;
            if (parentViewModel.LocalEvent.PreviousTask != EventTasksAttribute.NotAuthorized)
                TaskStatus.Add(new Segment() { Id = i++, QueryId = 0, SegmentName = Languages.TaskPrevious });
            if (parentViewModel.LocalEvent.SupportTasks != EventTasksAttribute.NotAuthorized)
                TaskStatus.Add(new Segment() { Id = i++, QueryId = 1, SegmentName = Languages.TaskSupport });
            if (parentViewModel.LocalEvent.LaterTasks != EventTasksAttribute.NotAuthorized)
                TaskStatus.Add(new Segment() { Id = i++, QueryId = 2, SegmentName = Languages.TaskCommitments });
            //Built ObservableCollection<string> with Segments Names
            this.SegmentedControlItems = new ObservableCollection<string>(ToSegmentString());
        }

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

        private void ActiveSearchBar()
        {
            this.IsVisibleSearch = !this.IsVisibleSearch;
        }

        private async Task SearchTaskAsync()
        {
            throw new NotImplementedException();
        }

        private async Task AddTaskAsync()
        {
            var mainViewModel = MainViewModel.GetInstance();
            mainViewModel.EditTask = new EditTaskViewModel();
            await App.Navigator.PushAsync(new TaskPage());
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
                string.Format(this.ApiTaskController, this.SelectedIndex),
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
            this.FilteredTaskList = new List<EventTask>(this.taskList); // For search
            TaskItems = new ObservableCollection<EventTasksItemViewModel>(this.ToEvenTasksItemViewModel());
            
        }

        private IEnumerable<EventTasksItemViewModel> ToEvenTasksItemViewModel()
        {
            return taskList.Select( t => new EventTasksItemViewModel
            {
                Title = t.Title,
                EndProgrammedDate = t.EndProgrammedDate,
                HasReport = t.HasReport,
                Id = t.Id,
                IsOwner = t.IsOwner,
                MonitorName = t.MonitorName,
                MonitorOfficeId = t.MonitorOfficeId,
                NextReportDate = t.NextReportDate,
                PercentageOfCompletion = t.PercentageOfCompletion,
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
            });
        }


        /// <summary>
        /// Event ocurrs when the selection is changed.
        /// </summary>
        private async Task OnSelectionChanged()
        {
            await this.GetEventTasksAsync();
        }
        #endregion

    }
}
