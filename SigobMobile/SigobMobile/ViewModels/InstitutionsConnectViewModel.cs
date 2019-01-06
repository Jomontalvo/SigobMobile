namespace SigobMobile.ViewModels
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using Models;
    using Services;
    using Xamarin.Forms;

    public class InstitutionsConnectViewModel : BaseViewModel
    {
        #region Services
        internal ApiService apiService;
        #endregion

        #region Attributes
        private ObservableCollection<InstitutionConnect> institutions;
        #endregion

        #region Properties
        public ObservableCollection<InstitutionConnect> Institutions
        {
            get { return this.institutions; }
            set { SetValue(ref this.institutions, value); }
        }
        #endregion

        #region Constructors
        public InstitutionsConnectViewModel()
        {
            this.apiService = new ApiService();
            this.LoadInstitutionsToConnect();

        }
        #endregion

        #region Methods
        private async void LoadInstitutionsToConnect()
        {
            var connection = await this.apiService.CheckConnection();
            if (!connection.IsSuccess)
            {
                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    connection.Message,
                    "Cancel");
                //Back to Login
                return;
            }

            var response = await this.apiService.GetList<InstitutionConnect>(
                "https://sigob-movil.firebaseio.com",
                "institutions", 
                ".json"
            );
            if (!response.IsSuccess)
            {
                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    response.Message,
                    "Cancel");
                //Back to login
                return;
            }
            var list = (List<InstitutionConnect>)response.Result;
            this.Institutions = new ObservableCollection<InstitutionConnect>(list);
        }
        #endregion
    }
}
