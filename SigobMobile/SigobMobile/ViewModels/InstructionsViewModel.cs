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
    using Services;
    using Telerik.XamarinForms.DataControls.ListView.Commands;
    using Views.ManagementCenter;
    using Xamarin.Forms;

    public class InstructionsViewModel : BaseViewModel
    {
        #region Services
        internal ApiService apiService;
        internal string apiInstructionsController = "instructions/filter/{0}";
        #endregion

        #region Attributes
        private List<Instruction> instructionList;
        private ObservableCollection<Instruction> instructions;
        private ObservableCollection<string> instructionStatus;
        private int selectedIndex;
        private bool isRefreshing;
        private bool isRefreshingWhenNotPull;
        private bool isVisibleSearch;
        private string filter;
        #endregion

        #region Properties
        public ObservableCollection<string> InstructionStatus => instructionStatus ?? (instructionStatus = new ObservableCollection<string>());
        public List<Instruction> InstructionList
        {
            get;
            set;
        }
        public ObservableCollection<Instruction> InstructionItems
        {
            get { return this.instructions; }
            set { SetValue(ref this.instructions, value); }
        }
        public bool IsRefreshing
        {
            get { return this.isRefreshing; }
            set { SetValue(ref this.isRefreshing, value); }
        }
        public bool IsRefreshingWhenNotPull
        {
            get { return this.isRefreshingWhenNotPull; }
            set { SetValue(ref this.isRefreshingWhenNotPull, value); }
        }
        public bool IsVisibleSearch
        {
            get { return this.isVisibleSearch; }
            set { SetValue(ref this.isVisibleSearch, value); }
        }
        public int SelectedIndex
        {
            get { return this.selectedIndex; }
            set
            { 
                SetValue(ref this.selectedIndex, value);
                IErrorHandler errorHandler = null;
                this.OnSelectionChanged().FireAndForgetSafeAsync(errorHandler);
            }
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
        #endregion

        #region Constructors
        public InstructionsViewModel()
        {
            IErrorHandler errorHandler = null;
            this.isRefreshingWhenNotPull = true;
            this.apiService = new ApiService();
            this.SelectedIndex = 0;
            this.RefreshCommand = new AsyncCommand<PullToRefreshRequestedCommandContext>(this.Refresh);
            this.ItemTapCommand = new AsyncCommand<ItemTapCommandContext>(this.ItemTapped);
            this.LoadSegmentFilters();
            this.LoadItems().FireAndForgetSafeAsync(errorHandler);
        }
        #endregion

        #region Methods

        /// <summary>
        /// Iniatial load of instructions
        /// </summary>
        public async Task LoadItems()
        {
            await this.LoadInstructions(false);
        }

        /// <summary>
        /// Loads the segment filters.
        /// </summary>
        private void LoadSegmentFilters()
        {
            InstructionStatus.Add(Languages.PendingStatus);
            InstructionStatus.Add(Languages.CompletedStatus);
            InstructionStatus.Add(Languages.AllStatus);
        }

        /// <summary>
        /// Event ocurrs when the selection is changed.
        /// </summary>
        private async Task OnSelectionChanged()
        {
            await this.LoadInstructions(false);
        }

        /// <summary>
        /// Loads all instructions.
        /// </summary>
        private async Task LoadInstructions( bool LoadFromPullRehresh)
        {
            this.IsRefreshing = true;
            this.IsRefreshingWhenNotPull = !LoadFromPullRehresh;
            var connection = await this.apiService.CheckConnection();
            if (!connection.IsSuccess)
            {
                this.IsRefreshing = false;
                await Application.Current.MainPage.DisplayAlert(
                    Languages.Error,
                    connection.Message,
                    Languages.Cancel);
                await App.Navigator.PopToRootAsync();
                return;
            }

            var response = await this.apiService.GetList<Instruction>(
                Settings.UrlBaseApiSigob,
                App.PrefixApiSigob,
                string.Format(this.apiInstructionsController,this.SelectedIndex),
                Settings.Token,
                Settings.DbToken
            );
            if (!response.IsSuccess)
            {
                this.IsRefreshing = false;
                await Application.Current.MainPage.DisplayAlert(
                    Languages.Error,
                    response.Message,
                    Languages.Cancel);
                await App.Navigator.PopAsync();
                return;
            }
            this.instructionList = (List<Instruction>)response.Result;
            InstructionItems =  new ObservableCollection<Instruction>(this.ToInstructionItemViewModel());
            this.InstructionList = new List<Instruction>(InstructionItems); // For search
            this.IsRefreshing = this.IsRefreshingWhenNotPull = false;

        }

        private IEnumerable<Instruction> ToInstructionItemViewModel()
        {
            return this.instructionList.Select(l => new Instruction
            {
                Id = l.Id,
                CanBeFinished = l.CanBeFinished,
                Description = l.Description,
                DocumentsAmount = l.DocumentsAmount,
                EndDate = l.EndDate.ToLocalTime(),
                ExpiryPeriod = l.ExpiryPeriod,
                IsFinished = l.IsFinished,
                IsModifiable = l.IsModifiable,
                ManagementCenterId = l.ManagementCenterId,
                ManagementCenterName = l.ManagementCenterName,
                Participants = l.Participants,
                ParticipantsAmount = l.ParticipantsAmount,
                ProgrammerFullName = l.ProgrammerFullName,
                ProgrammerId = l.ProgrammerId,
                ProgrammerPosition = l.ProgrammerPosition,
                TasksAmount = l.TasksAmount,
                Title = l.Title
            }).Where ( f => !String.IsNullOrEmpty(f.Description) );  //Filter
        }


        /// <summary>
        /// Search an filter instruction.
        /// </summary>
        private void Search()
        {
            if (string.IsNullOrEmpty(this.Filter))
            {
                this.InstructionItems = new ObservableCollection<Instruction>(
                    this.InstructionList);
            }
            else
            {
                this.InstructionItems = new ObservableCollection<Instruction>(
                    this.InstructionList.Where(
                        l => l.Title.ToLower().Contains(this.Filter.ToLower()) ||
                             l.Description.ToLower().Contains(this.Filter.ToLower()) ||
                             l.ProgrammerFullName.ToLower().Contains(this.Filter.ToLower())));
            }
        }

        /// <summary>
        /// Item tapped of listview.
        /// </summary>
        /// <param name="context">Context.</param>
        private async Task ItemTapped(ItemTapCommandContext context)
        {
            var tappedItem = context.Item;
            //add your logic here
            await App.Navigator.PushAsync(new InstructionPage(), true);
        }

        /// <summary>
        /// Switchs the search button visibility
        /// </summary>
        private void SwitchSearch()
        {
            this.IsVisibleSearch = !this.IsVisibleSearch;
        }

        /// <summary>
        /// Opens the calendars visibility options
        /// </summary>
        private async Task OpenCalendars()
        {
            var calendarsMainViewModel = MainViewModel.GetInstance();
            calendarsMainViewModel.Calendars = new CalendarsViewModel(this);
            await Application.Current.MainPage.Navigation.PushModalAsync(new NavigationPage(new CalendarsPage()));
        }

        /// <summary>
        /// Refresh the specified context.
        /// </summary>
        /// <param name="context">Context.</param>
        private async Task Refresh(PullToRefreshRequestedCommandContext context)
        {
            await LoadInstructions(true);
        }
        #endregion

        #region Async Commands
        public IAsyncCommand OpenCalendarsCommand => new AsyncCommand(OpenCalendars);
        public ICommand RefreshCommand { get; set; }
        public ICommand ItemTapCommand { get; set; }
        #endregion

        #region Commands
        public ICommand SearchCommand => new RelayCommand(Search);
        public ICommand SwitchSearchCommand => new RelayCommand(SwitchSearch);
        #endregion
    }
}
