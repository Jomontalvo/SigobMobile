namespace SigobMobile.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Threading.Tasks;
    using AsyncAwaitBestPractices.MVVM;
    using Common.Services;
    using Models;
    using SigobMobile.Helpers;
    using SigobMobile.Interfaces;

    public class SearchDocumentViewModel : BaseViewModel
    {
        #region Attributes
        private ApiService apiService;
        private string filter;
        private string filterInfo;
        private readonly int senderId;
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
            set
            {
                SetValue(ref this.filter, value);
                if (value == null)
                {
                    IErrorHandler errorHandler = null;
                    this.CloseSearchAsync().FireAndForgetSafeAsync(errorHandler);
                }
            } 
        }
        public string FilterInfo
        {
            get => this.filterInfo;
            set => SetValue(ref this.filterInfo, value);
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
        public SearchDocumentViewModel(int senderId)
        {
            this.senderId = senderId;
            this.LoadSegmentedFilters();
            this.apiService = new ApiService();
            Data = new ObservableCollection<CategoricalData>(dataTrayList);
            this.Filter = " ";
            this.SelectedIndex = 0;
        }
        #endregion

        #region Commands
        public IAsyncCommand SearchCurrentTrayCommand => new AsyncCommand(this.SearchCurrenTrayAsync);
        #endregion

        #region Methods
        /// <summary>
        /// Close curret search view when Cancel button is pressed
        /// </summary>
        private async Task CloseSearchAsync()
        {
            await App.Navigator.CurrentPage.Navigation.PopModalAsync();
        }

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
            if (this.senderId >= 0) //Sender is Tray
            {
                this.dataTrayList = new List<CategoricalData>  {
                new CategoricalData { Id = 0, Category = "All Trays", Value = 0 },
                new CategoricalData { Id = 1, Category = "Current Tray", Value = 1 }
                };
            }
            else //Sender is Correspondence Main Menu
            {
                this.dataTrayList = new List<CategoricalData>  {
                new CategoricalData { Id = 0, Category = Languages.AllStatus, Value = 0 },
                new CategoricalData { Id = 1, Category = Languages.External, Value = 1 },
                new CategoricalData { Id = 2, Category = Languages.Internal, Value = 2 }
                };
            }
        }

        /// <summary>
        /// When select segmented control
        /// </summary>
        private void OnSelectionChanged()
        {
            this.FilterInfo = (SelectedIndex == 0) ? "All Trays" : "Current Tray";
        }
        #endregion
    }
}
