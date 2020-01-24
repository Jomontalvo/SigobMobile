namespace SigobMobile.Common.Models
{
    using Newtonsoft.Json;

    public enum TrackingStatus
    {
        InRegistration = 0,
        Elaborating = 1,
        Completed = 2,
        AtOffice = 3,
        Pending = 4,
        TransferredReturned = 5,
        Transferred = 6,
        Anulled = 7,
        ProcessedPendingToConfirm = 8,
        Available1 = 9,
        Available2 = 10,
        Available3 = 11,
        Available4 = 12,
        ProcessedFromOffice = 13,
        ProcessedFromPending = 14,
        ProcessedTransferredReturned = 15,
        ProcessedTransferred = 16
    }

    public class LinkedDocument
    {
        [JsonProperty("tipoRelacion")]
        public string LinkedType { get; set; }

        [JsonProperty("codigoCorrespondencia")]
        public long Id { get; set; }

        [JsonProperty("descripcionCorrespondencia")]
        public string Subject { get; set; }

        [JsonProperty("fechaElaboracion")]
        public string ElaborationDate { get; set; }

        [JsonProperty("codigoRegistro")]
        public string RegistrationCode { get; set; }

        [JsonProperty("estadoRecorrido")]
        public TrackingStatus ManagementStatus { get; set; }

        [JsonProperty("origen")]
        public Source Source { get; set; }

        [JsonProperty("gradoReserva")]
        public TSigTuple PrivacyLevel { get; set; }

        [JsonProperty("esrespuesta")]
        public bool IsReply { get; set; }

        [JsonProperty("codigoRecorrido")]
        public int TrackingId { get; set; }

    }
}
