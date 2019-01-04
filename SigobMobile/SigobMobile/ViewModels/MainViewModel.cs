
namespace SigobMobile.ViewModels
{
    using System;

    /// <summary>
    /// Main view model.
    /// </summary>
    public class MainViewModel
    {
        #region ViewModels
        public LoginViewModel Login
        {
            get;
            set;
        }

        public InstitutionsConnectViewModel InstitutionsConnect 
        { 
            get; 
            set; 
        }
        #endregion

        #region Constructors
        public MainViewModel()
        {
            this.Login = new LoginViewModel();
        }
        #endregion
    }
}
