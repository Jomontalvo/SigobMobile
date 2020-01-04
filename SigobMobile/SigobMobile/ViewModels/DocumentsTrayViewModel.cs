namespace SigobMobile.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Threading.Tasks;
    using AsyncAwaitBestPractices.MVVM;
    using Common.Helpers;
    using Common.Models;
    using Common.Services;
    using Helpers;
    using Interfaces;
    using Views.Correspondence;
    using Xamarin.Forms;
    using DocumentItemViewModel = Models.Document;

    public class DocumentsTrayViewModel : BaseViewModel
    {
        #region API Constants 
        private readonly string apiController = "correspondences/trays/";
        private readonly string apiControllerSufix = "/index/";
        #endregion

        #region Attributes
        private readonly ApiService apiService;
        private readonly int id;
        private int rowPageIndex;
        private int itemCount;
        private string pagingInfoLabel;
        private bool isRegreshing;
        private bool isBusy;
        private bool isVisibleSearch;
        private Tray documentTray;
        private int rowCount;
        private List<Document> documentList;
        private ObservableCollection<DocumentItemViewModel> documents;

        #endregion

        #region Properties
        public int ItemCount
        {
            get { return this.itemCount; }
            set { SetValue(ref this.itemCount, value); }
        }
        public bool IsVisibleSearch
        {
            get { return this.isVisibleSearch; }
            set { SetValue(ref this.isVisibleSearch, value); }
        }
        public bool IsRefreshing
        {
            get { return this.isRegreshing; }
            set { SetValue(ref this.isRegreshing, value); }
        }
        public bool IsBusy
        {
            get { return this.isBusy; }
            set { SetValue(ref this.isBusy, value); }
        }
        public int RowCount
        {
            get { return this.rowCount; }
            set { SetValue(ref this.rowCount, value); }
        }
        public string PagingInfoLabel
        {
            get { return this.pagingInfoLabel; }
            set { SetValue(ref this.pagingInfoLabel, value); }
        }
        public ObservableCollection<DocumentItemViewModel> Documents
        {
            get { return this.documents; }
            set { SetValue(ref this.documents, value); }
        }
        #endregion

        #region Constructors
        /// <summary>
        /// Cionstructor for Search from Archive
        /// </summary>
        public DocumentsTrayViewModel()
        {
            this.apiService = new ApiService();
        }

        /// <summary>
        /// Constructor with Object Id parameter
        /// </summary>
        /// <param name="id"></param>
        public DocumentsTrayViewModel(int id)
        {
            this.id = id;
            this.IsVisibleSearch = false;
            this.apiService = new ApiService();
            this.Documents = new ObservableCollection<DocumentItemViewModel>();
            this.rowPageIndex = 0;
            IErrorHandler errorHandler = null;
            this.LoadDocumentsAsync().FireAndForgetSafeAsync(errorHandler);
        }
        #endregion

        #region Commands
        public IAsyncCommand AdvancedSearchCommand => new AsyncCommand(this.AdvancedSearch);
        public IAsyncCommand SearchCurrentTrayCommand => new AsyncCommand(this.SearchCurrentTray);
        public IAsyncCommand RefreshCommand => new AsyncCommand(this.Refresh);
        public IAsyncCommand LoadMoreDocumentsCommand => new AsyncCommand(this.LoadDocumentsAsync);
        #endregion

        #region Medthods

        /// <summary>
        /// Find in current Observable Collection
        /// </summary>
        /// <returns></returns>
        private Task SearchCurrentTray()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Call new Modal View with Search options
        /// </summary>
        private async Task AdvancedSearch()
        {
            var mainViewModel = MainViewModel.GetInstance();
            mainViewModel.SearchDocument = new SearchDocumentViewModel();
            await App.Navigator.CurrentPage.Navigation.PushModalAsync(new SearchDocumentPage(),true);
        }

        /// <summary>
        /// Refresh List of Documents in Selected Tray
        /// </summary>
        /// <returns></returns>
        private async Task Refresh()
        {
            this.IsRefreshing = true;
            this.ItemCount = this.rowPageIndex = 0;
            this.Documents = new ObservableCollection<DocumentItemViewModel>();
            await LoadDocumentsAsync();
            this.IsRefreshing = false;
            //}
        }

        /// <summary>
        /// Load Documents
        /// </summary>
        /// <returns></returns>
        private async Task LoadDocumentsAsync()
        {
            if (this.IsBusy) return;
            this.IsBusy = true;
            try
            {
                // 1. Verify connection
                var connection = await this.apiService.CheckConnection();
                if (!connection.IsSuccess)
                {
                    this.IsBusy = false;
                    await Application.Current.MainPage.DisplayAlert(
                        Languages.Error,
                        connection.Message,
                        Languages.Cancel);
                    return;
                }

                // 2. Get the menu list from API
                var response = await this.apiService.Get<Tray>(
                        Settings.UrlBaseApiSigob,
                        App.PrefixApiSigob,
                        $"{this.apiController}{id}{apiControllerSufix}{rowPageIndex}",
                        Settings.Token,
                        Settings.DbToken
                    );
                if (!response.IsSuccess)
                {
                    this.IsBusy = false;
                    await Application.Current.MainPage.DisplayAlert(
                        title: Languages.Error,
                        message: response.Message,
                        cancel: Languages.Cancel);
                    return;
                }

                //Get List values
                this.documentTray = (Tray)response.Result;
                this.RowCount = documentTray.Rows;
                this.documentList = documentTray.Documents;
                if (!this.documentList.Any()) return;

                //Get the last Index to prevent unnecessary new Load
                this.rowPageIndex = Convert.ToInt32(this.documentList.Select(d => d.Index).Last());
                this.ItemCount = this.rowPageIndex;
                // Append new List
                List<DocumentItemViewModel> partialDocumentPage = new List<DocumentItemViewModel>(ToDocumentItemViewModel(this.documentList));
                var newCollection = new ObservableCollection<DocumentItemViewModel>(partialDocumentPage);
                this.Documents = ConcatCollections(newCollection);
                this.PagingInfoLabel = $"{Languages.ShowRowCount}: {this.ItemCount}, {Languages.TotalRows}: {this.RowCount} ";
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert(
                    Languages.Error,
                    $"{Languages.GeneralError}: {ex.Message}",
                    Languages.Cancel);
            }
            finally
            {
                IsBusy = false;
            }
        }

        /// <summary>
        /// Concat two observable collection
        /// </summary>
        /// <param name="newCollection"></param>
        /// <returns></returns>
        private ObservableCollection<DocumentItemViewModel> ConcatCollections(ObservableCollection<DocumentItemViewModel> newCollection)
        {
            ObservableCollection<DocumentItemViewModel> currentDocuments = this.Documents;
            return new ObservableCollection<DocumentItemViewModel>(currentDocuments.Union(newCollection));
        }

        /// <summary>
        /// Convert List of DocumentItemViewModel to Observable Collection
        /// </summary>
        /// <returns></returns>
        private IEnumerable<DocumentItemViewModel> ToDocumentItemViewModel(List<Document> list)
        {
            return list.Select(d => new DocumentItemViewModel
            {
                Advertisement = d.Advertisement,
                AttachmentCount = d.AttachmentCount,
                Copy = d.Copy,
                DaysStayInArea = d.DaysStayInArea,
                DaysStayInOffice = d.DaysStayInOffice,
                Id = d.Id,
                Index = d.Index,
                Institution = d.Institution,
                IsReviewed = d.IsReviewed,
                IssuerOrRecipient = d.IssuerOrRecipient,
                Priority = d.Priority,
                RegistrationCode = d.RegistrationCode,
                RegistrationDate = d.RegistrationDate,
                Reply = d.Reply,
                Source = d.Source,
                Status = d.Status,
                StatusTracking = d.StatusTracking,
                Subject = d.Subject,
                Text = d.Text,
                TrackingId = d.TrackingId,
                TransferComment = d.TransferComment,
                TransferedBy = d.TransferedBy
            });
        }
        #endregion
    }
}
