namespace SigobMobile.ViewModels
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Windows.Input;
    using GalaSoft.MvvmLight.Command;
    using Helpers;
    using Models;
    using Services;
    using Xamarin.Forms;

    public class InstructionsViewModel : BaseViewModel
    {
        #region Services
        internal ApiService apiService;
        internal string apiInstructionsController = "instructions/filter/{0}";
        #endregion

        #region Attributes
        //private ObservableCollection<InstructionItemViewModel> instructions;
        private ObservableCollection<Instruction> instructions;
        private ObservableCollection<Grouping<string, InstructionItemViewModel>> instructionsGrouped;
        private bool isRefreshing;
        private List<Instruction> instructionList;
        #endregion

        #region Properties
        //public ObservableCollection<InstructionItemViewModel> Instructions
        //{
        //    get { return this.instructions; }
        //    set { SetValue(ref this.instructions, value); }
        //}
        public ObservableCollection<Instruction> InstructionsItems
        {
            get { return this.instructions; }
            set { SetValue(ref this.instructions, value); }
        }
        public ObservableCollection<Grouping<string, InstructionItemViewModel>> InstructionsGrouped
        {
            get { return this.instructionsGrouped; }
            set { SetValue(ref this.instructionsGrouped, value); }
        }

        public bool IsRefreshing
        {
            get { return this.isRefreshing; }
            set { SetValue(ref this.isRefreshing, value); }
        }
        #endregion

        #region Constructors
        public InstructionsViewModel()
        {
            this.apiService = new ApiService();
            this.LoadAllInstructions();
        }
        #endregion

        #region Methods
        private async void LoadAllInstructions()
        {
            this.IsRefreshing = true;
            var connection = await this.apiService.CheckConnection();
            if (!connection.IsSuccess)
            {
                this.IsRefreshing = false;
                await Application.Current.MainPage.DisplayAlert(
                    Languages.Error,
                    connection.Message,
                    Languages.Cancel);
                await Application.Current.MainPage.Navigation.PopAsync();
                return;
            }

            var response = await this.apiService.GetList<Instruction>(
                Settings.UrlBaseApiSigob,
                App.PrefixApiSigob,
                string.Format(this.apiInstructionsController,2),
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
                await Application.Current.MainPage.Navigation.PopAsync();
                return;
            }
            this.instructionList = (List<Instruction>)response.Result;
            //this.Instructions = new ObservableCollection<InstructionItemViewModel>(
                //this.ToInstructionItemViewModel());
            this.InstructionsItems = new ObservableCollection<Instruction>(
                this.ToInstructionItemViewModel());
            ////Order by group Country
            //var sorted = from instruction in Instructions
            //             orderby instruction.Id
            //             group instruction by $"|{instruction.ManagementCenterName}|"
            //             into instructionGroup
            //             select new Grouping<string, InstructionItemViewModel>(instructionGroup.Key, instructionGroup);

            //this.InstructionsGrouped = new ObservableCollection<Grouping<string, InstructionItemViewModel>>(sorted);
            this.IsRefreshing = false;
        }

                /// <summary>
        /// Convert List Instructions Model to InstructionItem View model.
        /// </summary>
        /// <returns>The institution item view mode.</returns>
        private IEnumerable<InstructionItemViewModel> ToInstructionItemViewModel()
        {
            return this.instructionList.Select(l => new InstructionItemViewModel
            {
                Id = l.Id,
                ManagementCenterId = l.ManagementCenterId,
                ManagementCenterName = l.ManagementCenterName,
                Title = l.Title,
                Description = l.Description,
                EndDate = l.EndDate,
                TasksAmount = l.TasksAmount

            });
        }
        #endregion

        #region Commands
        public ICommand RefreshCommand
        {
            get
            {
                return new RelayCommand(LoadAllInstructions);
            }
        }
        #endregion
    }
}
