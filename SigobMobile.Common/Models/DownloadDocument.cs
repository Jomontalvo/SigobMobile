namespace SigobMobile.Common.Models
{
    using System;
    using Newtonsoft.Json;

    public class DownloadDocument
    {
        [JsonProperty("nombre")]
        public string Name { get; set; }

        [JsonProperty("fecha_creacion")]
        public DateTime CreateDate { get; set; }

        [JsonProperty("extension")]
        public string Extension { get; set; }

        [JsonProperty("bytes")]
        public long Bytes { get; set; }

        [JsonProperty("mime")]
        public string Mime { get; set; }

        [JsonProperty("urlDocumento")]
        public string UrlDocument { get; set; }

        [JsonProperty("documento")]
        public string Document { get; set; }
    }
}
