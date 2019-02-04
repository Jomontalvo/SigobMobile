namespace SigobMobile.Views
{
    using System.Collections.Generic;
    using Models;
    using Xamarin.Forms;
    using Xamarin.Forms.Xaml;

    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MenuPage : ContentPage
    {
        //#region Properties
        //MasterDetailSigobPage RootPage { get => Application.Current.MainPage as MasterDetailSigobPage; }
        //List<MasterPageItem> menuItems;
        //#endregion

        public MenuPage()
        {
            InitializeComponent();
            //menuItems = new List<MasterPageItem>
            //{
            //    new MasterPageItem {Id = MenuItemType.Home, Title="Home" },
            //    new MasterPageItem {Id = MenuItemType.Security, Title="Security" },
            //    new MasterPageItem {Id = MenuItemType.Help, Title="Help" },
            //    new MasterPageItem {Id = MenuItemType.TermsAndConditions, Title="Terms and Conditions" },
            //    new MasterPageItem {Id = MenuItemType.ContactUs, Title="Contact us" },
            //    new MasterPageItem {Id = MenuItemType.Logout, Title="Logout" }
            //};

            //ListViewMenu.ItemsSource = menuItems;

            //ListViewMenu.SelectedItem = menuItems[0];
            //ListViewMenu.ItemSelected += async (sender, e) =>
            //{
            //    if (e.SelectedItem == null)
            //        return;
            //    var id = (int)((MasterPageItem)e.SelectedItem).Id;
            //    await RootPage.NavigateFromMenu(id);
            //};
        }
    }
}
