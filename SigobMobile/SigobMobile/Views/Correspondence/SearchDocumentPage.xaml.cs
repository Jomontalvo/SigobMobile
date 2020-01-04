using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace SigobMobile.Views.Correspondence
{
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
