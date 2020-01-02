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
        private int index;
        private int itemCount;
        private bool isRegreshing;
        private bool isBusy;
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
        public bool IsRefreshing
        {
            get { return this.isRegreshing; }
            set { SetValue(ref this.isRegreshing, value); }
        }
        public int RowCount
        {
            get { return this.rowCount; }
            set { SetValue(ref this.rowCount, value); }
        }
        public ObservableCollection<DocumentItemViewModel> Documents
        {
            get { return this.documents; }
            set { SetValue(ref this.documents, value); }
        }
        #endregion

        #region Constructors
        //public DocumentsTrayViewModel()
        //{
        //    this.apiService = new ApiService();
        //}

        /// <summary>
        /// Constructor with Object Id parameter
        /// </summary>
        /// <param name="id"></param>
        public DocumentsTrayViewModel(int id)
        {
            this.id = id;
            this.apiService = new ApiService();
            IErrorHandler errorHandler = null;
            this.GetInitialDocumentList().FireAndForgetSafeAsync(errorHandler);
        }
        #endregion

        #region Commands
        public IAsyncCommand RefreshCommand => new AsyncCommand(this.Refresh);
        public IAsyncCommand LoadMoreDocumentsCommand => new AsyncCommand(this.LoadMoreDocumentsAsync);
        #endregion

        #region Medthods
        /// <summary>
        /// Firstt load when instance the constructor.
        /// </summary>
        /// <returns></returns>
        private async Task GetInitialDocumentList()
        {
            await Refresh();
            IsRefreshing = false;
        }

        /// <summary>
        /// Refresh List of Documents in Selected Tray
        /// </summary>
        /// <returns></returns>
        private async Task Refresh()
        {
            try
            {
                this.RowCount = this.index = 0;
                this.documentTray = await LoadDocumentsAsync(this.index);
                if (this.documentTray == null)
                {
                    return;
                }
                //Get List values
                this.RowCount = documentTray.Rows;
                this.documentList = documentTray.Documents;

                //Create document List
                this.Documents = new ObservableCollection<DocumentItemViewModel>(ToDocumentItemViewModel(this.documentList));

                // Increase index counter
                index += documentList.Count;
                ItemCount = index;
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert(
                    Languages.Error,
                    $"{Languages.GeneralError}: {ex.Message}",
                    Languages.Cancel);
            }
            finally { this.IsRefreshing = false; }
        }

        /// <summary>
        /// Load more documents async
        /// </summary>
        /// <returns></returns>
        private async Task LoadMoreDocumentsAsync()
        {
            if (isBusy) return;
            Tray newLoadTray;
            try
            {
                isBusy = true;
                newLoadTray = await this.LoadDocumentsAsync(this.index);
                if (newLoadTray == null)
                {
                return;
                }
                //Get List values
                this.RowCount = newLoadTray.Rows;
                List<Document> newDocumentList = newLoadTray.Documents;

                //Add new values to Document List Collection
                var partialPageDocuments = new List<DocumentItemViewModel>(ToDocumentItemViewModel(newDocumentList));
                foreach (DocumentItemViewModel doc in partialPageDocuments)
                {
                    this.Documents.Add(doc);
                    this.index += 1;
                }
                ItemCount = this.index;
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert(
                    Languages.Error,
                    $"{Languages.GeneralError}: {ex.Message}",
                    Languages.Cancel);
            }
            finally { isBusy = false; }
        }

        /// <summary>
        /// Load Documents
        /// </summary>
        /// <returns></returns>
        private async Task<Tray> LoadDocumentsAsync(int pageIndex)
        {
            Tray myDocumentTray = new Tray();
            try
            {
                // 1. Verify connection
                var connection = await this.apiService.CheckConnection();
                if (!connection.IsSuccess)
                {
                    this.IsRefreshing = false;
                    await Application.Current.MainPage.DisplayAlert(
                        Languages.Error,
                        connection.Message,
                        Languages.Cancel);
                    return myDocumentTray;
                }

                // 2. Get the menu list from API
                var response = await this.apiService.Get<Tray>(
                        Settings.UrlBaseApiSigob,
                        App.PrefixApiSigob,
                        $"{this.apiController}{id}{apiControllerSufix}{pageIndex}",
                        Settings.Token,
                        Settings.DbToken
                    );
                if (!response.IsSuccess)
                {
                    this.IsRefreshing = false;
                    await Application.Current.MainPage.DisplayAlert(
                        title: Languages.Error,
                        message: response.Message,
                        cancel: Languages.Cancel);
                    return myDocumentTray;
                }
                myDocumentTray = (Tray)response.Result;
                return myDocumentTray;
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert(
                    Languages.Error,
                    $"{Languages.GeneralError}: {ex.Message}",
                    Languages.Cancel);
                return myDocumentTray;
            }
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
