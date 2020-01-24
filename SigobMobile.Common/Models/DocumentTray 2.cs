namespace SigobMobile.Common.Models
{
    using System;
    using Newtonsoft.Json;

    public class DocumentTray
    {
        [JsonProperty("idBandeja")]
        public int Id { get; set; }

        [JsonProperty("nombreBandeja")]
        public string Name { get; set; }

        [JsonProperty("cantidadItems")]
        public int ItemCount { get; set; }
    }
}
