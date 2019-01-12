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
        #endregion

        #region Login View
        public static string ForgotPassword { get { return Resources.ForgotPassword; } }
        public static string ForgotPassword { get { return Resources. ; } }
        #endregion

        #endregion
    }
}
