namespace SigobMobile.Common.Models
{
    public enum MenuItemType
    {
        Home = 0,
        Security = 1,
        TermsAndConditions = 2,
        Help = 3,
        ContactUs = 4,
        Logout = 5
    }

    public class MasterPageItem
    {
        //TODO: Add Icon attribute

        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        public MenuItemType Id { get; set; }

        /// <summary>
        /// Gets or sets the title of Menu Item in Master Page (menu)
        /// </summary>
        /// <value>The title.</value>
        public string Title { get; set; }
    }
}
