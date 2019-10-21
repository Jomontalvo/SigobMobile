namespace SigobMobile.Helpers
{
    using System;
    using Plugin.Settings;
    using Plugin.Settings.Abstractions;
    using Telerik.XamarinForms.Input;

    /// <summary>
    /// This is the Settings static class that can be used in your Core solution or in any
    /// of your client applications. All settings are laid out the same exact way with getters
    /// and setters. 
    /// </summary>
    public static class Settings
    {
        private static ISettings AppSettings
        {
            get
            {
                return CrossSettings.Current;
            }
        }

        #region Setting Constants

        private const string token = "token";
        private const string dbToken = "dbtoken";
        private const string urlBaseApiSigob = "uriAPI";
        private const string insitutionLogo = "logo";
        private const string userFullName = "fullName";
        private const string officeCode = "officeCode";
        //Central web contents
        private const string urlWebCentral = "uriCentral";
        private const string urlWebHelpContent = "uriHelp";
        private const string urlWebTermsContent = "uriTerms";
        private const string urlWebContactUsContent = "uriContact";
        private static readonly string SettingsDefault = string.Empty;

        private const string calendarViewMode = "calendarViewMode";
        private static readonly int SettingsCalendarViewDefault = (int)CalendarViewMode.Day;

        private const string calSelectedDate = "selectedDate";
        private static readonly DateTime SettingsSelectedDateDefault= DateTime.Today;

        private const string isVisibleManagementStatus = "isVisibleManagementStatus";
        private const string isVisibleCompletedStatus = "isVisibleCompletedStatus";
        private const string isVisibleSuspendStatus = "isVisibleSuspendStatus";
        private static readonly bool SettingsFilterStatus = true;

        private const string isEventColorByCalendar = "isEventColorByCalendar";
        //private static readonly bool SettingsEventColor = false;

        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="T:SigobMobile.Helpers.Settings"/> is visible
        /// management status.
        /// </summary>
        /// <value><c>true</c> if is visible management status; otherwise, <c>false</c>.</value>
        public static bool IsVisibleManagementStatus
        {
            get => AppSettings.GetValueOrDefault(isVisibleManagementStatus, SettingsFilterStatus);
            set => AppSettings.AddOrUpdateValue(isVisibleManagementStatus, value);
        }
        public static bool IsVisibleCompletedStatus
        {
            get => AppSettings.GetValueOrDefault(isVisibleCompletedStatus, SettingsFilterStatus);
            set => AppSettings.AddOrUpdateValue(isVisibleCompletedStatus, value);
        }
        public static bool IsVisibleSuspendStatus
        {
            get => AppSettings.GetValueOrDefault(isVisibleSuspendStatus, SettingsFilterStatus);
            set => AppSettings.AddOrUpdateValue(isVisibleSuspendStatus, value);
        }

        /// <summary>
        /// Get or sets a value indicating if events take calendar color or event type color (false)
        /// </summary>
        public static bool IsEventColorByCalendar
        {
            get => AppSettings.GetValueOrDefault(isEventColorByCalendar, !SettingsFilterStatus);
            set => AppSettings.AddOrUpdateValue(isEventColorByCalendar, value);
        }

        /// <summary>
        /// Gets or sets the last date visited in Management Center
        /// </summary>
        /// <value>The selected date.</value>
        public static DateTime SelectedDate
        {
            get => AppSettings.GetValueOrDefault(calSelectedDate, SettingsSelectedDateDefault);
            set => AppSettings.AddOrUpdateValue(calSelectedDate, value);
        }

        /// <summary>
        /// Gets or sets the current calendar view mode.
        /// </summary>
        /// <value>The current calendar view mode.</value>
        public static int CurrentCalendarViewMode
        {
            get => AppSettings.GetValueOrDefault(calendarViewMode, SettingsCalendarViewDefault);
            set => AppSettings.AddOrUpdateValue(calendarViewMode, value);
        }

        /// <summary>
        /// Gets or sets the token.
        /// </summary>
        /// <value>The token.</value>
        public static string Token
        {
            get
            {
                return AppSettings.GetValueOrDefault(token, SettingsDefault);
            }
            set
            {
                AppSettings.AddOrUpdateValue(token, value);
            }
        }

        /// <summary>
        /// Gets or sets the db token.
        /// </summary>
        /// <value>The db token.</value>
        public static string DbToken
        {
            get
            {
                return AppSettings.GetValueOrDefault(dbToken, SettingsDefault);
            }
            set
            {
                AppSettings.AddOrUpdateValue(dbToken, value);
            }
        }

        /// <summary>
        /// Gets or sets the URL base API Sigob.
        /// </summary>
        /// <value>The URL base API sigob.</value>
        public static string UrlBaseApiSigob
        {
            get => AppSettings.GetValueOrDefault(urlBaseApiSigob, SettingsDefault);
            set => AppSettings.AddOrUpdateValue(urlBaseApiSigob, value);
        }

        /// <summary>
        /// Gets or sets the URL Central Web content
        /// </summary>
        public static string UrlWebCentral
        {
            get => AppSettings.GetValueOrDefault(urlWebCentral, SettingsDefault);
            set => AppSettings.AddOrUpdateValue(urlWebCentral, value);
        }

        /// <summary>
        /// Gets or sets the URL Central Help content
        /// </summary>
        public static string UrlWebHelpContent
        {
            get => AppSettings.GetValueOrDefault(urlWebHelpContent, SettingsDefault);
            set => AppSettings.AddOrUpdateValue(urlWebHelpContent, value);
        }

        /// <summary>
        /// Gets or sets the URL Central Terms And Conditions content
        /// </summary>
        public static string UrlWebTermsContent
        {
            get => AppSettings.GetValueOrDefault(urlWebTermsContent, SettingsDefault);
            set => AppSettings.AddOrUpdateValue(urlWebTermsContent, value);
        }

        /// <summary>
        /// Gets or sets the URL Central Contact us content
        /// </summary>
        public static string UrlWebContactUsContent
        {
            get => AppSettings.GetValueOrDefault(urlWebContactUsContent, SettingsDefault);
            set => AppSettings.AddOrUpdateValue(urlWebContactUsContent, value);
        }

        /// <summary>
        /// Gets or sets the institution logo.
        /// </summary>
        /// <value>The logo institution.</value>
        public static string InstitutionLogo
        {
            get => AppSettings.GetValueOrDefault(insitutionLogo, SettingsDefault);
            set => AppSettings.AddOrUpdateValue(insitutionLogo, value);
        }

        /// <summary>
        /// Gets or sets the full name of active user.
        /// </summary>
        /// <value>The full name.</value>
        public static string FullName
        {
            get
            {
                return AppSettings.GetValueOrDefault(userFullName, SettingsDefault);
            }
            set
            {
                AppSettings.AddOrUpdateValue(userFullName, value);
            }
        }

        /// <summary>
        /// Gets or sets the SIGOB office code of active user.
        /// </summary>
        /// <value>The full name.</value>
        public static string OfficeCode
        {
            get
            {
                return AppSettings.GetValueOrDefault(officeCode, SettingsDefault);
            }
            set
            {
                AppSettings.AddOrUpdateValue(officeCode, value);
            }
        }

        #endregion
    }
}
