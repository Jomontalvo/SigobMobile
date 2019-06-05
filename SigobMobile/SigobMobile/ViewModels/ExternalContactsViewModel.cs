namespace SigobMobile.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Windows.Input;
    using AsyncAwaitBestPractices.MVVM;
    using GalaSoft.MvvmLight.Command;
    using Helpers;
    using Interfaces;
    using Models;
    using Plugin.Permissions;
    using Plugin.Permissions.Abstractions;
    using Services;
    using Xamarin.Forms;

    public class ExternalContactsViewModel : BaseViewModel
    {

        #region Services
        internal ApiService apiService;
        #endregion

        #region ApiControllers
        internal string apiContactsController = "participants/event/{0}/externalguests/index/{1}?pattern={2}";
        #endregion

        #region Attributes
        private readonly ParticipantsViewModel participantsViewModel;
        private string filter;
        private int rowPageIndex;
        private int idEvent;
        private List<Participant> externalContactList;
        private ObservableCollection<ParticipantItemViewModel> partialPageContacts;
        private ObservableCollection<ParticipantItemViewModel> institutionalDirectoryContacts;
        private ObservableCollection<Grouping<string, MobileContactItemViewModel>> mobileContactsGrouped;
        private bool isSelectedMainList;
        private bool isBusy;
        private bool isRefreshing;
        private int selectedIndex;
        private ObservableCollection<MobileContactItemViewModel> mobileContacts;
        private string searchPlaceHolderText;
        #endregion

        #region Properties
        public ObservableCollection<string> Categories { get; set; }
        public int SelectedCategory
        {
            get => this.selectedIndex;
            set
            {
                SetValue(ref this.selectedIndex, value);
                this.IsSelectedMainList = !IsSelectedMainList;
                this.OnSelectionChanged();
            }
        }

        public List<PhoneContact> MobileContactsList { get; set; }

        public ObservableCollection<Grouping<string, MobileContactItemViewModel>> MobileContactsGrouped
        {
            get { return this.mobileContactsGrouped; }
            set { SetValue(ref this.mobileContactsGrouped, value); }
        }

        public ObservableCollection<MobileContactItemViewModel> MobileContacts
        {
            get => this.mobileContacts;
            set => SetValue(ref this.mobileContacts, value);
        }

        public string SearchPlaceHolderText
        {
            get => this.searchPlaceHolderText;
            set => SetValue(ref this.searchPlaceHolderText, value);
        }

        public bool IsBusy
        {
            get => this.isBusy;
            set => SetValue(ref this.isBusy, value);
        }

        public bool IsRefreshing
        {
            get { return this.isRefreshing; }
            set { SetValue(ref this.isRefreshing, value); }
        }

        public bool IsSelectedMainList
        {
            get => this.isSelectedMainList;
            set => SetValue(ref this.isSelectedMainList, value);
        }

        public string Filter
        {
            get => this.filter;
            set
            {
                SetValue(ref this.filter, value);
                this.SearchWhenEmpty();
            }
        }
        public ObservableCollection<ParticipantItemViewModel> InstitutionalDirectoryContacts
        {
            get => this.institutionalDirectoryContacts;
            set => SetValue(ref this.institutionalDirectoryContacts, value);
        }
        #endregion

        #region Constructors
        public ExternalContactsViewModel(ParticipantsViewModel participantsViewModel)
        {
            this.participantsViewModel = participantsViewModel;
            this.LoadMainSettings();
        }
        #endregion

        #region Methods
        /// <summary>
        /// Loads the main settings.
        /// </summary>
        private void LoadMainSettings()
        {
            IErrorHandler errorHandler = null;
            this.apiService = new ApiService();
            this.Categories = new ObservableCollection<string>() { Languages.InstitutionalDirectoryTitle, Languages.ContactsTitle };
            this.InstitutionalDirectoryContacts = new ObservableCollection<ParticipantItemViewModel>();
            this.idEvent = participantsViewModel.eventCgViewModel.LocalEvent.Id;
            this.rowPageIndex = 0;
            this.SelectedCategory = 0;
            this.IsSelectedMainList = false;
            this.LoadContacts().FireAndForgetSafeAsync(errorHandler);
            this.LoadDeviceContacts().FireAndForgetSafeAsync(errorHandler);
        }

        /// <summary>
        /// Loads the device contacts.
        /// </summary>
        /// <returns>The device contacts.</returns>
        private async Task LoadDeviceContacts()
        {
            Page page = Application.Current.MainPage;
            IsRefreshing = true;
            var current = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Contacts);
            if(current != PermissionStatus.Granted)
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
            if(current == PermissionStatus.Granted)
            {
                var contacts = await DependencyService.Get<IUserContactsService>().GetAllContacts();
                this.MobileContactsList = new List<PhoneContact>((List<PhoneContact>)contacts);
                //this.MobileContacts = new ObservableCollection<PhoneContact>(contacts.OrderBy(c => c.FullName));

                this.MobileContacts = new ObservableCollection<MobileContactItemViewModel>(
                    this.ToContactItemViewModel());
                //Order by group Country
                var sorted = from contact in MobileContacts
                             orderby contact.FullName
                             group contact by $"{contact.FullName.Substring(0,1)}|{contact.FullName.Substring(0, 1)}|{ string.Empty }"
                             into contactsGroup
                             select new Grouping<string, MobileContactItemViewModel>(contactsGroup.Key, contactsGroup);

                this.MobileContactsGrouped = new ObservableCollection<Grouping<string, MobileContactItemViewModel>>(sorted);
            }
            this.IsRefreshing = false;
        }

        /// <summary>
        /// Convert the contact list in Contact Item view model list (enable commands)
        /// </summary>
        /// <returns>The contact item view model.</returns>
        private IEnumerable<MobileContactItemViewModel> ToContactItemViewModel()
        {
            return this.MobileContactsList.Select(l => new MobileContactItemViewModel
            {
                 FullName = l.FullName,
                 PhoneNumber = l.PhoneNumber,
                 Email = l.Email,
                 PhotoThumbnail = l.PhotoThumbnail
            });
        }

        /// <summary>
        /// Loads the Institutional Directory Contacts.
        /// </summary>
        /// <returns>The contacts.</returns>
        private async Task LoadContacts()
        {
            try
            {
                IsBusy = true;
                var connection = await this.apiService.CheckConnection();
                if (!connection.IsSuccess)
                {
                    await Application.Current.MainPage.DisplayAlert(
                        Languages.Error,
                        connection.Message,
                        Languages.Cancel);
                    IsBusy = false;
                    await Application.Current.MainPage.Navigation.PopModalAsync();
                    return;
                }

                var response = await this.apiService.GetList<Participant>(
                    Settings.UrlBaseApiSigob,
                    App.PrefixApiSigob,
                    string.Format(apiContactsController, idEvent, rowPageIndex, Filter),
                    Settings.Token,
                    Settings.DbToken
                );
                if (!response.IsSuccess)
                {
                    await Application.Current.MainPage.DisplayAlert(
                        Languages.Error,
                        response.Message,
                        Languages.Cancel);
                    await Application.Current.MainPage.Navigation.PopModalAsync();
                    IsBusy = false;
                    return;
                }
                this.externalContactList = (List<Participant>)response.Result;
                if (!this.externalContactList.Any()) return;
                //Get the last Id to prevent unnecessary new Load
                this.rowPageIndex = Convert.ToInt32(this.externalContactList.Select(p => p.PersonId).Last());
                this.partialPageContacts = new ObservableCollection<ParticipantItemViewModel>(ToExternalContactItemViewModel());
                foreach (ParticipantItemViewModel person in partialPageContacts)
                {
                    this.InstitutionalDirectoryContacts.Add(person);

                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert(
                    Languages.Error,
                    $"{Languages.GeneralError}: {ex.Message}",
                    Languages.Cancel);
            }
            finally { IsBusy = false; }
        }

        /// <summary>
        /// Convert List to ParticipantItemViewModel
        /// </summary>
        /// <returns>The external contact item view model.</returns>
        private IEnumerable<ParticipantItemViewModel> ToExternalContactItemViewModel()
        {
            return this.externalContactList.Select(l => new ParticipantItemViewModel
            {
                Area = l.Area,
                Email = l.Email,
                FullName = l.FullName,
                Gender = l.Gender,
                OfficeId = l.OfficeId,
                PersonId = l.PersonId,
                PhoneNumber = l.PhoneNumber,
                Position = l.Position
            }).OrderBy(l => l.FullName);
        }

        /// <summary>
        /// Search command method
        /// </summary>
        private void Search()
        {
            if (isSelectedMainList)
            {
                this.InstitutionalDirectoryContacts.Clear();
                this.rowPageIndex = 0;
                this.LoadContacts().FireAndForgetSafeAsync();
            }
            else
            {
                //if (string.IsNullOrEmpty(this.Filter))
                //{
                //    this.MobileContacts = new ObservableCollection<PhoneContact>(
                //        this.MobileContactsList);
                //}
                //else
                //{
                //    this.MobileContacts = new ObservableCollection<PhoneContact>(
                //        this.MobileContactsList.Where(
                //            l => l.FullName.ToLower().Contains(this.Filter.ToLower())));
                //}
            }
        }

        private void SearchWhenEmpty()
        {
            if ((!string.IsNullOrEmpty(this.Filter)) && isSelectedMainList) return;
            this.Search();
        }

        private void OnSelectionChanged()
        {
            SearchPlaceHolderText = (isSelectedMainList) ? Languages.SearchExternalContactsPlaceHolder : Languages.SearchDeviceContactsPlaceHolder;
            if (!string.IsNullOrEmpty(Filter)) this.Search();
        }

        private async Task BackToParentPage()
        {
            _ = await Application.Current.MainPage.Navigation.PopModalAsync();
            return;
        }
        #endregion

        #region Async Commands
        public IAsyncCommand LoadContactsCommand => new AsyncCommand(LoadContacts);
        public IAsyncCommand BackToParentPageCommand => new AsyncCommand(BackToParentPage);
        #endregion

        #region Commands
        public ICommand SearchCommand => new RelayCommand(Search);
        #endregion
    }
}
