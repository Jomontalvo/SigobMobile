namespace SigobMobile.ViewModels
{
    using SigobMobile.Models;

    public class ExternalDocumentViewModel : BaseViewModel
    {
        #region Attributes
        public Document Document { get; set; }
        #endregion

        #region Constructor
        public ExternalDocumentViewModel(Document document)
        {
            this.Document = document;
        }
        #endregion
    }
}
