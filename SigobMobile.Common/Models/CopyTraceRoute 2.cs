namespace SigobMobile.Common.Models
{
    using System;
    using Newtonsoft.Json;

    public class CopyTraceRoute
    {
        [JsonProperty("codigoRecorrido")]
        public int TraceId { get; set; }

        [JsonProperty("nroCopia")]
        public int Copy { get; set; }

        [JsonProperty("idCorrespondencia")]
        public int Id { get; set; }

        [JsonProperty("estadoCopia")]
        public TrackingStatus CopyStatus { get; set; }

        [JsonProperty("funcionarioInicioGestion")]
        public string StartOfficialName { get; set; }

        [JsonProperty("cargoInicioGestion")]
        public string StartPosition { get; set; }

        [JsonProperty("areaInicioGestion")]
        public string StartArea { get; set; }

        [JsonProperty("funcionarioUltimaGestion")]
        public string LastOfficialName { get; set; }

        [JsonProperty("cargoUltimaGestion")]
        public string LastPosition { get; set; }

        [JsonProperty("areaUltimaGestion")]
        public string LastArea { get; set; }

        [JsonProperty("fechaInicioGestion")]
        public DateTime StartDate { get; set; }

        [JsonProperty("fechaEntradaUltimaGestion")]
        public DateTime LastManagementDate { get; set; }

        [JsonProperty("duracion")]
        public decimal Duration { get; set; }

        [JsonProperty("unidadMedida")]
        public string MeasureUnit { get; set; }

        [JsonProperty("duracionGestionEsperada")]
        public int ExpectedDuration { get; set; }

        public bool IsCompleted { get => Duration != 0; }
    }
}