namespace SigobMobile.Views
{
    using SigobMobile.ViewModels;
    using Xamarin.Forms;
    using Models;
    public partial class InstitutionsConnectPage : ContentPage
    {
        public InstitutionsConnectPage()
        {
            InitializeComponent();
            //this.BindingContext = new InstitutionsConnectViewModel();
        }

        private async void OnItemTapped(object sender, ItemTappedEventArgs e)
        {
            var x = (InstitutionConnect)e.Item;
            await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    $"Selected item! {x.UrlApiService}",
                    "Cancel");
            MainViewModel.GetInstance().Login = new LoginViewModel();
            Application.Current.MainPage = new LoginPage();
        }
    }
}
