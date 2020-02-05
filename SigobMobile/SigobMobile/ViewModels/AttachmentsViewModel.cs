namespace SigobMobile.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Windows.Input;
    using GalaSoft.MvvmLight.Command;
    using SigobMobile.Common.Helpers;
    using SigobMobile.Common.Models;
    using SigobMobile.Common.Services;
    using SigobMobile.Helpers;
    using SigobMobile.Interfaces;

    public class AttachmentsViewModel : BaseViewModel
    {
        #region Services
        internal ApiService apiService;
        #endregion

        #region Attributes
        private readonly int parentId;
        private readonly SigobInstrument component;
        private readonly DocumentSource source;
        private ObservableCollection<AttachmentsItemViewModel> attachments;
        private ObservableCollection<Grouping<string, AttachmentsItemViewModel>> attachmentsGrouped;
        private bool isRefreshing;
        private bool isRunning;
        private List<Attachment> attachmentList;
        #endregion

        #region Properties
        public string ApiController => $"documents/parent/{this.parentId}" +
            $"/component/{(int)this.component}" +
            $"/source/{(int)this.source}";
        public bool IsRefreshing
        {
            get => this.isRefreshing;
            set => SetValue(ref this.isRefreshing, value);
        }
        public ObservableCollection<AttachmentsItemViewModel> Attachments
        {
            get => this.attachments;
            set => SetValue(ref this.attachments, value);
        }
        public ObservableCollection<Grouping<string, AttachmentsItemViewModel>> AttachmentsGrouped
        {
            get => this.attachmentsGrouped;
            set => SetValue(ref this.attachmentsGrouped, value);
        }
        public bool IsRunning
        {
            get => this.isRunning;
            set => SetValue(ref this.isRunning, value);
        }
        #endregion

        #region Constructors
        /// <summary>
        /// Create a new List of Attachments 
        /// </summary>
        /// <param name="parentId"></param>
        /// <param name="component"></param>
        /// <param name="source"></param>
        public AttachmentsViewModel(int parentId, SigobInstrument component, DocumentSource source)
        {
            this.IsRunning = true;
            this.parentId = parentId;
            this.component = component;
            this.source = source;
            this.apiService = new ApiService();
            this.LoadAttachments();
        }

        /// <summary>
        /// Display Attachment List from Sender
        /// </summary>
        /// <param name="attachments"></param>
        /// <param name="source"></param>
        public AttachmentsViewModel(List<Attachment> attachments, DocumentSource source)
        {
            this.IsRunning = true;
            this.apiService = new ApiService();
            this.source = source;
            this.attachmentList = attachments;
            this.GetAttachmentCollection();
            this.IsRunning = false;
        }
        #endregion

        #region Commands
        public ICommand RefreshCommand => new RelayCommand(this.LoadAttachments);
        #endregion

        #region Methods
        /// <summary>
        /// Load Attachment List
        /// </summary>
        private void LoadAttachments()
        {
            this.IsRefreshing = true;
            if (this.source != DocumentSource.Correspondence)
            {
                IErrorHandler errorHandler = null;
                this.LoadFiles().FireAndForgetSafeAsync(errorHandler);
            }
            this.IsRefreshing = false;
            this.IsRunning = false;
        }

        /// <summary>
        /// Get Files from ApiService
        /// </summary>
        /// <returns></returns>
        private async Task LoadFiles()
        {
            var connection = await this.apiService.CheckConnection();
            if (!connection.IsSuccess)
            {
                this.IsRefreshing = false;
                await App.Navigator.CurrentPage.DisplayAlert(
                    Languages.Error,
                    connection.Message,
                    Languages.Cancel);
                await App.Navigator.PopAsync();
                return;
            }

            var response = await this.apiService.GetList<Attachment>(
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
                    Languages.Error,
                    response.Message,
                    Languages.Cancel);
                await App.Navigator.PopAsync();
                return;
            }
            this.attachmentList = (List<Attachment>)response.Result;
            this.GetAttachmentCollection();
        }

        /// <summary>
        /// Build attachment observable collection
        /// </summary>
        private void GetAttachmentCollection()
        {
            this.Attachments = new ObservableCollection<AttachmentsItemViewModel>(
                this.ToAttachmentsItemViewModel());

            //Order by group Country
            var sorted = from attach in Attachments
                         orderby attach.FolderId
                         group attach by $"{string.Empty}|{attach.FolderName}|doc_menu_group_folder"
                         into attachmentGroup
                         select new Grouping<string, AttachmentsItemViewModel>(attachmentGroup.Key, attachmentGroup);

            this.AttachmentsGrouped = new ObservableCollection<Grouping<string, AttachmentsItemViewModel>>(sorted);
        }

        /// <summary>
        /// Convert to AttachmentItemViewModel
        /// </summary>
        /// <returns></returns>
        private IEnumerable<AttachmentsItemViewModel> ToAttachmentsItemViewModel()
        {
            return this.attachmentList.Select(a => new AttachmentsItemViewModel
            {
                AttachmentName = a.AttachmentName,
                Attribute = a.Attribute,
                Date = a.Date,
                FileType = a.FileType,
                FolderId = a.FolderId,
                FolderName = string.IsNullOrEmpty(a.FolderName) ? Languages.DefaultFolder : a.FolderName,
                Id = a.Id,
                IsFolderOwner = a.IsFolderOwner,
                LastModified = a.LastModified,
                Lenght = a.Lenght,
                OwnerCode = a.OwnerCode,
                OwnerFullName = a.OwnerFullName,
                Password = a.Password,
                Size = a.Size,
                Source = this.source
            });
        }
        #endregion

    }
}
