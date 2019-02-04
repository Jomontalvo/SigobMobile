namespace SigobMobile.ViewModels
{
    using Helpers;
    using Services;
    using Models;
    using Xamarin.Forms;
    using System.Collections.ObjectModel;
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows.Input;
    using GalaSoft.MvvmLight.Command;
    using Views;

    public class ApplicationsViewModel : BaseViewModel
    {
        #region Services
        internal ApiService apiService;
        internal string MenuController = "user/appmodules";
        #endregion

        #region Attributes
        private ObservableCollection<ApplicationItemViewModel> applications;
        private bool isRefreshing;
        private List<ApplicationMenuItem> applicationList;
        #endregion

        #region Properties
        public ObservableCollection<ApplicationItemViewModel> Applications
        {
            get { return this.applications; }
            set { SetValue(ref this.applications, value); }
        }

        public bool IsRefreshing
        {
            get { return this.isRefreshing; }
            set { SetValue(ref this.isRefreshing, value); }
        }
        #endregion

        #region Constructors
        public ApplicationsViewModel()
        {
            this.apiService = new ApiService();
            this.LoadApplicationsMenu();
        }
        #endregion

        #region Methods
        private async void LoadApplicationsMenu()
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
            var response = await this.apiService.GetList<ApplicationMenuItem>(
                Settings.UrlBaseApiSigob,
                App.PrefixApiSigob,
                this.MenuController,
                Settings.Token,
                Settings.DbToken
            );
            if (!response.IsSuccess)
            {
                this.IsRefreshing = false;
                await Application.Current.MainPage.DisplayAlert(
                    Languages.Error,
                    response.Message,
                    Languages.Cancel);
                //Error in EndPoint call. Delete persist token values and go to Login Page
                Settings.Token = Settings.DbToken = string.Empty;
                var mainViewModel = MainViewModel.GetInstance();
                mainViewModel.Token = mainViewModel.DbToken = string.Empty;
                Application.Current.MainPage = new NavigationPage(new LoginPage());
                return;
            }
            this.applicationList = (List<ApplicationMenuItem>)response.Result;
            this.Applications = new ObservableCollection<ApplicationItemViewModel>(
                this.ToApplicationItemViewModel());
            this.IsRefreshing = false;
        }

        /// <summary>
        /// Convert List InsttitutionConnect Model to InstitutionItemView model.
        /// </summary>
        /// <returns>The institution item view mode.</returns>
        private IEnumerable<ApplicationItemViewModel> ToApplicationItemViewModel()
        {
            return this.applicationList.Select(l => new ApplicationItemViewModel
            {
                TypeApplication = l.TypeApplication,
                ApplicationName = l.ApplicationName,
                IsVisible = l.IsVisible,
                Message_1 = l.Message_1,
                Message_2 = l.Message_2,
                NewItems = l.NewItems
            });
        }
        #endregion

        #region Commands
        public ICommand RefreshCommand
        {
            get
            {
                return new RelayCommand(LoadApplicationsMenu);
            }
        }
        #endregion
    }
}
