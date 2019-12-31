using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace SigobMobile
{
    using Common.Helpers;
    using Common.Models;
    using ViewModels;
    using Views;
    using Xamarin.Forms.PlatformConfiguration.iOSSpecific;

    public partial class App : Xamarin.Forms.Application
    {
        #region Global Constants and Variables

        #region Central API Firebase
        /// <summary>
        /// Gets the URL base central.
        /// </summary>
        /// <value>The URL base central.</value>
        public static string UrlBaseCentral
        {
            get { return "https://sigob-movil.firebaseio.com"; }
        }

        /// <summary>
        /// Gets the central institutions prefix.
        /// </summary>
        /// <value>The prefix central.</value>
        public static string PrefixCentral
        {
            get { return "institutions"; }
        }

        /// <summary>
        /// Gets the central parameters prefix
        /// </summary>
        public static string PrefixParametersCentral
        {
            get { return "parameters"; }
        }

        /// <summary>
        /// Gets the controller central.
        /// </summary>
        /// <value>The controller central.</value>
        public static string ControllerCentral
        {
            get { return ".json"; }
        }
        #endregion

        #region SIGOB Institution API
        /// <summary>
        /// Gets or sets the URL base API Sigob.
        /// </summary>
        /// <value>The URL base API sigob.</value>
        public static string UrlBaseApiSigob { get; set; }

        /// <summary>
        /// Gets the prefix API sigob.
        /// </summary>
        /// <value>The prefix API sigob.</value>
        public static string PrefixApiSigob
        {
            get { return "api/"; }
        }
        #endregion

        #region SIGOB Active Session
        /// <summary>
        /// Gets or sets the active session.
        /// </summary>
        /// <value>The active session.</value>
        public static SessionSigob ActiveSession
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the language error.
        /// </summary>
        /// <value>The language error.</value>
        public string LanguageError
        {
            get;
            set;
        }
        #endregion

        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets the navigator property.
        /// </summary>
        /// <value>The navigator.</value>
        public static Xamarin.Forms.NavigationPage Navigator { get; internal set; }
        public static MasterDetailSigobPage Master { get; internal set; }
        
        #endregion


        #region Contructors
        /// <summary>
        /// Initializes a new instance of the <see cref="T:SigobMobile.App"/> class.
        /// </summary>
        public App()
        {
            InitializeComponent();
            //Settings.Token = Settings.DbToken = Settings.InstitutionLogo =  string.Empty;
            if (string.IsNullOrEmpty(Settings.Token))
            {
                var navPage = new Xamarin.Forms.NavigationPage(new LoginPage());
                navPage.On<Xamarin.Forms.PlatformConfiguration.iOS>().SetPrefersLargeTitles(true);
                navPage.On<Xamarin.Forms.PlatformConfiguration.iOS>().SetLargeTitleDisplay(LargeTitleDisplayMode.Automatic);
                this.MainPage = navPage;
            }
            else
            {
                var mainViewModel = MainViewModel.GetInstance();

                mainViewModel.Token = Settings.Token;
                mainViewModel.DbToken = Settings.DbToken;

                //Load Master Detail with ApplicationsPage
                mainViewModel.Menu = new MenuViewModel();
                mainViewModel.Applications = new ApplicationsViewModel() { IsRefreshing = false };
                var mainSigobPage = new MasterDetailSigobPage();
                //NavigatePage properties IphoneX
                Xamarin.Forms.NavigationPage navDetailPage = (Xamarin.Forms.NavigationPage)mainSigobPage.Detail;
                navDetailPage.On<Xamarin.Forms.PlatformConfiguration.iOS>().SetPrefersLargeTitles(false);
                navDetailPage.On<Xamarin.Forms.PlatformConfiguration.iOS>().SetLargeTitleDisplay(LargeTitleDisplayMode.Never);
                Xamarin.Forms.Application.Current.MainPage = mainSigobPage;
            }
        }
        #endregion

        #region Methods
        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
        #endregion
    }
}
