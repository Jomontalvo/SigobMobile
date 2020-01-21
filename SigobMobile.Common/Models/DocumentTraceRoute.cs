namespace SigobMobile.Common.Models
{
    using System;
    using Newtonsoft.Json;

    public class DocumentTraceRoute
    {
        [JsonProperty("idOrden")]
        public int Id { get; set; }

        [JsonProperty("posicionEnRecorrido")]
        public byte StepPosition { get; set; }
        
        [JsonProperty("fecha")]
        public DateTime StartDate { get; set; }

        [JsonProperty("despacho")]
        public string OfficeId { get; set; }

        [JsonProperty("copia")]
        public long Copy { get; set; }

        [JsonProperty("nombre")]
        public string OfficialName { get; set; }

        [JsonProperty("cargo")]
        public string Position { get; set; }

        [JsonProperty("area")]
        public string Area { get; set; }

        [JsonProperty("comentario")]
        public object TransferComment { get; set; }

        [JsonProperty("anotacion")]
        public string Annotation { get; set; }

        [JsonProperty("resolucion")]
        public bool IsResolution { get; set; }

        [JsonProperty("tieneDocumentos")]
        public bool HasAttachments { get; set; }

        [JsonProperty("finGestion")]
        public DateTime? ManagementEndDate { get; set; }

        [JsonProperty("duracionEnDespacho")]
        public int DurationInOffice { get; set; }

        [JsonProperty("unidadMedidaDuracion")]
        public TimeScaleType DurationMeasure { get; set; }
    }
}
