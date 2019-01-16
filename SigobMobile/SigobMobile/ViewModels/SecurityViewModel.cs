namespace SigobMobile.ViewModels
{
    using Services;

        public class SecurityViewModel : BaseViewModel
    {
        #region Services
        internal ApiService apiService;
        //internal string apiController = "user/appmodules";
        #endregion

        #region Attributes
        private string securityLabel;
        #endregion

        #region Properties
        public string SecurityLabel
        {
            get { return this.securityLabel; }
            set { SetValue(ref this.securityLabel, value); }
        }
        #endregion

        #region Constructors
        public SecurityViewModel()
        {
            apiService = new ApiService();
            SecurityLabel = "Change Password";
        }
        #endregion
    }
}
