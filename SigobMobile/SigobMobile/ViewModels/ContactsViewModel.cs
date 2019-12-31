namespace SigobMobile.ViewModels
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Threading.Tasks;
    using Helpers;
    using Interfaces;
    using Plugin.Permissions;
    using Plugin.Permissions.Abstractions;
    using Common.Models;
    using Xamarin.Forms;

    public class ContactsViewModel : BaseViewModel
    {
        #region Attributes
        private ObservableCollection<PhoneContact> contactList;
        private List<PhoneContact> MobileContactsList;
        private bool isRefreshing;
        #endregion

        #region Properties
        public bool IsRefreshing
        {
            get => this.isRefreshing;
            set => SetValue(ref this.isRefreshing, value);
        }

        public ObservableCollection<PhoneContact> ContactList
        {
            get => this.contactList;
            set => SetValue(ref this.contactList, value);
        }
        #endregion

        #region Constructos
        public ContactsViewModel()
        {
            IErrorHandler errorHandler = null;
            this.LoadDeviceContacts().FireAndForgetSafeAsync(errorHandler);
        }
        #endregion

        #region Methods
        private async Task LoadDeviceContacts()
        {
            Page page = Application.Current.MainPage;
            IsRefreshing = true;
            var current = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Contacts);
            if (current != PermissionStatus.Granted)
            {
                if (Device.RuntimePlatform == Device.Android)
                {
                    var rationale = await CrossPermissions.Current.ShouldShowRequestPermissionRationaleAsync(Permission.Contacts);
                    if (rationale)
                    {
                        await page.DisplayAlert("Need it!", "Gimme permission", "Ok");
                    }
                }
                var status = await CrossPermissions.Current.RequestPermissionsAsync(new[] { Permission.Contacts });
                current = status[Permission.Contacts];
            }
            if (current == PermissionStatus.Granted)
            {
                var contacts = await DependencyService.Get<IUserContactsService>().GetAllContacts();
                this.MobileContactsList = new List<PhoneContact>((List<PhoneContact>)contacts);

                //this.MobileContacts = new ObservableCollection<MobileContactItemViewModel>(
                //    this.ToContactItemViewModel());
                ////Order by group Country
                //var sorted = from contact in MobileContacts
                //             orderby contact.FullName
                //             group contact by $"{contact.FullName.Substring(0, 1)}|{contact.FullName.Substring(0, 1)}|{ string.Empty }"
                //             into contactsGroup
                //             select new Grouping<string, MobileContactItemViewModel>(contactsGroup.Key, contactsGroup);

                //this.MobileContactsGrouped = new ObservableCollection<Grouping<string, MobileContactItemViewModel>>(sorted);
            }
            this.IsRefreshing = false;
        }

        #endregion

    }
}
