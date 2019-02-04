namespace SigobMobile.Helpers
{
    using Plugin.Settings;
    using Plugin.Settings.Abstractions;
    using SigobMobile.Models;

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
        private static readonly string SettingsDefault = string.Empty;

        #endregion

        #region Properties
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
            get
            {
                return AppSettings.GetValueOrDefault(urlBaseApiSigob, SettingsDefault);
            }
            set
            {
                AppSettings.AddOrUpdateValue(urlBaseApiSigob, value);
            }
        }
        #endregion
    }
}
