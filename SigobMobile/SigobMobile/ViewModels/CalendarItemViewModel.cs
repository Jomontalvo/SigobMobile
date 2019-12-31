namespace SigobMobile.ViewModels
{
    using System.Threading.Tasks;
    using System.Windows.Input;
    using AsyncAwaitBestPractices.MVVM;
    using Common.Helpers;
    using Common.Services;
    using Helpers;
    using Models;
    using Telerik.XamarinForms.Primitives.CheckBox.Commands;
    using Xamarin.Forms;

    public class CalendarItemViewModel : Calendar
    {
        #region Services
        internal ApiService apiService;
        internal string apiPostVisibilityController = "calendars/linkedcal/{0}/visible/{1}";
        #endregion

        #region Constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="T:SigobMobile.ViewModels.CalendarItemViewModel"/> class.
        /// </summary>
        public CalendarItemViewModel()
        {
            this.apiService = new ApiService();
            this.IsCheckedChangedCommand = new AsyncCommand<CheckBoxIsCheckChangedCommandContext>(this.CheckBoxChange);
        }
        #endregion

        #region Commands
        public ICommand IsCheckedChangedCommand { get; set; }
        #endregion

        #region Methods

        /// <summary>
        /// Sets the calendar visibility.
        /// </summary>
        /// <returns>The calendar visibility.</returns>
        public async Task SetCalendarVisibility(bool? isVisible)
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
                string.Format(apiPostVisibilityController, this.OfficeId, isVisible),
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
            await SetCalendarVisibility( context.NewState );
        }
        #endregion
    }
}
