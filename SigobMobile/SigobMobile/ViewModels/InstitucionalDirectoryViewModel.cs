namespace SigobMobile.ViewModels
{
    using System;
    using System.Threading.Tasks;
    using Helpers;
    using Interfaces;
    using Services;

    public class InstitucionalDirectoryViewModel : BaseViewModel
    {
        #region Services
        internal ApiService apiService;   
        #endregion

        #region Attributes
        private int selectedIndex;
        #endregion

        #region Properties
        public int SelectedIndex
        {
            get { return this.selectedIndex; }
            set
            {
                SetValue(ref this.selectedIndex, value);
                IErrorHandler errorHandler = null;
                this.OnSelectionChanged().FireAndForgetSafeAsync(errorHandler);
            }
        }
        #endregion

        #region Constructors
        public InstitucionalDirectoryViewModel()
        {
            this.apiService = new ApiService();
            this.LoadInstitutionalDirectoryContacts();
        }
        #endregion

        #region Methods

        private void LoadInstitutionalDirectoryContacts()
        {

        }

        private void LoadMobileContacts()
        {

        }

        private async Task OnSelectionChanged()
        {
            if (this.SelectedIndex == 0) //Institutional directory
                await Task.Delay(100);
            else if (this.SelectedIndex == 1) //Mobile contacts
                await Task.Delay(100);

        }
        #endregion

    }
}
