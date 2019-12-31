namespace SigobMobile.Models
{
    using System.Collections.Generic;
    using SigobMobile.ViewModels;


    /// <summary>
    /// Group Menu Mail Box Model
    /// </summary>
    public class MailBoxMenuGroup : List<MailBoxMenuItemViewModel>
    {
        public string Name { get; private set; }

        public MailBoxMenuGroup(string name, List<MailBoxMenuItemViewModel> menuItems) : base(menuItems)
        {
            Name = name;
        }
    }
}
