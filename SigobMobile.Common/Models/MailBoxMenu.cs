namespace SigobMobile.Common.Models
{
    /// <summary>
    /// Type Menu
    /// </summary>
    public enum TypeMenuBox
    {
        AppType = 0,
        TagType = 1
    }

    /// <summary>
    ///Trays Names (Management filter)
    /// </summary>
    public enum DocumentTrayId
    {
        All = 0,
        External = 1,
        Internal = 2,
        Draft = 3,
        ProcessedToConfirm = 4,
        Pending = 5,
        ForYourInformation = 6,
        Trasferred = 7,
        WithDeadline = 8,
        PendingEmail = 9,
        PendingLinkedInstitution = 10
    }

    /// <summary>
    /// Model for main view Correspondence Application
    /// </summary>
    public class MailBoxMenu
    {
        public int Id { get; set; }
        public string  Key { get; set; }
        public string Icon => (!string.IsNullOrEmpty(Key)) ? $"ic_doc_menu_100" : $"ic_doc_menu_{Id}";
        public string Name { get; set; }
        public TypeMenuBox Type => (!string.IsNullOrEmpty(Key)) ? TypeMenuBox.TagType : TypeMenuBox.AppType;
        public int ItemCount { get; set; }
    }
}
