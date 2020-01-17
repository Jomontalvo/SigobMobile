namespace SigobMobile.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using Common.Services;
    using AsyncAwaitBestPractices.MVVM;
    using System.Threading.Tasks;
    using Xamarin.Forms;
    using SigobMobile.Helpers;
    using SigobMobile.Common.Helpers;
    using SigobMobile.Interfaces;

    public class ScannedTiffViewModel : BaseViewModel
    {

        #region API Constants
        private readonly string apiController = "correspondences/externalcode/";
        private readonly string apiControllerScanId = "/scandocs/";
        private readonly string apiControllerType = "/type";
        private readonly int scanId;
        private readonly string registrationCode;
        private readonly byte typeScan;
        #endregion

        #region Attributes
        private bool isBusy;
        private bool isEnabled;
        private ApiService apiService;
        private List<string> listImages;
        private ObservableCollection<string> images;
        #endregion

        #region Properties
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

        public ObservableCollection<string> Images
        {
            get => this.images;
            set => SetValue(ref this.images, value);
        }
        #endregion

        #region Constructors
        public ScannedTiffViewModel(int scanId, string registrationCode, byte typeScan)
        {
            this.apiService = new ApiService();
            this.scanId = scanId;
            this.registrationCode = registrationCode;
            this.typeScan = typeScan;
            IErrorHandler errorHandler = null;
            this.LoadScannedImages().FireAndForgetSafeAsync(errorHandler);
        }
        #endregion

        #region Commands
        public IAsyncCommand CloseCommand => new AsyncCommand(Close);

        
        #endregion

        #region Methods
        private async Task LoadScannedImages()
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
                var response = await this.apiService.GetList<string>(
                        Settings.UrlBaseApiSigob,
                        App.PrefixApiSigob,
                        $"{this.apiController}{this.registrationCode}{apiControllerScanId}{this.scanId}{apiControllerType}{this.typeScan}",
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
                this.listImages = (List<string>)response.Result;
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
