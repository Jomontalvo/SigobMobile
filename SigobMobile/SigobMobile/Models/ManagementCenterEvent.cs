namespace SigobMobile.Models
{
    using Common.Models;
    using Helpers;
    using Xamarin.Forms;

    /// <summary>
    /// Readonly Properties
    /// </summary>
    public class ManagementCenterEvent : Common.Models.ManagementCenterEvent
    {
        public string PrivacyDescription
        {
            get
            {
                var detailPrivacy = PrivacyLevel switch
                {
                    SecurityLevelEvent.Public => Languages.PublicConfidentialityLevel,
                    SecurityLevelEvent.Low => Languages.LowConfidentialityLevel,
                    SecurityLevelEvent.Medium => Languages.MediumConfidentialityLevel,
                    SecurityLevelEvent.High => Languages.HighConfidentialityLevel,
                    SecurityLevelEvent.Private => Languages.PrivateConfidentialityLevel,
                    _ => Languages.NoConfidentialityLevel,
                };
                return detailPrivacy;
            }
        }
        public string StatusDescription
        {
            get
            {
                var status_ = Status switch
                {
                    StatusAppointment.InManagement => Languages.InManagementStatus,
                    StatusAppointment.Suspended => Languages.SuspendedStatus,
                    StatusAppointment.Finished => Languages.CompletedStatus,
                    _ => Languages.InManagementStatus,
                };
                return status_;
            }
        }
        public Color CalendarColor { get; set; }
        public Color TypeColor { get; set; }
        public bool IsInManagement { get => Status == StatusAppointment.InManagement ; }
    }
}
