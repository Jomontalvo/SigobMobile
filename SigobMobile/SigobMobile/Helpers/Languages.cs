namespace SigobMobile.Helpers
{
    using Xamarin.Forms;
    using Interfaces;
    using Properties;
    public static class Languages
    {
        #region Constructors
        static Languages()
        {
            var ci = DependencyService.Get<ILocalize>().GetCurrentCultureInfo();
            Resources.Culture = ci;
            DependencyService.Get<ILocalize>().SetLocale(ci);

        }
        #endregion

        #region Languages Properties (Main Dictionary)

        #region Dialog Box
        public static string Accept { get { return Resources.Accept; } }
        public static string Cancel { get { return Resources.Cancel; } }
        public static string Error { get { return Resources.Error; } }
        public static string Ok { get { return Resources.Ok; } }
        public static string Yes { get { return Resources.Yes; } }
        public static string No { get { return Resources.No; } }
        public static string Success { get { return Resources.Success; } }
        #endregion

        #region Login View
        public static string LoginViewTitle { get { return Resources.LoginViewTitle; } }
        public static string EnterCredentials { get { return Resources.EnterCredentials ; } }
        public static string Username { get { return Resources.Username; } }
        public static string Password { get { return Resources.Password; } }
        public static string SelectApiInstitution { get { return Resources.SelectApiInstitution; } }
        public static string Login { get { return Resources.Login; } }
        public static string ForgotPassword { get { return Resources.ForgotPassword; } }
        //Messages
        public static string InvalidCredentials { get { return Resources.InvalidCredentials; } }
        public static string UserValidationMsg { get { return Resources.UserValidationMsg; } }
        public static string PasswordValidationMsg { get { return Resources.PasswordValidationMsg; } }
        #endregion

        #region Institutions to connect View
        public static string InstitutionsConnect { get { return Resources.InstitutionsConnect; } }
        #endregion

        #region Main Menu Options
        public static string MasterPageTitle { get { return Resources.MasterPageTitle; } }
        public static string HomeMenu { get { return Resources.HomeMenu; } }
        public static string HelpMenu { get { return Resources.HelpMenu; } }
        public static string SecurityMenu { get { return Resources.SecurityMenu; } }
        public static string TermsAndConditionsMenu { get { return Resources.TermsAndConditionsMenu; } }
        public static string ContactUs { get { return Resources.ContactUs; } }
        public static string LogoutMenu { get { return Resources.LogoutMenu; } }
        #endregion

        #region My Profile View
        public static string ProfileViewTitle { get { return Resources.ProfileViewTitle; } }

        #endregion

        #endregion
    }
}
