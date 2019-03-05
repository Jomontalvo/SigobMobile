namespace SigobMobile.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Windows.Input;
    using GalaSoft.MvvmLight.Command;
    using Helpers;
    using Models;
    using Services;
    using Views.ManagementCenter;
    using Telerik.XamarinForms.DataControls.ListView.Commands;
    using Xamarin.Forms;

    public class InstructionsViewModel : BaseViewModel
    {
        #region Services
        internal ApiService apiService;
        internal string apiInstructionsController = "instructions/filter/{0}";
        #endregion

        #region Attributes
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
                this.OnSelectionChanged();
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
            this.isRefreshingWhenNotPull = true;
            this.apiService = new ApiService();
            this.SelectedIndex = 0;
            this.RefreshCommand = new Command<PullToRefreshRequestedCommandContext>(this.Refresh);
            this.ItemTapCommand = new Command<ItemTapCommandContext>(this.ItemTapped);
            this.LoadSegmentFilters();
            this.LoadItems();
        }
        #endregion

        #region Methods

        /// <summary>
        /// Iniatial load of instructions
        /// </summary>
        private async void LoadItems()
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
        /// Event ocurrs when thw selection is changed.
        /// </summary>
        private async void OnSelectionChanged()
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
            var instructionList = (List<Instruction>)response.Result;
            InstructionItems =  new ObservableCollection<Instruction>();
            foreach (var item in instructionList)
            {
                if (!String.IsNullOrEmpty(item.Description))
                {
                    item.EndDate = item.EndDate.ToLocalTime();
                    InstructionItems.Add(item);
                }
            }
            this.InstructionList = new List<Instruction>(InstructionItems);
            this.IsRefreshing = this.IsRefreshingWhenNotPull = false;

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
        private void ItemTapped(ItemTapCommandContext context)
        {
            var tappedItem = context.Item;
            //add your logic here
            Application.Current.MainPage.DisplayAlert("", "You've selected " + tappedItem, "OK");
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
        private async void OpenCalendars()
        {
            var calendarsMainViewModel = MainViewModel.GetInstance();
            calendarsMainViewModel.Calendars = new CalendarsViewModel();
            await Application.Current.MainPage.Navigation.PushModalAsync(new NavigationPage(new CalendarsPage()));
        }


        /// <summary>
        /// Opens the filters.
        /// </summary>
        private void OpenFilters()
        {
            var filtersMainViewModel = MainViewModel.GetInstance();
            filtersMainViewModel.CalendarFilters = new CalendarFiltersViewModel();
            Application.Current.MainPage.Navigation.PushModalAsync(new CalendarFiltersPage());
        }

        /// <summary>
        /// Refresh the specified context.
        /// </summary>
        /// <param name="context">Context.</param>
        private async void Refresh(PullToRefreshRequestedCommandContext context)
        {

            await LoadInstructions(true);
        }
        #endregion

        #region Commands
        public ICommand SearchCommand
        {
            get
            {
                return new RelayCommand(Search);
            }
        }
        public ICommand OpenCalendarsCommand
        {
            get
            {
                return new RelayCommand(OpenCalendars);
            }
        }
        public ICommand OpenFiltersCommand
        {
            get
            {
                return new RelayCommand(OpenFilters);
            }
        }
        public ICommand RefreshCommand { get; set; }
        public ICommand ItemTapCommand { get; set; }
        public ICommand SwitchSearchCommand => new RelayCommand(SwitchSearch);
        #endregion
    }
}
