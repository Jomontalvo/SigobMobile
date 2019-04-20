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
    using Models;
    using Services;
    using Views.Common;
    using Xamarin.Forms;

    /// <summary>
    /// Participants view model.
    /// </summary>
    public class ParticipantsViewModel : BaseViewModel
    {
        #region Services
        internal ApiService apiService;
        #endregion

        #region ApiControllers
        internal string apiEventParticipantsController = "participants/cg/{0}/event/{1}/invited/{2}";
        internal string apiAllParticipantsController = "participants/cg/{0}/agenda/{1}";
        internal string apiExternalParticipantsController = "participants/event/{0}/externalguests/index/{1}?pattern={2}";
        internal string apiSaveParticipants = "participants/event/{0}/instru/{1}/agenda/{2}";
        #endregion

        #region Attributes
        private string filter;
        //private ManagementCenterEvent localEvent;
        //private AgendaEvent localAppointment;
        private EventCgViewModel eventCgViewModel;
        private bool isRunning;
        private bool isFinding;
        public List<Participant> participantList;
        public List<Participant> availableList;
        private readonly int IdEvent;
        private readonly int ManagementCenterId;
        private readonly string OwnerOfficeId;
        private readonly char InstrumentType;
        private ObservableCollection<ParticipantItemViewModel> participants;
        private ObservableCollection<ParticipantItemViewModel> available;
        private EventParticipants guests;
        #endregion

        #region Properties
        public List<Participant> MobileParticipants => new List<Participant>();
        public bool IsFinding
        {
            get => this.isFinding;
            set => SetValue(ref this.isFinding, value);
        }
        public bool IsRunning
        {
            get => this.isRunning;
            set => SetValue(ref this.isRunning, value);
        }
        public string Filter
        {
            get { return this.filter; }
            set
            {
                SetValue(ref this.filter, value);
                this.Search();
            }
        }

        public List<Participant> ParticipantList
        {
            get => this.participantList;
            set => SetValue(ref this.participantList, value);
        }

        public List<Participant> AvailableList
        {
            get => this.availableList;
            set => SetValue(ref this.availableList, value);
        }


        public ObservableCollection<ParticipantItemViewModel> GuestContacts
        {
            get { return this.participants; }
            set { SetValue(ref this.participants, value); }
        }

        public ObservableCollection<ParticipantItemViewModel> AvailableContacts
        {
            get { return this.available; }
            set { SetValue(ref this.available, value); }
        }
        #endregion

        #region Constructors

        public ParticipantsViewModel(EventCgViewModel eventCgViewModel)
        {
            apiService = new ApiService();
            this.eventCgViewModel = eventCgViewModel;
            if (eventCgViewModel.LocalEvent.ManagementCenterId !=0)
            {
                IdEvent = eventCgViewModel.LocalEvent.Id;
                InstrumentType = '7';
                ManagementCenterId = eventCgViewModel.LocalEvent.ManagementCenterId;
                OwnerOfficeId = eventCgViewModel.LocalEvent.OwnerOfficeId;
            }
            else
            {
                //localAppointment = (AgendaEvent)ev;
                //IdEvent = localAppointment.Id;
                //InstrumentType = '4';
                //ManagementCenterId = 0;
                //OwnerOfficeId = localAppointment.OwnerOfficeId;
            }
            LoadParticipants().FireAndForgetSafeAsync();
        }
        #endregion

        #region Methods
        private async Task LoadParticipants()
        {
            IsRunning = true;
            await LoadGuestContacts();
            await LoadAvailableContacts();
            IsFinding = false;
            IsRunning = false;
        }

        /// <summary>
        /// Loads the available contacts.
        /// </summary>
        /// <returns>The available contacts.</returns>
        private async Task LoadAvailableContacts()
        {
            var connection = await this.apiService.CheckConnection();
            if (!connection.IsSuccess)
            {
                IsRunning = false;
                await Application.Current.MainPage.DisplayAlert(
                    Languages.Error,
                    connection.Message,
                    Languages.Cancel);
                await Application.Current.MainPage.Navigation.PopAsync();
                return;
            }
            if (IdEvent > 0)
            {
                var response = await this.apiService.GetList<Participant>(
                Settings.UrlBaseApiSigob,
                App.PrefixApiSigob,
                string.Format(apiEventParticipantsController, ManagementCenterId, IdEvent, false),
                Settings.Token,
                Settings.DbToken);
                if (!response.IsSuccess)
                {
                    this.IsRunning = false;
                    await Application.Current.MainPage.DisplayAlert(
                        Languages.Error,
                        response.Message,
                        Languages.Cancel);
                    return;
                }
                this.availableList = (List<Participant>)response.Result;
                this.AvailableContacts = new ObservableCollection<ParticipantItemViewModel>(ToAvailableItemViewModel());
            }
        }

        private IEnumerable<ParticipantItemViewModel> ToAvailableItemViewModel()
        {
            return this.availableList.Select(l => new ParticipantItemViewModel
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
        /// Loads the guest contacts.
        /// </summary>
        /// <returns>The guest contacts.</returns>
        private async Task LoadGuestContacts()
        {
            var connection = await this.apiService.CheckConnection();
            if (!connection.IsSuccess)
            {
                IsRunning = false;
                await Application.Current.MainPage.DisplayAlert(
                    Languages.Error,
                    connection.Message,
                    Languages.Cancel);
                await Application.Current.MainPage.Navigation.PopAsync();
                return;
            }
            Response response;
            if (IdEvent > 0) //Edit Event is the current operation
            {
                response = await this.apiService.GetList<Participant>(
                Settings.UrlBaseApiSigob,
                App.PrefixApiSigob,
                string.Format(apiEventParticipantsController, ManagementCenterId, IdEvent, true),
                Settings.Token,
                Settings.DbToken);
            }
            else //Add New Event is the current operation
            {
                response = await this.apiService.GetList<Participant>(
                Settings.UrlBaseApiSigob,
                App.PrefixApiSigob,
                string.Format(apiAllParticipantsController, ManagementCenterId, OwnerOfficeId),
                Settings.Token,
                Settings.DbToken);
            }
            if (!response.IsSuccess)
            {
                this.IsRunning = false;
                await Application.Current.MainPage.DisplayAlert(
                    Languages.Error,
                    response.Message,
                    Languages.Cancel);
                return;
            }
            if (IdEvent > 0)
            {
                this.participantList = (List<Participant>)response.Result;
                this.GuestContacts = new ObservableCollection<ParticipantItemViewModel>(ToParticipantItemViewModel());
            }
            else //Get the management center owner 
            {
                this.availableList = (List<Participant>)response.Result;
                this.AvailableContacts = new ObservableCollection<ParticipantItemViewModel>(ToAvailableItemViewModel().Where((c) => c.OfficeId != OwnerOfficeId));
            }
        }

        /// <summary>
        /// To the contact list.
        /// </summary>
        /// <returns>The contact list.</returns>
        private IEnumerable<ParticipantItemViewModel> ToParticipantItemViewModel()
        {
            return this.participantList.Select(l => new ParticipantItemViewModel
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
        /// Search contact in Available contacts list.
        /// </summary>
        private void Search()
        {
            if (!IsFinding) IsFinding = true;

            if (string.IsNullOrEmpty(this.Filter))
            {
                this.AvailableContacts = new ObservableCollection<ParticipantItemViewModel>(
                    this.ToAvailableItemViewModel());
                IsFinding = false;
            }
            else
            {
                this.AvailableContacts = new ObservableCollection<ParticipantItemViewModel>(
                    this.ToAvailableItemViewModel().Where(
                        l => l.FullName.ToLower().Contains(this.Filter.ToLower()) ||
                             l.Position.ToLower().Contains(this.Filter.ToLower()) ||
                             l.Area.ToLower().Contains(this.Filter.ToLower())));
            }

        }

        /// <summary>
        /// Oks the and close.
        /// </summary>
        private async Task SaveAndClose()
        {
            IsRunning = true;
            if (this.IsPrepareGuestsModel())
            {
                var connection = await this.apiService.CheckConnection();
                if (!connection.IsSuccess)
                {
                    IsRunning = false;
                    await Application.Current.MainPage.DisplayAlert(
                        Languages.Error,
                        connection.Message,
                        Languages.Cancel);
                    await Application.Current.MainPage.Navigation.PopAsync();
                    return;
                }
                var response = await this.apiService.Put<TransactionResponse>(
                    Settings.UrlBaseApiSigob,
                    App.PrefixApiSigob,
                    string.Format(apiSaveParticipants, IdEvent, InstrumentType, OwnerOfficeId),
                    Settings.Token,
                    Settings.DbToken,
                    guests);
                if (!response.IsSuccess)
                {
                    this.IsRunning = false;
                    await Application.Current.MainPage.DisplayAlert(
                        Languages.Error,
                        response.Message,
                        Languages.Cancel);
                    return;
                }
                this.UpdateParticipantsLabel();
                IsRunning = false;
                _ = await Application.Current.MainPage.Navigation.PopModalAsync();
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert(
                        Languages.Error,
                        Languages.GeneralError,
                        Languages.Cancel);
                return;
            }
        }

        /// <summary>
        /// Updates the participants label on Parent Page
        /// </summary>
        private void UpdateParticipantsLabel()
        {
            //Participants
            eventCgViewModel.LocalEvent.ParticipantsAccount = GuestContacts.Count();
            eventCgViewModel.LocalEvent.IsParticipant = (GuestContacts.FirstOrDefault(i => i.OfficeId == OwnerOfficeId) != null);
            if (eventCgViewModel.LocalEvent.ParticipantsAccount <= 1)
                eventCgViewModel.Participants = $"1 {Languages.EventParticipantText}";
            else
            {
                eventCgViewModel.Participants = (eventCgViewModel.LocalEvent.IsParticipant) ?
                    $"{Languages.EventParticipantsText} ({Languages.Me} {Languages.And} {eventCgViewModel.LocalEvent.ParticipantsAccount - 1}+)" :
                    $"{eventCgViewModel.LocalEvent.ParticipantsAccount} {Languages.EventParticipantsText}";
            }
        }

        /// <summary>
        /// Prepares the guests model for save transaction.
        /// </summary>
        private bool IsPrepareGuestsModel()
        {
            string internalContacts = string.Join(",", GuestContacts.Where((item) => !string.IsNullOrEmpty(item.OfficeId))
                                                                      .Select(item => item.OfficeId).ToArray());
            string externalContacts = string.Join(",", GuestContacts.Where((item) => string.IsNullOrEmpty(item.OfficeId))
                                                                      .Select(item => item.PersonId).ToArray());
            guests = new EventParticipants
            {
                InternalContacts = internalContacts,
                ExternalContacts = externalContacts,
                MobileContacts = MobileParticipants
            };
            return !string.IsNullOrEmpty(internalContacts);
        }

        /// <summary>
        /// Backs to parent page.
        /// <returns>The to parent page.</returns>
        /// </summary>
        private async Task BackToParentPage()
        {
            _ = await Application.Current.MainPage.Navigation.PopModalAsync();
            return;
        }

        private async Task OpenExternalContacts()
        { 
            var contactsViewModel = MainViewModel.GetInstance();
            contactsViewModel.Contacts = new ContactsViewModel();
            await Application.Current.MainPage.Navigation.PushModalAsync(new ContactsPage());
            //contactsViewModel.InstitucionalDirectory = new InstitucionalDirectoryViewModel();
            //await Application.Current.MainPage.Navigation.PushModalAsync(new InstitucionalDirectoryPage { Title = Languages.InstitutionalDirectoryTitle });
        }

        #endregion

        #region Commands
        public ICommand SearchCommand => new RelayCommand(Search);
        #endregion

        #region Async Commands
        public IAsyncCommand SaveAndCloseCommand => new AsyncCommand(SaveAndClose);
        public IAsyncCommand BackToParentPageCommand => new AsyncCommand(BackToParentPage);
        public IAsyncCommand OpenExternalContactsCommand => new AsyncCommand(OpenExternalContacts);
        #endregion
    }
}
