namespace SigobMobile.ViewModels
{
    using System.Windows.Input;

    public class LoginViewModel
    {
        #region Properties
        public string UserName
        {
            get;
            set;
        }

        public string Password
        {
            get;
            set;
        }

        public string Institution
        {
            get;
            set;
        }

        public bool IsRunning
        {
            get;
            set;
        }
        #endregion

        #region Constructors
        public LoginViewModel()
        {

        }
        #endregion
        #region Commands
        public ICommand LoginCommand
        {
            get;
            set;
        }

        #endregion
    }
}
