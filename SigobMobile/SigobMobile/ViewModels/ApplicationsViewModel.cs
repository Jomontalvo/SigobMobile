namespace SigobMobile.ViewModels
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Windows.Input;
    using Helpers;
    using Models;
    using Services;
    using Views;
    using Xamarin.Forms;

    public class ApplicationsViewModel : BaseViewModel
    {
        #region Services
        internal ApiService apiService;
        internal string apiMenuController = "user/appmodules";
        #endregion

        #region Attributes
        private ObservableCollection<ApplicationItemViewModel> applications;
        private bool isRefreshing;
        private List<ApplicationMenuItem> applicationList;
        private bool CanExecuteLoad() => !IsRefreshing;
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
            Task.Run(async () => await this.LoadApplicationsMenu());
        }
        #endregion

        #region Commands
        public ICommand RefreshCommand => new AsyncCommand(LoadApplicationsMenu, CanExecuteLoad);
        public ICommand SearchCommand => new AsyncCommand(Search);
        #endregion

        #region Methods
        /// <summary>
        /// Loads the applications menu.
        /// </summary>
        private async Task LoadApplicationsMenu()
        {
            try
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
                    this.apiMenuController,
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
                    Settings.Token = Settings.DbToken = Settings.InstitutionLogo = string.Empty;
                    var mainViewModel = MainViewModel.GetInstance();
                    mainViewModel.Token = mainViewModel.DbToken = string.Empty;
                    Application.Current.MainPage = new NavigationPage(new LoginPage());
                    return;
                }
                this.applicationList = (List<ApplicationMenuItem>)response.Result;
                this.Applications = new ObservableCollection<ApplicationItemViewModel>(
                    ToApplicationItemViewModel());
            }
            finally
            {
                this.IsRefreshing = false;
            }
        }

        /// <summary>
        /// Convert List InstitutionConnect Model to InstitutionItemView model.
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
                Message_2 = (!l.IsVisible) ? string.Empty : l.Message_2,
                NewItems = l.NewItems
            });
        }

        /// <summary>
        /// Search this instance.
        /// </summary>
        private async Task Search()
        {
            await Application.Current.MainPage.DisplayAlert(
                title: Languages.Success,
                message: Languages.Ok,
                cancel: Languages.Accept);
            return;
        }
        #endregion
    }
}
