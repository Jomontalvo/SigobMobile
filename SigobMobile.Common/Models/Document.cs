namespace SigobMobile.Common.Models
{
    using System;
    using System.Drawing;
    using Newtonsoft.Json;

    public enum ManagementTermStatus
    {
        /// <summary>
        /// Overcome status
        /// </summary>
        Red = 255,
        /// <summary>
        /// In management status
        /// </summary>
        Green = 32768,
        /// <summary>
        /// Warning status
        /// </summary>
        Yellow = 65535,
        /// <summary>
        /// No status
        /// </summary>
        White = 16777215
    }

    public class Document
    {
        [JsonProperty("index")]
        public int Index { get; set; }

        [JsonProperty("codigoCorrespondencia")]
        public int Id { get; set; }

        [JsonProperty("codigoRecorrido")]
        public int TrackingId { get; set; }

        [JsonProperty("codigoTramite")]
        public string RegistrationCode { get; set; }

        [JsonProperty("copia")]
        public int Copy { get; set; }

        [JsonProperty("procedencia")]
        public byte Source { get; set; }

        [JsonProperty("estadoPlazo")]
        public ManagementTermStatus Status { get; set; }

        [JsonProperty("prioridad")]
        public int Priority { get; set; }

        [JsonProperty("estadoRecorrido")]
        public byte StatusTracking { get; set; }

        [JsonProperty("revisado")]
        public bool IsReviewed { get; set; }

        [JsonProperty("fechaCreacion")]
        public DateTime RegistrationDate { get; set; }

        [JsonProperty("descripcion")]
        public string Subject { get; set; }

        [JsonProperty("transfiere")]
        public string TransferedBy { get; set; }

        [JsonProperty("emisorDestinatario")]
        public string SenderOrReceiver { get; set; }

        [JsonProperty("institucion")]
        public string Institution { get; set; }

        [JsonProperty("respondido")]
        public char Reply { get; set; }

        [JsonProperty("aviso")]
        public bool Advertisement { get; set; }

        [JsonProperty("anexos")]
        public int AttachmentCount { get; set; }

        [JsonProperty("comentario")]
        public string TransferComment { get; set; }

        [JsonProperty("resumen")]
        public string Text { get; set; }

        [JsonProperty("duracionDespacho")]
        public int DaysStayInOffice { get; set; }

        [JsonProperty("duracionArea")]
        public int DaysStayInArea { get; set; }

        public bool HasAttachment => (this.AttachmentCount > 0);

        public bool HasResponse => (this.Reply == 'R');
    }
}
