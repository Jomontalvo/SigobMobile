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
        public static string AllStatus => Resources.AllStatus;
        public static string CompletedStatus => Resources.CompletedStatus;
        public static string InManagementStatus => Resources.InManagementStatus;
        public static string ManagementStatusTitle => Resources.ManagementStatusTitle;
        public static string SuspendedStatus => Resources.SuspendedStatus;
        #endregion

        #region Filter for Confirmation Appointment Status
        public static string CalendarFiltersMessage => Resources.CalendarFiltersMessage; 
        public static string ConfirmedFilterTitle => Resources.ConfirmedFilterTitle;
        public static string OnlyConfirmed => Resources.OnlyConfirmed;
        public static string OnlyTentative => Resources.OnlyTentative;
        public static string ConfirmedAndTentative => Resources.ConfirmedAndTentative;
        public static string TentativeLabel => Resources.TentativeLabel;
        #endregion

        #region Time Measures
        public static string Day => Resources.Day;
        public static string Days => Resources.Days;
        public static string Month => Resources.Month;
        public static string Months => Resources.Months;
        public static string Year => Resources.Year;
        public static string Years => Resources.Years;
        #endregion

        #region Deadline Status (all components)
        public static string PendingStatus => Resources.PendingStatus;
        public static string CloseToDeadlinedStatus => Resources.CloseToDeadlinedStatus;
        public static string ExpiredStatus => Resources.ExpiredStatus;
        #endregion

        #region Search Tools
        public static string SearchPlaceHolder => Resources.SearchPlaceHolder;
        public static string Filters => Resources.Filters;
        public static string SelectAll => Resources.SelectAll;
        public static string UnselectAll => Resources.UnselectAll;
        public static string ShowAll => Resources.ShowAll;
        public static string HideAll => Resources.HideAll;
        #endregion

        #region Dialog Box
        public static string Accept => Resources.Accept;
        public static string Cancel => Resources.Cancel;
        public static string Error => Resources.Error;
        public static string Ok => Resources.Ok;
        public static string Yes => Resources.Yes;
        public static string No => Resources.No;
        public static string Success => Resources.Success;
        public static string GeneralError => Resources.GeneralError;
        public static string Add => Resources.Add;
        public static string Edit => Resources.Edit;
        public static string Save => Resources.Save;
        public static string Delete => Resources.Delete;
        #endregion

        #region Login View
        public static string LoginViewTitle => Resources.LoginViewTitle;
        public static string EnterCredentials => Resources.EnterCredentials;
        public static string Username => Resources.Username;
        public static string Password => Resources.Password;
        public static string SelectApiInstitution => Resources.SelectApiInstitution;
        public static string Login => Resources.Login;
        public static string ForgotPassword => Resources.ForgotPassword;
        //Messages
        public static string InvalidCredentials => Resources.InvalidCredentials;
        public static string UserValidationMsg => Resources.UserValidationMsg;
        public static string PasswordValidationMsg => Resources.PasswordValidationMsg;
        #endregion

        #region Institutions to connect View
        public static string InstitutionsConnect => Resources.InstitutionsConnect;
        #endregion

        #region Main Menu Options
        public static string MasterPageTitle => Resources.MasterPageTitle;
        public static string HomeMenu => Resources.HomeMenu;
        public static string HelpMenu => Resources.HelpMenu;
        public static string SecurityMenu => Resources.SecurityMenu;
        public static string TermsAndConditionsMenu => Resources.TermsAndConditionsMenu;
        public static string ContactUs => Resources.ContactUs;
        public static string LogoutMenu => Resources.LogoutMenu;
        #endregion

        #region My Profile View
        public static string ProfileViewTitle => Resources.ProfileViewTitle;
        public static string PersonalDataGroup => Resources.PersonalDataGroup;
        public static string OfficialDataGroup => Resources.OfficialDataGroup;
        public static string ContactDataGroup => Resources.ContactDataGroup;
        public static string FirstNameText => Resources.FirstNameText;
        public static string LastNameText => Resources.LastNameText;
        public static string InstitutionText => Resources.InstitutionText;
        public static string AreaText => Resources.AreaText;
        public static string PositionText => Resources.PositionText;
        public static string PhoneText => Resources.PhoneText;
        public static string CellPhoneText => Resources.CellPhoneText;
        public static string EmailText => Resources.EmailText;
        public static string GenderText => Resources.GenderText;
        public static string SelectGenderPlaceHolder => Resources.SelectGenderPlaceHolder;
        public static string MaleGender => Resources.MaleGender;
        public static string FemaleGender => Resources.FemaleGender;
        public static string UserImageText => Resources.UserImageText;
        public static string SourceImageQuestion => Resources.SourceImageQuestion;
        public static string FromCamera => Resources.FromCamera;
        public static string FromGallery => Resources.FromGallery;
        public static string DeletePicture => Resources.DeletePicture;

        #endregion

        #region Calendar Agenda View
        public static string AgendaViewTitle => Resources.AgendaViewTitle;
        public static string CalendarsTitle => Resources.CalendarsTitle;
        public static string Calendar => Resources.Calendar;
        public static string Today => Resources.Today;
        public static string CalendarViewModeText => Resources.CalendarViewModeText;
        public static string DailyView => Resources.DailyView;
        public static string MonthlyView => Resources.MonthlyView;
        public static string MultiDayView => Resources.MultiDayView;
        public static string YearView => Resources.YearView;
        public static string WeeklyView => Resources.WeeklyView;
        public static string SelectCalendarMessage => Resources.SelectCalendarMessage;
        public static string SelectCalendarColor => Resources.SelectCalendarColor;
        #endregion

        #region Appointment View
        public static string AppointmentViewTitle => Resources.AppointmentViewTitle;
        #endregion


        #endregion
    }
}
