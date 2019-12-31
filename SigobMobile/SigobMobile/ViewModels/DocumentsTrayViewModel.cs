namespace SigobMobile.ViewModels
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Threading.Tasks;
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
        private ApiService apiService;
        private int id;
        private int index;
        private int itemCount;
        private bool isRegreshing;
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
        public DocumentsTrayViewModel()
        {
            this.apiService = new ApiService();
        }

        public DocumentsTrayViewModel(int id)
        {
            this.id = id;
            this.apiService = new ApiService();
            IErrorHandler errorHandler = null;
            this.LoadDocumentsAsync().FireAndForgetSafeAsync(errorHandler);
        }
        #endregion


        #region Medthods

        private async Task LoadDocumentsAsync()
        {
            this.IsRefreshing = true;
            // 1. Verify connection
            var connection = await this.apiService.CheckConnection();
            if (!connection.IsSuccess)
            {
                this.IsRefreshing = false;
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
                    $"{this.apiController}{id}{apiControllerSufix}{index}",
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
                return;
            }
            this.documentTray = (Tray)response.Result;
            this.RefreshDocumentList();
            this.IsRefreshing = false;
        }

        private void RefreshDocumentList()
        {
            this.documentList = documentTray.Documents;
            Documents = new ObservableCollection<DocumentItemViewModel>(this.documentList.Select(d => new DocumentItemViewModel
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
            }
            ));
            ItemCount = documentTray.Rows;
        }
        #endregion
    }
}
