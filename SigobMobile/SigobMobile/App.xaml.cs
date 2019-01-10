using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace SigobMobile
{
    using Views;
    using Models;
    using Xamarin.Forms;


    public partial class App : Application
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
        /// Gets the prefix central.
        /// </summary>
        /// <value>The prefix central.</value>
        public static string PrefixCentral
        {
            get { return "institutions"; }
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
        /// Gets or sets the auth token.
        /// </summary>
        /// <value>The auth token.</value>
        public static string AuthToken
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the data base token.
        /// </summary>
        /// <value>The data base token.</value>
        public static string DataBaseToken
        {
            get;
            set;
        }
        #endregion

        #endregion

        #region Contructors
        public App()
        {
            InitializeComponent();

            this.MainPage = new NavigationPage(new LoginPage());
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
