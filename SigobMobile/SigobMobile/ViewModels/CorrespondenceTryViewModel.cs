using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using SigobMobile.Services;
using static SigobMobile.Models.Correspondence;

namespace SigobMobile.ViewModels
{
    public class CorrespondenceTryViewModel : BaseViewModel
    {
        #region Services
        internal ApiService apiService;
        #endregion

        #region Attributes
        private ObservableCollection<InstitutionItemViewModel> institutions;
        private bool isRefreshing;
        private List<CorrespondenceTry> institutionList;
        #endregion

        public CorrespondenceTryViewModel()
        {
        }
    }
}
