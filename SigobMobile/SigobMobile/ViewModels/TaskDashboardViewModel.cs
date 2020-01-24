namespace SigobMobile.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Windows.Input;
    using GalaSoft.MvvmLight.Command;
    using Helpers;
    using Interfaces;
    using Models;
    using Common.Services;
    using Telerik.XamarinForms.Chart;
    using Xamarin.Forms;
    using SigobMobile.Common.Helpers;
    using SigobMobile.Common.Models;
    using AsyncAwaitBestPractices.MVVM;
    using Calendar = Models.Calendar;

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
        private ObservableCollection<TaskSigob> taskCollection;
        private Tasks taskObject;
        private ObservableCollection<TaskCategoricalData> taskStatisticsCollection;
        private ObservableCollection<TaskCategoricalData> chartLegend;
        private bool isRefreshing;
        private string graphTitle;
        private string officialName;
        private int selectedIndex = -1;
        private bool isVisibleChart;
        private bool isVisibleGraphTitle;
        private bool isOpen;
        #endregion

        #region Properties
        public ObservableCollection<CalendarItemViewModel> CalendarSource
        {
            get { return this.calendars; }
            set { SetValue(ref this.calendars, value); }
        }
        public bool IsOpen
        {
            get => this.isOpen;
            set => SetValue(ref this.isOpen, value);
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

        public ObservableCollection<TaskSigob> TaskCollection
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
            IErrorHandler errorHandler = null;
            this.LoadCalendars().FireAndForgetSafeAsync(errorHandler);
            this.LoadTaskBoardAsync().FireAndForgetSafeAsync(errorHandler);
        }
        #endregion

        #region Methods
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
            //await Task.Delay(100);
            //await LoadTaskBoardAsync();
        }

        /// <summary>
        /// Load Task Board (List and Statistics
        /// </summary>
        public async Task LoadTaskBoardAsync()
        {
            IsAddItemVisible = (Device.RuntimePlatform == Device.iOS) ? true : false;
            await GetStatisticsAndTaskList(Settings.OfficeCode, TQueryOption.TasksOf);
            LoadSegmentedFilters();
        }

        /// <summary>
        /// Get Graph and Task List according user profile
        /// </summary>
        /// <returns></returns>
        private async Task GetStatisticsAndTaskList(string officeCode, TQueryOption queryOption)
        {
            this.IsRefreshing = true;
            try
            {
                var connection = await this.apiService.CheckConnection();
                if (!connection.IsSuccess)
                {
                    IsRefreshing = false;
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
                TaskCollection = new ObservableCollection<TaskSigob>(ToTask());
                TaskStatistics = new ObservableCollection<TaskCategoricalData>(ToTaskStatistics());
                // Set Legend and Array segment control values
                this.ChartLegend = new ObservableCollection<TaskCategoricalData>(taskObject.TaskStatistics.Where((point) => point.Value > 0));
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
        /// Convert task list in observable collection
        /// </summary>
        /// <returns></returns>
        private IEnumerable<TaskSigob> ToTask()
        {
            return this.taskList.Select(t => new TaskSigob
            {
                Id = t.Id,
                Title = t.Title,
                ResponsibleName = t.ResponsibleName,
                Description = t.Description,
                Type = t.Type,
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

        #region Commands
        public ICommand SliceTappedCommand => new RelayCommand<ChartSelectionBehavior>(SliceTapped);
        public ICommand RefreshTaskListCommand => new RelayCommand<ChartSelectionBehavior>(RefreshTaskList);
        public IAsyncCommand RefreshCommand => new AsyncCommand(RefreshAsync);
        public ICommand SelectCalendarCommand => new RelayCommand(SelectCalendar);
        public ICommand CloseSelectCalendarCommand => new RelayCommand(CloseSelectCalendar);

        private void CloseSelectCalendar()
        {
            this.IsOpen = false;
        }

        private void SelectCalendar()
        {
            this.IsOpen = true;
        }

        public ICommand SwipeChartCommand => new RelayCommand<string>(SwipeChart);


        private void SwipeChart(string direction)
        {

            IsVisibleChart = (direction != SwipeDirection.Up.ToString());
        }

        private async Task RefreshAsync()
        {
            IsRefreshing = true;
            await Application.Current.MainPage.DisplayAlert(
                        Languages.Error,
                        "Hola",
                        Languages.Cancel);
            IsRefreshing = false;
            return;
        }
        private void RefreshTaskList(ChartSelectionBehavior obj)
        {
            IsRefreshing = false;
        }

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
                this.GraphTitle = "Filtro eliminado";
                return;
            }
        }

        #endregion


    }
}
