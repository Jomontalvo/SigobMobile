namespace SigobMobile.Common.Models
{
    /// <summary>
    /// Login Credentials model
    /// </summary>
    public class LoginCredentials
    {
        /// <summary>
        /// Gets or sets the name of the user.
        /// </summary>
        /// <value>The name of the user.</value>
        public string UserName { get; set; }
        /// <summary>
        /// Gets or sets the password.
        /// </summary>
        /// <value>The password.</value>
        public string Password { get; set; }
        /// <summary>
        /// Gets or sets the attemps.
        /// </summary>
        /// <value>The attemps.</value>
        public byte Attemps { get; set; }
        /// <summary>
        /// Gets or sets the ip.
        /// </summary>
        /// <value>The ip.</value>
        public string Ip { get; set; }
        /// <summary>
        /// Gets or sets the user agent.
        /// </summary>
        /// <value>The user agent.</value>
        public string UserAgent { get; set; }
    }

    /// <summary>
    /// Change password model
    /// </summary>
    public class NewPasswordModel
    {
        /// <summary>
        /// Get or set the new password value
        /// </summary>
        public string Password { get; set; }
        /// <summary>
        /// Get or set the Ip address 
        /// </summary>
        public string Ip { get; set; }
        /// <summary>
        /// Get or set the User Agent Device
        /// </summary>
        public string UserAgent { get; set; }
    }
}

