namespace SigobMobile.ViewModels
{
    public class UrlViewerViewModel : BaseViewModel
    {
        #region Attributes
        string urlSourcePath;
        #endregion

        #region Properties
        public string UrlSourcePath
        {
            get => this.urlSourcePath;
            set => SetValue(ref this.urlSourcePath, value);
        }
        #endregion

        #region Constructors
        public UrlViewerViewModel(string url)
        {
            UrlSourcePath = url;
        }
        #endregion

        #region Commands

        #endregion

        #region Methods

        #endregion

    }
}
