namespace SigobMobile.ViewModels
{
    using System;
    using System.Threading.Tasks;
    using System.Windows.Input;
    using AsyncAwaitBestPractices.MVVM;
    using Common.Helpers;
    using Common.Services;
    using GalaSoft.MvvmLight.Command;
    using Helpers;
    using Models;
    using Telerik.XamarinForms.Primitives.CheckBox.Commands;
    using Xamarin.Forms;

    public class CalendarItemViewModel : Calendar
    {
        #region Services
        internal ApiService apiService;
        private string selectedOfficeId;
        private bool? visibility;
        #endregion

        #region ExtendedProperties
        internal string ApiPostVisibilityController => $"calendars/linkedcal/{this.selectedOfficeId}/visible/{this.visibility}";
        public string CalendarType => (this.ManagementCenterId != 0) ? Languages.ManagementCenters : Languages.PersonalAgendas;
        public bool IsSelectedForControl { get; set; }
        #endregion

        #region Constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="T:SigobMobile.ViewModels.CalendarItemViewModel"/> class.
        /// </summary>
        public CalendarItemViewModel()
        {
            this.apiService = new ApiService();
            //this.IsCheckedChangedCommand = new AsyncCommand<CheckBoxIsCheckChangedCommandContext>(this.CheckBoxChange);
        }
        #endregion

        #region Commands
        public ICommand SelectCalendarCommand => new AsyncCommand(SelectCalendarAsync);
        public ICommand IsCheckedChangedCommand => new AsyncCommand<CheckBoxIsCheckChangedCommandContext>(this.CheckBoxChange);
        #endregion

        #region Methods

        /// <summary>
        /// Sets the calendar visibility.
        /// </summary>
        /// <returns>The calendar visibility.</returns>
        public async Task SetCalendarVisibility()
        {
            var connection = await this.apiService.CheckConnection();
            if (!connection.IsSuccess)
            {
                await Application.Current.MainPage.DisplayAlert(
                    Languages.Error,
                    connection.Message,
                    Languages.Cancel);
                await Application.Current.MainPage.Navigation.PopModalAsync();
                return;
            }

            var response = await this.apiService.Post<bool>(
                Settings.UrlBaseApiSigob,
                App.PrefixApiSigob,
                ApiPostVisibilityController,
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
                return;
            }
            var resultCallApi = (bool)response.Result;
            if (!resultCallApi)
            {
                await Application.Current.MainPage.DisplayAlert(
                    Languages.Error,
                    Languages.GeneralError,
                    Languages.Cancel);
                await Application.Current.MainPage.Navigation.PopModalAsync();
                return;
            }
        }

        /// <summary>
        /// Execute when CheckBox is change.
        /// </summary>
        /// <param name="context">Context.</param>
        private async Task CheckBoxChange(CheckBoxIsCheckChangedCommandContext context)
        {
            //Set visibility of new calendar selected
            this.selectedOfficeId = this.OfficeId;
            this.visibility = context.NewState;
            await SetCalendarVisibility();
        }

        /// <summary>
        /// Select calendat to task control
        /// </summary>
        private async Task SelectCalendarAsync()
        {
            //Unselect all previous checked calendars
            var parentViewModel = MainViewModel.GetInstance().TaskDashboard;
            if (parentViewModel.currentOfficeId == this.OfficeId) return;
            parentViewModel.currentOfficeId = this.OfficeId;
            parentViewModel.IsOpenCalendar = false;
            parentViewModel.RefreshCalendarList();
            parentViewModel.isSegmentBuilt = false;
            await parentViewModel.LoadTaskBoardAsync();
        }

        #endregion
    }
}
