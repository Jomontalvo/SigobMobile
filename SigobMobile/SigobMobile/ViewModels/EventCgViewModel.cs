﻿namespace SigobMobile.ViewModels
{
    using Models;
    using Services;
    using Helpers;
    using Xamarin.Forms;
    using System;
    using System.Windows.Input;
    using GalaSoft.MvvmLight.Command;
    using Views.ManagementCenter;

    public class EventCgViewModel : BaseViewModel
    {
        #region Services
        internal ApiService apiService;
        #endregion

        #region ApiControllers
        internal string apiEventController = "cgcal/{0}/events/{1}";
        //internal string apiCalendarsController = "cgcal/{0}/events/{1}";
        //internal string apiEventTypeController = "cgcal/{0}/events/{1}";
        #endregion

        #region Attributes
        private bool isRunning;
        private bool isEditable;
        private ManagementCenterEvent eventDetails;
        private string interval;
        private string iconStatus;
        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="T:SigobMobile.ViewModels.EventCgViewModel"/> is editable.
        /// </summary>
        /// <value><c>true</c> if is editable; otherwise, <c>false</c>.</value>
        public bool IsEditable
        {
            get => this.isEditable;
            set => SetValue(ref this.isEditable, value);
        }

        /// <summary>
        /// For ActivityIndicator => Gets or sets a value indicating whether this
        /// <see cref="T:SigobMobile.ViewModels.EventManagementCenterViewModel"/> is running.
        /// </summary>
        /// <value><c>true</c> if is running; otherwise, <c>false</c>.</value>
        public bool IsRunning
        {
            get => this.isRunning;
            set => SetValue(ref this.isRunning, value);
        }

        /// <summary>
        /// Gets or sets the time interval when event is schedule.
        /// </summary>
        /// <value>The interval.</value>
        public string Interval
        {
            get => this.interval;
            set => SetValue(ref this.interval, value);
        }

        /// <summary>
        /// Property inherited from Previous Page Navigation
        /// </summary>
        /// <value>The local Appointment.</value>
        public Event LocalAppointment { get; set; }

        /// <summary>
        /// Gets or sets the event details.
        /// </summary>
        /// <value>The event details.</value>
        public ManagementCenterEvent EventCg { get; set; }

        /// <summary>
        /// Gets or sets the icon status.
        /// </summary>
        /// <value>The icon status.</value>
        public string IconStatus
        {
            get => this.iconStatus;
            set => SetValue(ref this.iconStatus, value);
        }
        #endregion

        #region Constructors
        public EventCgViewModel(Event appoitment)
        {
            this.IsEditable = true;
            this.LocalAppointment = appoitment;
            this.apiService = new ApiService();
            this.LoadEventDetails();
        }
        #endregion

        #region Commands
        public ICommand EditButtonCommand => new RelayCommand(EditButton);
        #endregion

        #region Methods

        /// <summary>
        /// Call Edit Event view.
        /// </summary>
        private async void EditButton()
        {
            var editEventViewModel = MainViewModel.GetInstance();
            editEventViewModel.EditEvent = new EditEventViewModel(this.EventCg);
            await Application.Current.MainPage.Navigation.PushModalAsync(new EditEventPage());
        }

        /// <summary>
        /// Load the event details.
        /// </summary>
        private async void LoadEventDetails()
        {
            IsRunning = true;
            var connection = await apiService.CheckConnection();
            if (!connection.IsSuccess)
            {
                IsRunning = false;
                await Application.Current.MainPage.DisplayAlert(
                    Languages.Error,
                    connection.Message,
                    Languages.Cancel);
                await Application.Current.MainPage.Navigation.PopToRootAsync();
                return;
            }
            var response = await this.apiService.Get<ManagementCenterEvent>(
                Settings.UrlBaseApiSigob,
                App.PrefixApiSigob,
                string.Format(this.apiEventController, LocalAppointment.Owner, LocalAppointment.Id),
                Settings.Token,
                Settings.DbToken
            );
            if (!response.IsSuccess)
            {
                this.IsRunning = false;
                await Application.Current.MainPage.DisplayAlert(
                    Languages.Error,
                    response.Message,
                    Languages.Cancel);
                return;
            }
            this.eventDetails = (ManagementCenterEvent)response.Result;
            this.EventCg = eventDetails;
            this.SetEventValues();
            this.IsRunning = false;
        }

        /// <summary>
        /// Set the event values.
        /// </summary>
        private void SetEventValues()
        {
            //Event interval
            this.Interval = DateTime.Now.ToString("t");

            //Event Management Status
            switch (EventCg.Status)
            {
                case StatusAppointment.InManagement:
                    IconStatus = "ic_ev_status_active";
                    break;
                case StatusAppointment.Finished:
                    IconStatus = "ic_ev_status_finished";
                    break;
                case StatusAppointment.Suspended:
                    IconStatus = "ic_ev_status_suspended";
                    break;
                default:
                    IconStatus = "ic_ev_status";
                    break;
            }

            //Attributes
            IsEditable = EventCg.AttributeOnEvents == EventAttribute.Create || 
                         (EventCg.AttributeOnEvents == EventAttribute.CreateOwner && EventCg.ProgrammerOfficeId == Settings.OfficeCode);
        }
        #endregion

    }
}
