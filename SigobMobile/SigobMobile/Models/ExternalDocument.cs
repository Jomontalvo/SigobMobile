namespace SigobMobile.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using SigobMobile.Common.Models;
    using SigobMobile.Helpers;
    using Xamarin.Forms;

    public class ExternalDocument : Common.Models.ExternalDocument
    {
        #region Enumarations
        private enum PrivacyLevelItem
        {
            Ordinary = 0,            Reserved = 1,            Public = 2,
            Publics = 31,
            AllReserved = 35,
            Private = 39
        }
        #endregion

        #region Properties
        public string SenderInformation => GetSendersInfo(this.Senders);

        public string ReceiverInformation => $"{this.ReceiverOfficer} ({this.ReceiverArea})";

        public ImageSource PrivacyLevelIcon => (PrivacyLevelItem)this.PrivacyLevel.Id switch
        {
            PrivacyLevelItem.Ordinary => "ic_doc_privacy_ordinary",
            PrivacyLevelItem.Reserved => "ic_doc_privacy_reserved",
            PrivacyLevelItem.Public => "ic_doc_privacy_public",
            PrivacyLevelItem.Publics => "ic_doc_privacy_public",
            PrivacyLevelItem.AllReserved => "ic_doc_privacy_reserved",
            PrivacyLevelItem.Private => "ic_doc_privacy_ordinary",
            _ => ""
        };

        public ImageSource ReplyIcon => this.Reply switch
        {
            ResponseStatus.Pending => "ic_doc_reply_pending",
            ResponseStatus.Received => "ic_notify_reply",
            _ => string.Empty
        };

        public bool ReplyRequired => this.Reply != ResponseStatus.NotNecesary;

        public string ScannedDocumentInfo => ScannedCount switch
        {
            0 => Languages.NoScannedDocuments,
            1 => $"{this.ScannedCount} {Languages.ScannedDocument}",
            _ => $"{this.ScannedCount} {Languages.ScannedDocuments}"
        };
        public string AttachmentsInfo => (AttachmentCount > 0)
            ? $"{AttachmentCount} {Languages.Attachments}"
            : $"{Languages.NoAttachments}";

        public string TrackingInfo => string.Format(Languages.Tracking, TrackingSteps, TimeInOffice, TimeScaleDescription);

        public string LinkedDocumentsInfo => this.LinkedDocumentsCount > 0
            ? $"{this.LinkedDocumentsCount} {Languages.LinkedDocuments}"
            :$"{Languages.NoLinkedDocuments}";

        public string InKnowledge => this.AwareOfficialsCount switch
        {
            0 => Languages.NoOneInformed,
            1 => Languages.InKnowledgeOne,
            _ => string.Format(Languages.InKnowledgeSeveral, this.AwareOfficialsCount)
        };

        public string ResultInfo => GetResults();

        public string TagsInfo => string.IsNullOrEmpty(this.Tags.Trim())
            ? $"No tags registered"
            : $"{this.Tags.Trim()}";
        #endregion

        /// <summary>
        /// Built string with sender information
        /// </summary>
        /// <returns></returns>
        #region Methods
        private string GetSendersInfo(List<Common.Models.Sender> senders)
        {
            string senderInfo = string.Empty;
            if (senders.Any())
            {
                var firstSender = senders.First();
                string institutionName = firstSender.Institution;

                if (firstSender.Institution.Substring(firstSender.Institution.Length - 3) == " - ")
                    institutionName = institutionName.Replace(" - ", string.Empty);

                senderInfo = string.IsNullOrEmpty(institutionName) ?
                    $"{firstSender.FullName} ({Languages.Ok})" :
                    $"{firstSender.FullName} ({institutionName})";
            }
            return senderInfo;
        }

        private string GetResults()
        {
            string replyInfo = this.Reply switch
            {
                ResponseStatus.NotNecesary => Languages.NotReplyRequired,
                ResponseStatus.Pending => Languages.NotReplied,
                ResponseStatus.Received => Languages.Replied,
                _ => Languages.NotReplyRequired
            };
            string result = ManagementResult != null
                ? $"{ManagementResult.Description} - ({replyInfo})"
                : $"{Languages.InManagementStatus} - ({replyInfo})";
            return result;
        }
        #endregion
    }
}