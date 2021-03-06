﻿namespace SigobMobile.ViewModels
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Threading.Tasks;
    using AsyncAwaitBestPractices.MVVM;
    using Common.Models;
    using Common.Services;
    using Helpers;
    using Interfaces;
    using Xamarin.Forms;

    public class InstitutionsConnectViewModel : BaseViewModel
    {
        #region Services
        internal ApiService apiService;
        #endregion

        #region Attributes
        private ObservableCollection<InstitutionItemViewModel> institutions;
        private ObservableCollection<Grouping<string, InstitutionItemViewModel>> institutionsGrouped;
        private bool isRefreshing;
        private List<InstitutionConnect> institutionList;
        #endregion

        #region Properties
        public ObservableCollection<InstitutionItemViewModel> Institutions
        {
            get { return this.institutions; }
            set { SetValue(ref this.institutions, value); }
        }

        public ObservableCollection<Grouping<string, InstitutionItemViewModel>> InstitutionsGrouped
        {
            get { return this.institutionsGrouped; }
            set { SetValue(ref this.institutionsGrouped, value); }
        }

        public bool IsRefreshing
        {
            get { return this.isRefreshing; }
            set { SetValue(ref this.isRefreshing, value); }
        }
        #endregion

        #region Constructors
        public InstitutionsConnectViewModel()
        {
            IErrorHandler errorHandler = null;
            this.apiService = new ApiService();
            this.LoadInstitutionsToConnect().FireAndForgetSafeAsync(errorHandler);
        }
        #endregion

        #region Methods
        /// <summary>
        /// Loads the institutions (with Sigob API service) to connect.
        /// </summary>
        private async Task LoadInstitutionsToConnect()
        {
            this.IsRefreshing = true;
            var connection = await this.apiService.CheckConnection();
            if (!connection.IsSuccess)
            {
                this.IsRefreshing = false;
                await Application.Current.MainPage.DisplayAlert(
                    Languages.Error,
                    connection.Message,
                    Languages.Cancel);
                await Application.Current.MainPage.Navigation.PopAsync();
                return;
            }

            var response = await this.apiService.GetList<InstitutionConnect>(
                App.UrlBaseCentral,
                App.PrefixCentral, 
                App.ControllerCentral
            );
            if (!response.IsSuccess)
            {
                this.IsRefreshing = false;
                await Application.Current.MainPage.DisplayAlert(
                    Languages.Error,
                    response.Message,
                    Languages.Cancel);
                await Application.Current.MainPage.Navigation.PopAsync();
                return;
            }
            this.institutionList = (List<InstitutionConnect>)response.Result;
            this.Institutions = new ObservableCollection<InstitutionItemViewModel>(
                this.ToInstitutionItemViewModel());

            //Order by group Country
            var sorted = from institution in Institutions
                         orderby institution.Country
                         group institution by $"{institution.NameSort}|{institution.Country}|{ institution.ISOCountryCode}"
                         into institutionGroup
                         select new Grouping<string, InstitutionItemViewModel>(institutionGroup.Key, institutionGroup);

            this.InstitutionsGrouped = new ObservableCollection<Grouping<string, InstitutionItemViewModel>>(sorted);
            this.IsRefreshing = false;
        }

        /// <summary>
        /// Convert List InsttitutionConnect Model to InstitutionItemView model.
        /// </summary>
        /// <returns>The institution item view mode.</returns>
        private IEnumerable<InstitutionItemViewModel> ToInstitutionItemViewModel()
        {
            return this.institutionList.Select(l => new InstitutionItemViewModel
            {
                Country = l.Country,
                EndDateValidity = l.EndDateValidity,
                Id = l.Id,
                ISOCountryCode = l.ISOCountryCode,
                Institution = l.Institution,
                SecurityLogin = l.SecurityLogin,
                Status = l.Status,
                UrlApiService = l.UrlApiService
            });
        }
        #endregion

        #region Commands
        public IAsyncCommand RefreshCommand => new AsyncCommand(LoadInstitutionsToConnect);
        #endregion
    }
}
