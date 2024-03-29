﻿namespace SigobMobile.ViewModels
{
    using System.Threading.Tasks;
    using AsyncAwaitBestPractices.MVVM;
    using Models;
    using Xamarin.Forms;

    public class EditEventViewModel : BaseViewModel
    {
        #region Attributes
        private bool isNewEvent;
        #endregion


        #region Properties
        public bool IsNewEvent
        {
            get => this.isNewEvent;
            set => SetValue(ref this.isNewEvent, value);
        }
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="T:SigobMobile.ViewModels.EditEventViewModel"/> class
        /// when the parent view select Edit Option.
        /// </summary>
        /// <param name="localEvent">Event object parameter.</param>
        public EditEventViewModel(ManagementCenterEvent localEvent)
        {
            IsNewEvent = false;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:SigobMobile.ViewModels.EditEventViewModel"/> class
        /// when the parent view select New Option.
        /// </summary>
        public EditEventViewModel()
        {
            IsNewEvent = true;
        }
        #endregion

        #region Commands
        public IAsyncCommand CancelEditCommand => new AsyncCommand(CancelEdit);
        #endregion

        #region Methods
        private async Task CancelEdit()
        {
            await Application.Current.MainPage.Navigation.PopModalAsync();
        }
        #endregion
    }
}
