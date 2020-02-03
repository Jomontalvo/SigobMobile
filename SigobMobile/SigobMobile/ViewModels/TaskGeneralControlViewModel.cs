namespace SigobMobile.ViewModels
{
    using Common.Services;

    public class TaskGeneralControlViewModel : BaseViewModel
    {
        #region Services
        internal ApiService apiService;
        #endregion

        #region Constructors
        public TaskGeneralControlViewModel()
        {
            this.apiService = new ApiService();
        }
        #endregion
    }
}
