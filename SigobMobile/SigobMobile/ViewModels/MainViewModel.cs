
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
            instance = this;
            this.Login = new LoginViewModel();
        }
        #endregion

        #region Singleton
        private static MainViewModel instance;

        public static MainViewModel GetInstance()
        {
            if (instance == null)
            {
                return new MainViewModel();
            }
            return instance;
        }
        #endregion
    }
}
