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
    using Services;
    using Telerik.XamarinForms.Chart;
    using Xamarin.Forms;

    public class TaskDashboardViewModel : BaseViewModel
    {
        #region Services
        internal ApiService apiService;
        #endregion

        #region ApiControllers
        internal string apiGetTaskProfile = "tasks/profile";
        internal string apiGetTaskStatistics = "tasks/agenda/{0}/option/{1}";
        #endregion

        #region Attributes
        private bool isAddItemVisible;
        private List<TaskSigob> taskList;
        private List<TaskCategoricalData> taskStatistics;
        private ObservableCollection<TaskSigob> taskCollection;
        private Tasks taskObject;
        private ObservableCollection<TaskCategoricalData> taskStatisticsCollection;
        private ObservableCollection<TaskCategoricalData> chartLegend;
        private bool isRefreshing;
        private string graphTitle;

        #endregion

        #region Properties
        public bool IsAddItemVisible
        {
            get { return this.isAddItemVisible; }
            set { SetValue(ref this.isAddItemVisible, value); }
        }
        public ObservableCollection<TaskCategoricalData> TaskStatistics
        {
            get { return this.taskStatisticsCollection; }
            set { SetValue(ref this.taskStatisticsCollection, value); }
        }

        public ObservableCollection<TaskSigob> TaskList
        {
            get { return this.taskCollection; }
            set { SetValue(ref this.taskCollection, value); }
        }

        public ObservableCollection<TaskCategoricalData> ChartLegend
        {
            get { return this.chartLegend; }
            set { SetValue(ref this.chartLegend, value); }
        }

        public bool IsRefreshing
        {
            get { return this.isRefreshing; }
            set { SetValue(ref this.isRefreshing, value); }
        }

        public string GraphTitle
        {
            get { return this.graphTitle; }
            set { SetValue(ref this.graphTitle, value); }
        }
        #endregion

        #region Constructors
        public TaskDashboardViewModel()
        {
            this.apiService = new ApiService();
            this.LoadTaskBoard();
        }
        #endregion

        #region Methods
        public void LoadTaskBoard()
        {
            this.IsAddItemVisible = (Device.RuntimePlatform == Device.iOS) ? true : false;
            IErrorHandler errorHandler = null;
            GetStatisticsAndTaskList(Settings.OfficeCode,TQueryOption.TasksOf).FireAndForgetSafeAsync(errorHandler);
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
                    await Application.Current.MainPage.Navigation.PopAsync();
                    return;
                }
                var response = await this.apiService.Get<Tasks>(
                    Settings.UrlBaseApiSigob,
                    App.PrefixApiSigob,
                    string.Format(this.apiGetTaskStatistics, officeCode, (byte)queryOption ),
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
                this.taskList = taskObject.TaskList.ToList<TaskSigob>();
                this.taskStatistics = taskObject.TaskStatistics.ToList<TaskCategoricalData>();
                TaskList = new ObservableCollection<TaskSigob>(ToTask());
                TaskStatistics = new ObservableCollection<TaskCategoricalData>(ToTaskStatistics());
                // Legend
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
            finally { this.IsRefreshing = false; }
            
        }

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

        private IEnumerable<TaskSigob> ToTask()
        {
            return this.taskList.Select(t => new TaskSigob
            {
                Id = t.Id,
                Title = t.Title,
                Description = t.Description,
                Type = t.Type
            });
        }

        
        #endregion

        #region Commands
        public ICommand SliceTappedCommand => new RelayCommand<ChartSelectionBehavior>(SliceTapped);

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
