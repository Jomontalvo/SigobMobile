namespace SigobMobile.Views
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Models;
    using ViewModels;
    using Xamarin.Forms;
    using Xamarin.Forms.Xaml;

    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MasterDetailSigobPage : MasterDetailPage
    {
        #region Properties
        Dictionary<int, NavigationPage> MenuPages = new Dictionary<int, NavigationPage>();
        #endregion

        #region Constructors
        public MasterDetailSigobPage()
        {
            InitializeComponent();
            MainViewModel.GetInstance().Applications = new ApplicationsViewModel();
            //MenuPages.Add((int)MenuItemType.Home, (NavigationPage)Detail);
            MenuPages.Add((int)MenuItemType.Home, new NavigationPage(new ApplicationsPage()));
        }
        #endregion

        #region Methods
        public async Task NavigateFromMenu(int id)
        {
            if (!MenuPages.ContainsKey(id))
            {
                var mainViewModel = MainViewModel.GetInstance();
                switch (id)
                {
                    case (int)MenuItemType.Home:
                        mainViewModel.Applications = new ApplicationsViewModel();
                        MenuPages.Add(id, new NavigationPage(new ApplicationsPage()));
                        break;
                    case (int)MenuItemType.Security:
                        mainViewModel.Security = new SecurityViewModel();
                        MenuPages.Add(id, new NavigationPage(new SecurityPage()));
                        break;
                }
            }

            var newPage = MenuPages[id];

            if (newPage != null && Detail != newPage)
            {
                Detail = newPage;

                if (Device.RuntimePlatform == Device.Android)
                    await Task.Delay(100);

                IsPresented = false;
            }
        }
        #endregion
    }
}
