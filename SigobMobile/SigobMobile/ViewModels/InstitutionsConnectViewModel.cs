namespace SigobMobile.ViewModels
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Windows.Input;
    using GalaSoft.MvvmLight.Command;
    using Models;
    using Services;
    using Helpers;
    using Xamarin.Forms;
    using System.Linq;
    using System;

    public class InstitutionsConnectViewModel : BaseViewModel
    {
        #region Services
        internal ApiService apiService;
        #endregion

        #region Attributes
        private ObservableCollection<InstitutionConnect> institutions;
        private ObservableCollection<Grouping<string, InstitutionConnect>> institutionsGrouped;

        private bool isRefreshing;
        #endregion

        #region Properties
        public ObservableCollection<InstitutionConnect> Institutions
        {
            get { return this.institutions; }
            set { SetValue(ref this.institutions, value); }
        }

        public ObservableCollection<Grouping<string, InstitutionConnect>> InstitutionsGrouped
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
            this.apiService = new ApiService();
            this.LoadInstitutionsToConnect();
        }
        #endregion

        #region Methods
        private async void LoadInstitutionsToConnect()
        {
            this.IsRefreshing = true;
            var connection = await this.apiService.CheckConnection();
            if (!connection.IsSuccess)
            {
                this.IsRefreshing = false;
                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    connection.Message,
                    "Cancel");
                //Back to Login
                return;
            }

            var response = await this.apiService.GetList<InstitutionConnect>(
                "https://sigob-movil.firebaseio.com",
                "institutions", 
                ".json"
            );
            if (!response.IsSuccess)
            {
                this.IsRefreshing = false;
                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    response.Message,
                    "Cancel");
                //Back to login
                return;
            }
            var list = (List<InstitutionConnect>)response.Result;
            this.Institutions = new ObservableCollection<InstitutionConnect>(list);
            //Order by group Country
            var sorted = from institution in Institutions
                         orderby institution.Country
                         group institution by institution.Country into institutionGroup
                         select new Grouping<string, InstitutionConnect>(institutionGroup.Key, institutionGroup);

            this.InstitutionsGrouped = new ObservableCollection<Grouping<string, InstitutionConnect>>(sorted);
            this.IsRefreshing = false;
        }

        private async void SelectItem()
        {
            await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    "Selected item!",
                    "Cancel");
            //Back to login
            return;
        }
        #endregion

        #region Commands
        public ICommand RefreshCommand
        {
            get
            {
                return new RelayCommand(LoadInstitutionsToConnect);
            }
        }
        #endregion
    }
}
