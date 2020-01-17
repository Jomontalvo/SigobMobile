namespace SigobMobile.Common.Models
{
    using System;
    using System.Collections.Generic;
    using Newtonsoft.Json;

    public enum ResponseStatus : byte
    {
        NotNecesary = 0,
        Received = 1,
        Pending = 2
    }

    public enum TimeScaleType : byte
    {
        Hours = 0,
        Days = 1,
        Weeks = 2
    }

    public class ExternalDocument
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("codigoRecorrido")]
        public int TrackingId { get; set; }

        [JsonProperty("codigoRegistro")]
        public string RegistrationCode { get; set; }

        [JsonProperty("codigoOrigen")]
        public string OriginalCode { get; set; }

        [JsonProperty("multiplesEmisores")]
        public bool HasManySenders { get; set; }

        [JsonProperty("lista_emisores")]
        public List<Sender> Senders { get; set; }

        [JsonProperty("copiaActiva")]
        public int CurrentCopy { get; set; }

        [JsonProperty("asunto")]
        public string Subject { get; set; }

        [JsonProperty("destinatario")]
        public string ReceiverOfficer { get; set; }

        [JsonProperty("areaDestinatario")]
        public string ReceiverArea { get; set; }

        [JsonProperty("objetivos")]
        public string Objectives { get; set; }

        [JsonProperty("gradoReserva")]
        public TSigTuple PrivacyLevel { get; set; }

        [JsonProperty("tipo")]
        public TSigTuple Type { get; set; }

        [JsonProperty("prioridad")]
        public TSigTuple Priority { get; set; }

        [JsonProperty("resultadoGestion")]
        public TSigTuple ManagementResult { get; set; }

        [JsonProperty("respondida")]
        public ResponseStatus Reply { get; set; }

        [JsonProperty("textoRespuesta")]
        public string ResponseStatusDescription { get; set; }

        [JsonProperty("fechaRegistro")]
        public DateTime RegistrationDate { get; set; }

        [JsonProperty("resumen")]
        public string Text { get; set; }

        [JsonProperty("tags")]
        public string Tags { get; set; }

        [JsonProperty("numeroDigitalizados")]
        public int ScannedCount { get; set; }

        [JsonProperty("documentosAnexos")]
        public int AttachmentCount { get; set; }

        [JsonProperty("listaDocumentosAnexos")]
        public List<Attachment> Attachments { get; set; }

        [JsonProperty("numeroCopias")]
        public int Copies { get; set; }

        [JsonProperty("transferidoPor")]
        public string TransferedBy { get; set; }

        [JsonProperty("fechaTransferencia")]
        public DateTime TransferDate { get; set; }

        [JsonProperty("comentarioTransferencia")]
        public string TransferComment { get; set; }

        [JsonProperty("duracion")]
        public int TimeInOffice { get; set; }

        [JsonProperty("tipoDuracion")]
        public TimeScaleType TimeScale { get; set; }

        [JsonProperty("unidadDuracion")]
        public string TimeScaleDescription { get; set; }

        [JsonProperty("pasosRecorrido")]
        public int TrackingSteps { get; set; }

        [JsonProperty("numeroDespachosEnConocimiento")]
        public int AwareOfficialsCount { get; set; }

        [JsonProperty("numeroCorrespondenciasRelacionadas")]
        public int LinkedDocumentsCount { get; set; }

        [JsonProperty("relacionadas")]
        public List<LinkedDocument> LinkedDocuments { get; set; }

    }
}
