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

        #region Management Status (all componentes)
        public static string InManagementStatus { get { return Resources.InManagementStatus; } }
        public static string SuspendedStatus { get { return Resources.SuspendedStatus; } }
        public static string CompletedStatus { get { return Resources.CompletedStatus; } }
        public static string AllStatus { get { return Resources.AllStatus; } }
        #endregion

        #region Time Measures
        public static string Day { get { return Resources.Day; } }
        public static string Days { get { return Resources.Days; } }
        public static string Month { get { return Resources.Month; } }
        public static string Months { get { return Resources.Months; } }
        public static string Year { get { return Resources.Year; } }
        public static string Years { get { return Resources.Years; } }
        #endregion

        #region Deadline Status (all components)
        public static string PendingStatus { get { return Resources.PendingStatus; } }
        public static string CloseToDeadlinedStatus { get { return Resources.CloseToDeadlinedStatus; } }
        public static string ExpiredStatus { get { return Resources.ExpiredStatus; } }
        #endregion

        #region Search Tools
        public static string SearchPlaceHolder { get { return Resources.SearchPlaceHolder; } }
        public static string Filters { get { return Resources.Filters; } }
        public static string SelectAll { get { return Resources.SelectAll; } }
        public static string UnselectAll { get { return Resources.UnselectAll; } }
        public static string ShowAll { get { return Resources.ShowAll; } }
        public static string HideAll { get { return Resources.HideAll; } }
        #endregion

        #region Dialog Box
        public static string Accept { get { return Resources.Accept; } }
        public static string Cancel { get { return Resources.Cancel; } }
        public static string Error { get { return Resources.Error; } }
        public static string Ok { get { return Resources.Ok; } }
        public static string Yes { get { return Resources.Yes; } }
        public static string No { get { return Resources.No; } }
        public static string Success { get { return Resources.Success; } }
        public static string GeneralError { get { return Resources.GeneralError; } }
        public static string Delete { get { return Resources.Delete; } }
        #endregion

        #region Login View
        public static string LoginViewTitle { get { return Resources.LoginViewTitle; } }
        public static string EnterCredentials { get { return Resources.EnterCredentials; } }
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
        public static string PersonalDataGroup { get { return Resources.PersonalDataGroup; } }
        public static string OfficialDataGroup { get { return Resources.OfficialDataGroup; } }
        public static string ContactDataGroup { get { return Resources.ContactDataGroup; } }
        public static string FirstNameText { get { return Resources.FirstNameText; } }
        public static string LastNameText { get { return Resources.LastNameText; } }
        public static string InstitutionText { get { return Resources.InstitutionText; } }
        public static string AreaText { get { return Resources.AreaText; } }
        public static string PositionText { get { return Resources.PositionText; } }
        public static string PhoneText { get { return Resources.PhoneText; } }
        public static string CellPhoneText { get { return Resources.CellPhoneText; } }
        public static string EmailText { get { return Resources.EmailText; } }
        public static string GenderText { get { return Resources.GenderText; } }
        public static string SelectGenderPlaceHolder { get { return Resources.SelectGenderPlaceHolder; } }
        public static string MaleGender { get { return Resources.MaleGender; } }
        public static string FemaleGender { get { return Resources.FemaleGender; } }
        public static string UserImageText { get { return Resources.UserImageText; } }
        public static string SourceImageQuestion { get { return Resources.SourceImageQuestion; } }
        public static string FromCamera { get { return Resources.FromCamera; } }
        public static string FromGallery { get { return Resources.FromGallery; } }
        public static string DeletePicture { get { return Resources.DeletePicture; } }

        #endregion

        #region Calendar Agenda View
        public static string AgendaViewTitle { get { return Resources.AgendaViewTitle; } }
        public static string CalendarsTitle { get { return Resources.CalendarsTitle; } }
        public static string Calendar { get { return Resources.Calendar; } }
        public static string Today { get { return Resources.Today; } }
        public static string DailyView { get { return Resources.DailyView; } }
        public static string MonthlyView { get { return Resources.MonthlyView; } }
        public static string MultiDayView { get { return Resources.MultiDayView; } }
        public static string AnnualView { get { return Resources.AnnualView; } }
        public static string AgendaListView { get { return Resources.AgendaListView; } }
        public static string SelectCalendarMessage { get { return Resources.SelectCalendarMessage; } }
        public static string SelectCalendarColor { get { return Resources.SelectCalendarColor; } }
        #endregion

        #endregion
    }
}
