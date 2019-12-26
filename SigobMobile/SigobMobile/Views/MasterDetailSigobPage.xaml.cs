namespace SigobMobile.Views
{
    using Xamarin.Forms;
    using Xamarin.Forms.Xaml;

    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MasterDetailSigobPage : MasterDetailPage
    {
        //#region Properties
        //Dictionary<int, NavigationPage> MenuPages = new Dictionary<int, NavigationPage>();
        //#endregion

        #region Constructors
        public MasterDetailSigobPage()
        {
            InitializeComponent();
            //MainViewModel.GetInstance().Applications = new ApplicationsViewModel();
            ////MenuPages.Add((int)MenuItemType.Home, (NavigationPage)Detail);
            //MenuPages.Add((int)MenuItemType.Home, new NavigationPage(new ApplicationsPage()));
        }
        #endregion

        #region Methods
        protected override void OnAppearing()
        {
            base.OnAppearing();
            App.Navigator = this.Navigator;
            App.Master = this;
        }
        #endregion
    }
}
