namespace SigobMobile.ViewModels
{
    using System.Collections.ObjectModel;
    using System.Threading.Tasks;
    using Helpers;
    using Interfaces;
    using Plugin.ContactService;
    using Plugin.ContactService.Shared;

    public class ContactsViewModel : BaseViewModel
    {
        #region Attributes
        private ObservableCollection<Contact> contactList;
        #endregion

        #region Properties
        public ObservableCollection<Contact> ContactList
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

        #region Methos

        private async Task LoadDeviceContacts()
        {
            var contacts = await CrossContactService.Current.GetContactListAsync();
            ContactList = new ObservableCollection<Contact>(contacts);
        }

        #endregion

    }
}
