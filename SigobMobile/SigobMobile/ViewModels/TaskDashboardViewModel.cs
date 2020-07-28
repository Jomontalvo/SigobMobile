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
    using Models;
    using Telerik.XamarinForms.Chart;
    using Views.Tasks;
    using Xamarin.Forms;
    using Calendar = Models.Calendar;
    using TaskSigob = Models.TaskSigob;

    public class TaskDashboardViewModel : BaseViewModel
    {
        #region Services
        internal ApiService apiService;
        #endregion

        #region ApiControllers
        internal string apiGetCalendarsController = "calendars";
        internal string apiGetTaskProfile = "tasks/profile";
        internal string apiGetTaskStatistics = "tasks/agenda/{0}/option/{1}";
        #endregion

        #region Attributes
        private List<Calendar> calendarList;
        private ObservableCollection<CalendarItemViewModel> calendars;
        private ObservableCollection<TaskControlMenuItemViewModel> personalMenuOptionsList;
        private bool isAddItemVisible;
        private List<TaskSigob> taskList;
        private List<TaskCategoricalData> taskStatistics;
        private List<Segment> taskStatus;
        private ObservableCollection<string> segmentedControlItems;
        private ObservableCollection<TaskItemViewModel> taskCollection;
        private Tasks taskObject;
        private ObservableCollection<TaskCategoricalData> taskStatisticsCollection;
        private ObservableCollection<TaskCategoricalData> chartLegend;
        private bool isRefreshing;
        private string graphTitle;
        internal string currentOfficeId;
        private string officialName;
        private string filter;
        private int selectedIndex = -1;
        private bool hasTask;
        private TTrafficLightStatus segmentFilter = TTrafficLightStatus.All;
        private bool isVisibleSearch;
        private bool isVisibleChart;
        private bool isVisibleGraphTitle;
        private bool isOpenCalendar;
        private bool isOpenControl;
        internal bool isSegmentBuilt = false;
        #endregion

        #region Properties
        public ObservableCollection<CalendarItemViewModel> CalendarSource
        {
            get => this.calendars;
            set => SetValue(ref this.calendars, value);
        }

        public string Filter
        {
            get => this.filter;
            set
            {
                SetValue(ref this.filter, value);
                this.RefreshTaskList();

            }
        }

        public bool IsVisibleSearch
        {
            get => this.isVisibleSearch;
            set => SetValue(ref this.isVisibleSearch, value);
        }

        public bool IsOpenCalendar
        {
            get => this.isOpenCalendar;
            set => SetValue(ref this.isOpenCalendar, value);
        }
        public bool IsOpenControl
        {
            get => this.isOpenControl;
            set => SetValue(ref this.isOpenControl, value);
        }

        public ObservableCollection<string> SegmentedControlItems
        {
            get => this.segmentedControlItems;
            set => SetValue(ref this.segmentedControlItems, value);
        }

        public List<Segment> TaskStatus
        {
            get => this.taskStatus;
            set => SetValue(ref this.taskStatus, value);
        }

        public bool IsAddItemVisible
        {
            get => this.isAddItemVisible;
            set => SetValue(ref this.isAddItemVisible, value);
        }
        public ObservableCollection<TaskCategoricalData> TaskStatistics
        {
            get => this.taskStatisticsCollection;
            set => SetValue(ref this.taskStatisticsCollection, value);
        }

        public ObservableCollection<TaskItemViewModel> TaskCollection
        {
            get => this.taskCollection;
            set => SetValue(ref this.taskCollection, value);
        }

        public ObservableCollection<TaskCategoricalData> ChartLegend
        {
            get { return this.chartLegend; }
            set { SetValue(ref this.chartLegend, value); }
        }

        public int SelectedIndex
        {
            get { return this.selectedIndex; }
            set
            {
                SetValue(ref this.selectedIndex, value);
                this.OnSelectionChangedAsync();
            }
        }

        public bool IsRefreshing
        {
            get { return this.isRefreshing; }
            set { SetValue(ref this.isRefreshing, value); }
        }

        public bool IsVisibleChart
        {
            get { return this.isVisibleChart; }
            set { SetValue(ref this.isVisibleChart, value); }
        }

        public bool IsVisibleGraphTitle
        {
            get { return this.isVisibleGraphTitle; }
            set { SetValue(ref this.isVisibleGraphTitle, value); }
        }

        public string GraphTitle
        {
            get { return this.graphTitle; }
            set { SetValue(ref this.graphTitle, value); }
        }

        public string OfficialName
        {
            get { return this.officialName; }
            set { SetValue(ref this.officialName, value); }
        }

        public bool HasTask
        {
            get => this.hasTask;
            set => SetValue(ref this.hasTask, value);
        }

        public ObservableCollection<TaskControlMenuItemViewModel> PersonalMenuOptionsList
        {
            get => this.personalMenuOptionsList;
            set => SetValue(ref this.personalMenuOptionsList, value);
        }

        #endregion

        #region Constructors
        public TaskDashboardViewModel()
        {
            this.apiService = new ApiService();
            this.IsVisibleChart = true;
            this.IsVisibleGraphTitle = false;
            IsAddItemVisible = (Device.RuntimePlatform == Device.iOS) ? true : false;
            this.currentOfficeId = Settings.TaskControlOfficeCode;
            IErrorHandler errorHandler = null;
            this.LoadCalendars().FireAndForgetSafeAsync(errorHandler);
            this.LoadTaskBoardAsync().FireAndForgetSafeAsync(errorHandler);
            
        }
        #endregion

        #region Commands
        public ICommand SearchCommand => new RelayCommand(SearchView);
        public ICommand SliceTappedCommand => new RelayCommand<ChartSelectionBehavior>(SliceTapped);
        public ICommand RefreshTaskListFromChartCommand => new RelayCommand<ChartSelectionBehavior>(RefreshTaskListFromChart);
        public ICommand SelectCalendarCommand => new RelayCommand(SelectCalendar);
        public ICommand CloseSelectCalendarCommand => new RelayCommand(CloseSelectCalendar);
        public ICommand SwipeChartCommand => new RelayCommand<string>(SwipeChart);
        public ICommand CloseControlMenuCommand => new RelayCommand(CloseControlMenu);
        public IAsyncCommand LoadMenuControlCommand => new AsyncCommand(LoadMenuControlAsync);
        public IAsyncCommand LoadGeneralControlMenuCommand => new AsyncCommand(LoadGeneralControlMenuAsync);
        public IAsyncCommand LoadPersonalControlMenuCommand => new AsyncCommand(LoadPersonalControlMenuAsync);
        public IAsyncCommand LoadOverdueControlMenuCommand => new AsyncCommand(LoadOverdueControlMenuAsync);
        public IAsyncCommand RefreshCommand => new AsyncCommand(LoadTaskBoardAsync);
        #endregion

        #region Methods
        /// <summary>
        /// Open or Close SearchBar
        /// </summary>
        private void SearchView()
        {
            this.IsVisibleSearch = !this.IsVisibleSearch;
        }

        /// <summary>
        /// Built Menu Options (Personal Control)
        /// </summary>
        private void LoadPersonalControlMenuOptions(int managementCenterId)
        {
            if (managementCenterId == 0)
            {
                PersonalMenuOptionsList = new ObservableCollection<TaskControlMenuItemViewModel>
                {
                    new TaskControlMenuItemViewModel
                    {
                        Icon = $"ic_task_status_10",
                        Id = 10,
                        IsEnabled = true,
                        Key = string.Empty,
                        MenuItemName = Languages.AllTaskStatus
                    },
                    new TaskControlMenuItemViewModel
                    {
                        Icon = "ic_task_status_3",
                        Id = 3,
                        IsEnabled = true,
                        Key = string.Empty,
                        MenuItemName = Languages.NewAssignedTasks
                    },
                    new TaskControlMenuItemViewModel
                    {
                        Icon = "ic_task_status_4",
                        Id = 4,
                        IsEnabled = true,
                        Key = string.Empty,
                        MenuItemName = Languages.TasksSetByMe
                    },
                    new TaskControlMenuItemViewModel
                    {
                        Icon = "ic_task_status_5",
                        Id = 5,
                        IsEnabled = true,
                        Key = string.Empty,
                        MenuItemName = Languages.TasksMonitoredByMe
                    },
                    new TaskControlMenuItemViewModel
                    {
                        Icon = "ic_task_status_8",
                        Id = 8,
                        IsEnabled = true,
                        Key = string.Empty,
                        MenuItemName = Languages.TaskCopies
                    },
                    new TaskControlMenuItemViewModel
                    {
                        Icon = "ic_task_status_9",
                        Id = 9,
                        IsEnabled = true,
                        Key = string.Empty,
                        MenuItemName = Languages.TaskMessages
                    }
                };
            }
            else
            {
                PersonalMenuOptionsList = new ObservableCollection<TaskControlMenuItemViewModel>
                {
                    new TaskControlMenuItemViewModel
                    {
                        Icon = $"ic_task_status_10",
                        Id = 10,
                        IsEnabled = true,
                        Key = string.Empty,
                        MenuItemName = Languages.AllTaskStatus
                    },
                    new TaskControlMenuItemViewModel
                    {
                        Icon = "ic_task_status_3",
                        Id = 3,
                        IsEnabled = true,
                        Key = string.Empty,
                        MenuItemName = Languages.NewAssignedTasks
                    },
                    new TaskControlMenuItemViewModel
                    {
                        Icon = "ic_task_status_4",
                        Id = 4,
                        IsEnabled = true,
                        Key = string.Empty,
                        MenuItemName = Languages.TasksSetByMe
                    },
                    new TaskControlMenuItemViewModel
                    {
                        Icon = "ic_task_status_5",
                        Id = 5,
                        IsEnabled = true,
                        Key = string.Empty,
                        MenuItemName = Languages.TasksMonitoredByMe
                    },
                    new TaskControlMenuItemViewModel
                    {
                        Icon = "ic_task_status_8",
                        Id = 8,
                        IsEnabled = true,
                        Key = string.Empty,
                        MenuItemName = Languages.TaskCopies
                    },
                    new TaskControlMenuItemViewModel
                    {
                        Icon = "ic_task_status_9",
                        Id = 9,
                        IsEnabled = true,
                        Key = string.Empty,
                        MenuItemName = Languages.TaskMessages
                    },
                    new TaskControlMenuItemViewModel
                    {
                        Icon = "ic_task_status_12",
                        Id = 12,
                        IsEnabled = true,
                        Key = string.Empty,
                        MenuItemName = Languages.TaskWithAuditReport
                    }
                };
            }
                
        }

        /// <summary>
        /// Delayed Task Control (For Management Center)
        /// </summary>
        /// <returns></returns>
        private async Task LoadOverdueControlMenuAsync()
        {
            this.IsOpenControl = false;
            var result = await App.Navigator.DisplayActionSheet(
                Languages.OverdueControl,
                Languages.Cancel,
                null,
                new string[]{
                Languages.OverdueTasksByResponsible,
                Languages.OverdueTasksByProgrammer,
                Languages.OverdueTasksByType});
            if (result != Languages.Cancel)
            {
                await App.Navigator.DisplayAlert(Languages.ControlTitle, Languages.GeneralError, null, Languages.Accept);
                return;
            }
            else return;
        }

        /// <summary>
        /// Load Options for Advanced Search Task
        /// </summary>
        /// <returns></returns>
        private async Task LoadGeneralControlMenuAsync()
        {
            this.IsOpenControl = false;
            var mainViewModel = MainViewModel.GetInstance();
            mainViewModel.TaskGeneralControl = new TaskGeneralControlViewModel();
            await App.Navigator.PushAsync(new TaskGeneralControlPage());
            return;
        }

        /// <summary>
        /// Task Personal Control
        /// </summary>
        /// <returns></returns>
        private async Task LoadPersonalControlMenuAsync()
        {
            this.IsOpenControl = false;
            var result = await App.Navigator.DisplayActionSheet(
                Languages.PersonalControl,
                Languages.Cancel,
                null,
                new string[]{
                Languages.NewAssignedTasks,
                Languages.OngoingTaskSetByMe,
                Languages.OngoingTasksThatMonitoring,
                Languages.CompletedTasksSetByMe,
                Languages.CompletedTasksThatMonitoring,
                Languages.TaskCopies,
                Languages.TaskMessages});
            if (result != Languages.Cancel)
            {
                await App.Navigator.DisplayAlert(Languages.ControlTitle, Languages.GeneralError, null, Languages.Accept);
                return;
            }
            else return;
        }

        /// <summary>
        /// Open Popup Menu for task control
        /// </summary>
        private async Task LoadMenuControlAsync()
        {
            string[] optionsArray = { Languages.GeneralControl, Languages.PersonalControl };
            CalendarItemViewModel selectedCalendar = this.CalendarSource.Single(c => c.IsSelectedForControl);
            if (selectedCalendar.ManagementCenterId > 0)
            {
                Array.Resize(ref optionsArray, optionsArray.Length + 1);
                optionsArray[^1] = ($"{Languages.OverdueControl}");
            }

            //Load Personal Control Options
            this.LoadPersonalControlMenuOptions(selectedCalendar.ManagementCenterId);


            var result = await App.Navigator.DisplayActionSheet(
                Languages.TaskControl,
                Languages.Cancel,
                null,
                optionsArray);

            if (result == Languages.GeneralControl)
            {
                await App.Navigator.DisplayAlert(Languages.ControlTitle, Languages.GeneralError, null, Languages.Accept);
                return;
            }
            else if (result == Languages.PersonalControl) this.IsOpenControl = true;
            else if (result == Languages.OverdueControl) this.IsOpenControl = false;
            else return;
        }

        /// <summary>
        /// Cancel Control Menu
        /// </summary>
        private void CloseControlMenu()
        {
            this.IsOpenControl = false;
        }

        /// <summary>
        /// Close Calendar List
        /// </summary>
        private void CloseSelectCalendar()
        {
            this.IsOpenCalendar = false;
        }

        /// <summary>
        /// Open Calendar List
        /// </summary>
        private void SelectCalendar()
        {
            this.IsOpenCalendar = true;
        }

        /// <summary>
        /// Hide Chart frame
        /// </summary>
        /// <param name="direction"></param>
        private void SwipeChart(string direction)
        {

            IsVisibleChart = (direction != SwipeDirection.Up.ToString());
        }

        /// <summary>
        /// Refresh Task List and Segments
        /// </summary>
        private void RefreshTaskList()
        {
            if (string.IsNullOrEmpty(this.Filter))
            {
                TaskCollection = new ObservableCollection<TaskItemViewModel>(
                this.taskList.Select(t => new TaskItemViewModel
                {
                    AuditorOfficeId = t.AuditorOfficeId,
                    AuditStatus = t.AuditStatus,
                    CanCloseReport = t.CanCloseReport,
                    CanViewTaskParent = t.CanViewTaskParent,
                    Changes = t.Changes,
                    ChangesValue = t.ChangesValue,
                    Completion = t.Completion,
                    CostExecuted = t.CostExecuted,
                    CostPlanned = t.CostPlanned,
                    Delay = t.Delay,
                    DeletionError = t.DeletionError,
                    Description = (!string.IsNullOrEmpty(t.Description)) ? t.Description.TrimEnd() : string.Empty,
                    EndDate = t.EndDate,
                    HasAudit = t.HasAudit,
                    HasMessage = t.HasMessage,
                    HistoricalDetail = t.HistoricalDetail,
                    HistoricalReport = t.HistoricalReport,
                    Id = t.Id,
                    Instrument = t.Instrument,
                    IsDeleted = t.IsDeleted,
                    IsInserted = t.IsInserted,
                    IsNew = t.IsNew,
                    IsTaskReviewed = t.IsTaskReviewed,
                    IsUpdating = t.IsUpdating,
                    LastErrorMessage = t.LastErrorMessage,
                    ModificationDetailDate = t.ModificationDetailDate,
                    ModificationReportDate = t.ModificationReportDate,
                    ModuleId = t.ModuleId,
                    Monitor = t.Monitor,
                    MonitorCanFinishTask = t.MonitorCanFinishTask,
                    NextReportDate = t.NextReportDate,
                    ParentEntity = t.ParentEntity,
                    Priority = t.Priority,
                    ProgrammedEndDate = t.ProgrammedEndDate,
                    Programmer = t.Programmer,
                    ReadOnly = t.ReadOnly,
                    ReiterationsCount = t.ReiterationsCount,
                    Report = (!string.IsNullOrEmpty(t.Report)) ? t.Report.TrimEnd() : Languages.NoReportAvailable,
                    ReportFrequency = t.ReportFrequency,
                    ReportStatus = t.ReportStatus,
                    Responsible = t.Responsible,
                    RevisedReport = t.RevisedReport,
                    Source = t.Source,
                    StartDate = t.StartDate,
                    Status = t.Status,
                    Template = t.Template,
                    TipoColorBlue = t.TipoColorBlue,
                    TipoColorGreen = t.TipoColorGreen,
                    TipoColorRed = t.TipoColorRed,
                    Title = t.Title,
                    TrafficLight = t.TrafficLight,
                    Type = t.Type
                })
                .Where(t => (segmentFilter == TTrafficLightStatus.All || t.TrafficLight == segmentFilter))
                .OrderBy(t => t.TrafficLight));
            }
            else
            {
                TaskCollection = new ObservableCollection<TaskItemViewModel>(
                this.taskList.Select(t => new TaskItemViewModel
                {
                    AuditorOfficeId = t.AuditorOfficeId,
                    AuditStatus = t.AuditStatus,
                    CanCloseReport = t.CanCloseReport,
                    CanViewTaskParent = t.CanViewTaskParent,
                    Changes = t.Changes,
                    ChangesValue = t.ChangesValue,
                    Completion = t.Completion,
                    CostExecuted = t.CostExecuted,
                    CostPlanned = t.CostPlanned,
                    Delay = t.Delay,
                    DeletionError = t.DeletionError,
                    Description = (!string.IsNullOrEmpty(t.Description)) ? t.Description.TrimEnd() : string.Empty,
                    EndDate = t.EndDate,
                    HasAudit = t.HasAudit,
                    HasMessage = t.HasMessage,
                    HistoricalDetail = t.HistoricalDetail,
                    HistoricalReport = t.HistoricalReport,
                    Id = t.Id,
                    Instrument = t.Instrument,
                    IsDeleted = t.IsDeleted,
                    IsInserted = t.IsInserted,
                    IsNew = t.IsNew,
                    IsTaskReviewed = t.IsTaskReviewed,
                    IsUpdating = t.IsUpdating,
                    LastErrorMessage = t.LastErrorMessage,
                    ModificationDetailDate = t.ModificationDetailDate,
                    ModificationReportDate = t.ModificationReportDate,
                    ModuleId = t.ModuleId,
                    Monitor = t.Monitor,
                    MonitorCanFinishTask = t.MonitorCanFinishTask,
                    NextReportDate = t.NextReportDate,
                    ParentEntity = t.ParentEntity,
                    Priority = t.Priority,
                    ProgrammedEndDate = t.ProgrammedEndDate,
                    Programmer = t.Programmer,
                    ReadOnly = t.ReadOnly,
                    ReiterationsCount = t.ReiterationsCount,
                    Report = (!string.IsNullOrEmpty(t.Report)) ? t.Report.TrimEnd() : Languages.NoReportAvailable,
                    ReportFrequency = t.ReportFrequency,
                    ReportStatus = t.ReportStatus,
                    Responsible = t.Responsible,
                    RevisedReport = t.RevisedReport,
                    Source = t.Source,
                    StartDate = t.StartDate,
                    Status = t.Status,
                    Template = t.Template,
                    TipoColorBlue = t.TipoColorBlue,
                    TipoColorGreen = t.TipoColorGreen,
                    TipoColorRed = t.TipoColorRed,
                    Title = t.Title,
                    TrafficLight = t.TrafficLight,
                    Type = t.Type
                })
                .Where(t => (segmentFilter == TTrafficLightStatus.All || t.TrafficLight == segmentFilter))
                .Where(t => (!string.IsNullOrEmpty(t.Title) && t.Title.ToLower().Contains(this.Filter.ToLower())) ||
                        (!string.IsNullOrEmpty(t.Description) && t.Description.ToLower().Contains(this.Filter.ToLower())) ||
                        (!string.IsNullOrEmpty(t.Responsible.Name) && t.Responsible.Name.ToLower().Contains(this.Filter.ToLower())) ||
                        (!string.IsNullOrEmpty(t.Report) && t.Report.ToLower().Contains(this.Filter.ToLower())) ||
                        (!string.IsNullOrEmpty(t.Monitor.Name) && t.Monitor.Name.ToLower().Contains(this.Filter.ToLower())))
            .OrderByDescending(t => t.TrafficLight)
            .ToList());
            }
        }

        private void RefreshTaskListFromChart(ChartSelectionBehavior obj)
        {
            IsRefreshing = false;
        }

        /// <summary>
        /// Tap a segment of Donut Chart
        /// </summary>
        /// <param name="serie"></param>
        private void SliceTapped(ChartSelectionBehavior serie)
        {
            if (serie.SelectedPoints.Any())
            {
                TaskCategoricalData selectedPoint = (TaskCategoricalData)serie.SelectedPoints.ElementAt(0).DataItem;
                this.SelectedIndex = this.TaskStatus.FirstOrDefault(s => s.QueryId == (byte)selectedPoint.Id).Id;
            }
            else
            {
                serie.ClearSelection();
                this.SelectedIndex = 0; //All
                return;
            }
        }

        /// <summary>
        /// Event ocurrs when the selection is changed.
        /// </summary>
        private void OnSelectionChangedAsync()
        {
            //this.IsRefreshing = true;
            int queryId = this.TaskStatus.FirstOrDefault(s => s.Id == SelectedIndex).QueryId;
            this.segmentFilter = (TTrafficLightStatus)queryId;
            this.RefreshTaskList();
            this.SetGraphTitle();
            //this.IsRefreshing = false;
        }

        private void SetGraphTitle()
        {
            this.GraphTitle = segmentFilter switch
            {
                TTrafficLightStatus.All => (this.currentOfficeId == Settings.OfficeCode)
                    ?Languages.MyTasks :
                    $"{Languages.TasksOf} {this.OfficialName}" ,
                TTrafficLightStatus.InProgress => Languages.InProgressStatus,
                TTrafficLightStatus.CloseToDeadline => Languages.CloseToDeadlinedStatus,
                TTrafficLightStatus.Overdue => Languages.OverdueStatus,
                TTrafficLightStatus.Completed => Languages.CompletedStatus,
                _ => Languages.Tasks
            };
        }

        /// <summary>
        /// Load Task Board (List and Statistics)
        /// </summary>
        public async Task LoadTaskBoardAsync()
        {
            this.IsRefreshing = true;
            await GetStatisticsAndTaskList(this.currentOfficeId, TQueryOption.TasksOf);
            this.LoadSegmentedFilters();
            Settings.TaskControlOfficeCode = this.currentOfficeId;
            this.IsRefreshing = false;
        }

        /// <summary>
        /// Get Graph and Task List according user profile
        /// </summary>
        /// <returns></returns>
        private async Task GetStatisticsAndTaskList(string officeCode, TQueryOption queryOption)
        {
            try
            {
                var connection = await this.apiService.CheckConnection();
                if (!connection.IsSuccess)
                {
                    this.IsRefreshing = false;
                    await Application.Current.MainPage.DisplayAlert(
                        Languages.Error,
                        connection.Message,
                        Languages.Cancel);
                    await App.Navigator.PopToRootAsync();
                    return;
                }
                var response = await this.apiService.Get<Tasks>(
                    Settings.UrlBaseApiSigob,
                    App.PrefixApiSigob,
                    string.Format(this.apiGetTaskStatistics, officeCode, (byte)queryOption),
                    Settings.Token,
                    Settings.DbToken
                );
                if (!response.IsSuccess)
                {
                    this.IsRefreshing = false;
                    await Application.Current.MainPage.DisplayAlert(
                        Languages.Error,
                        response.Message,
                        Languages.Cancel);
                    return;
                }
                this.taskObject = (Tasks)response.Result;
                this.OfficialName =  taskObject.OfficialName;
                this.HasTask = taskObject.TaskList.Count() > 0;
                this.taskList = taskObject.TaskList.ToList<TaskSigob>();
                this.taskStatistics = taskObject.TaskStatistics.ToList<TaskCategoricalData>();
                this.TaskStatistics = new ObservableCollection<TaskCategoricalData>(ToTaskStatistics());
                // Set Legend and Array segment control values
                this.ChartLegend = new ObservableCollection<TaskCategoricalData>(
                    taskObject.TaskStatistics.Where((point) => point.Value > 0));
                this.SetGraphTitle();
                this.RefreshTaskList();
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert(
                        Languages.Error,
                        ex.Message,
                        Languages.Cancel);
                await Application.Current.MainPage.Navigation.PopAsync();
                return;
            }
            finally
            {
                this.IsVisibleGraphTitle = true;
                this.IsRefreshing = false;
            }
        }

        /// <summary>
        /// Values for segmented control (also using as filters)
        /// </summary>
        private void LoadSegmentedFilters()
        {
            if (!this.isSegmentBuilt)
            {
                TaskStatus = new List<Segment>(ToTaskSegment(1));
                var firstItem = new Segment() { Id = 0, QueryId = 10, SegmentName = Languages.AllTaskStatus };
                TaskStatus.Insert(0, firstItem);
                //Built ObservableCollection<string> with Segments Names
                this.SegmentedControlItems = new ObservableCollection<string>(ToSegmentString());
                isSegmentBuilt = true;
            }
            else
            {
                return;
            }
        }

        private IEnumerable<string> ToSegmentString()
        {
            return this.taskStatus.Select(s => s.SegmentName);
        }

        /// <summary>
        /// Built Task Segment ObservableCollection
        /// </summary>
        /// <returns></returns>
        private IEnumerable<Segment> ToTaskSegment(int index)
        {
            return this.chartLegend.Select(s => new Segment
            {
                Id = index++,
                QueryId = (byte)s.Id,
                SegmentName = s.Category
            });
        }

        /// <summary>
        /// Convert data to chart list in observablecollection
        /// </summary>
        /// <returns></returns>
        private IEnumerable<TaskCategoricalData> ToTaskStatistics()
        {
            return this.taskStatistics.Select(s => new TaskCategoricalData
            {
                Id = s.Id,
                Value = (s.Value != 0) ? s.Value : null,
                ColorName = s.ColorName,
                A = s.A,
                R = s.R,
                G = s.G,
                B = s.B
            });
        }

        /// <summary>
        /// Loads the calendars.
        /// </summary>
        private async Task LoadCalendars()
        {
            var connection = await this.apiService.CheckConnection();
            if (!connection.IsSuccess)
            {
                this.IsRefreshing = false;
                await Application.Current.MainPage.DisplayAlert(
                    Languages.Error,
                    connection.Message,
                    Languages.Cancel);
                await Application.Current.MainPage.Navigation.PopModalAsync();
                return;
            }

            var response = await this.apiService.GetList<Calendar>(
                Settings.UrlBaseApiSigob,
                App.PrefixApiSigob,
                this.apiGetCalendarsController,
                Settings.Token,
                Settings.DbToken
            );
            if (!response.IsSuccess)
            {
                this.IsRefreshing = false;
                await Application.Current.MainPage.DisplayAlert(
                    Languages.Error,
                    response.Message,
                    Languages.Cancel);
                await Application.Current.MainPage.Navigation.PopModalAsync();
                return;
            }
            calendarList = (List<Calendar>)response.Result;
            this.RefreshCalendarList();
        }

        /// <summary>
        /// Refresh calendar list with selected calendar
        /// </summary>
        internal void RefreshCalendarList()
        {
            this.CalendarSource = new ObservableCollection<CalendarItemViewModel>(ToCalendarItemViewModel());
        }

        /// <summary>
        /// Convert list obtained with API Call to ViewModel ObservableCollection.
        /// </summary>
        /// <returns>The calendar item view model.</returns>
        private IEnumerable<CalendarItemViewModel> ToCalendarItemViewModel()
        {
            return calendarList.Select(c => new CalendarItemViewModel
            {
                AgendaName = c.AgendaName,
                BlueColor = c.BlueColor,
                GreenColor = c.GreenColor,
                IsOwner = c.IsOwner,
                IsVisible = c.IsVisible,
                ManagementCenterId = c.ManagementCenterId,
                NumberColor = c.NumberColor,
                OfficeId = c.OfficeId,
                Permission = c.Permission,
                RedColor = c.RedColor,
                IsSelectedForControl = c.OfficeId == currentOfficeId,
            });
        }
        #endregion
    }
}
