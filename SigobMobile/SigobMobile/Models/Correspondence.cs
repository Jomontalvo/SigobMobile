using System;
using Newtonsoft.Json;

namespace SigobMobile.Models
{
    public class Correspondence
    {
        public class CorrespondenceTry
        {
            [JsonProperty("totalFilas")]
            public long TotalRows { get; set; }

            [JsonProperty("itemsCorrespondencia")]
            public CorrespondenceList[] CorrespondenceItems { get; set; }
        }

        public class CorrespondenceList
        {
            [JsonProperty("index")]
            public long Index { get; set; }

            [JsonProperty("codigoCorrespondencia")]
            public long Id { get; set; }

            [JsonProperty("codigoRecorrido")]
            public long TrackingId { get; set; }

            [JsonProperty("codigoTramite")]
            public string CorrespondenceCode { get; set; }

            [JsonProperty("copia")]
            public long Copy { get; set; }

            [JsonProperty("procedencia")]
            public long Source { get; set; }

            [JsonProperty("estadoPlazo")]
            public long EstadoPlazo { get; set; }

            [JsonProperty("prioridad")]
            public long Prioridad { get; set; }

            [JsonProperty("estadoRecorrido")]
            public long EstadoRecorrido { get; set; }

            [JsonProperty("revisado")]
            public bool Revisado { get; set; }

            [JsonProperty("fechaCreacion")]
            public DateTimeOffset FechaCreacion { get; set; }

            [JsonProperty("descripcion")]
            public string Descripcion { get; set; }

            [JsonProperty("transfiere")]
            public string Transfiere { get; set; }

            [JsonProperty("emisorDestinatario")]
            public string EmisorDestinatario { get; set; }

            [JsonProperty("institucion")]
            public string Institucion { get; set; }

            [JsonProperty("respondido")]
            public string Respondido { get; set; }

            [JsonProperty("aviso")]
            public bool Aviso { get; set; }

            [JsonProperty("anexos")]
            public long Anexos { get; set; }

            [JsonProperty("comentario")]
            public object Comentario { get; set; }

            [JsonProperty("resumen")]
            public string Resumen { get; set; }

            [JsonProperty("duracionDespacho")]
            public long DuracionDespacho { get; set; }

            [JsonProperty("duracionArea")]
            public long DuracionArea { get; set; }
        }
    }
}
