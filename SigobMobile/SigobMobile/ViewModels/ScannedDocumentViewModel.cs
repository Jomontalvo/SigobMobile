namespace SigobMobile.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using AsyncAwaitBestPractices.MVVM;
    using Common.Services;
    using SigobMobile.Common.Helpers;
    using SigobMobile.Common.Models;
    using SigobMobile.Helpers;
    using SigobMobile.Interfaces;
    using Xamarin.Forms;

    public class ScannedDocumentViewModel : BaseViewModel
    {
        #region API Constants
        private readonly string apiController = "correspondences/externalcode/";
        private readonly string apiControllerScan = "/scandocs";
        private readonly string apiControllerScanId = "/scandoc/";
        private readonly string apiControllerType = "/type/";
        #endregion

        #region Attributes
        private readonly ApiService apiService;
        private bool isBusy;
        private bool isEnabled;
        private int selectedIndex = -1;
        private ScanDocument selectedItem;
        private readonly string registrationCode;
        private List<ScanDocument> scannedList;
        private IEnumerable<string> listImages;
        private Uri urlImagePdf;
        #endregion

        #region Properties
        public Uri UrlImagePdf
        {
            get => this.urlImagePdf;
            set => SetValue(ref this.urlImagePdf, value);
        }

        public ScanDocument SelectedItem
        {
            get => this.selectedItem;
            set
            {
                SetValue(ref this.selectedItem, value);
                IErrorHandler errorHandler = null;
                this.GetScannedDocument().FireAndForgetSafeAsync(errorHandler);
            } 
        }

        public int SelectedIndex
        {
            get => this.selectedIndex;
            set
            {
                if (selectedIndex != value && value >= 0)
                {
                    SetValue(ref this.selectedIndex, value);
                    // Trigger some action to take such as updating other labels or fields
                    SelectedItem = ScannedList[selectedIndex];
                }
            }
        }

        public List<ScanDocument> ScannedList
        {
            get => this.scannedList;
            set => SetValue(ref this.scannedList, value);
        }

        public bool IsBusy
        {
            get => this.isBusy;
            set => SetValue(ref this.isBusy, value);
        }

        public bool IsEnabled
        {
            get => this.isEnabled;
            set => SetValue(ref this.isEnabled, value);
        }
        #endregion

        #region Constructors
        public ScannedDocumentViewModel(string registrationCode)
        {
            this.SelectedIndex = -1;
            this.apiService = new ApiService();
            this.registrationCode = registrationCode;
            IErrorHandler errorHandler = null;
            this.LoadScannedDocuments().FireAndForgetSafeAsync(errorHandler);
        }
        #endregion

        #region Commands
        public IAsyncCommand CloseCommand => new AsyncCommand(Close);
        #endregion

        #region Methods
        /// <summary>
        /// Get Scanned Document
        /// </summary>
        /// <returns></returns>
        private async Task GetScannedDocument()
        {
            this.IsBusy = true;
            this.IsEnabled = false;
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
                // 2. Get the scanned document using API
                var response = await this.apiService.GetList<string>(
                        Settings.UrlBaseApiSigob,
                        App.PrefixApiSigob,
                        $"{this.apiController}{this.registrationCode}" +
                        $"{apiControllerScanId}{SelectedItem.Id }" +
                        $"{apiControllerType}{(byte)SelectedItem.ScannedType}",
                        Settings.Token,
                        Settings.DbToken
                    );
                if (!response.IsSuccess)
                {
                    this.IsBusy = false;
                    await App.Navigator.CurrentPage.DisplayAlert(
                        title: Languages.Error,
                        message: response.Message,
                        cancel: Languages.Cancel);
                    return;
                }
                this.listImages = (IEnumerable<string>)response.Result;
                if (listImages != null)
                {
                    Uri uri = new Uri(listImages.FirstOrDefault());
                    this.UrlImagePdf = uri;
                }
            }
            catch (Exception ex)
            {
                await App.Navigator.CurrentPage.DisplayAlert(
                    Languages.Error,
                    $"{Languages.GeneralError}: {ex.Message}",
                    Languages.Cancel);
            }
            finally
            {
                this.IsBusy = false;
                this.IsEnabled = true;
            }
        }

        /// <summary>
        /// Get List of Scanned Documents
        /// </summary>
        /// <returns></returns>
        private async Task LoadScannedDocuments()
        {
            this.IsBusy = true;
            this.IsEnabled = false;
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
                var response = await this.apiService.GetList<ScanDocument>(
                        Settings.UrlBaseApiSigob,
                        App.PrefixApiSigob,
                        $"{this.apiController}{this.registrationCode}{apiControllerScan}",
                        Settings.Token,
                        Settings.DbToken
                    );
                if (!response.IsSuccess)
                {
                    this.IsBusy = false;
                    await App.Navigator.CurrentPage.DisplayAlert(
                        title: Languages.Error,
                        message: response.Message,
                        cancel: Languages.Cancel);
                    return;
                }

                //Get List values
                this.ScannedList = (List<ScanDocument>)response.Result;
                this.SelectedIndex = 0; //First Scanned Document
            }
            catch (Exception ex)
            {
                await App.Navigator.CurrentPage.DisplayAlert(
                    Languages.Error,
                    $"{Languages.GeneralError}: {ex.Message}",
                    Languages.Cancel);
            }
            finally
            {
                IsBusy = false;
                IsEnabled = true;
            }
        }

        private Task Close()
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
