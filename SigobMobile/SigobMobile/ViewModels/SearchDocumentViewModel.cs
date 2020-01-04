namespace SigobMobile.ViewModels
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Threading.Tasks;
    using AsyncAwaitBestPractices.MVVM;
    //using Common.Services;
    using Models;

    public class SearchDocumentViewModel : BaseViewModel
    {
        #region Attributes
        //private ApiService apiService;
        private string filter;
        private int selectedIndex = -1;
        private List<CategoricalData> dataTrayList;
        private ObservableCollection<CategoricalData> data;
        #endregion

        #region Properties
        public ObservableCollection<CategoricalData> Data
        {
            get => this.data;
            set => SetValue(ref this.data, value);
        }
        public string Filter
        {
            get => this.filter;
            set => SetValue(ref this.filter, value);
        }
        public int SelectedIndex
        {
            get { return this.selectedIndex; }
            set
            {
                SetValue(ref this.selectedIndex, value);
                OnSelectionChanged();
            }
        }
        #endregion

        #region Constructor
        public SearchDocumentViewModel()
        {
            this.LoadSegmentedFilters();
            //this.apiService = new ApiService();
            Data = new ObservableCollection<CategoricalData>(dataTrayList);
            this.SelectedIndex = 0;
        }
        #endregion

        #region Commands
        public IAsyncCommand SearchCurrentTrayCommand => new AsyncCommand(this.SearchCurrenTrayAsync);
        #endregion

        #region Methods
        /// <summary>
        /// Search document with specified filters
        /// </summary>
        /// <returns></returns>
        private async  Task SearchCurrenTrayAsync()
        {
            await Task.Delay(2);
        }

        /// <summary>
        /// Add Segmented items for Search Context
        /// </summary>
        private void LoadSegmentedFilters()
        {
            this.dataTrayList = new List<CategoricalData>  {
            new CategoricalData { Id = 0, Category = "All Trays", Value = 0 },
            new CategoricalData { Id = 1, Category = "Current Tray", Value = 1 }
            };
        }

        /// <summary>
        /// When select segmented control
        /// </summary>
        private void OnSelectionChanged()
        {
            this.Filter = (SelectedIndex == 0) ? "All Trays" : "Current Tray";
        }
        #endregion
    }
}
