namespace SigobMobile.ViewModels
{
    using System;
    using SigobMobile.Helpers;
    using SigobMobile.Models;

    public class ExternalDocumentViewModel : BaseViewModel
    {
        #region Attributes
        private string backPageTitle;
        #endregion

        #region Properties
        public Document Document { get; set; }

        public string BackPageTitle
        {
            get => this.backPageTitle;
            set => SetValue(ref this.backPageTitle, value);
        }
        #endregion

        #region Constructor
        public ExternalDocumentViewModel(Document document)
        {
            this.Document = document;
            this.GetDefaultValues();
        }
        #endregion

        #region Methods
        private void GetDefaultValues()
        {
            var parent = App.Navigator.CurrentPage.GetType().Name;
            this.BackPageTitle = parent == "DocumentsTrayPage" ? Languages.TagTray : Languages.Ok;
        }
        #endregion
    }
}
