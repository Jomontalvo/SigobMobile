namespace SigobMobile.Views.Correspondence
{
    using Xamarin.Forms;

    public partial class SearchDocumentPage : ContentPage
    {
        public SearchDocumentPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            searchBarDocs.Focus();
        }
    }
}
