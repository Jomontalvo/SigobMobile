namespace SigobMobile.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Threading.Tasks;
    using AsyncAwaitBestPractices.MVVM;
    using SigobMobile.Common.Helpers;
    using SigobMobile.Common.Models;
    using SigobMobile.Common.Services;
    using SigobMobile.Helpers;
    using SigobMobile.Interfaces;
    using SigobMobile.Models;
    using SigobMobile.Views.Correspondence;
    using Xamarin.Forms;

    /// <summary>
    /// View model of Corresaapondence Menu
    /// </summary>
    public class MailBoxesViewModel : BaseViewModel
    {
        #region Attributes
        private readonly ApiService apiService;
        private readonly string apiController = "correspondences/menu";
        private bool isRefreshing;
        private CorrespondenceMenu menu;
        private List<DocumentTray> documentTrays;
        private List<DocumentTagTray> documentTagTrays;
        private List<MailBoxMenuGroup> trayMenuList;
        private ObservableCollection<MailBoxMenuGroup> trays;
        #endregion

        #region Properties
        public ObservableCollection<MailBoxMenuGroup> Trays
        {
            get { return this.trays; }
            set { SetValue(ref this.trays, value); }
        }

        public bool IsRefreshing
        {
            get { return this.isRefreshing; }
            set { SetValue(ref this.isRefreshing, value); }
        }

        #endregion

        #region Constructor
        public MailBoxesViewModel()
        {
            this.apiService = new ApiService();
            IErrorHandler errorHandler = null;
            this.RefreshAsync().FireAndForgetSafeAsync(errorHandler);
        }
        #endregion

        #region Commands
        public IAsyncCommand RefreshCommand => new AsyncCommand(this.RefreshAsync);
        public IAsyncCommand SearchCommand => new AsyncCommand(this.SearchAsync);
        #endregion

        #region Methods

        /// <summary>
        /// Call Search Service
        /// </summary>
        /// <returns></returns>
        private async Task SearchAsync()
        {
            int parentId = -1;
            var mainViewModel = MainViewModel.GetInstance();
            mainViewModel.SearchDocument = new SearchDocumentViewModel(parentId);
            await App.Navigator.CurrentPage.Navigation.PushModalAsync(new SearchDocumentPage(), true);
        }

        /// <summary>
        /// Refresh List of Trays
        /// </summary>
        /// <returns></returns>
        private async Task RefreshAsync()
        {
            this.IsRefreshing = true;
            await this.LoadTraysAsync();
            this.IsRefreshing = false;
            return;
        }

        /// <summary>
        /// Load Correspondence Try
        /// </summary>
        /// <returns></returns>
        public async Task LoadTraysAsync()
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
                return;
            }

            // 2. Get the menu list from API
            var response = await this.apiService.Get<CorrespondenceMenu>(
                    Settings.UrlBaseApiSigob,
                    App.PrefixApiSigob,
                    this.apiController,
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
            this.menu = (CorrespondenceMenu)response.Result;
            this.RefreshMenuTrayList();
        }

        /// <summary>
        /// Refresh Menu of Correspondence Trays
        /// </summary>
        private void RefreshMenuTrayList()
        {
            this.documentTrays = menu.DocumentTrays;
            this.documentTagTrays = menu.DocumentTagTrays;
            this.trayMenuList = new List<MailBoxMenuGroup>
            {
                new MailBoxMenuGroup(Languages.ManagementTray,
                new List<MailBoxMenuItemViewModel>(this.documentTrays.Select(m => new MailBoxMenuItemViewModel
                {
                    Id = m.Id,
                    Name = ((DocumentTrayId)m.Id) switch
                    {
                        DocumentTrayId.All => Languages.AllStatus,
                        DocumentTrayId.Draft => Languages.Draft,
                        DocumentTrayId.External => Languages.External,
                        DocumentTrayId.ForYourInformation => Languages.ForYourInformation,
                        DocumentTrayId.Internal => Languages.Internal,
                        DocumentTrayId.Pending => Languages.Pending,
                        DocumentTrayId.PendingEmail => Languages.PendingEmail,
                        DocumentTrayId.PendingLinkedInstitution => Languages.PendingLinkedInstitution,
                        DocumentTrayId.ProcessedToConfirm => Languages.ProcessedToConfirm,
                        DocumentTrayId.Trasferred => Languages.Trasferred,
                        DocumentTrayId.WithDeadline => Languages.WithDeadline,
                        _ => m.Name
                    },
                    IsSelected = (Settings.DocumentTraySelected == m.Id),
                    ItemCount = m.ItemCount
                }))
                ),
                new MailBoxMenuGroup(Languages.TagTray,
                new List<MailBoxMenuItemViewModel>(this.documentTagTrays.Select(m => new MailBoxMenuItemViewModel
                {
                    Id = Convert.ToInt32(m.Key) + 10,
                    Key = m.Key,
                    Name = m.Name,
                    IsSelected = (Settings.DocumentTraySelected == Convert.ToInt32(m.Key) + 10),
                    ItemCount = m.ItemCount
                }))
                )
            };
            //Load ObservableCollection
            this.Trays = new ObservableCollection<MailBoxMenuGroup>(this.trayMenuList);
        }
        #endregion
    }
}