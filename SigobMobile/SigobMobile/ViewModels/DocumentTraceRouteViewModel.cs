namespace SigobMobile.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Threading.Tasks;
    using AsyncAwaitBestPractices.MVVM;
    using Common.Helpers;
    using Common.Models;
    using Common.Services;
    using Helpers;
    using Interfaces;
    using SigobMobile.Models;

    public class DocumentTraceRouteViewModel : BaseViewModel
    {
        #region Attributes
        private readonly ApiService apiService;
        private readonly int id;
        private bool isRefreshing;
        private List<VisibleDocumentTraceRoute> originalTraceRoute;
        private ObservableCollection<TraceRouteGroup> traceRouteGroup;
        
        #endregion

        #region Properties
        public string ApiController { get => $"correspondences/{this.id}/trace"; }

        public int CurrentCopy { get; }

        public string CurrentCopyLabel { get => $"({Languages.Copy} {CurrentCopy})"; }


        public string RegistrationCode { get; }

        public ObservableCollection<TraceRouteGroup> TraceRoutes
        {
            get => this.traceRouteGroup;
            set => SetValue(ref this.traceRouteGroup, value);
        }
        public bool IsRefreshing
        {
            get => this.isRefreshing;
            set => SetValue(ref this.isRefreshing, value);
        }
        #endregion

        #region Constructors
        public DocumentTraceRouteViewModel(int id, int copy, string registrationCode)
        {
            this.id = id;
            CurrentCopy = copy;
            RegistrationCode = registrationCode;
            this.apiService = new ApiService();
            IErrorHandler errorHandler = null;
            this.LoadStepsAsync().FireAndForgetSafeAsync(errorHandler);
        }
        #endregion

        #region Commands
        public IAsyncCommand ShareTraceCommand => new AsyncCommand(ShareTraceAsync);
        #endregion

        #region Methods
        private async Task LoadStepsAsync()
        {
            this.IsRefreshing = true;
            try
            {
                // 1. Verify connection
                var connection = await this.apiService.CheckConnection();
                if (!connection.IsSuccess)
                {
                    this.IsRefreshing = false;
                    await App.Navigator.CurrentPage.DisplayAlert(
                        Languages.Error,
                        connection.Message,
                        Languages.Cancel);
                    return;
                }

                // 2. Get the menu list from API
                var response = await this.apiService.GetList<VisibleDocumentTraceRoute>(
                        Settings.UrlBaseApiSigob,
                        App.PrefixApiSigob,
                        this.ApiController,
                        Settings.Token,
                        Settings.DbToken
                    );
                if (!response.IsSuccess)
                {
                    this.IsRefreshing = false;
                    await App.Navigator.CurrentPage.DisplayAlert(
                        title: Languages.Error,
                        message: response.Message,
                        cancel: Languages.Cancel);
                    return;
                }

                //Get List values
                this.originalTraceRoute = (List<VisibleDocumentTraceRoute>)response.Result;
                this.TraceRoutes = new ObservableCollection<TraceRouteGroup>(ToTracerouteGroup());
                this.IsRefreshing = false;
            }
            catch (Exception ex)
            {
                await App.Navigator.CurrentPage.DisplayAlert(
                    Languages.Error,
                    $"{Languages.GeneralError}: {ex.Message}",
                    Languages.Cancel);
            }
        }

        /// <summary>
        /// Convert original model in grouped model
        /// </summary>
        /// <returns></returns>
        private IEnumerable<TraceRouteGroup> ToTracerouteGroup()
        {
            return this.originalTraceRoute.Select(r => new TraceRouteGroup(r.Copy, this.ToTraceRoutes(r.TraceRoutes).ToList()));
        }

        /// <summary>
        /// Convert TraceRoute Common Model to SigobMobile Model
        /// </summary>
        /// <param name="traceRoutes"></param>
        /// <returns></returns>
        private IEnumerable<Models.DocumentTraceRoute> ToTraceRoutes(List<Common.Models.DocumentTraceRoute> traceRoutes)
        {
            return traceRoutes.Select(r => new Models.DocumentTraceRoute
            {
                DurationInOffice = r.DurationInOffice,
                Annotation = r.Annotation,
                Area = r.Area,
                Copy = r.Copy,
                DurationMeasure = r.DurationMeasure,
                HasAttachments = r.HasAttachments,
                Id = r.Id,
                IsResolution = r.IsResolution,
                ManagementEndDate = r.ManagementEndDate,
                OfficeId = r.OfficeId,
                OfficialName = r.OfficialName,
                Position = r.Position,
                StartDate = r.StartDate.ToLocalTime(),
                StepPosition = r.StepPosition,
                TransferComment = r.TransferComment
            });
        }

        /// <summary>
        /// Share screen capture
        /// </summary>
        /// <returns></returns>
        private async Task ShareTraceAsync()
        {
            await App.Navigator.CurrentPage.DisplayAlert(
                    Languages.Error,
                    $"{Languages.GeneralError}",
                    Languages.Cancel);
        }
        #endregion
    }
}