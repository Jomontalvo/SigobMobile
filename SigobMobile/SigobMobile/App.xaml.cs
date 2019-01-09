using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace SigobMobile
{
    using Views;
    using Xamarin.Forms;


    public partial class App : Application
    {
        #region Global Constants
        /// <summary>
        /// Gets the URL base central.
        /// </summary>
        /// <value>The URL base central.</value>
        public static string UrlBaseCentral
        {
            get { return "https://sigob-movil.firebaseio.com"; }
        }

        public static string PrefixCentral
        {
            get { return "institutions"; }
        }

        public static string ControllerCentral
        {
            get { return ".json"; }
        }

        /// <summary>
        /// Gets or sets the URL base API Sigob.
        /// </summary>
        /// <value>The URL base API sigob.</value>
        public static string UrlBaseApiSigob
        {
            get; set;
        }
        public static string PrefixApiSigob
        {
            get { return "api"; }
        }

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
