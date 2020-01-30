using System;
using SigobMobile.Common.Services;

namespace SigobMobile.ViewModels
{
    public class EditTaskViewModel : BaseViewModel
    {
        #region Services
        private ApiService apiService;
        #endregion

        #region Constructors
        public EditTaskViewModel()
        {
            this.apiService = new ApiService();
        }
        #endregion
    }
}
