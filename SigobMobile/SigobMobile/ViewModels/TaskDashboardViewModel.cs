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
        private bool isAddItemVisible;
        private List<TaskSigob> taskList;
        private List<TaskCategoricalData> taskStatistics;
        private ObservableCollection<Segment> taskStatus;
        private ObservableCollection<string> segmentedControlItems;
        private ObservableCollection<TaskItemViewModel> taskCollection;
        private Tasks taskObject;
        private ObservableCollection<TaskCategoricalData> taskStatisticsCollection;
        private ObservableCollection<TaskCategoricalData> chartLegend;
        private bool isRefreshing;
        private string graphTitle;
        private string officialName;
        private string filter;
        private int selectedIndex = -1;
        private bool isVisibleSearch;
        private bool isVisibleChart;
        private bool isVisibleGraphTitle;
        private bool isOpen;
        private bool isOpenControl;
        #endregion

        #region Properties
        public ObservableCollection<CalendarItemViewModel> CalendarSource
        {
            get { return this.calendars; }
            set { SetValue(ref this.calendars, value); }
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

        public bool IsOpen
        {
            get => this.isOpen;
            set => SetValue(ref this.isOpen, value);
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

        public ObservableCollection<Segment> TaskStatus
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
                //IErrorHandler errorHandler = null;
                //OnSelectionChangedAsync().FireAndForgetSafeAsync(errorHandler);
                OnSelectionChangedAsync();
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
        #endregion

        #region Constructors
        public TaskDashboardViewModel()
        {
            this.apiService = new ApiService();
            this.IsVisibleChart = true;
            this.IsVisibleGraphTitle = false;
            IsAddItemVisible = (Device.RuntimePlatform == Device.iOS) ? true : false;
            IErrorHandler errorHandler = null;
            this.LoadTaskBoard().FireAndForgetSafeAsync(errorHandler);
            this.LoadCalendars().FireAndForgetSafeAsync(errorHandler);
        }
        #endregion

        #region Commands
        public ICommand SearchCommand => new RelayCommand(SearchView);
        public ICommand SliceTappedCommand => new RelayCommand<ChartSelectionBehavior>(SliceTapped);
        public ICommand RefreshTaskListFromChartCommand => new RelayCommand<ChartSelectionBehavior>(RefreshTaskListFromChart);
        public IAsyncCommand RefreshCommand => new AsyncCommand(LoadTaskBoard);
        public ICommand SelectCalendarCommand => new RelayCommand(SelectCalendar);
        public ICommand CloseSelectCalendarCommand => new RelayCommand(CloseSelectCalendar);
        public ICommand SwipeChartCommand => new RelayCommand<string>(SwipeChart);
        public ICommand LoadMenuControlCommand => new RelayCommand(LoadMenuControl);
        public ICommand CloseControlMenuCommand => new RelayCommand(CloseControlMenu);
        public IAsyncCommand LoadGeneralControlMenuCommand => new AsyncCommand(LoadGeneralControlMenuAsync);
        public IAsyncCommand LoadPersonalControlMenuCommand => new AsyncCommand(LoadPersonalControlMenuAsync);
        public IAsyncCommand LoadOverdueControlMenuCommand => new AsyncCommand(LoadOverdueControlMenuAsync);
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
        private void LoadMenuControl()
        {
            this.IsOpenControl = true;
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
            this.IsOpen = false;
        }

        /// <summary>
        /// Open Calendar List
        /// </summary>
        private void SelectCalendar()
        {
            this.IsOpen = true;
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
                    CanCloseReport = t.CanCloseReport,
                    Changes = t.Changes,
                    DaysLate = t.DaysLate,
                    Deleted = t.Deleted,
                    DeletionError = t.DeletionError,
                    Description = t.Description,
                    EndDate = t.EndDate,
                    EndProgrammedDate = t.EndProgrammedDate,
                    FrequencyDescription = t.FrequencyDescription,
                    HasMessage = t.HasMessage,
                    HistoriacalReport = t.HistoriacalReport,
                    HistoricalDetail = t.HistoricalDetail,
                    IconTaskOrigin = t.IconTaskOrigin,
                    Id = t.Id,
                    Inserted = t.Inserted,
                    Instrument = t.Instrument,
                    IsExternalResponsible = t.IsExternalResponsible,
                    IsNew = t.IsNew,
                    IsParentVisible = t.IsParentVisible,
                    LastErrorMessage = t.LastErrorMessage,
                    ModificationDetailDate = t.ModificationDetailDate,
                    ModificationReportDate = t.ModificationReportDate,
                    Modifications = t.Modifications,
                    ModuleId = t.ModuleId,
                    MonitorCanFinishTask = t.MonitorCanFinishTask,
                    MonitorName = t.MonitorName,
                    MonitorOfficeId = t.MonitorOfficeId,
                    NextReportDate = t.NextReportDate,
                    Source = t.Source,
                    Parent = t.Parent,
                    ParentEntity = t.ParentEntity,
                    Priority = t.Priority,
                    ProgrammerName = t.ProgrammerName,
                    ProgrammerOfficeId = t.ProgrammerOfficeId,
                    ReadOnly = t.ReadOnly,
                    Report = t.Report,
                    ReportFrequency = t.ReportFrequency,
                    ReportStatus = t.ReportStatus,
                    ResponsibleEmail = t.ResponsibleEmail,
                    ResponsibleName = t.ResponsibleName,
                    ResponsibleOfficeId = t.ResponsibleOfficeId,
                    RevisedReport = t.RevisedReport,
                    StartDate = t.StartDate,
                    Status = t.Status,
                    StatusDescription = t.StatusDescription,
                    Template = t.Template,
                    Title = t.Title,
                    Type = t.Type,
                    TypeBlue = t.TypeBlue,
                    TypeGreen = t.TypeGreen,
                    TypeRed = t.TypeRed
                }));
            }
            else
            {
                TaskCollection = new ObservableCollection<TaskItemViewModel>(
                this.taskList.Select(t => new TaskItemViewModel
                {
                    CanCloseReport = t.CanCloseReport,
                    Changes = t.Changes,
                    DaysLate = t.DaysLate,
                    Deleted = t.Deleted,
                    DeletionError = t.DeletionError,
                    Description = t.Description,
                    EndDate = t.EndDate,
                    EndProgrammedDate = t.EndProgrammedDate,
                    FrequencyDescription = t.FrequencyDescription,
                    HasMessage = t.HasMessage,
                    HistoriacalReport = t.HistoriacalReport,
                    HistoricalDetail = t.HistoricalDetail,
                    IconTaskOrigin = t.IconTaskOrigin,
                    Id = t.Id,
                    Inserted = t.Inserted,
                    Instrument = t.Instrument,
                    IsExternalResponsible = t.IsExternalResponsible,
                    IsNew = t.IsNew,
                    IsParentVisible = t.IsParentVisible,
                    LastErrorMessage = t.LastErrorMessage,
                    ModificationDetailDate = t.ModificationDetailDate,
                    ModificationReportDate = t.ModificationReportDate,
                    Modifications = t.Modifications,
                    ModuleId = t.ModuleId,
                    MonitorCanFinishTask = t.MonitorCanFinishTask,
                    MonitorName = t.MonitorName,
                    MonitorOfficeId = t.MonitorOfficeId,
                    NextReportDate = t.NextReportDate,
                    Source = t.Source,
                    Parent = t.Parent,
                    ParentEntity = t.ParentEntity,
                    Priority = t.Priority,
                    ProgrammerName = t.ProgrammerName,
                    ProgrammerOfficeId = t.ProgrammerOfficeId,
                    ReadOnly = t.ReadOnly,
                    Report = t.Report,
                    ReportFrequency = t.ReportFrequency,
                    ReportStatus = t.ReportStatus,
                    ResponsibleEmail = t.ResponsibleEmail,
                    ResponsibleName = t.ResponsibleName,
                    ResponsibleOfficeId = t.ResponsibleOfficeId,
                    RevisedReport = t.RevisedReport,
                    StartDate = t.StartDate,
                    Status = t.Status,
                    StatusDescription = t.StatusDescription,
                    Template = t.Template,
                    Title = t.Title,
                    Type = t.Type,
                    TypeBlue = t.TypeBlue,
                    TypeGreen = t.TypeGreen,
                    TypeRed = t.TypeRed
                })
                .Where(t => (!string.IsNullOrEmpty(t.Title) && t.Title.ToLower().Contains(this.Filter.ToLower())) ||
                        (!string.IsNullOrEmpty(t.Description) && t.Description.ToLower().Contains(this.Filter.ToLower())) ||
                        (!string.IsNullOrEmpty(t.ResponsibleName) && t.ResponsibleName.ToLower().Contains(this.Filter.ToLower())) ||
                        (!string.IsNullOrEmpty(t.Report) && t.Report.ToLower().Contains(this.Filter.ToLower())) ||
                        (!string.IsNullOrEmpty(t.MonitorName) && t.MonitorName.ToLower().Contains(this.Filter.ToLower())))
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
                this.GraphTitle = selectedPoint.Category.ToString();
                return;
            }
            else
            {
                serie.ClearSelection();
                this.GraphTitle = Languages.MyTasks;
                return;
            }
        }

        /// <summary>
        /// Event ocurrs when the selection is changed.
        /// </summary>
        private void OnSelectionChangedAsync()
        {
            this.GraphTitle = SelectedIndex switch
            {
                0 => Languages.MyTasks,
                1 => Languages.InProgressStatus,
                2 => Languages.CloseToDeadlinedStatus,
                3 => Languages.OverdueStatus,
                4 => Languages.CompletedStatus,
                _ => Languages.Tasks
            };

            //this.IsRefreshing = true;
            //int queryId = this.TaskStatus.FirstOrDefault(s => s.Id == SelectedIndex).QueryId;
            //var localEvent = MainViewModel.GetInstance().EventCg.LocalEvent;
            //switch (queryId)
            //{
            //    case 0:
            //        this.tick = EventTaskMoment.Previous;
            //        this.auth = localEvent.PreviousTask;
            //        break;
            //    case 1:
            //        this.tick = EventTaskMoment.Support;
            //        this.auth = localEvent.SupportTasks;
            //        break;
            //    case 2:
            //        this.tick = EventTaskMoment.Later;
            //        this.auth = localEvent.LaterTasks;
            //        break;
            //    case 3:
            //        this.tick = EventTaskMoment.All;
            //        this.auth = EventTasksAttribute.ViewAll;
            //        break;
            //    default:
            //        break;
            //}
            //await this.GetEventTasksAsync();
            //this.IsRefreshing = false;
            //await Task.Delay(100);
            //await LoadTaskBoardAsync();
        }

        /// <summary>
        /// Load Task Board (List and Statistics)
        /// </summary>
        public async Task LoadTaskBoard()
        {
            this.IsRefreshing = true;
            await GetStatisticsAndTaskList(Settings.OfficeCode, TQueryOption.TasksOf);
            LoadSegmentedFilters();
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
                this.GraphTitle = taskObject.GraphTitle;
                this.OfficialName = taskObject.OfficialName;
                this.taskList = taskObject.TaskList.ToList<TaskSigob>();
                this.taskStatistics = taskObject.TaskStatistics.ToList<TaskCategoricalData>();
                TaskStatistics = new ObservableCollection<TaskCategoricalData>(ToTaskStatistics());
                // Set Legend and Array segment control values
                this.ChartLegend = new ObservableCollection<TaskCategoricalData>(
                    taskObject.TaskStatistics.Where((point) => point.Value > 0));
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
            TaskStatus = new ObservableCollection<Segment>(ToTaskSegment(1));
            var firstItem = new Segment() { Id = 0, QueryId = 10, SegmentName = Languages.AllStatus };
            TaskStatus.Insert(0, firstItem);
            //Built ObservableCollection<string> with Segments Names
            this.SegmentedControlItems = new ObservableCollection<string>(ToSegmentString());
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
                RedColor = c.RedColor
            }).Where(c => c.OfficeId != Settings.OfficeCode);
        }
        #endregion
    }
}
